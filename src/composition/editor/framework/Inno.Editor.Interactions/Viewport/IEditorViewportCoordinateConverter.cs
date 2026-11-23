using Inno.Core.Mathematics;

namespace Inno.Editor.Interactions;

/// <summary>
/// Converts coordinates for one viewport camera without exposing its presentation backend.
/// </summary>
public interface IEditorViewportCoordinateConverter
{
    /// <summary>
    /// Converts viewport-local pixel coordinates to world coordinates.
    /// </summary>
    /// <param name="screenPosition">
    /// Viewport-local pixel coordinates.
    /// </param>
    /// <returns>
    /// World coordinates.
    /// </returns>
    Vector2 ScreenToWorld(Vector2 screenPosition);

    /// <summary>
    /// Converts world coordinates to viewport-local pixel coordinates.
    /// </summary>
    /// <param name="worldPosition">
    /// World coordinates.
    /// </param>
    /// <returns>
    /// Viewport-local pixel coordinates.
    /// </returns>
    Vector2 WorldToScreen(Vector2 worldPosition);
}
