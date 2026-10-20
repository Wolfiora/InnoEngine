using System.Runtime.CompilerServices;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using BGCS.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Inno.Editor.ImGui;
using Inno.Native.ImGui;
using EditorImGui = Inno.Editor.ImGui.ImGui;
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using InlineRenameResult = Inno.Editor.ImGui.ImGuiWidget.InlineRenameResult;
using TreeNodeOptions = Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions;
using TreeNodeResult = Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult;
using NativeImGui = Inno.Native.ImGui.ImGui;

using Xunit;

namespace Inno.Editor.Scripting.Tests;

public sealed unsafe class EditorStyleMetricsTests
{
    [Fact]
    public void ZoomScalesDimensionsAndFontWhilePreservingRatios()
    {
        var metrics = new EditorStyleMetrics();

        Assert.True(metrics.SetZoom(1.5f));
        Assert.Equal(1.5f, metrics.zoom, 3);
        Assert.Equal(12f, metrics.windowPadding.X, 3);
        Assert.Equal(10.5f, metrics.windowPadding.Y, 3);
        Assert.Equal(3f, metrics.frameRounding, 3);
        Assert.Equal(1.8f, metrics.fontScale, 3);
        Assert.Equal(0.4f, metrics.propertyLabelRatio, 3);
        Assert.Equal(3f, metrics.assetGridDefaultScale, 3);
        Assert.Equal(new Vector2(7.5f, 4.5f), metrics.cellPadding);
    }

    [Fact]
    public void ZoomCommandsClampAndResetToTheSupportedRange()
    {
        var metrics = new EditorStyleMetrics();

        Assert.True(metrics.SetZoom(100f));
        Assert.Equal(EditorStyleMetrics.C_MAX_ZOOM, metrics.zoom);
        Assert.False(metrics.ZoomIn());
        Assert.True(metrics.SetZoom(-100f));
        Assert.Equal(EditorStyleMetrics.C_MIN_ZOOM, metrics.zoom);
        Assert.False(metrics.ZoomOut());
        Assert.True(metrics.ResetZoom());
        Assert.Equal(1f, metrics.zoom);
        Assert.Throws<ArgumentOutOfRangeException>(() => metrics.SetZoom(float.NaN));
    }

