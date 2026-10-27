using System;
using System.IO;
using System.Reflection;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Core.IO;
using Inno.Core.Logging;
using Inno.Core.Settings;
using Inno.Editor.Core;
using Inno.Editor.Diagnostics;
using Inno.Editor.ImGui;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scene;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;

internal sealed class NativeProjectPanelHost : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativeProjectPanels", Guid.NewGuid().ToString("N"));
    private readonly EngineHost m_engine;
    private readonly RuntimeSession m_session;
    private readonly IDisposable m_execution;
    private readonly EditorConsole m_console;
    private readonly ProjectSettingsStore m_settings;
    private readonly PluginEnvironment m_plugins;

    internal NativeProjectPanelHost(bool supportsVertexOffsets = false)
    {
        _ = Assembly.Load("Inno.Editor.Panel.Hierarchy");
        _ = Assembly.Load("Inno.Editor.Panel.FileBrowser");
        _ = Assembly.Load("Inno.Editor.Workbench");
        _ = Assembly.Load("Inno.Scene.Assets");
        string source = Path.Combine(m_root, "Assets");
        string library = Path.Combine(m_root, "Library");
        string pluginRoot = Path.Combine(m_root, "Plugins");
        Directory.CreateDirectory(source);
        m_engine = new EngineHostBuilder().UseMetadataSources(
            new DotNetAssemblyCatalogSource(typeof(NativeProjectPanelHost).Assembly),
            new ReflectionTypeCatalogSource(),
            new ReflectionSerializationMetadataSource()).Build();
        m_session = m_engine.CreateSession(new RuntimeSessionOptions
        {
            kind = RuntimeSessionKind.Edit,
            applicationId = "inno.tests.native-panels",
            createLogSink = _ => new FileLogSink(Path.Combine(library, "Logs")),
            jobExecutionMode = RuntimeJobExecutionMode.SingleThread
        });
        m_execution = m_session.EnterExecutionScope();
        var identities = new IdentityAllocator();
        assets = new AssetPipeline(m_engine.modules, m_engine.types, m_engine.serialization, identities,
            m_engine.diagnostics, m_engine.logs, AssetPipelineOptions.Create(source, library) with { enableFileSystemWatcher = false });
        m_settings = new ProjectSettingsStore(new FileByteDocumentStore(Path.Combine(m_root, "Settings.Project.inno")),
            m_engine.types, m_engine.serialization, new ProjectId("tests.native-panels"), AssetSerializationContext.Create(assets));
        var pluginSources = new PluginSourceService(m_engine.serialization, pluginRoot, library);
        m_plugins = new PluginEnvironment(assets, m_settings, m_engine.serialization, pluginRoot, library, pluginSources.Scan(), m_engine.modules.generations);
        m_console = new EditorConsole(m_engine.logs, m_engine.diagnostics, new NativePanelHost.InactivePlayMode());
        m_console.Start();
        var ui = new EditorUiSession();
        var reloads = new EditorReloadCoordinator();
        runtime = new EditorInteractionRuntime(new EditorContext(m_root, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl")),
            m_engine.types, m_engine.logs, [m_engine.types, m_engine.serialization, m_session, identities, m_session.identities, assets, m_plugins, m_settings, reloads, ui, m_console]);
        runtime.Start();
        _ = runtime.panelCount;
        Assert.True(runtime.interactions.TryGetModule<NativeProjectPanelProbe>(out var probe));
        workspace = probe!.workspace;
        graphs = probe.graphs;
        edits = probe.edits;
        native = new NativePanelHost(runtime, ui, m_console, m_engine.logs, m_engine.diagnostics, supportsVertexOffsets);
    }

    internal AssetPipeline assets { get; }
    internal string projectRoot => m_root;
    internal EditorInteractionRuntime runtime { get; }
    internal IEditorSceneWorkspace workspace { get; }
    internal GraphEditorModule graphs { get; }
    internal SceneEdits edits { get; }
    internal NativePanelHost native { get; }

    internal GameScene CreateScene() => m_session.scenes.LoadNewSceneAdditive();

    public void Dispose()
    {
        runtime.Dispose();
        native.Dispose();
        m_console.Dispose();
        m_plugins.Dispose();
        assets.Dispose();
        m_settings.Dispose();
        m_execution.Dispose();
        m_session.Dispose();
        m_engine.Dispose();
        Directory.Delete(m_root, recursive: true);
    }
}

[EditorModule("tests.native-project-panels", order: 2000)]
public sealed class NativeProjectPanelProbe(
    IEditorSceneWorkspace workspace,
    GraphEditorModule graphs,
    SceneEdits edits
) : EditorModule {
    public IEditorSceneWorkspace workspace { get; } = workspace;
    public GraphEditorModule graphs { get; } = graphs;
    public SceneEdits edits { get; } = edits;
}
