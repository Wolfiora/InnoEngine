using System;
using Inno.Core.Mathematics;

namespace Inno.Editor.Rendering;

/// <summary>
/// Describes host-selected presentation preferences without prescribing rendering behavior.
/// </summary>
public readonly record struct EditorViewportPresentation
{
    /// <summary>
    /// Creates presentation preferences for one Editor viewport.
    /// </summary>
    /// <param name="backgroundColor">
    /// Linear clear color preferred by the host panel.
    /// </param>
    /// <param name="pixelDensity">
    /// Physical render pixels per logical presentation unit.
    /// </param>
    public EditorViewportPresentation(
        Color backgroundColor,
        float pixelDensity = 1f
    ) {
        if (!float.IsFinite(pixelDensity) || pixelDensity <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(pixelDensity));
        }
        this.backgroundColor = backgroundColor;
        this.pixelDensity = pixelDensity;
    }

    /// <summary>
    /// Gets the linear clear color preferred by the host panel.
    /// </summary>
    public Color backgroundColor { get; }

    /// <summary>
    /// Gets physical render pixels per logical presentation unit.
    /// </summary>
    public float pixelDensity { get; }
}
