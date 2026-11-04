using System;

using Inno.Core.Mathematics;

namespace Inno.UI;

/// <summary>
/// Captures one element's visual geometry without retaining a backend element or borrowing native memory.
/// </summary>
/// <remarks>
/// Bounds are axis-aligned context-pixel bounds after scrolling and transforms. Clip bounds describe
/// the enclosing rectangular clip, not an exact rounded or arbitrary clip mask. Use backend hit testing
/// for interaction. Density has already been applied and must not be multiplied again.
/// </remarks>
public readonly record struct UiElementLayout
{
    /// <summary>
    /// Creates a neutral element measurement belonging to a containing layout snapshot.
    /// </summary>
    /// <param name="elementId">
    /// The authored element ID used by the request.
    /// </param>
    /// <param name="status">
    /// Whether the element is missing, hidden, or measurable.
    /// </param>
    /// <param name="contentBounds">
    /// The transformed content box in context pixels; meaningful only when available.
    /// </param>
    /// <param name="borderBounds">
    /// The transformed border box in context pixels; meaningful only when available.
    /// </param>
    /// <param name="clipBounds">
    /// The effective rectangular clip intersected with the context viewport.
    /// </param>
    /// <param name="localToContext">
    /// The column-vector transform from border-local pixels to homogeneous context pixels.
    /// Divide transformed X and Y by W; singular transforms do not grant an inverse mapping.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The element ID is empty or the status is undefined.
    /// </exception>
    public UiElementLayout(
        string elementId,
        UiElementLayoutStatus status,
        Rect contentBounds,
        Rect borderBounds,
        Rect clipBounds,
        Matrix localToContext
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(nameof(status));
        this.elementId = elementId;
        this.status = status;
        this.contentBounds = contentBounds;
        this.borderBounds = borderBounds;
        this.clipBounds = clipBounds;
        this.localToContext = localToContext;
    }

    /// <summary>
    /// Gets the requested authored element identity within its document.
    /// </summary>
    public string elementId { get; }
    /// <summary>
    /// Gets whether the requested element has measurable geometry.
    /// </summary>
    public UiElementLayoutStatus status { get; }
    /// <summary>
    /// Gets the transformed content box in context pixels.
    /// </summary>
    public Rect contentBounds { get; }
    /// <summary>
    /// Gets the transformed border box in context pixels.
    /// </summary>
    public Rect borderBounds { get; }
    /// <summary>
    /// Gets the enclosing rectangular clip in context pixels.
    /// </summary>
    public Rect clipBounds { get; }
    /// <summary>
    /// Gets the transform from border-local pixels to homogeneous context pixels.
    /// </summary>
    public Matrix localToContext { get; }
    /// <summary>
    /// Projects border-local pixels into context pixels using this captured geometry.
    /// </summary>
    /// <param name="point">
    /// A finite point relative to the element's untransformed border origin.
    /// </param>
    /// <param name="result">
    /// Receives context pixels, or zero when the element or projection is unavailable.
    /// </param>
    /// <returns>
    /// Whether the captured element and homogeneous projection are usable.
    /// </returns>
    public bool TryLocalToContext(
        Vector2 point,
        out Vector2 result
    ) => TryProject(localToContext, point, out result);

    /// <summary>
    /// Maps context pixels back onto the element's local plane, including perspective transforms.
    /// </summary>
    /// <param name="point">
    /// A finite context-pixel point; density is already accounted for.
    /// </param>
    /// <param name="result">
    /// Receives border-local pixels, or zero for hidden, missing, or singular geometry.
    /// </param>
    /// <returns>
    /// Whether the captured planar transform is invertible and the point can be projected.
    /// </returns>
    public bool TryContextToLocal(
        Vector2 point,
        out Vector2 result
    ) {
        result = default;
        if (status != UiElementLayoutStatus.Available)
            return false;
        Matrix plane = localToContext;
        plane.m13 = plane.m23 = plane.m31 = plane.m32 = plane.m34 = plane.m43 = 0;
        plane.m33 = 1;
        float determinant = Matrix.Determinant(plane);
        if (!float.IsFinite(determinant) || MathF.Abs(determinant) < MathHelper.C_TOLERANCE)
            return false;
        return TryProject(Matrix.Invert(plane), point, out result);
    }

    private bool TryProject(
        Matrix transform,
        Vector2 point,
        out Vector2 result
    ) {
        result = default;
        if (status != UiElementLayoutStatus.Available || !float.IsFinite(point.x) || !float.IsFinite(point.y))
            return false;
        Vector4 projected = Vector4.Transform(new Vector4(point.x, point.y, 0, 1), transform);
        if (!float.IsFinite(projected.w) || MathF.Abs(projected.w) < MathHelper.C_TOLERANCE)
            return false;
        float x = projected.x / projected.w;
        float y = projected.y / projected.w;
        if (!float.IsFinite(x) || !float.IsFinite(y))
            return false;
        result = new Vector2(x, y);
        return true;
    }

}
