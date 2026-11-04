using System;
using System.Collections.Generic;
using System.Collections.Frozen;

namespace Inno.Core.Input;

/// <summary>
/// Freezes the held controls of one live controller connection at an input boundary.
/// </summary>
public sealed class GamepadState
{
    /// <summary>
    /// Copies held controls without retaining mutable collections supplied by a backend.
    /// </summary>
    /// <param name="deviceId">
    /// Nonzero connection-lifetime identity.
    /// </param>
    /// <param name="buttonsDown">
    /// Held standard buttons, or null for none.
    /// </param>
    /// <param name="axes">
    /// Finite normalized samples, or null for centered axes.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The identity is zero or a control/value is invalid.
    /// </exception>
    public GamepadState(
        ulong deviceId,
        IEnumerable<GamepadButton>? buttonsDown = null,
        IReadOnlyDictionary<GamepadAxis, float>? axes = null
    ) {
        ArgumentOutOfRangeException.ThrowIfZero(deviceId);
        this.deviceId = deviceId;
        this.buttonsDown = buttonsDown?.ToFrozenSet() ?? FrozenSet<GamepadButton>.Empty;
        foreach (GamepadButton button in this.buttonsDown)
            if (!Enum.IsDefined(button))
                throw new ArgumentOutOfRangeException(nameof(buttonsDown));
        this.axes = axes?.ToFrozenDictionary() ?? FrozenDictionary<GamepadAxis, float>.Empty;
        foreach ((GamepadAxis axis, float value) in this.axes)
            ValidateAxis(axis, value);
    }

    /// <summary>
    /// Gets the transient connection identity.
    /// </summary>
    public ulong deviceId { get; }
    /// <summary>
    /// Gets immutable held buttons; edges remain in the ordered input sequence.
    /// </summary>
    public IReadOnlySet<GamepadButton> buttonsDown { get; }
    /// <summary>
    /// Gets immutable raw normalized samples; missing axes read as zero.
    /// </summary>
    public IReadOnlyDictionary<GamepadAxis, float> axes { get; }

    /// <summary>
    /// Applies an axial dead zone and rescales the remaining travel continuously to full range.
    /// </summary>
    /// <param name="axis">
    /// Standard axis to query.
    /// </param>
    /// <param name="deadZone">
    /// Finite dead-zone radius in [0, 1); zero returns the raw normalized sample.
    /// </param>
    /// <returns>
    /// Zero within the dead zone or for an unsampled axis; otherwise normalized remaining travel.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The axis or dead zone is invalid.
    /// </exception>
    public float GetAxis(
        GamepadAxis axis,
        float deadZone = 0f
    ) {
        ValidateAxis(axis, 0f);
        if (!float.IsFinite(deadZone) || deadZone < 0f || deadZone >= 1f)
            throw new ArgumentOutOfRangeException(nameof(deadZone));
        float value = axes.GetValueOrDefault(axis);
        float magnitude = MathF.Abs(value);
        return magnitude <= deadZone ? 0f : MathF.CopySign((magnitude - deadZone) / (1f - deadZone), value);
    }

    internal static void ValidateAxis(
        GamepadAxis axis,
        float value
    ) {
        if (!Enum.IsDefined(axis))
            throw new ArgumentOutOfRangeException(nameof(axis));
        float minimum = axis is GamepadAxis.LeftTrigger or GamepadAxis.RightTrigger ? 0f : -1f;
        if (!float.IsFinite(value) || value < minimum || value > 1f)
            throw new ArgumentOutOfRangeException(nameof(value));
    }
}
