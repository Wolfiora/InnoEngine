using System.Runtime.CompilerServices;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using BGCS.Runtime;
using Inno.Adapter.Modules.DotNet;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using Inno.Core.Diagnostics;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.Diagnostics;
using Inno.Editor.Interactions;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;
using ImGuiApi = Inno.Native.ImGui.ImGui;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;

public sealed unsafe class ConsolePanelNativeLayoutTests
{
    [Fact]
    public void SnapshotFailureQuarantinesTheConsoleWithoutLeakingChildWindows()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoConsoleFailure", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        _ = Assembly.Load("Inno.Editor.Panel.Logging");
        ImGuiNativeContext* nativeContext = ImGuiApi.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = ImGuiApi.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(1200f, 800f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            io->IniFilename = null;
            using var modules = new ModuleHost(new() { catalogSource = new DotNetAssemblyCatalogSource(typeof(ConsolePanelNativeLayoutTests).Assembly) });
            using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
            using var logs = new LogRouter();
            var diagnostics = new DiagnosticHub();
            using IDisposable diagnosticScope = diagnostics.EnterScope();
            using var ui = new EditorUiSession();
            using var runtime = new EditorInteractionRuntime(
                new EditorContext(root, new EditorKeyboardPolicy(Inno.Core.Input.KeyModifier.Control, "Super")),
                types,
                logs,
                [new RejectedConsole()]);
            runtime.Start();
            EditorPanelExtension panel = Assert.Single(runtime.panels, value => value.id == "diagnostics.console");
            panel.isOpen = true;
            using IDisposable scope = ui.Enter();
            for (int frame = 0; frame < 2; frame++)
            {
                ImGuiApi.NewFrame();
                ImGuiApi.SetNextWindowSize(new(800f, 600f));
                using NativeStringLease title = Utf8Codec.EncodeNullTerminated("Failure parent", allowsNull: false);
                _ = ImGuiApi.Raw.Begin((sbyte*)title.pointer, null, 0);
                nint parent = (nint)ImGuiApi.Raw.GetCurrentWindow();
                if (frame == 0)
                {
                    Assert.False(panel.Draw(runtime.context));
                    Assert.False(panel.isOpen);
                }
                Assert.Equal(parent, (nint)ImGuiApi.Raw.GetCurrentWindow());
                Inno.Editor.ImGui.ImGui.Text("Following valid drawing");
                ImGuiApi.End();
                ImGuiApi.Render();
            }
        }
        finally
        {
            ImGuiApi.Raw.DestroyContext(nativeContext);
            Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData(false, 1f)]
    [InlineData(true, 1f)]
    [InlineData(false, 1.5f)]
    [InlineData(true, 1.5f)]
    public void RealConsoleKeepsCompleteLabelsOnFirstExpansionAndResize(
        bool diagnostic,
        float scale
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoConsoleLayout", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        _ = Assembly.Load("Inno.Editor.Panel.Logging");
        ImGuiNativeContext* context = ImGuiApi.Raw.CreateContext(null);
        try
        {
            ImGuiApi.Raw.GetIO()->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(1200, 800));
            ImGuiApi.Raw.GetIO()->DeltaTime = 1f / 60;
            ImGuiApi.Raw.GetIO()->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            ImGuiApi.Raw.GetIO()->Fonts->RendererHasTextures = 1;
            ImGuiApi.Raw.ScaleAllSizes(ImGuiApi.Raw.GetStyle(), scale);
            using var modules = new ModuleHost(new() { catalogSource = new DotNetAssemblyCatalogSource(typeof(ConsolePanelNativeLayoutTests).Assembly)});
            using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
            using var logs = new LogRouter();
            var hub = new DiagnosticHub();
            using var console = new EditorConsole(logs, hub, new InactivePlayMode());
            console.Start();
            using var reporter = hub.CreateReporter(new("shader", "Rendering"));
            using var ui = new EditorUiSession();
            using var runtime = new EditorInteractionRuntime(
                new EditorContext(root, new EditorKeyboardPolicy(Inno.Core.Input.KeyModifier.Control, "Super")),
                types,
                logs,
                [console]
            );
            runtime.Start();
            logs.Flush();
            console.Clear();
            if (diagnostic) reporter.Publish(new("BGFX_IR_GENERATION", "BGFX has no position builtin for stage Compute.", DiagnosticSeverity.Error));
            else logs.CreateLogger<ConsolePanelNativeLayoutTests>().Write(LogLevel.Error, "BGFX has no position builtin for stage Compute.");
            logs.Flush();
            EditorPanelExtension panel = Assert.Single(runtime.panels, value => value.id == "diagnostics.console");
            panel.isOpen = true;
            void Draw(float width)
            {
                using IDisposable scope = ui.Enter();
                ImGuiApi.NewFrame();
                ImGuiApi.SetNextWindowPos(new(0, 0));
                ImGuiApi.SetNextWindowSize(new(width, 700));
                using NativeStringLease nativeTestText0 = Utf8Codec.EncodeNullTerminated("Console Layout", allowsNull: false);
                _ = ImGuiApi.Raw.Begin((sbyte*)nativeTestText0.pointer, null, 0);
                Assert.True(panel.Draw(runtime.context));
                ImGuiApi.End();
                ImGuiApi.Render();
                Assert.Single(console.Capture().occurrences);
            }
            Draw(960); Draw(960);
            ImGuiWindow* card = default;
            for (int i = 0; i < context->Windows.Size; i++)
            {
                ImGuiWindow* window = context->Windows.Data[i];
                if (Marshal.PtrToStringUTF8((nint)window->Name)?.Contains("##ConsoleEntryCard", StringComparison.Ordinal) == true) card = window;
            }
            Assert.True(card != null);
            using NativeStringLease nativeTestText1 = Utf8Codec.EncodeNullTerminated("##HeaderTable", allowsNull: false);
            ImGuiTable* header = ImGuiApi.Raw.TableFindByID(ImGuiApi.Raw.GetID(card, (sbyte*)nativeTestText1.pointer, null));
            Assert.True(header != null);
            Vector2 toggle = new(header->Columns.Data[0].WorkMinX + 6, header->RowPosY1 + ImGuiApi.GetFontSize() / 2);
            ImGuiApi.Raw.AddMousePosEvent(ImGuiApi.Raw.GetIO(), toggle.X, toggle.Y);
            Draw(960);
            Assert.True(context->HoveredId != 0, $"No toggle hovered: {toggle}; window {Marshal.PtrToStringUTF8((nint)context->HoveredWindow->Name)}");
            ImGuiApi.Raw.AddMouseButtonEvent(ImGuiApi.Raw.GetIO(), 0, (true ? (byte)1 : (byte)0)); Draw(960);
            Assert.True(context->ActiveId != 0, "Toggle did not activate.");
            ImGuiApi.Raw.AddMouseButtonEvent(ImGuiApi.Raw.GetIO(), 0, (false ? (byte)1 : (byte)0)); Draw(960);
            // Switching from fixed height to AutoResizeY uses a native measurement frame.
            Draw(960);
            foreach (float width in new[] { 960f, 500f, 260f, 140f, 960f })
            {
                Draw(width);
                uint occurrenceId = ImGuiApi.Raw.GetID(card, unchecked((int)Assert.Single(console.Capture().occurrences).sequence));
                using NativeStringLease nativeTestText2 = Utf8Codec.EncodeNullTerminated("##ConsoleEntryDetails", allowsNull: false);
                ImGuiTable* table = ImGuiApi.Raw.TableFindByID(ImGuiApi.Raw.ImHashStr((sbyte*)nativeTestText2.pointer, 0, occurrenceId));
                bool details = table != null && table->LastFrameActive == ImGuiApi.GetFrameCount();
                if (details)
                {
                    ImGuiTableColumn label = table->Columns.Data[0], value = table->Columns.Data[1];
                    Assert.True(label.WidthGiven >= ImGuiApi.CalcTextSize("Session:").X - 1);
                    Assert.True(value.MinX > label.MinX + ImGuiApi.CalcTextSize("Source:").X);
                    Assert.True(label.ClipRect.Max.X >= label.WorkMinX + ImGuiApi.CalcTextSize("Source:").X);
                }
                if (width >= 500) Assert.True(details, $"Width {width}, details {details}, toggle {toggle}, card {card->Pos} / {card->Size}");
                if (width == 140) Assert.False(details);
            }
        }
        finally { ImGuiApi.Raw.DestroyContext(context); Directory.Delete(root, true); }
    }

    private sealed class RejectedConsole : IEditorConsole
    {
        public int capacity { get; set; }
        public bool clearOnPlay { get; set; }
        public EditorConsoleSnapshot Capture() => throw new InvalidOperationException("Snapshot source failed.");
        public void Clear() { }
    }

    private sealed class InactivePlayMode : IEditorPlayMode
    {
        public EditorPlayModeState state => EditorPlayModeState.Editing;
        public bool isPlaying => false;
        public string? lastFailure => null;
        public LogSessionId activeSessionId => LogSessionId.none;
        private readonly Inno.Core.Events.EventDispatcher m_stateDispatcher = new();
        public IDisposable ListenStateChanges(Action<EditorPlayModeState> listener) => m_stateDispatcher.CreateHub();

        public bool EnterPlayMode() => false;
        public bool ExitPlayMode() => false;
    }
}
