namespace Inno.Text;

/// <summary>
/// Provides script-friendly access to Unicode shaping and glyph rasterization.
/// </summary>
public static class Text
{
    /// <summary>
    /// Shapes text with default automatic language and direction detection.
    /// </summary>
    /// <param name="font">
    /// The imported font source.
    /// </param>
    /// <param name="text">
    /// The Unicode source text.
    /// </param>
    /// <param name="style">
    /// Font selection and sizing.
    /// </param>
    /// <returns>
    /// The immutable shaped layout.
    /// </returns>
    public static TextLayout Shape(
        FontAsset font,
        string text,
        TextStyle style
    ) => TextExecutionContext.current.Shape(font, text, style, TextShapingOptions.automatic);

    /// <summary>
    /// Shapes text with explicit language, script, and direction hints.
    /// </summary>
    /// <param name="font">
    /// The imported font source.
    /// </param>
    /// <param name="text">
    /// The Unicode source text.
    /// </param>
    /// <param name="style">
    /// Font selection and sizing.
    /// </param>
    /// <param name="options">
    /// Language, script, and direction hints.
    /// </param>
    /// <returns>
    /// The immutable shaped layout.
    /// </returns>
    public static TextLayout Shape(
        FontAsset font,
        string text,
        TextStyle style,
        TextShapingOptions options
    ) => TextExecutionContext.current.Shape(font, text, style, options);

    /// <summary>
    /// Rasterizes one shaped glyph into immutable 8-bit coverage.
    /// </summary>
    /// <param name="font">
    /// The imported font source.
    /// </param>
    /// <param name="faceIndex">
    /// The zero-based collection face index.
    /// </param>
    /// <param name="glyphID">
    /// The font-specific glyph identifier.
    /// </param>
    /// <param name="fontSize">
    /// The positive logical pixel size.
    /// </param>
    /// <returns>
    /// The immutable glyph bitmap.
    /// </returns>
    public static GlyphBitmap Rasterize(
        FontAsset font,
        int faceIndex,
        uint glyphID,
        float fontSize
    ) => TextExecutionContext.current.Rasterize(font, faceIndex, glyphID, fontSize);
}
