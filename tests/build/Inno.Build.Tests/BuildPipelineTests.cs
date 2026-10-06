using Inno.Core.IO;
using Inno.Content;
using Inno.Adapter.Content.FileSystem;
using Inno.Adapter.Serialization.DotNet;
using Inno.Adapter.Modules.DotNet;
using System;
using Inno.Build.Managed;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build.Platform.MacOS;
using Inno.Build.Platform.Windows;
using Inno.Core.Identity;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Types;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scripting.Compiler;

using Xunit;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Build.Tests;

[Collection("Build pipeline serialization")]
public sealed class BuildPipelineTests : IDisposable
{
    private readonly AssetPipeline m_assets;
    private readonly EngineHost m_engine;
    private readonly IdentityAllocator m_identities = new();
    private readonly BuildPipeline m_pipeline;
    private readonly PluginEnvironment m_plugins;
    private readonly ScriptCompiler m_compiler;
    private readonly string m_projectRoot;
    private readonly string m_root;
    private readonly ProjectSettingsStore m_settings;
    private readonly SceneWorld m_sceneWorld;
    private readonly IDisposable m_sceneWorldScope;
    private readonly string m_supportPackRoot;

    public BuildPipelineTests()
    {
        m_root = Path.Combine(Path.GetTempPath(), "InnoBuildTests", Guid.NewGuid().ToString("N"));
        m_projectRoot = Path.Combine(m_root, "Project");
        string assetRoot = Path.Combine(m_projectRoot, "Assets");
        string pluginRoot = Path.Combine(m_projectRoot, "Plugins");
        string libraryRoot = Path.Combine(m_projectRoot, "Library");
        m_supportPackRoot = Path.Combine(m_root, "SupportPacks");
        Directory.CreateDirectory(assetRoot);
        Directory.CreateDirectory(pluginRoot);
        CreateSupportPack(BuildTargetId.macOSArm64, "Inno.Player");
        CreateSupportPack(BuildTargetId.windowsX64, "Inno.Player.exe");

        m_engine = new EngineHostBuilder()
                .UseMetadataSources(new DotNetAssemblyCatalogSource(typeof(BuildPipelineTests).Assembly),
                    new ReflectionTypeCatalogSource(), new ReflectionSerializationMetadataSource())
            .Build();
        m_sceneWorld = new SceneWorld(m_identities, m_engine.types);
        m_sceneWorldScope = m_sceneWorld.EnterScope();
        var sources = new PluginSourceService(m_engine.serialization, pluginRoot, libraryRoot);
        PluginScanResult scan = sources.Scan();
        m_assets = new AssetPipeline(
            m_engine.modules,
            m_engine.types,
            m_engine.serialization,
            m_identities,
            m_engine.diagnostics,
            m_engine.logs,
            AssetPipelineOptions.Create(assetRoot, libraryRoot) with
            {
                enableFileSystemWatcher = false
            });
        m_settings = new ProjectSettingsStore(
            new FileByteDocumentStore(Path.GetFullPath(Path.Combine(m_projectRoot, "Settings.Project.inno"))),
            m_engine.types,
            m_engine.serialization,
            new ProjectId("tests.build"),
            AssetSerializationContext.Create(m_assets));
        m_plugins = new PluginEnvironment(
            m_assets,
            m_settings,
            m_engine.serialization,
            pluginRoot,
            libraryRoot,
            scan,
            m_engine.modules.generations);
        m_compiler = new ScriptCompiler(
            new ScriptCompilerOptions
            {
                projectRootDirectory = m_projectRoot
            },
            m_assets,
            m_plugins);
        m_pipeline = CreatePipeline();
    }

