using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using BGCS.Runtime;
using Inno.Adapter.Modules.DotNet;
using Inno.Core.Diagnostics;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Interactions;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using GuiCond = Inno.Editor.ImGui.ImGuiCond;
using GuiWindowFlags = Inno.Editor.ImGui.ImGuiWindowFlags;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.ImGui.Native.Tests;

public abstract unsafe class WidgetLayoutFixture
{
    protected static void BeginWindow(
        string name,
        Vector2 size
    ) {
        NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), (int)(GuiCond.Always), default);
        NativeImGui.SetNextWindowSize(size, (int)(GuiCond.Always));
        using NativeStringLease nativeTestText0 = Utf8Codec.EncodeNullTerminated(name, allowsNull: false);
        NativeImGui.Raw.Begin(
            (sbyte*)nativeTestText0.pointer,
            null,
            (int)(GuiWindowFlags.NoTitleBar | GuiWindowFlags.NoResize | GuiWindowFlags.NoMove | GuiWindowFlags.NoSavedSettings)
        );
    }

    protected sealed class NativeContext : IDisposable
    {
        private readonly ImGuiNativeContext* m_context;
        private readonly EditorUI m_ui = new();
        private readonly IDisposable m_uiScope;

        public NativeContext(float zoom)
        {
            m_context = NativeImGui.Raw.CreateContext(null);
            m_uiScope = m_ui.Enter();
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(1024f, 768f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            io->IniFilename = null;
            EditorUI.current.widgets.style.SetZoom(zoom);
            EditorUI.current.widgets.SetupStyle();
        }

        public void Frame(Action draw)
        {
            NativeImGui.NewFrame();
            draw();
            NativeImGui.Render();
        }

        public void Click(
            Vector2 position,
            int button,
            Action draw,
            bool settleHover = true
        ) {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            NativeImGui.Raw.AddMousePosEvent(io, position.X, position.Y);
            if (settleHover)
            {
                Frame(draw);
            }
            NativeImGui.Raw.AddMouseButtonEvent(io, button, (true ? (byte)1 : (byte)0));
            Frame(draw);
            NativeImGui.Raw.AddMouseButtonEvent(io, button, (false ? (byte)1 : (byte)0));
            Frame(draw);
        }

        public void Dispose()
        {
            m_uiScope.Dispose();
            m_ui.Dispose();
            NativeImGui.Raw.DestroyContext(m_context);
        }
    }
}
