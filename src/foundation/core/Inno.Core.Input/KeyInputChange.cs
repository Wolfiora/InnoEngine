namespace Inno.Core.Input;

/// <summary>
/// Captures a physical key transition or repeat at its original input position.
/// </summary>
public sealed record KeyInputChange : InputChange
{
    /// <summary>
    /// Creates a key change independent of any platform event object.
    /// </summary>
    /// <param name="key">
    /// The physical key.
    /// </param>
    /// <param name="isPressed">
    /// True for a press or repeat; false for a release.
    /// </param>
    /// <param name="isRepeat">
    /// True only for an operating-system key repeat.
    /// </param>
    /// <param name="modifiers">
    /// Physical modifiers at this change.
    /// </param>
    /// <exception cref="System.ArgumentException">
    /// A release is marked as a repeated press.
    /// </exception>
    public KeyInputChange(
        KeyCode key,
        bool isPressed,
        bool isRepeat,
        KeyModifier modifiers
    ) : base(modifiers) {
        if (isRepeat && !isPressed)
            throw new System.ArgumentException("A released key cannot be a repeat.", nameof(isRepeat));
        this.key = key;
        this.isPressed = isPressed;
        this.isRepeat = isRepeat;
    }

    /// <summary>
    /// Gets the physical key.
    /// </summary>
    public KeyCode key { get; }
    /// <summary>
    /// Gets whether this change presses or repeats the key.
    /// </summary>
    public bool isPressed { get; }
    /// <summary>
    /// Gets whether this is a repeated press rather than a new transition.
    /// </summary>
    public bool isRepeat { get; }
}
