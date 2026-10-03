using System;
using System.Collections.Generic;

using Inno.Adapter.Input;
using Inno.Core.Events;
using Inno.Input;

namespace Inno.Adapter.Input.Sdl3;

/// <summary>
/// Routes translated SDL events through Core Events to isolated runtime-session input backends.
/// </summary>
public sealed class Sdl3InputSource : IInputEventSource
{
    private readonly List<Sdl3InputBackend> m_backends = [];
    private readonly EventDispatcher m_events = new();
    private readonly EventHub m_inputHub;
    private readonly uint m_windowId;
    private bool m_disposed;

    /// <summary>
    /// Creates an input source restricted to one window, or to all windows when the identifier is zero.
    /// </summary>
    /// <param name="windowId">
    /// The platform window identifier accepted by this source; zero accepts events from every window.
    /// </param>
    public Sdl3InputSource(uint windowId)
    {
        m_windowId = windowId;
        m_inputHub = m_events.CreateHub();
    }

    /// <summary>
    /// Creates one isolated backend that receives subsequent events from this source.
    /// </summary>
    /// <returns>
    /// A caller-owned backend whose disposal unregisters it from this source.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this source has been disposed.
    /// </exception>
    public IInputBackend CreateBackend()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        var backend = new Sdl3InputBackend(m_windowId, RemoveBackend);
        backend.ConnectSource(m_inputHub.Listen<Event>(backend.ProcessEvent));
        m_backends.Add(backend);
        return backend;
    }

    /// <summary>
    /// Dispatches one translated platform event to active session backends, honoring event consumption.
    /// </summary>
    /// <param name="evnt">
    /// The backend-neutral event produced by the SDL platform adapter.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="evnt"/> is null.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this source has been disposed.
    /// </exception>
    public void ProcessEvent(Event evnt)
    {
        ArgumentNullException.ThrowIfNull(evnt);
        ObjectDisposedException.ThrowIf(m_disposed, this);
        m_events.Emit(evnt);
    }

    /// <summary>
    /// Disconnects all session backends and rejects subsequent event delivery.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
            return;
        m_disposed = true;
        m_inputHub.Dispose();
        Sdl3InputBackend[] snapshot = m_backends.ToArray();
        m_backends.Clear();
        foreach (Sdl3InputBackend backend in snapshot)
            backend.DisconnectSource();
    }

    private void RemoveBackend(Sdl3InputBackend backend) => m_backends.Remove(backend);
}
