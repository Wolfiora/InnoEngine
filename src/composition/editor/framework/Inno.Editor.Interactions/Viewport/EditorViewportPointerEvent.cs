using Inno.Core.Input;
using Inno.Core.Mathematics;

namespace Inno.Editor.Interactions;

/// <summary>
/// Contains one immutable viewport pointer sample in screen and world coordinates.
/// </summary>
public readonly record struct EditorViewportPointerEvent
{
    /// <summary>
    /// Creates one viewport pointer sample.
    /// </summary>
    /// <param name="pointerID">
    /// Platform pointer identity.
    /// </param>
    /// <param name="phase">
    /// Gesture lifecycle phase.
    /// </param>
    /// <param name="screenPosition">
    /// Viewport-local pixel position.
    /// </param>
    /// <param name="worldPosition">
    /// Corresponding world position.
    /// </param>
    /// <param name="button">
    /// Platform button number.
    /// </param>
    /// <param name="modifiers">
    /// Active keyboard modifiers.
    /// </param>
    public EditorViewportPointerEvent(
        int pointerID,
        EditorViewportPointerPhase phase,
        Vector2 screenPosition,
        Vector2 worldPosition,
        int button,
        KeyModifier modifiers
    ) {
        this.pointerID = pointerID;
        this.phase = phase;
        this.screenPosition = screenPosition;
        this.worldPosition = worldPosition;
        this.button = button;
        this.modifiers = modifiers;
    }

    /// <summary>
    /// Gets the platform pointer identity.
    /// </summary>
    public int pointerID { get; }

    /// <summary>
    /// Gets the gesture lifecycle phase.
    /// </summary>
    public EditorViewportPointerPhase phase { get; }

    /// <summary>
    /// Gets the viewport-local pixel position.
    /// </summary>
    public Vector2 screenPosition { get; }

    /// <summary>
    /// Gets the corresponding world position.
    /// </summary>
    public Vector2 worldPosition { get; }

    /// <summary>
    /// Gets the platform button number.
    /// </summary>
    public int button { get; }

    /// <summary>
    /// Gets active keyboard modifiers.
    /// </summary>
    public KeyModifier modifiers { get; }
}
