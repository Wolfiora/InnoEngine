namespace Inno.Text;

/// <summary>
/// Describes scalable metrics for one font face and logical size.
/// </summary>
/// <param name="ascender">
/// The scaled face ascender in logical pixels, using the backend's coordinate convention.
/// </param>
/// <param name="descender">
/// The scaled face descender in logical pixels, using the backend's coordinate convention.
/// </param>
/// <param name="lineHeight">
/// The recommended line spacing in logical pixels for the selected face and size.
/// </param>
/// <param name="underlinePosition">
/// The scaled underline position relative to the baseline in logical pixels.
/// </param>
/// <param name="underlineThickness">
/// The scaled underline thickness in logical pixels.
/// </param>
public readonly record struct TextMetrics(
    float ascender,
    float descender,
    float lineHeight,
    float underlinePosition,
    float underlineThickness
);
