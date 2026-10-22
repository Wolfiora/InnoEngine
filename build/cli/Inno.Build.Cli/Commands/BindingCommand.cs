using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Bindings;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;
using Inno.Build.Toolchains.Bgfx.Shaders;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.IO;
using Inno.Rendering;

namespace Inno.Build.Cli;

internal static class BindingCommand
{
    internal static async Task RunAsync(
        BuildCommandContext request,
        BuildCommandLine options,
        CancellationToken cancellationToken
    ) {
        string root = request.root;
        string configuration = request.configuration;
        BuildTargetId target = request.target;
        BuildTargetId toolsTarget = request.toolsTarget;
        BuildCompositionContext context = request.composition;
        BuildDistribution distribution = request.distribution;

        string productId = options.Read("product", "editor");
        ProductNativeBuildPlan product = distribution.ResolveNativeProduct(target.value, productId).plan;
        NativeBuildContext bindings = new(root, configuration.ToLowerInvariant());
        bindings = bindings.WithBindingGenerator(distribution.ResolveBindingGenerator(target.value));
        bindings = bindings.WithToolchain(await distribution.ResolveNativeToolchain(target.value)
            .ResolveAsync(bindings, context.host, target.value, cancellationToken).ConfigureAwait(false));
        var generation = new NativeBindingGenerationRequest(product.steps.Select(static step => step.component)
            .Where(static owner => owner.bindingDefinition is not null).DistinctBy(static owner => owner.nativeProject),
            target.value, NativeBindingOutputMode.TargetArtifacts, componentOptions: product.steps
                .Where(static step => step.component.bindingDefinition is not null)
                .GroupBy(static step => step.component.nativeProject, StringComparer.Ordinal)
                .ToDictionary(static group => group.Key, static group => group.First().options, StringComparer.Ordinal));
        using NativeBindingGenerationResult completed = await distribution.ResolveBindingGenerator(target.value).GenerateAsync(bindings, generation, cancellationToken).ConfigureAwait(false);
        
    }
}
