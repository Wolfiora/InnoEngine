using System;
using System.Collections.Generic;
using System.Numerics;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;
/// <summary>
/// Provides reusable editor controls and rendering helpers built on the native ImGui API.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    private readonly Dictionary<(string Owner, uint Window), TreeWidgetWindowState> m_treeStatesByWindow = [];
    private bool m_hasNextTreeNodeOpen;
    private bool m_nextTreeNodeOpen;
    private List<TreeWidgetNodeState> m_treeNodeStack => GetTreeWindowState().treeNodeStack;
    private List<string> m_lastNodeIdsByDepth => GetTreeWindowState().lastNodeIdsByDepth;
    private List<TreeWidgetLineSegment> m_lineSegments => GetTreeWindowState().lineSegments;
    private Dictionary<string, bool> m_hasNextSiblingById => GetTreeWindowState().hasNextSiblingById;
    private Dictionary<string, bool> m_openStatesById => GetTreeWindowState().openStatesById;
    private int m_lastFrame { get => GetTreeWindowState().lastFrame; set => GetTreeWindowState().lastFrame = value; }

    /// <summary>
    /// Draws a full-width interactive tree row whose non-leaf content toggles expansion when
    /// double-clicked, while preserving single-click disclosure-arrow behavior.
    /// </summary>
    /// <param name="id">
    /// Stable row identifier.
    /// </param>
    /// <param name="onDraw">
    /// Content drawing callback that receives the native row geometry established by ImGui.
    /// </param>
    /// <param name="options">
    /// Tree row options.
    /// </param>
    /// <returns>
    /// Interaction and row geometry for the submitted item.
    /// </returns>
    public TreeNodeResult TreeNode(
        string id,
        Action<TreeNodeDrawContext> onDraw,
        in TreeNodeOptions options
    ) {
        m_session.EnsureDrawing();
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(onDraw);
        bool isLeaf = options.isLeaf;
        BeginTreeFrameIfNeeded();
        ImDrawList* drawList = NativeImGui.Raw.GetWindowDrawList();
        NativeImGui.Raw.ChannelsSplit(drawList, 2);
        NativeImGui.Raw.ChannelsSetCurrent(drawList, 1);
        try
        {
            return DrawTreeNode(id, onDraw, options, isLeaf, drawList);
        }
        finally
        {
            NativeImGui.Raw.ChannelsMerge(drawList);
        }
    }

    private TreeNodeResult DrawTreeNode(
        string id,
        Action<TreeNodeDrawContext> onDraw,
        in TreeNodeOptions options,
        bool isLeaf,
        ImDrawList* drawList
    ) {
        bool open = !isLeaf && m_openStatesById.TryGetValue(id, out bool storedOpen) && storedOpen;
        if (!isLeaf && m_hasNextTreeNodeOpen)
        {
            open = m_nextTreeNodeOpen;
            m_openStatesById[id] = open;
        }

        m_hasNextTreeNodeOpen = false;
        NativeImGui.SetNextItemOpen(open, (int)(ImGuiCond.Always));
        Vector2 nodeCursor = NativeImGui.GetCursorScreenPos();
        // Each disclosure, content row and overlay owns its own hit region.
        ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None;
        if (isLeaf)
            flags |= ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen;
        else
            flags |= ImGuiTreeNodeFlags.OpenOnArrow;
        int depth = Math.Max(0, NativeImGui.Raw.GetCurrentWindow()->DC.TreeDepth);
        PruneTreeNodeStack(depth);
        bool hasNextSibling = TrackSiblingState(id, depth);
        if (!options.hideGuideLines)
            DrawTreeGuideLines(nodeCursor, hasNextSibling, !isLeaf);
        PushTransparentTreeNodeHeaderColors();
        bool isOpen;
        try
        {
            isOpen = NativeImGui.TreeNodeEx($"##{id}", (int)(flags));
        }
        finally
        {
            NativeImGui.PopStyleColor(3);
        }

        float nativeRowMaxY = NativeImGui.GetItemRectMax().Y;
        try
        {
            if (!isLeaf && NativeImGui.IsItemToggledOpen())
                m_openStatesById[id] = isOpen;
            TreeWidgetHighlightRect contentRect = DrawTreeNodeContentContainer(id, nodeCursor, nativeRowMaxY, onDraw, options.drawViewportOverlay, out Vector2 interactionMin);
            bool hovered = NativeImGui.IsItemHovered((int)(ImGuiHoveredFlags.AllowWhenOverlappedByItem));
            bool clicked = hovered && NativeImGui.IsItemClicked((int)(ImGuiMouseButton.Left));
            bool doubleClicked = hovered && NativeImGui.IsMouseDoubleClicked((int)(ImGuiMouseButton.Left));
            if (!isLeaf && doubleClicked)
                m_openStatesById[id] = !isOpen;
            bool showHoverHighlight = hovered && !options.suppressHoverHighlight && NativeImGui.Raw.IsDragDropActive() == 0;
            if (options.selected || showHoverHighlight)
                DrawTreeRowBackground(drawList, contentRect, NativeImGui.GetColorU32((int)(options.selected ? ImGuiCol.Header : ImGuiCol.HeaderHovered)));
            else if (options.showBackground)
                DrawTreeRowBackground(drawList, contentRect, NativeImGui.ColorConvertFloat4ToU32(options.backgroundColor));
            if (isOpen && !isLeaf)
            {
                m_treeNodeStack.Add(new TreeWidgetNodeState { id = id, cursor = nodeCursor, rowMaxY = contentRect.max.Y, hasNextSibling = hasNextSibling });
            }

            return new TreeNodeResult(isOpen && !isLeaf, clicked, doubleClicked, hovered, contentRect.min, contentRect.max, interactionMin);
        }
        catch
        {
            if (isOpen && !isLeaf)
                NativeImGui.TreePop();
            throw;
        }
    }

    /// <summary>
    /// Overrides the retained expansion state of the next submitted non-leaf tree row.
    /// </summary>
    /// <param name="open">
    /// Whether the next row should be expanded.
    /// </param>
    public void SetNextTreeNodeOpen(bool open)
    {
        m_session.EnsureDrawing();
        m_hasNextTreeNodeOpen = true;
        m_nextTreeNodeOpen = open;
    }

    private TreeWidgetWindowState GetTreeWindowState()
    {
        var windowKey = (m_session.owner, NativeImGui.Raw.GetCurrentWindow()->ID);
        if (!m_treeStatesByWindow.TryGetValue(windowKey, out TreeWidgetWindowState? state))
        {
            state = new TreeWidgetWindowState();
            m_treeStatesByWindow[windowKey] = state;
        }

        return state;
    }

    private TreeWidgetHighlightRect DrawTreeNodeContentContainer(
        string id,
        Vector2 nodeCursor,
        float nativeRowMaxY,
        Action<TreeNodeDrawContext> onDraw,
        Action? drawViewportOverlay,
        out Vector2 interactionMin
    ) {
        ImGuiWindow* window = NativeImGui.Raw.GetCurrentWindow();
        Vector2 windowPos = NativeImGui.GetWindowPos();
        float contentX = nodeCursor.X + NativeImGui.GetTreeNodeToLabelSpacing();
        float contentRightX = window->WorkRect.Max.X;
        float contentBottomY = window->InnerRect.Max.Y;
        float contentOffsetX = contentX - windowPos.X + window->Scroll.X - window->DC.GroupOffset.X - window->DC.ColumnsOffset.X;
        NativeImGui.SameLine(contentOffsetX, 0f);
        NativeImGui.BeginGroup();
        NativeImGui.PushClipRect(new Vector2(contentX, nodeCursor.Y), new Vector2(contentRightX, contentBottomY), true);
        try
        {
            onDraw(new TreeNodeDrawContext(MathF.Max(1f, nativeRowMaxY - nodeCursor.Y)));
        }
        finally
        {
            NativeImGui.PopClipRect();
            NativeImGui.EndGroup();
        }

        Vector2 contentMin = NativeImGui.GetItemRectMin();
        Vector2 contentMax = NativeImGui.GetItemRectMax();
        TreeWidgetHighlightRect rect = new()
        {
            min = new Vector2(windowPos.X, MathF.Min(nodeCursor.Y, contentMin.Y)),
            max = new Vector2(contentRightX, MathF.Max(nativeRowMaxY, contentMax.Y))
        };
        Vector2 hitMin = new(contentX, rect.min.Y);
        Vector2 hitMax = rect.max;
        interactionMin = hitMin;
        DrawTreeNodeViewportOverlay(window, drawViewportOverlay);
        float contentBoundaryX = window->DC.CursorMaxPos.X;
        float idealBoundaryX = window->DC.IdealMaxPos.X;
        NativeImGui.SetCursorScreenPos(hitMin);
        _ = NativeImGui.InvisibleButton($"##tree_content_hit_{id}", hitMax - hitMin);
        window->DC.CursorMaxPos.X = contentBoundaryX;
        window->DC.IdealMaxPos.X = idealBoundaryX;
        return rect;
    }

    private void DrawTreeNodeViewportOverlay(
        ImGuiWindow* window,
        Action? drawViewportOverlay
    ) {
        if (drawViewportOverlay is null)
            return;
        float contentBoundaryX = window->DC.CursorMaxPos.X;
        float idealBoundaryX = window->DC.IdealMaxPos.X;
        try
        {
            drawViewportOverlay();
        }
        finally
        {
            window->DC.CursorMaxPos.X = contentBoundaryX;
            window->DC.IdealMaxPos.X = idealBoundaryX;
        }
    }

    private void PushTransparentTreeNodeHeaderColors()
    {
        NativeImGui.PushStyleColor((int)(ImGuiCol.Header), EditorPalette.transparent);
        NativeImGui.PushStyleColor((int)(ImGuiCol.HeaderHovered), EditorPalette.transparent);
        NativeImGui.PushStyleColor((int)(ImGuiCol.HeaderActive), EditorPalette.transparent);
    }

    private void BeginTreeFrameIfNeeded()
    {
        int frame = NativeImGui.GetFrameCount();
        if (frame == m_lastFrame)
            return;
        TreeWidgetWindowState state = GetTreeWindowState();
        state.treeNodeStack.Clear();
        state.lastNodeIdsByDepth.Clear();
        state.lineSegments.Clear();
        m_lastFrame = frame;
    }

    private void PruneTreeNodeStack(int nativeDepth)
    {
        while (m_treeNodeStack.Count > nativeDepth)
            m_treeNodeStack.RemoveAt(m_treeNodeStack.Count - 1);
    }

    private bool TrackSiblingState(
        string id,
        int depth
    ) {
        bool hasNextSibling = m_hasNextSiblingById.TryGetValue(id, out bool cachedHasNextSibling) && cachedHasNextSibling;
        while (m_lastNodeIdsByDepth.Count <= depth)
            m_lastNodeIdsByDepth.Add(string.Empty);
        string previousId = m_lastNodeIdsByDepth[depth];
        if (!string.IsNullOrEmpty(previousId) && !string.Equals(previousId, id, StringComparison.Ordinal))
            m_hasNextSiblingById[previousId] = true;
        m_hasNextSiblingById[id] = false;
        m_lastNodeIdsByDepth[depth] = id;
        for (int i = depth + 1; i < m_lastNodeIdsByDepth.Count; i++)
            m_lastNodeIdsByDepth[i] = string.Empty;
        return hasNextSibling;
    }

    private void DrawTreeGuideLines(
        Vector2 nodeCursor,
        bool hasNextSibling,
        bool hasDisclosureArrow
    ) {
        if (m_treeNodeStack.Count == 0)
            return;
        ImGuiStyle* style = NativeImGui.Raw.GetStyle();
        float textLineHeight = NativeImGui.GetTextLineHeight();
        float lineOverlap = this.style.treeGuideLineOverlap;
        float rowMaxY = nodeCursor.Y + textLineHeight + style->ItemSpacing.Y + lineOverlap;
        float rowCenterY = nodeCursor.Y + textLineHeight * 0.5f;
        float nodeToLabel = NativeImGui.GetTreeNodeToLabelSpacing();
        float guideOffset = nodeToLabel * 0.5f - this.style.treeGuideLeftOffset;
        float labelStartX = nodeCursor.X + nodeToLabel;
        float fileConnectorPadding = MathF.Max(1f, style->ItemInnerSpacing.X);
        float folderConnectorPadding = fileConnectorPadding * 2f + this.style.treeFolderConnectorPadding;
        float targetX = hasDisclosureArrow ? nodeCursor.X + guideOffset - folderConnectorPadding : labelStartX - fileConnectorPadding;
        for (int i = 0; i < m_treeNodeStack.Count - 1; i++)
        {
            TreeWidgetNodeState ancestorState = m_treeNodeStack[i];
            TreeWidgetNodeState pathChildState = m_treeNodeStack[i + 1];
            if (pathChildState.hasNextSibling)
            {
                float ancestorX = ancestorState.cursor.X + guideOffset;
                float ancestorStartY = MathF.Max(nodeCursor.Y, ancestorState.rowMaxY);
                AddTreeLine(new Vector2(ancestorX, ancestorStartY), new Vector2(ancestorX, rowMaxY));
            }
        }

        TreeWidgetNodeState parentState = m_treeNodeStack[^1];
        float branchX = parentState.cursor.X + guideOffset;
        float branchStartY = MathF.Max(parentState.rowMaxY, nodeCursor.Y);
        AddTreeLine(new Vector2(branchX, branchStartY), new Vector2(branchX, hasNextSibling ? rowMaxY : rowCenterY));
        if (targetX > branchX)
            AddTreeLine(new Vector2(branchX, rowCenterY), new Vector2(targetX, rowCenterY));
    }

    private void AddTreeLine(
        Vector2 from,
        Vector2 to
    ) {
        TreeWidgetLineSegment line = new()
        {
            from = SnapTreeLinePoint(from),
            to = SnapTreeLinePoint(to)
        };
        m_lineSegments.Add(line);
        ImGui.DrawNativeLine(NativeImGui.Raw.GetWindowDrawList(), line.from, line.to, NativeImGui.ColorConvertFloat4ToU32(EditorPalette.treeGuide), this.style.borderSize);
    }

    private Vector2 SnapTreeLinePoint(Vector2 point)
    {
        return new Vector2(MathF.Floor(point.X) + 0.5f, MathF.Floor(point.Y) + 0.5f);
    }

    private void DrawTreeRowBackground(
        ImDrawList* drawList,
        TreeWidgetHighlightRect rect,
        uint color
    ) {
        NativeImGui.Raw.ChannelsSetCurrent(drawList, 0);
        ImGui.DrawNativeFilledRectangle(drawList, rect.min, rect.max, color, 0, 0);
        NativeImGui.Raw.ChannelsSetCurrent(drawList, 1);
    }
}

