using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using BGCS.Runtime;
using Xunit;
using Inno.Editor.ImGui;
using Inno.Native.ImGui;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using GuiChildFlags = Inno.Editor.ImGui.ImGuiChildFlags;
using GuiCol = Inno.Editor.ImGui.ImGuiCol;
using GuiCond = Inno.Editor.ImGui.ImGuiCond;
using GuiInlineRenameResult = Inno.Editor.ImGui.ImGuiWidget.InlineRenameResult;
using GuiMouseButton = Inno.Editor.ImGui.ImGuiMouseButton;
using GuiStyleVar = Inno.Editor.ImGui.ImGuiStyleVar;
using GuiTableRowFlags = Inno.Editor.ImGui.ImGuiTableRowFlags;
using GuiTreeNodeResult = Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult;
using GuiWindowFlags = Inno.Editor.ImGui.ImGuiWindowFlags;
using NativeCond = Inno.Native.ImGui.ImGuiCond;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.ImGui.Native.Tests;

public sealed unsafe class InlineRenameLayoutTests
{
    [Fact]
    public void InlineRenameIsCenteredAndSelectsTheCompleteValueWhenFocused()
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
            string value = "Scene";
            bool requestFocus = true;
            NativeImGui.NewFrame();
            using NativeStringLease nativeTestText2 = Utf8Codec.EncodeNullTerminated("Inline Rename Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText2.pointer, null, 0);
            Vector2 rowMin = NativeImGui.GetCursorScreenPos();
            float rowHeight = NativeImGui.GetFrameHeight() + 8f;
            _ = EditorUI.current.widgets.InlineRename("scene", ref value, ref requestFocus, rowHeight, width: 180f);
            Vector2 fieldMin = NativeImGui.GetItemRectMin();
            Vector2 fieldMax = NativeImGui.GetItemRectMax();
            Assert.True(fieldMin.Y >= rowMin.Y);
            Assert.True(fieldMax.Y <= rowMin.Y + rowHeight);
            Assert.True(fieldMax.Y - fieldMin.Y < rowHeight);
            Assert.Equal(rowMin.Y + rowHeight * 0.5f, (fieldMin.Y + fieldMax.Y) * 0.5f, 3);
            NativeImGui.TextUnformatted("Content after inline rename.");
            Assert.NotEqual(0u, NativeImGui.Raw.GetCurrentWindow()->ID);
            NativeImGui.End();
            NativeImGui.Render();
            NativeImGui.NewFrame();
            using NativeStringLease nativeTestText3 = Utf8Codec.EncodeNullTerminated("Inline Rename Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText3.pointer, null, 0);
            _ = EditorUI.current.widgets.InlineRename("scene", ref value, ref requestFocus, rowHeight, width: 180f);
            Vector2 focusedFieldMin = NativeImGui.GetItemRectMin();
            Vector2 focusedFieldMax = NativeImGui.GetItemRectMax();
            uint inputID = NativeImGui.GetItemID();
            ImGuiInputTextState* inputState = NativeImGui.Raw.GetInputTextState(inputID);
            Assert.False(requestFocus);
            Assert.False((inputState is null));
            Assert.Equal(0, NativeImGui.Raw.GetSelectionStart(inputState));
            Assert.Equal(value.Length, NativeImGui.Raw.GetSelectionEnd(inputState));
            uint navCursorColor = NativeImGui.GetColorU32((int)(GuiCol.NavCursor));
            AssertDrawListDoesNotContainColor(
                NativeImGui.Raw.GetWindowDrawList(),
                navCursorColor,
                "The inline rename focus outline was submitted to the window draw list."
            );
            AssertColorBoundsStayCloseToItem(
                NativeImGui.Raw.GetForegroundDrawList(NativeImGui.Raw.GetMainViewport()),
                navCursorColor,
                focusedFieldMin,
                focusedFieldMax,
                1f + EditorUI.current.widgets.style.interactionOverlayThickness
            );
            NativeImGui.End();
            NativeImGui.Render();
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void InlineRenameReleasesFocusBeforeAContextMenuCanOpen()
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
            string value = "Untitled Scene";
            bool requestFocus = true;
            GuiInlineRenameResult result = GuiInlineRenameResult.None;
            Vector2 center = default;
            void Frame()
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(30f, 30f), (int)(GuiCond.Always), default);
                NativeImGui.SetNextWindowSize(new Vector2(320f, 180f), (int)(GuiCond.Always));
                using NativeStringLease nativeTestText4 = Utf8Codec.EncodeNullTerminated("Inline Rename Context Test", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText4.pointer, null, 0);
                result = EditorUI.current.widgets.InlineRename(
                    "created_asset",
                    ref value,
                    ref requestFocus,
                    NativeImGui.GetFrameHeight(),
                    width: 220f
                );
                center = (NativeImGui.GetItemRectMin() + NativeImGui.GetItemRectMax()) * 0.5f;
                NativeImGui.End();
                NativeImGui.Render();
            }

