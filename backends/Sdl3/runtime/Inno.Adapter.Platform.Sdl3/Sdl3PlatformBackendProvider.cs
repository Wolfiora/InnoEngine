using System;
using Inno.Platform;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Supplies the Sdl3 implementation through the neutral platform creation boundary.
/// </summary>
public sealed class Sdl3PlatformBackendProvider : PlatformBackendProvider
{
    private readonly ISdl3HostIntegration m_hostIntegration;

    /// <summary>
    /// Creates an explicitly composed registration for the bundled implementation.
    /// </summary>
    /// <param name="hostIntegration">
    /// The borrowed immutable system integration required by every created application.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Host integration is absent.
    /// </exception>
    public Sdl3PlatformBackendProvider(ISdl3HostIntegration hostIntegration) : base(PlatformBackendId.sdl3)
    {
        ArgumentNullException.ThrowIfNull(hostIntegration);
        m_hostIntegration = hostIntegration;
    }

    /// <inheritdoc />
    public override IPlatformApplication CreateApplication()
    {
        return new Sdl3PlatformApplication(m_hostIntegration);
    }
}

