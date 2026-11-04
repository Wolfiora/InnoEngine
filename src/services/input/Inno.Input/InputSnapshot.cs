using System;
using System.Collections.Generic;
using System.Collections.Frozen;

using Inno.Core.Input;
using Inno.Core.Mathematics;

namespace Inno.Input;

/// <summary>
/// Captures immutable keyboard and pointer state at one runtime frame boundary.
/// </summary>
public sealed class InputSnapshot
{
    private readonly FrozenSet<KeyCode> m_keysDown;
    private readonly FrozenSet<MouseButton> m_mouseButtonsDown;

    /// <summary>
    /// Captures held state and an immutable ordered sequence at one frame boundary.
    /// </summary>
    /// <param name="frameIndex">
    /// The nonnegative owner frame index.
    /// </param>
    /// <param name="sequence">
    /// The ordered changes; all transient summaries derive from this value.
    /// </param>
    /// <param name="keysDown">
    /// Physical keys held after the captured changes.
    /// </param>
    /// <param name="mouseButtonsDown">
    /// Pointer buttons held after the captured changes.
    /// </param>
    /// <param name="mousePosition">
    /// Final pointer coordinates in the owning window.
    /// </param>
    /// <param name="modifiers">
    /// Physical modifiers at the frame boundary.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The sequence is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The frame index is negative.
    /// </exception>
    public InputSnapshot(
        long frameIndex,
        InputSequence sequence,
        IEnumerable<KeyCode>? keysDown = null,
        IEnumerable<MouseButton>? mouseButtonsDown = null,
        Vector2 mousePosition = default,
        KeyModifier modifiers = KeyModifier.None
    ) {
        ArgumentNullException.ThrowIfNull(sequence);
        if (frameIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(frameIndex));
        this.frameIndex = frameIndex;
        this.sequence = sequence;
        m_keysDown = Copy(keysDown);
        m_mouseButtonsDown = Copy(mouseButtonsDown);
        this.mousePosition = mousePosition;
        this.modifiers = modifiers;
    }

    /// <summary>
    /// Gets the immutable input changes in acquisition order.
    /// </summary>
    public InputSequence sequence { get; }

    /// <summary>
    /// Gets an empty initial snapshot.
    /// </summary>
    public static InputSnapshot empty { get; } = new(0, InputSequence.empty);

    /// <summary>
    /// Gets the runtime frame that owns this snapshot.
    /// </summary>
    public long frameIndex { get; }

    /// <summary>
    /// Gets the current pointer position in window coordinates.
    /// </summary>
    public Vector2 mousePosition { get; }

    /// <summary>
    /// Gets pointer movement accumulated since the previous snapshot.
    /// </summary>
    public Vector2 mouseDelta => sequence.mouseDelta;

    /// <summary>
    /// Gets wheel movement accumulated since the previous snapshot.
    /// </summary>
    public Vector2 scrollDelta => sequence.scrollDelta;

    /// <summary>
    /// Gets the active keyboard modifier mask at this frame boundary.
    /// </summary>
    public KeyModifier modifiers { get; }

    /// <summary>
    /// Gets the ordered UTF-8 text commits captured during this frame.
    /// </summary>
    public IReadOnlyList<string> textInput => sequence.textInput;

    /// <summary>
    /// Gets the physical keys newly pressed during this frame.
    /// </summary>
    public IReadOnlyCollection<KeyCode> keysPressed => sequence.keysPressed;

    /// <summary>
    /// Gets the physical keys newly released during this frame.
    /// </summary>
    public IReadOnlyCollection<KeyCode> keysReleased => sequence.keysReleased;

    /// <summary>
    /// Gets the pointer buttons newly pressed during this frame.
    /// </summary>
    public IReadOnlyCollection<MouseButton> mouseButtonsPressed => sequence.buttonsPressed;

    /// <summary>
    /// Gets the pointer buttons newly released during this frame.
    /// </summary>
    public IReadOnlyCollection<MouseButton> mouseButtonsReleased => sequence.buttonsReleased;

    /// <summary>
    /// Determines whether a key is currently held.
    /// </summary>
    /// <param name="key">
    /// The backend-neutral physical key.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the key is held.
    /// </returns>
    public bool IsKeyDown(KeyCode key) => m_keysDown.Contains(key);

    /// <summary>
    /// Determines whether a key was newly pressed during this frame.
    /// </summary>
    /// <param name="key">
    /// The backend-neutral physical key.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the key transitioned to down.
    /// </returns>
    public bool WasKeyPressed(KeyCode key) => sequence.keysPressed.Contains(key);

    /// <summary>
    /// Determines whether a key was newly released during this frame.
    /// </summary>
    /// <param name="key">
    /// The backend-neutral physical key.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the key transitioned to up.
    /// </returns>
    public bool WasKeyReleased(KeyCode key) => sequence.keysReleased.Contains(key);

    /// <summary>
    /// Determines whether a pointer button is currently held.
    /// </summary>
    /// <param name="button">
    /// The backend-neutral pointer button.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the button is held.
    /// </returns>
    public bool IsMouseButtonDown(MouseButton button) => m_mouseButtonsDown.Contains(button);

    /// <summary>
    /// Determines whether a pointer button was newly pressed during this frame.
    /// </summary>
    /// <param name="button">
    /// The backend-neutral pointer button.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the button transitioned to down.
    /// </returns>
    public bool WasMouseButtonPressed(MouseButton button) => sequence.buttonsPressed.Contains(button);

    /// <summary>
    /// Determines whether a pointer button was newly released during this frame.
    /// </summary>
    /// <param name="button">
    /// The backend-neutral pointer button.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the button transitioned to up.
    /// </returns>
    public bool WasMouseButtonReleased(MouseButton button) => sequence.buttonsReleased.Contains(button);

    private static FrozenSet<TValue> Copy<TValue>(IEnumerable<TValue>? values)
        where TValue : struct, Enum
        => values is null ? FrozenSet<TValue>.Empty : values.ToFrozenSet();
}
