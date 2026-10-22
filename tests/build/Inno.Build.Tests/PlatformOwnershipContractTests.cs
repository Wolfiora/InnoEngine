using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Bindings;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.MacOSArm64;
using Inno.Build.Products.Standard;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;
using Inno.Build.WindowsX64;
using Inno.Integration.BrowserWasm32.Bgfx;
using Inno.Testing.Source;

namespace Inno.Build.Tests;

public sealed class PlatformOwnershipContractTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RealPlatformPreflightRejectsUnavailableSdkOrHostWithoutCreatingOutput(bool unsupportedHost)
    {
        string output = Path.Combine(Path.GetTempPath(), "InnoPlatformPreflight", Guid.NewGuid().ToString("N"));
        string sdk = Path.Combine(output, "unavailable-dotnet.exe");
        var host = unsupportedHost ? new BuildHostDescriptor("MacOS", "arm64")
            : new BuildHostDescriptor("Windows", "x64");
        var source = new WindowsX64PlayerSupportPackSource(
            BuildTestProducts.Distribution().ResolveNativeProduct("windows-x64", "player"),
            host, sdk, BuildTestProducts.Distribution().ResolveBindingGenerator("windows-x64"), new WindowsX64SupportPackValidator());
        var publisher = new PlayerSupportPackPublisher([source]);

        if (unsupportedHost)
            await Assert.ThrowsAsync<PlatformNotSupportedException>(() => publisher.PublishAsync(
                TestRepository.GetRoot(typeof(PlatformOwnershipContractTests).Assembly), output, new BuildTargetId("windows-x64"), sdk).AsTask());
        else
            await Assert.ThrowsAsync<FileNotFoundException>(() => publisher.PublishAsync(
                TestRepository.GetRoot(typeof(PlatformOwnershipContractTests).Assembly), output, new BuildTargetId("windows-x64"), sdk).AsTask());

        Assert.False(Directory.Exists(output));
    }

    [Fact]
    public void StaticComponentsHaveOneAggregateOwnerAndNoUnusedStandaloneProducer()
    {
        ProductNativeBuildPlan plan = BuildTestProducts.StaticPlayer();

        Assert.All(plan.steps, static step => {
            Assert.Null(step.build);
            Assert.NotNull(step.component.staticBuild);
        });
        Assert.Throws<ArgumentException>(() => new ProductNativeBuildStep("fixture", Step("fixture", []).component, new(NativeLibraryKind.Static),
            [], null, null, static (
                relative,
                target
            ) => relative));
    }

    [Fact]
    public void StandardShaderTargetIsIndependentOfTheOfflineToolTarget()
    {
        var context = new BuildCompositionContext("unresolved-sdk", Path.GetFullPath(Path.GetTempPath()),
            new BuildHostDescriptor("Windows", "x64"), new BuildTargetId("windows-x64"));
        StandardBuildDistribution distribution = StandardBuildDistribution.Create(context);

        Assert.Same(BrowserWasm32BgfxShaderProfiles.target,
            distribution.ResolveShaderTarget(new BuildTargetId("browser-wasm")));
        Assert.Throws<NotSupportedException>(() => distribution.build.ResolveNativeProduct("linux-arm64", "shader-tools"));
        Assert.Throws<NotSupportedException>(() => distribution.ResolveShaderTarget(new("unregistered-target")));
        Assert.Throws<NotSupportedException>(() => distribution.build.ResolveNativeProduct("browser-wasm", "shader-tools"));
    }

    [Fact]
    public void DistributionDoesNotDeriveProductTargetsFromTheToolHost()
    {
        var windows = new BuildCompositionContext("unresolved-sdk", Path.GetFullPath(Path.GetTempPath()),
            new BuildHostDescriptor("Windows", "x64"), new BuildTargetId("windows-x64"));
        var apple = new BuildCompositionContext("unresolved-sdk", Path.GetFullPath(Path.GetTempPath()),
            new BuildHostDescriptor("MacOS", "arm64"), new BuildTargetId("macos-arm64"));
        BuildDistribution first = StandardBuildDistribution.Create(windows).build;
        BuildDistribution second = StandardBuildDistribution.Create(apple).build;

        Assert.Equal(first.availableTargets, second.availableTargets);
        Assert.Contains(new BuildTargetId("browser-wasm"), first.availableTargets);
        ProductNativeBuildPlan browser = first.ResolveNativeProduct("browser-wasm", "player").plan;
        Assert.Equal(browser.steps.Select(static step => step.component.id),
            second.ResolveNativeProduct("browser-wasm", "player").plan.steps.Select(static step => step.component.id));
        Assert.All(browser.steps, static step =>
        {
            Assert.Equal(NativeLibraryKind.Static, step.options.libraryKind);
            Assert.Null(step.build);
            Assert.NotNull(step.component.staticBuild);
        });
        Assert.Throws<NotSupportedException>(() => first.ResolveNativeProduct("browser-wasm", "editor"));
        Assert.DoesNotContain(new BuildTargetId("desktop"), first.availableTargets);
        Assert.DoesNotContain(new BuildTargetId("linux-x64"), first.availableTargets);
        Assert.Throws<NotSupportedException>(() => first.ResolveNativeToolchain("linux-x64"));
        Assert.Throws<NotSupportedException>(() => first.ResolveNativeToolchain("fixture-unregistered"));
        Assert.Equal("x64", WindowsX64BuildModule.target.architecture);
        Assert.Equal("arm64", MacOSArm64BuildModule.target.architecture);
    }

    [Theory]
    [InlineData("MacOS", "arm64", "windows-x64")]
    [InlineData("Windows", "x64", "macos-arm64")]
    [InlineData("Windows", "arm64", "windows-x64")]
    public async Task UnsupportedHostAndTargetFailBeforeResolvingAnSdkOrCreatingArtifacts(
        string system,
        string architecture,
        string target
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoPlatformPreflight", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), string.Empty);
        try
        {
            var context = new NativeBuildContext(root, "debug");
            var host = new BuildHostDescriptor(system, architecture);
            INativeToolchainProvider provider = target == "macos-arm64"
                ? new MacOSArm64NativeToolchainProvider("deliberately-unavailable-sdk")
                : new WindowsX64NativeToolchainProvider("deliberately-unavailable-sdk");

            await Assert.ThrowsAsync<PlatformNotSupportedException>(() =>
                provider.ResolveAsync(context, host, target, CancellationToken.None).AsTask());
            Assert.False(Directory.Exists(Path.Combine(root, "artifacts")));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void ProductClosureFreezesOrderAndRejectsDuplicateOrUnfulfilledDependencies()
    {
        ProductNativeBuildStep first = Step("first", []);
        ProductNativeBuildStep second = Step("second", ["first"]);
        ProductNativeBuildStep[] declarations = [first, second];
        var plan = new ProductNativeBuildPlan("fixture-product", declarations);
        declarations[0] = second;

        Assert.Same(first, plan.steps[0]);
        Assert.Same(second, plan.steps[1]);
        Assert.Throws<ArgumentException>(() => new ProductNativeBuildPlan("fixture-product", []));
        Assert.Throws<ArgumentException>(() => new ProductNativeBuildPlan("fixture-product", [first, first]));
        Assert.Throws<ArgumentException>(() => new ProductNativeBuildPlan("fixture-product", [second, first]));
    }

    [Fact]
    public async Task UnselectedToolchainAndCancellationNeverRunAComponentProducer()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoPlatformPreflight", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), string.Empty);
        try
        {
            var context = new NativeBuildContext(root, "debug");
            var plan = new ProductNativeBuildPlan("fixture-product", [Step("first", [])]);
            await Assert.ThrowsAsync<InvalidOperationException>(() => new ComponentNativeProductExecutor().BuildAsync(context, plan, CancellationToken.None));
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => new ComponentNativeProductExecutor().BuildAsync(context, plan, cancellation.Token));
            Assert.False(Directory.Exists(Path.Combine(root, "artifacts")));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static ProductNativeBuildStep Step(
        string id,
        string[] dependencies
    ) => new(id, new NativeComponentDescriptor(id,
        "backends/Fixture/native/Inno.Native.Fixture/Inno.Native.Fixture.csproj",
        "backends/Fixture/build/Inno.Build.Toolchains.Fixture/Inno.Build.Toolchains.Fixture.csproj"), new(NativeLibraryKind.Shared), dependencies,
        static (
            context,
            completed,
            cancellation
        ) => throw new Xunit.Sdk.XunitException("Preflight must not execute a component producer."),
        static context => throw new Xunit.Sdk.XunitException("Preflight must reject missing tools before source declaration."),
        static (
            relative,
            target
        ) => relative);
}
