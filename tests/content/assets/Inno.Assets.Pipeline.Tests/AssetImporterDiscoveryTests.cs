using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Content;
using Inno.Core.Diagnostics;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.References;
using Inno.Testing.Content;
using Inno.Testing.Diagnostics;

namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetImporterDiscoveryTests : AssetLoaderFixture
{
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
    public void ExtensionContractsUseImplementationIdentityWithoutManualVersions()
    {
        Assert.Null(typeof(AssetImporter).GetProperty("version"));
        Assert.Null(typeof(AssetBuildProcessor).GetProperty("version"));
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
}
