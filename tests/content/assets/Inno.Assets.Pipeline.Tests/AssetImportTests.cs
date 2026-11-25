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
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.References;
using Inno.Testing.Content;
using Inno.Testing.Diagnostics;

namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetImportTests : AssetLoaderFixture
{
    [Fact]
    public void ReadOnlyImportFailureDoesNotRewriteMetadataOrThrowAgainWhileRecordingFailure()
    {
        using var workspace = new TestWorkspace();
        using var package = new TestWorkspace();
        package.WriteText("README.md", "Documentation");
        byte[] metadata = m_serialization.Serialize(new MountedSourceMetadata
        {
            persistentId = Guid.NewGuid(),
            sourceKind = (int)AssetSourceKind.File,
            importerId = "tests.unavailable.importer"
        });
        File.WriteAllBytes(package.SourcePath("README.md.imeta"), metadata);
        var source = new AssetSourceID("tests.readonly");
        using var loader = new AssetLoader(
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            [
                new AssetSourceMount(AssetSourceID.project, workspace.assetRoot, false),
                new AssetSourceMount(source, package.assetRoot, true)
            ],
            workspace.libraryRoot
        );

        Assert.False(loader.Import(new AssetPath(source, "README.md")));
        Assert.Equal(metadata, File.ReadAllBytes(package.SourcePath("README.md.imeta")));
        Assert.True(loader.TryGetInfo(new AssetPath(source, "README.md"), out AssetInfo? failure));
        Assert.Equal(AssetImportStatus.Failed, failure!.status);
        Assert.Contains(failure.diagnostics, value => value.Contains("Read-only source metadata", StringComparison.Ordinal));
    }

    [Fact]
    public void ImportWritesCacheWithoutCreatingCanonicalInstance()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Config/game.txt", "one");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);

        Assert.True(loader.Import(AssetPath.Project("Config/game.txt")));
        Assert.True(File.Exists(workspace.SourcePath("Config/game.txt.imeta")));
        Assert.True(loader.TryGetPersistentID(AssetPath.Project("Config/game.txt"), out Guid persistentID));
        Assert.NotEqual(Guid.Empty, persistentID);
        Assert.True(loader.TryGetArtifact(persistentID, "runtime", out AssetArtifactInfo? artifact));
        Assert.NotNull(artifact);
        using ArtifactLease imported = loader.AcquireArtifact(persistentID, "runtime");
        Assert.NotEmpty(imported.ReadAllBytes());
        Assert.True(loader.TryGetAssetType(AssetPath.Project("Config/game.txt"), out Type? assetType));
        Assert.Equal(typeof(TextAsset), assetType);
        Assert.Empty(loader.GetLoadedPaths());
    }

    [Fact]
    public void PathAndPersistentIDLoadsReturnCanonicalInstance()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/shared.txt", "shared");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);

        TextAsset byPath = Assert.IsType<TextAsset>(loader.Load(AssetPath.Project("Text/shared.txt"), typeof(TextAsset)));
        TextAsset byID = Assert.IsType<TextAsset>(loader.Load(byPath.identity.persistentID, typeof(TextAsset)));

        Assert.Same(byPath, byID);
        Assert.Equal(new[] { AssetPath.Project("Text/shared.txt") }, loader.GetLoadedPaths());
    }

    [Fact]
    public void BlockedSourceVerificationRetainsLastGoodArtifactAndReportsImportFailure()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/cached.txt", "cached");
        using (AssetLoader first = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs))
        {
            first.Rescan();
        }
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        loader.Rescan();
        using var sourceLock = new FileStream(
            workspace.SourcePath("Text/cached.txt"),
            FileMode.Open,
            FileAccess.ReadWrite,
            FileShare.None
        );

        TextAsset asset = Assert.IsType<TextAsset>(
            loader.Load(AssetPath.Project("Text/cached.txt"), typeof(TextAsset))
        );

        Assert.Equal("cached", asset.content);
        Assert.True(loader.TryGetInfo(AssetPath.Project("Text/cached.txt"), out AssetInfo? info));
        Assert.Equal(AssetImportStatus.Failed, info!.status);
        Assert.Contains(info.diagnostics, diagnostic => diagnostic.Contains("IOException", StringComparison.Ordinal));
    }

    [Fact]
    public void ChangedStampWithEqualContentDoesNotReimportCanonicalAsset()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/touched.txt", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        TextAsset before = Assert.IsType<TextAsset>(
            loader.Load(AssetPath.Project("Text/touched.txt"), typeof(TextAsset))
        );
        long version = before.contentVersion;
        string sourcePath = workspace.SourcePath("Text/touched.txt");
        File.SetLastWriteTimeUtc(sourcePath, File.GetLastWriteTimeUtc(sourcePath).AddSeconds(1));

        TextAsset after = Assert.IsType<TextAsset>(
            loader.Load(AssetPath.Project("Text/touched.txt"), typeof(TextAsset))
        );

        Assert.Same(before, after);
        Assert.Equal(version, after.contentVersion);
    }

    [Fact]
    public void BlockedDependencyVerificationRetainsLastGoodArtifactAndReportsImportFailure()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Import/schema.inc", "schema");
        workspace.WriteText("Import/root.importgraph", "Import/schema.inc");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        Assert.True(loader.Import(AssetPath.Project("Import/root.importgraph")));
        using var sourceLock = new FileStream(
            workspace.SourcePath("Import/schema.inc"),
            FileMode.Open,
            FileAccess.ReadWrite,
            FileShare.None
        );

        ImportGraphAsset asset = Assert.IsType<ImportGraphAsset>(
            loader.Load(AssetPath.Project("Import/root.importgraph"), typeof(ImportGraphAsset))
        );

        Assert.False(asset.isMissing);
        Assert.True(loader.TryGetInfo(AssetPath.Project("Import/root.importgraph"), out AssetInfo? info));
        Assert.Equal(AssetImportStatus.Failed, info!.status);
    }

    [Fact]
    public async Task ConcurrentAsyncLoadsShareImportAndCancellationOnlyCancelsOneWaiter()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Slow/item.slowasset", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        using var cancellation = new CancellationTokenSource();

        ValueTask<AssetObject?> first = loader.LoadAsync(AssetPath.Project("Slow/item.slowasset"), typeof(SlowAsset));
        Assert.True(SlowAssetImporter.IMPORT_STARTED.Wait(TimeSpan.FromSeconds(3)));
        ValueTask<AssetObject?> second = loader.LoadAsync(
            AssetPath.Project("Slow/item.slowasset"),
            typeof(SlowAsset),
            cancellation.Token
        );
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await second);
        SlowAssetImporter.ALLOW_IMPORT.Set();
        SlowAsset loaded = Assert.IsType<SlowAsset>(await first);

        Assert.Equal("value", loaded.value);
        Assert.Equal(1, SlowAssetImporter.importCount);
    }

    [Fact]
    public void ChangedDependencyWithPreservedTimestampReimportsTheCanonicalAsset()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Import/schema.inc", "first");
        workspace.WriteText("Import/root.importgraph", "Import/schema.inc");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        ImportGraphAsset before = Assert.IsType<ImportGraphAsset>(
            loader.Load(AssetPath.Project("Import/root.importgraph"), typeof(ImportGraphAsset))
        );
        long version = before.contentVersion;
        string dependency = workspace.SourcePath("Import/schema.inc");
        DateTime timestamp = File.GetLastWriteTimeUtc(dependency);
        workspace.WriteText("Import/schema.inc", "other");
        File.SetLastWriteTimeUtc(dependency, timestamp);

        ImportGraphAsset after = Assert.IsType<ImportGraphAsset>(
            loader.Load(AssetPath.Project("Import/root.importgraph"), typeof(ImportGraphAsset))
        );

        Assert.Same(before, after);
        Assert.Equal(version + 1, after.contentVersion);
        Assert.False(after.isMissing);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void StaleSourceReimportsAndUpdatesCanonicalInstanceInPlace(bool preserveTimestamp)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/reload.txt", "one");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        TextAsset before = Assert.IsType<TextAsset>(loader.Load(AssetPath.Project("Text/reload.txt"), typeof(TextAsset)));
        Guid persistentID = before.identity.persistentID;
        long version = before.contentVersion;
        string sourcePath = workspace.SourcePath("Text/reload.txt");
        DateTime sourceTime = File.GetLastWriteTimeUtc(sourcePath);

        workspace.WriteText("Text/reload.txt", "two");
        if (preserveTimestamp)
        {
            File.SetLastWriteTimeUtc(sourcePath, sourceTime);
        }
        TextAsset after = Assert.IsType<TextAsset>(loader.Load(AssetPath.Project("Text/reload.txt"), typeof(TextAsset)));

        Assert.Same(before, after);
        Assert.Equal(persistentID, after.identity.persistentID);
        Assert.Equal("two", after.content);
        Assert.Equal(version + 1, after.contentVersion);
    }

    [Fact]
    public void ImportDependencyCycleIsRejectedWithClosedPathChain()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Import/a.importgraph", "Import/b.importgraph");
        workspace.WriteText("Import/b.importgraph", "Import/a.importgraph");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);

        Assert.True(loader.Import(AssetPath.Project("Import/a.importgraph")));
        Assert.False(loader.Import(AssetPath.Project("Import/b.importgraph")));
        Assert.True(loader.TryGetInfo(AssetPath.Project("Import/b.importgraph"), out AssetInfo? info));
        Assert.NotNull(info);
        Assert.Equal(AssetImportStatus.Failed, info.status);
        string diagnostic = Assert.Single(info.diagnostics);

        Assert.Contains("Import/a.importgraph", diagnostic);
        Assert.Contains("Import/b.importgraph", diagnostic);
        Assert.Contains("->", diagnostic);
    }

    [Fact]
    public void SavePreservesIdentityIncrementsVersionAndRejectsDifferentPath()
    {
        using var workspace = new TestWorkspace();
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        var asset = new MutableAsset { value = "one" };

        Assert.True(loader.Save(AssetPath.Project("Data/value.mutableasset"), asset));
        Guid persistentID = asset.identity.persistentID;
        long version = asset.contentVersion;
        asset.value = "two";
        Assert.True(loader.Save(asset));

        Assert.Equal(persistentID, asset.identity.persistentID);
        Assert.Equal(version + 1, asset.contentVersion);
        Assert.Equal("two", workspace.ReadText("Data/value.mutableasset"));
        Assert.Same(asset, loader.Load(persistentID, typeof(MutableAsset)));
        Assert.Throws<InvalidOperationException>(() => loader.Save(AssetPath.Project("Data/copy.mutableasset"), asset));
    }

    [Fact]
    public void FailedReimportKeepsPreviousCanonicalStateAndVersion()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Data/stable.mutableasset", "one");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        MutableAsset asset = Assert.IsType<MutableAsset>(
            loader.Load(AssetPath.Project("Data/stable.mutableasset"), typeof(MutableAsset))
        );
        long version = asset.contentVersion;

        workspace.WriteText("Data/stable.mutableasset", "!invalid!");
        Assert.False(loader.Import(AssetPath.Project("Data/stable.mutableasset")));
        Assert.True(loader.TryGetInfo(AssetPath.Project("Data/stable.mutableasset"), out AssetInfo? info));
        Assert.NotNull(info);
        Assert.Equal(AssetImportStatus.Failed, info.status);
        Assert.Contains(info.diagnostics, static value => value.Contains("InvalidDataException"));

        Assert.Equal("one", asset.value);
        Assert.Equal(version, asset.contentVersion);
    }

    [Fact]
    public void FailedReimportPublishesCurrentDiagnosticUntilSuccessfulRetry()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Data/diagnostic.mutableasset", "valid");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        var sink = new TestDiagnosticSink();
        m_diagnostics.RegisterSink(sink);
        try
        {
            MutableAsset asset = Assert.IsType<MutableAsset>(
                loader.Load(AssetPath.Project("Data/diagnostic.mutableasset"), typeof(MutableAsset))
            );
            workspace.WriteText("Data/diagnostic.mutableasset", "!invalid!");

            Assert.False(loader.Import(AssetPath.Project("Data/diagnostic.mutableasset")));
            DiagnosticReport report = Assert.Single(
                sink.reports.Values.Where(value => (value.source.displayName == "Data/diagnostic.mutableasset"))
            );
            Diagnostic diagnostic = Assert.Single(report.diagnostics);
            Assert.Equal("ASSET-IMPORT", diagnostic.code);
            Assert.Equal(DiagnosticSeverity.Error, diagnostic.severity);

            workspace.WriteText("Data/diagnostic.mutableasset", "recovered");
            Assert.True(loader.Import(AssetPath.Project("Data/diagnostic.mutableasset")));
            Assert.DoesNotContain(
                sink.reports.Values,
                value => value.source.displayName == "Data/diagnostic.mutableasset"
            );
            Assert.Equal("recovered", asset.value);
        }
        finally
        {
            m_diagnostics.UnregisterSink(sink);
        }
    }

    [Fact]
    public void UnchangedFailedSourceIsNotRetriedUntilItsInputChanges()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Data/retry.mutableasset", "!invalid!");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        AssetPath path = AssetPath.Project("Data/retry.mutableasset");
        Assert.False(loader.Import(path));
        int failedAttempts = MutableAssetImporter.attempts;
        for (int index = 0; index < 5; index++)
        {
            loader.Rescan();
        }
        Assert.Equal(failedAttempts, MutableAssetImporter.attempts);

        workspace.WriteText("Data/retry.mutableasset", "repaired");
        loader.Rescan();
        Assert.Equal(failedAttempts + 1, MutableAssetImporter.attempts);
        Assert.Equal("repaired", Assert.IsType<MutableAsset>(loader.Load(path, typeof(MutableAsset))).value);
    }

    [Fact]
    public void FailedSavePreservesCommittedSourceMetaArtifactAndVersion()
    {
        using var workspace = new TestWorkspace();
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        var asset = new MutableAsset { value = "one" };
        Assert.True(loader.Save(AssetPath.Project("Data/rollback.mutableasset"), asset));
        byte[] sourceBefore = File.ReadAllBytes(workspace.SourcePath("Data/rollback.mutableasset"));
        byte[] metaBefore = File.ReadAllBytes(workspace.SourcePath("Data/rollback.mutableasset.imeta"));
        Assert.True(loader.TryGetArtifact(asset.identity.persistentID, "runtime", out AssetArtifactInfo? artifact));
        Assert.NotNull(artifact);
        byte[] artifactBefore;
        using (ArtifactLease before = loader.AcquireArtifact(asset.identity.persistentID, "runtime"))
        {
            artifactBefore = before.ReadAllBytes();
        }
        long versionBefore = asset.contentVersion;
        asset.value = "!invalid!";

        Assert.Throws<InvalidDataException>(() => loader.Save(asset));

        Assert.Equal(sourceBefore, File.ReadAllBytes(workspace.SourcePath("Data/rollback.mutableasset")));
        Assert.Equal(metaBefore, File.ReadAllBytes(workspace.SourcePath("Data/rollback.mutableasset.imeta")));
        using (ArtifactLease after = loader.AcquireArtifact(asset.identity.persistentID, "runtime"))
        {
            Assert.Equal(artifactBefore, after.ReadAllBytes());
        }
        Assert.Equal(versionBefore, asset.contentVersion);
    }
}
