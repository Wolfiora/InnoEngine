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
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Testing.Content;
using Inno.Testing.Diagnostics;

namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetCatalogIdentityTests : AssetLoaderFixture
{
    [Fact]
    public void MissingReferenceNeverBindsAnotherIdentityAtItsLastKnownPath()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/replaced.txt", "replacement");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        TextAsset replacement = Assert.IsType<TextAsset>(loader.Load(AssetPath.Project("Text/replaced.txt"), typeof(TextAsset)));
        Guid intended = Guid.NewGuid();
        Assert.True(m_types.TryGetTypeRef(typeof(TextAsset), out TypeRef type));

        AssetObject missing = loader.ResolveReference(intended, type.stableID, "Text/replaced.txt", typeof(TextAsset));

        Assert.True(missing.isMissing);
        Assert.Equal(intended, missing.identity.persistentID);
        Assert.NotSame(replacement, missing);
        Assert.Same(missing, loader.ResolveReference(intended, type.stableID, "Text/replaced.txt", typeof(TextAsset)));
    }

    [Fact]
    public void RenameAndDeleteUpdateCanonicalInstanceWithoutChangingIdentity()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/old.txt", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        TextAsset asset = Assert.IsType<TextAsset>(loader.Load(AssetPath.Project("Text/old.txt"), typeof(TextAsset)));
        Guid persistentID = asset.identity.persistentID;

        workspace.Move("Text/old.txt", "Text/new.txt");
        loader.ApplySourceChanges([
            new AssetChangedEvent("Text/new.txt", WatcherChangeTypes.Renamed, "Text/old.txt")
        ]);

        Assert.Equal("Text/new.txt", asset.assetPath.ToString());
        Assert.Same(asset, loader.Load(persistentID, typeof(TextAsset)));

        workspace.DeleteSource("Text/new.txt");
        loader.ApplySourceChanges([
            new AssetChangedEvent("Text/new.txt", WatcherChangeTypes.Deleted)
        ]);

        Assert.True(asset.isMissing);
        Assert.Equal(persistentID, asset.identity.persistentID);
        Assert.True(asset.runtimePayload.IsEmpty);
        Assert.False(File.Exists(workspace.SourcePath("Text/new.txt.imeta")));
        Assert.False(loader.TryGetInfo(AssetPath.Project("Text/new.txt"), out _));
        Assert.True(loader.TryGetInfo(persistentID, out AssetInfo? tombstone));
        Assert.NotNull(tombstone);
        Assert.Equal(AssetImportStatus.Missing, tombstone.status);
        Assert.True(tombstone.artifactKey.isEmpty);
        Assert.False(tombstone.lastSuccessfulArtifactKey.isEmpty);
    }

    [Fact]
    public void RecreatedSourceWithoutMetadataReceivesNewIdentity()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/recover.txt", "one");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        TextAsset asset = Assert.IsType<TextAsset>(loader.Load(AssetPath.Project("Text/recover.txt"), typeof(TextAsset)));

        workspace.DeleteSource("Text/recover.txt");
        loader.ApplySourceChanges([
            new AssetChangedEvent("Text/recover.txt", WatcherChangeTypes.Deleted)
        ]);
        workspace.WriteText("Text/recover.txt", "two");
        loader.ApplySourceChanges([
            new AssetChangedEvent("Text/recover.txt", WatcherChangeTypes.Created)
        ]);

        TextAsset replacement = Assert.IsType<TextAsset>(
            loader.Load(AssetPath.Project("Text/recover.txt"), typeof(TextAsset))
        );
        Assert.True(asset.isMissing);
        Assert.False(replacement.isMissing);
        Assert.Equal("two", replacement.content);
        Assert.NotEqual(asset.identity.persistentID, replacement.identity.persistentID);
        Assert.Same(asset, loader.Load(asset.identity.persistentID, typeof(TextAsset)));
    }

    [Fact]
    public void TombstoneSurvivesCatalogRestartWithoutOccupyingItsFormerPath()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/value.txt", "one");
        Guid oldID;
        using (AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs))
        {
            loader.Rescan();
            Assert.True(loader.TryGetPersistentID(AssetPath.Project("Text/value.txt"), out oldID));
            workspace.DeleteSource("Text/value.txt");
            loader.ApplySourceChanges([
                new AssetChangedEvent("Text/value.txt", WatcherChangeTypes.Deleted)
            ]);
        }

        workspace.WriteText("Text/value.txt", "two");
        using AssetLoader restarted = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        restarted.Rescan();

        Assert.True(restarted.TryGetInfo(oldID, out AssetInfo? tombstone));
        Assert.Equal(AssetImportStatus.Missing, tombstone!.status);
        Assert.True(restarted.TryGetPersistentID(AssetPath.Project("Text/value.txt"), out Guid newID));
        Assert.NotEqual(oldID, newID);
    }

    [Fact]
    public void CatalogRestartPrefersCurrentSourceIdentityOverHistoricalPathRecord()
    {
        using var workspace = new TestWorkspace();
        const string C_PATH = "Text/identity.txt";
        workspace.WriteText(C_PATH, "value");
        Guid historicalID;
        using (AssetLoader first = workspace.CreateLoader(
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs
        )) {
            first.Rescan();
            Assert.True(first.TryGetPersistentID(AssetPath.Project(C_PATH), out historicalID));
        }

        string sidecarPath = workspace.SourcePath(C_PATH + ".imeta");
        MountedSourceMetadata metadata = m_serialization.Deserialize<MountedSourceMetadata>(
            File.ReadAllBytes(sidecarPath)
        );
        Guid currentID = Guid.NewGuid();
        metadata.persistentId = currentID;
        File.WriteAllBytes(sidecarPath, m_serialization.Serialize(metadata));

        using (AssetLoader recovered = workspace.CreateLoader(
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs
        )) {
            recovered.Rescan();
            Assert.True(recovered.TryGetPersistentID(AssetPath.Project(C_PATH), out Guid indexedID));
            Assert.Equal(currentID, indexedID);
            Assert.True(recovered.TryGetInfo(historicalID, out AssetInfo? historical));
            Assert.Equal(AssetImportStatus.Missing, historical!.status);
        }

        using AssetLoader restarted = workspace.CreateLoader(
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs
        );
        restarted.Rescan();
        Assert.True(restarted.TryGetPersistentID(AssetPath.Project(C_PATH), out Guid stableID));
        Assert.Equal(currentID, stableID);
        MountedSourceMetadata stableMetadata = m_serialization.Deserialize<MountedSourceMetadata>(
            File.ReadAllBytes(sidecarPath)
        );
        Assert.Equal(currentID, stableMetadata.persistentId);
    }

    [Fact]
    public void CatalogRestartDoesNotPromoteHistoricalTombstonesToMissingReferences()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/removed.txt", "one");
        Guid removedID;
        using (AssetLoader loader = workspace.CreateLoader(
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs
        )) {
            loader.Rescan();
            Assert.True(loader.TryGetPersistentID(AssetPath.Project("Text/removed.txt"), out removedID));
            workspace.DeleteSource("Text/removed.txt");
            loader.ApplySourceChanges([
                new AssetChangedEvent("Text/removed.txt", WatcherChangeTypes.Deleted)
            ]);
        }

        var sink = new TestDiagnosticSink();
        m_diagnostics.RegisterSink(sink);
        try
        {
            using var restarted = new AssetPipeline(
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

            Assert.True(restarted.TryGetInfo(removedID, out AssetInfo? tombstone));
            Assert.Equal(AssetImportStatus.Missing, tombstone!.status);

            using AssetSourceMountTransaction generation = restarted.PrepareSourceMounts(restarted.sourceMounts);
            generation.Activate();

            Assert.Empty(generation.recoveryChanges);
            Assert.DoesNotContain(
                sink.reports.Values.SelectMany(static report => report.diagnostics),
                static diagnostic => diagnostic.code == "ASSET-REFERENCE"
            );

            generation.Complete();

            AssetObject explicitlyResolved = ((IAssetReferenceResolver)restarted).Resolve(
                removedID,
                tombstone.stableAssetTypeID,
                "Text/removed.txt",
                typeof(TextAsset),
                "$test.reference"
            );
            Assert.True(explicitlyResolved.isMissing);
            Assert.Contains(
                sink.reports.Values.SelectMany(static report => report.diagnostics),
                static diagnostic => diagnostic.code == "ASSET-REFERENCE"
            );
        }
        finally
        {
            m_diagnostics.UnregisterSink(sink);
        }
    }

    [Fact]
    public void RestoredSourceAndMetadataReactivatesOriginalIdentityInPlace()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/recover.txt", "one");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        TextAsset asset = Assert.IsType<TextAsset>(loader.Load(AssetPath.Project("Text/recover.txt"), typeof(TextAsset)));
        Guid id = asset.identity.persistentID;
        byte[] metadata = File.ReadAllBytes(workspace.SourcePath("Text/recover.txt.imeta"));

        workspace.DeleteSource("Text/recover.txt");
        loader.ApplySourceChanges([
            new AssetChangedEvent("Text/recover.txt", WatcherChangeTypes.Deleted)
        ]);
        workspace.WriteText("Text/recover.txt", "two");
        File.WriteAllBytes(workspace.SourcePath("Text/recover.txt.imeta"), metadata);
        loader.ApplySourceChanges([
            new AssetChangedEvent("Text/recover.txt", WatcherChangeTypes.Created)
        ]);

        TextAsset restored = Assert.IsType<TextAsset>(loader.Load(AssetPath.Project("Text/recover.txt"), typeof(TextAsset)));
        Assert.Same(asset, restored);
        Assert.Equal(id, restored.identity.persistentID);
        Assert.False(restored.isMissing);
        Assert.Equal("two", restored.content);
    }

    [Fact]
    public void DeleteCreateRenameFallbackPreservesIdentityOnlyWhenFingerprintMatchIsUnique()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/old.txt", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        loader.Rescan();
        Assert.True(loader.TryGetPersistentID(AssetPath.Project("Text/old.txt"), out Guid id));

        workspace.Move("Text/old.txt", "Text/new.txt");
        loader.ApplySourceChanges([
            new AssetChangedEvent("Text/old.txt", WatcherChangeTypes.Deleted),
            new AssetChangedEvent("Text/new.txt", WatcherChangeTypes.Created)
        ]);

        Assert.True(loader.TryGetPersistentID(AssetPath.Project("Text/new.txt"), out Guid movedID));
        Assert.Equal(id, movedID);
        Assert.False(File.Exists(workspace.SourcePath("Text/old.txt.imeta")));
        Assert.True(File.Exists(workspace.SourcePath("Text/new.txt.imeta")));
    }

    [Fact]
    public void AmbiguousDeleteCreateRenameFallbackDoesNotGuessAnIdentity()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/first.txt", "same");
        workspace.WriteText("Text/second.txt", "same");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        loader.Rescan();
        Assert.True(loader.TryGetPersistentID(AssetPath.Project("Text/first.txt"), out Guid firstID));
        Assert.True(loader.TryGetPersistentID(AssetPath.Project("Text/second.txt"), out Guid secondID));

        workspace.DeleteSource("Text/first.txt");
        workspace.DeleteSource("Text/second.txt");
        workspace.WriteText("Text/new.txt", "same");
        loader.ApplySourceChanges([
            new AssetChangedEvent("Text/first.txt", WatcherChangeTypes.Deleted),
            new AssetChangedEvent("Text/second.txt", WatcherChangeTypes.Deleted),
            new AssetChangedEvent("Text/new.txt", WatcherChangeTypes.Created)
        ]);

        Assert.True(loader.TryGetPersistentID(AssetPath.Project("Text/new.txt"), out Guid newID));
        Assert.NotEqual(firstID, newID);
        Assert.NotEqual(secondID, newID);
        Assert.True(loader.TryGetInfo(AssetPath.Project("Text/new.txt"), out AssetInfo? newInfo));
        Assert.Contains(newInfo!.diagnostics, diagnostic =>
            diagnostic.Contains("matched 2 removed assets", StringComparison.Ordinal));
        Assert.True(loader.TryGetInfo(firstID, out AssetInfo? firstTombstone));
        Assert.True(loader.TryGetInfo(secondID, out AssetInfo? secondTombstone));
        Assert.Equal(AssetImportStatus.Missing, firstTombstone!.status);
        Assert.Equal(AssetImportStatus.Missing, secondTombstone!.status);
    }

    [Fact]
    public void RescanTracksUnsupportedSourcesAndFolderIdentityWithoutFakeArtifacts()
    {
        using var workspace = new TestWorkspace();
        Directory.CreateDirectory(workspace.SourcePath("EmptyFolder"));
        workspace.WriteText("Unknown/value.unknown", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);

        loader.Rescan();

        Assert.True(loader.TryGetInfo(AssetPath.Project("EmptyFolder"), out AssetInfo? folder));
        Assert.Equal(AssetSourceKind.Directory, folder!.sourceKind);
        Assert.Equal(AssetImportStatus.Imported, folder.status);
        Assert.True(File.Exists(workspace.SourcePath("EmptyFolder.imeta")));
        Assert.True(folder.artifactKey.isEmpty);
        Assert.True(loader.TryGetInfo(AssetPath.Project("Unknown/value.unknown"), out AssetInfo? unsupported));
        Assert.Equal(AssetImportStatus.Unsupported, unsupported!.status);
        Assert.Equal(Guid.Empty, unsupported.persistentID);
        Assert.False(File.Exists(workspace.SourcePath("Unknown/value.unknown.imeta")));
        Assert.True(unsupported.artifactKey.isEmpty);
    }

    [Fact]
    public void RescanRecoversAnonymousCatalogEntryFromSourceIdentityWithoutFalseMissingWarning()
    {
        using var workspace = new TestWorkspace();
        const string C_PATH = "Deferred/value.unavailable";
        workspace.WriteText(C_PATH, "source awaiting its module generation");
        using (AssetLoader first = workspace.CreateLoader(
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs
        )) {
            first.Rescan();
            Assert.True(first.TryGetInfo(AssetPath.Project(C_PATH), out AssetInfo? unsupported));
            Assert.Equal(Guid.Empty, unsupported!.persistentID);
        }

        Guid persistentID = Guid.NewGuid();
        Guid expectedStableTypeID = Guid.NewGuid();
        File.WriteAllBytes(
            workspace.SourcePath(C_PATH + ".imeta"),
            m_serialization.Serialize(new MountedSourceMetadata
            {
                persistentId = persistentID,
                sourceKind = (int)AssetSourceKind.File,
                importerId = "tests.unavailable-generation"
            })
        );
        var sink = new TestDiagnosticSink();
        m_diagnostics.RegisterSink(sink);
        try
        {
            using (AssetLoader recovered = workspace.CreateLoader(
                m_types,
                m_serialization,
                m_identities,
                m_diagnostics,
                m_logs
            )) {
                recovered.Rescan();
                Assert.True(recovered.TryGetInfo(persistentID, out AssetInfo? pending));
                Assert.Equal(C_PATH, pending!.assetPath.ToString());
                Assert.Equal("tests.unavailable-generation", pending.importerID);
                Assert.Equal(AssetImportStatus.Pending, pending.status);

                AssetObject placeholder = recovered.ResolveReference(
                    persistentID,
                    expectedStableTypeID,
                    C_PATH,
                    typeof(AssetObject)
                );
                Assert.True(placeholder.isMissing);
                Assert.DoesNotContain(
                    sink.reports.Values.SelectMany(static report => report.diagnostics),
                    static diagnostic => diagnostic.code == "ASSET-REFERENCE"
                );
            }

            using AssetLoader restarted = workspace.CreateLoader(
                m_types,
                m_serialization,
                m_identities,
                m_diagnostics,
                m_logs
            );
            restarted.Rescan();
            Assert.True(restarted.TryGetInfo(persistentID, out AssetInfo? retained));
            Assert.Equal("tests.unavailable-generation", retained!.importerID);
        }
        finally
        {
            m_diagnostics.UnregisterSink(sink);
        }
    }

    [Fact]
    public void RescanPreservesLastGoodCatalogWhileItsPathImporterIsUnavailable()
    {
        using var workspace = new TestWorkspace();
        const string C_PATH = "Deferred/value.deferredasset";
        workspace.WriteText(C_PATH, "last-good source");
        Guid persistentID;
        Guid stableTypeID;
        AssetArtifactKey artifactKey;
        using (AssetLoader first = workspace.CreateLoader(
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs
        )) {
            first.Rescan();
            Assert.True(first.TryGetInfo(AssetPath.Project(C_PATH), out AssetInfo? imported));
            persistentID = imported!.persistentID;
            stableTypeID = imported.stableAssetTypeID;
            artifactKey = imported.artifactKey;
            Assert.False(artifactKey.isEmpty);
        }

        var sink = new TestDiagnosticSink();
        m_diagnostics.RegisterSink(sink);
        DeferredAssetImporter.isAvailable = false;
        m_modules.Rebuild();
        try
        {
            using AssetLoader restarted = workspace.CreateLoader(
                m_types,
                m_serialization,
                m_identities,
                m_diagnostics,
                m_logs
            );
            restarted.Rescan();

            Assert.True(restarted.TryGetInfo(AssetPath.Project(C_PATH), out AssetInfo? preserved));
            Assert.Equal(persistentID, preserved!.persistentID);
            Assert.Equal(stableTypeID, preserved.stableAssetTypeID);
            Assert.Equal(AssetImportStatus.Imported, preserved.status);
            Assert.Equal(artifactKey, preserved.artifactKey);
            AssetObject resolved = restarted.ResolveReference(persistentID, stableTypeID, C_PATH, typeof(AssetObject));
            Assert.False(resolved.isMissing);
            Assert.DoesNotContain(
                sink.reports.Values.SelectMany(static report => report.diagnostics),
                static diagnostic => diagnostic.code == "ASSET-REFERENCE"
            );
        }
        finally
        {
            DeferredAssetImporter.isAvailable = true;
            m_modules.Rebuild();
            m_diagnostics.UnregisterSink(sink);
        }
    }

    [Fact]
    public void SourceMetadataRestoresPersistentIdentityWhenLibraryIsRebuilt()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/value.txt", "value");
        Guid id;
        using (AssetLoader first = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs))
        {
            first.Rescan();
            Assert.True(first.TryGetPersistentID(AssetPath.Project("Text/value.txt"), out id));
        }
        Directory.Delete(workspace.libraryRoot, recursive: true);
        Directory.CreateDirectory(workspace.libraryRoot);

        using AssetLoader rebuilt = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        rebuilt.Rescan();

        Assert.True(rebuilt.TryGetPersistentID(AssetPath.Project("Text/value.txt"), out Guid restored));
        Assert.Equal(id, restored);
    }
}
