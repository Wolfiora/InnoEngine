using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System.Runtime.CompilerServices;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using BGCS.Runtime;
using System;
using System.Numerics;
using Inno.Native.ImGui;
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using NativeImGui = Inno.Native.ImGui.ImGui;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;
public sealed unsafe class EditorPopupLayoutTests
{
    [Fact]
    public void ContextMenuSearchUsesCompletePopupContentWidth()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            using var uiSession = new Inno.Editor.ImGui.EditorUiSession();
            using IDisposable uiScope = uiSession.Enter();
            ConfigureContext();
            EditorUI.current.widgets.SetupStyle();
            string query = string.Empty;
            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowSize(new Vector2(400f, 280f), (int)(ImGuiCond.Always));
                using NativeStringLease nativeTestText0 = Utf8Codec.EncodeNullTerminated("Context menu bounds", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText0.pointer, null, 0);
                _ = NativeImGui.Button("Target");
                NativeImGui.OpenPopup("commands");
                Assert.True(EditorUI.current.widgets.BeginContextMenu("commands"));
                _ = EditorUI.current.widgets.SearchInput("commands", "Search commands...", ref query);
                if (frame == 2)
                {
                    float inputWidth = NativeImGui.GetItemRectSize().X;
                    float contentWidth = NativeImGui.GetContentRegionAvail().X;
                    Assert.True(inputWidth >= EditorUI.current.widgets.style.searchPopupWidth - 1f);
                    Assert.InRange(MathF.Abs(inputWidth - contentWidth), 0f, 2f);
                    Assert.InRange(NativeImGui.GetScrollMaxX(), 0f, 1f);
                }

                NativeImGui.Selectable("Rename");
                EditorUI.current.widgets.EndContextMenu();
                NativeImGui.End();
                NativeImGui.Render();
            }
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void TwoItemSelectorOpensBelowControlWithoutOverflow()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            using var uiSession = new Inno.Editor.ImGui.EditorUiSession();
            using IDisposable uiScope = uiSession.Enter();
            ConfigureContext();
            EditorUI.current.widgets.SetupStyle();
            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), (int)(ImGuiCond.Always), default(Vector2));
                NativeImGui.SetNextWindowSize(new Vector2(300f, 250f), (int)(ImGuiCond.Always));
                using NativeStringLease nativeTestText1 = Utf8Codec.EncodeNullTerminated("Selector bounds", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText1.pointer, null, (int)(ImGuiWindowFlags.NoTitleBar));
                NativeImGui.SetNextItemWidth(140f);
                float triggerBottom = NativeImGui.GetCursorScreenPos().Y + NativeImGui.GetFrameHeight();
                NativeImGui.OpenPopup("##menu_selector_popup_tag");
                Assert.True(EditorUI.current.widgets.BeginBoundedCombo("tag", "Untagged"));
                Vector2 popupPosition = NativeImGui.GetWindowPos();
                Vector2 popupSize = NativeImGui.GetWindowSize();
                _ = NativeImGui.Selectable("Untagged");
                _ = NativeImGui.Selectable("Test");
                if (frame == 2)
                {
                    Assert.InRange(popupPosition.Y, triggerBottom - 1f, triggerBottom + 1f);
                    Assert.True(popupSize.Y <= 250f * 0.45f + 1f);
                    Assert.True(popupPosition.Y + popupSize.Y <= 270f + 1f);
                    Assert.InRange(NativeImGui.GetScrollMaxY(), 0f, 1f);
                }

                EditorUI.current.widgets.EndBoundedCombo();
                NativeImGui.End();
                NativeImGui.Render();
            }
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void LongSelectorNearWindowBottomScrollsWithoutOpeningUpward()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            using var uiSession = new Inno.Editor.ImGui.EditorUiSession();
            using IDisposable uiScope = uiSession.Enter();
            ConfigureContext();
            EditorUI.current.widgets.SetupStyle();
            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), (int)(ImGuiCond.Always), default(Vector2));
                NativeImGui.SetNextWindowSize(new Vector2(300f, 250f), (int)(ImGuiCond.Always));
                using NativeStringLease nativeTestText2 = Utf8Codec.EncodeNullTerminated("Selector near bottom", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText2.pointer, null, (int)(ImGuiWindowFlags.NoTitleBar));
                NativeImGui.SetCursorScreenPos(new Vector2(35f, 225f));
                float triggerBottom = NativeImGui.GetCursorScreenPos().Y + NativeImGui.GetFrameHeight();
                NativeImGui.SetNextItemWidth(140f);
                NativeImGui.OpenPopup("##menu_selector_popup_long");
                Assert.True(EditorUI.current.widgets.BeginBoundedCombo("long", "Choose"));
                Vector2 popupPosition = NativeImGui.GetWindowPos();
                Vector2 popupSize = NativeImGui.GetWindowSize();
                for (int index = 0; index < 40; index++)
                    _ = NativeImGui.Selectable($"Item {index}");
                if (frame == 2)
                {
                    Assert.InRange(popupPosition.Y, triggerBottom - 1f, triggerBottom + 1f);
                    Assert.True(popupPosition.Y + popupSize.Y <= 270f + 1f);
                    Assert.True(NativeImGui.GetScrollMaxY() > 0f);
                }

                EditorUI.current.widgets.EndBoundedCombo();
                NativeImGui.End();
                NativeImGui.Render();
            }
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void SelectorInsideLayoutChildUsesContainingPanelBounds()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            using var uiSession = new Inno.Editor.ImGui.EditorUiSession();
            using IDisposable uiScope = uiSession.Enter();
            ConfigureContext();
            EditorUI.current.widgets.SetupStyle();
            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), (int)(ImGuiCond.Always), default(Vector2));
                NativeImGui.SetNextWindowSize(new Vector2(300f, 250f), (int)(ImGuiCond.Always));
                using NativeStringLease nativeTestText3 = Utf8Codec.EncodeNullTerminated("Panel with layout child", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText3.pointer, null, (int)(ImGuiWindowFlags.NoTitleBar));
                _ = NativeImGui.BeginChild("Fields", new Vector2(200f, 55f), 0, 0);
                NativeImGui.SetNextItemWidth(140f);
                NativeImGui.OpenPopup("##menu_selector_popup_nested");
                Assert.True(EditorUI.current.widgets.BeginBoundedCombo("nested", "Choose"));
                Vector2 popupPosition = NativeImGui.GetWindowPos();
                Vector2 popupSize = NativeImGui.GetWindowSize();
                for (int index = 0; index < 40; index++)
                    _ = NativeImGui.Selectable($"Item {index}");
                if (frame == 2)
                {
                    Assert.True(popupSize.Y > 55f);
                    Assert.True(popupPosition.Y + popupSize.Y <= 270f + 1f);
                }

                EditorUI.current.widgets.EndBoundedCombo();
                NativeImGui.EndChild();
                NativeImGui.End();
                NativeImGui.Render();
            }
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    private static void ConfigureContext()
    {
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
        io->DeltaTime = 1f / 60f;
        io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
        io->Fonts->RendererHasTextures = 1;
    }
}
