using System;

namespace Inno.UI;

/// <summary>
/// Contains immutable RGBA8 texture pixels addressable by a document source name.
/// </summary>
public sealed class UITextureData
{
    private readonly byte[] m_pixels;

    /// <summary>
    /// Creates a validated immutable texture.
    /// </summary>
    /// <param name="width">
    /// The positive texture width.
    /// </param>
    /// <param name="height">
    /// The positive texture height.
    /// </param>
    /// <param name="pixels">
    /// Tightly packed RGBA8 pixels.
    /// </param>
    public UITextureData(
        int width,
        int height,
        ReadOnlySpan<byte> pixels
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        if (pixels.Length != checked(width * height * 4))
        {
            throw new ArgumentException("UI texture pixels must be tightly packed RGBA8.", nameof(pixels));
        }
        this.width = width;
        this.height = height;
        m_pixels = pixels.ToArray();
    }

    /// <summary>
    /// Gets the texture width.
    /// </summary>
    public int width { get; }

    /// <summary>
    /// Gets the texture height.
    /// </summary>
    public int height { get; }

    /// <summary>
    /// Gets the immutable RGBA8 pixels.
    /// </summary>
    public ReadOnlyMemory<byte> pixels => m_pixels;
}
