using Inno.Integration.Windows.Sdl3;
using Inno.Adapter.Platform;
using Inno.Platform;
using Xunit;

namespace Inno.Adapter.Platform.Sdl3.Tests;

public sealed class WindowSurfaceTests
{
    [Fact]
    public void OwnedAndBorrowedWrappersUseTheSameHostSurfaceResolution()
    {
        var host = new WindowsSdl3HostIntegration();
        using var app = new Sdl3PlatformApplication(host);
        using var window = app.CreateWindow(new PlatformWindowOptions
        { title = "Surface contract", width = 40, height = 40, visible = false });
        Assert.Equal(host.ResolveNativeSurface(window.sdlWindowHandle), window.nativeHandles);
        Assert.Equal(PlatformNativeHandleId.win32, window.nativeHandles.handleKind);
        Assert.True(window.pixelWidth > 0);
        Assert.True(window.pixelHeight > 0);
    }
}
