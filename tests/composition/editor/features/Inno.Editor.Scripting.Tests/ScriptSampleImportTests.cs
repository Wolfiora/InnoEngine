using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Execution;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Editor.Annotations;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.Plugins;
using Inno.Scene;
using Inno.Scene.Components;
using Inno.Scripting.Compiler;
using Inno.Scripting.Reload;
using Inno.Testing.Scripting;

namespace Inno.Editor.Scripting.Tests;

public sealed class ScriptSampleImportTests : ScriptingPipelineFixture
{
    [Fact]
    public void InstalledPluginSampleImportsAsAnImmediatelyCompilableWritableCopy()
    {
        using var fixture = new ScriptingFixture(WriteSamplePlugin);
        var source = new AssetPath(new AssetSourceID("tests.samples"), "~Starter");
        var sourceFile = new AssetPath(new AssetSourceID("tests.samples"), "~Starter/StarterBehavior.cs");
        Assert.True(fixture.assets.TryGetFileSystemEntry(source, out AssetFileEntry sample));
        Assert.True(sample.isReadOnly);
        Assert.True(sample.isSample);
        Assert.True(sample.isSampleContent);
        Assert.True(fixture.assets.TryGetFileSystemEntry(sourceFile, out AssetFileEntry sampleContent));
        Assert.True(sampleContent.isReadOnly);
        Assert.True(sampleContent.isSampleContent);
        Assert.True(fixture.assets.TryGetInfo(sourceFile, out AssetInfo? templateInfo));
        Guid sourcePersistentID = Assert.IsType<AssetInfo>(templateInfo).persistentID;
        ScriptCompilationResult authoring = fixture.Compile();
        Assert.True(authoring.success, FormatDiagnostics(authoring));
        Assert.Contains(authoring.compiledAssemblyNames, name => name.EndsWith(".Samples", StringComparison.Ordinal));
        Assert.DoesNotContain(
            authoring.runtimeAssemblyPaths,
            path => Path.GetFileNameWithoutExtension(path).EndsWith(".Samples", StringComparison.Ordinal)
        );
        ScriptCompilationResult player = fixture.CompileRuntimeDeployment();
        Assert.True(player.success, FormatDiagnostics(player));
        Assert.DoesNotContain(player.compiledAssemblyNames, name => name.EndsWith(".Samples", StringComparison.Ordinal));
        using AssetSampleImportTransaction import = PrepareSample(fixture, source);
        import.BeginValidation(async (
            sources,
            cancellationToken
        ) =>
        {
            ScriptCompilationResult candidate = await fixture.compiler.CompileAuthoringGenerationAsync(
                sourceSnapshot: sources,
                cancellationToken: cancellationToken
            );
            Assert.True(candidate.success, FormatDiagnostics(candidate));
        });
        Assert.True(SpinWait.SpinUntil(() => import.isValidationComplete, 60_000));
        import.Commit();
        AssetPath imported = import.target;
        Assert.Equal(AssetPath.Project("~Starter"), imported);
        Assert.True(File.Exists(Path.Combine(fixture.projectRoot, "Assets", "~Starter", "StarterBehavior.cs")));
        Assert.DoesNotContain(
            "ce3b52c6-2a07-42ea-b632-a307a0ef7407",
            File.ReadAllText(Path.Combine(fixture.projectRoot, "Assets", "~Starter", "StarterBehavior.cs")),
            StringComparison.OrdinalIgnoreCase
        );
        Assert.True(fixture.assets.TryGetInfo(AssetPath.Project("~Starter/StarterBehavior.cs"), out AssetInfo? importedInfo));
        Assert.NotEqual(sourcePersistentID, Assert.IsType<AssetInfo>(importedInfo).persistentID);
        Assert.True(fixture.assets.TryGetFileSystemEntry(imported, out AssetFileEntry importedDirectory));
        Assert.False(importedDirectory.isReadOnly);
        Assert.False(importedDirectory.isSample);
        Assert.False(importedDirectory.isSampleContent);
        ScriptCompilationResult compilation = fixture.Compile();
        Assert.True(compilation.success, FormatDiagnostics(compilation));
        fixture.compiler.GenerateProjectFiles();
        string gameProject = File.ReadAllText(Path.Combine(fixture.projectRoot, "Inno.GameScripts.csproj"));
        Assert.Contains("Compile Include=\"Assets/~Starter/StarterBehavior.cs\"", gameProject);
    }

