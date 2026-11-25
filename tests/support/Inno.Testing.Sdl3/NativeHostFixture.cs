using System;
using Inno.Adapter.Platform.Sdl3;
using Inno.Integration.MacOSArm64.Sdl3;
using Inno.Integration.WindowsX64.Sdl3;

namespace Inno.Testing.Sdl3;

/// <summary>
/// Selects the SDL3 host integration of the desktop that runs the native window contracts.
/// </summary>
public static class NativeHostFixture
{
    /// <summary>
    /// Creates the host integration of the current desktop.
    /// </summary>
    /// <returns>
    /// A new integration owned by the caller.
    /// </returns>
    /// <exception cref="PlatformNotSupportedException">
    /// The current operating system has no implemented desktop host.
    /// </exception>
    public static ISdl3HostIntegration Create()
        => OperatingSystem.IsWindows()
           ? new WindowsX64Sdl3HostIntegration()
           : OperatingSystem.IsMacOS()
             ? new MacOSArm64Sdl3HostIntegration()
             : throw new PlatformNotSupportedException("Native window contracts require an implemented desktop host.");
}
