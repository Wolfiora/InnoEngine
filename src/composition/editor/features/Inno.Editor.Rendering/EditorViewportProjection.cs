namespace Inno.Editor.Rendering;

/// <summary>
/// Identifies the projection family represented by neutral Editor viewport navigation state.
/// </summary>
public enum EditorViewportProjection
{
    /// <summary>
    /// Uses a parallel projection controlled by an orthographic half-height.
    /// </summary>
    Orthographic,
    /// <summary>
    /// Uses a perspective projection controlled by a vertical field of view.
    /// </summary>
    Perspective
}
