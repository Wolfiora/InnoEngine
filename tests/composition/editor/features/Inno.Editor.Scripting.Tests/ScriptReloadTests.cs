using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Inno.Assets;
using Inno.Core.Diagnostics;
using Inno.Core.IO;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.PlayMode;
using Inno.Editor.Scene;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.Plugins;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scene.Components;
using Inno.Scripting.Compiler;
using Inno.Scripting.Reload;

namespace Inno.Editor.Scripting.Tests;

public sealed class ScriptReloadTests : ScriptingPipelineFixture
{
    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void SuccessfulReloadActivatesCandidateAndFailedCompilationRetainsIt()
    {
        m_fixture.WriteVersionedBehavior(1);
        using ScriptReloadHost reload = m_fixture.CreateReloadHost();
        reload.Start();
        ScriptCompilationResult firstResult = m_fixture.CompilePending(reload);
        Assert.True(firstResult.success, FormatDiagnostics(firstResult));
        Assert.True(reload.ApplyPendingReload());
        WeakReference active = CaptureActiveType(m_fixture, "VersionedBehavior", expectedVersion: 1);
        for (int version = 2; version <= 11; version++)
        {
            m_fixture.Write("VersionedBehavior.cs", "using InnoEngine.Scene; public sealed class VersionedBehavior : GameBehavior {");
            ScriptCompilationResult failed = m_fixture.CompilePending(reload);
            Assert.False(failed.success);
            Assert.False(reload.ApplyPendingReload());
            AssertRetainedActiveType(m_fixture, active, version - 1);

            m_fixture.WriteVersionedBehavior(version);
            ScriptCompilationResult replacement = m_fixture.CompilePending(reload);
            Assert.True(replacement.success, FormatDiagnostics(replacement));
            Assert.True(reload.ApplyPendingReload());
            CompleteUnloadVerification(reload);
            Assert.False(active.IsAlive, "The previous script generation remained reachable after its reload barrier.");
            active = CaptureActiveType(m_fixture, "VersionedBehavior", version);
        }
    }

    [Fact]
    public void EditorSourceChangeRetiresTheWholeProjectAuthoringGeneration()
    {
        m_fixture.WriteVersionedBehavior(1);
        m_fixture.Write("Editor/GenericPresentation.editor.cs", """
            public class GenericPresentation<T> { public int version => 1; }
            public sealed class ProjectPresentation : GenericPresentation<VersionedBehavior> { }
            """);
        using ScriptReloadHost reload = m_fixture.CreateReloadHost();
        reload.Start();
        Assert.True(m_fixture.CompilePending(reload).success);
        Assert.True(reload.ApplyPendingReload());
        WeakReference runtime = CaptureActiveType(m_fixture, "VersionedBehavior", 1);
        WeakReference editor = CaptureActiveType(m_fixture, "ProjectPresentation", 1);
        m_fixture.Write("Editor/GenericPresentation.editor.cs", """
            public class GenericPresentation<T> { public int version => 2; }
            public sealed class ProjectPresentation : GenericPresentation<VersionedBehavior> { }
            """);
        ScriptCompilationResult candidate = m_fixture.CompilePending(reload);
        Assert.True(candidate.success, FormatDiagnostics(candidate));
        Assert.True(reload.ApplyPendingReload());
        CompleteUnloadVerification(reload);
        Assert.False(runtime.IsAlive);
        Assert.False(editor.IsAlive);
        Assert.Equal(1, ReadVersion(m_fixture.ResolveActiveType("VersionedBehavior")));
        Assert.Equal(2, ReadVersion(m_fixture.ResolveActiveType("ProjectPresentation")));
    }

