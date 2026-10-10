using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Prepares a coherent target closure through the borrowed generator, without invoking MSBuild.
/// </summary>
public static class NativeBindingPreparation
{
    /// <summary>
    /// Requests one binding batch for all generated components of a product.
    /// </summary>
    /// <param name="context">
    /// The operation owning a frozen target selection and configured generator.
    /// </param>
    /// <param name="components">
    /// The declared component closure; components without bindings are excluded.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels generation and drains active work before returning.
    /// </param>
    /// <returns>
    /// The complete generation map, or an empty map for a product with no generated bindings.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The tool selection or required binding generator is absent.
    /// </exception>
    public static async Task<IReadOnlyDictionary<string, NativeBindingGenerationDescriptor>> PrepareAsync(
        NativeBuildContext context,
        IReadOnlyList<NativeComponentDescriptor> components,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(components);
        cancellationToken.ThrowIfCancellationRequested();
        NativeComponentDescriptor[] owners = components.Where(static owner => owner.bindingDefinition is not null)
            .DistinctBy(static owner => owner.nativeProject).ToArray();
        if (owners.Length == 0)
            return new Dictionary<string, NativeBindingGenerationDescriptor>();
        var request = new NativeBindingGenerationRequest(owners, context.RequireToolchain().targetId,
            NativeBindingOutputMode.TargetArtifacts);
        return await context.RequireBindingGenerator().GenerateAsync(context, request, cancellationToken).ConfigureAwait(false);
    }
}
