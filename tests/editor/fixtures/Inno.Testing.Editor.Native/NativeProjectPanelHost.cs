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
using Inno.Core.Serialization;
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

namespace Inno.Testing.Editor.Native;

/// <summary>
/// Owns a temporary project, authoring services and real native panels for contract and performance acceptance.
/// </summary>
/// <remarks>
/// Use and dispose the complete fixture on one thread. Returned services remain owned by this fixture.
/// </remarks>
public sealed class NativeProjectPanelHost : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativeProjectPanels", Guid.NewGuid().ToString("N"));
    private readonly EngineHost m_engine;
    private readonly RuntimeSession m_session;
    private readonly IDisposable m_execution;
    private readonly EditorConsole m_console;
    private readonly ProjectSettingsStore m_settings;
    private readonly PluginEnvironment m_plugins;

    /// <summary>
    /// Creates the explicit test composition and its isolated project resources.
    /// </summary>
    /// <param name="composition">The calling test assembly declaring all discovered modules and serialized test types.</param>
    /// <param name="supportsVertexOffsets">Whether to advertise and validate native vertex offsets.</param>
    /// <param name="includeSettings">Whether to compose real project and build settings services.</param>
    /// <param name="settingsDocument">An optional public document-store boundary for settings persistence.</param>
    public NativeProjectPanelHost(
        Assembly composition,
        bool supportsVertexOffsets = false,
        bool includeSettings = false,
        IByteDocumentStore? settingsDocument = null
    ) {
        _ = Assembly.Load("Inno.Editor.Panel.Hierarchy");
        _ = Assembly.Load("Inno.Editor.Panel.FileBrowser");
        _ = Assembly.Load("Inno.Editor.Workbench");
        _ = Assembly.Load("Inno.Scene.Assets");
        string source = Path.Combine(m_root, "Assets");
        string library = Path.Combine(m_root, "Library");
        string pluginRoot = Path.Combine(m_root, "Plugins");
        Directory.CreateDirectory(source);
        m_engine = new EngineHostBuilder().UseMetadataSources(
            new DotNetAssemblyCatalogSource(composition),
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
        m_settings = new ProjectSettingsStore(settingsDocument ?? new FileByteDocumentStore(Path.Combine(m_root, "Settings.Project.inno")),
            m_engine.types, m_engine.serialization, new ProjectId("tests.native-panels"), AssetSerializationContext.Create(assets));
        var pluginSources = new PluginSourceService(m_engine.serialization, pluginRoot, library);
        m_plugins = new PluginEnvironment(assets, m_settings, m_engine.serialization, pluginRoot, library, pluginSources.Scan(), m_engine.modules.generations);
        m_console = new EditorConsole(m_engine.logs, m_engine.diagnostics, new NativePanelHost.InactivePlayMode());
        m_console.Start();
        var ui = new EditorUiSession();
        reloads = new EditorReloadCoordinator();
        SettingsPanelServices? settingsServices = includeSettings
            ? new SettingsPanelServices(m_root, assets, m_plugins, m_settings, m_engine)
            : null;
        if (includeSettings)
            _ = Assembly.Load("Inno.Editor.Panel.Settings");
        runtime = new EditorInteractionRuntime(
            new EditorContext(m_root, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl")),
            m_engine.types,
            m_engine.logs,
            [
                m_engine.types, m_engine.serialization, m_session, identities, m_session.identities,
                assets, m_plugins, m_settings, reloads, ui, m_console,
                .. (settingsServices?.services ?? [])
            ]);
        runtime.Start();
        _ = runtime.panelCount;
        Assert.True(runtime.interactions.TryGetModule<NativeProjectPanelProbe>(out var probe));
        workspace = probe!.workspace;
        graphs = probe.graphs;
        edits = probe.edits;
        native = new NativePanelHost(runtime, ui, m_console, m_engine.logs, m_engine.diagnostics, supportsVertexOffsets);
    }

    /// <summary>
    /// Gets the borrowed reload coordinator.
    /// </summary>
    public EditorReloadCoordinator reloads { get; }
    /// <summary>
    /// Gets the borrowed project settings store.
    /// </summary>
    public ProjectSettingsStore projectSettings => m_settings;
    /// <summary>
    /// Gets the borrowed authoring asset pipeline.
    /// </summary>
    public AssetPipeline assets { get; }
    /// <summary>
    /// Gets the borrowed engine serialization registry.
    /// </summary>
    public SerializationRegistry serialization => m_engine.serialization;
    /// <summary>
    /// Gets the absolute temporary project directory removed during disposal.
    /// </summary>
    public string projectRoot => m_root;
    /// <summary>
    /// Gets the borrowed interaction runtime.
    /// </summary>
    public EditorInteractionRuntime runtime { get; }
    /// <summary>
    /// Gets the borrowed scene workspace resolved through normal module composition.
    /// </summary>
    public IEditorSceneWorkspace workspace { get; }
    /// <summary>
    /// Gets the borrowed graph editor module.
    /// </summary>
    public GraphEditorModule graphs { get; }
    /// <summary>
    /// Gets the borrowed scene editing service.
    /// </summary>
    public SceneEdits edits { get; }
    /// <summary>
    /// Gets the borrowed native panel host.
    /// </summary>
    public NativePanelHost native { get; }

    /// <summary>
    /// Creates and loads an additive scene in the owned Edit session.
    /// </summary>
    /// <returns>The scene owned by this fixture's session.</returns>
    public GameScene CreateScene() => m_session.scenes.LoadNewSceneAdditive();
    /// <summary>
    /// Rebuilds the actual type catalog after a declared discovery change.
    /// </summary>
    public void RefreshTypes() => m_engine.types.Rebuild();

    /// <summary>
    /// Releases authoring, native and session resources before removing the owned temporary project.
    /// </summary>
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
