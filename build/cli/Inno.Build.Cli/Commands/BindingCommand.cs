using Inno.Build.Bindings;
using System.Collections.Generic;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains.Bgfx;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Composition;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx.Shaders;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;
using Inno.Core.IO;

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
        ProductNativeBuildPlan product = distribution.ResolveNativeProduct(target.value, productId);
        NativeBuildContext bindings = new(root, configuration.ToLowerInvariant());
        bindings = bindings.WithBindingGenerator(context.bindingGenerator);
        bindings = bindings.WithToolchain(await distribution.ResolveNativeToolchain(target.value)
            .ResolveAsync(bindings, context.host, target.value, cancellationToken).ConfigureAwait(false));
        var generation = new NativeBindingGenerationRequest(product.steps.Select(static step => step.component)
            .Where(static owner => owner.bindingDefinition is not null).DistinctBy(static owner => owner.nativeProject),
            target.value, NativeBindingOutputMode.TargetArtifacts);
        using NativeBindingGenerationResult completed = await context.bindingGenerator.GenerateAsync(bindings, generation, cancellationToken).ConfigureAwait(false);
        
    }
}
