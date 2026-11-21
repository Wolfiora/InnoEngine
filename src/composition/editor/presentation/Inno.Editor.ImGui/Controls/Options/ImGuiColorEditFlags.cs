using System;

using NativeColorEditFlags = Inno.Native.ImGui.ImGuiColorEditFlags;

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
    None = (int)NativeColorEditFlags.None,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: ignore Alpha component (will only read 3 components from the input pointer).
    /// </summary>
    NoAlpha = (int)NativeColorEditFlags.NoAlpha,
    /// <summary>
    /// // ColorEdit: disable picker when clicking on color square.
    /// </summary>
    NoPicker = (int)NativeColorEditFlags.NoPicker,
    /// <summary>
    /// // ColorEdit: disable toggling options menu when right-clicking on inputs/small preview.
    /// </summary>
    NoOptions = (int)NativeColorEditFlags.NoOptions,
    /// <summary>
    /// // ColorEdit, ColorPicker: disable color square preview next to the inputs. (e.g. to show only the inputs)
    /// </summary>
    NoSmallPreview = (int)NativeColorEditFlags.NoSmallPreview,
    /// <summary>
    /// // ColorEdit, ColorPicker: disable inputs sliders/text widgets (e.g. to show only the small preview color square).
    /// </summary>
    NoInputs = (int)NativeColorEditFlags.NoInputs,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: disable tooltip when hovering the preview.
    /// </summary>
    NoTooltip = (int)NativeColorEditFlags.NoTooltip,
    /// <summary>
    /// // ColorEdit, ColorPicker: disable display of inline text label (the label is still forwarded to the tooltip and picker).
    /// </summary>
    NoLabel = (int)NativeColorEditFlags.NoLabel,
    /// <summary>
    /// // ColorPicker: disable bigger color preview on right side of the picker, use small color square preview instead.
    /// </summary>
    NoSidePreview = (int)NativeColorEditFlags.NoSidePreview,
    /// <summary>
    /// // ColorEdit: disable drag and drop target/source. ColorButton: disable drag and drop source.
    /// </summary>
    NoDragDrop = (int)NativeColorEditFlags.NoDragDrop,
    /// <summary>
    /// // ColorButton: disable border (which is enforced by default)
    /// </summary>
    NoBorder = (int)NativeColorEditFlags.NoBorder,
    /// <summary>
    /// // ColorEdit: disable rendering R/G/B/A color marker. May also be disabled globally by setting style.ColorMarkerSize = 0.
    /// </summary>
    NoColorMarkers = (int)NativeColorEditFlags.NoColorMarkers,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: disable alpha in the preview,. Contrary to _NoAlpha it may still be edited when calling ColorEdit4()/ColorPicker4(). For ColorButton() this does the same as _NoAlpha.
    /// </summary>
    AlphaOpaque = (int)NativeColorEditFlags.AlphaOpaque,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: disable rendering a checkerboard background behind transparent color.
    /// </summary>
    AlphaNoBg = (int)NativeColorEditFlags.AlphaNoBg,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: display half opaque / half transparent preview.
    /// </summary>
    AlphaPreviewHalf = (int)NativeColorEditFlags.AlphaPreviewHalf,
    /// <summary>
    /// // ColorEdit, ColorPicker: show vertical alpha bar/gradient in picker.
    /// </summary>
    AlphaBar = (int)NativeColorEditFlags.AlphaBar,
    /// <summary>
    /// Selects the hdr presentation behavior.
    /// </summary>
    Hdr = (int)NativeColorEditFlags.Hdr,
    /// <summary>
    /// Selects the display rgb presentation behavior.
    /// </summary>
    DisplayRgb = (int)NativeColorEditFlags.DisplayRgb,
    /// <summary>
    /// Selects the display hsv presentation behavior.
    /// </summary>
    DisplayHsv = (int)NativeColorEditFlags.DisplayHsv,
    /// <summary>
    /// // "
    /// </summary>
    DisplayHex = (int)NativeColorEditFlags.DisplayHex,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: _display_ values formatted as 0..255.
    /// </summary>
    Uint8 = (int)NativeColorEditFlags.Uint8,
    /// <summary>
    /// // ColorEdit, ColorPicker, ColorButton: _display_ values formatted as 0.0f..1.0f floats instead of 0..255 integers. No round-trip of value via integers.
    /// </summary>
    Float = (int)NativeColorEditFlags.Float,
    /// <summary>
    /// // ColorPicker: bar for Hue, rectangle for Sat/Value.
    /// </summary>
    PickerHueBar = (int)NativeColorEditFlags.PickerHueBar,
    /// <summary>
    /// // ColorPicker: wheel for Hue, triangle for Sat/Value.
    /// </summary>
    PickerHueWheel = (int)NativeColorEditFlags.PickerHueWheel,
    /// <summary>
    /// Selects the input rgb presentation behavior.
    /// </summary>
    InputRgb = (int)NativeColorEditFlags.InputRgb,
    /// <summary>
    /// Selects the input hsv presentation behavior.
    /// </summary>
    InputHsv = (int)NativeColorEditFlags.InputHsv,
    /// <summary>
    /// Selects the default options presentation behavior.
    /// </summary>
    DefaultOptions = (int)NativeColorEditFlags.DefaultOptions,
    /// <summary>
    /// Selects the alpha mask presentation behavior.
    /// </summary>
    AlphaMask = (int)NativeColorEditFlags.AlphaMask,
    /// <summary>
    /// Selects the display mask presentation behavior.
    /// </summary>
    DisplayMask = (int)NativeColorEditFlags.DisplayMask,
    /// <summary>
    /// Selects the data type mask presentation behavior.
    /// </summary>
    DataTypeMask = (int)NativeColorEditFlags.DataTypeMask,
    /// <summary>
    /// Selects the picker mask presentation behavior.
    /// </summary>
    PickerMask = (int)NativeColorEditFlags.PickerMask,
    /// <summary>
    /// Selects the input mask presentation behavior.
    /// </summary>
    InputMask = (int)NativeColorEditFlags.InputMask
}
