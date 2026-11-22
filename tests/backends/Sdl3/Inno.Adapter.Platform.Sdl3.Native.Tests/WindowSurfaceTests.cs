using System;
using Xunit;
using Inno.Adapter.Platform;
using Inno.Integration.WindowsX64.Sdl3;
using Inno.Platform;

namespace Inno.Adapter.Platform.Sdl3.Native.Tests;

public sealed class WindowSurfaceTests
{
    [Fact]
    public void OwnedAndBorrowedWrappersUseTheSameHostSurfaceResolution()
    {
        if (NativeWindowTestHost.RunIfRequired(
            nameof(WindowSurfaceTests) + "." + nameof(OwnedAndBorrowedWrappersUseTheSameHostSurfaceResolution)
        )) {
            return;
        }

        ISdl3HostIntegration host = NativeHostFixture.Create();
        using var app = new Sdl3PlatformApplication(host);
        using Sdl3PlatformWindow window = app.CreateWindow(new PlatformWindowOptions
        {
            title = "Surface contract",
            width = 40,
            height = 40,
            visible = false
        });
        Assert.Equal(host.ResolveNativeSurface(window.sdlWindowHandle), window.nativeHandles);
        Assert.Equal(
            OperatingSystem.IsWindows() ? PlatformNativeHandleID.win32 : PlatformNativeHandleID.cocoa,
            window.nativeHandles.handleKind
        );
        Assert.True(window.pixelWidth > 0);
        Assert.True(window.pixelHeight > 0);
    }
}
