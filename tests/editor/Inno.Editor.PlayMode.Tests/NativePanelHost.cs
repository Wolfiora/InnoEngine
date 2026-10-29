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
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using ImGuiCond = Inno.Native.ImGui.ImGuiCond;
using ImGuiWindowFlags = Inno.Native.ImGui.ImGuiWindowFlags;

namespace Inno.Editor.PlayMode.Tests;

internal sealed unsafe class NativePanelHost : IDisposable
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

    internal NativePanelHost(params string[] assemblies) : this(false, assemblies) { }

    internal NativePanelHost(
        bool supportsVertexOffsets,
        params string[] assemblies
    ) : this(supportsVertexOffsets, new EditorKeyboardPolicy(KeyModifier.Control, "Super"), assemblies) { }

    internal NativePanelHost(
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
        m_modules = new ModuleHost(new() { catalogSource = new DotNetAssemblyCatalogSource(typeof(NativePanelHost).Assembly) });
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

    internal DiagnosticHub diagnostics { get; }
    internal LogRouter logs { get; }
    internal EditorConsole console { get; }
    internal EditorInteractionRuntime runtime { get; }
    internal EditorUiSession ui { get; }

    internal EditorPanelExtension GetPanel(string id)
    {
        EditorPanelExtension panel = runtime.panels.Single(value => value.id == id);
        panel.isOpen = true;
        return panel;
    }

    internal void Frame(
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

    internal void SetDisplayMetrics(
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

    internal void DrawPanel(
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
