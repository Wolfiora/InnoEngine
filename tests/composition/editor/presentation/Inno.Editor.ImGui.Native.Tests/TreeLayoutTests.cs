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
using GuiMouseButton = Inno.Editor.ImGui.ImGuiMouseButton;
using GuiStyleVar = Inno.Editor.ImGui.ImGuiStyleVar;
using GuiTableRowFlags = Inno.Editor.ImGui.ImGuiTableRowFlags;
using GuiTreeNodeOptions = Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions;
using GuiTreeNodeResult = Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult;
using GuiWindowFlags = Inno.Editor.ImGui.ImGuiWindowFlags;
using NativeCond = Inno.Native.ImGui.ImGuiCond;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.ImGui.Native.Tests;

public sealed unsafe class TreeLayoutTests
{
    [Fact]
    public void TreeGuideDepthRemainsBalancedAcrossSupportedZoomLevels()
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
            foreach (float zoom in new[]
            {
                0.75f,
                1f,
                1.25f,
                1.5f
            }

            ) {
                _ = EditorUI.current.widgets.style.SetZoom(zoom);
                EditorUI.current.widgets.SetupStyle();
                DrawTreeFrame();
            }
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void TreeRegionScrollsHorizontallyOnlyForRealContentOverflow()
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
            EditorUI.current.widgets.SetupStyle();
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
            EditorUI.current.widgets.SetupStyle();
            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(480f, 320f), (int)(GuiCond.Always));
            using NativeStringLease nativeTestText10 = Utf8Codec.EncodeNullTerminated("Gapped Parent Guide Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText10.pointer, null, 0);
            float expectedHeight = NativeImGui.GetTextLineHeight();
            EditorUI.current.widgets.SetNextTreeNodeOpen(true);
            GuiTreeNodeResult parent = EditorUI.current.widgets.TreeNode(
                "connected_parent",
                static _ => NativeImGui.TextUnformatted("Parent"),
                new GuiTreeNodeOptions()
            );
            Assert.True(parent.isOpen);
            GuiTreeNodeResult child = EditorUI.current.widgets.TreeNode(
                "connected_child",
                static _ => NativeImGui.TextUnformatted("Child"),
                new GuiTreeNodeOptions { isLeaf = true }
            );
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
            EditorUI.current.widgets.SetupStyle();
            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(480f, 320f), (int)(GuiCond.Always));
            using NativeStringLease nativeTestText11 = Utf8Codec.EncodeNullTerminated("Dragging Tree Guide Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText11.pointer, null, 0);
            ImGuiNativeContext* currentContext = NativeImGui.Raw.GetCurrentContext();
            currentContext->DragDropActive = 1;
            EditorUI.current.widgets.SetNextTreeNodeOpen(true);
            GuiTreeNodeResult root = EditorUI.current.widgets.TreeNode(
                "drag_root",
                static _ => NativeImGui.TextUnformatted("Assets"),
                new GuiTreeNodeOptions()
            );
            if (root.isOpen)
            {
                _ = EditorUI.current.widgets.TreeNode(
                    "drag_child",
                    static _ => NativeImGui.TextUnformatted("Scene"),
                    new GuiTreeNodeOptions { isLeaf = true }
                );
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
            EditorUI.current.widgets.SetupStyle();
            DrawCurrentTreeBackgroundFrame(new Vector2(20f, 20f), new Vector2(480f, 320f));
            DrawCurrentTreeBackgroundFrame(new Vector2(80f, 60f), new Vector2(300f, 220f));
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void TreeRowsRetainTheirCompactContentHeight()
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
            EditorUI.current.widgets.SetupStyle();
            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(320f, 240f), (int)(GuiCond.Always));
            using NativeStringLease nativeTestText12 = Utf8Codec.EncodeNullTerminated("Compact Tree Row Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText12.pointer, null, 0);
            float expectedHeight = NativeImGui.GetTextLineHeight();
            GuiTreeNodeResult row = EditorUI.current.widgets.TreeNode(
                "compact_row",
                static _ => NativeImGui.TextUnformatted("GameObject"),
                new GuiTreeNodeOptions { isLeaf = true }
            );
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
            EditorUI.current.widgets.SetupStyle();
            GuiTreeNodeResult initial = DrawDoubleClickTreeFrame();
            var contentCenter = new Vector2(initial.contentMin.X + 20f, (initial.min.Y + initial.max.Y) * 0.5f);
            NativeImGui.Raw.AddMousePosEvent(io, contentCenter.X, contentCenter.Y);
            NativeImGui.Raw.AddMouseButtonEvent(io, 0, (true ? (byte)1 : (byte)0));
            _ = DrawDoubleClickTreeFrame();
            NativeImGui.Raw.AddMouseButtonEvent(io, 0, (false ? (byte)1 : (byte)0));
            _ = DrawDoubleClickTreeFrame();
            NativeImGui.Raw.AddMouseButtonEvent(io, 0, (true ? (byte)1 : (byte)0));
            GuiTreeNodeResult doubleClicked = DrawDoubleClickTreeFrame();
            NativeImGui.Raw.AddMouseButtonEvent(io, 0, (false ? (byte)1 : (byte)0));
            GuiTreeNodeResult expanded = DrawDoubleClickTreeFrame();
            Assert.True(doubleClicked.isDoubleClicked);
            Assert.True(expanded.isOpen);
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    private static void DrawTreeFrame(bool assertContinuousGuides = false)
    {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowSize(new Vector2(480f, 320f), (int)(NativeCond.Always));
        using NativeStringLease nativeTestText13 = Utf8Codec.EncodeNullTerminated("Tree Zoom Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText13.pointer, null, 0);
        EditorUI.current.widgets.SetNextTreeNodeOpen(true);
        GuiTreeNodeResult root = EditorUI.current.widgets.TreeNode(
            "root",
            static _ => NativeImGui.TextUnformatted("Assets"),
            new GuiTreeNodeOptions()
        );
        if (root.isOpen)
        {
            EditorUI.current.widgets.SetNextTreeNodeOpen(true);
            GuiTreeNodeResult folder = EditorUI.current.widgets.TreeNode(
                "folder",
                static _ => NativeImGui.TextUnformatted("Scene"),
                new GuiTreeNodeOptions()
            );
            if (folder.isOpen)
            {
                _ = EditorUI.current.widgets.TreeNode(
                    "nested-file",
                    static _ => NativeImGui.TextUnformatted("TestScene1.iscene"),
                    new GuiTreeNodeOptions { isLeaf = true }
                );
                NativeImGui.TreePop();
            }

            _ = EditorUI.current.widgets.TreeNode(
                "sibling-folder",
                static _ => NativeImGui.TextUnformatted("Settings"),
                new GuiTreeNodeOptions { isLeaf = true }
            );
            NativeImGui.TreePop();
        }

        if (assertContinuousGuides)
        {
            AssertCurrentDrawListContainsTreeGuideColor();
        }
        NativeImGui.End();
        NativeImGui.Render();
    }

    private static GuiTreeNodeResult DrawDoubleClickTreeFrame()
    {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), (int)(GuiCond.Always), default);
        NativeImGui.SetNextWindowSize(new Vector2(320f, 240f), (int)(GuiCond.Always));
        using NativeStringLease nativeTestText14 = Utf8Codec.EncodeNullTerminated("Double Click Tree Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText14.pointer, null, 0);
        GuiTreeNodeResult result = EditorUI.current.widgets.TreeNode(
            "double_click_parent",
            static _ => NativeImGui.TextUnformatted("Parent"),
            new GuiTreeNodeOptions()
        );
        if (result.isOpen)
        {
            _ = EditorUI.current.widgets.TreeNode(
                "double_click_child",
                static _ => NativeImGui.TextUnformatted("Child"),
                new GuiTreeNodeOptions { isLeaf = true }
            );
            NativeImGui.TreePop();
        }

        NativeImGui.End();
        NativeImGui.Render();
        return result;
    }

    private static void DrawTreeOverflowFrame(bool assertRanges)
    {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowSize(new Vector2(560f, 320f), (int)(GuiCond.Always));
        using NativeStringLease nativeTestText15 = Utf8Codec.EncodeNullTerminated("Tree Overflow Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText15.pointer, null, 0);
        if (NativeImGui.BeginChild(
            "##short_tree",
            new Vector2(220f, 100f),
            (int)(GuiChildFlags.None),
            (int)(GuiWindowFlags.HorizontalScrollbar)
        )) {
            _ = EditorUI.current.widgets.TreeNode(
                "short",
                static _ => NativeImGui.TextUnformatted("Object"),
                new GuiTreeNodeOptions { isLeaf = true }
            );
            if (assertRanges)
            {
                Assert.Equal(0f, NativeImGui.GetScrollMaxX(), 3);
                Assert.False((NativeImGui.Raw.GetCurrentWindow()->ScrollbarX != 0));
                Assert.True((NativeImGui.Raw.GetCurrentWindow()->Flags & (int)GuiWindowFlags.HorizontalScrollbar) != 0);
            }
        }

        NativeImGui.EndChild();
        if (NativeImGui.BeginChild(
            "##long_tree",
            new Vector2(220f, 100f),
            (int)(GuiChildFlags.None),
            (int)(GuiWindowFlags.HorizontalScrollbar)
        )) {
            _ = EditorUI.current.widgets.TreeNode(
                "long",
                static _ => NativeImGui.TextUnformatted("Object_With_A_Name_That_Is_Intentionally_Wider_Than_The_Tree_Viewport"),
                new GuiTreeNodeOptions { isLeaf = true }
            );
            if (assertRanges)
            {
                Assert.True(NativeImGui.GetScrollMaxX() > 0f);
                Assert.True((NativeImGui.Raw.GetCurrentWindow()->ScrollbarX != 0));
                Assert.True((NativeImGui.Raw.GetCurrentWindow()->Flags & (int)GuiWindowFlags.HorizontalScrollbar) != 0);
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
        NativeImGui.SetNextWindowSize(new Vector2(360f, 220f), (int)(GuiCond.Always));
        using NativeStringLease nativeTestText16 = Utf8Codec.EncodeNullTerminated("Scrolled Tree Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText16.pointer, null, 0);
        NativeImGui.SetNextWindowScroll(new Vector2(requestedScroll, 0f));
        if (NativeImGui.BeginChild(
            "##scrolled_tree",
            new Vector2(220f, 120f),
            (int)(GuiChildFlags.None),
            (int)(GuiWindowFlags.HorizontalScrollbar)
        )) {
            Vector2 contentCursor = default;
            GuiTreeNodeResult row = EditorUI.current.widgets.TreeNode("scrolled", _ =>
            {
                contentCursor = NativeImGui.GetCursorScreenPos();
                NativeImGui.TextUnformatted("Scene_With_A_Name_That_Is_Intentionally_Wider_Than_The_Tree_Viewport");
            }, new GuiTreeNodeOptions { isLeaf = true, showBackground = true, backgroundColor = EditorPalette.title });
            if (assertGeometry)
            {
                ImGuiWindow* window = NativeImGui.Raw.GetCurrentWindow();
                Assert.True(NativeImGui.GetScrollX() > 0f);
                Assert.True((window->ScrollbarX != 0));
                Assert.Equal(row.contentMin.X, contentCursor.X, 3);
                AssertCurrentTreeBackgroundSpansViewport(window, NativeImGui.ColorConvertFloat4ToU32(EditorPalette.title));
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
        NativeImGui.SetNextWindowSize(new Vector2(560f, 260f), (int)(GuiCond.Always));
        using NativeStringLease nativeTestText17 = Utf8Codec.EncodeNullTerminated("Resizable Tree Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText17.pointer, null, 0);
        NativeImGui.SetNextWindowScroll(new Vector2(requestedScroll, 0f));
        if (NativeImGui.BeginChild(
            "##resizable_tree",
            new Vector2(width, 120f),
            (int)(GuiChildFlags.None),
            (int)(GuiWindowFlags.HorizontalScrollbar)
        )) {
            _ = EditorUI.current.widgets.TreeNode(
                "resizable_row",
                static _ => NativeImGui.TextUnformatted("GameObject"),
                new GuiTreeNodeOptions
                {
                    isLeaf = true,
                    drawViewportOverlay = static () =>
                    {
                        ImGuiWindow* window = NativeImGui.Raw.GetCurrentWindow();
                        var overlaySize = new Vector2(NativeImGui.GetFontSize(), NativeImGui.GetTextLineHeight());
                        NativeImGui.SameLine(0f, -1f);
                        Vector2 cursor = NativeImGui.GetCursorScreenPos();
                        NativeImGui.SetCursorScreenPos(new Vector2(MathF.Max(cursor.X, window->WorkRect.Max.X - overlaySize.X), cursor.Y));
                        NativeImGui.Dummy(overlaySize);
                    }
                }
            );
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
        AssertCurrentDrawListContainsColor(guideColor, "The current drag frame did not submit any tree-guide vertices.");
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
                "The child guide overlapped the parent disclosure row instead of retaining its visual gap."
            );
        }
    }

    private static void DrawCurrentTreeBackgroundFrame(
        Vector2 position,
        Vector2 size
    ) {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowPos(position, (int)(GuiCond.Always), default);
        NativeImGui.SetNextWindowSize(size, (int)(GuiCond.Always));
        using NativeStringLease nativeTestText18 = Utf8Codec.EncodeNullTerminated("Current Tree Background Test", allowsNull: false);
        _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText18.pointer, null, 0);
        EditorUI.current.widgets.SetNextTreeNodeOpen(true);
        GuiTreeNodeResult root = EditorUI.current.widgets.TreeNode(
            "current_background_root",
            static _ => NativeImGui.TextUnformatted("TestScene"),
            new GuiTreeNodeOptions { showBackground = true, backgroundColor = EditorPalette.title, suppressHoverHighlight = true }
        );
        if (root.isOpen)
        {
            _ = EditorUI.current.widgets.TreeNode(
                "current_background_child",
                static _ => NativeImGui.TextUnformatted("GameObject"),
                new GuiTreeNodeOptions { isLeaf = true, selected = true }
            );
            NativeImGui.TreePop();
        }

        AssertCurrentDrawListContainsColor(
            NativeImGui.ColorConvertFloat4ToU32(EditorPalette.title),
            "The current frame did not submit the Scene row background."
        );
        AssertCurrentDrawListContainsColor(
            NativeImGui.GetColorU32((int)(GuiCol.Header)),
            "The current frame did not submit the selected row highlight."
        );
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
            {
                return;
            }
        }

        Assert.Fail(failureMessage);
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
            {
                continue;
            }
            minimumX = MathF.Min(minimumX, vertex.Pos.X);
            maximumX = MathF.Max(maximumX, vertex.Pos.X);
        }

        Assert.NotEqual(float.MaxValue, minimumX);
        Assert.Equal(NativeImGui.GetWindowPos().X, minimumX, 3);
        Assert.Equal(window->WorkRect.Max.X, maximumX, 3);
    }
}
