using System;
using Inno.Platform;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Supplies the Sdl3 implementation through the neutral platform creation boundary.
/// </summary>
public sealed class Sdl3PlatformBackendProvider : PlatformBackendProvider
{
    /// <summary>
    /// Creates an explicitly composed registration for the bundled implementation.
    /// </summary>
    public Sdl3PlatformBackendProvider() : base(PlatformBackendId.sdl3) { }

    /// <inheritdoc />
    public override IPlatformApplication CreateApplication()
    {
        return new Sdl3PlatformApplication();
    }
}

