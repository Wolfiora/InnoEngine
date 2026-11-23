using System;

namespace Inno.UI;

/// <summary>
/// Defines one independent UI layout and interaction surface.
/// </summary>
public readonly record struct UIContextOptions
{
    /// <summary>
    /// Creates validated context options.
    /// </summary>
    /// <param name="name">
    /// A non-empty diagnostic context name.
    /// </param>
    /// <param name="width">
    /// The positive initial width in pixels.
    /// </param>
    /// <param name="height">
    /// The positive initial height in pixels.
    /// </param>
    /// <param name="density">
    /// The positive density-independent pixel ratio.
    /// </param>
    public UIContextOptions(
        string name,
        int width,
        int height,
        float density = 1f
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        if (!float.IsFinite(density) || density <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(density));
        }
        this.name = name.Trim();
        this.width = width;
        this.height = height;
        this.density = density;
    }

    /// <summary>
    /// Gets the diagnostic context name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the initial width in pixels.
    /// </summary>
    public int width { get; }

    /// <summary>
    /// Gets the initial height in pixels.
    /// </summary>
    public int height { get; }

    /// <summary>
    /// Gets the density-independent pixel ratio.
    /// </summary>
    public float density { get; }
}
