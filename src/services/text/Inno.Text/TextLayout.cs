using System;
using System.Collections.Generic;

namespace Inno.Text;

/// <summary>
/// Contains immutable positioned glyphs and aggregate bounds for one shaped string.
/// </summary>
public sealed class TextLayout
{
    private readonly TextGlyph[] m_glyphs;

    /// <summary>
    /// Creates an immutable text layout.
    /// </summary>
    /// <param name="glyphs">
    /// The positioned glyph sequence.
    /// </param>
    /// <param name="metrics">
    /// The face metrics used by the layout.
    /// </param>
    /// <param name="width">
    /// The logical horizontal advance.
    /// </param>
    /// <param name="height">
    /// The logical line height.
    /// </param>
    public TextLayout(
        IEnumerable<TextGlyph> glyphs,
        TextMetrics metrics,
        float width,
        float height
    ) {
        ArgumentNullException.ThrowIfNull(glyphs);
        if (!float.IsFinite(width) || width < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }
        if (!float.IsFinite(height) || height < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }
        m_glyphs = [..glyphs];
        this.metrics = metrics;
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// Gets the immutable positioned glyph sequence.
    /// </summary>
    public IReadOnlyList<TextGlyph> glyphs => m_glyphs;

    /// <summary>
    /// Gets the font metrics used to build this layout.
    /// </summary>
    public TextMetrics metrics { get; }

    /// <summary>
    /// Gets the aggregate horizontal advance.
    /// </summary>
    public float width { get; }

    /// <summary>
    /// Gets the aggregate line height.
    /// </summary>
    public float height { get; }
}
