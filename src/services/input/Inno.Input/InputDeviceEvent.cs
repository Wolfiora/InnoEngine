using Inno.Core.Events;

namespace Inno.Input;

/// <summary>
/// Announces a committed device transition through the owning session's event dispatcher.
/// </summary>
/// <param name="change">
/// A neutral copy of the transition retained by the next frame snapshot.
/// </param>
public sealed class InputDeviceEvent(InputDeviceChange change) : Event
{
    /// <summary>
    /// Gets the transition independent of the provider's lifetime.
    /// </summary>
    public InputDeviceChange change { get; } = change;
}
