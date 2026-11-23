using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Serialization.DotNet;
using Inno.Content;
using Inno.Core.Diagnostics;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.References;
using Inno.Testing.Diagnostics;

namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetCandidatePublicationTests : AssetLoaderFixture
{
    [Fact]
    public void IntroducedImportFailureLeavesTheLiveAssetAndCatalogUntouched()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.mutableasset", "previous");
        using var pipeline = new AssetPipeline(
            m_modules,
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            new AssetPipelineOptions { assetRoot = workspace.assetRoot, libraryRoot = workspace.libraryRoot }
        );
        MutableAsset previous = pipeline.Load<MutableAsset>(AssetPath.Project("value.mutableasset"));
        byte[] sidecar = File.ReadAllBytes(workspace.SourcePath("value.mutableasset.imeta"));
        workspace.WriteText("value.mutableasset", "!invalid!");
        Assert.Throws<InvalidDataException>(m_modules.Rebuild);
        Assert.Same(previous, m_identities.Get<AssetObject>(previous.identity.persistentID));
        Assert.Equal("previous", previous.value);
        Assert.True(pipeline.TryGetInfo(previous.identity.persistentID, out AssetInfo? info));
        Assert.Equal(AssetImportStatus.Imported, info!.status);
        Assert.Equal(sidecar, File.ReadAllBytes(workspace.SourcePath("value.mutableasset.imeta")));
        workspace.WriteText("value.mutableasset", "recovered");
        m_modules.Rebuild();
        MutableAsset recovered = pipeline.Load<MutableAsset>(previous.identity.persistentID);
        Assert.NotSame(previous, recovered);
        Assert.Equal("recovered", recovered.value);
    }

    [Fact]
    public void CandidateMetadataRejectsExternalChangesWithoutOverwritingThem()
    {
        using var workspace = new TestWorkspace();
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        workspace.WriteText("new.txt", "candidate");
        using AssetCatalogCandidate candidate = loader.PrepareCatalogCandidate(
            [new AssetSourceMount(AssetSourceID.project, workspace.assetRoot, false)]
        );
        using AssetLoader candidateLoader = candidate.loader;
        candidateLoader.Rescan();
        string sidecarPath = workspace.SourcePath("new.txt.imeta");
        Assert.False(File.Exists(sidecarPath));
        byte[] external = m_serialization.Serialize(new MountedSourceMetadata
        {
            persistentId = Guid.NewGuid(),
            sourceKind = (int)AssetSourceKind.File,
            importerId = "external.importer"
        });
        File.WriteAllBytes(sidecarPath, external);
        Assert.Throws<IOException>(candidate.Commit);
        Assert.Equal(external, File.ReadAllBytes(sidecarPath));
    }

    [Fact]
    public void CandidateMetadataCompensatesEarlierSidecarsWhenALaterWriteFails()
    {
        using var workspace = new TestWorkspace();
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        workspace.WriteText("a.txt", "first");
        workspace.WriteText("z.txt", "last");
        using AssetCatalogCandidate candidate = loader.PrepareCatalogCandidate(
            [new AssetSourceMount(AssetSourceID.project, workspace.assetRoot, false)]
        );
        using AssetLoader candidateLoader = candidate.loader;
        candidateLoader.Rescan();
        Directory.CreateDirectory(workspace.SourcePath("z.txt.imeta"));
        Exception failure = Assert.ThrowsAny<Exception>(candidate.Commit);
        Assert.True(failure is IOException or UnauthorizedAccessException, failure.ToString());
        Assert.False(File.Exists(workspace.SourcePath("a.txt.imeta")));
        Assert.True(Directory.Exists(workspace.SourcePath("z.txt.imeta")));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CandidateDiagnosticsPublishOnlyWithTheirSourceGenerationAndRetireWithoutErasingAnotherOwner(bool commit)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("previous.mutableasset", "!invalid!");
        var sink = new TestDiagnosticSink();
        m_diagnostics.RegisterSink(sink);
        using var pipeline = new AssetPipeline(
            m_modules,
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            new AssetPipelineOptions { assetRoot = workspace.assetRoot, libraryRoot = workspace.libraryRoot }
        );
        Assert.Single(sink.reports.Values);
        string previousSource = Assert.Single(sink.reports.Keys);
        workspace.WriteText("new.mutableasset", "!invalid!");
        using AssetSourceMountTransaction candidate = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
        Assert.Equal(previousSource, Assert.Single(sink.reports.Keys));
        candidate.Activate();
        Assert.Equal(2, sink.reports.Count);
        if (commit)
        {
            candidate.Complete();
            Assert.Equal(2, sink.reports.Count);
            Assert.Contains(previousSource, sink.reports.Keys);
        }
        else
        {
            candidate.Rollback();
            Assert.Equal(previousSource, Assert.Single(sink.reports.Keys));
        }
        pipeline.Dispose();
        Assert.Empty(sink.reports);
        m_diagnostics.UnregisterSink(sink);
    }

    [Fact]
    public void CandidateCommitRestoresCatalogAndSidecarsWhenJournalRetirementFails()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("previous.txt", "last good");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        loader.Rescan();
        string snapshotPath = Path.Combine(workspace.libraryRoot, "AssetDatabase", "Catalog.snapshot");
        string journalPath = Path.Combine(workspace.libraryRoot, "AssetDatabase", "Catalog.journal");
        byte[] previous = File.ReadAllBytes(snapshotPath);
        workspace.WriteText("new.txt", "candidate");
        using AssetCatalogCandidate candidate = loader.PrepareCatalogCandidate(
            [new AssetSourceMount(AssetSourceID.project, workspace.assetRoot, false)]
        );
        using AssetLoader candidateLoader = candidate.loader;
        candidateLoader.Rescan();
        Directory.CreateDirectory(journalPath);

        Exception? failure = Record.Exception(candidate.Commit);
        Assert.True(failure is IOException or UnauthorizedAccessException, failure?.ToString());
        Assert.Equal(previous, File.ReadAllBytes(snapshotPath));
        Assert.False(File.Exists(workspace.SourcePath("new.txt.imeta")));
        Assert.True(Directory.Exists(journalPath));
        Assert.False(loader.TryGetInfo(AssetPath.Project("new.txt"), out _));
    }

    [Fact]
    public void CandidateDiagnosticRecoveryCanRollBackToThePreviousUnresolvedReport()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.mutableasset", "!invalid!");
        var sink = new TestDiagnosticSink();
        m_diagnostics.RegisterSink(sink);
        using var pipeline = new AssetPipeline(
            m_modules,
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            new AssetPipelineOptions { assetRoot = workspace.assetRoot, libraryRoot = workspace.libraryRoot }
        );
        DiagnosticReport previous = Assert.Single(sink.reports.Values);
        workspace.WriteText("value.mutableasset", "fixed");
        using AssetSourceMountTransaction candidate = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
        Assert.Same(previous, Assert.Single(sink.reports.Values));
        candidate.Activate();
        Assert.Empty(sink.reports);
        candidate.Rollback();
        DiagnosticReport restored = Assert.Single(sink.reports.Values);
        Assert.Equal(previous.source.id, restored.source.id);
        Assert.Equal(
            previous.diagnostics.Select(static issue => issue.message),
            restored.diagnostics.Select(static issue => issue.message)
        );
        m_diagnostics.UnregisterSink(sink);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SourceCandidatePublishesCanonicalIdentitiesOnlyDuringSharedRecovery(bool commit)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.txt", "old");
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
        TextAsset previous = pipeline.Load<TextAsset>(AssetPath.Project("value.txt"));
        RuntimeIdentity previousIdentity = previous.identity.runtimeIdentity!.Value;
        workspace.WriteText("value.txt", "candidate");
        using AssetSourceMountTransaction transaction = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
        TextAsset candidate = transaction.Load<TextAsset>(AssetPath.Project("value.txt"));

        Assert.Equal(previous.identity.persistentID, candidate.identity.persistentID);
        Assert.Null(candidate.identity.runtimeID);
        Assert.Same(previous, m_identities.Get<AssetObject>(previousIdentity));
        Assert.Empty(transaction.recoveryChanges);
        transaction.Activate();
        ReferenceRecoveryChange recovered = Assert.Single(transaction.recoveryChanges);
        Assert.Equal(ReferenceResolutionState.Resolved, recovered.resolution.state);
        Assert.Equal(candidate.identity.runtimeIdentity, recovered.resolution.runtimeIdentity);
        Assert.Same(candidate, m_identities.Get<AssetObject>(previous.identity.persistentID));
        Assert.Null(m_identities.Get<AssetObject>(previousIdentity));

        if (commit)
        {
            transaction.Complete();
            Assert.Same(candidate, pipeline.Load<TextAsset>(candidate.identity.persistentID));
            Assert.NotNull(candidate.identity.runtimeID);
        }
        else
        {
            RuntimeIdentity candidateIdentity = candidate.identity.runtimeIdentity!.Value;
            transaction.Rollback();
            Assert.Same(previous, pipeline.Load<TextAsset>(previous.identity.persistentID));
            Assert.Same(previous, m_identities.Get<AssetObject>(previous.identity.persistentID));
            Assert.Null(m_identities.Get<AssetObject>(candidateIdentity));
            Assert.Empty(transaction.recoveryChanges);
        }
    }

    [Fact]
    public void SourceCandidateIdentityConflictRestoresThePreviousDomain()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("previous.txt", "old");
        using var pipeline = new AssetPipeline(
            m_modules,
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            new AssetPipelineOptions { assetRoot = workspace.assetRoot, libraryRoot = workspace.libraryRoot }
        );
        TextAsset previous = pipeline.Load<TextAsset>(AssetPath.Project("previous.txt"));
        workspace.WriteText("candidate.txt", "new");
        using AssetSourceMountTransaction transaction = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
        TextAsset candidate = transaction.Load<TextAsset>(AssetPath.Project("candidate.txt"));
        var unrelated = new IdentityConflict();
        m_identities.Register(unrelated, candidate.identity.persistentID);
        try
        {
            Assert.Throws<InvalidOperationException>(transaction.Activate);
            Assert.Same(previous, m_identities.Get<AssetObject>(previous.identity.persistentID));
            Assert.Same(unrelated, m_identities.Get<IdentityObject>(candidate.identity.persistentID));
            transaction.Rollback();
            Assert.Same(previous, pipeline.Load<TextAsset>(previous.identity.persistentID));
        }
        finally
        {
            m_identities.Unregister(unrelated);
        }
    }

    [Fact]
    public void SourceRemovalAndReturnKeepTheSameRecoverySlotAndNeutralState()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.txt", "preserved");
        using var pipeline = new AssetPipeline(
            m_modules,
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            new AssetPipelineOptions { assetRoot = workspace.assetRoot, libraryRoot = workspace.libraryRoot }
        );
        TextAsset previous = pipeline.Load<TextAsset>(AssetPath.Project("value.txt"));
        Guid id = previous.identity.persistentID;
        byte[] metadata = File.ReadAllBytes(workspace.SourcePath("value.txt.imeta"));
        File.Delete(workspace.SourcePath("value.txt"));
        using AssetSourceMountTransaction removal = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
        removal.Activate();
        ReferenceRecoveryChange missing = Assert.Single(removal.recoveryChanges);
        Assert.Equal(id, missing.resolution.descriptor.targetPersistentID);
        Assert.Equal(ReferenceResolutionState.Missing, missing.resolution.state);
        removal.Complete();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        using AssetSourceMountTransaction stillMissing = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
        stillMissing.Activate();
        Assert.Equal(ReferenceResolutionState.Missing, Assert.Single(stillMissing.recoveryChanges).resolution.state);
        stillMissing.Complete();
        workspace.WriteText("value.txt", "preserved");
        File.WriteAllBytes(workspace.SourcePath("value.txt.imeta"), metadata);
        using AssetSourceMountTransaction recovery = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
        recovery.Activate();
        ReferenceRecoveryChange restored = Assert.Single(recovery.recoveryChanges);
        Assert.Equal(missing.missingState.key, restored.missingState.key);
        Assert.Equal(missing.missingState.payload.ToArray(), restored.missingState.payload.ToArray());
        Assert.Equal(ReferenceResolutionState.Resolved, restored.resolution.state);
        recovery.Complete();
        Assert.Equal(id, pipeline.Load<TextAsset>(AssetPath.Project("value.txt")).identity.persistentID);
    }

    private sealed class IdentityConflict : IdentityObject;
}
