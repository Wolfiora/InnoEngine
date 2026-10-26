using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;

namespace Inno.Build.Cli;

internal static class SupportBuildCommand
{
    internal static async Task RunAsync(
        BuildCommandContext request,
        BuildCommandLine options,
        CancellationToken cancellationToken
    ) {
        string productId = options.Read("product", "editor");
        if (productId is not ("editor" or "player"))
            throw new ArgumentException("Support preparation requires product 'editor' or 'player'.");
        NativeProductContribution product = request.distribution.ResolveNativeProduct(request.target.value, productId);
        var context = new NativeBuildContext(request.root, request.configuration.ToLowerInvariant());
        context = context.WithBindingGenerator(request.distribution.ResolveBindingGenerator(request.target.value));
        NativeToolchainSelection toolchain = await request.distribution.ResolveNativeToolchain(request.target.value)
            .ResolveAsync(context, request.composition.host, request.target.value, cancellationToken).ConfigureAwait(false);
        context = context.WithToolchain(toolchain);
        StandardBuildCaches.CreateOwners(context.engineRoot, product.plan.steps.Select(static step => step.component).ToArray());
        using ProductNativeBuildResult products = await product.BuildAsync(context, cancellationToken).ConfigureAwait(false);
        Console.WriteLine($"Prepared {productId} support for {request.target}: {products.Count} artifacts.");
        Console.WriteLine($"Native processes: {context.statistics.nativeProcesses}; binding generations: {context.statistics.bindingGenerations}.");
    }
}
