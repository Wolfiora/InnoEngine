using System;

namespace Inno.Rendering;

/// <summary>
/// Stores one backend-neutral presentation extent in physical pixels.
/// </summary>
public readonly record struct RenderPresentationSize
{
    /// <summary>
    /// Creates a positive presentation extent.
    /// </summary>
    /// <param name="width">
    /// Positive physical-pixel width.
    /// </param>
    /// <param name="height">
    /// Positive physical-pixel height.
    /// </param>
    public RenderPresentationSize(
        int width,
        int height
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// Gets the physical-pixel width.
    /// </summary>
    public int width { get; }

    /// <summary>
    /// Gets the physical-pixel height.
    /// </summary>
    public int height { get; }

    /// <summary>
    /// Gets whether both dimensions describe a real drawable extent; a default struct is invalid.
    /// </summary>
    public bool isValid => width > 0 && height > 0;
}