    [Fact]
    public void InvalidSampleScriptPreflightLeavesNoProjectCopy()
    {
        using var fixture = new ScriptingFixture((
            root,
            serialization
        ) => WriteSamplePlugin(root, serialization, "public sealed class BrokenSample { this is invalid; }"));
        var source = new AssetPath(new AssetSourceID("tests.samples"), "~Starter");
        using AssetSampleImportTransaction import = PrepareSample(fixture, source);
        import.BeginValidation(async (
            sources,
            cancellationToken
        ) =>
        {
            ScriptCompilationResult candidate = await fixture.compiler.CompileAuthoringGenerationAsync(
                sourceSnapshot: sources,
                cancellationToken: cancellationToken
            );
            if (!candidate.success)
            {
                throw new InvalidOperationException(FormatDiagnostics(candidate));
            }
        });
        Assert.True(SpinWait.SpinUntil(() => import.isValidationComplete, 60_000));
        Assert.Throws<InvalidOperationException>(() => import.Commit());
        new RetirementBarrier("Test sample import").Wait(import.Rollback);
        Assert.False(Directory.Exists(Path.Combine(fixture.projectRoot, "Assets", "~Starter")));
        Assert.False(fixture.assets.TryGetFileSystemEntry(AssetPath.Project("~Starter"), out _));
    }

    [Fact]
    public void SampleImportPreservesEveryLeadingTildeAndRejectsDestinationCollisions()
    {
        var source = new AssetPath(new AssetSourceID("tests.samples"), "Samples/~~~Starter");
        Assert.Equal("~~~Starter", AssetSample.GetImportName(source));
        Assert.True(AssetSample.IsRuntimeExcluded(AssetPath.Project("~~~Starter/File.cs"), false));
        Assert.False(AssetSample.Contains(AssetPath.Project("~~~Starter/File.cs"), false));
        using var fixture = new ScriptingFixture(WriteSamplePlugin);
        fixture.assets.CreateDirectory(AssetPath.Project("~Starter"));
        Assert.Throws<IOException>(() => fixture.assets.PrepareSampleImport(new AssetPath(new AssetSourceID("tests.samples"), "~Starter")));
        fixture.host.modules.generations.EnsureReady("retry after rejected import");
    }

    [Fact]
    public void SampleValidationReturnsToOwnerWhileWorkIsPendingAndPublishesExactlyOnce()
    {
        using var fixture = new ScriptingFixture(WriteSamplePlugin);
        int notifications = 0;
        fixture.assets.changed += _ => notifications++;
        using AssetSampleImportTransaction import = PrepareSample(fixture, new AssetPath(new AssetSourceID("tests.samples"), "~Starter"));
        long revision = fixture.assets.revision;
        Assert.False(fixture.assets.TryGetFileSystemEntry(import.target, out _));
        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        import.BeginValidation((
            _,
            _
        ) => new ValueTask(completion.Task));
        for (int frame = 0; frame < 30; frame++)
        {
            fixture.assets.Update();
            Assert.False(import.isValidationComplete);
            Assert.Equal(0, notifications);
            Assert.Equal(revision, fixture.assets.revision);
        }

        Assert.Throws<InvalidOperationException>(() => import.Commit());
        Assert.Throws<InvalidOperationException>(() => fixture.assets.CreateDirectory(AssetPath.Project("Concurrent")));
        Assert.Throws<InvalidOperationException>(() => fixture.host.modules.generations.EnsureReady("reload while importing"));
        completion.SetResult();
        Assert.True(SpinWait.SpinUntil(() => import.isValidationComplete, 60_000));
        import.Commit();
        Assert.Equal(1, notifications);
        Assert.Equal(revision + 1, fixture.assets.revision);
        Assert.Throws<InvalidOperationException>(() => import.Commit());
        fixture.host.modules.generations.EnsureReady("reload after import");
    }

