using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Content;
using Inno.Content.Testing;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.References;

namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetLoaderTests : IDisposable
{
    private readonly string m_moduleArtifacts = Path.Combine(
        Path.GetTempPath(),
        "AssetImporterModules-" + Guid.NewGuid().ToString("N")
    );

    private readonly ModuleHost m_modules;
    private readonly TypeCatalog m_types;
    private readonly SerializationRegistry m_serialization;
    private readonly LogRouter m_logs = new();
    private readonly IdentityAllocator m_identities = new();
    private readonly DiagnosticHub m_diagnostics = new();
    private readonly IDisposable m_diagnosticScope;
    private readonly IDisposable m_identityScope;

    public AssetLoaderTests()
    {
        _ = typeof(PrivateConstructorAssetImporter);
        _ = typeof(DeferredAssetImporter);
        _ = typeof(TestBuildProcessor);
        DeferredAssetImporter.isAvailable = true;
        ExtensionDependentImporter.available = false;
        ExtensionDependentImporter.attempts = 0;
        m_identityScope = m_identities.EnterScope();
        m_diagnosticScope = m_diagnostics.EnterScope();
        m_modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(AssetLoaderTests).Assembly)
        });
        m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
        m_serialization = new SerializationRegistry(m_types, new ReflectionSerializationMetadataSource());
        SlowAssetImporter.Reset();
        ImporterConflictProbe.duplicateExtension = false;
        MutableAssetImporter.attempts = 0;
    }

    public void Dispose()
    {
        m_serialization.Dispose();
        m_types.Dispose();
        m_modules.Dispose();
        m_logs.Dispose();
        m_diagnosticScope.Dispose();
        m_identityScope.Dispose();
        m_modules.generations.Wait();
        if (Directory.Exists(m_moduleArtifacts))
        {
            Directory.Delete(m_moduleArtifacts, recursive: true);
        }
    }

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

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ExtensionDiscoveryDefersOnlyMissingExtensionsAndRetriesOnceWhenCompleted(bool available)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("surface.extensionasset", "authored");
        var sink = new TestDiagnosticSink();
        m_diagnostics.RegisterSink(sink);
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
                deferUnavailableExtensions = true
            }
        );
        AssetPath path = AssetPath.Project("surface.extensionasset");
        Assert.True(pipeline.TryGetInfo(path, out AssetInfo? pending));
        Assert.Equal(AssetImportStatus.Pending, pending!.status);
        Assert.Equal(DiagnosticSeverity.Info, Assert.Single(Assert.Single(sink.reports.Values).diagnostics).severity);
        byte[] sidecar = File.ReadAllBytes(workspace.SourcePath("surface.extensionasset.imeta"));
        int attempts = ExtensionDependentImporter.attempts;
        for (int i = 0; i < 5; i++)
        {
            pipeline.Rescan();
        }
        Assert.Equal(attempts, ExtensionDependentImporter.attempts);
        Assert.Throws<InvalidOperationException>(
            () => pipeline.ExportRuntimeArtifacts(Path.Combine(workspace.libraryRoot, "PendingExport"))
        );

        ExtensionDependentImporter.available = available;
        pipeline.CompleteExtensionDiscovery();
        Assert.True(pipeline.TryGetInfo(path, out AssetInfo? completed));
        Assert.Equal(available ? AssetImportStatus.Imported : AssetImportStatus.Failed, completed!.status);
        if (available)
        {
            Assert.Empty(sink.reports);
        }
        else
        {
            Assert.Equal(DiagnosticSeverity.Error, Assert.Single(Assert.Single(sink.reports.Values).diagnostics).severity);
        }
        Assert.Equal(pending.persistentID, completed.persistentID);
        Assert.Equal(sidecar, File.ReadAllBytes(workspace.SourcePath("surface.extensionasset.imeta")));
        Assert.Equal("authored", File.ReadAllText(workspace.SourcePath("surface.extensionasset")));
        attempts = ExtensionDependentImporter.attempts;
        for (int i = 0; i < 5; i++)
        {
            pipeline.Rescan();
        }
        Assert.Equal(attempts, ExtensionDependentImporter.attempts);
        if (!available)
        {
            Assert.Throws<InvalidOperationException>(
                () => pipeline.ExportRuntimeArtifacts(Path.Combine(workspace.libraryRoot, "MissingExport"))
            );
            ExtensionDependentImporter.available = true;
            Assert.True(pipeline.Import(path));
            Assert.True(pipeline.TryGetInfo(path, out AssetInfo? restored));
            Assert.Equal(AssetImportStatus.Imported, restored!.status);
            Assert.Equal(pending.persistentID, restored.persistentID);
        }
        m_diagnostics.UnregisterSink(sink);
    }

    [Fact]
    public void ReadOnlyExtensionWaitPreservesSidecarAndBecomesStrictAfterDiscovery()
    {
        using var workspace = new TestWorkspace();
        using var package = new TestWorkspace();
        package.WriteText("surface.extensionasset", "authored");
        byte[] metadata = m_serialization.Serialize(new MountedSourceMetadata
        {
            persistentId = Guid.NewGuid(),
            sourceKind = (int)AssetSourceKind.File,
            importerId = "tests.extension-dependent"
        });
        File.WriteAllBytes(package.SourcePath("surface.extensionasset.imeta"), metadata);
        var source = new AssetSourceID("tests.readonly-extension");
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
                deferUnavailableExtensions = true,
                sourceMounts =
                [
                    new AssetSourceMount(AssetSourceID.project, workspace.assetRoot, false),
                    new AssetSourceMount(source, package.assetRoot, true)
                ]
            }
        );
        var path = new AssetPath(source, "surface.extensionasset");
        Assert.True(pipeline.TryGetInfo(path, out AssetInfo? pending));
        Assert.Equal(AssetImportStatus.Pending, pending!.status);
        Assert.Throws<InvalidDataException>(pipeline.CompleteExtensionDiscovery);
        Assert.True(pipeline.TryGetInfo(path, out AssetInfo? failed));
        Assert.Equal(AssetImportStatus.Failed, failed!.status);
        Assert.Equal(metadata, File.ReadAllBytes(package.SourcePath("surface.extensionasset.imeta")));
        ExtensionDependentImporter.available = true;
        Assert.True(pipeline.Import(path));
        Assert.True(pipeline.TryGetInfo(path, out AssetInfo? recovered));
        Assert.Equal(AssetImportStatus.Imported, recovered!.status);
        Assert.Equal(pending.persistentID, recovered.persistentID);
    }

    [Fact]
    public void DiscoveryDoesNotDeferMalformedContentAndCandidateActivationDoesNotHideMissingExtensions()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("bad.mutableasset", "!invalid!");
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
                deferUnavailableExtensions = true
            }
        );
        Assert.True(pipeline.TryGetInfo(AssetPath.Project("bad.mutableasset"), out AssetInfo? bad));
        Assert.Equal(AssetImportStatus.Failed, bad!.status);
        workspace.WriteText("surface.extensionasset", "authored");
        pipeline.Rescan();
        m_modules.Rebuild();
        Assert.True(pipeline.TryGetInfo(AssetPath.Project("surface.extensionasset"), out AssetInfo? waiting));
        Assert.Equal(AssetImportStatus.Pending, waiting!.status);
        ExtensionDependentImporter.available = true;
        pipeline.CompleteExtensionDiscovery();
        ExtensionDependentImporter.available = false;
        workspace.WriteText("surface.extensionasset", "changed before activation");
        Assert.Throws<InvalidDataException>(m_modules.Rebuild);
        ExtensionDependentImporter.available = true;
        m_modules.Rebuild();
        Assert.True(pipeline.TryGetInfo(AssetPath.Project("surface.extensionasset"), out AssetInfo? recovered));
        Assert.Equal(AssetImportStatus.Imported, recovered!.status);
        Assert.Equal(waiting.persistentID, recovered.persistentID);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MissingExtensionPropagatesToImportDependentsWithoutHydratingEmptyArtifacts(bool reference)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("surface.extensionasset", "authored");
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
                deferUnavailableExtensions = true
            }
        );
        Assert.True(pipeline.TryGetInfo(AssetPath.Project("surface.extensionasset"), out AssetInfo? root));
        workspace.WriteText("material.extensiondependent", reference ? root!.persistentID.ToString() : "surface.extensionasset");
        pipeline.Rescan();
        Assert.True(pipeline.TryGetInfo(AssetPath.Project("material.extensiondependent"), out AssetInfo? pending));
        Assert.Equal(AssetImportStatus.Pending, pending!.status);
        Assert.Contains(pending.diagnostics, static text => text.Contains("tests.surface", StringComparison.Ordinal));
        InvalidOperationException unavailable = Assert.Throws<InvalidOperationException>(() =>
            pipeline.Load<ExtensionDependentAsset>(AssetPath.Project("surface.extensionasset")));
        Assert.Null(unavailable.InnerException);
        ExtensionDependentImporter.available = true;
        Assert.True(pipeline.Import(AssetPath.Project("surface.extensionasset")));
        pipeline.Rescan();
        Assert.True(pipeline.TryGetInfo(AssetPath.Project("material.extensiondependent"), out AssetInfo? restored));
        Assert.Equal(AssetImportStatus.Imported, restored!.status);
        Assert.Equal(pending.persistentID, restored.persistentID);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AssemblyCatalogPublishesAnIsolatedLoaderAndRollsBackWithoutTouchingLastGood(bool reject)
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
        byte[] previousMeta = File.ReadAllBytes(workspace.SourcePath("value.hookasset.imeta"));
        var observer = new CatalogObserver();
        using IDisposable registration = m_modules.RegisterCatalogParticipant(observer);
        workspace.WriteText("value.hookasset", "candidate");
        workspace.WriteText("new.txt", "new candidate source");
        observer.prepare = () =>
        {
            Assert.Same(previous, m_identities.Get<AssetObject>(previous.identity.persistentID));
            Assert.Equal("previous", Encoding.UTF8.GetString(previous.runtimePayload.Span));
            Assert.False(pipeline.TryGetInfo(AssetPath.Project("new.txt"), out _));
            Assert.False(File.Exists(workspace.SourcePath("new.txt.imeta")));
        };
        observer.activate = () =>
        {
            AssetObject published = m_identities.Get<AssetObject>(previous.identity.persistentID)!;
            Assert.NotSame(previous, published);
            Assert.Equal("candidate", Encoding.UTF8.GetString(published.runtimePayload.Span));
            Assert.Equal("previous", Encoding.UTF8.GetString(previous.runtimePayload.Span));
            Assert.False(File.Exists(workspace.SourcePath("new.txt.imeta")));
            if (reject)
            {
                throw new InvalidOperationException("Expected later participant rejection.");
            }
        };
        if (reject)
        {
            Assert.Throws<InvalidOperationException>(m_modules.Rebuild);
            Assert.Same(previous, m_identities.Get<AssetObject>(previous.identity.persistentID));
            Assert.Equal(0, previous.unloadingCount);
            Assert.Equal("previous", Encoding.UTF8.GetString(previous.runtimePayload.Span));
            Assert.Equal(previousMeta, File.ReadAllBytes(workspace.SourcePath("value.hookasset.imeta")));
            Assert.False(File.Exists(workspace.SourcePath("new.txt.imeta")));
            Assert.False(pipeline.TryGetInfo(AssetPath.Project("new.txt"), out _));
        }
        else
        {
            m_modules.Rebuild();
            Assert.NotSame(previous, pipeline.Load<HookAsset>(previous.identity.persistentID));
            Assert.Equal(1, previous.unloadingCount);
            Assert.True(File.Exists(workspace.SourcePath("new.txt.imeta")));
        }
        observer.prepare = null;
        observer.activate = null;
        if (reject)
        {
            m_modules.Rebuild();
            Assert.NotSame(previous, pipeline.Load<HookAsset>(previous.identity.persistentID));
            Assert.Equal(1, previous.unloadingCount);
            Assert.True(File.Exists(workspace.SourcePath("new.txt.imeta")));
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AssemblyCatalogJoinsAnExistingSourceCandidateWithoutTakingItsPublicationOwnership(bool reject)
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("value.txt", "previous");
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
        var observer = new CatalogObserver();
        using IDisposable registration = m_modules.RegisterCatalogParticipant(observer);
        workspace.WriteText("value.txt", "candidate");
        using AssetSourceMountTransaction candidate = pipeline.PrepareSourceMounts(pipeline.sourceMounts);
        TextAsset replacement = candidate.Load<TextAsset>(AssetPath.Project("value.txt"));
        observer.activate = () =>
        {
            Assert.Same(previous, m_identities.Get<AssetObject>(previous.identity.persistentID));
            if (reject) throw new InvalidOperationException("Expected joined candidate rejection.");
        };
        if (reject)
        {
            Assert.Throws<InvalidOperationException>(m_modules.Rebuild);
        }
        else
        {
            m_modules.Rebuild();
        }
        Assert.Same(previous, m_identities.Get<AssetObject>(previous.identity.persistentID));
        Assert.True(candidate.TryGetInfo(AssetPath.Project("value.txt"), out _));
        observer.activate = null;
        if (reject)
        {
            candidate.Rollback();
        }
        else
        {
            candidate.Activate();
            candidate.Complete();
            Assert.Equal(
                "candidate",
                Encoding.UTF8.GetString(pipeline.Load<TextAsset>(replacement.identity.persistentID).runtimePayload.Span)
            );
        }
    }

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
            catalogSource = new DotNetAssemblyCatalogSource(typeof(AssetLoaderTests).Assembly)
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
    public void ExtensionContractsUseImplementationIdentityWithoutManualVersions()
    {
        Assert.Null(typeof(AssetImporter).GetProperty("version"));
        Assert.Null(typeof(AssetBuildProcessor).GetProperty("version"));
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
    public void AutomaticallyDiscoveredPrivateImporterLoadsWithoutRegistrationApi()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Private/item.privateasset", "private");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);

        PrivateConstructorAsset asset = Assert.IsType<PrivateConstructorAsset>(
            loader.Load(AssetPath.Project("Private/item.privateasset"), typeof(PrivateConstructorAsset))
        );

        Assert.Equal("private", asset.value);
        m_types.Rebuild();
        Assert.Same(asset, loader.Load(AssetPath.Project("Private/item.privateasset"), typeof(PrivateConstructorAsset)));
    }

    [Fact]
    public void SameAssetTypeCanUseMultipleImportersForDifferentExtensions()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Graphs/a.depgraph", string.Empty);
        workspace.WriteText("Graphs/b.depgraph2", string.Empty);
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);

        Assert.IsType<DependencyAsset>(loader.Load(AssetPath.Project("Graphs/a.depgraph"), typeof(DependencyAsset)));
        Assert.IsType<DependencyAsset>(loader.Load(AssetPath.Project("Graphs/b.depgraph2"), typeof(DependencyAsset)));
    }

    [Fact]
    public void DuplicateImporterIDIsRejectedDuringAutomaticDiscovery()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Conflict/initialize.txt", "initialize");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        Assert.True(loader.Import(AssetPath.Project("Conflict/initialize.txt")));
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => m_modules.Load(new DotNetModuleSource
            {
                artifactRootDirectory = m_moduleArtifacts,
                moduleName = "DuplicateAssetImporters",
                mainAssemblyPath = Path.Combine(
                    AppContext.BaseDirectory,
                    "Modules",
                    "DuplicateAssetImporters",
                    "Inno.Assets.Pipeline.DuplicateImporterFixture.dll"
                ),
                domain = AssemblyDomain.InnoPlugin,
                scope = AssemblyScope.Editor
            })
        );

        Assert.Contains("importer id", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void DuplicateImporterExtensionIsRejectedDuringAutomaticDiscovery()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Conflict/value.conflict", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        ImporterConflictProbe.duplicateExtension = true;

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => loader.Import(AssetPath.Project("Conflict/value.conflict"))
        );

        Assert.Contains("extension", exception.Message, StringComparison.OrdinalIgnoreCase);
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
        for (int cycle = 0; cycle < 32; cycle++)
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
        Assert.Equal(96, statistics.payloadReadsStarted);
        Assert.Equal(32, statistics.sharedPayloadReads);
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

    [Fact]
    public void ContentAddressedStoreDeduplicatesEqualImportsAndCollectsUnreachableBundles()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/first.txt", "same");
        workspace.WriteText("Text/second.txt", "same");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        loader.Rescan();
        Assert.True(loader.TryGetInfo(AssetPath.Project("Text/first.txt"), out AssetInfo? first));
        Assert.True(loader.TryGetInfo(AssetPath.Project("Text/second.txt"), out AssetInfo? second));
        Assert.Equal(first!.artifactKey, second!.artifactKey);
        Assert.True(loader.TryGetArtifact(first.persistentID, "runtime", out AssetArtifactInfo? oldArtifact));
        Assert.NotNull(oldArtifact);

        workspace.WriteText("Text/first.txt", "changed");
        Assert.True(loader.Import(AssetPath.Project("Text/first.txt")));
        Assert.True(loader.TryGetArtifact(first.persistentID, "runtime", out AssetArtifactInfo? currentArtifact));
        Assert.NotNull(currentArtifact);
        Assert.NotEqual(oldArtifact.key, currentArtifact.key);
        string oldPath = workspace.FindArtifactFile(oldArtifact);
        Assert.True(File.Exists(oldPath));

        Assert.Equal(0, loader.CollectArtifacts(TimeSpan.Zero, maximumSizeBytes: 0));
        Assert.True(File.Exists(oldPath));
        workspace.DeleteSource("Text/second.txt");
        File.Delete(workspace.SourcePath("Text/second.txt.imeta"));
        loader.Rescan();
        Assert.True(loader.CollectArtifacts(TimeSpan.Zero, maximumSizeBytes: 0) >= 1);
        Assert.False(File.Exists(oldPath));
        Assert.True(File.Exists(workspace.FindArtifactFile(currentArtifact)));
    }

    [Fact]
    public void ArtifactLookupRejectsWrongLengthAndExportRejectsSameLengthCorruption()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/value.txt", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        loader.Rescan();
        Assert.True(loader.TryGetInfo(AssetPath.Project("Text/value.txt"), out AssetInfo? info));
        Assert.True(loader.TryGetArtifact(info!.persistentID, "runtime", out AssetArtifactInfo? artifact));
        string path = workspace.FindArtifactFile(artifact!);
        byte[] original = File.ReadAllBytes(path);

        File.WriteAllBytes(path, [1]);
        Assert.Throws<InvalidDataException>(() => loader.TryGetArtifact(info.persistentID, "runtime", out _));
        byte[] changed = (byte[])original.Clone();
        changed[0] ^= 0x01;
        File.WriteAllBytes(path, changed);
        Assert.True(loader.TryGetArtifact(info.persistentID, "runtime", out _));
        Assert.Throws<InvalidDataException>(() => loader.ExportRuntimeArtifacts(Path.Combine(workspace.libraryRoot, "CorruptedRuntime")));
        File.WriteAllBytes(path, original);
        Assert.True(loader.TryGetArtifact(info.persistentID, "runtime", out _));
        loader.ExportRuntimeArtifacts(Path.Combine(workspace.libraryRoot, "ValidRuntime"));
    }

    [Fact]
    public async Task AggregateBuildProcessorProducesStableContentAddressedOutput()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/input.txt", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        loader.Rescan();
        Assert.True(loader.TryGetInfo(AssetPath.Project("Text/input.txt"), out AssetInfo? input));
        Assert.NotNull(input);
        var definition = new TestBuildDefinitionAsset { label = "bundle" };

        AssetArtifactKey first = await loader.BuildAsync(definition, [input]);
        AssetArtifactKey repeated = await loader.BuildAsync(definition, [input]);

        Assert.False(first.isEmpty);
        Assert.Equal(first, repeated);
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

    private sealed class CatalogObserver : IAssemblyCatalogParticipant
    {
        internal Action? prepare;
        internal Action? activate;

        public IAssemblyCatalogTransaction Prepare(AssemblyCatalogSnapshot catalog)
        {
            prepare?.Invoke();
            return new Observation(activate);
        }

        private sealed class Observation(Action? activate) : IAssemblyCatalogTransaction
        {
            public object? context => null;

            public void Activate() => activate?.Invoke();

            public void Complete() {}

            public void Rollback() {}
        }
    }

    private sealed class IdentityConflict : IdentityObject;

    private sealed class MountedSourceMetadata : ISerializable
    {
        [SerializableProperty]
        public Guid persistentId { get; set; }

        [SerializableProperty]
        public int sourceKind { get; set; }

        [SerializableProperty]
        public string importerId { get; set; } = string.Empty;

        [SerializableProperty]
        public byte[] importerSettingsBytes { get; set; } = [];
    }

    private sealed class TestWorkspace : IDisposable
    {
        private readonly string m_root = Path.Combine(
            Path.GetTempPath(),
            "InnoAssetLoaderTests",
            Guid.NewGuid().ToString("N")
        );

        internal TestWorkspace()
        {
            Directory.CreateDirectory(assetRoot);
            Directory.CreateDirectory(libraryRoot);
        }

        public void Dispose()
        {
            SlowAssetImporter.ALLOW_IMPORT.Set();
            if (Directory.Exists(m_root))
            {
                Directory.Delete(m_root, recursive: true);
            }
        }

        internal string assetRoot => Path.Combine(m_root, "Assets");
        internal string libraryRoot => Path.Combine(m_root, "Library");

        internal AssetLoader CreateLoader(
            TypeCatalog types,
            SerializationRegistry serialization,
            IdentityAllocator identities,
            DiagnosticHub diagnostics,
            LogRouter logs
        ) => new(types, serialization, identities, diagnostics, logs, assetRoot, libraryRoot);

        internal string SourcePath(string relativePath) => Path.Combine(assetRoot, relativePath);

        internal string FindArtifactFile(AssetArtifactInfo artifact)
        {
            string key = artifact.key.value;
            string directory = Path.Combine(
                libraryRoot,
                "Artifacts",
                key[..2].ToLowerInvariant(),
                key[2..4].ToLowerInvariant(),
                key,
                "outputs"
            );
            return Directory.EnumerateFiles(directory).Single(path => (
                new FileInfo(path).Length == artifact.length
                && Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))) == artifact.contentHash
            ));
        }

        internal void WriteText(
            string relativePath,
            string content
        ) {
            string path = SourcePath(relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, content, new UTF8Encoding(false));
        }

        internal string ReadText(string relativePath) => File.ReadAllText(SourcePath(relativePath));

        internal void Move(
            string oldRelativePath,
            string newRelativePath
        ) {
            string target = SourcePath(newRelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.Move(SourcePath(oldRelativePath), target);
        }

        internal void DeleteSource(string relativePath)
        {
            string path = SourcePath(relativePath);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    private sealed class TestDiagnosticSink : IDiagnosticSink
    {
        public void Replace(DiagnosticReport report) => reports[report.source.id] = report;

        public void Clear(DiagnosticSource source) => reports.Remove(source.id);

        internal Dictionary<string, DiagnosticReport> reports { get; } = new(StringComparer.Ordinal);
    }
}

[StableTypeID("501988d0-c069-4dcf-97f6-e899b24e5801")]
internal sealed class PrivateConstructorAsset : AssetObject
{
    [SerializableProperty]
    internal string value { get; set; } = string.Empty;
}

[AssetImporter("inno.tests.private-constructor")]
internal sealed class PrivateConstructorAssetImporter : AssetImporter<PrivateConstructorAsset>
{
    private static readonly IReadOnlyList<string> EXTENSIONS = [".privateasset"];

    private PrivateConstructorAssetImporter() {}

    public override IReadOnlyList<string> supportedExtensions => EXTENSIONS;

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<PrivateConstructorAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new PrivateConstructorAsset { value = context.ReadUtf8Text() });
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[StableTypeID("8d0d31ab-f9ea-4297-b865-e9014ae82a94")]
internal sealed class DeferredAsset : AssetObject;

[StableTypeID("79cef88a-3d5a-4c36-8095-a59cae3c641c")]
internal sealed class ExtensionDependentAsset : AssetObject;

[AssetImporter("tests.extension-dependent")]
internal sealed class ExtensionDependentImporter : AssetImporter<ExtensionDependentAsset>
{
    internal static bool available;
    internal static int attempts;
    public override IReadOnlyList<string> supportedExtensions { get; } = [".extensionasset"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<ExtensionDependentAsset> output,
        CancellationToken cancellationToken
    ) {
        attempts++;
        if (!available)
        {
            throw new AssetImportExtensionUnavailableException("tests.target", "tests.surface");
        }
        output.SetAsset(new ExtensionDependentAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[AssetImporter("tests.extension-consumer")]
internal sealed class ExtensionConsumerImporter : AssetImporter<ExtensionDependentAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".extensiondependent"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<ExtensionDependentAsset> output,
        CancellationToken cancellationToken
    ) {
        string source = context.ReadUtf8Text();
        if (Guid.TryParse(source, out Guid id))
        {
            _ = context.references.Resolve(
                id,
                context.services.GetStableTypeID<ExtensionDependentAsset>(),
                "surface.extensionasset",
                typeof(ExtensionDependentAsset),
                "$.shader"
            );
        }
        else
        {
            _ = context.ResolveDependency<ExtensionDependentAsset>(AssetPath.Project(source));
        }
        output.SetAsset(new ExtensionDependentAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[AssetImporter("inno.tests.deferred")]
internal sealed class DeferredAssetImporter : AssetImporter<DeferredAsset>
{
    private static readonly IReadOnlyList<string> EXTENSIONS = [".deferredasset"];

    public override IReadOnlyList<string> supportedExtensions => isAvailable ? EXTENSIONS : [];

    internal static bool isAvailable { get; set; } = true;

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DeferredAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new DeferredAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[StableTypeID("a49b603c-0f5f-4861-903a-3819513da002")]
internal sealed class DependencyAsset : AssetObject;

[AssetImporter("inno.tests.runtime-dependency")]
internal sealed class DependencyAssetImporter : AssetImporter<DependencyAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".depgraph"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DependencyAsset> output,
        CancellationToken cancellationToken
    ) {
        string dependency = context.ReadUtf8Text().Trim();
        if (!string.IsNullOrWhiteSpace(dependency))
        {
            output.DependsOnAsset(AssetPath.Parse(dependency));
        }
        output.SetAsset(new DependencyAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[AssetImporter("inno.tests.runtime-dependency-alternate")]
internal sealed class AlternateDependencyAssetImporter : AssetImporter<DependencyAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".depgraph2"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DependencyAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new DependencyAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[AssetImporter("inno.tests.build-input")]
internal sealed class BuildInputAssetImporter : AssetImporter<DependencyAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".buildinput"];
    public override AssetDeploymentScope deploymentScope => AssetDeploymentScope.AuthoringOnly;

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DependencyAsset> output,
        CancellationToken cancellationToken
    ) {
        string text = context.ReadUtf8Text();
        if (text == "failed")
        {
            throw new InvalidDataException("Required authoring input failed.");
        }
        if (Guid.TryParse(text, out Guid dependency))
        {
            context.DependsOnArtifact(dependency);
        }
        output.SetAsset(new DependencyAsset());
        return output.WriteArtifactAsync("authoring", context.sourceBytes, cancellationToken, AssetDeploymentScope.AuthoringOnly);
    }
}

[AssetImporter("inno.tests.mixed-scope")]
internal sealed class MixedScopeAssetImporter : AssetImporter<DependencyAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".mixedscope"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DependencyAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new DependencyAsset());
        if (context.ReadUtf8Text() == "authoring")
        {
            output.SetDeploymentScope(AssetDeploymentScope.AuthoringOnly);
            return output.WriteArtifactAsync(
                "graph",
                context.sourceBytes,
                cancellationToken,
                AssetDeploymentScope.AuthoringOnly
            );
        }
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[AssetImporter("inno.tests.build-consumer")]
internal sealed class BuildConsumerAssetImporter : AssetImporter<DependencyAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".buildconsumer"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DependencyAsset> output,
        CancellationToken cancellationToken
    ) {
        context.DependsOnArtifact(Guid.Parse(context.ReadUtf8Text()));
        output.SetAsset(new DependencyAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[StableTypeID("a80d363f-8e49-4615-89ee-589613b91c03")]
internal sealed class ImportGraphAsset : AssetObject;

[AssetImporter("inno.tests.import-dependency")]
internal sealed class ImportGraphAssetImporter : AssetImporter<ImportGraphAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".importgraph"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<ImportGraphAsset> output,
        CancellationToken cancellationToken
    ) {
        string dependency = context.ReadUtf8Text().Trim();
        if (!string.IsNullOrWhiteSpace(dependency))
        {
            output.DependsOnSource(AssetPath.Parse(dependency));
        }
        output.SetAsset(new ImportGraphAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[StableTypeID("490def27-4cdd-48a0-b324-9b7438600404")]
internal sealed class SlowAsset : AssetObject
{
    [SerializableProperty]
    internal string value { get; set; } = string.Empty;
}

[AssetImporter("inno.tests.slow")]
internal sealed class SlowAssetImporter : AssetImporter<SlowAsset>
{
    internal static readonly ManualResetEventSlim IMPORT_STARTED = new(false);
    internal static readonly ManualResetEventSlim ALLOW_IMPORT = new(false);
    internal static int importCount;

    public override IReadOnlyList<string> supportedExtensions { get; } = [".slowasset"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<SlowAsset> output,
        CancellationToken cancellationToken
    ) {
        Interlocked.Increment(ref importCount);
        IMPORT_STARTED.Set();
        if (!ALLOW_IMPORT.Wait(TimeSpan.FromSeconds(5)))
        {
            throw new TimeoutException("The slow importer test gate was not released.");
        }
        output.SetAsset(new SlowAsset { value = context.ReadUtf8Text() });
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }

    internal static void Reset()
    {
        importCount = 0;
        IMPORT_STARTED.Reset();
        ALLOW_IMPORT.Reset();
    }
}

[StableTypeID("fa17dc36-b9cc-4187-b5da-72906ad00505")]
internal sealed class MutableAsset : AssetObject
{
    [SerializableProperty]
    internal string value { get; set; } = string.Empty;
}

[AssetImporter("inno.tests.mutable")]
internal sealed class MutableAssetImporter : AssetImporter<MutableAsset>
{
    internal static int attempts;
    public override IReadOnlyList<string> supportedExtensions { get; } = [".mutableasset"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<MutableAsset> output,
        CancellationToken cancellationToken
    ) {
        attempts++;
        string value = context.ReadUtf8Text();
        if (value == "!invalid!")
        {
            throw new InvalidDataException("The mutable asset source is invalid.");
        }
        output.SetAsset(new MutableAsset { value = value });
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }

    protected override ValueTask<ReadOnlyMemory<byte>?> ExportAsync(
        AssetExportContext context,
        MutableAsset asset,
        CancellationToken cancellationToken
    ) => ValueTask.FromResult<ReadOnlyMemory<byte>?>(Encoding.UTF8.GetBytes(asset.value));
}

[StableTypeID("9f9f5f9f-4d86-414f-9ff9-78ad8ec60606")]
internal sealed class HookAsset : AssetObject
{
    internal int payloadChangeCount;
    internal int unloadingCount;
    internal int pendingUnloads;
    internal bool failUnloading;
    internal bool retirementExpired;

    protected override void OnRuntimePayloadChanged(
        ReadOnlyMemory<byte> previousPayload,
        ReadOnlyMemory<byte> currentPayload
    ) => Interlocked.Increment(ref payloadChangeCount);

    protected override void OnUnloading()
    {
        Interlocked.Increment(ref unloadingCount);
        if (retirementExpired)
        {
            throw new RetirementTimeoutException("The test asset work exceeded its retirement deadline.");
        }
        if (pendingUnloads > 0)
        {
            pendingUnloads--;
            throw new RetirementPendingException("The test asset still owns unfinished work.");
        }
        if (failUnloading)
        {
            throw new InvalidOperationException("The test asset release failed.");
        }
    }
}

[AssetImporter("inno.tests.hook")]
internal sealed class HookAssetImporter : AssetImporter<HookAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".hookasset"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<HookAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new HookAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

internal static class ImporterConflictProbe
{
    internal static bool duplicateExtension;
}

[StableTypeID("da675da1-9276-40c4-9964-0eb4b8ff9a07")]
internal sealed class ImporterConflictAsset : AssetObject;

[AssetImporter("inno.tests.conflict-a")]
internal sealed class ImporterConflictAssetImporterA : AssetImporter<ImporterConflictAsset>
{
    public override IReadOnlyList<string> supportedExtensions
        => ImporterConflictProbe.duplicateExtension
           ? [".conflict"]
           : [".probea"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<ImporterConflictAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new ImporterConflictAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[AssetImporter("inno.tests.conflict-b")]
internal sealed class ImporterConflictAssetImporterB : AssetImporter<ImporterConflictAsset>
{
    public override IReadOnlyList<string> supportedExtensions
        => ImporterConflictProbe.duplicateExtension
           ? [".conflict"]
           : [".probeb"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<ImporterConflictAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new ImporterConflictAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

[StableTypeID("8f87f452-203e-4cd4-9c91-c93af4802141")]
internal sealed class TestBuildDefinitionAsset : AssetObject
{
    [SerializableProperty]
    internal string label { get; set; } = string.Empty;
}

[AssetBuildProcessor("inno.tests.aggregate-build")]
internal sealed class TestBuildProcessor : AssetBuildProcessor<TestBuildDefinitionAsset>
{
    protected override ValueTask BuildAsync(
        AssetBuildContext<TestBuildDefinitionAsset> context,
        AssetArtifactWriter output,
        CancellationToken cancellationToken
    ) {
        string value = context.definition.label + ":" + context.inputs.Count;
        return output.WriteAsync("result", Encoding.UTF8.GetBytes(value), cancellationToken);
    }
}
