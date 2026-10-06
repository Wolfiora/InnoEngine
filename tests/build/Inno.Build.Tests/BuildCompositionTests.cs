using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets.Pipeline;
using Inno.Build.Composition;
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
        var context = new BuildCompositionContext("explicit-sdk", Path.GetFullPath(Path.GetTempPath()));
        BuildDistribution editor = BuiltInBuildDistribution.Create(context);
        BuildDistribution cli = BuiltInBuildDistribution.Create(context);
        BuildDistribution msbuild = BuiltInBuildDistribution.Create(context);
        Assert.Equal(editor.availableTargets, cli.availableTargets);
        Assert.Equal(cli.availableTargets, msbuild.availableTargets);
        Assert.Equal(3, editor.availableTargets.Count);
        Assert.Equal(editor.managedDeployments.availableDeployments, msbuild.managedDeployments.availableDeployments);
        Assert.Equal(4, editor.managedDeployments.availableDeployments.Count);
        Assert.Throws<ArgumentException>(() => new BuildCompositionContext("sdk", "relative"));
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
            BuildTargetFactory factory = (
                authoring,
                serialization,
                types
            ) => target;
            BuildTargetFactory[] factories = [factory];
            IPlayerSupportPackSource[] sources = [target];
            var distribution = new BuildDistribution(factories, [new FixtureCompiler()], sources);
            factories[0] = (
                authoring,
                serialization,
                types
            ) => throw new InvalidOperationException("The caller mutated its collection.");
            sources[0] = null!;
            Assert.Same(target, Assert.Single(distribution.CreateTargets(assets, engine.serialization, engine.types)));
            distribution.ValidateDeployment(target, target.defaultManagedDeployment);
            Assert.Throws<InvalidOperationException>(() => distribution.ValidateDeployment(target, ManagedDeploymentId.coreClr));
            var invalid = new BuildDistribution([factory], [new FixtureCompiler()], [new FixtureTarget(new BuildTargetId("different"))]);
            Assert.Throws<InvalidOperationException>(() => invalid.CreateTargets(assets, engine.serialization, engine.types));
            var unsupported = new BuildDistribution([factory], [new FixtureCompiler("wrong-rid")], [target]);
            Assert.Throws<InvalidOperationException>(() => unsupported.CreateTargets(assets, engine.serialization, engine.types));
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
        BuildTargetFactory factory = (
            assets,
            serialization,
            types
        ) => target;
        Assert.Throws<ArgumentException>(() => new BuildDistribution([], [new FixtureCompiler()], []));
        Assert.Throws<ArgumentException>(() => new BuildDistribution([factory], [new FixtureCompiler()], [target, target]));
        Assert.Throws<ArgumentException>(() => new BuildDistribution([factory, factory], [new FixtureCompiler()], [target, target]));
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

    private sealed class FixtureTarget(BuildTargetId? identity = null) : IGameBuildTarget, IPlayerSupportPackSource
    {
        public BuildTargetId id { get; } = identity ?? new BuildTargetId("fixture-platform");
        public BuildTargetId target => id;
        public ManagedDeploymentId defaultManagedDeployment => new("fixture-managed");
        public string runtimeIdentifier => "fixture-rid";
        public string displayName => "Fixture";
        public bool isPreferredOnCurrentHost => false;
        public void Validate(string directory) => throw new InvalidOperationException("Composition must not read a pack.");
        public ValueTask PrepareAsync(
            PlayerSupportPackBuildContext context,
            CancellationToken cancellationToken
        ) => throw new InvalidOperationException("Composition must not prepare a pack.");
        public ValueTask BuildContentAsync(
            GameBuildContentContext context,
            CancellationToken cancellationToken = default
        ) => throw new InvalidOperationException("Composition must not compile content.");
        public ValueTask<string> PackageAsync(
            GameBuildPackageContext context,
            CancellationToken cancellationToken = default
        ) => throw new InvalidOperationException("Composition must not package content.");
    }
}
