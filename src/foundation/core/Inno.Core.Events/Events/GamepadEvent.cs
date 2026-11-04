using System;
using Inno.Core.Input;

namespace Inno.Core.Events;

/// <summary>
/// Delivers one controller fact through the existing platform event stream.
/// </summary>
public sealed class GamepadEvent : Event
{
    /// <summary>
    /// Routes a controller fact to a focused window, or broadcasts a connection transition.
    /// </summary>
    /// <param name="windowId">
    /// Destination window, or zero only for a connection transition.
    /// </param>
    /// <param name="change">
    /// Immutable controller fact in acquisition order.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The change is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A button or axis is broadcast without an input owner.
    /// </exception>
    public GamepadEvent(
        uint windowId,
        GamepadInputChange change
    ) {
        ArgumentNullException.ThrowIfNull(change);
        if (windowId == 0 && change is not GamepadConnectionChange)
            throw new ArgumentException("Controller controls require an explicit window owner.", nameof(windowId));
        this.windowId = windowId;
        this.change = change;
    }

    /// <summary>
    /// Gets the destination window or zero for connection broadcasts.
    /// </summary>
    public uint windowId { get; }
    /// <summary>
    /// Gets the immutable device fact.
    /// </summary>
    public GamepadInputChange change { get; }
}
