using System;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Platform;
using Xunit;

namespace Inno.Input.Tests;

public sealed class NativeWindowSurfaceTests
{
    [Fact]
    public void NativeSurfaceIdentifiersAreOpenAndRejectWhitespace()
    {
        var thirdParty = new PlatformNativeHandleId("tests.surface.custom");
        Assert.True(thirdParty.isValid);
        Assert.Equal(new PlatformNativeHandleId("tests.surface.custom"), thirdParty);
        Assert.NotEqual(new PlatformNativeHandleId("tests.surface.Custom"), thirdParty);
        Assert.False(default(PlatformNativeHandleId).isValid);
        Assert.Throws<ArgumentException>(() => new PlatformNativeHandleId(" "));
        Assert.Throws<ArgumentException>(() => new PlatformNativeHandleId("tests. surface"));
    }

    [Fact]
    public void DisposedWindowRejectsBorrowingItsNativeSurface()
    {
        if (!OperatingSystem.IsWindows())
            return;
        using var application = new Sdl3PlatformApplication(new Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration());
        using Sdl3PlatformWindow window = application.CreateWindow(new PlatformWindowOptions
        {
            title = "Native surface lifetime test",
            width = 32,
            height = 32,
            visible = false
        });
        INativeWindowSurface surface = window;
        Assert.Equal(PlatformNativeHandleId.win32, surface.nativeHandles.handleKind);
        Assert.NotEqual(IntPtr.Zero, surface.nativeHandles.windowHandle);
        window.Dispose();
        Assert.Throws<ObjectDisposedException>(() => surface.nativeHandles);
    }
}
