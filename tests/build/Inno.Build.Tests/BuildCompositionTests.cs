using Inno.Build.Bindings;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets.Pipeline;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Build.Managed;
using Inno.Build.SupportPacks;
using Inno.Core.Identity;
using Inno.Runtime;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BuildCompositionTests
{
    [Fact]
    public void EveryHostObtainsTheSameFrozenBuiltInDistribution()
    {
        var context = new BuildCompositionContext("explicit-sdk", Path.GetFullPath(Path.GetTempPath()), new BuildHostDescriptor("Windows", "x64"), new BuildTargetId("windows-x64"), new NativeBindingGenerator("explicit-sdk"));
        BuildDistribution editor = StandardBuildDistribution.Create(context).build;
        BuildDistribution cli = StandardBuildDistribution.Create(context).build;
        BuildDistribution msbuild = StandardBuildDistribution.Create(context).build;
        Assert.Equal(editor.availableTargets, cli.availableTargets);
        Assert.Equal(cli.availableTargets, msbuild.availableTargets);
        Assert.Equal(3, editor.availableTargets.Count);
        Assert.Equal(editor.managedDeployments.availableDeployments, msbuild.managedDeployments.availableDeployments);
        Assert.Equal(4, editor.managedDeployments.availableDeployments.Count);
        Assert.Equal(editor.ResolveNativeProduct("windows-x64", "editor").steps.Select(static step => step.id),
            msbuild.ResolveNativeProduct("windows-x64", "editor").steps.Select(static step => step.id));
        Assert.Throws<NotSupportedException>(() => editor.ResolveNativeProduct("browser-wasm", "editor"));
        Assert.Throws<ArgumentException>(() => new BuildCompositionContext("sdk", "relative", new BuildHostDescriptor("Windows", "x64"), new BuildTargetId("windows-x64"), new NativeBindingGenerator("explicit-sdk")));
    }

    [Fact]
    public void TargetsContributeDistinctImmutableProductClosuresWithoutChangingTheInvokingHost()
    {
        var first = new FixtureTarget(new BuildTargetId("fixture-a"));
        var second = new FixtureTarget(new BuildTargetId("fixture-b"));
        ProductNativeBuildPlan complete = StandardNativeBuildPlans.CreatePlayer(Inno.Integration.Windows.Bgfx.WindowsBgfxIntegration.nativeProfile);
        var reduced = new ProductNativeBuildPlan("player", [complete.steps[0]]);
        ProductNativeBuildPlan[] products = [complete];
        var firstContribution = new BuildPlatformContribution(Descriptor(first), () => first, first, new FixtureToolchain(), nativeProducts: products);
        var secondContribution = new BuildPlatformContribution(Descriptor(second), () => second, second, new FixtureToolchain(), nativeProducts: [reduced]);
        var distribution = new BuildDistribution([Game(firstContribution, first), Game(secondContribution, second)], [new FixtureCompiler()]);
        products[0] = null!;
        Assert.Same(complete, distribution.ResolveNativeProduct("fixture-a", "player"));
        Assert.Same(reduced, distribution.ResolveNativeProduct("fixture-b", "player"));
        Assert.Throws<NotSupportedException>(() => distribution.ResolveNativeProduct("fixture-a", "editor"));
        Assert.Throws<NotSupportedException>(() => distribution.ResolveNativeProduct("absent-target", "player"));
        Assert.Throws<ArgumentException>(() => new BuildPlatformContribution(Descriptor(first), () => first, first, new FixtureToolchain(), nativeProducts: [complete, reduced]));
    }

    [Fact]
    public void CustomTargetComposesAndRejectsInconsistentDefaultsBeforePublication()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoBuildCompositionTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "Assets"));
        try
        {
            using EngineHost engine = new EngineHostBuilder()
                .UseMetadataSources(
                    new DotNetAssemblyCatalogSource(typeof(BuildCompositionTests).Assembly),
                    new ReflectionTypeCatalogSource(), new ReflectionSerializationMetadataSource())
                .Build();
            using var assets = new AssetPipeline(
                engine.modules, engine.types, engine.serialization, new IdentityAllocator(), engine.diagnostics, engine.logs,
                AssetPipelineOptions.Create(Path.Combine(root, "Assets"), Path.Combine(root, "Library")) with
                { enableFileSystemWatcher = false });
            var target = new FixtureTarget();
            BuildTargetFactory factory = () => target;
            GameBuildContribution[] platforms = [Contribution(target, factory)];
            var distribution = new BuildDistribution(platforms, [new FixtureCompiler()]);
            platforms[0] = null!;
            Assert.Same(target, Assert.Single(distribution.CreateBindings(assets, engine.serialization, engine.types)).packager);
            distribution.ValidateDeployment(target, target.defaultManagedDeployment);
            Assert.Throws<InvalidOperationException>(() => distribution.ValidateDeployment(target, ManagedDeploymentId.coreClr));
            Assert.Throws<ArgumentException>(() => new BuildPlatformContribution(
                Descriptor(target), factory, new FixtureTarget(new BuildTargetId("different")), new FixtureToolchain()));
            var invalid = new BuildDistribution([Contribution(target, () =>
                new FixtureTarget(new BuildTargetId("different")))], [new FixtureCompiler()]);
            Assert.Throws<InvalidOperationException>(() => invalid.CreateBindings(assets, engine.serialization, engine.types));
            var unsupported = new BuildDistribution([Contribution(target, factory)], [new FixtureCompiler("wrong-rid")]);
            Assert.Throws<InvalidOperationException>(() => unsupported.CreateBindings(assets, engine.serialization, engine.types));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void DistributionRejectsAmbiguousOrIncompleteRegistrationSets()
    {
        var target = new FixtureTarget();
        BuildTargetFactory factory = () => target;
        Assert.Throws<ArgumentException>(() => new BuildDistribution([], [new FixtureCompiler()]));
        Assert.Throws<ArgumentException>(() => new BuildDistribution([Contribution(target, factory), Contribution(target, factory)], [new FixtureCompiler()]));
        Assert.Throws<ArgumentException>(() => new BuildDistribution([Contribution(target, factory), null!], [new FixtureCompiler()]));
    }

    private static PlatformTargetDescriptor Descriptor(FixtureTarget target) =>
        new(target.id, "Fixture", "fixture-cpu", "fixture-abi", target.runtimeIdentifier, supportsEditor: false);

    private static GameBuildContribution Contribution(
        FixtureTarget target,
        BuildTargetFactory factory
    ) => Game(new(Descriptor(target), factory, target, new FixtureToolchain()), target);

    private static GameBuildContribution Game(
        BuildPlatformContribution platform,
        FixtureTarget compiler
    ) => new(platform, (
        assets,
        serialization,
        types
    ) => compiler);

    private sealed class FixtureToolchain : INativeToolchainProvider
    {
        public ValueTask<NativeToolchainSelection> ResolveAsync(
            NativeBuildContext context,
            BuildHostDescriptor host,
            string targetId,
            CancellationToken cancellationToken
        ) => throw new InvalidOperationException("Composition must not resolve an SDK.");
    }

    private sealed class FixtureCompiler(string runtimeIdentifier = "fixture-rid") : IManagedDeploymentCompiler
    {
        public ManagedDeploymentId id => new("fixture-managed");
        public ManagedDeploymentCapabilities capabilities { get; } = new(
            [runtimeIdentifier], dynamicCode: false, aheadOfTime: true, nativeStaticLinking: true);

        public ValueTask<ManagedDeploymentResult> CompileAsync(
            ManagedDeploymentRequest request,
            CancellationToken cancellationToken = default
        ) => throw new InvalidOperationException("Composition must not start publication.");
    }

    private sealed class FixtureTarget(BuildTargetId? identity = null) : IGameBuildTarget, IPlayerSupportPackSource, IGameContentCompiler
    {
        public BuildTargetId id { get; } = identity ?? new BuildTargetId("fixture-platform");
        public BuildTargetId target => id;
        public ManagedDeploymentId defaultManagedDeployment => new("fixture-managed");
        public string runtimeIdentifier => "fixture-rid";
        public string displayName => "Fixture";
        public void Validate(string directory) => throw new InvalidOperationException("Composition must not read a pack.");
        public ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
            PlayerSupportPackPlanningContext context,
            CancellationToken cancellationToken
        ) => throw new InvalidOperationException("Composition must not prepare a pack.");
        public ValueTask CompileAsync(
            GameBuildContentContext context,
            CancellationToken cancellationToken = default
        ) => throw new InvalidOperationException("Composition must not compile content.");
        public ValueTask<string> PackageAsync(
            GameBuildPackageContext context,
            CancellationToken cancellationToken = default
        ) => throw new InvalidOperationException("Composition must not package content.");
    }
}
