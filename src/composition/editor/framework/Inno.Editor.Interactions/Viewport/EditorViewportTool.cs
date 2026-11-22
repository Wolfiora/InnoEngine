using System;

namespace Inno.Editor.Interactions;

/// <summary>
/// Defines one presentation-independent, reloadable viewport editing tool.
/// </summary>
public abstract class EditorViewportTool
{
    /// <summary>
    /// Gets the globally stable tool identity.
    /// </summary>
    public abstract string id { get; }

    /// <summary>
    /// Gets the cursor requested while this tool is active.
    /// </summary>
    public virtual EditorViewportCursor cursor => EditorViewportCursor.Arrow;

    /// <summary>
    /// Handles a pointer press.
    /// </summary>
    /// <param name="context">
    /// Gesture and coordinate context.
    /// </param>
    /// <param name="pointer">
    /// Immutable pointer sample.
    /// </param>
    public virtual void OnPointerDown(
        EditorViewportToolContext context,
        EditorViewportPointerEvent pointer
    ) => ArgumentNullException.ThrowIfNull(context);

    /// <summary>
    /// Handles pointer movement or hover.
    /// </summary>
    /// <param name="context">
    /// Gesture and coordinate context.
    /// </param>
    /// <param name="pointer">
    /// Immutable pointer sample.
    /// </param>
    public virtual void OnPointerMove(
        EditorViewportToolContext context,
        EditorViewportPointerEvent pointer
    ) => ArgumentNullException.ThrowIfNull(context);

    /// <summary>
    /// Handles a pointer release.
    /// </summary>
    /// <param name="context">
    /// Gesture and coordinate context.
    /// </param>
    /// <param name="pointer">
    /// Immutable pointer sample.
    /// </param>
    public virtual void OnPointerUp(
        EditorViewportToolContext context,
        EditorViewportPointerEvent pointer
    ) => ArgumentNullException.ThrowIfNull(context);

    /// <summary>
    /// Handles platform cancellation of an active gesture.
    /// </summary>
    /// <param name="context">
    /// Gesture and coordinate context.
    /// </param>
    /// <param name="pointer">
    /// Immutable pointer sample.
    /// </param>
    public virtual void OnPointerCancel(
        EditorViewportToolContext context,
        EditorViewportPointerEvent pointer
    ) {
        ArgumentNullException.ThrowIfNull(context);
        context.CompleteHistoryGesture(commit: false);
        context.ReleasePointer();
    }

    /// <summary>
    /// Handles a focused viewport shortcut before global action routing.
    /// </summary>
    /// <param name="context">
    /// Gesture and coordinate context.
    /// </param>
    /// <param name="shortcut">
    /// Pressed key sample.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the tool consumed the key.
    /// </returns>
    public virtual bool OnShortcut(
        EditorViewportToolContext context,
        EditorViewportShortcut shortcut
    ) {
        ArgumentNullException.ThrowIfNull(context);
        return false;
    }

    /// <summary>
    /// Draws transient gizmos and diagnostics over the viewport.
    /// </summary>
    /// <param name="context">
    /// Gesture and coordinate context.
    /// </param>
    public virtual void DrawOverlay(EditorViewportToolContext context) => ArgumentNullException.ThrowIfNull(context);
}

/// <summary>
/// Selects the lifecycle phase of one viewport pointer sample.
/// </summary>
public enum EditorViewportPointerPhase
{
    /// <summary>
    /// The pointer button began a gesture.
    /// </summary>
    Down,
    /// <summary>
    /// The pointer moved or changed hover position.
    /// </summary>
    Move,
    /// <summary>
    /// The pointer button ended a gesture.
    /// </summary>
    Up,
    /// <summary>
    /// The platform cancelled the gesture.
    /// </summary>
    Cancel
}

/// <summary>
/// Selects a presentation-independent cursor requested by a viewport tool.
/// </summary>
public enum EditorViewportCursor
{
    /// <summary>
    /// Uses the normal arrow cursor.
    /// </summary>
    Arrow,
    /// <summary>
    /// Uses a precise crosshair.
    /// </summary>
    Crosshair,
    /// <summary>
    /// Uses a hand suitable for panning.
    /// </summary>
    Hand,
    /// <summary>
    /// Uses four-direction movement arrows.
    /// </summary>
    Move,
    /// <summary>
    /// Uses horizontal resizing arrows.
    /// </summary>
    ResizeHorizontal,
    /// <summary>
    /// Uses vertical resizing arrows.
    /// </summary>
    ResizeVertical,
    /// <summary>
    /// Hides the pointer while the tool owns it.
    /// </summary>
    Hidden
}
