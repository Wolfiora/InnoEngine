using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Content;
using Inno.Content.Testing;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.IO;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Modules;
using Inno.Player.Runtime;
using Inno.Scene;
using Inno.Shell;
using Inno.Storage;
using Xunit;

namespace Inno.Runtime.Tests;

public sealed partial class PlayerWithoutFileSystemTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CompletePlayerStartsFromOwnedMemoryAndRetiresAfterSuccessfulOrFailingFrames(bool failFrame)
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoMemoryPlayer", Guid.NewGuid().ToString("N"));
        var adapters = new MemoryAdapters(failFrame);
        MemoryPlayerSource source = CreateDeployment(root);
        Assert.False(Directory.Exists(root));
        var modules = new DotNetAssemblyCatalogSource(typeof(PlayerWithoutFileSystemTests).Assembly);
        var deployment = GameCodeDeployment.FromManifest(source.manifest.modules);
        var options = new PlayerLaunchOptions
        {
            modules = modules,
            types = new ReflectionTypeCatalogSource(),
            serializationMetadata = new ReflectionSerializationMetadataSource(),
            contentSource = source,
            adapters = adapters,
            createStorage = _ => new MemoryStorage(),
            moduleActivator = new StaticPlayerModuleActivator(deployment,
                new Dictionary<string, IReadOnlyList<Assembly>>
                {
                    ["Game"] = [typeof(PlayerWithoutFileSystemTests).Assembly]
                }),
            frameDriver = new PollingShellFrameDriver(),
            smokeFrameLimit = 3,
            windowVisible = false,
            jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
            logDeliveryMode = LogDeliveryMode.Inline
        };
        if (failFrame)
            Assert.Throws<InvalidOperationException>(() =>
                OwnerThreadExecution.Run(() => PlayerApplication.RunAsync(options)));
        else
            Assert.Equal(0, OwnerThreadExecution.Run(() => PlayerApplication.RunAsync(options)));
        Assert.Equal(1, source.metadataReads);
        Assert.Equal(1, source.preparations);
        Assert.Equal(1, source.disposals);
        Assert.Equal(failFrame ? 0 : 3, adapters.completedFrames);
        Assert.Equal(new[] { "ui", "text", "render", "input", "window", "platform" }, adapters.retired);
        Assert.Throws<ObjectDisposedException>(() => modules.GetAssemblies());
        Assert.False(Directory.Exists(root));
    }

    private static MemoryPlayerSource CreateDeployment(string root)
    {
        try
        {
            string assetsDirectory = Path.Combine(root, "Assets");
            Directory.CreateDirectory(assetsDirectory);
            using EngineHost engine = CreateHost();
            var identities = new IdentityAllocator();
            var world = new SceneWorld(identities, engine.types);
            using IDisposable sceneScope = world.EnterScope();
            using var assets = new AssetPipeline(engine.modules, engine.types, engine.serialization,
                identities, engine.diagnostics, engine.logs,
                AssetPipelineOptions.Create(assetsDirectory, Path.Combine(root, "Library")) with
                {
                    enableFileSystemWatcher = false
                });
            Assert.True(assets.Save(AssetPath.Project("Startup.iscene"),
                SceneAsset.Capture(new GameScene("Memory scene"), engine.serialization, assets)));
            using var settings = new ProjectSettingsStore(
                new FileByteDocumentStore(Path.Combine(root, SettingsFileNames.project)),
                engine.types, engine.serialization, new ProjectId("tests.memory.player"),
                AssetSerializationContext.Create(assets));
            string exported = Path.Combine(root, "Exported");
            assets.ExportRuntimeArtifacts(exported);
            File.WriteAllBytes(Path.Combine(exported, SettingsFileNames.project), settings.CaptureDocument());
            var content = ContentTestStore.FromDirectory(exported);
            var manifest = new GameRuntimeManifest
            {
                applicationId = "tests.memory.player",
                productName = "Memory Player",
                startupScene = "Startup.iscene",
                modules = [new GameRuntimeModule
                {
                    name = "Game",
                    domain = AssemblyDomain.InnoScripting,
                    assemblies = [new GameRuntimeAssembly
                    {
                        name = typeof(PlayerWithoutFileSystemTests).Assembly.GetName().Name!,
                        contentFingerprint = new string('a', 64)
                    }]
                }]
            };
            using SerializationGeneration serialization = engine.serialization.CaptureGeneration();
            return new MemoryPlayerSource(manifest, content,
                new PlayerContentMetadata(RuntimeManifestEnvelope.Encode(manifest, serialization),
                    serialization.Serialize(new RuntimeContentCatalog
                    {
                        contentHash = content.descriptor.contentHash,
                        packFileName = content.descriptor.fileName,
                        snapshotFingerprint = new string('A', 64),
                        runtimeAssemblyCount = 1
                    })));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private sealed class MemoryPlayerSource(
        GameRuntimeManifest game,
        ContentTestStore content,
        PlayerContentMetadata metadata
    ) : IPlayerContentSource, IRuntimeContentStore
    {
        internal readonly GameRuntimeManifest manifest = game;
        internal int metadataReads;
        internal int preparations;
        internal int disposals;
        public ContentPackDescriptor descriptor => content.descriptor;
        public ContentPackIndex index => content.index;

        public async ValueTask<PlayerContentMetadata> ReadMetadataAsync(CancellationToken cancellationToken = default)
        {
            metadataReads++;
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
            return metadata;
        }

        public async ValueTask<IRuntimeContentStore> PrepareAsync(
            ContentPackDescriptor pack,
            StorageScope scope,
            SerializationGeneration serialization,
            CancellationToken cancellationToken = default
        ) {
            preparations++;
            Assert.Equal(descriptor, pack);
            Assert.Equal(manifest.applicationId, scope.value);
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
            return this;
        }

        public ContentReadLease Acquire(ContentKey key) => content.Acquire(key);

        public void Dispose()
        {
            disposals++;
            content.Dispose();
        }
    }
}
