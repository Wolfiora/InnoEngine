namespace Inno.Core.Input;

/// <summary>
/// Announces a controller connection or retires all held state for that connection.
/// </summary>
public sealed record GamepadConnectionChange : GamepadInputChange
{
    /// <summary>
    /// Captures an ordered device lifetime transition.
    /// </summary>
    /// <param name="deviceId">
    /// Nonzero connection identity.
    /// </param>
    /// <param name="connected">
    /// True for attachment; false when the connection and all held controls retire.
    /// </param>
    public GamepadConnectionChange(
        ulong deviceId,
        bool connected
    ) : base(deviceId) => this.connected = connected;

    /// <summary>
    /// Gets whether this transition establishes a live connection.
    /// </summary>
    public bool connected { get; }
}
