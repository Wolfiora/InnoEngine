using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Serialization.DotNet;
using Inno.Content;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.References;
using Inno.Testing.Content;

namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetRuntimeLoadingTests : AssetLoaderFixture
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FailedRuntimeDependencyRecoversAfterPublicationOrLoaderRestart(bool reopen)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("parent.depgraph", "child.deferredasset");
        workspace.WriteText("child.deferredasset", "unchanged");
        DeferredAssetImporter.isAvailable = false;
        m_types.Rebuild();
        AssetLoader? loader = null;
        try
        {
            loader = new AssetLoader(
                m_types,
                m_serialization,
                m_identities,
                m_diagnostics,
                m_logs,
                workspace.assetRoot,
                workspace.libraryRoot
            );
            loader.Rescan();
            AssetPath parent = AssetPath.Project("parent.depgraph");
            Assert.True(loader.TryGetInfo(parent, out AssetInfo? failed));
            Assert.Equal(AssetImportStatus.Failed, failed!.status);
            Guid identity = failed.persistentID;
            loader.Rescan();
            Assert.True(loader.TryGetInfo(parent, out AssetInfo? unchanged));
            Assert.Equal(AssetImportStatus.Failed, unchanged!.status);

            if (reopen)
            {
                loader.Dispose();
                loader = null;
            }
            DeferredAssetImporter.isAvailable = true;
            m_types.Rebuild();
            loader ??= new AssetLoader(
                m_types,
                m_serialization,
                m_identities,
                m_diagnostics,
                m_logs,
                workspace.assetRoot,
                workspace.libraryRoot
            );
            loader.Rescan();
            Assert.True(loader.TryGetInfo(parent, out AssetInfo? recovered));
            Assert.Equal(AssetImportStatus.Imported, recovered!.status);
            Assert.Equal(identity, recovered.persistentID);
            Assert.True(loader.TryGetInfo(AssetPath.Project("child.deferredasset"), out AssetInfo? child));
            Assert.Equal(AssetImportStatus.Imported, child!.status);
        }
        finally
        {
            loader?.Dispose();
            DeferredAssetImporter.isAvailable = true;
        }
    }

    [Fact]
    public void OneImporterCanKeepGraphInputsInAuthoringWhileExportingExecutableAssets()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("node.mixedscope", "authoring");
        workspace.WriteText("shader.mixedscope", "runtime");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        Assert.NotNull(loader.Load(AssetPath.Project("node.mixedscope"), typeof(DependencyAsset)));
        Assert.NotNull(loader.Load(AssetPath.Project("shader.mixedscope"), typeof(DependencyAsset)));
        string contentRoot = Path.Combine(workspace.libraryRoot, "Runtime");
        loader.ExportRuntimeArtifacts(contentRoot);
        using SerializationGeneration serialization = m_serialization.CaptureGeneration();
        using ContentTestStore contentStore = ContentTestStore.FromDirectory(contentRoot);
        using var database = new AssetDatabase(contentStore, serialization, m_types.current, new IdentityAllocator());
        Assert.True(database.TryLoad(AssetPath.Project("shader.mixedscope"), out DependencyAsset? shader));
        Assert.NotNull(shader);
        Assert.False(database.TryLoad(AssetPath.Project("node.mixedscope"), out DependencyAsset? node));
        Assert.Null(node);
    }

    [Fact]
    public async Task RuntimeLeaseRetriesPendingBudgetEvictionWithoutReleasingAnotherLease()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.hookasset", "leased payload");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        Assert.NotNull(loader.Load(AssetPath.Project("value.hookasset"), typeof(HookAsset)));
        string contentRoot = Path.Combine(workspace.libraryRoot, "Runtime");
        loader.ExportRuntimeArtifacts(contentRoot);
        using SerializationGeneration serialization = m_serialization.CaptureGeneration();
        var identities = new IdentityAllocator();
        using ContentTestStore contentStore = ContentTestStore.FromDirectory(contentRoot);
        using var database = new AssetDatabase(
            contentStore,
            serialization,
            m_types.current,
            identities,
            residencyBudgetBytes: 0
        );
        Task<AssetLease<HookAsset>> pending = database.AcquireAsync<HookAsset>(
            AssetPath.Project("value.hookasset")
        ).AsTask();
        Assert.True(
            SpinWait.SpinUntil(() =>
            {
                database.CompletePendingLoads();
                return pending.IsCompleted;
            }, TimeSpan.FromSeconds(5))
        );
        using AssetLease<HookAsset> first = await pending;
        HookAsset asset = first.asset;
        RuntimeIdentity identity = asset.identity.runtimeIdentity!.Value;
        using AssetLease<HookAsset> last = await database.AcquireAsync<HookAsset>(asset.identity.persistentID);
        asset.pendingUnloads = 1;
        first.Dispose();
        first.Dispose();
        Assert.Equal(0, asset.unloadingCount);
        Assert.Same(asset, last.asset);

        Assert.Throws<RetirementPendingException>(last.Dispose);
        Assert.Same(asset, last.asset);
        Assert.Same(asset, identities.Get<AssetObject>(identity));
        Assert.Equal("leased payload", Encoding.UTF8.GetString(asset.runtimePayload.Span));
        last.Dispose();
        last.Dispose();
        Assert.Equal(2, asset.unloadingCount);
        Assert.Equal(0, database.residencyStatistics.residentAssetCount);
        Assert.Null(identities.Get<AssetObject>(identity));
        Assert.Throws<ObjectDisposedException>(() => last.asset);
    }

    [Fact]
    public void RuntimeDependencyCycleIsAllowedAndLoadsAsOneCanonicalSubgraph()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Graphs/a.depgraph", "Graphs/b.depgraph");
        workspace.WriteText("Graphs/b.depgraph", "Graphs/a.depgraph");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);

        DependencyAsset root = Assert.IsType<DependencyAsset>(
            loader.Load(AssetPath.Project("Graphs/a.depgraph"), typeof(DependencyAsset))
        );
        IReadOnlyList<AssetDependency> direct = loader.GetDependencies(root);
        IReadOnlyList<AssetDependency> recursive = loader.GetDependencies(root, recursive: true);

        Assert.Single(direct);
        Assert.Single(recursive);
        Assert.Equal("Graphs/b.depgraph", direct[0].lastKnownPath);
        Assert.Equal(2, loader.GetLoadedPaths().Count);
    }

    [Theory]
    [InlineData("success")]
    [InlineData("cancel")]
    [InlineData("invalid")]
    public async Task AsyncRuntimeExportReleasesItsGenerationLeaseAfterEveryOutcome(string outcome)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("leaf.buildinput", "original");
        using var pipeline = new AssetPipeline(
            m_modules,
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            new AssetPipelineOptions
            {
                assetRoot = workspace.assetRoot,
                libraryRoot = workspace.libraryRoot,
                enableFileSystemWatcher = false
            }
        );
        DependencyAsset leaf = pipeline.Load<DependencyAsset>(AssetPath.Project("leaf.buildinput"));
        workspace.WriteText("root.buildconsumer", leaf.identity.persistentID.ToString());
        _ = pipeline.Load<DependencyAsset>(AssetPath.Project("root.buildconsumer"));
        if (outcome == "invalid") workspace.WriteText("leaf.buildinput", "changed input");
        using var cancellation = new CancellationTokenSource();
        if (outcome == "cancel") cancellation.Cancel();
        Task<AssetRuntimeContentInfo> export = pipeline.ExportRuntimeArtifactsAsync(
            Path.Combine(workspace.libraryRoot, "AsyncRuntime"),
            cancellation.Token
        );
        if (outcome == "cancel")
        {
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => export);
        }
        else if (outcome == "invalid")
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => export);
        }
        else
        {
            _ = await export;
        }
        m_modules.generations.EnsureReady("replace the generation after export");
    }

    [Theory]
    [InlineData("failed", true)]
    [InlineData("changed", false)]
    public void RuntimeExportRejectsTransitiveAuthoringInputsWithoutDiscardingLastGood(
        string change,
        bool importChange
    ) {
        using var workspace = new TestWorkspace();
        workspace.WriteText("leaf.buildinput", "original");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        Assert.True(loader.Import(AssetPath.Project("leaf.buildinput")));
        AssetObject leaf = Assert.IsType<DependencyAsset>(loader.Load(AssetPath.Project("leaf.buildinput"), typeof(DependencyAsset)));
        workspace.WriteText("middle.buildinput", leaf.identity.persistentID.ToString());
        Assert.True(loader.Import(AssetPath.Project("middle.buildinput")));
        AssetObject middle = Assert.IsType<DependencyAsset>(loader.Load(AssetPath.Project("middle.buildinput"), typeof(DependencyAsset)));
        workspace.WriteText("root.buildconsumer", middle.identity.persistentID.ToString());
        Assert.True(loader.Import(AssetPath.Project("root.buildconsumer")));
        loader.ExportRuntimeArtifacts(Path.Combine(workspace.libraryRoot, "ValidRuntime"));

        workspace.WriteText("leaf.buildinput", change);
        if (importChange)
        {
            Assert.False(loader.Import(AssetPath.Project("leaf.buildinput")));
        }
        using (ArtifactLease lastGood = loader.AcquireArtifact(leaf.identity.persistentID, "authoring"))
        {
            Assert.Equal("original", Encoding.UTF8.GetString(lastGood.ReadAllBytes()));
        }
        string invalidDestination = Path.Combine(workspace.libraryRoot, "InvalidRuntime");
        InvalidOperationException error = Assert.Throws<InvalidOperationException>(() => loader.ExportRuntimeArtifacts(invalidDestination));
        Assert.Contains("root.buildconsumer -> middle.buildinput -> leaf.buildinput", error.Message);
        Assert.False(Directory.Exists(invalidDestination));

        workspace.WriteText("leaf.buildinput", "original");
        Assert.True(loader.Import(AssetPath.Project("leaf.buildinput")));
        loader.ExportRuntimeArtifacts(Path.Combine(workspace.libraryRoot, "RestoredRuntime"));

        workspace.WriteText("root.buildconsumer", "not a valid dependency identity");
        Assert.False(loader.Import(AssetPath.Project("root.buildconsumer")));
        error = Assert.Throws<InvalidOperationException>(() =>
            loader.ExportRuntimeArtifacts(Path.Combine(workspace.libraryRoot, "FailedRootRuntime")));
        Assert.Contains("root.buildconsumer", error.Message);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ColdRootsShareDependencyIOAndRetireReservationsAcrossRepeatedCycles(bool cancelFirst)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Shared.txt", new string('s', 65_536));
        workspace.WriteText("A.depgraph", "Shared.txt");
        workspace.WriteText("B.depgraph", "Shared.txt");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        Assert.NotNull(loader.Load(AssetPath.Project("A.depgraph"), typeof(DependencyAsset)));
        Assert.NotNull(loader.Load(AssetPath.Project("B.depgraph"), typeof(DependencyAsset)));
        string contentRoot = Path.Combine(workspace.libraryRoot, "Runtime");
        loader.ExportRuntimeArtifacts(contentRoot);
        using SerializationGeneration serialization = m_serialization.CaptureGeneration();
        using ContentTestStore contentStore = ContentTestStore.FromDirectory(contentRoot);
        using var database = new AssetDatabase(
            contentStore,
            serialization,
            m_types.current,
            new IdentityAllocator(),
            residencyBudgetBytes: 0,
            preparationBudgetBytes: 100_000
        );
        for (int cycle = 0; cycle < 2; cycle++)
        {
            using var cancellation = new CancellationTokenSource();
            Task<AssetLease<DependencyAsset>> first = database.AcquireAsync<DependencyAsset>(
                AssetPath.Project("A.depgraph"),
                cancellation.Token
            ).AsTask();
            long firstBytes = database.preparingBytes;
            Task<AssetLease<DependencyAsset>> second = database.AcquireAsync<DependencyAsset>(
                AssetPath.Project("B.depgraph")
            ).AsTask();
            Assert.Equal(3, database.preparationStatistics.pendingPayloads);
            Assert.True(database.preparingBytes < firstBytes * 2);
            if (cancelFirst)
            {
                cancellation.Cancel();
            }
            Assert.True(
                SpinWait.SpinUntil(() =>
                {
                    database.CompletePendingLoads();
                    return first.IsCompleted && second.IsCompleted;
                }, TimeSpan.FromSeconds(5))
            );
            if (cancelFirst)
            {
                Assert.True(first.IsCanceled);
            }
            else
            {
                (await first).Dispose();
            }
            (await second).Dispose();
            Assert.Equal(0, database.preparationStatistics.pendingPayloads);
            Assert.Equal(0, database.preparingBytes);
            Assert.Equal(0, database.preparationStatistics.pendingRequests);
            Assert.Equal(0, database.residencyStatistics.residentAssetCount);
        }
        AssetPreparationStatistics statistics = database.preparationStatistics;
        Assert.Equal(6, statistics.payloadReadsStarted);
        Assert.Equal(2, statistics.sharedPayloadReads);
        Assert.Equal(2, statistics.peakPendingRequests);
        Assert.InRange(statistics.peakReservedBytes, 65_536, 100_000);
        Assert.Equal(0, statistics.rejectedRequests);
    }

    [Fact]
    public void ColdAdmissionCountersRemainBoundedAndDisposeDrainsEveryCanceledWaiter()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Shared.txt", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        Assert.NotNull(loader.Load(AssetPath.Project("Shared.txt"), typeof(TextAsset)));
        string contentRoot = Path.Combine(workspace.libraryRoot, "Runtime");
        loader.ExportRuntimeArtifacts(contentRoot);
        using SerializationGeneration serialization = m_serialization.CaptureGeneration();
        using ContentTestStore contentStore = ContentTestStore.FromDirectory(contentRoot);
        using var database = new AssetDatabase(contentStore, serialization, m_types.current, new IdentityAllocator());
        Task<AssetLease<TextAsset>>[] requests = Enumerable.Range(0, 32).Select(_ =>
            database.AcquireAsync<TextAsset>(AssetPath.Project("Shared.txt")).AsTask()).ToArray();
        Assert.Throws<InvalidOperationException>(() => database.AcquireAsync<TextAsset>(AssetPath.Project("Shared.txt")));
        Assert.Equal(32, database.preparationStatistics.pendingRequests);
        Assert.Equal(32, database.preparationStatistics.peakPendingRequests);
        Assert.Equal(1, database.preparationStatistics.rejectedRequests);
        Assert.Equal(1, database.preparationStatistics.payloadReadsStarted);
        database.Dispose();
        Assert.All(requests, request => Assert.True(request.IsCanceled));
        Assert.Equal(0, database.preparationStatistics.pendingPayloads);
        Assert.Equal(0, database.preparationStatistics.pendingRequests);
        Assert.Equal(0, database.preparationStatistics.reservedBytes);
    }

    [Fact]
    public void CorruptSharedPayloadFailsBothRootsWithoutLeakingReservationsOrPublishingAssets()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Shared.txt", "payload");
        workspace.WriteText("A.depgraph", "Shared.txt");
        workspace.WriteText("B.depgraph", "Shared.txt");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        Assert.NotNull(loader.Load(AssetPath.Project("A.depgraph"), typeof(DependencyAsset)));
        Assert.NotNull(loader.Load(AssetPath.Project("B.depgraph"), typeof(DependencyAsset)));
        AssetObject shared = Assert.IsType<TextAsset>(loader.Load(AssetPath.Project("Shared.txt"), typeof(TextAsset)));
        string contentRoot = Path.Combine(workspace.libraryRoot, "Runtime");
        loader.ExportRuntimeArtifacts(contentRoot);
        using SerializationGeneration serialization = m_serialization.CaptureGeneration();
        using ContentTestStore contentStore = ContentTestStore.FromDirectory(contentRoot);
        using var database = new AssetDatabase(contentStore, serialization, m_types.current, new IdentityAllocator());
        using (ArtifactLease artifact = database.AcquireArtifact(shared.identity.persistentID, "runtime"))
        {
            ContentKey key = Assert.Single(
                contentStore.index.entries.Where(entry => (
                    entry.key.value!.StartsWith("Artifacts/", StringComparison.Ordinal)
                    && entry.key.value.Contains("/outputs/", StringComparison.Ordinal)
                    && entry.contentHash == artifact.info.contentHash
                ))
            ).key;
            byte[] bytes = artifact.ReadAllBytes();
            bytes[0] ^= 0xFF;
            contentStore.CorruptPayload(key, bytes);
        }
        Task<AssetLease<DependencyAsset>> first = database.AcquireAsync<DependencyAsset>(AssetPath.Project("A.depgraph")).AsTask();
        Task<AssetLease<DependencyAsset>> second = database.AcquireAsync<DependencyAsset>(AssetPath.Project("B.depgraph")).AsTask();
        Assert.True(
            SpinWait.SpinUntil(() =>
            {
                database.CompletePendingLoads();
                return first.IsCompleted && second.IsCompleted;
            }, TimeSpan.FromSeconds(5))
        );
        Assert.True(first.IsFaulted);
        Assert.True(second.IsFaulted);
        Assert.Contains("integrity", first.Exception!.ToString());
        Assert.Contains("integrity", second.Exception!.ToString());
        Assert.Equal(0, database.preparingBytes);
        Assert.Equal(0, database.preparationStatistics.pendingPayloads);
        Assert.Equal(0, database.residencyStatistics.residentAssetCount);
    }

    [Fact]
    public void RuntimeDependencyRetentionKeepsDependencyAliveWhileRootIsReferenced()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/dependency.txt", "dependency");
        workspace.WriteText("Graphs/root.depgraph", "Text/dependency.txt");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        DependencyAsset root = Assert.IsType<DependencyAsset>(
            loader.Load(AssetPath.Project("Graphs/root.depgraph"), typeof(DependencyAsset))
        );

        Assert.Equal(0, loader.UnloadUnusedAssets());
        Assert.Equal(2, loader.GetLoadedPaths().Count);
        GC.KeepAlive(root);
    }

    [Fact]
    public void UnusedRuntimeDependencyCycleIsCollectedAsAUnit()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Graphs/a.depgraph", "Graphs/b.depgraph");
        workspace.WriteText("Graphs/b.depgraph", "Graphs/a.depgraph");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        (WeakReference first, WeakReference second) = LoadCycleWithoutEscaping(loader);

        int released = loader.UnloadUnusedAssets();

        Assert.Equal(2, released);
        Assert.False(first.IsAlive);
        Assert.False(second.IsAlive);
        Assert.Empty(loader.GetLoadedPaths());
    }

    [Fact]
    public void ReferenceDiagnosticsDoNotKeepAssetsAlive()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/dependency.txt", "dependency");
        workspace.WriteText("Graphs/root.depgraph", "Text/dependency.txt");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        WeakReference weak = LoadAndInspectWithoutEscaping(loader);

        Assert.Equal(2, loader.UnloadUnusedAssets());
        Assert.False(weak.IsAlive);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference first, WeakReference second) LoadCycleWithoutEscaping(AssetLoader loader)
    {
        AssetObject first = loader.Load(AssetPath.Project("Graphs/a.depgraph"), typeof(DependencyAsset))!;
        AssetDependency secondDescriptor = Assert.Single(loader.GetDependencies(first));
        AssetObject second = loader.Load(secondDescriptor.persistentID, typeof(DependencyAsset))!;
        return (new WeakReference(first), new WeakReference(second));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference LoadAndInspectWithoutEscaping(AssetLoader loader)
    {
        AssetObject root = loader.Load(AssetPath.Project("Graphs/root.depgraph"), typeof(DependencyAsset))!;
        AssetDependency descriptor = Assert.Single(loader.GetDependencies(root));
        AssetObject dependency = loader.Load(descriptor.persistentID, typeof(TextAsset))!;
        AssetReferenceInfo info = loader.GetReferenceInfo(dependency);
        Assert.Equal(1, info.knownReferenceCount);
        Assert.Equal(AssetReferenceKind.AssetDependency, info.references[0].kind);
        return new WeakReference(root);
    }
}
