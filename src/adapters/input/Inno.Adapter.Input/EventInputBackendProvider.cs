using System;
using Inno.Platform;

namespace Inno.Adapter.Input;

/// <summary>
/// Supplies the common event implementation through the neutral input creation boundary.
/// </summary>
public sealed class EventInputBackendProvider : InputBackendProvider
{
    /// <summary>
    /// Creates an explicitly composed registration for the bundled implementation.
    /// </summary>
    public EventInputBackendProvider() : base(InputBackendId.events) { }

    /// <inheritdoc />
    public override IInputEventSource CreateEventSource(
        IPlatformWindow window,
        bool acceptAllWindows
    ) {
        ArgumentNullException.ThrowIfNull(window);
        return new EventInputSource(acceptAllWindows ? 0 : window.windowId);
    }
}