    [Fact]
    public void SampleCancellationRetainsDependenciesUntilUncooperativeValidationDrains()
    {
        using var fixture = new ScriptingFixture(WriteSamplePlugin);
        int notifications = 0;
        fixture.assets.changed += _ => notifications++;
        using AssetSampleImportTransaction import = PrepareSample(fixture, new AssetPath(new AssetSourceID("tests.samples"), "~Starter"));
        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        import.BeginValidation((
            _,
            _
        ) => new ValueTask(completion.Task));
        import.Cancel();
        Assert.Throws<RetirementPendingException>(() => import.Rollback());
        Assert.Throws<RetirementPendingException>(() => fixture.assets.Dispose());
        Assert.True(fixture.assets.isInitialized);
        Assert.Throws<InvalidOperationException>(() => fixture.host.modules.generations.EnsureReady("reload before drain"));
        completion.SetResult();
        Assert.True(SpinWait.SpinUntil(() => import.isValidationComplete, 60_000));
        new RetirementBarrier("Test sample import").Wait(import.Rollback);
        Assert.False(fixture.assets.TryGetFileSystemEntry(import.target, out _));
        Assert.False(File.Exists(Path.Combine(fixture.projectRoot, "Assets", "~Starter.imeta")));
        Assert.Equal(0, notifications);
        fixture.host.modules.generations.EnsureReady("reload after canceled work drains");
    }

    [Fact]
    public void SampleHistoryFinalizationFailureRollsBackWithoutPublication()
    {
        using var fixture = new ScriptingFixture(WriteSamplePlugin);
        int notifications = 0;
        fixture.assets.changed += _ => notifications++;
        using AssetSampleImportTransaction import = PrepareSample(fixture, new AssetPath(new AssetSourceID("tests.samples"), "~Starter"));
        import.BeginValidation((
            _,
            _
        ) => ValueTask.CompletedTask);
        Assert.True(SpinWait.SpinUntil(() => import.isValidationComplete, 60_000));
        Assert.Throws<IOException>(() => import.Commit(_ => throw new IOException("History spill failed.")));
        new RetirementBarrier("Test sample import").Wait(import.Rollback);
        Assert.Equal(0, notifications);
        Assert.False(fixture.assets.TryGetFileSystemEntry(import.target, out _));
        fixture.host.modules.generations.EnsureReady("continue after finalization failure");
    }

    [Fact]
    public void SampleCopyCancellationLeavesNoProjectOrTransactionDirectory()
    {
        using var fixture = new ScriptingFixture(WriteSamplePlugin);
        using AssetSampleImportTransaction import = fixture.assets.PrepareSampleImport(
            new AssetPath(new AssetSourceID("tests.samples"), "~Starter")
        );
        import.Cancel();
        Assert.True(
            SpinWait.SpinUntil(() =>
            {
                try
                {
                    import.Rollback();
                    return true;
                }
                catch (RetirementPendingException)
                {
                    return false;
                }
            }, 15_000)
        );
        Assert.False(Directory.Exists(Path.Combine(fixture.projectRoot, "Assets", "~Starter")));
        string transactions = Path.Combine(fixture.assets.libraryRoot, "AssetDatabase", "Transactions");
        Assert.True(!Directory.Exists(transactions) || !Directory.EnumerateFileSystemEntries(transactions).Any());
        fixture.host.modules.generations.EnsureReady("continue after copy cancellation");
    }

    [Fact]
    public void PendingAutomaticCompilationDefersUntilSampleRollback()
    {
        using var fixture = new ScriptingFixture(WriteSamplePlugin);
        using ScriptReloadHost reload = fixture.CreateReloadHost(autoCompile: true);
        reload.Start();
        using AssetSampleImportTransaction import = fixture.assets.PrepareSampleImport(
            new AssetPath(new AssetSourceID("tests.samples"), "~Starter")
        );
        Assert.False(reload.TryCompilePending(out Task<ScriptCompilationResult>? pending));
        Assert.Null(pending);
        Assert.True(reload.isCompilationPending);
        import.Cancel();
        Assert.True(
            SpinWait.SpinUntil(() =>
            {
                try
                {
                    import.Rollback();
                    return true;
                }
                catch (RetirementPendingException)
                {
                    return false;
                }
            }, 15_000)
        );
        Assert.True(reload.TryCompilePending(out Task<ScriptCompilationResult>? compilation));
        Assert.True(SpinWait.SpinUntil(() => compilation!.IsCompleted, 15_000));
        Assert.True(compilation!.IsCompletedSuccessfully);
        Assert.True(reload.lastCompilation!.success);
    }

