using System;

using Inno.Core.Input;

namespace Inno.Core.Events;

/// <summary>
/// Routes an input-method transition through the existing window event dispatch protocol.
/// </summary>
public sealed class TextCompositionEvent : Event
{
    /// <summary>
    /// Associates an immutable composition fact with its source window.
    /// </summary>
    /// <param name="windowId">
    /// Nonzero source window identity.
    /// </param>
    /// <param name="change">
    /// Transition owned by this window's current text-input operation.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The change is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The window identity is zero.
    /// </exception>
    public TextCompositionEvent(
        uint windowId,
        TextCompositionChange change
    ) {
        ArgumentOutOfRangeException.ThrowIfZero(windowId);
        ArgumentNullException.ThrowIfNull(change);
        this.windowId = windowId;
        this.change = change;
    }

    /// <summary>
    /// Gets the source window, never a process-global text destination.
    /// </summary>
    public uint windowId { get; }
    /// <summary>
    /// Gets the immutable composition transition.
    /// </summary>
    public TextCompositionChange change { get; }
}
