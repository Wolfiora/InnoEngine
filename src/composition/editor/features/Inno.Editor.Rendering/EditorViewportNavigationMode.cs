namespace Inno.Editor.Rendering;

/// <summary>
/// Identifies the active interaction model without prescribing a rendering camera type.
/// </summary>
public enum EditorViewportNavigationMode
{
    /// <summary>
    /// Pans and zooms over a provider-defined plane.
    /// </summary>
    Planar,
    /// <summary>
    /// Rotates around a focus pivot and dollies along the view direction.
    /// </summary>
    Orbit,
    /// <summary>
    /// Moves and looks freely from the current view position.
    /// </summary>
    Fly
}
