using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter;
using Inno.Adapter.Audio;
using Inno.Adapter.Input;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Platform;
using Inno.Adapter.Rendering;
using Inno.Adapter.Serialization.DotNet;
using Inno.Adapter.Storage;
using Inno.Adapter.Text;
using Inno.Adapter.UI;
using Inno.Content;
using Inno.Content.Testing;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Player.Runtime;
using Inno.Shell;
using Inno.Storage;
using Xunit;

namespace Inno.Runtime.Tests;

public sealed class PlayerContentSourceTests
{
    [Theory]
    [InlineData("missing")]
    [InlineData("mismatch")]
    [InlineData("canceled")]
    public async Task PreparedContentIsCheckedAndRetiredBeforeAnyPlatformAdapterStarts(string failure)
    {
        var manifest = new GameRuntimeManifest
        {
            applicationId = "tests.memory",
            productName = "Memory Player",
            startupScene = "Startup.iscene",
            modules = [new GameRuntimeModule
            {
                name = "Game",
                domain = AssemblyDomain.InnoScripting,
                assemblies = [new GameRuntimeAssembly
                {
                    name = typeof(PlayerContentSourceTests).Assembly.GetName().Name!,
                    contentFingerprint = new string('a', 64)
                }]
            }]
        };
        PlayerContentMetadata metadata;
        using (EngineHost metadataOwner = new EngineHostBuilder()
            .UseMetadataSources(new DotNetAssemblyCatalogSource(typeof(PlayerContentSourceTests).Assembly),
                new ReflectionTypeCatalogSource(), new ReflectionSerializationMetadataSource()).Build())
        using (SerializationGeneration serialization = metadataOwner.serialization.CaptureGeneration())
        {
            string hash = new string('A', 64);
            metadata = new PlayerContentMetadata(RuntimeManifestEnvelope.Encode(manifest, serialization),
                serialization.Serialize(new RuntimeContentCatalog
                {
                    contentHash = hash, packFileName = $"content-{hash}.pack",
                    snapshotFingerprint = hash, runtimeAssemblyCount = 1
                }));
        }
        using var cancellation = new CancellationTokenSource();
        var source = new PreparedSource(metadata, failure, cancellation);
        var modules = new DotNetAssemblyCatalogSource(typeof(PlayerContentSourceTests).Assembly);
        var activator = new StaticPlayerModuleActivator(GameCodeDeployment.FromManifest(manifest.modules),
            new Dictionary<string, IReadOnlyList<Assembly>>
            {
                ["Game"] = [typeof(PlayerContentSourceTests).Assembly]
            });
        PlayerLaunchOptions options = CreateOptions(modules, source, activator);
        if (failure == "missing")
            await Assert.ThrowsAsync<InvalidOperationException>(() => PlayerApplication.RunAsync(options, cancellation.Token));
        else if (failure == "mismatch")
            await Assert.ThrowsAsync<InvalidDataException>(() => PlayerApplication.RunAsync(options, cancellation.Token));
        else
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => PlayerApplication.RunAsync(options, cancellation.Token));
        Assert.Equal(1, source.preparations);
        Assert.Equal(failure == "missing" ? 0 : 1, source.store?.disposals ?? 0);
        Assert.Throws<ObjectDisposedException>(() => modules.GetAssemblies());
    }

    [Fact]
    public void MetadataCopiesBothDocumentsBeforeTheBorrowedSourceCanReuseItsBuffers()
    {
        byte[] manifest = [1, 2, 3];
        byte[] catalog = [4, 5, 6];
        var metadata = new PlayerContentMetadata(manifest, catalog);
        Array.Clear(manifest);
        Array.Clear(catalog);
        Assert.Equal(new byte[] { 1, 2, 3 }, metadata.manifest.ToArray());
        Assert.Equal(new byte[] { 4, 5, 6 }, metadata.catalog.ToArray());
        Assert.Throws<ArgumentException>(() => new PlayerContentMetadata([], [1]));
        Assert.Throws<ArgumentException>(() => new PlayerContentMetadata([1], []));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task MetadataFailureRetiresTheTransferredModuleOwnerBeforeAnyAdapterOrContentPreparation(
        bool malformed
    ) {
        var source = new MemorySource { malformed = malformed };
        var modules = new DotNetAssemblyCatalogSource(typeof(PlayerContentSourceTests).Assembly);
        PlayerLaunchOptions options = CreateOptions(modules, source);
        if (malformed)
            await Assert.ThrowsAsync<InvalidDataException>(() => PlayerApplication.RunAsync(options));
        else
            await Assert.ThrowsAsync<IOException>(() => PlayerApplication.RunAsync(options));
        Assert.Equal(1, source.metadataReads);
        Assert.Equal(0, source.preparations);
        Assert.Throws<ObjectDisposedException>(() => modules.GetAssemblies());
    }

    [Fact]
    public async Task CancellationBeforeStartupDoesNotAcquireBorrowedContentOrTransferModuleOwnership()
    {
        var source = new MemorySource();
        using var modules = new DotNetAssemblyCatalogSource(typeof(PlayerContentSourceTests).Assembly);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            PlayerApplication.RunAsync(CreateOptions(modules, source), cancellation.Token));
        Assert.Equal(0, source.metadataReads);
        Assert.Equal(0, source.preparations);
        Assert.NotEmpty(modules.GetAssemblies());
    }

    private static PlayerLaunchOptions CreateOptions(
        IAssemblyCatalogSource modules,
        IPlayerContentSource source,
        IPlayerModuleActivator? activator = null
    ) => new()
    {
        modules = modules,
        types = new ReflectionTypeCatalogSource(),
        serializationMetadata = new ReflectionSerializationMetadataSource(),
        adapters = new RejectedAdapterCatalog(),
        contentSource = source,
        createStorage = _ => throw new InvalidOperationException("Storage must not start before content validation."),
        moduleActivator = activator ?? new StaticPlayerModuleActivator(new GameCodeDeployment([]),
            new Dictionary<string, IReadOnlyList<Assembly>>()),
        frameDriver = new PollingShellFrameDriver(),
        logDeliveryMode = LogDeliveryMode.Inline
    };

    private sealed class PreparedSource(
        PlayerContentMetadata metadata,
        string failure,
        CancellationTokenSource cancellation
    ) : IPlayerContentSource
    {
        internal int preparations;
        internal PreparedStore? store;

        public ValueTask<PlayerContentMetadata> ReadMetadataAsync(CancellationToken cancellationToken = default)
            => ValueTask.FromResult(metadata);

        public ValueTask<IRuntimeContentStore> PrepareAsync(
            ContentPackDescriptor pack,
            StorageScope scope,
            SerializationGeneration serialization,
            CancellationToken cancellationToken = default
        ) {
            preparations++;
            Assert.Equal("tests.memory", scope.value);
            if (failure == "missing")
                return ValueTask.FromResult<IRuntimeContentStore>(null!);
            store = new PreparedStore(failure == "mismatch" ? null : pack);
            if (failure == "canceled")
                cancellation.Cancel();
            return ValueTask.FromResult<IRuntimeContentStore>(store);
        }
    }

    private sealed class PreparedStore(ContentPackDescriptor? expected) : IRuntimeContentStore
    {
        private readonly ContentTestStore m_content = new([]);
        internal int disposals;

        public ContentPackDescriptor descriptor => expected ?? m_content.descriptor;
        public ContentPackIndex index => m_content.index;
        public ContentReadLease Acquire(ContentKey key) => m_content.Acquire(key);
        public void Dispose()
        {
            disposals++;
            m_content.Dispose();
        }
    }

    private sealed class MemorySource : IPlayerContentSource
    {
        internal bool malformed;
        internal int metadataReads;
        internal int preparations;

        public ValueTask<PlayerContentMetadata> ReadMetadataAsync(CancellationToken cancellationToken = default)
        {
            metadataReads++;
            return malformed
                ? ValueTask.FromResult(new PlayerContentMetadata([1], [2]))
                : ValueTask.FromException<PlayerContentMetadata>(new IOException("Metadata unavailable."));
        }

        public ValueTask<IRuntimeContentStore> PrepareAsync(
            ContentPackDescriptor pack,
            StorageScope scope,
            SerializationGeneration serialization,
            CancellationToken cancellationToken = default
        ) {
            preparations++;
            throw new InvalidOperationException("Invalid metadata cannot reach content preparation.");
        }
    }

    private sealed class RejectedAdapterCatalog : IAdapterCatalog
    {
        public IPlatformBackendFactory platform => throw Rejected();
        public IInputBackendFactory input => throw Rejected();
        public IStorageBackendFactory storage => throw Rejected();
        public IRenderingBackendFactory rendering => throw Rejected();
        public IAudioBackendFactory audio => throw Rejected();
        public ITextBackendFactory text => throw Rejected();
        public IUiBackendFactory ui => throw Rejected();

        private static InvalidOperationException Rejected()
            => new("An adapter cannot be selected before deployment metadata is validated.");
    }
}
