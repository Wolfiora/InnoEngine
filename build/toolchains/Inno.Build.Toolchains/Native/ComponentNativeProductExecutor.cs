using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Executes portable component producers in their declared dependency order.
/// </summary>
public sealed class ComponentNativeProductExecutor : INativeProductExecutor
{
    /// <inheritdoc />
    public async Task<ProductNativeBuildResult> BuildAsync(
        NativeBuildContext context,
        ProductNativeBuildPlan plan,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(plan);
        cancellationToken.ThrowIfCancellationRequested();
        if (plan.steps.Any(static step => step.build is null))
            throw new NotSupportedException("This closure requires its platform's static aggregate executor.");
        NativeToolchainSelection toolchain = context.RequireToolchain();
        plan.CreateRecipes(context);
        NativeBindingGenerationResult bindings = await NativeBindingPreparation.PrepareAsync(context,
            plan.steps.Select(static step => step.component).ToArray(), cancellationToken,
            plan.steps.Where(static step => step.component.bindingDefinition is not null)
                .GroupBy(static step => step.component.nativeProject, StringComparer.Ordinal)
                .ToDictionary(static group => group.Key, static group => group.First().options, StringComparer.Ordinal)).ConfigureAwait(false);
        var completed = new Dictionary<string, NativeBuildProduct>(StringComparer.Ordinal);
        var borrowed = new ReadOnlyDictionary<string, NativeBuildProduct>(completed);
        try
        {
            context = context.WithBindings(bindings);
            if (await ProductNativeCacheLookup.TryAcquireAsync(context, plan.steps, cancellationToken).ConfigureAwait(false)
                is IReadOnlyList<NativeBuildProduct> cached)
                return new ProductNativeBuildResult(cached, bindings);
            foreach (ProductNativeBuildStep step in plan.steps)
            {
                cancellationToken.ThrowIfCancellationRequested();
                NativeBuildContext configured = step.configuration is string configuration ? context.WithConfiguration(configuration) : context;
                NativeBuildProduct product = await step.build!(configured.WithComponentOptions(step.component, step.options), borrowed, cancellationToken).ConfigureAwait(false);
                if (product.component != step.id || product.targetId != toolchain.targetId)
                {
                    product.Dispose();
                    throw new InvalidOperationException($"Native step '{step.id}' returned a foreign component or target.");
                }
                completed.Add(step.id, product);
            }
            return new ProductNativeBuildResult(plan.steps.Select(step => completed[step.id]), bindings);
        }
        catch (Exception buildFailure)
        {
            try
            {
                ProductNativeBuildResult.Release(completed.Values.ToArray(), bindings);
            }
            catch (Exception releaseFailure)
            {
                throw new AggregateException("Native preparation failed and reading ownership could not be completely released.",
                    buildFailure, releaseFailure);
            }
            throw;
        }
    }

}