    public void Dispose()
    {
        m_plugins.Dispose();
        m_assets.Dispose();
        m_settings.Dispose();
        m_sceneWorldScope.Dispose();
        m_sceneWorld.Dispose();
        m_engine.Dispose();
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    [Fact]
    public void ProjectSettingsUseTheHostSuppliedInitialIdentity()
    {
        Assert.Equal("tests.build", m_settings.projectId.value);
        Assert.False(m_settings.HasProjectOverride(ProjectIdentitySettings.settingId));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void DesktopPackRejectsAMissingContentSource(bool windows)
    {
        BuildTargetId target = windows ? BuildTargetId.windowsX64 : BuildTargetId.macOSArm64;
        string pack = GetSupportPackDirectory(target);
        File.Delete(Path.Combine(pack, "PlayerLink", "FilePlayerContentSource.cs"));
        IPlayerSupportPackValidator validator = windows
            ? new WindowsSupportPackValidator() : new MacOSSupportPackValidator();

        InvalidDataException missing = Assert.Throws<InvalidDataException>(() => validator.Validate(pack));

        Assert.Contains("FilePlayerContentSource.cs", missing.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void BuildTargetsAreOpenIdsOwnedByTheRegisteredAdapterCatalog()
    {
        Assert.Equal(
            [BuildTargetId.macOSArm64, BuildTargetId.windowsX64],
            m_pipeline.availableGameTargets);
        Assert.True(m_pipeline.TryGetGameTargetDisplayName(
            BuildTargetId.macOSArm64,
            out string macOSDisplayName));
        Assert.Equal("macOS (Apple silicon)", macOSDisplayName);
        Assert.False(m_pipeline.TryGetGameTargetDisplayName(
            new BuildTargetId("linux-x64"),
            out string missingDisplayName));
        Assert.Empty(missingDisplayName);

        var profile = new BuildProfile
        {
            applicationId = "tests.open-target",
            productName = "Open Target",
            startupScene = "Scenes/Startup.iscene",
            target = new BuildTargetId("linux-x64")
        };
        profile.Validate();
    }

    [Fact]
    public void BuildTargetInventoryCannotBeMutatedThroughItsCollectionView()
    {
        IList<BuildTargetId> targets = Assert.IsAssignableFrom<IList<BuildTargetId>>(m_pipeline.availableGameTargets);

        Assert.True(targets.IsReadOnly);
        Assert.Throws<NotSupportedException>(() => targets[0] = new BuildTargetId("unknown-target"));
        Assert.Equal([BuildTargetId.macOSArm64, BuildTargetId.windowsX64], m_pipeline.availableGameTargets);
    }

    [Theory]
    [InlineData("../other-game")]
    [InlineData("C:/absolute")]
    [InlineData("studio//game")]
    public void ProfileRejectsPersistentDataPathsOutsidePortableApplicationFolders(string path)
    {
        BuildProfile profile = CreateProfile(BuildTargetId.windowsX64);
        profile.persistentDataPath = path;
        Assert.Throws<InvalidDataException>(profile.Validate);
    }

    [Theory]
    [InlineData("macos-arm64")]
    [InlineData("windows-x64")]
    public async Task GameBuildPublishesOnlyVerifiedContentPacksAndRuntimeAssemblies(string targetValue)
    {
        SaveStartupScene();
        TypeCacheSnapshot runtimeTypes = m_engine.types.current;
        BuildTargetId target = new(targetValue);
        using SerializationGeneration serialization = m_engine.serialization.CaptureGeneration();
        BuildProfile profile = CreateProfile(target);
        profile.persistentDataPath = "studio/testgame";

        BuildResult result = await m_pipeline.BuildGameAsync(new GameBuildRequest
        {
            profile = profile,
            outputDirectory = Path.Combine(m_root, "Builds", targetValue)
        });

        Assert.True(result.succeeded);
        string outputPath = Assert.IsType<string>(result.outputPath);
        string contentHash = Assert.IsType<string>(result.contentHash);
        string packagedContent = target == BuildTargetId.macOSArm64
            ? Path.Combine(outputPath, "Contents", "Resources", "Content")
            : Path.Combine(outputPath, "Content");
        string executable = target == BuildTargetId.macOSArm64
            ? Path.Combine(outputPath, "Contents", "MacOS", "Test Game")
            : Path.Combine(outputPath, "Test Game.exe");
        Assert.True(File.Exists(executable));
        string playerRoot = target == BuildTargetId.macOSArm64
            ? Path.Combine(outputPath, "Contents", "MacOS")
            : outputPath;
        Assert.False(Directory.Exists(Path.Combine(playerRoot, "References")));
        string deployedNative = target == BuildTargetId.macOSArm64
            ? Path.Combine(outputPath, "Contents", "MacOS", "native")
            : Path.Combine(outputPath, "native");
        Assert.True(File.Exists(Path.Combine(deployedNative, target == BuildTargetId.macOSArm64
            ? "libinno-text-release.dylib"
            : "inno-text-release.dll")));
        Assert.True(File.Exists(Path.Combine(deployedNative, target == BuildTargetId.macOSArm64
            ? "libinno-ui-release.dylib"
            : "inno-ui-release.dll")));
        Assert.Equal(
            ["catalog.inno", $"content-{contentHash}.pack", "runtime.manifest"],
            Directory.EnumerateFiles(packagedContent)
                .Select(Path.GetFileName)
                .Order(StringComparer.Ordinal));

        byte[] envelope = File.ReadAllBytes(Path.Combine(packagedContent, "runtime.manifest"));
        Assert.Equal("tests.game", RuntimeManifestEnvelope.ReadApplicationId(envelope));
        Assert.Equal("studio/testgame", RuntimeManifestEnvelope.ReadPersistentDataPath(envelope));
        GameRuntimeManifest manifest = RuntimeManifestEnvelope.Decode(envelope, serialization);
        Assert.Equal("studio/testgame", manifest.persistentDataPath);
        Assert.Equal("Scenes/Startup.iscene", manifest.startupScene);
        GameRuntimeModule runtimeModule = Assert.Single(manifest.modules);
        Assert.Equal("RuntimeScripts", runtimeModule.name);
        Assert.Equal("Inno.GameScripts", Assert.Single(runtimeModule.assemblies).name);
        Assert.Equal(Inno.Extensibility.Modules.AssemblyDomain.InnoScripting, runtimeModule.domain);

        string persistentRoot = Path.Combine(m_root, "Persistent", manifest.applicationId);
        RuntimeContentCatalog contentCatalog = serialization.Deserialize<RuntimeContentCatalog>(
            File.ReadAllBytes(Path.Combine(packagedContent, "catalog.inno")));
        contentCatalog.Validate();
        using PackContentStore packStore = ContentPackReader.Open(
            File.OpenRead(Path.Combine(packagedContent, contentCatalog.packFileName)),
            new ContentPackDescriptor(contentCatalog.contentHash, contentCatalog.packFileName), serialization);
        using FileContentStore contentStore = await FileContentPreparation.PrepareAsync(packStore,
            new FileContentCacheOptions(persistentRoot));
        using var runtimeAssets = new AssetDatabase(
            contentStore,
            serialization,
            runtimeTypes,
            new IdentityAllocator(),
            residencyBudgetBytes: 0);
        Task<AssetLease<SceneAsset>> coldLoad = runtimeAssets.AcquireAsync<SceneAsset>(
            AssetPath.Project("Scenes/Startup.iscene")).AsTask();
        long preparingBytes = runtimeAssets.preparingBytes;
        Assert.True(preparingBytes > 0);
        using var coalescedCancellation = new CancellationTokenSource();
        Task<AssetLease<SceneAsset>> coalesced = runtimeAssets.AcquireAsync<SceneAsset>(
            AssetPath.Project("Scenes/Startup.iscene"), coalescedCancellation.Token).AsTask();
        Assert.Equal(preparingBytes, runtimeAssets.preparingBytes);
        coalescedCancellation.Cancel();
        Assert.False(coldLoad.IsCompleted);
        Assert.True(SpinWait.SpinUntil(() =>
        {
            runtimeAssets.CompletePendingLoads();
            return coldLoad.IsCompleted;
        }, TimeSpan.FromSeconds(5)));
        using (AssetLease<SceneAsset> lease = await coldLoad)
            Assert.NotNull(lease.asset);
        Assert.True(coalesced.IsCanceled);
        Assert.Equal(0, runtimeAssets.preparingBytes);
        Assert.Equal(0, runtimeAssets.residencyStatistics.residentAssetCount);
        using (var limited = new AssetDatabase(contentStore, serialization, runtimeTypes,
                   new IdentityAllocator(), preparationBudgetBytes: 1))
        {
            Assert.Throws<InvalidOperationException>(() => limited.AcquireAsync<SceneAsset>(
                AssetPath.Project("Scenes/Startup.iscene")));
            Assert.Equal(0, limited.preparingBytes);
        }
        using var loadCancellation = new CancellationTokenSource();
        Task<AssetLease<SceneAsset>> canceledLoad = runtimeAssets.AcquireAsync<SceneAsset>(
            AssetPath.Project("Scenes/Startup.iscene"), loadCancellation.Token).AsTask();
        loadCancellation.Cancel();
        Assert.True(SpinWait.SpinUntil(() =>
        {
            runtimeAssets.CompletePendingLoads();
            return canceledLoad.IsCompleted;
        }, TimeSpan.FromSeconds(5)));
        Assert.True(canceledLoad.IsCanceled);
        Assert.True(contentStore.index.TryGetEntry(new ContentKey("AssetDatabase/Catalog.snapshot"), out _));
        Assert.DoesNotContain(contentStore.index.entries, static entry => entry.key.value!.StartsWith("Managed/", StringComparison.Ordinal));
        Assert.DoesNotContain(
            contentStore.index.entries,
            static entry => entry.key.value!.EndsWith(".iscene", StringComparison.OrdinalIgnoreCase)
                            || entry.key.value.EndsWith(".imeta", StringComparison.OrdinalIgnoreCase)
                            || entry.key.value.EndsWith(".cs", StringComparison.OrdinalIgnoreCase));
        Assert.Equal(1, result.assetCount);
        Assert.Equal(1, result.runtimeAssemblyCount);
    }

    [Fact]
    public async Task GameBuildWaitsForPendingStartupSceneImportBeforeValidation()
    {
        AssetPath stagedPath = AssetPath.Project("Staged/Startup.iscene");
        var scene = new GameScene("Startup");
        scene.CreateObject("Player");
        Assert.True(m_assets.Save(
            stagedPath,
            SceneAsset.Capture(scene, m_engine.serialization, m_assets)));
        string stagedSource = Path.Combine(m_projectRoot, "Assets", "Staged", "Startup.iscene");
        string destinationDirectory = Path.Combine(m_projectRoot, "Assets", "Scenes");
        string destinationSource = Path.Combine(destinationDirectory, "Startup.iscene");
        Directory.CreateDirectory(destinationDirectory);
        File.Move(stagedSource, destinationSource);
        File.Move(stagedSource + ".imeta", destinationSource + ".imeta");
        m_assets.Rescan();

        BuildResult result = await m_pipeline.BuildGameAsync(new GameBuildRequest
        {
            profile = CreateProfile(BuildTargetId.macOSArm64),
            outputDirectory = Path.Combine(m_root, "Builds", "PendingImport")
        });

        Assert.True(result.succeeded);
        Assert.NotNull(result.outputPath);
    }

    [Fact]
    public async Task BackgroundArtifactExportUsesAnOwnerThreadSerializationSnapshot()
    {
        SaveStartupScene();
        string destination = Path.Combine(m_root, "RuntimeArtifacts");

        AssetRuntimeContentInfo result = await m_assets.ExportRuntimeArtifactsAsync(destination);

        Assert.Equal(1, result.assetCount);
        Assert.True(File.Exists(Path.Combine(destination, "AssetDatabase", "Catalog.snapshot")));
    }

    [Fact]
    public async Task BackgroundArtifactExportRejectsACorruptCurrentFormatManifest()
    {
        SaveStartupScene();
        string manifest = Assert.Single(Directory.EnumerateFiles(
            m_assets.artifactRoot,
            "manifest",
            SearchOption.AllDirectories));
        File.WriteAllBytes(manifest, [0x42, 0x41, 0x44]);

        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() =>
            m_assets.ExportRuntimeArtifactsAsync(Path.Combine(m_root, "CorruptRuntimeArtifacts")));

        Assert.Contains("corrupt current-format manifest", failure.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ChangedAuthoringGenerationCannotCommitAMixedBuildSnapshot()
    {
        SaveStartupScene();
        var target = new BlockingBuildTarget();
        var pipeline = new BuildPipeline(
            m_assets,
            m_plugins,
            m_settings,
            m_engine.serialization,
            m_engine.generations,
            m_compiler,
            m_supportPackRoot,
            [target],
            CreateManagedDeployments());
        string outputRoot = Path.Combine(m_root, "Builds", "MixedGeneration");

        Task<BuildResult> build = pipeline.BuildGameAsync(new GameBuildRequest
        {
            profile = CreateProfile(BuildTargetId.macOSArm64),
            outputDirectory = outputRoot
        }).AsTask();
        Assert.True(SpinWait.SpinUntil(
            () => target.started.Task.IsCompleted,
            TimeSpan.FromSeconds(2)));
        Assert.True(m_assets.Save(AssetPath.Project("Changed.txt"), new TextAsset("new generation")));
        target.release.SetResult();

        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() => build);

        Assert.Contains("generation changed", failure.Message, StringComparison.OrdinalIgnoreCase);
        Assert.False(Directory.Exists(Path.Combine(outputRoot, "Test Game.app")));
    }

    [Fact]
    public async Task CompilationFailureLeavesNoProductOrStagingDirectory()
    {
        SaveStartupScene();
        string scriptPath = Path.Combine(m_projectRoot, "Assets", "Scripts", "Broken.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(scriptPath)!);
        File.WriteAllText(scriptPath, "public sealed class Broken {");
        m_assets.Rescan();
        string outputRoot = Path.Combine(m_root, "Builds", "Failed");

        BuildResult result = await m_pipeline.BuildGameAsync(new GameBuildRequest
        {
            profile = CreateProfile(BuildTargetId.macOSArm64),
            outputDirectory = outputRoot
        });

        Assert.False(result.succeeded);
        Assert.Null(result.outputPath);
        Assert.Contains(result.diagnostics, static diagnostic =>
            diagnostic.severity == BuildDiagnosticSeverity.Error);
        Assert.False(Directory.Exists(Path.Combine(outputRoot, "Test Game.app")));
        if (Directory.Exists(outputRoot))
        {
            Assert.Empty(Directory.EnumerateDirectories(
                outputRoot,
                ".inno-build-*",
                SearchOption.TopDirectoryOnly));
        }
    }

    [Fact]
    public async Task CanceledGameBuildLeavesNoProductOrStagingDirectory()
    {
        SaveStartupScene();
        string outputRoot = Path.Combine(m_root, "Builds", "CanceledGame");
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            m_pipeline.BuildGameAsync(
                new GameBuildRequest
                {
                    profile = CreateProfile(BuildTargetId.macOSArm64),
                    outputDirectory = outputRoot
                },
                cancellationToken: cancellation.Token).AsTask());

        Assert.False(Directory.Exists(Path.Combine(outputRoot, "Test Game.app")));
        if (Directory.Exists(outputRoot))
        {
            Assert.Empty(Directory.EnumerateDirectories(
                outputRoot,
                ".inno-build-*",
                SearchOption.TopDirectoryOnly));
        }
    }

    [Fact]
    public async Task GameBuildRejectsManagedProjectOutputWithoutLeavingAProduct()
    {
        SaveStartupScene();
        string output = Path.Combine(m_projectRoot, "Assets", "Builds");

        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            m_pipeline.BuildGameAsync(new GameBuildRequest
            {
                profile = CreateProfile(BuildTargetId.macOSArm64),
                outputDirectory = output
            }).AsTask());

        Assert.Contains("managed project content", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.False(Directory.Exists(Path.Combine(output, "Test Game.app")));
    }

    [Theory]
    [InlineData("CON")]
    [InlineData("nul.txt")]
    [InlineData("Trailing.")]
    [InlineData("Trailing ")]
    [InlineData("Bad:Name")]
    public void BuildProfileRejectsNamesThatAreInvalidOnEitherTarget(string productName)
    {
        BuildProfile profile = CreateProfile(BuildTargetId.macOSArm64, productName);

        Assert.Throws<InvalidDataException>(profile.Validate);
    }

    [Fact]
    public void BuildProfileStoreRoundTripsGeneratedCurrentFormat()
    {
        string path = Path.Combine(m_projectRoot, "BuildProfile.inno");
        var store = new BuildProfileStore(path, m_engine.serialization);
        BuildProfile source = CreateProfile(BuildTargetId.windowsX64, "Stored Game");
        source.windowWidth = 1920;
        source.windowHeight = 1080;

        store.Save(source);
        BuildProfile restored = store.Load();

        Assert.True(store.exists);
        Assert.Equal(source.applicationId, restored.applicationId);
        Assert.Equal(source.productName, restored.productName);
        Assert.Equal(source.startupScene, restored.startupScene);
        Assert.Equal(BuildTargetId.windowsX64, restored.target);
        Assert.Equal(1920, restored.windowWidth);
        Assert.Equal(1080, restored.windowHeight);
    }

    [Fact]
    public void BuildProfileStorePreservesCommittedDocumentWhenCandidateIsInvalid()
    {
        string path = Path.Combine(m_projectRoot, "BuildProfile.inno");
        var store = new BuildProfileStore(path, m_engine.serialization);
        BuildProfile committed = CreateProfile(BuildTargetId.macOSArm64, "Committed Game");
        store.Save(committed);
        byte[] committedBytes = File.ReadAllBytes(path);
        BuildProfile invalid = CreateProfile(BuildTargetId.windowsX64, "Invalid/Game");

        Assert.Throws<InvalidDataException>(() => store.Save(invalid));

        Assert.Equal(committedBytes, File.ReadAllBytes(path));
        Assert.Empty(Directory.EnumerateFiles(m_projectRoot, "BuildProfile.inno.staging-*"));
    }

    [Fact]
    public void BuildProfileStoreRejectsCorruptCurrentFormatWithoutFallback()
    {
        string path = Path.Combine(m_projectRoot, "BuildProfile.inno");
        File.WriteAllBytes(path, [0x42, 0x41, 0x44]);
        var store = new BuildProfileStore(path, m_engine.serialization);

        InvalidDataException failure = Assert.Throws<InvalidDataException>(store.Load);

        Assert.Contains("current-format", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void SupportPackRejectsAuthoringAssetPipelineAssemblies()
    {
        string supportPack = GetSupportPackDirectory(BuildTargetId.macOSArm64);
        File.WriteAllBytes(Path.Combine(supportPack, "Inno.Assets.Pipeline.dll"), [0x49, 0x4E, 0x4E, 0x4F]);
        var catalog = new PlayerSupportPackCatalog(m_supportPackRoot);

        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => catalog.Resolve(BuildTargetId.macOSArm64, new Inno.Build.Platform.MacOS.MacOSSupportPackValidator()));

        Assert.Contains("forbidden build-time file", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UnknownManagedDeploymentNeverStartsSupportPackPreparation()
    {
        var provisioner = new TestSupportPackProvisioner(_ =>
            throw new InvalidOperationException("Unsupported deployments must not prepare a Pack."));
        BuildPipeline pipeline = CreatePipeline(provisioner);

        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            pipeline.EnsurePlayerSupportPackAsync(
                BuildTargetId.windowsX64, new ManagedDeploymentId("unregistered")).AsTask());

        Assert.Contains("unregistered", failure.Message, StringComparison.Ordinal);
        Assert.Equal(0, provisioner.callCount);
    }

    [Fact]
    public async Task UnsupportedPlatformDeploymentPairNeverStartsSupportPackPreparation()
    {
        var provisioner = new TestSupportPackProvisioner(_ =>
            throw new InvalidOperationException("Unsupported platforms must not prepare a Pack."));
        var deployments = new ManagedDeploymentCatalog(
            [new FixtureManagedDeploymentCompiler(["browser-wasm"])]);
        BuildPipeline pipeline = CreatePipeline(provisioner, deployments);

        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            pipeline.EnsurePlayerSupportPackAsync(
                BuildTargetId.windowsX64, ManagedDeploymentId.coreClr).AsTask());

        Assert.Contains("win-x64", failure.Message, StringComparison.Ordinal);
        Assert.Equal(0, provisioner.callCount);
    }

    [Fact]
    public async Task MissingSupportPackIsPreparedBeforeTheOwnerThreadExport()
    {
        SaveStartupScene();
        Directory.Delete(Path.Combine(m_supportPackRoot, BuildTargetId.macOSArm64.value), recursive: true);
        var provisioner = new TestSupportPackProvisioner(target =>
            CreateSupportPack(target, "Inno.Player"));

        BuildPipeline pipeline = CreatePipeline(provisioner);
        _ = await pipeline.EnsurePlayerSupportPackAsync(BuildTargetId.macOSArm64, deployment: null);
        BuildResult result = await pipeline.BuildGameAsync(new GameBuildRequest
        {
            profile = CreateProfile(BuildTargetId.macOSArm64),
            outputDirectory = Path.Combine(m_root, "Builds", "Provisioned")
        });

        Assert.True(result.succeeded);
        Assert.Equal(1, provisioner.callCount);
        Assert.True(Directory.Exists(Path.Combine(m_supportPackRoot, BuildTargetId.macOSArm64.value)));
    }

    [Fact]
    public async Task MissingPackFailsPromptlyWhileAsynchronousPreparationIsPending()
    {
        SaveStartupScene();
        Directory.Delete(Path.Combine(m_supportPackRoot, BuildTargetId.macOSArm64.value), recursive: true);
        using var cancellation = new CancellationTokenSource();
        var provisioner = new PendingSupportPackProvisioner();
        BuildPipeline pipeline = CreatePipeline(provisioner);
        Task<string> preparation = pipeline.EnsurePlayerSupportPackAsync(
            BuildTargetId.macOSArm64, deployment: null, cancellation.Token).AsTask();
        try
        {
            Assert.False(preparation.IsCompleted);
            Task<BuildResult> build = pipeline.BuildGameAsync(new GameBuildRequest
            {
                profile = CreateProfile(BuildTargetId.macOSArm64),
                outputDirectory = Path.Combine(m_root, "Builds", "Pending")
            }).AsTask();

            Assert.True(build.IsCompleted);
            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => build);
            m_engine.generations.EnsureReady("verify the rejected snapshot released its read lease");
            Assert.False(Directory.Exists(Path.Combine(m_root, "Builds", "Pending")));
        }
        finally
        {
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => preparation);
        }
    }

    [Fact]
    public void EditorShutdownDrainsSupportPackCleanupBeforeReleasingExportServices()
    {
        SaveStartupScene();
        Directory.Delete(Path.Combine(m_supportPackRoot, BuildTargetId.macOSArm64.value), recursive: true);
        var provisioner = new DrainingSupportPackProvisioner();
        BuildPipeline pipeline = CreatePipeline(provisioner);
        var defaults = BuildSettings.CreateDefault("Test Game", "Scenes/Startup.iscene", BuildTargetId.macOSArm64);
        var buildSettings = new BuildSettingsStore(
            Path.Combine(m_projectRoot, "Settings.Build.inno"), m_engine.serialization, defaults);
        m_engine.modules.Register("Tests.Exporting", [Assembly.Load(new AssemblyName("Inno.Editor.Exporting"))]);
        var context = new EditorContext(m_projectRoot);
        using var runtime = new EditorInteractionRuntime(
            context, m_engine.types, m_engine.logs, [pipeline, buildSettings, m_settings]);
        runtime.Start();
        Assert.True(runtime.interactions.For("editor/main-menu").Execute("export/game"));
        EditorModalExtension modal = Assert.Single(runtime.modals.Where(static item => item.id == "export.game"));
        EditorModalExtension progress = Assert.Single(runtime.modals.Where(static item => item.id == "export.game.progress"));
        ImGuiContextPtr imgui = NativeImGui.CreateContext();
        try
        {
            ImGuiIOPtr io = NativeImGui.GetIO();
            io.DisplaySize = new Vector2(1200f, 900f);
            io.DeltaTime = 1f / 60f;
            io.BackendFlags |= ImGuiBackendFlags.RendererHasTextures;
            io.Fonts.RendererHasTextures = true;
            Vector2 exportCenter = Vector2.Zero;
            for (int frame = 0; frame < 4; frame++)
            {
                if (frame == 2)
                {
                    io.AddMousePosEvent(exportCenter.X, exportCenter.Y);
                    io.AddMouseButtonEvent(0, true);
                }
                if (frame == 3)
                    io.AddMouseButtonEvent(0, false);
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(20f), ImGuiCond.Always);
                NativeImGui.SetNextWindowSize(new Vector2(1000f, 800f), ImGuiCond.Always);
                _ = NativeImGui.Begin("Export lifecycle verification", ImGuiWindowFlags.NoSavedSettings);
                Assert.True(modal.Draw(context));
                exportCenter = (NativeImGui.GetItemRectMin() + NativeImGui.GetItemRectMax()) * 0.5f;
                NativeImGui.End();
                NativeImGui.Render();
            }
            Assert.True(provisioner.started);
            Assert.True(progress.TryGetPresentation(out EditorModalExtension.Presentation presentation));
            Assert.True(presentation.isVisible);
            _ = runtime.interactions.For("editor/main-menu").Execute("export/plugin");
            EditorModalExtension plugin = Assert.Single(runtime.modals.Where(static item => item.id == "export.plugin"));
            Assert.True(plugin.TryGetPresentation(out EditorModalExtension.Presentation pluginPresentation));
            Assert.False(pluginPresentation.isVisible);

            Assert.True(runtime.interactions.TryGetModule<EditorModule>(out EditorModule? module));
            Assert.NotNull(module);
            Assert.Equal("Inno.Editor.Exporting", module.GetType().Assembly.GetName().Name);
            Assert.Throws<RetirementPendingException>(() => module.Stop(context));
            Assert.True(provisioner.canceled.Task.IsCompletedSuccessfully);
            Assert.False(provisioner.completed);
            provisioner.cleanup.SetResult();
            runtime.Dispose();
            Assert.True(provisioner.completed);
            m_engine.generations.EnsureReady("verify export retirement drained its services");
        }
        finally
        {
            provisioner.cleanup.TrySetResult();
            NativeImGui.DestroyContext(imgui);
        }
    }

    [Fact]
    public async Task InvalidInstalledSupportPackDoesNotTriggerProvisioning()
    {
        SaveStartupScene();
        File.Delete(Path.Combine(GetSupportPackDirectory(BuildTargetId.macOSArm64),
            "native", "libminiaudio-release.dylib"));
        var provisioner = new TestSupportPackProvisioner(target =>
            CreateSupportPack(target, "Inno.Player"));

        await Assert.ThrowsAsync<InvalidDataException>(() =>
            CreatePipeline(provisioner).BuildGameAsync(new GameBuildRequest
            {
                profile = CreateProfile(BuildTargetId.macOSArm64),
                outputDirectory = Path.Combine(m_root, "Builds", "InvalidPack")
            }).AsTask());

        Assert.Equal(0, provisioner.callCount);
    }

    [Fact]
    public void SupportPackRejectsMissingMiniAudioRuntime()
    {
        string supportPack = GetSupportPackDirectory(BuildTargetId.macOSArm64);
        File.Delete(Path.Combine(supportPack, "native", "libminiaudio-release.dylib"));
        var catalog = new PlayerSupportPackCatalog(m_supportPackRoot);

        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => catalog.Resolve(BuildTargetId.macOSArm64, new Inno.Build.Platform.MacOS.MacOSSupportPackValidator()));

        Assert.Contains("libminiaudio-release.dylib", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void SupportPackRejectsForeignNativeRuntime()
    {
        string supportPack = GetSupportPackDirectory(BuildTargetId.macOSArm64);
        File.WriteAllBytes(Path.Combine(supportPack, "native", "SDL3-release.so"), [0x49, 0x4E, 0x4E, 0x4F]);
        var catalog = new PlayerSupportPackCatalog(m_supportPackRoot);

        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => catalog.Resolve(BuildTargetId.macOSArm64, new Inno.Build.Platform.MacOS.MacOSSupportPackValidator()));

        Assert.Contains("foreign native runtime", exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("libinno-text-release.dylib")]
    [InlineData("libinno-ui-release.dylib")]
    public void SupportPackRejectsMissingTextOrUiRuntime(string nativeRuntime)
    {
        string supportPack = GetSupportPackDirectory(BuildTargetId.macOSArm64);
        File.Delete(Path.Combine(supportPack, "native", nativeRuntime));
        var catalog = new PlayerSupportPackCatalog(m_supportPackRoot);

        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => catalog.Resolve(BuildTargetId.macOSArm64, new Inno.Build.Platform.MacOS.MacOSSupportPackValidator()));

        Assert.Contains(nativeRuntime, exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task PluginBuildIsDeterministicSourceOnlyAndUsesTheInstallContract()
    {
        m_assets.CreateDirectory(AssetPath.Project("Content"));
        Assert.True(m_assets.Save(
            AssetPath.Project("Content/value.txt"),
            new TextAsset("deterministic")));
        string firstPath = Path.Combine(m_root, "first.iplugin");
        string secondPath = Path.Combine(m_root, "second.iplugin");

        ValueTask<BuildResult> firstBuild = m_pipeline.BuildPluginAsync(new PluginBuildRequest
        {
            pluginId = "tests.export",
            displayName = "Export Test",
            outputPath = firstPath
        });
        ValueTask<BuildResult> secondBuild = m_pipeline.BuildPluginAsync(new PluginBuildRequest
        {
            pluginId = "tests.export",
            displayName = "Export Test",
            outputPath = secondPath
        });
        BuildResult[] results = await Task.WhenAll(firstBuild.AsTask(), secondBuild.AsTask());
        BuildResult first = results[0];
        BuildResult second = results[1];

        Assert.True(first.succeeded);
        Assert.True(second.succeeded);
        Assert.Equal(first.contentHash, second.contentHash);
        Assert.Equal(File.ReadAllBytes(firstPath), File.ReadAllBytes(secondPath));
        using (ZipArchive archive = ZipFile.OpenRead(firstPath))
        {
            Assert.Contains(archive.Entries, static entry => entry.FullName == "Plugin.inno");
            Assert.Contains(archive.Entries, static entry => entry.FullName == "Assets/Content/value.txt");
            Assert.All(archive.Entries, static entry => Assert.Equal(1980, entry.LastWriteTime.Year));
            Assert.DoesNotContain(archive.Entries, static entry =>
                string.Equals(Path.GetExtension(entry.FullName), ".dll", StringComparison.OrdinalIgnoreCase));
        }

        string installRoot = Path.Combine(m_root, "Install");
        Directory.CreateDirectory(installRoot);
        File.Copy(firstPath, Path.Combine(installRoot, "export.iplugin"));
        PluginScanResult scan = new PluginSourceService(
            m_engine.serialization,
            installRoot,
            Path.Combine(m_root, "InstallLibrary")).Scan();
        Assert.Empty(scan.diagnostics);
        PluginCandidate candidate = Assert.Single(scan.candidates);
        Assert.Equal("tests.export", candidate.manifest.pluginId);
        Assert.Empty(candidate.manifest.dependencies);
        Assert.Equal(PluginSourceKind.Package, candidate.sourceKind);
    }

    [Fact]
    public async Task CanceledPluginBuildLeavesNoPackageOrStagingFile()
    {
        Assert.True(m_assets.Save(AssetPath.Project("value.txt"), new TextAsset("content")));
        string output = Path.Combine(m_root, "Canceled.iplugin");
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            m_pipeline.BuildPluginAsync(
                new PluginBuildRequest
                {
                    pluginId = "tests.canceled",
                    displayName = "Canceled",
                    outputPath = output
                },
                cancellationToken: cancellation.Token).AsTask());

        Assert.False(File.Exists(output));
        Assert.Empty(Directory.EnumerateFiles(m_root, "Canceled.iplugin.staging-*", SearchOption.TopDirectoryOnly));
    }

    private static BuildProfile CreateProfile(BuildTargetId target, string productName = "Test Game")
        => new()
        {
            applicationId = "tests.game",
            productName = productName,
            startupScene = "Scenes/Startup.iscene",
            target = target
        };

    private void SaveStartupScene()
    {
        var scene = new GameScene("Startup");
        scene.CreateObject("Player");
        Assert.True(m_assets.Save(
            AssetPath.Project("Scenes/Startup.iscene"),
            SceneAsset.Capture(scene, m_engine.serialization, m_assets)));
    }

    private void CreateSupportPack(BuildTargetId target, string executable)
    {
        string directory = Path.Combine(m_root, "pack-candidate-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        string references = Path.Combine(directory, "References");
        Directory.CreateDirectory(references);
        foreach (string assembly in Directory
                     .EnumerateFiles(AppContext.BaseDirectory, "Inno.*.dll", SearchOption.TopDirectoryOnly)
                     .Where(static path => !IsAuthoringAssembly(Path.GetFileName(path))))
        {
            File.Copy(assembly, Path.Combine(references, Path.GetFileName(assembly)));
        }
        File.WriteAllBytes(Path.Combine(directory, executable), [0x49, 0x4E, 0x4E, 0x4F]);
        string link = Path.Combine(directory, "PlayerLink");
        Directory.CreateDirectory(link);
        File.WriteAllText(Path.Combine(link, "Player.csproj"), "<Project><PropertyGroup><AssemblyName>Inno.Player</AssemblyName></PropertyGroup></Project>");
        foreach (string source in new[] { "Program.cs", "DesktopPlayerComposition.cs", "FilePlayerContentSource.cs", "global.json" })
            File.WriteAllText(Path.Combine(link, source), "fixture input");
        string analyzers = Path.Combine(link, "Analyzers");
        Directory.CreateDirectory(analyzers);
        File.WriteAllText(Path.Combine(analyzers, "Inno.Runtime.Generators.dll"), "fixture analyzer");
        File.WriteAllText(Path.Combine(analyzers, "Inno.Core.Serialization.Generators.dll"), "fixture analyzer");
        string linkReferences = Path.Combine(link, "References");
        Directory.CreateDirectory(linkReferences);
        foreach (string source in Directory.EnumerateFiles(references, "*.dll"))
            File.Copy(source, Path.Combine(linkReferences, Path.GetFileName(source)));
        File.WriteAllText(Path.Combine(linkReferences, "Inno.Player.Runtime.dll"), "fixture runtime");
        File.WriteAllText(Path.Combine(linkReferences, "BGCS.Runtime.dll"), "fixture interop");
        File.WriteAllBytes(Path.Combine(link, executable), [0x49, 0x4E, 0x4E, 0x4F]);
        string native = Path.Combine(directory, "native");
        Directory.CreateDirectory(native);
        string[] required = target == BuildTargetId.macOSArm64
            ? ["libbgfx-shared-lib-release.dylib", "SDL3-release.dylib", "libminiaudio-release.dylib",
                "libinno-text-release.dylib", "libinno-ui-release.dylib"]
            : ["bgfx-shared-lib-release.dll", "SDL3-release.dll", "miniaudio-release.dll",
                "inno-text-release.dll", "inno-ui-release.dll"];
        string linkNative = Path.Combine(link, "native");
        Directory.CreateDirectory(linkNative);
        foreach (string file in required)
        {
            File.WriteAllBytes(Path.Combine(native, file), [0x49, 0x4E, 0x4E, 0x4F]);
            File.WriteAllBytes(Path.Combine(linkNative, file), [0x49, 0x4E, 0x4E, 0x4F]);
        }
        IPlayerSupportPackValidator validator = target == BuildTargetId.macOSArm64
            ? new Inno.Build.Platform.MacOS.MacOSSupportPackValidator()
            : new Inno.Build.Platform.Windows.WindowsSupportPackValidator();
        _ = new PlayerSupportPackCatalog(m_supportPackRoot).PublishAsync(
            target, directory, validator).AsTask().GetAwaiter().GetResult();
    }

    private string GetSupportPackDirectory(BuildTargetId target)
    {
        string targetRoot = Path.Combine(m_supportPackRoot, target.value);
        return Path.Combine(targetRoot, File.ReadAllText(Path.Combine(targetRoot, "current")));
    }

    private BuildPipeline CreatePipeline(
        IPlayerSupportPackProvisioner? provisioner = null,
        ManagedDeploymentCatalog? managedDeployments = null
    )
        => new(
            m_assets,
            m_plugins,
            m_settings,
            m_engine.serialization,
            m_engine.generations,
            m_compiler,
            m_supportPackRoot,
            [
                new MacOSArm64GameBuildTarget(m_assets, m_engine.serialization, m_engine.types),
                new WindowsX64GameBuildTarget(m_assets, m_engine.serialization, m_engine.types)
            ],
            managedDeployments ?? CreateManagedDeployments(),
            provisioner);

    private static ManagedDeploymentCatalog CreateManagedDeployments()
        => new([new FixtureManagedDeploymentCompiler()]);

    private sealed class FixtureManagedDeploymentCompiler : IManagedDeploymentCompiler
    {
        internal FixtureManagedDeploymentCompiler(IReadOnlyList<string>? runtimeIdentifiers = null)
        {
            capabilities = new ManagedDeploymentCapabilities(
                runtimeIdentifiers ?? ["win-x64", "osx-arm64"],
                dynamicCode: true, aheadOfTime: false, nativeStaticLinking: false);
        }

        public ManagedDeploymentId id => ManagedDeploymentId.coreClr;

        public ManagedDeploymentCapabilities capabilities { get; }

        public ValueTask<ManagedDeploymentResult> CompileAsync(
            ManagedDeploymentRequest request,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            string projectRoot = Path.GetDirectoryName(request.projectPath)!;
            Assert.True(File.Exists(Path.Combine(projectRoot, "PlayerDeploymentDefinition.g.cs")));
            Assert.NotEmpty(Directory.EnumerateFiles(request.codeInputDirectory, "*.dll"));
            Directory.CreateDirectory(request.outputDirectory);
            string executable = request.runtimeIdentifier == "win-x64" ? "Inno.Player.exe" : "Inno.Player";
            File.Copy(Path.Combine(projectRoot, executable), Path.Combine(request.outputDirectory, executable));
            foreach (string native in Directory.EnumerateFiles(Path.Combine(projectRoot, "native")))
            {
                string destination = Path.Combine(request.outputDirectory, "native", Path.GetFileName(native));
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                File.Copy(native, destination);
            }
            return ValueTask.FromResult(new ManagedDeploymentResult(id, request.outputDirectory, "fixture-sdk",
                Directory.EnumerateFiles(request.outputDirectory, "*", SearchOption.AllDirectories)
                    .Select(file => Path.GetRelativePath(request.outputDirectory, file)).ToArray()));
        }
    }

    private sealed class PendingSupportPackProvisioner : IPlayerSupportPackProvisioner
    {
        private readonly TaskCompletionSource m_completion = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public ValueTask ProvisionAsync(
            BuildTargetId target,
            string supportPackRoot,
            CancellationToken cancellationToken = default
        ) => new(m_completion.Task.WaitAsync(cancellationToken));
    }

    private sealed class DrainingSupportPackProvisioner : IPlayerSupportPackProvisioner
    {
        internal TaskCompletionSource canceled { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        internal TaskCompletionSource cleanup { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        internal bool started { get; private set; }
        internal bool completed { get; private set; }

        public async ValueTask ProvisionAsync(
            BuildTargetId target,
            string supportPackRoot,
            CancellationToken cancellationToken = default
        ) {
            started = true;
            using CancellationTokenRegistration registration = cancellationToken.Register(() => canceled.TrySetResult());
            await cleanup.Task.ConfigureAwait(false);
            completed = true;
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private sealed class TestSupportPackProvisioner(Action<BuildTargetId> provision)
        : IPlayerSupportPackProvisioner
    {
        internal int callCount { get; private set; }

        public ValueTask ProvisionAsync(
            BuildTargetId target,
            string supportPackRoot,
            CancellationToken cancellationToken = default)
        {
            _ = supportPackRoot;
            cancellationToken.ThrowIfCancellationRequested();
            callCount++;
            provision(target);
            return ValueTask.CompletedTask;
        }
    }

    private static bool IsAuthoringAssembly(string name)
        => name.Contains("Inno.Editor", StringComparison.OrdinalIgnoreCase)
           || name.Contains("Inno.Build", StringComparison.OrdinalIgnoreCase)
           || name.Contains("Inno.Scripting.Compiler", StringComparison.OrdinalIgnoreCase)
           || name.Contains("Inno.Assets.Pipeline", StringComparison.OrdinalIgnoreCase)
           || name.Contains("Inno.Plugins.Authoring", StringComparison.OrdinalIgnoreCase);

    private sealed class BlockingBuildTarget : IGameBuildTarget
    {
        internal TaskCompletionSource started { get; } = new(
            TaskCreationOptions.RunContinuationsAsynchronously);
        internal TaskCompletionSource release { get; } = new(
            TaskCreationOptions.RunContinuationsAsynchronously);

        public void Validate(string directory) => new Inno.Build.Platform.MacOS.MacOSSupportPackValidator().Validate(directory);

        public BuildTargetId id => BuildTargetId.macOSArm64;

        public ManagedDeploymentId defaultManagedDeployment => ManagedDeploymentId.coreClr;

        public string runtimeIdentifier => "osx-arm64";

        public string displayName => "Blocking test target";

        public bool isPreferredOnCurrentHost => true;

        public async ValueTask BuildContentAsync(
            GameBuildContentContext context,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(context);
            started.SetResult();
            await release.Task.WaitAsync(cancellationToken);
        }

        public ValueTask<string> PackageAsync(
            GameBuildPackageContext context,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(context);
            cancellationToken.ThrowIfCancellationRequested();
            throw new InvalidOperationException("A changed generation must fail before packaging.");
        }
    }
}

[CollectionDefinition("Build pipeline serialization", DisableParallelization = true)]
public sealed class BuildPipelineSerializationCollection
{
}