            Frame();
            Frame();
            Assert.False(requestFocus);
            Assert.NotEqual(0u, NativeImGui.Raw.GetActiveID());
            NativeImGui.Raw.AddMousePosEvent(io, center.X, center.Y);
            NativeImGui.Raw.AddMouseButtonEvent(io, (int)GuiMouseButton.Right, (true ? (byte)1 : (byte)0));
            Frame();
            NativeImGui.Raw.AddMouseButtonEvent(io, (int)GuiMouseButton.Right, (false ? (byte)1 : (byte)0));
            Frame();
            Assert.Equal(GuiInlineRenameResult.FocusLost, result);
            Assert.Equal(0u, NativeImGui.Raw.GetActiveID());
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void InlineRenameCentersWithinTheNativeTableRowBounds()
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
            string value = "Scene";
            bool requestFocus = false;
            NativeImGui.NewFrame();
            using NativeStringLease nativeTestText5 = Utf8Codec.EncodeNullTerminated("Inline Rename Table Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText5.pointer, null, 0);
            NativeImGui.PushStyleVar((int)(GuiStyleVar.CellPadding), new Vector2(6f, 3f));
            Assert.True(NativeImGui.BeginTable("##rename_table", 1, 0, default, 0f));
            float rowHeight = NativeImGui.GetTextLineHeight() + NativeImGui.Raw.GetStyle()->CellPadding.Y * 2f;
            NativeImGui.TableNextRow((int)(GuiTableRowFlags.None), rowHeight);
            _ = NativeImGui.TableSetColumnIndex(0);
            ImGuiTable* table = NativeImGui.Raw.GetCurrentTable();
            Vector2 cursor = NativeImGui.GetCursorScreenPos();
            NativeImGui.SetCursorScreenPos(new Vector2(cursor.X, table->RowPosY1));
            _ = EditorUI.current.widgets.InlineRename(
                "table_scene",
                ref value,
                ref requestFocus,
                table->RowPosY2 - table->RowPosY1,
                width: 180f
            );
            Vector2 fieldMin = NativeImGui.GetItemRectMin();
            Vector2 fieldMax = NativeImGui.GetItemRectMax();
            Assert.True(fieldMin.Y >= table->RowPosY1);
            Assert.True(fieldMax.Y <= table->RowPosY2);
            Assert.Equal((table->RowPosY1 + table->RowPosY2) * 0.5f, (fieldMin.Y + fieldMax.Y) * 0.5f, 3);
            NativeImGui.EndTable();
            NativeImGui.PopStyleVar();
            NativeImGui.End();
            NativeImGui.Render();
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    private static void AssertDrawListDoesNotContainColor(
        ImDrawList* drawList,
        uint color,
        string failureMessage
    ) {
        for (int i = 0; i < drawList->VtxBuffer.Size; i++)
        {
            if (drawList->VtxBuffer.Data[i].Col == color)
            {
                Assert.Fail(failureMessage);
            }
        }
    }

    private static void AssertColorBoundsStayCloseToItem(
        ImDrawList* drawList,
        uint color,
        Vector2 itemMin,
        Vector2 itemMax,
        float maximumExpansion
    ) {
        var colorMin = new Vector2(float.MaxValue);
        var colorMax = new Vector2(float.MinValue);
        for (int i = 0; i < drawList->VtxBuffer.Size; i++)
        {
            ImDrawVert vertex = drawList->VtxBuffer.Data[i];
            if (vertex.Col != color)
            {
                continue;
            }
            colorMin = Vector2.Min(colorMin, Unsafe.BitCast<NativeVector2, Vector2>(vertex.Pos));
            colorMax = Vector2.Max(colorMax, Unsafe.BitCast<NativeVector2, Vector2>(vertex.Pos));
        }

        Assert.NotEqual(float.MaxValue, colorMin.X);
        Assert.True(colorMin.X < itemMin.X);
        Assert.True(colorMin.Y < itemMin.Y);
        Assert.True(colorMax.X > itemMax.X);
        Assert.True(colorMax.Y > itemMax.Y);
        Assert.True(colorMin.X >= itemMin.X - maximumExpansion);
        Assert.True(colorMin.Y >= itemMin.Y - maximumExpansion);
        Assert.True(colorMax.X <= itemMax.X + maximumExpansion);
        Assert.True(colorMax.Y <= itemMax.Y + maximumExpansion);
    }
}
