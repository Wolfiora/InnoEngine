using System;

namespace Inno.Runtime;

/// <summary>
/// Stores one centered game-content region within a presentation surface.
/// </summary>
public readonly record struct GamePresentationViewport
{
    /// <summary>
    /// Creates a validated game presentation viewport.
    /// </summary>
    /// <param name="x">
    /// Non-negative horizontal offset.
    /// </param>
    /// <param name="y">
    /// Non-negative vertical offset.
    /// </param>
    /// <param name="width">
    /// Positive content width.
    /// </param>
    /// <param name="height">
    /// Positive content height.
    /// </param>
    public GamePresentationViewport(
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
    /// Gets the horizontal content offset.
    /// </summary>
    public int x { get; }

    /// <summary>
    /// Gets the vertical content offset.
    /// </summary>
    public int y { get; }

    /// <summary>
    /// Gets the content width.
    /// </summary>
    public int width { get; }

    /// <summary>
    /// Gets the content height.
    /// </summary>
    public int height { get; }
}
