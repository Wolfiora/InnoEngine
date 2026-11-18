using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Inno.Adapter.Content.FileSystem;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Build.Managed;
using Inno.Content;
using Inno.Core.IO;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build.MacOSArm64;
using Inno.Build.WindowsX64;
using Inno.Core.Identity;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Types;




using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scripting.Compiler;

using Xunit;
using Inno.Build;
using Inno.Testing.Build;

namespace Inno.Build.Products.Tests;

public sealed class GameBuildWorkflowTests : BuildPipelineFixture
{

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
        string packagedContent = target == new BuildTargetId("macos-arm64")
            ? Path.Combine(outputPath, "Contents", "Resources", "Content")
            : Path.Combine(outputPath, "Content");
        string executable = target == new BuildTargetId("macos-arm64")
            ? Path.Combine(outputPath, "Contents", "MacOS", "Test Game")
            : Path.Combine(outputPath, "Test Game.exe");
        Assert.True(File.Exists(executable));
        string playerRoot = target == new BuildTargetId("macos-arm64")
            ? Path.Combine(outputPath, "Contents", "MacOS")
            : outputPath;
        Assert.False(Directory.Exists(Path.Combine(playerRoot, "References")));
        string deployedNative = target == new BuildTargetId("macos-arm64")
            ? Path.Combine(outputPath, "Contents", "MacOS", "native")
            : Path.Combine(outputPath, "native");
        Assert.True(File.Exists(Path.Combine(deployedNative, target == new BuildTargetId("macos-arm64")
            ? "libinno-text-release.dylib"
            : "inno-text-release.dll")));
        Assert.True(File.Exists(Path.Combine(deployedNative, target == new BuildTargetId("macos-arm64")
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
            profile = CreateProfile(new BuildTargetId("macos-arm64")),
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
            [new GameBuildTargetBinding(target, target)],
            CreateManagedDeployments());
        string outputRoot = Path.Combine(m_root, "Builds", "MixedGeneration");

        Task<BuildResult> build = pipeline.BuildGameAsync(new GameBuildRequest
        {
            profile = CreateProfile(new BuildTargetId("macos-arm64")),
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
            profile = CreateProfile(new BuildTargetId("macos-arm64")),
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
    [SupportedOSPlatform("windows")]
    public async Task CommittedGameReportsBackupCleanupAsAWarningWithoutLosingSuccess()
    {
        if (!OperatingSystem.IsWindows())
            return;
        SaveStartupScene();
        var request = new GameBuildRequest
        {
            profile = CreateProfile(new BuildTargetId("windows-x64")),
            outputDirectory = Path.Combine(m_root, "Builds", "CleanupFailure")
        };
        string previous = Path.Combine(request.outputDirectory, request.profile.productName + "-Windows-x64");
        Directory.CreateDirectory(previous);
        string locked = Path.Combine(previous, "retained-readonly.txt");
        File.WriteAllText(locked, "previous output");
        File.SetAttributes(locked, FileAttributes.ReadOnly);
        try
        {
            BuildResult current = await m_pipeline.BuildGameAsync(request);
            Assert.True(current.succeeded);
            Assert.Equal(previous, current.outputPath);
            Assert.True(File.Exists(Path.Combine(current.outputPath!, "Test Game.exe")));
            Assert.False(File.Exists(locked));
            Assert.Contains(current.diagnostics, static diagnostic => diagnostic.code == "INNO-BUILD-CLEANUP"
                && diagnostic.severity == BuildDiagnosticSeverity.Warning);
        }
        finally
        {
            foreach (string retained in Directory.EnumerateFiles(request.outputDirectory, "retained-readonly.txt", SearchOption.AllDirectories))
                File.SetAttributes(retained, FileAttributes.Normal);
        }
    }

    [Fact]
    public async Task CommittedGameRemainsSuccessfulWhenItsFinalProgressObserverThrows()
    {
        SaveStartupScene();
        BuildResult result = await m_pipeline.BuildGameAsync(new GameBuildRequest
        {
            profile = CreateProfile(new BuildTargetId("windows-x64")),
            outputDirectory = Path.Combine(m_root, "Builds", "ProgressFailure")
        }, new CommitFailingProgress());
        Assert.True(result.succeeded);
        Assert.True(File.Exists(Path.Combine(result.outputPath!, "Test Game.exe")));
        Assert.Contains(result.diagnostics, static diagnostic => diagnostic.code == "INNO-BUILD-PROGRESS"
            && diagnostic.severity == BuildDiagnosticSeverity.Warning);
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
                    profile = CreateProfile(new BuildTargetId("macos-arm64")),
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
                profile = CreateProfile(new BuildTargetId("macos-arm64")),
                outputDirectory = output
            }).AsTask());

        Assert.Contains("managed project content", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.False(Directory.Exists(Path.Combine(output, "Test Game.app")));
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
}
