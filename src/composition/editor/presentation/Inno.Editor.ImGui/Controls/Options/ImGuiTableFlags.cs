using System;

using NativeTableFlags = Inno.Native.ImGui.ImGuiTableFlags;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported table flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiTableFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)NativeTableFlags.None,
    /// <summary>
    /// Enable resizing columns.
    /// </summary>
    Resizable = (int)NativeTableFlags.Resizable,
    /// <summary>
    /// Enable reordering columns in header row (need calling TableSetupColumn() + TableHeadersRow() to display headers)
    /// </summary>
    Reorderable = (int)NativeTableFlags.Reorderable,
    /// <summary>
    /// Enable hiding/disabling columns in context menu.
    /// </summary>
    Hideable = (int)NativeTableFlags.Hideable,
    /// <summary>
    /// Enable sorting. Call TableGetSortSpecs() to obtain sort specs. Also see ImGuiTableFlags_SortMulti and ImGuiTableFlags_SortTristate.
    /// </summary>
    Sortable = (int)NativeTableFlags.Sortable,
    /// <summary>
    /// Disable persisting columns order, width, visibility and sort settings in the .ini file.
    /// </summary>
    NoSavedSettings = (int)NativeTableFlags.NoSavedSettings,
    /// <summary>
    /// Right-click on columns body/contents will display table context menu. By default it is available in TableHeadersRow().
    /// </summary>
    ContextMenuInBody = (int)NativeTableFlags.ContextMenuInBody,
    /// <summary>
    /// Set each RowBg color with ImGuiCol_TableRowBg or ImGuiCol_TableRowBgAlt (equivalent of calling TableSetBgColor with ImGuiTableBgFlags_RowBg0 on each row manually)
    /// </summary>
    RowBg = (int)NativeTableFlags.RowBg,
    /// <summary>
    /// Draw horizontal borders between rows.
    /// </summary>
    BordersInnerH = (int)NativeTableFlags.BordersInnerH,
    /// <summary>
    /// Draw horizontal borders at the top and bottom.
    /// </summary>
    BordersOuterH = (int)NativeTableFlags.BordersOuterH,
    /// <summary>
    /// Draw vertical borders between columns.
    /// </summary>
    BordersInnerV = (int)NativeTableFlags.BordersInnerV,
    /// <summary>
    /// Draw vertical borders on the left and right sides.
    /// </summary>
    BordersOuterV = (int)NativeTableFlags.BordersOuterV,
    /// <summary>
    /// Draw horizontal borders.
    /// </summary>
    BordersH = (int)NativeTableFlags.BordersH,
    /// <summary>
    /// Draw vertical borders.
    /// </summary>
    BordersV = (int)NativeTableFlags.BordersV,
    /// <summary>
    /// Draw inner borders.
    /// </summary>
    BordersInner = (int)NativeTableFlags.BordersInner,
    /// <summary>
    /// Draw outer borders.
    /// </summary>
    BordersOuter = (int)NativeTableFlags.BordersOuter,
    /// <summary>
    /// Draw all borders.
    /// </summary>
    Borders = (int)NativeTableFlags.Borders,
    /// <summary>
    /// Disable vertical borders in columns Body (borders will always appear in Headers). -&gt; May move to style
    /// </summary>
    NoBordersInBody = (int)NativeTableFlags.NoBordersInBody,
    /// <summary>
    /// Disable vertical borders in columns Body until hovered for resize (borders will always appear in Headers). -&gt; May move to style
    /// </summary>
    NoBordersInBodyUntilResize = (int)NativeTableFlags.NoBordersInBodyUntilResize,
    /// <summary>
    /// Columns default to _WidthFixed or _WidthAuto (if resizable or not resizable), matching contents width.
    /// </summary>
    SizingFixedFit = (int)NativeTableFlags.SizingFixedFit,
    /// <summary>
    /// Columns default to _WidthFixed or _WidthAuto (if resizable or not resizable), matching the maximum contents width of all columns. Implicitly enable ImGuiTableFlags_NoKeepColumnsVisible.
    /// </summary>
    SizingFixedSame = (int)NativeTableFlags.SizingFixedSame,
    /// <summary>
    /// Columns default to _WidthStretch with default weights proportional to each columns contents widths.
    /// </summary>
    SizingStretchProp = (int)NativeTableFlags.SizingStretchProp,
    /// <summary>
    /// Columns default to _WidthStretch with default weights all equal, unless overridden by TableSetupColumn().
    /// </summary>
    SizingStretchSame = (int)NativeTableFlags.SizingStretchSame,
    /// <summary>
    /// Make outer width auto-fit to columns, overriding outer_size.x value. Only available when ScrollX/ScrollY are disabled and Stretch columns are not used.
    /// </summary>
    NoHostExtendX = (int)NativeTableFlags.NoHostExtendX,
    /// <summary>
    /// Make outer height stop exactly at outer_size.y (prevent auto-extending table past the limit). Only available when ScrollX/ScrollY are disabled. Data below the limit will be clipped and not visible.
    /// </summary>
    NoHostExtendY = (int)NativeTableFlags.NoHostExtendY,
    /// <summary>
    /// Disable keeping column always minimally visible when ScrollX is off and table gets too small. Not recommended if columns are resizable.
    /// </summary>
    NoKeepColumnsVisible = (int)NativeTableFlags.NoKeepColumnsVisible,
    /// <summary>
    /// Disable distributing remainder width to stretched columns (width allocation on a 100-wide table with 3 columns: Without this flag: 33,33,34. With this flag: 33,33,33). With larger number of columns, resizing will appear to be less smooth.
    /// </summary>
    PreciseWidths = (int)NativeTableFlags.PreciseWidths,
    /// <summary>
    /// Disable clipping rectangle for every individual columns (reduce draw command count, items will be able to overflow into other columns). Generally incompatible with TableSetupScrollFreeze().
    /// </summary>
    NoClip = (int)NativeTableFlags.NoClip,
    /// <summary>
    /// Default if BordersOuterV is on. Enable outermost padding. Generally desirable if you have headers.
    /// </summary>
    PadOuterX = (int)NativeTableFlags.PadOuterX,
    /// <summary>
    /// Default if BordersOuterV is off. Disable outermost padding.
    /// </summary>
    NoPadOuterX = (int)NativeTableFlags.NoPadOuterX,
    /// <summary>
    /// Disable inner padding between columns (double inner padding if BordersOuterV is on, single inner padding if BordersOuterV is off).
    /// </summary>
    NoPadInnerX = (int)NativeTableFlags.NoPadInnerX,
    /// <summary>
    /// Enable horizontal scrolling. Require 'outer_size' parameter of BeginTable() to specify the container size. Changes default sizing policy. Because this creates a child window, ScrollY is currently generally recommended when using ScrollX.
    /// </summary>
    ScrollX = (int)NativeTableFlags.ScrollX,
    /// <summary>
    /// Enable vertical scrolling. Require 'outer_size' parameter of BeginTable() to specify the container size.
    /// </summary>
    ScrollY = (int)NativeTableFlags.ScrollY,
    /// <summary>
    /// Hold shift when clicking headers to sort on multiple column. TableGetSortSpecs() may return specs where (SpecsCount &gt; 1).
    /// </summary>
    SortMulti = (int)NativeTableFlags.SortMulti,
    /// <summary>
    /// Allow no sorting, disable default sorting. TableGetSortSpecs() may return specs where (SpecsCount == 0).
    /// </summary>
    SortTristate = (int)NativeTableFlags.SortTristate,
    /// <summary>
    /// Highlight column headers when hovered (may evolve into a fuller highlight)
    /// </summary>
    HighlightHoveredColumn = (int)NativeTableFlags.HighlightHoveredColumn,
    /// <summary>
    /// Selects the sizing mask presentation behavior.
    /// </summary>
    SizingMask = (int)NativeTableFlags.SizingMask
}
