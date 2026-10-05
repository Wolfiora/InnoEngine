using System;
using Inno.Platform;

namespace Inno.Adapter.Input.Sdl3;

/// <summary>
/// Supplies the Sdl3 implementation through the neutral input creation boundary.
/// </summary>
public sealed class Sdl3InputBackendProvider : InputBackendProvider
{
    /// <summary>
    /// Creates an explicitly composed registration for the bundled implementation.
    /// </summary>
    public Sdl3InputBackendProvider() : base(InputBackendId.sdl3) { }

    /// <inheritdoc />
    public override IInputEventSource CreateEventSource(
        IPlatformWindow window,
        bool acceptAllWindows
    ) {
        ArgumentNullException.ThrowIfNull(window);
        return new Sdl3InputSource(acceptAllWindows ? 0 : window.windowId);
    }
}

