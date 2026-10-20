using Inno.Testing.Source;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Bindings;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Build.WindowsX64;
using Inno.Integration.WindowsX64.Bgfx;
using Xunit;

namespace Inno.Build.Tests;

public sealed class ProductNativeCostTests
{
    [Fact]
    public async Task RealPresentationBuildUsesDeclaredCMakeAndFrozenWindowSource()
    {
        if (Environment.GetEnvironmentVariable("INNO_ACCEPTANCE_IMGUI_BUILD") is null)
            return;
        string root = TestRepository.GetRoot(typeof(ProductNativeCostTests).Assembly);
        ProductNativeBuildStep[] steps = StandardNativeBuildPlans.CreateEditor(WindowsX64BgfxIntegration.nativeProfile).steps
            .Where(static step => step.id is "cimgui" or "cimguizmo").ToArray();
        var plan = new ProductNativeBuildPlan("presentation-acceptance", steps);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(15));
        var context = new NativeBuildContext(root, "debug").WithBindingGenerator(new NativeBindingGenerator());
        NativeToolchainSelection sdk = await new WindowsX64NativeToolchainProvider("dotnet").ResolveAsync(context,
            new BuildHostDescriptor("Windows", "x64"), "windows-x64", cancellation.Token);
        context = context.WithToolchain(sdk);
        using ProductNativeBuildResult result = await plan.BuildAsync(context, cancellation.Token);
        Assert.Equal(2, result.Count);
        Assert.All(result, product => Assert.Contains(product.files, file => Path.GetExtension(file) == ".dll"));
    }

    [Fact]
    public async Task RealHotProductBatchReadsLessThanSequentialPublicProducers()
    {
        if (Environment.GetEnvironmentVariable("INNO_ACCEPTANCE_NATIVE_COST") is not string evidence)
            return;
        string root = TestRepository.GetRoot(typeof(ProductNativeCostTests).Assembly);
        ProductNativeBuildPlan plan = StandardNativeBuildPlans.CreateEditor(WindowsX64BgfxIntegration.nativeProfile);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(20));
        async Task<NativeBuildContext> Context()
        {
            var context = new NativeBuildContext(root, "debug").WithBindingGenerator(new NativeBindingGenerator());
            NativeToolchainSelection sdk = await new WindowsX64NativeToolchainProvider("dotnet").ResolveAsync(context,
                new BuildHostDescriptor("Windows", "x64"), "windows-x64", cancellation.Token);
            return context.WithToolchain(sdk);
        }
        NativeBuildContext serial = await Context();
        Stopwatch elapsed = Stopwatch.StartNew();
        using (NativeBindingGenerationResult bindings = await NativeBindingPreparation.PrepareAsync(serial,
            plan.steps.Select(static step => step.component).ToArray(), cancellation.Token))
        {
            serial = serial.WithBindings(bindings);
            var products = new Dictionary<string, NativeBuildProduct>(StringComparer.Ordinal);
            try
            {
                foreach (ProductNativeBuildStep step in plan.steps)
                {
                    NativeBuildContext selected = (step.configuration is string configuration
                        ? serial.WithConfiguration(configuration) : serial).WithComponentOptions(step.component, step.options);
                    products.Add(step.id, await step.build!(selected, products, cancellation.Token));
                }
            }
            finally
            {
                foreach (NativeBuildProduct product in products.Values.Reverse())
                    product.Dispose();
            }
        }
        double serialSeconds = elapsed.Elapsed.TotalSeconds;
        Assert.Equal(0, serial.statistics.nativeProcesses);
        Assert.Equal(0, serial.statistics.bindingGenerations);
        NativeBuildContext batch = await Context();
        elapsed.Restart();
        using ProductNativeBuildResult result = await plan.BuildAsync(batch, cancellation.Token);
        double batchSeconds = elapsed.Elapsed.TotalSeconds;
        Assert.Equal(plan.steps.Count, result.Count);
        Assert.Equal(0, batch.statistics.nativeProcesses);
        Assert.Equal(0, batch.statistics.bindingGenerations);
        Assert.True(batch.statistics.hashedBytes < serial.statistics.hashedBytes);
        Assert.Single(batch.statistics.phases.Where(static phase => phase.phase == "native-product-lock"));
        Directory.CreateDirectory(evidence);
        await File.WriteAllBytesAsync(Path.Combine(evidence, "native-serial-batch-cost.json"), JsonSerializer.SerializeToUtf8Bytes(new
        {
            serialSeconds, batchSeconds, serial = serial.statistics, batch = batch.statistics
        }, new JsonSerializerOptions { WriteIndented = true }), cancellation.Token);
    }
}
