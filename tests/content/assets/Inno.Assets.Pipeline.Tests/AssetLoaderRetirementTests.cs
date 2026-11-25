using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Testing.Content;

namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetLoaderRetirementTests : AssetLoaderFixture
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SourceRetirementDrainsPendingHooksBeforeCompletingTheTransaction(bool commit)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.hookasset", "previous");
        using var pipeline = new AssetPipeline(
            m_modules,
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            new AssetPipelineOptions { assetRoot = workspace.assetRoot, libraryRoot = workspace.libraryRoot }
        );
        HookAsset previous = pipeline.Load<HookAsset>(AssetPath.Project("value.hookasset"));
        workspace.WriteText("value.hookasset", "candidate");
        using AssetSourceMountTransaction transaction = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
        HookAsset candidate = transaction.Load<HookAsset>(AssetPath.Project("value.hookasset"));
        HookAsset retiring = commit ? previous : candidate;
        HookAsset retained = commit ? candidate : previous;
        retiring.pendingUnloads = 2;
        transaction.Activate();

        if (commit)
        {
            transaction.Complete();
        }
        else
        {
            transaction.Rollback();
        }

        Assert.Equal(3, retiring.unloadingCount);
        Assert.True(retiring.runtimePayload.IsEmpty);
        Assert.Null(retiring.identity.runtimeID);
        Assert.Same(retained, pipeline.Load<HookAsset>(retained.identity.persistentID));
        Assert.Same(retained, m_identities.Get<AssetObject>(retained.identity.persistentID));
        Assert.Equal(0, retained.unloadingCount);
        Assert.False(retained.runtimePayload.IsEmpty);
        using AssetSourceMountTransaction next = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
    }

    [Fact]
    public void LoaderRetirementRetainsPendingAssetPayloadAndRegistration()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.hookasset", "payload");
        AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        HookAsset asset = Assert.IsType<HookAsset>(loader.Load(AssetPath.Project("value.hookasset"), typeof(HookAsset)));
        RuntimeIdentity identity = asset.identity.runtimeIdentity!.Value;
        asset.pendingUnloads = 1;

        Assert.Throws<RetirementPendingException>(loader.Dispose);
        Assert.Same(asset, m_identities.Get<AssetObject>(identity));
        Assert.Equal("payload", Encoding.UTF8.GetString(asset.runtimePayload.Span));
        Assert.Throws<ObjectDisposedException>(() => loader.Load(AssetPath.Project("value.hookasset"), typeof(HookAsset)));
        loader.Dispose();
        loader.Dispose();
        Assert.Null(m_identities.Get<AssetObject>(identity));
        Assert.Equal(2, asset.unloadingCount);
        Assert.True(asset.runtimePayload.IsEmpty);
    }

    [Fact]
    public void PipelineShutdownDrainsCanonicalAssetsBeforeReleasingTheirOwners()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.hookasset", "payload");
        var pipeline = new AssetPipeline(
            m_modules,
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            new AssetPipelineOptions { assetRoot = workspace.assetRoot, libraryRoot = workspace.libraryRoot }
        );
        HookAsset asset = pipeline.Load<HookAsset>(AssetPath.Project("value.hookasset"));
        asset.pendingUnloads = 1;
        pipeline.Dispose();
        pipeline.Dispose();
        Assert.Equal(2, asset.unloadingCount);
        Assert.Null(asset.identity.runtimeID);
        Assert.False(pipeline.isInitialized);
    }

    [Fact]
    public void LoaderRetirementKeepsEarlierFailuresAcrossPendingResources()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("a.hookasset", "pending");
        workspace.WriteText("b.hookasset", "failure");
        AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        HookAsset pending = Assert.IsType<HookAsset>(loader.Load(AssetPath.Project("a.hookasset"), typeof(HookAsset)));
        HookAsset failing = Assert.IsType<HookAsset>(loader.Load(AssetPath.Project("b.hookasset"), typeof(HookAsset)));
        pending.pendingUnloads = 1;
        failing.failUnloading = true;

        Assert.Throws<RetirementPendingException>(loader.Dispose);
        AggregateException failure = Assert.Throws<AggregateException>(loader.Dispose);
        Assert.Contains("The test asset release failed.", failure.ToString(), StringComparison.Ordinal);
        loader.Dispose();
        Assert.Equal(2, pending.unloadingCount);
        Assert.Equal(1, failing.unloadingCount);
        Assert.Null(pending.identity.runtimeID);
        Assert.Null(failing.identity.runtimeID);
    }

    [Fact]
    public void LoaderShutdownIncludesCanonicalTombstones()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.hookasset", "payload");
        AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        HookAsset asset = Assert.IsType<HookAsset>(loader.Load(AssetPath.Project("value.hookasset"), typeof(HookAsset)));
        File.Delete(workspace.SourcePath("value.hookasset"));
        loader.Rescan();
        Assert.True(asset.isMissing);
        loader.Dispose();
        Assert.Equal(1, asset.unloadingCount);
        Assert.Null(asset.identity.runtimeID);
    }

    [Fact]
    public async Task LoaderDisposalDoesNotBlockAnInFlightLoadOrAdmitNewWork()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.slowasset", "payload");
        AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        Task<AssetObject?> loading = loader.LoadAsync(AssetPath.Project("value.slowasset"), typeof(SlowAsset)).AsTask();
        try
        {
            Assert.True(SlowAssetImporter.IMPORT_STARTED.Wait(TimeSpan.FromSeconds(3)));
            Assert.Throws<RetirementPendingException>(loader.Dispose);
            Assert.Throws<ObjectDisposedException>(() => loader.LoadAsync(AssetPath.Project("value.slowasset"), typeof(SlowAsset)));
        }
        finally
        {
            SlowAssetImporter.ALLOW_IMPORT.Set();
        }
        AssetObject asset = Assert.IsType<SlowAsset>(await loading);
        Assert.NotNull(asset.identity.runtimeID);
        loader.Dispose();
        Assert.Null(asset.identity.runtimeID);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SourceRetirementTimeoutFaultsAdmissionAndKeepsBothGenerationsOwned(bool commit)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.hookasset", "previous");
        var modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(AssetLoaderFixture).Assembly)
        });
        var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        var serialization = new SerializationRegistry(types, new ReflectionSerializationMetadataSource());
        var pipeline = new AssetPipeline(
            modules,
            types,
            serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            new AssetPipelineOptions { assetRoot = workspace.assetRoot, libraryRoot = workspace.libraryRoot }
        );
        HookAsset previous = pipeline.Load<HookAsset>(AssetPath.Project("value.hookasset"));
        workspace.WriteText("value.hookasset", "candidate");
        AssetSourceMountTransaction transaction = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
        HookAsset candidate = transaction.Load<HookAsset>(AssetPath.Project("value.hookasset"));
        HookAsset retiring = commit ? previous : candidate;
        HookAsset retained = commit ? candidate : previous;
        retiring.retirementExpired = true;
        transaction.Activate();

        if (commit)
        {
            Assert.Throws<RetirementTimeoutException>(transaction.Complete);
        }
        else
        {
            Assert.Throws<RetirementTimeoutException>(transaction.Rollback);
        }

        Assert.False(retiring.runtimePayload.IsEmpty);
        Assert.False(retained.runtimePayload.IsEmpty);
        Assert.Equal(0, retained.unloadingCount);
        Assert.ThrowsAny<RetirementPendingException>(() => pipeline.Load<HookAsset>(retained.identity.persistentID));
        retiring.retirementExpired = false;
        Assert.ThrowsAny<RetirementPendingException>(transaction.Dispose);
        Assert.ThrowsAny<RetirementPendingException>(pipeline.Dispose);
        Assert.ThrowsAny<RetirementPendingException>(types.Dispose);
        Assert.ThrowsAny<RetirementPendingException>(modules.Dispose);
        Assert.Equal(1, retiring.unloadingCount);
        Assert.Equal(0, retained.unloadingCount);
        GC.KeepAlive(serialization);
    }

    [Fact]
    public void RuntimeDatabaseRetainsPendingPayloadAndRegistrationUntilShutdownCompletes()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.hookasset", "runtime payload");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        Assert.NotNull(loader.Load(AssetPath.Project("value.hookasset"), typeof(HookAsset)));
        string contentRoot = Path.Combine(workspace.libraryRoot, "Runtime");
        loader.ExportRuntimeArtifacts(contentRoot);
        using SerializationGeneration serialization = m_serialization.CaptureGeneration();
        var identities = new IdentityAllocator();
        using ContentTestStore contentStore = ContentTestStore.FromDirectory(contentRoot);
        var database = new AssetDatabase(contentStore, serialization, m_types.current, identities);
        HookAsset asset = database.Load<HookAsset>(AssetPath.Project("value.hookasset"));
        RuntimeIdentity identity = asset.identity.runtimeIdentity!.Value;
        asset.pendingUnloads = 1;

        Assert.Throws<RetirementPendingException>(database.Dispose);
        Assert.Same(asset, identities.Get<AssetObject>(identity));
        Assert.Equal("runtime payload", Encoding.UTF8.GetString(asset.runtimePayload.Span));
        Assert.Throws<ObjectDisposedException>(() => database.Load<HookAsset>(asset.identity.persistentID));
        database.Dispose();
        database.Dispose();
        Assert.Null(identities.Get<AssetObject>(identity));
        Assert.Equal(2, asset.unloadingCount);
        Assert.True(asset.runtimePayload.IsEmpty);
    }

    [Fact]
    public void DisposeReleasesRuntimeResourcesExactlyOnce()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Hooks/value.hookasset", "value");
        AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        HookAsset asset = Assert.IsType<HookAsset>(loader.Load(AssetPath.Project("Hooks/value.hookasset"), typeof(HookAsset)));

        loader.Dispose();
        loader.Dispose();

        Assert.True(asset.payloadChangeCount >= 1);
        Assert.Equal(1, asset.unloadingCount);
        GC.KeepAlive(asset);
    }
}
