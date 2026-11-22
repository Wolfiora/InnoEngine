using System;
using Inno.Core.Mathematics;

namespace Inno.Editor.Rendering;

/// <summary>
/// Describes the exact backend-neutral view and projection used to draw a viewport so host tools can manipulate
/// selected scene transforms without knowing the controller's camera model.
/// </summary>
public readonly record struct EditorViewportManipulationSpace
{
    /// <summary>
    /// Creates a manipulation space matching one rendered viewport frame.
    /// </summary>
    /// <param name="viewMatrix">
    /// World-to-view matrix used by the submitted frame.
    /// </param>
    /// <param name="projectionMatrix">
    /// View-to-clip matrix used by the submitted frame.
    /// </param>
    /// <param name="isOrthographic">
    /// Whether the projection is orthographic; this does not imply planar manipulation.
    /// </param>
    /// <param name="plane">
    /// The contributor-selected transform handle orientation, independent of projection.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The plane is not one of the defined neutral manipulation orientations.
    /// </exception>
    public EditorViewportManipulationSpace(
        Matrix viewMatrix,
        Matrix projectionMatrix,
        bool isOrthographic,
        EditorViewportManipulationPlane plane = EditorViewportManipulationPlane.Spatial
    ) {
        if (!Enum.IsDefined(plane))
        {
            throw new ArgumentOutOfRangeException(nameof(plane));
        }
        this.viewMatrix = viewMatrix;
        this.projectionMatrix = projectionMatrix;
        this.isOrthographic = isOrthographic;
        this.plane = plane;
    }

    /// <summary>
    /// Gets the world-to-view matrix used by the submitted frame.
    /// </summary>
    public Matrix viewMatrix { get; }

    /// <summary>
    /// Gets the view-to-clip matrix used by the submitted frame.
    /// </summary>
    public Matrix projectionMatrix { get; }

    /// <summary>
    /// Gets whether the submitted frame used an orthographic projection.
    /// </summary>
    public bool isOrthographic { get; }

    /// <summary>
    /// Gets the contributor-declared neutral manipulation plane, or Spatial for all axes.
    /// </summary>
    public EditorViewportManipulationPlane plane { get; }
}