    [Fact]
    public void SceneReloadParticipantSurvivesCollectionAndMigratesLiveScriptComponents()
    {
        m_fixture.Write("ReloadableBehavior.cs", """
            using InnoEngine.Scene;
            using InnoEngine.Serialization;

            public sealed class ReloadableBehavior : GameBehavior
            {
                [SerializableProperty]
                public int retained { get; set; } = 7;
            }
            """);
        var reloads = new EditorReloadCoordinator();
        using ScriptReloadHost reload = m_fixture.CreateReloadHost(reloads);
        reload.Start();
        ScriptCompilationResult firstResult = m_fixture.CompilePending(reload);
        Assert.True(firstResult.success, FormatDiagnostics(firstResult));
        Assert.True(reload.ApplyPendingReload());
        using EditorInteractionRuntime runtime = m_fixture.CreateEditorRuntime(reloads);
        runtime.Start();
        _ = Assert.IsAssignableFrom<IEditorSceneWorkspace>(SceneReloadProbe.workspace);
        using IDisposable sceneScope = m_fixture.editorSession.EnterExecutionScope();
        GameScene scene = m_fixture.editorSession.scenes.LoadNewSceneAdditive("Reload Test");
        GameObject gameObject = scene.CreateObject("Reload Target");
        RetiredScriptObject retired = AttachReloadableBehavior(m_fixture, gameObject);
        ForceFullCollection();
        m_fixture.Write("ReloadableBehavior.cs", """
            using InnoEngine.Scene;
            using InnoEngine.Serialization;

            public sealed class ReloadableBehavior : GameBehavior
            {
                [SerializableProperty]
                public int retained { get; set; } = 7;

                [SerializableProperty]
                public int added { get; set; } = 2;
            }
            """);
        ScriptCompilationResult secondResult = m_fixture.CompilePending(reload);
        Assert.True(secondResult.success, FormatDiagnostics(secondResult));
        Assert.True(reload.ApplyPendingReload());
        Type currentType = m_fixture.ResolveActiveType("ReloadableBehavior");
        GameComponent current = Assert.Single(
            gameObject
                .GetComponents()
                .Where(component => string.Equals(component.GetType().Name, "ReloadableBehavior", StringComparison.Ordinal))
        );
        Assert.Same(currentType, current.GetType());
        Assert.Equal(41, currentType.GetProperty("retained")!.GetValue(current));
        Assert.Equal(2, currentType.GetProperty("added")!.GetValue(current));
        CompleteUnloadVerification(reload);
        Assert.False(retired.component.IsAlive, "The retired script component remained reachable.");
        Assert.False(retired.type.IsAlive, "The retired script type remained reachable.");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    [Trait("Acceptance", "Lifecycle")]
    public void RemovedOrBrokenUpdatedPluginCommitsUnavailableGenerationAndRecovers(bool updateInPlace)
    {
        using var fixture = new ScriptingFixture(WriteUnavailableGenerationPlugin);
        fixture.Write("DependentBehavior.cs", """
            using InnoEngine.Reflection;
            using InnoEngine.Scene;
            using InnoEngine.Serialization;
            using UnavailabilityPlugin;

            [StableTypeID("69e8df54-51f2-40fd-9b47-c4ebdf18052e")]
            public sealed class DependentBehavior : GameBehavior
            {
                [SerializableProperty]
                public int retained { get; set; } = 11;

                public PluginBehavior? plugin { get; set; }
            }
            """);
        var reloads = new EditorReloadCoordinator();
        using ScriptReloadHost reload = fixture.CreateReloadHost(reloads);
        reload.Start();
        ScriptCompilationResult initial = fixture.CompilePending(reload);
        Assert.True(initial.success, FormatDiagnostics(initial));
        Assert.True(reload.ApplyPendingReload());
        RuntimeSession session = fixture.CreateEditorSession();
        using IDisposable executionScope = session.EnterExecutionScope();
        var sceneReload = new SceneReloadService(session.scenes, fixture.host.serialization, fixture.assets);
        using IDisposable registration = reloads.Register(new TestSceneReloadParticipant(sceneReload));
        GameScene scene = session.scenes.LoadNewScene("Plugin Availability");
        AvailabilitySceneState state = CreateAvailabilitySceneState(fixture, scene);
        string installedPlugin = Path.Combine(fixture.projectRoot, "Plugins", "unavailability.iplugin");
        string detachedPlugin = Path.Combine(fixture.projectRoot, "UnavailabilityPlugin.detached");
        byte[] validPluginPackage = File.ReadAllBytes(installedPlugin);
        for (int cycle = 0; cycle < 10; cycle++)
        {
            if (updateInPlace)
            {
                WriteUnavailableGenerationPlugin(
                    fixture.projectRoot,
                    fixture.host.serialization,
                    "this Plugin update is deliberately invalid C#"
                );
            }
            else
            {
                File.Move(installedPlugin, detachedPlugin);
            }

            Assert.True(fixture.RefreshPlugins());
            ScriptCompilationResult unavailable = fixture.CompilePendingPluginReload(reload);
            Assert.False(unavailable.success);
            Assert.Contains(unavailable.diagnostics, static diagnostic => diagnostic.severity == DiagnosticSeverity.Error);
            Assert.True(reload.ApplyPendingReload());
            Assert.DoesNotContain(
                fixture.host.modules.modules,
                static module => module.domain is AssemblyDomain.InnoPlugin or AssemblyDomain.InnoScripting
            );
            Assert.Equal(updateInPlace ? 1 : 0, fixture.activePlugins.Count);
            MissingGameComponent missingPlugin = Assert.IsType<MissingGameComponent>(
                state.pluginOwner.GetComponents().Single(component => component is not Transform)
            );
            MissingGameComponent missingScript = Assert.IsType<MissingGameComponent>(
                state.scriptOwner.GetComponents().Single(component => component is not Transform)
            );
            Assert.Equal(state.pluginComponentID, missingPlugin.identity.persistentID);
            Assert.Equal(state.scriptComponentID, missingScript.identity.persistentID);
            Assert.Equal("UnavailabilityPlugin.PluginBehavior", missingPlugin.missingTypeName);
            Assert.Equal("DependentBehavior", missingScript.missingTypeName);
            CompleteUnloadVerification(reload);
            if (updateInPlace)
            {
                File.WriteAllBytes(installedPlugin, validPluginPackage);
            }
            else
            {
                File.Move(detachedPlugin, installedPlugin);
            }
            Assert.True(fixture.RefreshPlugins());
            ScriptCompilationResult recoveredCompilation = fixture.CompilePendingPluginReload(reload);
            Assert.True(recoveredCompilation.success, FormatDiagnostics(recoveredCompilation));
            Assert.True(reload.ApplyPendingReload());
            AssertRecoveredAvailability(state);
            CompleteUnloadVerification(reload);
            Assert.Equal(GenerationState.Ready, fixture.host.modules.generations.state);
        }
    }

    [Fact]
    public void NeutralSettingsCacheDoesNotPinRetiredScriptTypesWithoutAnExplicitRebuild()
    {
        using var fixture = new ScriptingFixture();
        WriteSetting(17);
        using ScriptReloadHost reload = fixture.CreateReloadHost();
        reload.Start();
        ScriptCompilationResult initial = fixture.CompilePending(reload);
        Assert.True(initial.success, FormatDiagnostics(initial));
        Assert.True(reload.ApplyPendingReload());
        using var detachedSettings = new ProjectSettingsStore(
            new FileByteDocumentStore(Path.GetFullPath(Path.Combine(fixture.projectRoot, "DetachedSettings.inno"))),
            fixture.host.types,
            fixture.host.serialization,
            new ProjectID("tests.detached.settings"),
            AssetSerializationContext.Create(fixture.assets)
        );
        WeakReference previousType = CaptureCachedSettingType(detachedSettings);
        WriteSetting(21);
        ScriptCompilationResult replacement = fixture.CompilePending(reload);
        Assert.True(replacement.success, FormatDiagnostics(replacement));
        Assert.True(reload.ApplyPendingReload());
        CompleteUnloadVerification(reload);
        Assert.False(previousType.IsAlive, "A neutral settings cache must not retain the old script type.");
        Assert.True(CaptureCachedSettingType(detachedSettings).IsAlive);
        void WriteSetting(int value)
            => fixture.Write("DetachedSetting.cs", $$"""
                using InnoEngine.Reflection;
                using InnoEngine.Serialization;
                using InnoEngine.Settings;
                [StableTypeID("109af64a-a07e-4f84-8b86-7b06a0bce526")]
                [ProjectSettingDefinition("tests.detached.setting")]
                public sealed class DetachedSetting : ISerializable
                {
                    [SerializableProperty] public int value { get; set; } = {{value}};
                }
                """);
    }

    [Fact]
    public void AuthoringRuntimeSessionDoesNotPinRetiredPluginAndScriptContexts()
    {
        using var fixture = new ScriptingFixture(WriteUnavailableGenerationPlugin);
        var reloads = new EditorReloadCoordinator();
        using ScriptReloadHost reload = fixture.CreateReloadHost(reloads);
        reload.Start();
        ScriptCompilationResult initial = fixture.CompilePending(reload);
        Assert.True(initial.success, FormatDiagnostics(initial));
        Assert.True(reload.ApplyPendingReload());
        _ = fixture.CreateEditorSession();
        ScriptCompilationResult replacement = fixture.CompilePendingPluginReload(reload);
        Assert.True(replacement.success, FormatDiagnostics(replacement));
        Assert.True(reload.ApplyPendingReload());
        CompleteUnloadVerification(reload);
    }

    [Fact]
    public void PluginRemovalUnloadsTheCommittedMissingGenerationWithoutASecondReload()
    {
        using var fixture = new ScriptingFixture(WriteUnavailableGenerationPlugin);
        var reloads = new EditorReloadCoordinator();
        using ScriptReloadHost reload = fixture.CreateReloadHost(reloads);
        reload.Start();
        ScriptCompilationResult initial = fixture.CompilePending(reload);
        Assert.True(initial.success, FormatDiagnostics(initial));
        Assert.True(reload.ApplyPendingReload());
        RuntimeSession session = fixture.CreateEditorSession();
        using IDisposable executionScope = session.EnterExecutionScope();
        var sceneReload = new SceneReloadService(session.scenes, fixture.host.serialization, fixture.assets);
        using IDisposable registration = reloads.Register(new TestSceneReloadParticipant(sceneReload));
        MissingGenerationExpectation expectation = CommitPluginRemoval(fixture, reload, session);
        CompleteUnloadVerification(reload);
        Assert.False(expectation.retiredComponent.IsAlive, "The retired Plugin component remained strongly reachable.");
        Assert.False(expectation.retiredType.IsAlive, "The retired Plugin runtime Type remained strongly reachable.");
        Assert.DoesNotContain(fixture.host.modules.modules, static module => module.domain == AssemblyDomain.InnoPlugin);
        MissingGameComponent missing = Assert.IsType<MissingGameComponent>(
            expectation.owner.GetComponents().Single(component => component is not Transform)
        );
        Assert.Equal(expectation.componentID, missing.identity.persistentID);
        Assert.Equal("UnavailabilityPlugin.PluginBehavior", missing.missingTypeName);
    }

    [Fact]
    public void PluginReloadWaitsForPlaySessionRetirementBeforeActivatingItsAssemblyGeneration()
    {
        using var fixture = new ScriptingFixture(WriteUnavailableGenerationPlugin);
        var reloads = new EditorReloadCoordinator();
        using ScriptReloadHost reload = fixture.CreateReloadHost(reloads);
        reload.Start();
        ScriptCompilationResult initial = fixture.CompilePending(reload);
        Assert.True(initial.success, FormatDiagnostics(initial));
        Assert.True(reload.ApplyPendingReload());
        CompleteUnloadVerification(reload);
        var scenes = new PluginPlayScene(fixture);
        var history = new CountingHistoryIsolation();
        using var playMode = new EditorPlayModeController(
            fixture.host,
            new RuntimeSessionOptions
            {
                kind = RuntimeSessionKind.Play,
                applicationID = "tests.scripting.play-reload",
                createLogSink = _ => new FileLogSink(
                    Path.Combine(Path.Combine(fixture.projectRoot, "Persistent", "tests.scripting.play-reload"), "Logs")
                ),
                jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
                referenceResolvers = [fixture.assets]
            },
            scenes,
            new ReadyScriptCompilation(),
            history,
            fixture.host.logs
        );
        using IDisposable registration = reloads.RegisterGuard(playMode);
        Assert.True(playMode.EnterPlayMode());
        playMode.AdvanceTransition();
        playMode.AdvanceTransition();
        Assert.Equal(EditorPlayModeState.Playing, playMode.state);
        reload.ReloadPlugins();
        Assert.True(reload.isCompilationPending);
        Assert.False(reloads.CanRequestReload(out _));
        Assert.False(reloads.CanActivateReload(out _));
        playMode.Tick(1f / 60f);
        Assert.Equal(EditorPlayModeState.Playing, playMode.state);
        Assert.Equal(0, scenes.restoreCount);
        Assert.True(playMode.ExitPlayMode());
        Assert.False(reloads.CanActivateReload(out _));
        playMode.AdvanceTransition();
        playMode.AdvanceTransition();
        Assert.Equal(EditorPlayModeState.Editing, playMode.state);
        Assert.Equal(1, scenes.restoreCount);
        Assert.Equal(1, history.disposeCount);
        Assert.True(reloads.CanActivateReload(out _));
        ScriptCompilationResult replacement = fixture.CompilePending(reload);
        Assert.True(replacement.success, FormatDiagnostics(replacement));
        Assert.True(reload.ApplyPendingReload());
        CompleteUnloadVerification(reload);
    }

    [Fact]
    public void ShutdownWaitsForPreviousRetirementBeforeUnloadingActiveScripts()
    {
        using var fixture = new ScriptingFixture(WriteUnavailableGenerationPlugin);
        using ScriptReloadHost reload = fixture.CreateReloadHost(new EditorReloadCoordinator());
        reload.Start();
        Assert.True(fixture.CompilePending(reload).success);
        Assert.True(reload.ApplyPendingReload());
        CompleteUnloadVerification(reload);
        Assert.True(fixture.CompilePendingPluginReload(reload).success);
        Assert.True(reload.ApplyPendingReload());
        Assert.Equal(GenerationState.AwaitingCollection, fixture.host.modules.generations.state);
        reload.Dispose();
        fixture.host.modules.generations.Wait();
        Assert.Equal(GenerationState.Ready, fixture.host.modules.generations.state);
        Assert.Empty(fixture.host.modules.modules);
    }

    [Fact]
    public void ShutdownPreservesLoadedScriptAssetAsMissingUntilItsTypeReturns()
    {
        using var fixture = new ScriptingFixture();
        fixture.Write("ShutdownAsset.cs", """
            using InnoEngine.Assets;
            using InnoEngine.Reflection;

            [StableTypeID("527df6c8-1373-48c9-9230-805a85590544")]
            public sealed class ShutdownAsset : AssetObject
            {
            }
            """);
        fixture.Write("ShutdownAssetImporter.editor.cs", """
            using System.Collections.Generic;
            using System.Threading;
            using System.Threading.Tasks;
            using InnoEngine.Assets;
            using InnoEditor.Assets;

            [AssetImporter("tests.shutdown-script-asset")]
            public sealed class ShutdownAssetImporter : AssetImporter<ShutdownAsset>
            {
                public override IReadOnlyList<string> supportedExtensions { get; } = [".shutdownasset"];

                protected override async ValueTask ImportAsync(
                    AssetImportContext context,
                    AssetImportWriter<ShutdownAsset> output,
                    CancellationToken cancellationToken)
                {
                    output.SetAsset(new ShutdownAsset());
                    await output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken: cancellationToken);
                }
            }
            """);
        fixture.Write("Content/value.shutdownasset", "last-good");
        (Guid persistentID, Guid stableTypeID) = LoadScriptAssetAndUnloadScripts(fixture);
        Assert.Empty(fixture.host.modules.modules);
        Assert.True(fixture.assets.TryGetInfo(persistentID, out AssetInfo? preserved));
        Assert.Equal(AssetImportStatus.Imported, preserved!.status);
        Assert.Equal(stableTypeID, preserved.stableAssetTypeID);
        AssetObject missing = ((IAssetReferenceResolver)fixture.assets).Resolve(
            persistentID,
            stableTypeID,
            "Content/value.shutdownasset",
            typeof(AssetObject),
            "$test"
        );
        Assert.True(missing.isMissing);
        Assert.Equal(persistentID, missing.identity.persistentID);
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void ContentPlayStopReloadPlaySoakRetiresEverySessionBeforeTheNextGeneration()
    {
        using var fixture = new ScriptingFixture(WriteUnavailableGenerationPlugin);
        var reloads = new EditorReloadCoordinator();
        using ScriptReloadHost reload = fixture.CreateReloadHost(reloads);
        reload.Start();
        Assert.True(fixture.CompilePending(reload).success);
        Assert.True(reload.ApplyPendingReload());
        CompleteUnloadVerification(reload);
        var scenes = new PluginPlayScene(fixture);
        var history = new CountingHistoryIsolation();
        using var play = new EditorPlayModeController(
            fixture.host,
            new RuntimeSessionOptions
            {
                kind = RuntimeSessionKind.Play,
                applicationID = "tests.content-soak",
                createLogSink = _ => new FileLogSink(
                    Path.Combine(Path.Combine(fixture.projectRoot, "Persistent", "tests.content-soak"), "Logs")
                ),
                jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
                referenceResolvers = [fixture.assets]
            },
            scenes,
            new ReadyScriptCompilation(),
            history,
            fixture.host.logs
        );
        using IDisposable registration = reloads.RegisterGuard(play);
        for (int iteration = 0; iteration < 10; iteration++)
        {
            Assert.True(play.EnterPlayMode());
            play.AdvanceTransition();
            play.AdvanceTransition();
            Assert.True(play.state == EditorPlayModeState.Playing, play.lastFailure);
            for (int frame = 0; frame < 60; frame++)
            {
                play.Tick(1f / 60f);
            }
            Assert.True(play.ExitPlayMode());
            play.AdvanceTransition();
            play.AdvanceTransition();
            Assert.Equal(EditorPlayModeState.Editing, play.state);
            Assert.True(fixture.CompilePendingPluginReload(reload).success);
            Assert.True(reload.ApplyPendingReload());
            CompleteUnloadVerification(reload);
            Assert.Equal(GenerationState.Ready, fixture.host.modules.generations.state);
            Assert.Equal(iteration + 1, scenes.restoreCount);
            Assert.Equal(iteration + 1, history.disposeCount);
        }

        Assert.True(play.EnterPlayMode());
        play.AdvanceTransition();
        play.AdvanceTransition();
        Assert.Equal(EditorPlayModeState.Playing, play.state);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void AssertRecoveredAvailability(AvailabilitySceneState state)
    {
        GameComponent recoveredPlugin = state.pluginOwner.GetComponents().Single(component => component is not Transform);
        GameComponent recoveredScript = state.scriptOwner.GetComponents().Single(component => component is not Transform);
        Assert.IsNotType<MissingGameComponent>(recoveredPlugin);
        Assert.IsNotType<MissingGameComponent>(recoveredScript);
        Assert.Equal(state.pluginComponentID, recoveredPlugin.identity.persistentID);
        Assert.Equal(state.scriptComponentID, recoveredScript.identity.persistentID);
        Assert.Equal(47, recoveredPlugin.GetType().GetProperty("retained")!.GetValue(recoveredPlugin));
        Assert.Equal(73, recoveredScript.GetType().GetProperty("retained")!.GetValue(recoveredScript));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference CaptureCachedSettingType(ProjectSettingsStore settings)
    {
        Assert.True(settings.TryClone(new ProjectSettingID("tests.detached.setting"), out ISerializable? value));
        Assert.NotNull(value);
        return new WeakReference(value.GetType());
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (Guid persistentID, Guid stableTypeID) LoadScriptAssetAndUnloadScripts(ScriptingFixture fixture)
    {
        using ScriptReloadHost reload = fixture.CreateReloadHost(new EditorReloadCoordinator());
        reload.Start();
        ScriptCompilationResult compilation = fixture.CompilePending(reload);
        Assert.True(compilation.success, FormatDiagnostics(compilation));
        Assert.True(reload.ApplyPendingReload());
        Type assetType = fixture.ResolveActiveType("ShutdownAsset");
        AssetObject loaded = fixture.assets.Load(AssetPath.Project("Content/value.shutdownasset"), assetType);
        Assert.True(fixture.assets.TryGetInfo(loaded.identity.persistentID, out AssetInfo? imported));
        Assert.Equal(AssetImportStatus.Imported, imported!.status);
        Guid persistentID = loaded.identity.persistentID;
        Guid stableTypeID = imported.stableAssetTypeID;
        reload.Dispose();
        return (persistentID, stableTypeID);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void AssertRetainedActiveType(
        ScriptingFixture fixture,
        WeakReference expected,
        int expectedVersion
    ) {
        Type active = fixture.ResolveActiveType("VersionedBehavior");
        Assert.Same(expected.Target, active);
        Assert.Equal(expectedVersion, ReadVersion(active));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static RetiredScriptObject AttachReloadableBehavior(
        ScriptingFixture fixture,
        GameObject gameObject
    ) {
        Type type = fixture.ResolveActiveType("ReloadableBehavior");
        GameComponent component = gameObject.AddComponent(type);
        type.GetProperty("retained")!.SetValue(component, 41);
        return new RetiredScriptObject(new WeakReference(type), new WeakReference(component));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static AvailabilitySceneState CreateAvailabilitySceneState(
        ScriptingFixture fixture,
        GameScene scene
    ) {
        GameObject pluginOwner = scene.CreateObject("Plugin Owner");
        GameObject scriptOwner = scene.CreateObject("Script Owner");
        Type pluginType = fixture.ResolveActiveType("PluginBehavior");
        Type scriptType = fixture.ResolveActiveType("DependentBehavior");
        GameComponent pluginComponent = pluginOwner.AddComponent(pluginType);
        GameComponent scriptComponent = scriptOwner.AddComponent(scriptType);
        pluginType.GetProperty("retained")!.SetValue(pluginComponent, 47);
        scriptType.GetProperty("retained")!.SetValue(scriptComponent, 73);
        return new AvailabilitySceneState(
            pluginOwner,
            scriptOwner,
            pluginComponent.identity.persistentID,
            scriptComponent.identity.persistentID
        );
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static MissingGenerationExpectation CommitPluginRemoval(
        ScriptingFixture fixture,
        ScriptReloadHost reload,
        RuntimeSession session
    ) {
        GameScene scene = session.scenes.LoadNewScene("Plugin Removal");
        GameObject owner = scene.CreateObject("Plugin Owner");
        Type pluginType = fixture.ResolveActiveType("PluginBehavior");
        GameComponent component = owner.AddComponent(pluginType);
        Guid componentID = component.identity.persistentID;
        string installedPlugin = Path.Combine(fixture.projectRoot, "Plugins", "unavailability.iplugin");
        string detachedPlugin = Path.Combine(fixture.projectRoot, "UnavailabilityPlugin.detached");
        File.Move(installedPlugin, detachedPlugin);
        Assert.True(fixture.RefreshPlugins());
        ScriptCompilationResult unavailable = fixture.CompilePendingPluginReload(reload);
        Assert.True(unavailable.success, FormatDiagnostics(unavailable));
        Assert.DoesNotContain(unavailable.moduleDeployments, static request => request.domain == AssemblyDomain.InnoPlugin);
        Assert.DoesNotContain(
            unavailable.moduleDeployments.SelectMany(static request => request.upstreamModuleNames),
            static moduleName => string.Equals(moduleName, "Plugin.tests.unavailability", StringComparison.Ordinal)
        );
        Assert.True(reload.ApplyPendingReload());
        Assert.DoesNotContain(
            fixture.host.modules.modules,
            static module => (
                module.domain == AssemblyDomain.InnoPlugin
                || module.upstreamModuleNames.Contains("Plugin.tests.unavailability", StringComparer.Ordinal)
            )
        );
        return new MissingGenerationExpectation(owner, componentID, new WeakReference(component), new WeakReference(pluginType));
    }

    private static void ForceFullCollection()
    {
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
        GC.WaitForPendingFinalizers();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
    }

    private static void WriteUnavailableGenerationPlugin(
        string projectRoot,
        SerializationRegistry serialization
    ) => WriteUnavailableGenerationPlugin(projectRoot, serialization, """
            using InnoEngine.Reflection;
            using InnoEngine.Scene;
            using InnoEngine.Serialization;

            namespace UnavailabilityPlugin;

            [StableTypeID("a7b5f773-184f-4bd1-bf93-e4b1088d9d68")]
            public sealed class PluginBehavior : GameBehavior
            {
                [SerializableProperty]
                public int retained { get; set; } = 5;
            }
            """);

    private static void WriteUnavailableGenerationPlugin(
        string projectRoot,
        SerializationRegistry serialization,
        string source
    ) => WritePluginPackage(
        projectRoot,
        "unavailability.iplugin",
        serialization,
        new PluginManifest { pluginId = "tests.unavailability", displayName = "Unavailability Test Plugin" },
        new Dictionary<string, byte[]>
        {
            ["Assets/PluginBehavior.cs"] = Encoding.UTF8.GetBytes(source),
            ["Assets/PluginBehavior.cs.imeta"] = CreateScriptSourceMeta(
                serialization,
                Guid.Parse("37f9751f-a7de-43c7-94ac-c8e854762d49")
            )
        }
    );

    private readonly record struct MissingGenerationExpectation(
        GameObject owner,
        Guid componentID,
        WeakReference retiredComponent,
        WeakReference retiredType
    );

    private readonly record struct RetiredScriptObject(
        WeakReference type,
        WeakReference component
    );

    private readonly record struct AvailabilitySceneState(
        GameObject pluginOwner,
        GameObject scriptOwner,
        Guid pluginComponentID,
        Guid scriptComponentID
    );

    private sealed class PluginPlayScene(ScriptingFixture fixture) : IEditorScenePlayMode
    {
        public IDisposable BeginPlayMode(RuntimeSession runtimeSession)
        {
            var scene = new GameScene("Plugin Play Reload");
            GameObject owner = scene.CreateObject("Plugin Component Owner");
            _ = owner.AddComponent(fixture.ResolveActiveType("PluginBehavior"));
            runtimeSession.scenes.LoadScene(scene);
            return new RestoreLease(this);
        }

        internal int restoreCount { get; private set; }

        private sealed class RestoreLease(PluginPlayScene owner) : IDisposable
        {
            private bool m_disposed;

            public void Dispose()
            {
                if (m_disposed)
                {
                    return;
                }
                m_disposed = true;
                owner.restoreCount++;
            }
        }
    }

    private sealed class CountingHistoryIsolation : IEditorHistoryIsolation
    {
        public IDisposable BeginHistoryIsolation() => new HistoryLease(this);

        internal int disposeCount { get; private set; }

        private sealed class HistoryLease(CountingHistoryIsolation owner) : IDisposable
        {
            private bool m_disposed;

            public void Dispose()
            {
                if (m_disposed)
                {
                    return;
                }
                m_disposed = true;
                owner.disposeCount++;
            }
        }
    }

    private sealed class ReadyScriptCompilation : IEditorScriptCompilation
    {
        private readonly ReadyCompilationTicket m_ticket = new();

        public bool requiresExclusiveProgress => false;
        public EditorScriptCompilationProgress progress => new(0, 0f, "Test script status.", false);

        public IScriptCompilationTicket? currentTicket => m_ticket;
        public EditorScriptCompilationState state => EditorScriptCompilationState.Ready;
        public ScriptCompilationResult? lastCompilation => null;

        public void CancelCompilation(long operationID) {}

        public IScriptCompilationTicket RequestCompilation() => m_ticket;

        private sealed class ReadyCompilationTicket : IScriptCompilationTicket
        {
            public long requestID => 1;
            public ScriptCompilationTicketState state => ScriptCompilationTicketState.Succeeded;
            public string status => "The test compilation ticket succeeded.";
            public ScriptCompilationResult? result => null;
            public bool isCompleted => true;
        }
    }

    private sealed class TestSceneReloadParticipant(SceneReloadService reload) : IEditorReloadParticipant
    {
        public IGenerationChange Capture(AssemblyReloadContext context)
            => new TestSceneReloadTransaction(reload.Capture(context.GetContext<TypeCacheReloadContext>()));

        public void RefreshDiagnostics() {}
    }

    private sealed class TestSceneReloadTransaction(ISceneReloadStateTransfer transfer) : IGenerationChange
    {
        public void PrepareForActivation() => transfer.PrepareForActivation();

        public void Apply() => transfer.Apply();

        public void Complete() => transfer.Complete();

        public void RollbackStructure() => transfer.RollbackStructure();

        public void RestorePreviousState() => transfer.RestorePreviousState();
    }
}