    [Fact]
    public void EditorSampleImportUsesSharedModalAndUndoRedo()
    {
        using var fixture = new ScriptingFixture(WriteSamplePlugin);
        using EditorInteractionRuntime runtime = fixture.CreateEditorRuntime();
        runtime.Start();
        IEditorScriptCompilation scripting = Assert.IsAssignableFrom<IEditorScriptCompilation>(ScriptingCompilationProbe.compilation);
        WaitForInitialScriptCompilation(fixture, runtime, scripting);
        Assert.True(
            fixture.assets.TryGetFileSystemEntry(new AssetPath(new AssetSourceID("tests.samples"), "~Starter"), out AssetFileEntry sample)
        );
        Assert.True(runtime.interactions.For("panel/asset.file-browser", sample).Execute("file-browser/import-sample"));
        EditorModalExtension modal = runtime.modals.Single(value => value.id == "asset-sample-import.progress");
        Assert.True(modal.TryGetPresentation(out EditorModalExtension.Presentation presentation));
        Assert.True(presentation.isVisible);
        Assert.True(presentation.blocksInteraction);
        Assert.False(presentation.canMove);
        EditorModalExtension compilationModal = runtime.modals.Single(value => value.id == "scripting.compilation");
        Assert.True(compilationModal.TryGetPresentation(out EditorModalExtension.Presentation compilationPolicy));
        Assert.Equal(compilationPolicy.allowScrolling, presentation.allowScrolling);
        PumpEditorUntil(
            fixture,
            runtime,
            () => modal.TryGetPresentation(out EditorModalExtension.Presentation state) && !state.isVisible,
            () => scripting.progress.status
        );
        Assert.True(fixture.assets.TryGetFileSystemEntry(AssetPath.Project("~Starter"), out _));
        Assert.True(runtime.interactions.history.Undo().succeeded);
        Assert.False(fixture.assets.TryGetFileSystemEntry(AssetPath.Project("~Starter"), out _));
        Assert.True(runtime.interactions.history.Redo().succeeded);
        Assert.True(fixture.assets.TryGetFileSystemEntry(AssetPath.Project("~Starter"), out _));
    }

    [Fact]
    public void SampleTransformRunsOnWorkerAndEditorStopRetainsPendingImport()
    {
        using var fixture = new ScriptingFixture(WriteSamplePlugin);
        using EditorInteractionRuntime runtime = fixture.CreateEditorRuntime();
        runtime.Start();
        IEditorScriptCompilation scripting = Assert.IsAssignableFrom<IEditorScriptCompilation>(ScriptingCompilationProbe.compilation);
        WaitForInitialScriptCompilation(fixture, runtime, scripting);
        using var control = new ControlledSampleSourceRewriter.Control();
        ControlledSampleSourceRewriter.current = control;
        try
        {
            Assert.True(
                fixture.assets.TryGetFileSystemEntry(
                    new AssetPath(new AssetSourceID("tests.samples"), "~Starter"),
                    out AssetFileEntry sample
                )
            );
            Assert.True(runtime.interactions.For("panel/asset.file-browser", sample).Execute("file-browser/import-sample"));
            Assert.True(control.started.Wait(TimeSpan.FromSeconds(5)));
            Assert.NotEqual(Environment.CurrentManagedThreadId, control.workerThread);
            for (int frame = 0; frame < 30; frame++)
            {
                runtime.Update(new EditorFrame(1f / 60f, frame / 60f, true));
            }
            Assert.False(fixture.assets.TryGetFileSystemEntry(AssetPath.Project("~Starter"), out _));
            Task shutdownObserver = Task.Run(() =>
            {
                try
                {
                    Assert.True(control.canceled.Wait(TimeSpan.FromSeconds(5)));
                    Assert.True(fixture.assets.isInitialized);
                    Assert.Throws<InvalidOperationException>(
                        () => fixture.host.modules.generations.EnsureReady("retire scripting before sample work")
                    );
                }
                finally
                {
                    control.release.Set();
                }
            });
            runtime.Dispose();
            Assert.True(SpinWait.SpinUntil(() => shutdownObserver.IsCompleted, 15_000));
            Assert.True(shutdownObserver.IsCompletedSuccessfully, shutdownObserver.Exception?.ToString());
        }
        finally
        {
            control.release.Set();
            ControlledSampleSourceRewriter.current = null;
            Assert.True(
                SpinWait.SpinUntil(() =>
                {
                    try
                    {
                        runtime.Dispose();
                        return true;
                    }
                    catch (RetirementPendingException)
                    {
                        return false;
                    }
                }, 15_000)
            );
        }

        Assert.False(Directory.Exists(Path.Combine(fixture.projectRoot, "Assets", "~Starter")));
        fixture.host.modules.generations.Wait();
        fixture.host.modules.generations.EnsureReady("continue after editor sample retirement");
    }

