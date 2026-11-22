using System.Numerics;

using EngineMatrix = Inno.Core.Mathematics.Matrix;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Supplies frame-local matrices and interaction policy without native handles.
/// </summary>
public readonly record struct TransformGizmoOptions
{
    /// <summary>
    /// Gets the viewport's screen-space minimum.
    /// </summary>
    public Vector2 minimum { get; init; }

    /// <summary>
    /// Gets the viewport's screen-space maximum.
    /// </summary>
    public Vector2 maximum { get; init; }

    /// <summary>
    /// Gets the column-vector view matrix.
    /// </summary>
    public EngineMatrix view { get; init; }

    /// <summary>
    /// Gets the projection matrix supplied by the viewport owner.
    /// </summary>
    public EngineMatrix projection { get; init; }

    /// <summary>
    /// Gets whether this viewport uses an orthographic projection.
    /// </summary>
    public bool orthographic { get; init; }

    /// <summary>
    /// Gets whether manipulation can consume pointer input.
    /// </summary>
    public bool enabled { get; init; }

    /// <summary>
    /// Gets the requested handles.
    /// </summary>
    public TransformGizmoOperation operation { get; init; }

    /// <summary>
    /// Gets the handle coordinate space.
    /// </summary>
    public TransformGizmoSpace space { get; init; }

    /// <summary>
    /// Gets whether the X translation axis is excluded by the viewport plane.
    /// </summary>
    public bool maskX { get; init; }

    /// <summary>
    /// Gets whether the Y translation axis is excluded by the viewport plane.
    /// </summary>
    public bool maskY { get; init; }

    /// <summary>
    /// Gets whether the Z translation axis is excluded by the viewport plane.
    /// </summary>
    public bool maskZ { get; init; }
}
