using System;
using Inno.Core.Events;
using Inno.Input;
using Inno.Core.Mathematics;
using Inno.Platform;

namespace Inno.Adapter.Input;

/// <summary>
/// Routes backend-neutral platform events into isolated runtime input backends.
/// </summary>
public interface IInputEventSource : IDisposable
{
    /// <summary>
    /// Creates an isolated input backend owned by one runtime session.
    /// </summary>
    /// <returns>
    /// A caller-owned input backend connected to this event source.
    /// </returns>
    IInputBackend CreateBackend();

    /// <summary>
    /// Selects the host window and input-coordinate origin used by runtime text-input sessions.
    /// </summary>
    /// <param name="window">
    /// The borrowed focused window, or null to release the route. Replacing it retires the former session first.
    /// </param>
    /// <param name="origin">
    /// Finite runtime-input origin in window-client logical coordinates. Moving the origin preserves the same window session.
    /// </param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// The origin is not finite.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// The call does not use the source owner thread or session retirement fails.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The source accepts one fixed window and the supplied target identifies a different window.
    /// </exception>
    void SetTextInputTarget(
        IPlatformWindow? window,
        Vector2 origin
    );

    /// <summary>
    /// Routes one backend-neutral platform event to active input backends.
    /// </summary>
    /// <param name="evnt">
    /// Event produced by the active platform application.
    /// </param>
    void ProcessEvent(Event evnt);
}
