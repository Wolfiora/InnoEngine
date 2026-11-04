using System;

namespace Inno.Core.Input;

/// <summary>
/// Captures a physical controller button transition without manufacturing keyboard events or repeats.
/// </summary>
public sealed record GamepadButtonChange : GamepadInputChange
{
    /// <summary>
    /// Captures one standard controller button edge.
    /// </summary>
    /// <param name="deviceId">
    /// Nonzero connection identity.
    /// </param>
    /// <param name="button">
    /// Standard physical button position.
    /// </param>
    /// <param name="pressed">
    /// True for press, false for release.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The button is undefined.
    /// </exception>
    public GamepadButtonChange(
        ulong deviceId,
        GamepadButton button,
        bool pressed
    ) : base(deviceId) {
        if (!Enum.IsDefined(button))
            throw new ArgumentOutOfRangeException(nameof(button));
        this.button = button;
        this.pressed = pressed;
    }

    /// <summary>
    /// Gets the physical button position.
    /// </summary>
    public GamepadButton button { get; }
    /// <summary>
    /// Gets whether the button became held.
    /// </summary>
    public bool pressed { get; }
}
