using System;

namespace Inno.Adapter.Platform;

/// <summary>
/// Supplies borrowed native presentation handles to cooperating adapters.
/// </summary>
/// <remarks>
/// Window implementations opt into this SPI when they support native rendering integration.
/// Shared platform, Shell, and Player contracts do not expose these ABI details.
/// </remarks>
public interface INativeWindowSurface
{
    /// <summary>
    /// Gets handles owned by the live window; consumers must neither free nor retain them beyond its lifetime.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// The owning window has been released.
    /// </exception>
    PlatformNativeHandles nativeHandles { get; }
}
