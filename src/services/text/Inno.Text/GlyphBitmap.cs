using System;

namespace Inno.Text;

/// <summary>
/// Contains one rasterized glyph as tightly packed 8-bit coverage.
/// </summary>
public sealed class GlyphBitmap
{
    private readonly byte[] m_pixels;

    /// <summary>
    /// Creates a validated immutable glyph bitmap.
    /// </summary>
    /// <param name="width">
    /// The bitmap width in pixels.
    /// </param>
    /// <param name="height">
    /// The bitmap height in pixels.
    /// </param>
    /// <param name="bearingX">
    /// The horizontal bearing in pixels.
    /// </param>
    /// <param name="bearingY">
    /// The vertical bearing in pixels.
    /// </param>
    /// <param name="advanceX">
    /// The horizontal glyph advance.
    /// </param>
    /// <param name="pixels">
    /// Tightly packed row-major 8-bit coverage.
    /// </param>
    public GlyphBitmap(
        int width,
        int height,
        int bearingX,
        int bearingY,
        float advanceX,
        ReadOnlySpan<byte> pixels
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(width);
        ArgumentOutOfRangeException.ThrowIfNegative(height);
        if (pixels.Length != checked(width * height))
        {
            throw new ArgumentException("Glyph coverage length must match its dimensions.", nameof(pixels));
        }
        if (!float.IsFinite(advanceX))
        {
            throw new ArgumentOutOfRangeException(nameof(advanceX));
        }
        this.width = width;
        this.height = height;
        this.bearingX = bearingX;
        this.bearingY = bearingY;
        this.advanceX = advanceX;
        m_pixels = pixels.ToArray();
    }

    /// <summary>
    /// Gets the bitmap width in pixels.
    /// </summary>
    public int width { get; }

    /// <summary>
    /// Gets the bitmap height in pixels.
    /// </summary>
    public int height { get; }

    /// <summary>
    /// Gets the horizontal bearing in pixels.
    /// </summary>
    public int bearingX { get; }

    /// <summary>
    /// Gets the vertical bearing in pixels.
    /// </summary>
    public int bearingY { get; }

    /// <summary>
    /// Gets the horizontal glyph advance.
    /// </summary>
    public float advanceX { get; }

    /// <summary>
    /// Gets tightly packed row-major 8-bit coverage.
    /// </summary>
    public ReadOnlyMemory<byte> pixels => m_pixels;
}
