using Inno.Core.Identity;

namespace Inno.Input;

/// <summary>
/// Describes one ordered transition retained independently of the plugin that produced it.
/// </summary>
/// <param name="instance">
/// The domain-qualified identifier of the connected instance.
/// </param>
/// <param name="typeID">
/// The stable device protocol ID.
/// </param>
/// <param name="kind">
/// The connection or control transition.
/// </param>
/// <param name="controlID">
/// The control identifier for a value change, otherwise null.
/// </param>
/// <param name="value">
/// The new control value for a value change.
/// </param>
public readonly record struct InputDeviceChange(
    RuntimeIdentity instance,
    string typeID,
    InputDeviceChangeKind kind,
    string? controlID,
    double value
);

/// <summary>
/// Identifies a connection lifetime or control transition.
/// </summary>
public enum InputDeviceChangeKind
{
    /// <summary>
    /// A new instance joined the session.
    /// </summary>
    Connected,
    /// <summary>
    /// A control reported a new scalar value.
    /// </summary>
    ValueChanged,
    /// <summary>
    /// The instance left the session.
    /// </summary>
    Disconnected
}