/// <summary>
/// Provides the native geometry of a tree row while its custom content is being drawn.
/// </summary>
public readonly struct TreeNodeDrawContext
{
    internal TreeNodeDrawContext(float rowHeight)
    {
        this.rowHeight = rowHeight;
    }

    /// <summary>
    /// Gets the actual native row height established for the current tree node.
    /// </summary>
    public float rowHeight { get; }
}

/// <summary>
/// Configures an interactive tree row.
/// </summary>
public readonly struct TreeNodeOptions
{
    /// <summary>
    /// Gets whether the row is selected.
    /// </summary>
    public bool selected { get; init; }
    /// <summary>
    /// Gets whether the row has no expandable children.
    /// </summary>
    public bool isLeaf { get; init; }
    /// <summary>
    /// Gets whether a custom background is drawn behind an unselected row.
    /// </summary>
    public bool showBackground { get; init; }
    /// <summary>
    /// Gets the custom background color used when <see cref="showBackground"/> is enabled.
    /// </summary>
    public Vector4 backgroundColor { get; init; }
    /// <summary>
    /// Gets whether the row keeps its configured background while hovered.
    /// </summary>
    public bool suppressHoverHighlight { get; init; }
    /// <summary>
    /// Gets whether ancestor and branch guide lines are omitted for this row.
    /// </summary>
    public bool hideGuideLines { get; init; }
    /// <summary>
    /// Gets the callback that draws controls fixed to the current viewport without extending the
    /// tree's horizontal content boundary.
    /// </summary>
    public Action? drawViewportOverlay { get; init; }
}

