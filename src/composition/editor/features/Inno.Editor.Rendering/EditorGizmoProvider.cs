using Inno.Core.Identity;
using Inno.Core.Mathematics;

namespace Inno.Editor.Rendering;

/// <summary>
/// Contributes Editor-only icons and bounds without modifying a rendering model.
/// </summary>
public abstract class EditorGizmoProvider
{
    /// <summary>
    /// Submits transient icons and lines for the presented scenes.
    /// </summary>
    /// <param name="context">
    /// The current Scene viewport request.
    /// </param>
    /// <param name="sink">
    /// The collector that owns submitted primitives for this frame.
    /// </param>
    public abstract void Collect(
        EditorGizmoContext context,
        IEditorGizmoSink sink
    );
}

/// <summary>
/// One selectable Editor icon.
/// </summary>
/// <param name="owner">
/// The object that owns the resulting lifetime and state.
/// </param>
/// <param name="position">
/// The vector3 value used to initialize this instance.
/// </param>
/// <param name="iconID">
/// The stable visual identity resolved by the Scene View presentation.
/// </param>
public readonly record struct EditorGizmoIcon(
    Identity owner,
    Vector3 position,
    string iconID
);

/// <summary>
/// One non-interactive Editor line.
/// </summary>
/// <param name="start">
/// The vector3 value used to initialize this instance.
/// </param>
/// <param name="end">
/// The vector3 value used to initialize this instance.
/// </param>
public readonly record struct EditorGizmoLine(
    Vector3 start,
    Vector3 end
);
