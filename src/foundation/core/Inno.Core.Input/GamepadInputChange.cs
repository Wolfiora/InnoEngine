using System;

namespace Inno.Core.Input;

/// <summary>
/// Identifies an ordered controller fact by its connection-lifetime identity, never a native pointer.
/// </summary>
public abstract record GamepadInputChange : InputChange
{
    /// <summary>
    /// Captures a nonzero connection identity assigned by its input host.
    /// </summary>
    /// <param name="deviceId">
    /// Process-local connection identity; reconnection must not reuse a retired identity.
    /// </param>
    private protected GamepadInputChange(ulong deviceId) : base(KeyModifier.None)
    {
        ArgumentOutOfRangeException.ThrowIfZero(deviceId);
        this.deviceId = deviceId;
    }

    /// <summary>
    /// Gets the connection identity; it must not be persisted as a user device preference.
    /// </summary>
    public ulong deviceId { get; }
}
