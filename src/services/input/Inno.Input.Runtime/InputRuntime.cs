using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Events;
using Inno.Core.Identity;
using Inno.Runtime;
using Inno.Runtime.Contracts;

namespace Inno.Input.Runtime;

/// <summary>
/// Captures one immutable input snapshot and binds it for the complete runtime frame.
/// </summary>
public sealed partial class InputRuntime : RuntimeSubsystem, IInputService, IInputDeviceSource
{
    private readonly IInputBackend m_backend;

    /// <summary>
    /// Creates a feature whose backend ownership transfers to this instance.
    /// </summary>
    /// <param name="backend">
    /// The platform backend captured at frame boundaries.
    /// </param>
    /// <param name="identities">
    /// The session allocator for live device connections.
    /// </param>
    /// <param name="events">
    /// The dispatcher receiving committed transitions in this session.
    /// </param>
    public InputRuntime(
        IInputBackend backend,
        IdentityAllocator identities,
        EventDispatcher events
    ) {
        ArgumentNullException.ThrowIfNull(backend);
        ArgumentNullException.ThrowIfNull(identities);
        ArgumentNullException.ThrowIfNull(events);
        m_backend = backend;
        m_identities = identities;
        m_events = events;
    }

    /// <summary>
    /// Gets the immutable input state captured at the beginning of the current frame.
    /// </summary>
    public InputSnapshot snapshot { get; private set; } = InputSnapshot.empty;

    /// <summary>
    /// Captures pending backend input and binds the resulting snapshot to the runtime frame.
    /// </summary>
    /// <param name="frame">
    /// The immutable frame state whose index labels the captured snapshot.
    /// </param>
    protected override void OnBeginFrame(RuntimeFrame frame)
    {
        snapshot = m_backend.Capture(frame.frameIndex);
        if (m_connections.Count != 0 || m_changes.Count != 0)
        {
            snapshot = snapshot.WithDevices(m_connections.Select(static device => device.Capture()), m_changes);
            m_changes.Clear();
        }
        OwnFrameScope(InputExecutionContext.EnterScope(this));
    }

    /// <summary>
    /// Releases the input backend after its frame scope and owned work have quiesced.
    /// </summary>
    protected override void OnStop()
    {
        m_accepting = false;
        List<Exception>? failures = null;
        foreach (Connection device in m_connections.ToArray())
        {
            try
            {
                device.Dispose();
            }
            catch (Exception exception)
            {
                (failures ??= []).Add(exception);
            }
        }
        try
        {
            m_backend.Dispose();
        }
        catch (Exception exception)
        {
            (failures ??= []).Add(exception);
        }
        m_changes.Clear();
        if (failures is not null) throw new AggregateException("Input retirement failed.", failures);
    }
}
