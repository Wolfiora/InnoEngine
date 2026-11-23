using Inno.Core.Identity;
using Inno.Core.Mathematics;

namespace Inno.Editor.Rendering;

/// <summary>
/// Receives transient world-space gizmo geometry from independent Editor extensions.
/// </summary>
public interface IEditorGizmoSink
{
    /// <summary>
    /// Adds a visible, selectable icon at an object's world position.
    /// </summary>
    /// <param name="owner">
    /// The scene object selected when the icon is clicked.
    /// </param>
    /// <param name="position">
    /// The icon position in world space.
    /// </param>
    /// <param name="iconID">
    /// A stable semantic icon ID. Unknown IDs use the generic scene icon.
    /// </param>
    void Icon(
        Identity owner,
        Vector3 position,
        string iconID
    );

    /// <summary>
    /// Adds a non-interactive world-space line, normally for selected bounds.
    /// </summary>
    /// <param name="start">
    /// The first world-space endpoint.
    /// </param>
    /// <param name="end">
    /// The second world-space endpoint.
    /// </param>
    void Line(
        Vector3 start,
        Vector3 end
    );
}
