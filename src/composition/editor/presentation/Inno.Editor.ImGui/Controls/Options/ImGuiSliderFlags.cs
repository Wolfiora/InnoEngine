using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported slider flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiSliderFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiSliderFlags.None,
    /// <summary>
    /// Make the widget logarithmic (linear otherwise). Consider using ImGuiSliderFlags_NoRoundToFormat with this if using a format-string with small amount of digits.
    /// </summary>
    Logarithmic = (int)Inno.Native.ImGui.ImGuiSliderFlags.Logarithmic,
    /// <summary>
    /// Disable rounding underlying value to match precision of the display format string (e.g. %.3f values are rounded to those 3 digits).
    /// </summary>
    NoRoundToFormat = (int)Inno.Native.ImGui.ImGuiSliderFlags.NoRoundToFormat,
    /// <summary>
    /// Disable Ctrl+Click or Enter key allowing to input text directly into the widget.
    /// </summary>
    NoInput = (int)Inno.Native.ImGui.ImGuiSliderFlags.NoInput,
    /// <summary>
    /// Enable wrapping around from max to min and from min to max. Only supported by DragXXX() functions for now.
    /// </summary>
    WrapAround = (int)Inno.Native.ImGui.ImGuiSliderFlags.WrapAround,
    /// <summary>
    /// Clamp value to min/max bounds when input manually with Ctrl+Click. By default Ctrl+Click allows going out of bounds.
    /// </summary>
    ClampOnInput = (int)Inno.Native.ImGui.ImGuiSliderFlags.ClampOnInput,
    /// <summary>
    /// Clamp even if min==max==0.0f. Otherwise due to legacy reason DragXXX functions don't clamp with those values. When your clamping limits are dynamic you almost always want to use it.
    /// </summary>
    ClampZeroRange = (int)Inno.Native.ImGui.ImGuiSliderFlags.ClampZeroRange,
    /// <summary>
    /// Disable keyboard modifiers altering tweak speed. Useful if you want to alter tweak speed yourself based on your own logic.
    /// </summary>
    NoSpeedTweaks = (int)Inno.Native.ImGui.ImGuiSliderFlags.NoSpeedTweaks,
    /// <summary>
    /// DragScalarN(), SliderScalarN(): Draw R/G/B/A color markers on each component.
    /// </summary>
    ColorMarkers = (int)Inno.Native.ImGui.ImGuiSliderFlags.ColorMarkers,
    /// <summary>
    /// Selects the always clamp presentation behavior.
    /// </summary>
    AlwaysClamp = (int)Inno.Native.ImGui.ImGuiSliderFlags.AlwaysClamp,
    /// <summary>
    /// Selects the invalid mask presentation behavior.
    /// </summary>
    InvalidMask = (int)Inno.Native.ImGui.ImGuiSliderFlags.InvalidMask,
}