    [Fact]
    public void SampleDirectoryMetadataRemapsNestedImportSettingsIdentities()
    {
        Guid sourceID = Guid.Parse("75f6a70b-93b2-47f0-8747-cc359474b7a3");
        using var fixture = new ScriptingFixture((
            root,
            serialization
        ) =>
        {
            WriteSamplePlugin(root, serialization);
            string package = Path.Combine(root, "Plugins", "samples.iplugin");
            using ZipArchive archive = ZipFile.Open(package, ZipArchiveMode.Update);
            archive.GetEntry("Assets/~Starter.imeta")!.Delete();
            WritePluginPackageEntry(
                archive,
                "Assets/~Starter.imeta",
                serialization.Serialize(
                    new ScriptingAssetSourceMeta
                    {
                        persistentId = Guid.Parse("7726b1d2-9aee-4d2c-a865-2fd53155095f"),
                        sourceKind = (int)AssetSourceKind.Directory,
                        importerSettingsBytes = serialization.Serialize(new SampleIdentitySettings { referenceId = sourceID })
                    }
                )
            );
        });
        using AssetSampleImportTransaction import = PrepareSample(fixture, new AssetPath(new AssetSourceID("tests.samples"), "~Starter"));
        import.BeginValidation((
            sources,
            _
        ) =>
        {
            Assert.True(sources.TryGetInfo(AssetPath.Project("~Starter/StarterBehavior.cs"), out AssetInfo? info));
            ScriptingAssetSourceMeta metadata = fixture.host.serialization.Deserialize<ScriptingAssetSourceMeta>(
                File.ReadAllBytes(Path.Combine(fixture.projectRoot, "Assets", "~Starter.imeta"))
            );
            SampleIdentitySettings settings = fixture.host.serialization.Deserialize<SampleIdentitySettings>(
                metadata.importerSettingsBytes
            );
            Assert.Equal(info!.persistentID, settings.referenceId);
            Assert.NotEqual(sourceID, settings.referenceId);
            return ValueTask.CompletedTask;
        });
        Assert.True(SpinWait.SpinUntil(() => import.isValidationComplete, 60_000));
        import.Commit();
    }

    [Fact]
    public void SampleAssetPreimportRunsOnWorkerAndReceivesCancellation()
    {
        using var fixture = new ScriptingFixture((
            root,
            serialization
        ) =>
        {
            WriteSamplePlugin(root, serialization);
            using ZipArchive package = ZipFile.Open(Path.Combine(root, "Plugins", "samples.iplugin"), ZipArchiveMode.Update);
            WritePluginPackageEntry(package, "Assets/~Starter/Slow.samplebusy", Encoding.UTF8.GetBytes("sample"));
            WritePluginPackageEntry(
                package,
                "Assets/~Starter/Slow.samplebusy.imeta",
                serialization.Serialize(
                    new ScriptingAssetSourceMeta
                    {
                        persistentId = Guid.Parse("587849fc-777c-4501-a1b2-ec042ca40314"),
                        sourceKind = (int)AssetSourceKind.File,
                        importerId = "tests.scripting.controlled-sample-asset"
                    }
                )
            );
        });
        using var control = new ControlledSampleAssetImporter.Control();
        ControlledSampleAssetImporter.current = control;
        AssetSampleImportTransaction import = fixture.assets.PrepareSampleImport(
            new AssetPath(new AssetSourceID("tests.samples"), "~Starter")
        );
        try
        {
            Assert.True(
                SpinWait.SpinUntil(() =>
                {
                    Assert.False(import.Advance());
                    return control.started.IsSet;
                }, 15_000)
            );
            Assert.NotEqual(Environment.CurrentManagedThreadId, control.workerThread);
            long revision = fixture.assets.revision;
            for (int frame = 0; frame < 30; frame++)
            {
                fixture.assets.Update();
                Assert.False(import.Advance());
                Assert.False(fixture.assets.TryGetFileSystemEntry(import.target, out _));
                Assert.Equal(revision, fixture.assets.revision);
            }

            import.Cancel();
            Assert.True(control.canceled.Wait(TimeSpan.FromSeconds(5)));
        }
        finally
        {
            new RetirementBarrier("Test sample preimport").Wait(import.Dispose);
            ControlledSampleAssetImporter.current = null;
        }

        Assert.False(Directory.Exists(Path.Combine(fixture.projectRoot, "Assets", "~Starter")));
        Assert.False(fixture.assets.TryGetFileSystemEntry(import.target, out _));
        fixture.host.modules.generations.EnsureReady("continue after asset preimport cancellation");
    }

