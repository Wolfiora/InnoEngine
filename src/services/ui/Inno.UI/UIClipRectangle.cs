namespace Inno.UI;

/// <summary>
/// Defines an integer clipping rectangle in UI surface coordinates.
/// </summary>
/// <param name="x">
/// The horizontal or first component.
/// </param>
/// <param name="y">
/// The vertical or second component.
/// </param>
/// <param name="width">
/// The width in logical units or pixels required by this operation.
/// </param>
/// <param name="height">
/// The height in logical units or pixels required by this operation.
/// </param>
public readonly record struct UIClipRectangle(
    int x,
    int y,
    int width,
    int height
);
