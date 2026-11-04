using System;

using Inno.Core.Mathematics;

namespace Inno.UI;

/// <summary>
/// Maps an already density-resolved UI context into the rectangle where its frame is displayed.
/// </summary>
public readonly record struct UiViewportMapping
{
    /// <summary>
    /// Creates the mapping selected by the viewport owner, including letterboxing or editor zoom.
    /// </summary>
    /// <param name="contextSize">
    /// The positive context width and height in pixels.
    /// </param>
    /// <param name="viewport">
    /// The finite, positive destination rectangle in the owner's viewport coordinate space.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A coordinate is not finite or a size is not positive.
    /// </exception>
    public UiViewportMapping(
        Vector2 contextSize,
        Rect viewport
    ) {
        if (!float.IsFinite(contextSize.x) || !float.IsFinite(contextSize.y) || contextSize.x <= 0 || contextSize.y <= 0)
            throw new ArgumentOutOfRangeException(nameof(contextSize));
        if (!float.IsFinite(viewport.x) || !float.IsFinite(viewport.y) || !float.IsFinite(viewport.width) ||
            !float.IsFinite(viewport.height) || viewport.width <= 0 || viewport.height <= 0)
            throw new ArgumentOutOfRangeException(nameof(viewport));
        this.contextSize = contextSize;
        this.viewport = viewport;
    }

    /// <summary>
    /// Gets the density-resolved UI context dimensions.
    /// </summary>
    public Vector2 contextSize { get; }
    /// <summary>
    /// Gets the destination rectangle chosen by the viewport owner.
    /// </summary>
    public Rect viewport { get; }

    /// <summary>
    /// Converts context pixels to the destination coordinate space without applying density again.
    /// </summary>
    /// <param name="point">
    /// A finite point in UI context pixels, including points outside the context.
    /// </param>
    /// <returns>
    /// The corresponding viewport point without clamping.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// This value is uninitialized.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The point or result is not finite.
    /// </exception>
    public Vector2 ToViewport(Vector2 point)
    {
        Validate(point);
        Vector2 result = new(viewport.x + point.x / contextSize.x * viewport.width,
            viewport.y + point.y / contextSize.y * viewport.height);
        Validate(result);
        return result;
    }

    /// <summary>
    /// Converts a destination point into context pixels without applying density again.
    /// </summary>
    /// <param name="point">
    /// A finite viewport point, including points outside the display rectangle.
    /// </param>
    /// <returns>
    /// The corresponding context-pixel point without clamping.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// This value is uninitialized.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The point or result is not finite.
    /// </exception>
    public Vector2 ToContext(Vector2 point)
    {
        Validate(point);
        Vector2 result = new((point.x - viewport.x) / viewport.width * contextSize.x,
            (point.y - viewport.y) / viewport.height * contextSize.y);
        Validate(result);
        return result;
    }

    private void Validate(Vector2 point)
    {
        if (contextSize.x <= 0 || contextSize.y <= 0 || viewport.width <= 0 || viewport.height <= 0)
            throw new InvalidOperationException("The viewport mapping is uninitialized.");
        if (!float.IsFinite(point.x) || !float.IsFinite(point.y))
            throw new ArgumentOutOfRangeException(nameof(point));
    }
}
