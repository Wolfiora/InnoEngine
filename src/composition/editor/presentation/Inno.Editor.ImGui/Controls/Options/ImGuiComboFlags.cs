using System;

using NativeComboFlags = Inno.Native.ImGui.ImGuiComboFlags;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported combo flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiComboFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)NativeComboFlags.None,
    /// <summary>
    /// Align the popup toward the left by default
    /// </summary>
    PopupAlignLeft = (int)NativeComboFlags.PopupAlignLeft,
    /// <summary>
    /// Max ~4 items visible. Tip: If you want your combo popup to be a specific size you can use SetNextWindowSizeConstraints() prior to calling BeginCombo()
    /// </summary>
    HeightSmall = (int)NativeComboFlags.HeightSmall,
    /// <summary>
    /// Max ~8 items visible (default)
    /// </summary>
    HeightRegular = (int)NativeComboFlags.HeightRegular,
    /// <summary>
    /// Max ~20 items visible
    /// </summary>
    HeightLarge = (int)NativeComboFlags.HeightLarge,
    /// <summary>
    /// As many fitting items as possible
    /// </summary>
    HeightLargest = (int)NativeComboFlags.HeightLargest,
    /// <summary>
    /// Display on the preview box without the square arrow button
    /// </summary>
    NoArrowButton = (int)NativeComboFlags.NoArrowButton,
    /// <summary>
    /// Display only a square arrow button
    /// </summary>
    NoPreview = (int)NativeComboFlags.NoPreview,
    /// <summary>
    /// Width dynamically calculated from preview contents
    /// </summary>
    WidthFitPreview = (int)NativeComboFlags.WidthFitPreview,
    /// <summary>
    /// Selects the height mask presentation behavior.
    /// </summary>
    HeightMask = (int)NativeComboFlags.HeightMask
}
