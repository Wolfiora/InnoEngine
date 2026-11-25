using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using BGCS.Runtime;
using Xunit;
using Inno.Editor.ImGui;
using Inno.Native.ImGui;

using EditorImGui = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUISession;
using GuiChildFlags = Inno.Editor.ImGui.ImGuiChildFlags;
using GuiCol = Inno.Editor.ImGui.ImGuiCol;
using GuiCond = Inno.Editor.ImGui.ImGuiCond;
using GuiMouseButton = Inno.Editor.ImGui.ImGuiMouseButton;
using GuiStyleVar = Inno.Editor.ImGui.ImGuiStyleVar;
using GuiTableRowFlags = Inno.Editor.ImGui.ImGuiTableRowFlags;
using GuiTreeNodeOptions = Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions;
using GuiTreeNodeResult = Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult;
using GuiWindowFlags = Inno.Editor.ImGui.ImGuiWindowFlags;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.ImGui.Native.Tests;

public sealed unsafe class PopupLayoutTests
{
    [Fact]
    public void MenuSelectorPopupIsBoundedAndScrollsLongContent()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            NativeImGui.Raw.GetIO()->IniFilename = null;
            using var uiSession = new EditorUI();
            using IDisposable uiScope = uiSession.Enter();
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            for (int frame = 0; frame < 2; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(40f, 40f), (int)(GuiCond.Always), default);
                NativeImGui.SetNextWindowSize(new Vector2(480f, 320f), (int)(GuiCond.Always));
                using NativeStringLease nativeTestText6 = Utf8Codec.EncodeNullTerminated("Menu Selector Test", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText6.pointer, null, 0);
                if (frame == 0)
                {
                    NativeImGui.OpenPopup("##menu_selector_popup_test");
                }
                Assert.True(EditorUI.current.widgets.BeginMenuSelector("test", "Untagged", 180f, 240f));
                GuiWindowFlags flags = (GuiWindowFlags)NativeImGui.Raw.GetCurrentWindow()->Flags;
                Assert.True(flags.HasFlag(GuiWindowFlags.AlwaysAutoResize));
                Assert.False(flags.HasFlag(GuiWindowFlags.NoScrollbar));
                Assert.False(flags.HasFlag(GuiWindowFlags.NoScrollWithMouse));
                Assert.True(flags.HasFlag(GuiWindowFlags.NoSavedSettings));
                Assert.True(NativeImGui.GetWindowSize().X >= 240f);
                string newTag = string.Empty;
                NativeImGui.SetNextItemWidth(160f);
                _ = EditorImGui.InputTextWithHint("##new_tag", "Add tag...", ref newTag, 128);
                float inputCenterY = (NativeImGui.GetItemRectMin().Y + NativeImGui.GetItemRectMax().Y) * 0.5f;
                NativeImGui.SameLine(0f, -1f);
                _ = EditorUI.current.widgets.ClickableText(
                    "add_tag",
                    "+",
                    new Vector2(EditorUI.current.widgets.GetCompactIconSize().X, NativeImGui.GetFrameHeight())
                );
                float actionCenterY = (NativeImGui.GetItemRectMin().Y + NativeImGui.GetItemRectMax().Y) * 0.5f;
                Assert.Equal(inputCenterY, actionCenterY, 3);
                for (int index = 0; index < 64; index++)
                {
                    NativeImGui.Selectable($"Entry {index:D2}");
                }
                if (frame == 1)
                {
                    Assert.True(NativeImGui.GetWindowSize().Y <= io->DisplaySize.Y + 1f);
                    Assert.True(NativeImGui.GetScrollMaxY() > 0f);
                }

