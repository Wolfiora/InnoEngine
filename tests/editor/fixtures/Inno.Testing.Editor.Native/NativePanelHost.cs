using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Inno.Adapter.Modules.DotNet;
using Inno.Core.Diagnostics;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.Diagnostics;
using Inno.Editor.ImGui;
using Inno.Editor.Interactions;
using Inno.Editor.PlayMode;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using ImGuiCond = Inno.Native.ImGui.ImGuiCond;
using ImGuiWindowFlags = Inno.Native.ImGui.ImGuiWindowFlags;

namespace Inno.Testing.Editor.Native;

/// <summary>
/// Owns a real ImGui context and frame validation for a declared test composition.
/// </summary>
/// <remarks>
/// Construct, draw and dispose on the same thread. Do not overlap hosts sharing ImGui global state.
/// </remarks>
public sealed unsafe class NativePanelHost : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativePanels", Guid.NewGuid().ToString("N"));
    private readonly ImGuiNativeContext* m_nativeContext;
    private readonly NativeClipboard m_clipboard;
    private readonly IDisposable? m_diagnosticScope;
    private readonly ModuleHost? m_modules;
    private readonly TypeCatalog? m_types;
    private readonly bool m_ownsServices;
    private readonly bool m_validateVertexOffsets;
    private float m_time;
    private Vector2 m_panelSize = new(1000f, 800f);

    /// <summary>
    /// Creates a panel host with the default keyboard convention and vertex-offset validation disabled.
    /// </summary>
    /// <param name="composition">The calling test assembly owning the complete discovery closure.</param>
    /// <param name="assemblies">Additional declared panel assemblies to load before discovery.</param>
    public NativePanelHost(Assembly composition, params string[] assemblies) : this(composition, false, assemblies) { }

    /// <summary>
    /// Creates a panel host with an explicit vertex-offset capability.
    /// </summary>
    /// <param name="composition">The calling test assembly owning the complete discovery closure.</param>
    /// <param name="supportsVertexOffsets">Whether to advertise and validate native vertex offsets.</param>
    /// <param name="assemblies">Additional declared panel assemblies to load before discovery.</param>
    public NativePanelHost(
        Assembly composition,
        bool supportsVertexOffsets,
        params string[] assemblies
    ) : this(composition, supportsVertexOffsets, new EditorKeyboardPolicy(KeyModifier.Control, "Super"), assemblies) { }

    /// <summary>
    /// Creates the owned diagnostics, discovery, interactions and native frame context.
    /// </summary>
    /// <param name="composition">The calling test assembly owning the complete discovery closure.</param>
    /// <param name="supportsVertexOffsets">Whether to advertise and validate native vertex offsets.</param>
    /// <param name="keyboard">The explicit keyboard convention used by the test.</param>
    /// <param name="assemblies">Additional declared panel assemblies to load before discovery.</param>
    public NativePanelHost(
        Assembly composition,
        bool supportsVertexOffsets,
        EditorKeyboardPolicy keyboard,
        params string[] assemblies
    ) {
        m_validateVertexOffsets = supportsVertexOffsets;
        m_ownsServices = true;
        Directory.CreateDirectory(m_root);
        foreach (string assembly in assemblies)
            _ = Assembly.Load(assembly);
        diagnostics = new DiagnosticHub();
        m_diagnosticScope = diagnostics.EnterScope();
        logs = new LogRouter();
        m_modules = new ModuleHost(new() { catalogSource = new DotNetAssemblyCatalogSource(composition) });
        m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
        console = new EditorConsole(logs, diagnostics, new InactivePlayMode());
        console.Start();
        runtime = new EditorInteractionRuntime(new EditorContext(m_root, keyboard), m_types, logs, [console]);
        runtime.Start();
        ui = new EditorUiSession();
        m_nativeContext = CreateNativeContext(ui, supportsVertexOffsets);
        m_clipboard = new NativeClipboard(m_nativeContext);
        logs.Flush();
        console.Clear();
    }

    internal NativePanelHost(
        EditorInteractionRuntime runtime,
        EditorUiSession ui,
        EditorConsole console,
        LogRouter logs,
        DiagnosticHub diagnostics,
        bool supportsVertexOffsets = false
    ) {
        this.runtime = runtime;
        this.ui = ui;
        this.console = console;
        this.logs = logs;
        m_validateVertexOffsets = supportsVertexOffsets;
        this.diagnostics = diagnostics;
        m_root = string.Empty;
        m_nativeContext = CreateNativeContext(ui, supportsVertexOffsets);
        m_clipboard = new NativeClipboard(m_nativeContext);
    }

    /// <summary>
    /// Gets the borrowed diagnostic hub for this host's lifetime.
    /// </summary>
    public DiagnosticHub diagnostics { get; }
    /// <summary>
    /// Gets the borrowed log router for this host's lifetime.
    /// </summary>
    public LogRouter logs { get; }
    /// <summary>
    /// Gets the borrowed console consuming the host's logs.
    /// </summary>
    public EditorConsole console { get; }
    /// <summary>
    /// Gets the borrowed interaction runtime and discovered panels.
    /// </summary>
    public EditorInteractionRuntime runtime { get; }
    /// <summary>
    /// Gets the borrowed UI session owning the host's frame state.
    /// </summary>
    public EditorUiSession ui { get; }

    /// <summary>
    /// Finds and opens exactly one discovered panel.
    /// </summary>
    /// <param name="id">The registered panel identity.</param>
    /// <returns>The borrowed panel, valid until this host is disposed.</returns>
    /// <exception cref="InvalidOperationException">The identity has no unique discovered panel.</exception>
    public EditorPanelExtension GetPanel(string id)
    {
        EditorPanelExtension panel = runtime.panels.Single(value => value.id == id);
        panel.isOpen = true;
        return panel;
    }

    /// <summary>
    /// Executes a real native frame and validates its resulting draw data.
    /// </summary>
    /// <param name="draw">The drawing operation executed within the UI scope.</param>
    /// <param name="deltaTime">The explicit elapsed frame time in seconds.</param>
    public void Frame(
        Action draw,
        float deltaTime = 1f / 60f
    ) {
        m_time += deltaTime;
        runtime.Update(new EditorFrame(deltaTime, m_time, true));
        NativeImGui.Raw.GetIO()->DeltaTime = deltaTime;
        using IDisposable scope = ui.Enter();
        NativeImGui.NewFrame();
        try
        {
            draw();
        }
        finally
        {
            NativeImGui.Render();
            if (m_validateVertexOffsets)
                ValidateDrawData();
            m_clipboard.ThrowIfFaulted();
        }
    }

    /// <summary>
    /// Sets the native display dimensions, framebuffer scale and UI zoom.
    /// </summary>
    /// <param name="logicalSize">The logical display size in UI coordinates.</param>
    /// <param name="framebufferScale">The physical framebuffer scale.</param>
    /// <param name="zoom">The UI style zoom.</param>
    public void SetDisplayMetrics(
        Vector2 logicalSize,
        float framebufferScale,
        float zoom
    ) {
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(logicalSize);
        io->DisplayFramebufferScale = Unsafe.BitCast<Vector2, NativeVector2>(new(framebufferScale));
        m_panelSize = Vector2.Min(new(1000f, 800f), logicalSize - new Vector2(40f));
        ui.widgets.style.SetZoom(zoom);
        using IDisposable scope = ui.Enter();
        ui.widgets.SetupStyle();
    }

    /// <summary>
    /// Draws the actual panel inside a bounded native window and completes the frame.
    /// </summary>
    /// <param name="panel">The borrowed panel belonging to this host's interaction runtime.</param>
    /// <param name="afterDraw">An optional operation performed after the panel draws and before the window closes.</param>
    /// <param name="deltaTime">The explicit elapsed frame time in seconds.</param>
    /// <exception cref="InvalidOperationException">The panel is quarantined by a real draw failure.</exception>
    public void DrawPanel(
        EditorPanelExtension panel,
        Action? afterDraw = null,
        float deltaTime = 1f / 60f
    ) {
        Frame(() =>
        {
            NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), (int)ImGuiCond.Always);
            NativeImGui.SetNextWindowSize(m_panelSize, (int)ImGuiCond.Always);
            bool open = true;
            _ = NativeImGui.Begin("Panel acceptance", ref open, (int)ImGuiWindowFlags.NoSavedSettings);
            try
            {
                if (panel.isOpen && !panel.Draw(runtime.context))
                {
                    logs.Flush();
                    string failures = string.Join(Environment.NewLine, console.Capture().occurrences
                        .Where(static entry => entry.level >= LogLevel.Error)
                        .Select(static entry => entry.message + Environment.NewLine + entry.stackTrace));
                    throw new InvalidOperationException("The actual panel was quarantined: " + panel.id + Environment.NewLine + failures);
                }
                afterDraw?.Invoke();
            }
            finally
            {
                NativeImGui.End();
            }
        }, deltaTime);
    }

    /// <summary>
    /// Releases the owned UI, native context and services on their creating thread.
    /// </summary>
    public void Dispose()
    {
        if (m_ownsServices)
        {
            runtime.Dispose();
            console.Dispose();
        }
        ui.Dispose();
        if (m_ownsServices)
        {
            m_types?.Dispose();
            m_modules?.Dispose();
            logs.Dispose();
            m_diagnosticScope?.Dispose();
        }
        NativeImGui.Raw.DestroyContext(m_nativeContext);
        m_clipboard.Dispose();
        if (m_ownsServices)
            Directory.Delete(m_root, recursive: true);
    }

    private static ImGuiNativeContext* CreateNativeContext(
        EditorUiSession ui,
        bool supportsVertexOffsets
    ) {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(1200f, 900f));
        io->DeltaTime = 1f / 60f;
        io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
        if (supportsVertexOffsets)
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasVtxOffset;
        io->ConfigFlags |= (int)ImGuiConfigFlags.NavEnableKeyboard;
        // Fixture input uses an explicit Control convention on every runner.
        io->ConfigMacOsxBehaviors = 0;
        io->Fonts->RendererHasTextures = 1;
        io->IniFilename = null;
        using (ui.Enter())
            ui.widgets.SetupStyle();
        return context;
    }

    private static void ValidateDrawData()
    {
        ImDrawData* data = NativeImGui.Raw.GetDrawData();
        for (int listIndex = 0; listIndex < data->CmdListsCount; listIndex++)
        {
            ImDrawList* list = data->CmdLists.Data[listIndex];
            for (int commandIndex = 0; commandIndex < list->CmdBuffer.Size; commandIndex++)
            {
                ImDrawCmd command = list->CmdBuffer.Data[commandIndex];
                ulong lastIndex = (ulong)command.IdxOffset + command.ElemCount;
                if (lastIndex > (ulong)list->IdxBuffer.Size)
                    throw new InvalidOperationException("Native draw data exceeded its index buffer.");
                for (ulong index = command.IdxOffset; index < lastIndex; index++)
                    if ((ulong)command.VtxOffset + list->IdxBuffer.Data[index] >= (ulong)list->VtxBuffer.Size)
                        throw new InvalidOperationException("Native draw data exceeded its vertex buffer after applying the command offset.");
            }
        }
    }

    internal sealed class InactivePlayMode : IEditorPlayMode
    {
        private readonly Inno.Core.Events.EventDispatcher m_events = new();
        public EditorPlayModeState state => EditorPlayModeState.Editing;
        public bool isPlaying => false;
        public string? lastFailure => null;
        public LogSessionId activeSessionId => LogSessionId.none;
        public IDisposable ListenStateChanges(Action<EditorPlayModeState> listener) => m_events.CreateHub();
        public bool EnterPlayMode() => false;
        public bool ExitPlayMode() => false;
    }
}
