using System;
using Inno.Adapter.Platform.Sdl3;
using Inno.Integration.WindowsX64.Sdl3;
using Inno.Integration.MacOSArm64.Sdl3;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

internal static class NativeHostFixture
{
    internal static ISdl3HostIntegration Create() => OperatingSystem.IsWindows()
        ? new WindowsX64Sdl3HostIntegration()
        : OperatingSystem.IsMacOS()
            ? new MacOSArm64Sdl3HostIntegration()
            : throw new PlatformNotSupportedException("Native window contracts require an implemented desktop host.");
}
