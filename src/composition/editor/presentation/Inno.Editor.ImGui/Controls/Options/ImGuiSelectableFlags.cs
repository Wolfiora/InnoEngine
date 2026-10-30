using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported selectable flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiSelectableFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiSelectableFlags.None,
    /// <summary>
    /// Clicking this doesn't close parent popup window (overrides ImGuiItemFlags_AutoClosePopups)
    /// </summary>
    NoAutoClosePopups = (int)Inno.Native.ImGui.ImGuiSelectableFlags.NoAutoClosePopups,
    /// <summary>
    /// Frame will span all columns of its container table (text will still fit in current column)
    /// </summary>
    SpanAllColumns = (int)Inno.Native.ImGui.ImGuiSelectableFlags.SpanAllColumns,
    /// <summary>
    /// Generate press events on double clicks too
    /// </summary>
    AllowDoubleClick = (int)Inno.Native.ImGui.ImGuiSelectableFlags.AllowDoubleClick,
    /// <summary>
    /// Cannot be selected, display grayed out text
    /// </summary>
    Disabled = (int)Inno.Native.ImGui.ImGuiSelectableFlags.Disabled,
    /// <summary>
    /// (WIP) Hit testing to allow subsequent widgets to overlap this one
    /// </summary>
    AllowOverlap = (int)Inno.Native.ImGui.ImGuiSelectableFlags.AllowOverlap,
    /// <summary>
    /// Make the item be displayed as if it is hovered
    /// </summary>
    Highlight = (int)Inno.Native.ImGui.ImGuiSelectableFlags.Highlight,
    /// <summary>
    /// Auto-select when moved into, unless Ctrl is held. Automatic when in a BeginMultiSelect() block.
    /// </summary>
    SelectOnNav = (int)Inno.Native.ImGui.ImGuiSelectableFlags.SelectOnNav,
}
