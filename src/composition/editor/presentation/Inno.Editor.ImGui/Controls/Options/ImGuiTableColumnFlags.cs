using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported table column flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiTableColumnFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.None,
    /// <summary>
    /// Overriding/master disable flag: hide column, won't show in context menu (unlike calling TableSetColumnEnabled() which manipulates the user accessible state)
    /// </summary>
    Disabled = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.Disabled,
    /// <summary>
    /// Default as a hidden/disabled column.
    /// </summary>
    DefaultHide = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.DefaultHide,
    /// <summary>
    /// Default as a sorting column.
    /// </summary>
    DefaultSort = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.DefaultSort,
    /// <summary>
    /// Column will stretch. Preferable with horizontal scrolling disabled (default if table sizing policy is _SizingStretchSame or _SizingStretchProp).
    /// </summary>
    WidthStretch = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.WidthStretch,
    /// <summary>
    /// Column will not stretch. Preferable with horizontal scrolling enabled (default if table sizing policy is _SizingFixedFit and table is resizable).
    /// </summary>
    WidthFixed = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.WidthFixed,
    /// <summary>
    /// Disable manual resizing.
    /// </summary>
    NoResize = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.NoResize,
    /// <summary>
    /// Disable manual reordering this column, this will also prevent other columns from crossing over this column.
    /// </summary>
    NoReorder = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.NoReorder,
    /// <summary>
    /// Disable ability to hide/disable this column.
    /// </summary>
    NoHide = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.NoHide,
    /// <summary>
    /// Disable clipping for this column (all NoClip columns will render in a same draw command).
    /// </summary>
    NoClip = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.NoClip,
    /// <summary>
    /// Disable ability to sort on this field (even if ImGuiTableFlags_Sortable is set on the table).
    /// </summary>
    NoSort = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.NoSort,
    /// <summary>
    /// Disable ability to sort in the ascending direction.
    /// </summary>
    NoSortAscending = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.NoSortAscending,
    /// <summary>
    /// Disable ability to sort in the descending direction.
    /// </summary>
    NoSortDescending = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.NoSortDescending,
    /// <summary>
    /// TableHeadersRow() will submit an empty label for this column. Convenient for some small columns. Name will still appear in context menu or in angled headers. You may append into this cell by calling TableSetColumnIndex() right after the TableHeadersRow() call.
    /// </summary>
    NoHeaderLabel = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.NoHeaderLabel,
    /// <summary>
    /// Disable header text width contribution to automatic column width.
    /// </summary>
    NoHeaderWidth = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.NoHeaderWidth,
    /// <summary>
    /// Make the initial sort direction Ascending when first sorting on this column (default).
    /// </summary>
    PreferSortAscending = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.PreferSortAscending,
    /// <summary>
    /// Make the initial sort direction Descending when first sorting on this column.
    /// </summary>
    PreferSortDescending = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.PreferSortDescending,
    /// <summary>
    /// Use current Indent value when entering cell (default for column 0).
    /// </summary>
    IndentEnable = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.IndentEnable,
    /// <summary>
    /// Ignore current Indent value when entering cell (default for columns &gt; 0). Indentation changes _within_ the cell will still be honored.
    /// </summary>
    IndentDisable = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.IndentDisable,
    /// <summary>
    /// TableHeadersRow() will submit an angled header row for this column. Note this will add an extra row.
    /// </summary>
    AngledHeader = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.AngledHeader,
    /// <summary>
    /// Status: is enabled == not hidden by user/api (referred to as "Hide" in _DefaultHide and _NoHide) flags.
    /// </summary>
    IsEnabled = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.IsEnabled,
    /// <summary>
    /// Status: is visible == is enabled AND not clipped by scrolling.
    /// </summary>
    IsVisible = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.IsVisible,
    /// <summary>
    /// Status: is currently part of the sort specs
    /// </summary>
    IsSorted = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.IsSorted,
    /// <summary>
    /// Status: is hovered by mouse
    /// </summary>
    IsHovered = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.IsHovered,
    /// <summary>
    /// Selects the width mask presentation behavior.
    /// </summary>
    WidthMask = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.WidthMask,
    /// <summary>
    /// Selects the indent mask presentation behavior.
    /// </summary>
    IndentMask = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.IndentMask,
    /// <summary>
    /// Selects the status mask presentation behavior.
    /// </summary>
    StatusMask = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.StatusMask,
    /// <summary>
    /// Selects the no direct resize presentation behavior.
    /// </summary>
    NoDirectResize = (int)Inno.Native.ImGui.ImGuiTableColumnFlags.NoDirectResize,
}
