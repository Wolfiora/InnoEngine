using System;

using NativeTreeNodeFlags = Inno.Native.ImGui.ImGuiTreeNodeFlags;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported tree node flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiTreeNodeFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)NativeTreeNodeFlags.None,
    /// <summary>
    /// Draw as selected
    /// </summary>
    Selected = (int)NativeTreeNodeFlags.Selected,
    /// <summary>
    /// Draw frame with background (e.g. for CollapsingHeader)
    /// </summary>
    Framed = (int)NativeTreeNodeFlags.Framed,
    /// <summary>
    /// Hit testing to allow subsequent widgets to overlap this one
    /// </summary>
    AllowOverlap = (int)NativeTreeNodeFlags.AllowOverlap,
    /// <summary>
    /// Don't do a TreePush() when open (e.g. for CollapsingHeader) = no extra indent nor pushing on ID stack
    /// </summary>
    NoTreePushOnOpen = (int)NativeTreeNodeFlags.NoTreePushOnOpen,
    /// <summary>
    /// Don't automatically and temporarily open node when Logging is active (by default logging will automatically open tree nodes)
    /// </summary>
    NoAutoOpenOnLog = (int)NativeTreeNodeFlags.NoAutoOpenOnLog,
    /// <summary>
    /// Default node to be open
    /// </summary>
    DefaultOpen = (int)NativeTreeNodeFlags.DefaultOpen,
    /// <summary>
    /// Open on double-click instead of simple click (default for multi-select unless any _OpenOnXXX behavior is set explicitly). Both behaviors may be combined.
    /// </summary>
    OpenOnDoubleClick = (int)NativeTreeNodeFlags.OpenOnDoubleClick,
    /// <summary>
    /// Open when clicking on the arrow part (default for multi-select unless any _OpenOnXXX behavior is set explicitly). Both behaviors may be combined.
    /// </summary>
    OpenOnArrow = (int)NativeTreeNodeFlags.OpenOnArrow,
    /// <summary>
    /// No collapsing, no arrow (use as a convenience for leaf nodes).
    /// </summary>
    Leaf = (int)NativeTreeNodeFlags.Leaf,
    /// <summary>
    /// Display a bullet instead of arrow. IMPORTANT: node can still be marked open/close if you don't set the _Leaf flag!
    /// </summary>
    Bullet = (int)NativeTreeNodeFlags.Bullet,
    /// <summary>
    /// Use FramePadding (even for an unframed text node) to vertically align text baseline to regular widget height. Equivalent to calling AlignTextToFramePadding() before the node.
    /// </summary>
    FramePadding = (int)NativeTreeNodeFlags.FramePadding,
    /// <summary>
    /// Extend hit box to the right-most edge, even if not framed. This is not the default in order to allow adding other items on the same line without using AllowOverlap mode.
    /// </summary>
    SpanAvailWidth = (int)NativeTreeNodeFlags.SpanAvailWidth,
    /// <summary>
    /// Extend hit box to the left-most and right-most edges (cover the indent area).
    /// </summary>
    SpanFullWidth = (int)NativeTreeNodeFlags.SpanFullWidth,
    /// <summary>
    /// Narrow hit box + narrow hovering highlight, will only cover the label text.
    /// </summary>
    SpanLabelWidth = (int)NativeTreeNodeFlags.SpanLabelWidth,
    /// <summary>
    /// Frame will span all columns of its container table (label will still fit in current column)
    /// </summary>
    SpanAllColumns = (int)NativeTreeNodeFlags.SpanAllColumns,
    /// <summary>
    /// Label will span all columns of its container table
    /// </summary>
    LabelSpanAllColumns = (int)NativeTreeNodeFlags.LabelSpanAllColumns,
    /// <summary>
    /// Nav: left arrow moves back to parent. This is processed in TreePop() when there's an unfulfilled Left nav request remaining.
    /// </summary>
    NavLeftJumpsToParent = (int)NativeTreeNodeFlags.NavLeftJumpsToParent,
    /// <summary>
    /// Selects the collapsing header presentation behavior.
    /// </summary>
    CollapsingHeader = (int)NativeTreeNodeFlags.CollapsingHeader,
    /// <summary>
    /// No lines drawn
    /// </summary>
    DrawLinesNone = (int)NativeTreeNodeFlags.DrawLinesNone,
    /// <summary>
    /// Horizontal lines to child nodes. Vertical line drawn down to TreePop() position: cover full contents. Faster (for large trees).
    /// </summary>
    DrawLinesFull = (int)NativeTreeNodeFlags.DrawLinesFull,
    /// <summary>
    /// Horizontal lines to child nodes. Vertical line drawn down to bottom-most child node. Slower (for large trees).
    /// </summary>
    DrawLinesToNodes = (int)NativeTreeNodeFlags.DrawLinesToNodes
}
