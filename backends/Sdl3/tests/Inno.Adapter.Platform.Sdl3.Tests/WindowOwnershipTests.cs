using System;
using System.Threading.Tasks;
using Inno.Integration.Windows.Sdl3;
using Inno.Native.Sdl3;
using Inno.Platform;
using Xunit;

namespace Inno.Adapter.Platform.Sdl3.Tests;

public sealed unsafe class WindowOwnershipTests
{
    [Fact]
    public void ApplicationDirectoryContainsOnlyCreatedOrAdoptedWindows()
    {
        using var first = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        using var second = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        using var window = first.CreateWindow(new PlatformWindowOptions
        { title = "Owned surface", width = 40, height = 40, visible = false });
        Assert.Single(first.GetWindows());
        Assert.Empty(second.GetWindows());
        window.Dispose();
        Assert.Empty(first.GetWindows());
        Assert.True(SDL.GetWindowFromID(window.windowId).IsNull);
    }

    [Fact]
    public void AdoptedWindowIsInvalidatedBeforeItsOriginalOwnerDestroysIt()
    {
        using var app = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        SDLWindow native = SDL.CreateWindow("Borrowed surface", 40, 40, SDLWindowFlags.Hidden);
        Assert.False(native.IsNull);
        uint id = SDL.GetWindowID(native);
        try
        {
            var borrowed = app.AdoptWindow((nint)native.Handle);
            Assert.Single(app.GetWindows());
            Assert.Throws<ArgumentException>(() => app.AdoptWindow((nint)native.Handle));
            app.ReleaseWindow(borrowed);
            Assert.Empty(app.GetWindows());
            Assert.True(borrowed.isClosed);
            Assert.Equal(0, borrowed.sdlWindowHandle);
            Assert.Throws<ObjectDisposedException>(() => borrowed.nativeHandles);
            Assert.False(SDL.GetWindowFromID(id).IsNull);
            borrowed.Dispose();
            Assert.False(SDL.GetWindowFromID(id).IsNull);
        }
        finally
        {
            SDL.DestroyWindow(native);
        }
        Assert.True(SDL.GetWindowFromID(id).IsNull);
    }

    [Fact]
    public void CrossApplicationReleaseAndCrossThreadCreationAreRejected()
    {
        using var first = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        using var second = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        using var window = first.CreateWindow(new PlatformWindowOptions
        { title = "Thread owner", width = 40, height = 40, visible = false });
        Assert.Throws<ArgumentException>(() => second.ReleaseWindow(window));
        Assert.Throws<InvalidOperationException>(() => Task.Run(() => first.CreateWindow(new PlatformWindowOptions())).GetAwaiter().GetResult());
        Assert.Single(first.GetWindows());
    }
}
