using System;
using System.Collections.Generic;
using Inno.Core.Input;
using Inno.Core.Mathematics;

namespace Inno.UI;

/// <summary>
/// Carries one immutable input snapshot across the UI backend boundary.
/// </summary>
public sealed class UIInputSnapshot
{
    /// <summary>
    /// Creates one backend-neutral UI input snapshot.
    /// </summary>
    /// <param name="mousePosition">
    /// Pointer position in surface coordinates.
    /// </param>
    /// <param name="scrollDelta">
    /// Pointer wheel movement.
    /// </param>
    /// <param name="modifiers">
    /// Active keyboard modifiers.
    /// </param>
    /// <param name="keysPressed">
    /// Physical keys pressed this frame.
    /// </param>
    /// <param name="keysReleased">
    /// Physical keys released this frame.
    /// </param>
    /// <param name="buttonsPressed">
    /// Pointer buttons pressed this frame.
    /// </param>
    /// <param name="buttonsReleased">
    /// Pointer buttons released this frame.
    /// </param>
    /// <param name="textInput">
    /// Ordered Unicode text commits.
    /// </param>
    public UIInputSnapshot(
        Vector2 mousePosition,
        Vector2 scrollDelta,
        KeyModifier modifiers,
        IReadOnlyCollection<KeyCode> keysPressed,
        IReadOnlyCollection<KeyCode> keysReleased,
        IReadOnlyCollection<MouseButton> buttonsPressed,
        IReadOnlyCollection<MouseButton> buttonsReleased,
        IReadOnlyList<string> textInput
    ) {
        ArgumentNullException.ThrowIfNull(keysPressed);
        ArgumentNullException.ThrowIfNull(keysReleased);
        ArgumentNullException.ThrowIfNull(buttonsPressed);
        ArgumentNullException.ThrowIfNull(buttonsReleased);
        ArgumentNullException.ThrowIfNull(textInput);
        this.mousePosition = mousePosition;
        this.scrollDelta = scrollDelta;
        this.modifiers = modifiers;
        this.keysPressed = keysPressed;
        this.keysReleased = keysReleased;
        this.buttonsPressed = buttonsPressed;
        this.buttonsReleased = buttonsReleased;
        this.textInput = textInput;
    }

    /// <summary>
    /// Gets the pointer position.
    /// </summary>
    public Vector2 mousePosition { get; }

    /// <summary>
    /// Gets pointer wheel movement.
    /// </summary>
    public Vector2 scrollDelta { get; }

    /// <summary>
    /// Gets active keyboard modifiers.
    /// </summary>
    public KeyModifier modifiers { get; }

    /// <summary>
    /// Gets physical keys pressed this frame.
    /// </summary>
    public IReadOnlyCollection<KeyCode> keysPressed { get; }

    /// <summary>
    /// Gets physical keys released this frame.
    /// </summary>
    public IReadOnlyCollection<KeyCode> keysReleased { get; }

    /// <summary>
    /// Gets pointer buttons pressed this frame.
    /// </summary>
    public IReadOnlyCollection<MouseButton> buttonsPressed { get; }

    /// <summary>
    /// Gets pointer buttons released this frame.
    /// </summary>
    public IReadOnlyCollection<MouseButton> buttonsReleased { get; }

    /// <summary>
    /// Gets ordered Unicode text commits.
    /// </summary>
    public IReadOnlyList<string> textInput { get; }
}
