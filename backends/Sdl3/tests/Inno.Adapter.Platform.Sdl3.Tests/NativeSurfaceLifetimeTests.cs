using System;
using Inno.Adapter.Platform;
using Inno.Platform;
using Xunit;

namespace Inno.Adapter.Platform.Sdl3.Tests;

public sealed class NativeSurfaceLifetimeTests
{
    [Fact]
    public void DisposedWindowRejectsBorrowingItsNativeSurface()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(NativeSurfaceLifetimeTests) + "." + nameof(DisposedWindowRejectsBorrowingItsNativeSurface)))
            return;
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using Sdl3PlatformWindow window = application.CreateWindow(new PlatformWindowOptions
        {
            title = "Native surface lifetime test",
            width = 32,
            height = 32,
            visible = false
        });
        INativeWindowSurface surface = window;
        Assert.Equal(OperatingSystem.IsWindows() ? PlatformNativeHandleId.win32 : PlatformNativeHandleId.cocoa, surface.nativeHandles.handleKind);
        Assert.NotEqual(IntPtr.Zero, surface.nativeHandles.windowHandle);
        window.Dispose();
        Assert.Throws<ObjectDisposedException>(() => surface.nativeHandles);
    }
}