/// <summary>
/// Describes interaction state produced by a tree row.
/// </summary>
public readonly struct TreeNodeResult
{
    internal TreeNodeResult(
        bool isOpen,
        bool isClicked,
        bool isDoubleClicked,
        bool isHovered,
        Vector2 min,
        Vector2 max,
        Vector2 contentMin
    ) {
        this.isOpen = isOpen;
        this.isClicked = isClicked;
        this.isDoubleClicked = isDoubleClicked;
        this.isHovered = isHovered;
        this.min = min;
        this.max = max;
        this.contentMin = contentMin;
    }

    /// <summary>
    /// Gets whether child content should be rendered.
    /// </summary>
    public bool isOpen { get; }
    /// <summary>
    /// Gets whether the content row was clicked.
    /// </summary>
    public bool isClicked { get; }
    /// <summary>
    /// Gets whether the content row was double-clicked. Double-clicking a non-leaf content row
    /// also toggles its retained expansion state for the next frame.
    /// </summary>
    public bool isDoubleClicked { get; }
    /// <summary>
    /// Gets whether the full row is hovered.
    /// </summary>
    public bool isHovered { get; }
    /// <summary>
    /// Gets the row minimum screen coordinate.
    /// </summary>
    public Vector2 min { get; }
    /// <summary>
    /// Gets the row maximum screen coordinate.
    /// </summary>
    public Vector2 max { get; }
    /// <summary>
    /// Gets the minimum screen coordinate of the row's interactive content, excluding tree indentation.
    /// </summary>
    public Vector2 contentMin { get; }
}

internal sealed class TreeWidgetWindowState
{
    internal readonly List<TreeWidgetNodeState> treeNodeStack = [];
    internal readonly List<string> lastNodeIdsByDepth = [];
    internal readonly List<TreeWidgetLineSegment> lineSegments = [];
    internal readonly Dictionary<string, bool> hasNextSiblingById = new(StringComparer.Ordinal);
    internal readonly Dictionary<string, bool> openStatesById = new(StringComparer.Ordinal);
    internal int lastFrame = -1;
}

internal struct TreeWidgetNodeState
{
    internal string id;
    internal Vector2 cursor;
    internal float rowMaxY;
    internal bool hasNextSibling;
}

internal struct TreeWidgetLineSegment
{
    internal Vector2 from;
    internal Vector2 to;
}

internal struct TreeWidgetHighlightRect
{
    internal Vector2 min;
    internal Vector2 max;
}