                EditorUI.current.widgets.EndMenuSelector();
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
    public void SmallMenuSelectorDoesNotReserveAScrollbar()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            NativeImGui.Raw.GetIO()->IniFilename = null;
            using var uiSession = new EditorUI();
            using IDisposable uiScope = uiSession.Enter();
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(40f, 40f), (int)(GuiCond.Always), default);
                NativeImGui.SetNextWindowSize(new Vector2(480f, 320f), (int)(GuiCond.Always));
                using NativeStringLease nativeTestText7 = Utf8Codec.EncodeNullTerminated("Small Menu Selector Test", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText7.pointer, null, 0);
                if (frame == 0)
                {
                    NativeImGui.OpenPopup("##menu_selector_popup_small");
                }
                Assert.True(EditorUI.current.widgets.BeginMenuSelector("small", "Current", 180f, 240f));
                for (int index = 0; index < 3; index++)
                {
                    NativeImGui.Selectable($"Entry {index}");
                }
                if (frame == 2)
                {
                    Assert.False((NativeImGui.Raw.GetCurrentWindow()->ScrollbarY != 0));
                    Assert.Equal(0f, NativeImGui.GetScrollMaxY(), 3);
                }

                EditorUI.current.widgets.EndMenuSelector();
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
    public void SmallContextMenuDoesNotReserveScrollbars()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            NativeImGui.Raw.GetIO()->IniFilename = null;
            using var uiSession = new EditorUI();
            using IDisposable uiScope = uiSession.Enter();
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                using NativeStringLease nativeTestText8 = Utf8Codec.EncodeNullTerminated("Small Context Menu Test", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText8.pointer, null, 0);
                NativeImGui.Selectable("Target");
                if (frame == 0)
                {
                    NativeImGui.OpenPopup("context");
                }
                Assert.True(EditorUI.current.widgets.BeginContextMenu("context"));
                NativeImGui.MenuItem("Rename", string.Empty, false, true);
                NativeImGui.MenuItem("Delete", string.Empty, false, true);
                if (frame == 2)
                {
                    Assert.False((NativeImGui.Raw.GetCurrentWindow()->ScrollbarX != 0));
                    Assert.False((NativeImGui.Raw.GetCurrentWindow()->ScrollbarY != 0));
                }

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
    public void BoundedComboPopupUsesTheSharedScrollingContract()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            NativeImGui.Raw.GetIO()->IniFilename = null;
            using var uiSession = new EditorUI();
            using IDisposable uiScope = uiSession.Enter();
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            List<float> popupWidths = [];
            int openFrames = 0;
            for (int frame = 0; frame < 5; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(400f, 340f), (int)(GuiCond.Always), default);
                NativeImGui.SetNextWindowSize(new Vector2(220f, 120f), (int)(GuiCond.Always));
                using NativeStringLease nativeTestText9 = Utf8Codec.EncodeNullTerminated("Bounded Combo Test", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText9.pointer, null, 0);
                uint parentViewportID = NativeImGui.Raw.GetWindowViewport()->ID;
                if (frame == 0)
                {
                    NativeImGui.OpenPopup("##menu_selector_popup_##asset");
                }

                NativeImGui.SetNextItemWidth(180f);
                bool open = EditorUI.current.widgets.BeginBoundedCombo("##asset", "project:Material");
                if (open)
                {
                    openFrames++;
                    Assert.Equal(parentViewportID, NativeImGui.Raw.GetWindowViewport()->ID);
                    string search = string.Empty;
                    _ = EditorUI.current.widgets.SearchInput("assets", "Search assets...", ref search);
                    for (int index = 0; index < 64; index++)
                    {
                        NativeImGui.Selectable($"project:Material{index:D2}");
                    }
                    popupWidths.Add(NativeImGui.GetWindowSize().X);
                    if (openFrames >= 2)
                    {
                        Assert.InRange(NativeImGui.GetWindowSize().X, 179f, 181f);
                        Assert.True(NativeImGui.GetWindowSize().Y <= io->DisplaySize.Y + 1f);
                        Assert.True(NativeImGui.GetScrollMaxY() > 0f);
                        ImGuiViewport* popupViewport = NativeImGui.Raw.GetWindowViewport();
                        Vector2 popupMinimum = NativeImGui.GetWindowPos();
                        Vector2 popupMaximum = popupMinimum + NativeImGui.GetWindowSize();
                        Assert.True(popupMinimum.X >= popupViewport->WorkPos.X - 1f);
                        Assert.True(popupMinimum.Y >= popupViewport->WorkPos.Y - 1f);
                        Assert.True(popupMaximum.X <= popupViewport->WorkPos.X + popupViewport->WorkSize.X + 1f);
                        Assert.True(popupMaximum.Y <= popupViewport->WorkPos.Y + popupViewport->WorkSize.Y + 1f);
                    }

                    EditorUI.current.widgets.EndBoundedCombo();
                }

                NativeImGui.End();
                NativeImGui.Render();
            }

            Assert.True(openFrames >= 2);
            Assert.All(popupWidths, width => Assert.InRange(width, 179f, 181f));
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }
}
