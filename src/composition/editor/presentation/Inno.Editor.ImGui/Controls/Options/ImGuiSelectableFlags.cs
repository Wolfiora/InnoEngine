using System;

using NativeSelectableFlags = Inno.Native.ImGui.ImGuiSelectableFlags;

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
    None = (int)NativeSelectableFlags.None,
    /// <summary>
    /// Clicking this doesn't close parent popup window (overrides ImGuiItemFlags_AutoClosePopups)
    /// </summary>
    NoAutoClosePopups = (int)NativeSelectableFlags.NoAutoClosePopups,
    /// <summary>
    /// Frame will span all columns of its container table (text will still fit in current column)
    /// </summary>
    SpanAllColumns = (int)NativeSelectableFlags.SpanAllColumns,
    /// <summary>
    /// Generate press events on double clicks too
    /// </summary>
    AllowDoubleClick = (int)NativeSelectableFlags.AllowDoubleClick,
    /// <summary>
    /// Cannot be selected, display grayed out text
    /// </summary>
    Disabled = (int)NativeSelectableFlags.Disabled,
    /// <summary>
    /// (WIP) Hit testing to allow subsequent widgets to overlap this one
    /// </summary>
    AllowOverlap = (int)NativeSelectableFlags.AllowOverlap,
    /// <summary>
    /// Make the item be displayed as if it is hovered
    /// </summary>
    Highlight = (int)NativeSelectableFlags.Highlight,
    /// <summary>
    /// Auto-select when moved into, unless Ctrl is held. Automatic when in a BeginMultiSelect() block.
    /// </summary>
    SelectOnNav = (int)NativeSelectableFlags.SelectOnNav
}
