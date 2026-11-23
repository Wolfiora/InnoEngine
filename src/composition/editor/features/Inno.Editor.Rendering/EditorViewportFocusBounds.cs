using System;
using Inno.Core.Mathematics;

namespace Inno.Editor.Rendering;

/// <summary>
/// Describes a world-space sphere that can be framed by host navigation.
/// </summary>
public readonly record struct EditorViewportFocusBounds
{
    /// <summary>
    /// Creates a finite focus bound.
    /// </summary>
    /// <param name="center">
    /// World-space focus center.
    /// </param>
    /// <param name="radius">
    /// Non-negative world-space radius.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the center contains a non-finite value.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the radius is negative or non-finite.
    /// </exception>
    public EditorViewportFocusBounds(
        Vector3 center,
        float radius
    ) {
        if (!IsFinite(center))
        {
            throw new ArgumentException("Focus center components must be finite.", nameof(center));
        }
        if (!float.IsFinite(radius) || radius < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(radius));
        }
        this.center = center;
        this.radius = radius;
    }

    /// <summary>
    /// Gets the world-space focus center.
    /// </summary>
    public Vector3 center { get; }

    /// <summary>
    /// Gets the non-negative world-space radius.
    /// </summary>
    public float radius { get; }

    private static bool IsFinite(Vector3 value) => float.IsFinite(value.x) && float.IsFinite(value.y) && float.IsFinite(value.z);
}
