using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Build.Windows.Host;
using Inno.Build.WindowsX64;
using Inno.Testing.Source;

namespace Inno.Build.NativePerformance.Tests;

[SupportedOSPlatform("windows")]
public sealed class ProductNativeCostTests
{
    private readonly ITestOutputHelper m_output;
    private readonly BuildHostDescriptor m_host;
    private readonly BuildDistribution m_distribution;

    public ProductNativeCostTests(ITestOutputHelper output)
    {
        m_output = output;
        m_host = new WindowsBuildHostProvider().TryCapture()
            ?? throw new PlatformNotSupportedException("Native cost acceptance requires its declared Windows host.");
        m_distribution = StandardBuildDistribution.Create(new BuildCompositionContext(
            "explicit-sdk", Path.GetTempPath(), m_host, new BuildTargetId("windows-x64"))).build;
    }

    [Fact]
    public async Task RealPresentationBuildUsesDeclaredCMakeAndFrozenWindowSource()
    {
        string root = TestRepository.GetRoot(typeof(ProductNativeCostTests).Assembly);
        ProductNativeBuildStep[] steps = m_distribution.ResolveNativeProduct("windows-x64", "editor").plan.steps
            .Where(static step => step.id is "cimgui" or "cimguizmo").ToArray();
        var plan = new ProductNativeBuildPlan("presentation-acceptance", steps);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(15));
        var context = new NativeBuildContext(root, "debug").WithBindingGenerator(m_distribution.ResolveBindingGenerator("windows-x64"));
        NativeToolchainSelection sdk = await new WindowsX64NativeToolchainProvider("dotnet").ResolveAsync(context,
            m_host, "windows-x64", cancellation.Token);
        context = context.WithToolchain(sdk);
        using ProductNativeBuildResult result = await new ComponentNativeProductExecutor().BuildAsync(context, plan, cancellation.Token);
        Assert.Equal(2, result.Count);
        Assert.All(result, product => Assert.Contains(product.files, file => Path.GetExtension(file) == ".dll"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task RealHotProductBatchReadsLessThanSequentialPublicProducers(int sample)
    {
        string root = TestRepository.GetRoot(typeof(ProductNativeCostTests).Assembly);
        ProductNativeBuildPlan plan = m_distribution.ResolveNativeProduct("windows-x64", "editor").plan;
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(20));
        async Task<NativeBuildContext> Context()
        {
            var context = new NativeBuildContext(root, "debug").WithBindingGenerator(m_distribution.ResolveBindingGenerator("windows-x64"));
            NativeToolchainSelection sdk = await new WindowsX64NativeToolchainProvider("dotnet").ResolveAsync(context,
                m_host, "windows-x64", cancellation.Token);
            return context.WithToolchain(sdk);
        }
        NativeBuildContext serial = await Context();
        Stopwatch elapsed = Stopwatch.StartNew();
        using (NativeBindingGenerationResult bindings = await NativeBindingPreparation.PrepareAsync(serial,
            plan.steps.Select(static step => step.component).ToArray(), cancellation.Token,
            plan.steps.Where(static step => step.component.bindingDefinition is not null)
                .GroupBy(static step => step.component.nativeProject, StringComparer.Ordinal)
                .ToDictionary(static group => group.Key, static group => group.First().options, StringComparer.Ordinal)))
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
        m_output.WriteLine(JsonSerializer.Serialize(new { sample, phase = "serial", seconds = serialSeconds, serial.statistics }));
        Assert.Equal(0, serial.statistics.nativeProcesses);
        Assert.Equal(0, serial.statistics.bindingGenerations);
        NativeBuildContext batch = await Context();
        elapsed.Restart();
        using ProductNativeBuildResult result = await new ComponentNativeProductExecutor().BuildAsync(batch, plan, cancellation.Token);
        double batchSeconds = elapsed.Elapsed.TotalSeconds;
        m_output.WriteLine(JsonSerializer.Serialize(new { sample, phase = "batch", seconds = batchSeconds, batch.statistics }));
        Assert.Equal(plan.steps.Count, result.Count);
        Assert.Equal(0, batch.statistics.nativeProcesses);
        Assert.Equal(0, batch.statistics.bindingGenerations);
        Assert.True(batch.statistics.hashedBytes < serial.statistics.hashedBytes);
        Assert.Single(batch.statistics.phases.Where(static phase => phase.phase == "native-product-lock"));
    }
}
