namespace Inno.Text;

/// <summary>
/// Describes one positioned glyph emitted by Unicode shaping.
/// </summary>
/// <param name="glyphID">
/// The glyph identifier in the font face that produced this layout.
/// </param>
/// <param name="cluster">
/// The source cluster index reported by the backend. The FreeType/HarfBuzz backend
/// reports a UTF-8 byte offset, not a UTF-16 character index.
/// </param>
/// <param name="advanceX">
/// The horizontal pen advance in logical pixels, including backend-applied letter spacing.
/// </param>
/// <param name="advanceY">
/// The vertical pen advance in logical pixels, including backend-applied letter spacing.
/// </param>
/// <param name="offsetX">
/// The horizontal placement offset from the pen position in logical pixels.
/// </param>
/// <param name="offsetY">
/// The vertical placement offset from the pen position in logical pixels.
/// </param>
public readonly record struct TextGlyph(
    uint glyphID,
    uint cluster,
    float advanceX,
    float advanceY,
    float offsetX,
    float offsetY
);
