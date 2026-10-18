using System;
using System.Collections.Generic;

namespace Inno.Rendering;

/// <summary>
/// Defines a destination pixel rectangle without assuming producer or rendering semantics.
/// </summary>
public readonly record struct RenderViewport
{
    /// <summary>
    /// Creates a render viewport.
    /// </summary>
    /// <param name="x">
    /// Left pixel offset in the destination.
    /// </param>
    /// <param name="y">
    /// Top pixel offset in the destination.
    /// </param>
    /// <param name="width">
    /// Positive viewport width.
    /// </param>
    /// <param name="height">
    /// Positive viewport height.
    /// </param>
    public RenderViewport(
        int x,
        int y,
        int width,
        int height
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// Gets the left pixel offset.
    /// </summary>
    public int x { get; }

    /// <summary>
    /// Gets the top pixel offset.
    /// </summary>
    public int y { get; }

    /// <summary>
    /// Gets the viewport width.
    /// </summary>
    public int width { get; }

    /// <summary>
    /// Gets the viewport height.
    /// </summary>
    public int height { get; }
}