    [Fact]
    public void SampleCompilerReferencePreparationRunsOnWorker()
    {
        using var fixture = new ScriptingFixture(WriteSamplePlugin);
        using AssetSampleImportTransaction import = PrepareSample(fixture, new AssetPath(new AssetSourceID("tests.samples"), "~Starter"));
        using var progress = new CompilationGate();
        try
        {
            import.BeginValidation(async (
                sources,
                cancellationToken
            ) =>
            {
                ScriptCompilationResult result = await fixture.compiler.CompileAuthoringGenerationAsync(
                    progress,
                    sources,
                    cancellationToken
                ).ConfigureAwait(false);
                Assert.True(result.success, FormatDiagnostics(result));
            });
            Assert.True(progress.started.Wait(TimeSpan.FromSeconds(5)));
            Assert.NotEqual(Environment.CurrentManagedThreadId, progress.workerThread);
            for (int frame = 0; frame < 30; frame++)
            {
                fixture.assets.Update();
                Assert.False(import.isValidationComplete);
                Assert.False(fixture.assets.TryGetFileSystemEntry(import.target, out _));
            }
        }
        finally
        {
            progress.release.Set();
            Assert.True(SpinWait.SpinUntil(() => import.isValidationComplete, 60_000));
        }

        import.Commit();
        Assert.True(fixture.assets.TryGetFileSystemEntry(import.target, out _));
    }

    /// <summary>
    /// Verifies nonblocking watcher suspension and reconciliation after successful or canceled imports.
    /// </summary>
    /// <param name = "commit">
    /// Whether the candidate is committed rather than rolled back.
    /// </param>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SampleIndexingDoesNotWaitForWatcherQuietPeriod(bool commit)
    {
        using var fixture = new ScriptingFixture(
            WriteSamplePlugin,
            enableFileSystemWatcher: true,
            fileWatcherFlushDelayMs: 3000
        );
        AssetSampleImportTransaction import = fixture.assets.PrepareSampleImport(
            new AssetPath(new AssetSourceID("tests.samples"), "~Starter")
        );
        try
        {
            TimeSpan maximumAdvance = TimeSpan.Zero;
            bool indexed = SpinWait.SpinUntil(() =>
            {
                Stopwatch frame = Stopwatch.StartNew();
                bool ready = import.Advance();
                if (frame.Elapsed > maximumAdvance)
                {
                    maximumAdvance = frame.Elapsed;
                }
                return ready;
            }, 15_000);
            Assert.True(indexed);
            Assert.True(
                maximumAdvance < TimeSpan.FromSeconds(2),
                $"Owner-thread Advance waited {maximumAdvance} with a three-second watcher quiet period."
            );
            fixture.Write("External.txt", "Changed while watching was paused.");
            Assert.False(fixture.assets.TryGetFileSystemEntry(AssetPath.Project("External.txt"), out _));
            if (commit)
            {
                import.BeginValidation(static (
                    _,
                    _
                ) => ValueTask.CompletedTask);
                Assert.True(import.isValidationComplete);
                import.Commit();
            }
            else
            {
                new RetirementBarrier("Test canceled watched sample").Wait(import.Rollback);
            }
            fixture.assets.Update();
            Assert.True(fixture.assets.TryGetFileSystemEntry(AssetPath.Project("External.txt"), out _));
            Assert.Equal(commit, fixture.assets.TryGetFileSystemEntry(AssetPath.Project("~Starter"), out _));
        }
        finally
        {
            new RetirementBarrier("Test watched sample import").Wait(import.Dispose);
        }
    }

