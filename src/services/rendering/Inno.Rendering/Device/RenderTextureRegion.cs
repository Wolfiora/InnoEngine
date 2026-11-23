using System;

namespace Inno.Rendering;

/// <summary>
/// Describes one texture subresource box for copy and blit commands.
/// </summary>
public readonly record struct RenderTextureRegion
{
    /// <summary>
    /// Creates a texture subresource box.
    /// </summary>
    /// <param name="mip">
    /// Zero-based mip level.
    /// </param>
    /// <param name="x">
    /// Horizontal texel origin.
    /// </param>
    /// <param name="y">
    /// Vertical texel origin.
    /// </param>
    /// <param name="layer">
    /// Array layer or depth-slice origin.
    /// </param>
    /// <param name="width">
    /// Positive texel width.
    /// </param>
    /// <param name="height">
    /// Positive texel height.
    /// </param>
    /// <param name="depth">
    /// Positive layer or depth-slice count.
    /// </param>
    public RenderTextureRegion(
        int mip,
        int x,
        int y,
        int layer,
        int width,
        int height,
        int depth = 1
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(mip);
        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);
        ArgumentOutOfRangeException.ThrowIfNegative(layer);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depth);
        this.mip = mip;
        this.x = x;
        this.y = y;
        this.layer = layer;
        this.width = width;
        this.height = height;
        this.depth = depth;
    }

    /// <summary>
    /// Gets the zero-based mip level.
    /// </summary>
    public int mip { get; }

    /// <summary>
    /// Gets the horizontal texel origin.
    /// </summary>
    public int x { get; }

    /// <summary>
    /// Gets the vertical texel origin.
    /// </summary>
    public int y { get; }

    /// <summary>
    /// Gets the array layer or depth-slice origin.
    /// </summary>
    public int layer { get; }

    /// <summary>
    /// Gets the texel width.
    /// </summary>
    public int width { get; }

    /// <summary>
    /// Gets the texel height.
    /// </summary>
    public int height { get; }

    /// <summary>
    /// Gets the layer or depth-slice count.
    /// </summary>
    public int depth { get; }
}
