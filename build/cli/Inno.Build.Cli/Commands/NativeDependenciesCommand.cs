using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;

namespace Inno.Build.Cli;
internal static class NativeDependenciesCommand
{
    internal static async Task RunAsync(
        BuildCommandContext request,
        BuildCommandLine options,
        CancellationToken cancellation
    ) {
        string operation = options.Read("operation", "produce");
        if (operation == "import-cache")
        {
            await NativeArtifactTransfer.ImportAsync(request.root, options.Require("input"), cancellation).ConfigureAwait(false);
            return;
        }

        if (operation == "export-cache")
        {
            await NativeArtifactTransfer.ExportAsync(request.root, options.Require("output"), cancellation).ConfigureAwait(false);
            return;
        }

        if (operation == "describe")
        {
            BuildPlatformContribution platform = request.distribution.platforms.SingleOrDefault(platform => platform.descriptor.id == request.target)
                ?? throw new NotSupportedException("No native dependency platform is registered for '" + request.target + "'.");
            Inno.Core.IO.AtomicFile.WriteAllBytes(options.Require("output"), System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new
            {
                target = platform.descriptor.id.value,
                products = platform.nativeProducts.Keys.Order(StringComparer.Ordinal).ToArray()
            }));
            return;
        }

        NativeProductContribution product = request.distribution.ResolveNativeProduct(request.target.value, options.Require("product"));
        if (operation == "verify-bindings")
        {
            if (options.Read("native-dependencies", string.Empty).Length != 0)
                throw new ArgumentException("Producer binding verification cannot consume a prebuilt selection.");
            var steps = product.plan.steps.Where(static step => step.component.bindingDefinition is not null)
                .GroupBy(static step => step.component.nativeProject, StringComparer.Ordinal).ToArray();
            if (steps.Length == 0)
                return;
            var generator = request.distribution.ResolveBindingGenerator(request.target.value);
            var bindingContext = new NativeBuildContext(request.root, request.configuration.ToLowerInvariant()).WithBindingGenerator(generator);
            bindingContext = bindingContext.WithToolchain(await request.distribution.ResolveNativeToolchain(request.target.value)
                .ResolveAsync(bindingContext, request.composition.host, request.target.value, cancellation).ConfigureAwait(false));
            // Verify the exact target publications being released, with their product linkage/options.
            // Authored host descriptors contain the author's SDK paths and are not target package evidence.
            var bindingRequest = new NativeBindingGenerationRequest(steps.Select(static group => group.First().component),
                request.target.value, NativeBindingOutputMode.TargetArtifacts, checkOnly: true,
                componentOptions: steps.ToDictionary(static group => group.Key, static group => group.First().options, StringComparer.Ordinal));
            using NativeBindingGenerationResult verified = await generator.GenerateAsync(bindingContext, bindingRequest, cancellation).ConfigureAwait(false);
            Console.WriteLine($"Verified target binding publications: {steps.Length}; native processes: {bindingContext.statistics.nativeProcesses}.");
            return;
        }

        if (operation is "select" or "evaluate")
        {
            string output = options.Require("output");
            try
            {
                var assets = await NativeDependencyManifest.SelectAsync(request.root, options.Require("native-dependencies"),
                    request.target.value, request.configuration.ToLowerInvariant(), product.plan, cancellation).ConfigureAwait(false);
                byte[] result = operation == "select" ? JsonSerializer.SerializeToUtf8Bytes(assets)
                    : JsonSerializer.SerializeToUtf8Bytes(new { matched = true, assets });
                Inno.Core.IO.AtomicFile.WriteAllBytes(output, result);
            }
            catch (InvalidDataException failure) when (operation == "evaluate")
            {
                Inno.Core.IO.AtomicFile.WriteAllBytes(output, JsonSerializer.SerializeToUtf8Bytes(new { matched = false, reason = failure.Message }));
            }
            return;
        }

        if (operation != "produce")
            throw new ArgumentException("Unknown native dependency operation: " + operation);
        if (options.Read("native-dependencies", string.Empty).Length != 0)
            throw new ArgumentException("Dependency production cannot consume a prebuilt selection.");
        var phase = Stopwatch.StartNew();
        Console.WriteLine($"Native producer: {product.plan.productId}/{request.target.value}/{request.configuration}; resolving the explicit toolchain.");
        var context = new NativeBuildContext(request.root, request.configuration.ToLowerInvariant()).WithBindingGenerator(request.distribution.ResolveBindingGenerator(request.target.value));
        context = context.WithToolchain(await request.distribution.ResolveNativeToolchain(request.target.value).ResolveAsync(context, request.composition.host, request.target.value, cancellation).ConfigureAwait(false));
        Report("toolchain");
        StandardBuildCaches.CreateOwners(request.root, product.plan.steps.Select(static step => step.component).ToArray());
        string identity = await ProductNativePreparation.FreezeAsync(context, product.plan, cancellation).ConfigureAwait(false);
        Report("input-capture");
        using ProductNativeBuildResult products = await product.BuildAsync(context, cancellation).ConfigureAwait(false);
        Report("production");
        if (identity != await ProductNativePreparation.FreezeAsync(context.BeginInputVerification("dependency-export"), product.plan, cancellation).ConfigureAwait(false))
            throw new InvalidOperationException("Native producer inputs changed before export.");
        Report("pre-export-verification");
        string selection = await NativeDependencyManifest.ExportAsync(context, product.plan, products, options.Require("release"), options.Require("output"), cancellation).ConfigureAwait(false);
        Report("export");
        if (identity != await ProductNativePreparation.FreezeAsync(context.BeginInputVerification("dependency-export-complete"), product.plan, cancellation).ConfigureAwait(false))
            throw new InvalidOperationException("Native producer inputs changed during export.");
        Report("closing-verification");
        Console.WriteLine("Native dependency selection: " + selection);
        Console.WriteLine($"Native processes: {context.statistics.nativeProcesses}; hashed bytes: {context.statistics.hashedBytes}.");

        void Report(string name)
        {
            Console.WriteLine(FormattableString.Invariant($"Native producer phase={name} seconds={phase.Elapsed.TotalSeconds:F3} nativeProcesses={context.statistics.nativeProcesses} hashedBytes={context.statistics.hashedBytes}"));
            phase.Restart();
        }
    }
}
