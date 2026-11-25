using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Build.WindowsX64;
using Inno.Testing.Source;

namespace Inno.Build.NativeProducer.Integration.Tests;

[SupportedOSPlatform("windows")]
public sealed class NativeProductProducerTests
{
    private readonly BuildHostDescriptor m_host;
    private readonly BuildDistribution m_distribution;

    public NativeProductProducerTests()
    {
        m_host = new WindowsBuildHostProvider().TryCapture()
                 ?? throw new PlatformNotSupportedException("Native producer acceptance requires its declared Windows host.");
        m_distribution = StandardBuildDistribution.Create(
            new BuildCompositionContext("explicit-sdk", Path.GetTempPath(), m_host, new BuildTargetID("windows-x64"))
        ).build;
    }

    [Fact]
    public async Task RealPresentationBuildUsesDeclaredCMakeAndFrozenWindowSource()
    {
        string root = TestRepository.GetRoot(typeof(NativeProductProducerTests).Assembly);
        ProductNativeBuildStep[] steps = m_distribution.ResolveNativeProduct("windows-x64", "editor").plan.steps
            .Where(static step => step.id is "cimgui" or "cimguizmo")
            .ToArray();
        var plan = new ProductNativeBuildPlan("presentation-acceptance", steps);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(15));
        NativeBuildContext context = new NativeBuildContext(root, "debug").WithBindingGenerator(
            m_distribution.ResolveBindingGenerator("windows-x64")
        );
        NativeToolchainSelection sdk = await new WindowsX64NativeToolchainProvider("dotnet").ResolveAsync(
            context,
            m_host,
            "windows-x64",
            cancellation.Token
        );
        context = context.WithToolchain(sdk);
        using ProductNativeBuildResult result = await new ComponentNativeProductExecutor().BuildAsync(context, plan, cancellation.Token);
        Assert.Equal(2, result.Count);
        Assert.All(result, product => Assert.Contains(product.files, file => Path.GetExtension(file) == ".dll"));
    }
}