    [Fact]
    public void WrappedTextRendersThroughTheSafeLiteralTextPath()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            Inno.Native.ImGui.ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;

            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(260f, 180f), (int)(ImGuiCond.Always));
            using NativeStringLease nativeTestText0 = Utf8Codec.EncodeNullTerminated("Wrapped Text Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText0.pointer, null, 0);
            float lineHeight = NativeImGui.GetTextLineHeight();
            EditorWidget.WrappedText(
                "Literal %s text remains safe and wraps. This deliberately long compilation status " +
                "must stay inside the modal content width instead of extending beyond its right edge.");
            Assert.True(NativeImGui.GetItemRectSize().Y > lineHeight);
            Inno.Native.ImGui.ImDrawList* windowDrawList = NativeImGui.Raw.GetWindowDrawList();
            Inno.Native.ImGui.ImDrawList* foregroundDrawList = NativeImGui.Raw.GetForegroundDrawList(NativeImGui.Raw.GetMainViewport());
            int windowVertexCount = windowDrawList->VtxBuffer.Size;
            int foregroundVertexCount = foregroundDrawList->VtxBuffer.Size;
            EditorWidget.DropTargetHighlight(
                new Vector2(20f, 20f),
                new Vector2(120f, 80f));
            Assert.Equal(windowVertexCount, windowDrawList->VtxBuffer.Size);
            Assert.True(foregroundDrawList->VtxBuffer.Size > foregroundVertexCount);
            NativeImGui.End();
            NativeImGui.Render();
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void CenteredWrappedTextStaysInsidePaddingAndUsesMultipleLines()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;

            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(420f, 260f), (int)(ImGuiCond.Always));
            using NativeStringLease nativeTestText1 = Utf8Codec.EncodeNullTerminated("Centered Wrapped Text Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText1.pointer, null, 0);
            Vector2 origin = NativeImGui.GetCursorScreenPos();
            var area = new Vector2(320f, 180f);
            var padding = new Vector2(36f, 24f);
            ImDrawList* drawList = NativeImGui.Raw.GetWindowDrawList();
            int vertexStart = drawList->VtxBuffer.Size;
            uint textColor = NativeImGui.GetColorU32((int)(ImGuiCol.Text));

            EditorWidget.CenteredWrappedText(
                "A deliberately long viewport diagnostic wraps into several centered lines " +
                "without touching either horizontal edge of the presentation area.",
                area,
                padding);

            Assert.Equal(area, NativeImGui.GetItemRectSize());
            Vector2 minimum = new(float.MaxValue);
            Vector2 maximum = new(float.MinValue);
            for (int i = vertexStart; i < drawList->VtxBuffer.Size; i++)
            {
                ImDrawVert vertex = drawList->VtxBuffer.Data[i];
                if (vertex.Col != textColor)
                    continue;

                minimum = Vector2.Min(minimum, Unsafe.BitCast<NativeVector2, Vector2>(vertex.Pos));
                maximum = Vector2.Max(maximum, Unsafe.BitCast<NativeVector2, Vector2>(vertex.Pos));
            }

            Assert.NotEqual(float.MaxValue, minimum.X);
            Assert.True(minimum.X >= origin.X + padding.X - 1f);
            Assert.True(maximum.X <= origin.X + area.X - padding.X + 1f);
            Assert.True(minimum.Y >= origin.Y + padding.Y - 1f);
            Assert.True(maximum.Y <= origin.Y + area.Y - padding.Y + 1f);
            Assert.True(maximum.Y - minimum.Y > NativeImGui.GetTextLineHeight());
            NativeImGui.End();
            NativeImGui.Render();
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void InlineRenameIsCenteredAndSelectsTheCompleteValueWhenFocused()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            Inno.Native.ImGui.ImGuiIO* io = NativeImGui.Raw.GetIO();
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
            _ = EditorWidget.InlineRename(
                "scene",
                ref value,
                ref requestFocus,
                rowHeight,
                width: 180f);
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
            _ = EditorWidget.InlineRename(
                "scene",
                ref value,
                ref requestFocus,
                rowHeight,
                width: 180f);
            Vector2 focusedFieldMin = NativeImGui.GetItemRectMin();
            Vector2 focusedFieldMax = NativeImGui.GetItemRectMax();
            uint inputId = NativeImGui.GetItemID();
            ImGuiInputTextState* inputState = NativeImGui.Raw.GetInputTextState(inputId);

            Assert.False(requestFocus);
            Assert.False((inputState == null));
            Assert.Equal(0, NativeImGui.Raw.GetSelectionStart(inputState));
            Assert.Equal(value.Length, NativeImGui.Raw.GetSelectionEnd(inputState));
            uint navCursorColor = NativeImGui.GetColorU32((int)(ImGuiCol.NavCursor));
            AssertDrawListDoesNotContainColor(
                NativeImGui.Raw.GetWindowDrawList(),
                navCursorColor,
                "The inline rename focus outline was submitted to the window draw list.");
            AssertColorBoundsStayCloseToItem(
                NativeImGui.Raw.GetForegroundDrawList(NativeImGui.Raw.GetMainViewport()),
                navCursorColor,
                focusedFieldMin,
                focusedFieldMax,
                1f + EditorWidget.style.interactionOverlayThickness);
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
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            string value = "Untitled Scene";
            bool requestFocus = true;
            InlineRenameResult result = InlineRenameResult.None;
            Vector2 center = default;

            void Frame()
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(30f, 30f), (int)(ImGuiCond.Always), default(Vector2));
                NativeImGui.SetNextWindowSize(new Vector2(320f, 180f), (int)(ImGuiCond.Always));
                using NativeStringLease nativeTestText4 = Utf8Codec.EncodeNullTerminated("Inline Rename Context Test", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText4.pointer, null, 0);
                result = EditorWidget.InlineRename(
                    "created_asset",
                    ref value,
                    ref requestFocus,
                    NativeImGui.GetFrameHeight(),
                    width: 220f);
                center = (NativeImGui.GetItemRectMin() + NativeImGui.GetItemRectMax()) * 0.5f;
                NativeImGui.End();
                NativeImGui.Render();
            }

            Frame();
            Frame();
            Assert.False(requestFocus);
            Assert.NotEqual(0u, NativeImGui.Raw.GetActiveID());

            NativeImGui.Raw.AddMousePosEvent(io, center.X, center.Y);
            NativeImGui.Raw.AddMouseButtonEvent(io, (int)ImGuiMouseButton.Right, (true ? (byte)1 : (byte)0));
            Frame();
            NativeImGui.Raw.AddMouseButtonEvent(io, (int)ImGuiMouseButton.Right, (false ? (byte)1 : (byte)0));
            Frame();

            Assert.Equal(InlineRenameResult.FocusLost, result);
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
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
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
            NativeImGui.PushStyleVar((int)(ImGuiStyleVar.CellPadding), new Vector2(6f, 3f));
            Assert.True(NativeImGui.BeginTable("##rename_table", 1, 0, default(Vector2), 0f));
            float rowHeight = NativeImGui.GetTextLineHeight() +
                              NativeImGui.Raw.GetStyle()->CellPadding.Y * 2f;
            NativeImGui.TableNextRow((int)(ImGuiTableRowFlags.None), rowHeight);
            _ = NativeImGui.TableSetColumnIndex(0);
            ImGuiTable* table = NativeImGui.Raw.GetCurrentTable();
            Vector2 cursor = NativeImGui.GetCursorScreenPos();
            NativeImGui.SetCursorScreenPos(new Vector2(cursor.X, table->RowPosY1));

            _ = EditorWidget.InlineRename(
                "table_scene",
                ref value,
                ref requestFocus,
                table->RowPosY2 - table->RowPosY1,
                width: 180f);
            Vector2 fieldMin = NativeImGui.GetItemRectMin();
            Vector2 fieldMax = NativeImGui.GetItemRectMax();

            Assert.True(fieldMin.Y >= table->RowPosY1);
            Assert.True(fieldMax.Y <= table->RowPosY2);
            Assert.Equal(
                (table->RowPosY1 + table->RowPosY2) * 0.5f,
                (fieldMin.Y + fieldMax.Y) * 0.5f,
                3);
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

    [Fact]
    public void TreeGuideDepthRemainsBalancedAcrossSupportedZoomLevels()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            Inno.Native.ImGui.ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;

            foreach (float zoom in new[] { 0.75f, 1f, 1.25f, 1.5f })
            {
                _ = EditorWidget.style.SetZoom(zoom);
                EditorWidget.SetupStyle();
                DrawTreeFrame();
            }
        }
        finally
        {
            _ = EditorWidget.style.ResetZoom();
            EditorWidget.SetupStyle();
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void MenuSelectorPopupIsBoundedAndScrollsLongContent()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            Inno.Native.ImGui.ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;

            for (int frame = 0; frame < 2; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(40f, 40f), (int)(ImGuiCond.Always), default(Vector2));
                NativeImGui.SetNextWindowSize(new Vector2(480f, 320f), (int)(ImGuiCond.Always));
                using NativeStringLease nativeTestText6 = Utf8Codec.EncodeNullTerminated("Menu Selector Test", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText6.pointer, null, 0);
                if (frame == 0)
                    NativeImGui.OpenPopup("##menu_selector_popup_test");
                Assert.True(EditorWidget.BeginMenuSelector("test", "Untagged", 180f, 240f));
                ImGuiWindowFlags flags = (ImGuiWindowFlags)NativeImGui.Raw.GetCurrentWindow()->Flags;
                Assert.True(flags.HasFlag(ImGuiWindowFlags.AlwaysAutoResize));
                Assert.False(flags.HasFlag(ImGuiWindowFlags.NoScrollbar));
                Assert.False(flags.HasFlag(ImGuiWindowFlags.NoScrollWithMouse));
                Assert.True(flags.HasFlag(ImGuiWindowFlags.NoSavedSettings));
                Assert.True(NativeImGui.GetWindowSize().X >= 240f);
                string newTag = string.Empty;
                NativeImGui.SetNextItemWidth(160f);
                _ = EditorImGui.InputTextWithHint(
                    "##new_tag",
                    "Add tag...",
                    ref newTag,
                    128);
                float inputCenterY = (NativeImGui.GetItemRectMin().Y + NativeImGui.GetItemRectMax().Y) * 0.5f;
                NativeImGui.SameLine(0f, -1f);
                _ = EditorWidget.ClickableText(
                    "add_tag",
                    "+",
                    new Vector2(EditorWidget.GetCompactIconSize().X, NativeImGui.GetFrameHeight()));
                float actionCenterY = (NativeImGui.GetItemRectMin().Y + NativeImGui.GetItemRectMax().Y) * 0.5f;
                Assert.Equal(inputCenterY, actionCenterY, 3);
                for (int index = 0; index < 64; index++)
                    NativeImGui.Selectable($"Entry {index:D2}");
                if (frame == 1)
                {
                    Assert.True(NativeImGui.GetWindowSize().Y <= io->DisplaySize.Y + 1f);
                    Assert.True(NativeImGui.GetScrollMaxY() > 0f);
                }
                EditorWidget.EndMenuSelector();
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
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;

            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(40f, 40f), (int)(ImGuiCond.Always), default(Vector2));
                NativeImGui.SetNextWindowSize(new Vector2(480f, 320f), (int)(ImGuiCond.Always));
                using NativeStringLease nativeTestText7 = Utf8Codec.EncodeNullTerminated("Small Menu Selector Test", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText7.pointer, null, 0);
                if (frame == 0)
                    NativeImGui.OpenPopup("##menu_selector_popup_small");
                Assert.True(EditorWidget.BeginMenuSelector("small", "Current", 180f, 240f));
                for (int index = 0; index < 3; index++)
                    NativeImGui.Selectable($"Entry {index}");
                if (frame == 2)
                {
                    Assert.False((NativeImGui.Raw.GetCurrentWindow()->ScrollbarY != 0));
                    Assert.Equal(0f, NativeImGui.GetScrollMaxY(), 3);
                }
                EditorWidget.EndMenuSelector();
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
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
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
                    NativeImGui.OpenPopup("context");
                Assert.True(EditorWidget.BeginContextMenu("context"));
                NativeImGui.MenuItem("Rename", string.Empty, false, true);
                NativeImGui.MenuItem("Delete", string.Empty, false, true);
                if (frame == 2)
                {
                    Assert.False((NativeImGui.Raw.GetCurrentWindow()->ScrollbarX != 0));
                    Assert.False((NativeImGui.Raw.GetCurrentWindow()->ScrollbarY != 0));
                }
                EditorWidget.EndContextMenu();
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
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;

            var popupWidths = new List<float>();
            int openFrames = 0;
            for (int frame = 0; frame < 5; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(400f, 340f), (int)(ImGuiCond.Always), default(Vector2));
                NativeImGui.SetNextWindowSize(new Vector2(220f, 120f), (int)(ImGuiCond.Always));
                using NativeStringLease nativeTestText9 = Utf8Codec.EncodeNullTerminated("Bounded Combo Test", allowsNull: false);
                _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText9.pointer, null, 0);
                uint parentViewportId = NativeImGui.Raw.GetWindowViewport()->ID;
                if (frame == 0)
                {
                    NativeImGui.OpenPopup("##menu_selector_popup_##asset");
                }
                NativeImGui.SetNextItemWidth(180f);
                bool open = EditorWidget.BeginBoundedCombo("##asset", "project:Material");
                if (open)
                {
                    openFrames++;
                    Assert.Equal(parentViewportId, NativeImGui.Raw.GetWindowViewport()->ID);
                    string search = string.Empty;
                    _ = EditorWidget.SearchInput("assets", "Search assets...", ref search);
                    for (int index = 0; index < 64; index++)
                        NativeImGui.Selectable($"project:Material{index:D2}");
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
                    EditorWidget.EndBoundedCombo();
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

    [Fact]
    public void TreeRegionScrollsHorizontallyOnlyForRealContentOverflow()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;

            DrawTreeOverflowFrame(assertRanges: false);
            DrawTreeOverflowFrame(assertRanges: true);
            DrawHorizontallyScrolledTreeFrame(0f, assertGeometry: false);
            DrawHorizontallyScrolledTreeFrame(48f, assertGeometry: false);
            DrawHorizontallyScrolledTreeFrame(48f, assertGeometry: true);
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void TreeRegionReleasesViewportWidthAfterItShrinks()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;

            DrawResizableTreeFrame(420f, 0f, assertNoOverflow: false);
            DrawResizableTreeFrame(220f, 48f, assertNoOverflow: false);
            DrawResizableTreeFrame(220f, 48f, assertNoOverflow: true);
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void TreeGuideSegmentsRemainVisibleAcrossCompactRows()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            EditorWidget.SetupStyle();

            DrawTreeFrame();
            DrawTreeFrame(assertContinuousGuides: true);
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void ChildGuideRetainsAVisualGapBelowTheParentWithoutChangingCompactRowHeight()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            EditorWidget.SetupStyle();

            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(480f, 320f), (int)(ImGuiCond.Always));
            using NativeStringLease nativeTestText10 = Utf8Codec.EncodeNullTerminated("Gapped Parent Guide Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText10.pointer, null, 0);
            float expectedHeight = NativeImGui.GetTextLineHeight();
            EditorWidget.SetNextTreeNodeOpen(true);
            TreeNodeResult parent = EditorWidget.TreeNode(
                "connected_parent",
                static _ => NativeImGui.TextUnformatted("Parent"),
                new TreeNodeOptions());
            Assert.True(parent.isOpen);
            TreeNodeResult child = EditorWidget.TreeNode(
                "connected_child",
                static _ => NativeImGui.TextUnformatted("Child"),
                new TreeNodeOptions { isLeaf = true });
            NativeImGui.TreePop();

            Assert.Equal(expectedHeight, parent.max.Y - parent.min.Y, 3);
            Assert.Equal(expectedHeight, child.max.Y - child.min.Y, 3);
            AssertCurrentTreeGuideDoesNotTouchY((parent.min.Y + parent.max.Y) * 0.5f);
            NativeImGui.End();
            NativeImGui.Render();
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void TreeGuidesAreDrawnFromTheCurrentFrameWhileDragging()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            EditorWidget.SetupStyle();

            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(480f, 320f), (int)(ImGuiCond.Always));
            using NativeStringLease nativeTestText11 = Utf8Codec.EncodeNullTerminated("Dragging Tree Guide Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText11.pointer, null, 0);
            ImGuiNativeContext* currentContext = NativeImGui.Raw.GetCurrentContext();
            currentContext->DragDropActive = 1;
            EditorWidget.SetNextTreeNodeOpen(true);
            TreeNodeResult root = EditorWidget.TreeNode(
                "drag_root",
                static _ => NativeImGui.TextUnformatted("Assets"),
                new TreeNodeOptions());
            if (root.isOpen)
            {
                _ = EditorWidget.TreeNode(
                    "drag_child",
                    static _ => NativeImGui.TextUnformatted("Scene"),
                    new TreeNodeOptions { isLeaf = true });
                NativeImGui.TreePop();
            }
            AssertCurrentDrawListContainsTreeGuideColor();
            currentContext->DragDropActive = 0;
            NativeImGui.End();
            NativeImGui.Render();
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void TreeRowBackgroundsUseCurrentGeometryWhileTheWindowChanges()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            EditorWidget.SetupStyle();

            DrawCurrentTreeBackgroundFrame(
                new Vector2(20f, 20f),
                new Vector2(480f, 320f));
            DrawCurrentTreeBackgroundFrame(
                new Vector2(80f, 60f),
                new Vector2(300f, 220f));
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void TreeRowsRetainTheirCompactContentHeight()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            EditorWidget.SetupStyle();

            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(320f, 240f), (int)(ImGuiCond.Always));
            using NativeStringLease nativeTestText12 = Utf8Codec.EncodeNullTerminated("Compact Tree Row Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText12.pointer, null, 0);
            float expectedHeight = NativeImGui.GetTextLineHeight();
            TreeNodeResult row = EditorWidget.TreeNode(
                "compact_row",
                static _ => NativeImGui.TextUnformatted("GameObject"),
                new TreeNodeOptions { isLeaf = true });
            Assert.Equal(expectedHeight, row.max.Y - row.min.Y, 3);
            NativeImGui.End();
            NativeImGui.Render();
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void DoubleClickingTreeContentTogglesNonLeafExpansion()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            EditorWidget.SetupStyle();

            TreeNodeResult initial = DrawDoubleClickTreeFrame();
            Vector2 contentCenter = new(
                initial.contentMin.X + 20f,
                (initial.min.Y + initial.max.Y) * 0.5f);
            NativeImGui.Raw.AddMousePosEvent(io, contentCenter.X, contentCenter.Y);
            NativeImGui.Raw.AddMouseButtonEvent(io, 0, (true ? (byte)1 : (byte)0));
            _ = DrawDoubleClickTreeFrame();
            NativeImGui.Raw.AddMouseButtonEvent(io, 0, (false ? (byte)1 : (byte)0));
            _ = DrawDoubleClickTreeFrame();
            NativeImGui.Raw.AddMouseButtonEvent(io, 0, (true ? (byte)1 : (byte)0));
            TreeNodeResult doubleClicked = DrawDoubleClickTreeFrame();
            NativeImGui.Raw.AddMouseButtonEvent(io, 0, (false ? (byte)1 : (byte)0));
            TreeNodeResult expanded = DrawDoubleClickTreeFrame();

            Assert.True(doubleClicked.isDoubleClicked);
            Assert.True(expanded.isOpen);
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void InspectorContentPreservesStandardPaddingWithoutArtificialHorizontalOverflow()
    {
        var context = NativeImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            EditorWidget.SetupStyle();

            DrawInspectorCardFrame(assertHorizontalRange: false);
            DrawInspectorCardFrame(assertHorizontalRange: true);
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    private static void DrawTreeFrame(bool assertContinuousGuides = false)
    {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowSize(new Vector2(480f, 320f), (int)(Inno.Native.ImGui.ImGuiCond.Always));
        using NativeStringLease nativeTestText13 = Utf8Codec.EncodeNullTerminated("Tree Zoom Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText13.pointer, null, 0);
        EditorWidget.SetNextTreeNodeOpen(true);
        TreeNodeResult root = EditorWidget.TreeNode(
            "root",
            static _ => NativeImGui.TextUnformatted("Assets"),
            new TreeNodeOptions());
        if (root.isOpen)
        {
            EditorWidget.SetNextTreeNodeOpen(true);
            TreeNodeResult folder = EditorWidget.TreeNode(
                "folder",
                static _ => NativeImGui.TextUnformatted("Scene"),
                new TreeNodeOptions());
            if (folder.isOpen)
            {
                _ = EditorWidget.TreeNode(
                    "nested-file",
                    static _ => NativeImGui.TextUnformatted("TestScene1.iscene"),
                    new TreeNodeOptions { isLeaf = true });
                NativeImGui.TreePop();
            }

            _ = EditorWidget.TreeNode(
                "sibling-folder",
                static _ => NativeImGui.TextUnformatted("Settings"),
                new TreeNodeOptions { isLeaf = true });
            NativeImGui.TreePop();
        }
        if (assertContinuousGuides)
            AssertCurrentDrawListContainsTreeGuideColor();
        NativeImGui.End();
        NativeImGui.Render();
    }

    private static TreeNodeResult DrawDoubleClickTreeFrame()
    {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), (int)(ImGuiCond.Always), default(Vector2));
        NativeImGui.SetNextWindowSize(new Vector2(320f, 240f), (int)(ImGuiCond.Always));
        using NativeStringLease nativeTestText14 = Utf8Codec.EncodeNullTerminated("Double Click Tree Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText14.pointer, null, 0);
        TreeNodeResult result = EditorWidget.TreeNode(
            "double_click_parent",
            static _ => NativeImGui.TextUnformatted("Parent"),
            new TreeNodeOptions());
        if (result.isOpen)
        {
            _ = EditorWidget.TreeNode(
                "double_click_child",
                static _ => NativeImGui.TextUnformatted("Child"),
                new TreeNodeOptions { isLeaf = true });
            NativeImGui.TreePop();
        }
        NativeImGui.End();
        NativeImGui.Render();
        return result;
    }

    private static void DrawTreeOverflowFrame(bool assertRanges)
    {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowSize(new Vector2(560f, 320f), (int)(ImGuiCond.Always));
        using NativeStringLease nativeTestText15 = Utf8Codec.EncodeNullTerminated("Tree Overflow Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText15.pointer, null, 0);
        if (NativeImGui.BeginChild(
                "##short_tree",
                new Vector2(220f, 100f),
                (int)(ImGuiChildFlags.None),
                (int)(ImGuiWindowFlags.HorizontalScrollbar)))
        {
            _ = EditorWidget.TreeNode(
                "short",
                static _ => NativeImGui.TextUnformatted("Object"),
                new TreeNodeOptions { isLeaf = true });
            if (assertRanges)
            {
                Assert.Equal(0f, NativeImGui.GetScrollMaxX(), 3);
                Assert.False((NativeImGui.Raw.GetCurrentWindow()->ScrollbarX != 0));
                Assert.True((NativeImGui.Raw.GetCurrentWindow()->Flags & (int)ImGuiWindowFlags.HorizontalScrollbar) != 0);
            }
        }
        NativeImGui.EndChild();

        if (NativeImGui.BeginChild(
                "##long_tree",
                new Vector2(220f, 100f),
                (int)(ImGuiChildFlags.None),
                (int)(ImGuiWindowFlags.HorizontalScrollbar)))
        {
            _ = EditorWidget.TreeNode(
                "long",
                static _ => NativeImGui.TextUnformatted(
                    "Object_With_A_Name_That_Is_Intentionally_Wider_Than_The_Tree_Viewport"),
                new TreeNodeOptions { isLeaf = true });
            if (assertRanges)
            {
                Assert.True(NativeImGui.GetScrollMaxX() > 0f);
                Assert.True((NativeImGui.Raw.GetCurrentWindow()->ScrollbarX != 0));
                Assert.True((NativeImGui.Raw.GetCurrentWindow()->Flags & (int)ImGuiWindowFlags.HorizontalScrollbar) != 0);
            }
        }
        NativeImGui.EndChild();
        NativeImGui.End();
        NativeImGui.Render();
    }

    private static void DrawHorizontallyScrolledTreeFrame(
        float requestedScroll,
        bool assertGeometry
    ) {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowSize(new Vector2(360f, 220f), (int)(ImGuiCond.Always));
        using NativeStringLease nativeTestText16 = Utf8Codec.EncodeNullTerminated("Scrolled Tree Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText16.pointer, null, 0);
        NativeImGui.SetNextWindowScroll(new Vector2(requestedScroll, 0f));
        if (NativeImGui.BeginChild(
                "##scrolled_tree",
                new Vector2(220f, 120f),
                (int)(ImGuiChildFlags.None),
                (int)(ImGuiWindowFlags.HorizontalScrollbar)))
        {
            Vector2 contentCursor = default;
            TreeNodeResult row = EditorWidget.TreeNode(
                "scrolled",
                _ =>
                {
                    contentCursor = NativeImGui.GetCursorScreenPos();
                    NativeImGui.TextUnformatted(
                        "Scene_With_A_Name_That_Is_Intentionally_Wider_Than_The_Tree_Viewport");
                },
                new TreeNodeOptions
                {
                    isLeaf = true,
                    showBackground = true,
                    backgroundColor = EditorPalette.hierarchySceneRow
                });
            if (assertGeometry)
            {
                ImGuiWindow* window = NativeImGui.Raw.GetCurrentWindow();
                Assert.True(NativeImGui.GetScrollX() > 0f);
                Assert.True((window->ScrollbarX != 0));
                Assert.Equal(row.contentMin.X, contentCursor.X, 3);
                AssertCurrentTreeBackgroundSpansViewport(
                    window,
                    NativeImGui.ColorConvertFloat4ToU32(EditorPalette.hierarchySceneRow));
            }
        }
        NativeImGui.EndChild();
        NativeImGui.End();
        NativeImGui.Render();
    }

    private static void DrawResizableTreeFrame(
        float width,
        float requestedScroll,
        bool assertNoOverflow
    ) {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowSize(new Vector2(560f, 260f), (int)(ImGuiCond.Always));
        using NativeStringLease nativeTestText17 = Utf8Codec.EncodeNullTerminated("Resizable Tree Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText17.pointer, null, 0);
        NativeImGui.SetNextWindowScroll(new Vector2(requestedScroll, 0f));
        if (NativeImGui.BeginChild(
                "##resizable_tree",
                new Vector2(width, 120f),
                (int)(ImGuiChildFlags.None),
                (int)(ImGuiWindowFlags.HorizontalScrollbar)))
        {
            _ = EditorWidget.TreeNode(
                "resizable_row",
                static _ => NativeImGui.TextUnformatted("GameObject"),
                new TreeNodeOptions
                {
                    isLeaf = true,
                    drawViewportOverlay = static () =>
                    {
                        ImGuiWindow* window = NativeImGui.Raw.GetCurrentWindow();
                        Vector2 overlaySize = new(
                            NativeImGui.GetFontSize(),
                            NativeImGui.GetTextLineHeight());
                        NativeImGui.SameLine(0f, -1f);
                        Vector2 cursor = NativeImGui.GetCursorScreenPos();
                        NativeImGui.SetCursorScreenPos(new Vector2(
                            MathF.Max(cursor.X, window->WorkRect.Max.X - overlaySize.X),
                            cursor.Y));
                        NativeImGui.Dummy(overlaySize);
                    }
                });
            if (assertNoOverflow)
            {
                Assert.Equal(0f, NativeImGui.GetScrollX(), 3);
                Assert.Equal(0f, NativeImGui.GetScrollMaxX(), 3);
                Assert.False((NativeImGui.Raw.GetCurrentWindow()->ScrollbarX != 0));
            }
        }
        NativeImGui.EndChild();
        NativeImGui.End();
        NativeImGui.Render();
    }

    private static void AssertCurrentDrawListContainsTreeGuideColor()
    {
        uint guideColor = NativeImGui.ColorConvertFloat4ToU32(EditorPalette.treeGuide);
        AssertCurrentDrawListContainsColor(
            guideColor,
            "The current drag frame did not submit any tree-guide vertices.");
    }

    private static void AssertCurrentTreeGuideDoesNotTouchY(float excludedY)
    {
        ImDrawList* drawList = NativeImGui.Raw.GetWindowDrawList();
        uint guideColor = NativeImGui.ColorConvertFloat4ToU32(EditorPalette.treeGuide);
        for (int i = 0; i < drawList->VtxBuffer.Size; i++)
        {
            ImDrawVert vertex = drawList->VtxBuffer.Data[i];
            Assert.False(
                vertex.Col == guideColor && MathF.Abs(vertex.Pos.Y - excludedY) <= 1f,
                "The child guide overlapped the parent disclosure row instead of retaining its visual gap.");
        }
    }

    private static void DrawCurrentTreeBackgroundFrame(
        Vector2 position,
        Vector2 size
    ) {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowPos(position, (int)(ImGuiCond.Always), default(Vector2));
        NativeImGui.SetNextWindowSize(size, (int)(ImGuiCond.Always));
        using NativeStringLease nativeTestText18 = Utf8Codec.EncodeNullTerminated("Current Tree Background Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText18.pointer, null, 0);
        EditorWidget.SetNextTreeNodeOpen(true);
        TreeNodeResult root = EditorWidget.TreeNode(
            "current_background_root",
            static _ => NativeImGui.TextUnformatted("TestScene"),
            new TreeNodeOptions
            {
                showBackground = true,
                backgroundColor = EditorPalette.hierarchySceneRow,
                suppressHoverHighlight = true
            });
        if (root.isOpen)
        {
            _ = EditorWidget.TreeNode(
                "current_background_child",
                static _ => NativeImGui.TextUnformatted("GameObject"),
                new TreeNodeOptions
                {
                    isLeaf = true,
                    selected = true
                });
            NativeImGui.TreePop();
        }

        AssertCurrentDrawListContainsColor(
            NativeImGui.ColorConvertFloat4ToU32(EditorPalette.hierarchySceneRow),
            "The current frame did not submit the Scene row background.");
        AssertCurrentDrawListContainsColor(
            NativeImGui.GetColorU32((int)(ImGuiCol.Header)),
            "The current frame did not submit the selected row highlight.");
        NativeImGui.End();
        NativeImGui.Render();
    }

    private static void AssertCurrentDrawListContainsColor(
        uint color,
        string failureMessage
    ) {
        ImDrawList* drawList = NativeImGui.Raw.GetWindowDrawList();
        for (int i = 0; i < drawList->VtxBuffer.Size; i++)
        {
            if (drawList->VtxBuffer.Data[i].Col == color)
                return;
        }

        Assert.Fail(failureMessage);
    }

    private static void AssertDrawListDoesNotContainColor(
        ImDrawList* drawList,
        uint color,
        string failureMessage
    ) {
        for (int i = 0; i < drawList->VtxBuffer.Size; i++)
        {
            if (drawList->VtxBuffer.Data[i].Col == color)
                Assert.Fail(failureMessage);
        }
    }

    private static void AssertColorBoundsStayCloseToItem(
        ImDrawList* drawList,
        uint color,
        Vector2 itemMin,
        Vector2 itemMax,
        float maximumExpansion
    ) {
        Vector2 colorMin = new(float.MaxValue);
        Vector2 colorMax = new(float.MinValue);
        for (int i = 0; i < drawList->VtxBuffer.Size; i++)
        {
            ImDrawVert vertex = drawList->VtxBuffer.Data[i];
            if (vertex.Col != color)
                continue;

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

    private static void AssertCurrentTreeBackgroundSpansViewport(
        ImGuiWindow* window,
        uint color
    ) {
        ImDrawList* drawList = NativeImGui.Raw.GetWindowDrawList();
        float minimumX = float.MaxValue;
        float maximumX = float.MinValue;
        for (int i = 0; i < drawList->VtxBuffer.Size; i++)
        {
            ImDrawVert vertex = drawList->VtxBuffer.Data[i];
            if (vertex.Col != color)
                continue;

            minimumX = MathF.Min(minimumX, vertex.Pos.X);
            maximumX = MathF.Max(maximumX, vertex.Pos.X);
        }

        Assert.NotEqual(float.MaxValue, minimumX);
        Assert.Equal(NativeImGui.GetWindowPos().X, minimumX, 3);
        Assert.Equal(window->WorkRect.Max.X, maximumX, 3);
    }

    private static void DrawInspectorCardFrame(bool assertHorizontalRange)
    {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowSize(new Vector2(320f, 260f), (int)(ImGuiCond.Always));
        bool isOpen = true;
        EditorWidget.PanelWindow(
            "Inspector Width Test",
            ref isOpen,
            () => DrawInspectorCardBody(assertHorizontalRange),
            ImGuiWindowFlags.NoSavedSettings,
            useWindowPadding: false);
        NativeImGui.Render();
    }

    private static void DrawInspectorCardBody(bool assertHorizontalRange)
    {
        ImGuiWindow* panelWindow = NativeImGui.Raw.GetCurrentWindow();
        Vector2 panelBodyMinimum = NativeImGui.GetCursorScreenPos();
        Vector2 panelBodySize = NativeImGui.GetContentRegionAvail();
        if (assertHorizontalRange)
        {
            Assert.Equal(Vector2.Zero, Unsafe.BitCast<NativeVector2, Vector2>(panelWindow->WindowPadding));
            Assert.Equal(NativeImGui.GetWindowPos().X, panelBodyMinimum.X);
        }

        if (NativeImGui.BeginChild("##inspector_scroll", Vector2.Zero, 0, 0))
        {
            if (assertHorizontalRange)
            {
                Assert.Equal(panelBodyMinimum, NativeImGui.GetWindowPos());
                Assert.Equal(panelBodySize, NativeImGui.GetWindowSize());
            }

            EditorWidget.ConstrainedContent(
                "##inspector_content",
                () =>
                {
                    if (assertHorizontalRange)
                    {
                        Assert.Equal(
                            EditorWidget.style.windowPadding,
                            Unsafe.BitCast<NativeVector2, Vector2>(NativeImGui.Raw.GetCurrentWindow()->WindowPadding));
                        Assert.Equal(
                            NativeImGui.GetWindowPos() + EditorWidget.style.windowPadding,
                            NativeImGui.GetCursorScreenPos());
                    }

                    EditorWidget.LabelChip("Tag", EditorPalette.inspectorTagLabel);
                    float tagMaximumX = NativeImGui.GetItemRectMax().X;
                    NativeImGui.SameLine(0f, 0f);
                    NativeImGui.Dummy(new Vector2(80f, NativeImGui.GetFrameHeight()));
                    if (assertHorizontalRange)
                        Assert.Equal(tagMaximumX, NativeImGui.GetItemRectMin().X, 3);
                    NativeImGui.SameLine(0f, EditorWidget.style.inspectorHeaderSectionSpacing);
                    EditorWidget.LabelChip("Layer", EditorPalette.inspectorLayerLabel);
                    float layerMaximumX = NativeImGui.GetItemRectMax().X;
                    NativeImGui.SameLine(0f, 0f);
                    NativeImGui.Dummy(new Vector2(80f, NativeImGui.GetFrameHeight()));
                    if (assertHorizontalRange)
                        Assert.Equal(layerMaximumX, NativeImGui.GetItemRectMin().X, 3);

                    bool open = EditorWidget.CollapsingCard(
                        "long_behavior",
                        "BehaviorWithAnIntentionallyLongTypeNameThatMustBeClipped",
                        drawLeadingControl: static () => NativeImGui.Dummy(
                            new Vector2(NativeImGui.GetFrameHeight())),
                        drawTrailingControl: static () => NativeImGui.Dummy(
                            new Vector2(48f, NativeImGui.GetFrameHeight())),
                        trailingControlWidth: 48f);
                    if (open)
                    {
                        NativeImGui.Unindent();
                        EditorWidget.CardBody(
                            "long_behavior",
                            static () => EditorWidget.PropertyRow(
                                "long_property",
                                "Property With A Very Long Display Name",
                                static () => NativeImGui.Dummy(new Vector2(
                                    MathF.Max(1f, NativeImGui.GetContentRegionAvail().X),
                                    NativeImGui.GetFrameHeight()))));
                        NativeImGui.Indent();
                        NativeImGui.TreePop();
                        NativeImGui.Dummy(Vector2.Zero);
                    }
                    if (assertHorizontalRange)
                        Assert.Equal(0f, NativeImGui.GetScrollMaxX(), 3);
                });
            if (assertHorizontalRange)
                Assert.Equal(0f, NativeImGui.GetScrollMaxX(), 3);
        }
        NativeImGui.EndChild();
    }

}