    private static AssetSampleImportTransaction PrepareSample(
        ScriptingFixture fixture,
        AssetPath source
    ) {
        AssetSampleImportTransaction import = fixture.assets.PrepareSampleImport(source);
        Assert.True(SpinWait.SpinUntil(() => import.Advance(), 15_000));
        return import;
    }

    private static void WaitForInitialScriptCompilation(
        ScriptingFixture fixture,
        EditorInteractionRuntime runtime,
        IEditorScriptCompilation scripting
    ) {
        // Startup compiles the sample and activates its generation before the import can begin.
        PumpEditorUntil(
            fixture,
            runtime,
            () => scripting.state is EditorScriptCompilationState.Ready or EditorScriptCompilationState.Failed,
            DescribeCompilation,
            timeoutMilliseconds: 60_000
        );
        Assert.True(scripting.state == EditorScriptCompilationState.Ready, DescribeCompilation());

        string DescribeCompilation()
        {
            string diagnostics = scripting.lastCompilation is { } result ? FormatDiagnostics(result) : string.Empty;
            return $"{scripting.state}: {scripting.progress.status}{Environment.NewLine}{diagnostics}";
        }
    }

    private static void WriteSamplePlugin(
        string projectRoot,
        SerializationRegistry serialization
    ) {
        const string C_SOURCE = "using InnoEngine.Reflection; using InnoEngine.Scene; "
                                + "[StableTypeID(\"ce3b52c6-2a07-42ea-b632-a307a0ef7407\")] "
                                + "public sealed class StarterBehavior : GameBehavior { }";
        WriteSamplePlugin(projectRoot, serialization, C_SOURCE);
    }

    private static void WriteSamplePlugin(
        string projectRoot,
        SerializationRegistry serialization,
        string source
    ) => WritePluginPackage(
        projectRoot,
        "samples.iplugin",
        serialization,
        new PluginManifest { pluginId = "tests.samples", displayName = "Sample Test Plugin" },
        new Dictionary<string, byte[]>
        {
            ["Assets/"] = [],
            ["Assets/~Starter/"] = [],
            ["Assets/~Starter.imeta"] = serialization.Serialize(
                new ScriptingAssetSourceMeta
                {
                    persistentId = Guid.Parse("7726b1d2-9aee-4d2c-a865-2fd53155095f"),
                    sourceKind = (int)AssetSourceKind.Directory
                }
            ),
            ["Assets/~Starter/StarterBehavior.cs"] = Encoding.UTF8.GetBytes(source),
            ["Assets/~Starter/StarterBehavior.cs.imeta"] = CreateScriptSourceMeta(
                serialization,
                Guid.Parse("75f6a70b-93b2-47f0-8747-cc359474b7a3")
            )
        }
    );

    private sealed class CompilationGate : IProgress<ScriptCompilationProgress>, IDisposable
    {
        /// <summary>
        /// Suspends reference preparation to verify that no Editor frame waits for the compiler.
        /// </summary>
        /// <param name = "value">
        /// The real compiler progress callback.
        /// </param>
        public void Report(ScriptCompilationProgress value)
        {
            if (value.stage != "Building the script API profile...")
            {
                return;
            }
            workerThread = Environment.CurrentManagedThreadId;
            started.Set();
            if (!release.Wait(TimeSpan.FromSeconds(15)))
            {
                throw new TimeoutException("The test-owned compilation gate was not released.");
            }
        }

        /// <summary>
        /// Releases the test-owned synchronization handles after compilation drains.
        /// </summary>
        public void Dispose()
        {
            started.Dispose();
            release.Dispose();
        }

        internal ManualResetEventSlim started { get; } = new();
        internal ManualResetEventSlim release { get; } = new();
        internal int workerThread { get; private set; }
    }
}
