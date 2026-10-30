using System;

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
    None = (int)Inno.Native.ImGui.ImGuiComboFlags.None,
    /// <summary>
    /// Align the popup toward the left by default
    /// </summary>
    PopupAlignLeft = (int)Inno.Native.ImGui.ImGuiComboFlags.PopupAlignLeft,
    /// <summary>
    /// Max ~4 items visible. Tip: If you want your combo popup to be a specific size you can use SetNextWindowSizeConstraints() prior to calling BeginCombo()
    /// </summary>
    HeightSmall = (int)Inno.Native.ImGui.ImGuiComboFlags.HeightSmall,
    /// <summary>
    /// Max ~8 items visible (default)
    /// </summary>
    HeightRegular = (int)Inno.Native.ImGui.ImGuiComboFlags.HeightRegular,
    /// <summary>
    /// Max ~20 items visible
    /// </summary>
    HeightLarge = (int)Inno.Native.ImGui.ImGuiComboFlags.HeightLarge,
    /// <summary>
    /// As many fitting items as possible
    /// </summary>
    HeightLargest = (int)Inno.Native.ImGui.ImGuiComboFlags.HeightLargest,
    /// <summary>
    /// Display on the preview box without the square arrow button
    /// </summary>
    NoArrowButton = (int)Inno.Native.ImGui.ImGuiComboFlags.NoArrowButton,
    /// <summary>
    /// Display only a square arrow button
    /// </summary>
    NoPreview = (int)Inno.Native.ImGui.ImGuiComboFlags.NoPreview,
    /// <summary>
    /// Width dynamically calculated from preview contents
    /// </summary>
    WidthFitPreview = (int)Inno.Native.ImGui.ImGuiComboFlags.WidthFitPreview,
    /// <summary>
    /// Selects the height mask presentation behavior.
    /// </summary>
    HeightMask = (int)Inno.Native.ImGui.ImGuiComboFlags.HeightMask,
}
