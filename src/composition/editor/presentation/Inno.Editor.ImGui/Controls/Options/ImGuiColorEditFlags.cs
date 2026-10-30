using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported color edit flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiColorEditFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiColorEditFlags.None,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: ignore Alpha component (will only read 3 components from the input pointer).
    /// </summary>
    NoAlpha = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoAlpha,
    /// <summary>
    /// // ColorEdit: disable picker when clicking on color square.
    /// </summary>
    NoPicker = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoPicker,
    /// <summary>
    /// // ColorEdit: disable toggling options menu when right-clicking on inputs/small preview.
    /// </summary>
    NoOptions = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoOptions,
    /// <summary>
    /// // ColorEdit, ColorPicker: disable color square preview next to the inputs. (e.g. to show only the inputs)
    /// </summary>
    NoSmallPreview = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoSmallPreview,
    /// <summary>
    /// // ColorEdit, ColorPicker: disable inputs sliders/text widgets (e.g. to show only the small preview color square).
    /// </summary>
    NoInputs = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoInputs,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: disable tooltip when hovering the preview.
    /// </summary>
    NoTooltip = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoTooltip,
    /// <summary>
    /// // ColorEdit, ColorPicker: disable display of inline text label (the label is still forwarded to the tooltip and picker).
    /// </summary>
    NoLabel = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoLabel,
    /// <summary>
    /// // ColorPicker: disable bigger color preview on right side of the picker, use small color square preview instead.
    /// </summary>
    NoSidePreview = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoSidePreview,
    /// <summary>
    /// // ColorEdit: disable drag and drop target/source. ColorButton: disable drag and drop source.
    /// </summary>
    NoDragDrop = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoDragDrop,
    /// <summary>
    /// // ColorButton: disable border (which is enforced by default)
    /// </summary>
    NoBorder = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoBorder,
    /// <summary>
    /// // ColorEdit: disable rendering R/G/B/A color marker. May also be disabled globally by setting style.ColorMarkerSize = 0.
    /// </summary>
    NoColorMarkers = (int)Inno.Native.ImGui.ImGuiColorEditFlags.NoColorMarkers,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: disable alpha in the preview,. Contrary to _NoAlpha it may still be edited when calling ColorEdit4()/ColorPicker4(). For ColorButton() this does the same as _NoAlpha.
    /// </summary>
    AlphaOpaque = (int)Inno.Native.ImGui.ImGuiColorEditFlags.AlphaOpaque,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: disable rendering a checkerboard background behind transparent color.
    /// </summary>
    AlphaNoBg = (int)Inno.Native.ImGui.ImGuiColorEditFlags.AlphaNoBg,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: display half opaque / half transparent preview.
    /// </summary>
    AlphaPreviewHalf = (int)Inno.Native.ImGui.ImGuiColorEditFlags.AlphaPreviewHalf,
    /// <summary>
    /// // ColorEdit, ColorPicker: show vertical alpha bar/gradient in picker.
    /// </summary>
    AlphaBar = (int)Inno.Native.ImGui.ImGuiColorEditFlags.AlphaBar,
    /// <summary>
    /// Selects the hdr presentation behavior.
    /// </summary>
    Hdr = (int)Inno.Native.ImGui.ImGuiColorEditFlags.Hdr,
    /// <summary>
    /// Selects the display rgb presentation behavior.
    /// </summary>
    DisplayRgb = (int)Inno.Native.ImGui.ImGuiColorEditFlags.DisplayRgb,
    /// <summary>
    /// Selects the display hsv presentation behavior.
    /// </summary>
    DisplayHsv = (int)Inno.Native.ImGui.ImGuiColorEditFlags.DisplayHsv,
    /// <summary>
    /// // "
    /// </summary>
    DisplayHex = (int)Inno.Native.ImGui.ImGuiColorEditFlags.DisplayHex,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: _display_ values formatted as 0..255.
    /// </summary>
    Uint8 = (int)Inno.Native.ImGui.ImGuiColorEditFlags.Uint8,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: _display_ values formatted as 0.0f..1.0f floats instead of 0..255 integers. No round-trip of value via integers.
    /// </summary>
    Float = (int)Inno.Native.ImGui.ImGuiColorEditFlags.Float,
    /// <summary>
    /// // ColorPicker: bar for Hue, rectangle for Sat/Value.
    /// </summary>
    PickerHueBar = (int)Inno.Native.ImGui.ImGuiColorEditFlags.PickerHueBar,
    /// <summary>
    /// // ColorPicker: wheel for Hue, triangle for Sat/Value.
    /// </summary>
    PickerHueWheel = (int)Inno.Native.ImGui.ImGuiColorEditFlags.PickerHueWheel,
    /// <summary>
    /// Selects the input rgb presentation behavior.
    /// </summary>
    InputRgb = (int)Inno.Native.ImGui.ImGuiColorEditFlags.InputRgb,
    /// <summary>
    /// Selects the input hsv presentation behavior.
    /// </summary>
    InputHsv = (int)Inno.Native.ImGui.ImGuiColorEditFlags.InputHsv,
    /// <summary>
    /// Selects the default options presentation behavior.
    /// </summary>
    DefaultOptions = (int)Inno.Native.ImGui.ImGuiColorEditFlags.DefaultOptions,
    /// <summary>
    /// Selects the alpha mask presentation behavior.
    /// </summary>
    AlphaMask = (int)Inno.Native.ImGui.ImGuiColorEditFlags.AlphaMask,
    /// <summary>
    /// Selects the display mask presentation behavior.
    /// </summary>
    DisplayMask = (int)Inno.Native.ImGui.ImGuiColorEditFlags.DisplayMask,
    /// <summary>
    /// Selects the data type mask presentation behavior.
    /// </summary>
    DataTypeMask = (int)Inno.Native.ImGui.ImGuiColorEditFlags.DataTypeMask,
    /// <summary>
    /// Selects the picker mask presentation behavior.
    /// </summary>
    PickerMask = (int)Inno.Native.ImGui.ImGuiColorEditFlags.PickerMask,
    /// <summary>
    /// Selects the input mask presentation behavior.
    /// </summary>
    InputMask = (int)Inno.Native.ImGui.ImGuiColorEditFlags.InputMask,
}
