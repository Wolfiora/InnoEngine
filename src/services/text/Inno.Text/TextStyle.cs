using System;

namespace Inno.Text;

/// <summary>
/// Defines immutable font selection and sizing for one shaping operation.
/// </summary>
public readonly record struct TextStyle
{
    /// <summary>
    /// Creates validated text styling.
    /// </summary>
    /// <param name="fontSize">
    /// The positive font size in logical pixels.
    /// </param>
    /// <param name="faceIndex">
    /// The zero-based face index within the font collection.
    /// </param>
    /// <param name="weight">
    /// The requested CSS-compatible weight from 1 through 1000.
    /// </param>
    /// <param name="style">
    /// The requested face slant.
    /// </param>
    /// <param name="letterSpacing">
    /// Additional logical pixels inserted after each glyph.
    /// </param>
    public TextStyle(
        float fontSize,
        int faceIndex = 0,
        int weight = 400,
        TextFontStyle style = TextFontStyle.Normal,
        float letterSpacing = 0f
    ) {
        if (!float.IsFinite(fontSize) || fontSize <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(fontSize));
        }
        ArgumentOutOfRangeException.ThrowIfNegative(faceIndex);
        if (weight is < 1 or > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(weight));
        }
        if (!Enum.IsDefined(style))
        {
            throw new ArgumentOutOfRangeException(nameof(style));
        }
        if (!float.IsFinite(letterSpacing))
        {
            throw new ArgumentOutOfRangeException(nameof(letterSpacing));
        }
        this.fontSize = fontSize;
        this.faceIndex = faceIndex;
        this.weight = weight;
        this.style = style;
        this.letterSpacing = letterSpacing;
    }

    /// <summary>
    /// Gets a 16-pixel regular default style.
    /// </summary>
    public static TextStyle defaultValue { get; } = new(16f);

    /// <summary>
    /// Gets the logical font size.
    /// </summary>
    public float fontSize { get; }

    /// <summary>
    /// Gets the collection face index.
    /// </summary>
    public int faceIndex { get; }

    /// <summary>
    /// Gets the CSS-compatible font weight.
    /// </summary>
    public int weight { get; }

    /// <summary>
    /// Gets the requested face slant.
    /// </summary>
    public TextFontStyle style { get; }

    /// <summary>
    /// Gets additional spacing appended to each glyph advance.
    /// </summary>
    public float letterSpacing { get; }
}
