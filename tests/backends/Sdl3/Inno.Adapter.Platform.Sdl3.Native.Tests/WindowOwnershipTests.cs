using System;
using System.Threading.Tasks;
using Xunit;
using Inno.Integration.WindowsX64.Sdl3;
using Inno.Native.Sdl3;
using Inno.Platform;

namespace Inno.Adapter.Platform.Sdl3.Native.Tests;

public sealed unsafe class WindowOwnershipTests
{
    [Fact]
    public void ApplicationDirectoryContainsOnlyCreatedOrAdoptedWindows()
    {
        if (NativeWindowTestHost.RunIfRequired(
            nameof(WindowOwnershipTests) + "." + nameof(ApplicationDirectoryContainsOnlyCreatedOrAdoptedWindows)
        )) {
            return;
        }

        using var first = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var second = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using Sdl3PlatformWindow window = first.CreateWindow(new PlatformWindowOptions
        {
            title = "Owned surface",
            width = 40,
            height = 40,
            visible = false
        });
        Assert.Single(first.GetWindows());
        Assert.Empty(second.GetWindows());
        window.Dispose();
        Assert.Empty(first.GetWindows());
        Assert.True(SDL.Raw.GetWindowFromId(window.windowID) is null);
    }

    [Fact]
    public void AdoptedWindowIsInvalidatedBeforeItsOriginalOwnerDestroysIt()
    {
        if (NativeWindowTestHost.RunIfRequired(
            nameof(WindowOwnershipTests) + "." + nameof(AdoptedWindowIsInvalidatedBeforeItsOriginalOwnerDestroysIt)
        )) {
            return;
        }

        using var app = new Sdl3PlatformApplication(NativeHostFixture.Create());
        ReadOnlySpan<byte> title = "Borrowed surface\0"u8;
        SDLWindow* native;
        fixed (byte* bytes = title)
        {
            native = SDL.Raw.CreateWindow((sbyte*)bytes, 40, 40, (ulong)SDLWindowFlags.Hidden);
        }
        Assert.True(native is not null);
        uint id = SDL.Raw.GetWindowId(native);
        try
        {
            Sdl3PlatformWindow borrowed = app.AdoptWindow((nint)native);
            Assert.Single(app.GetWindows());
            Assert.Throws<ArgumentException>(() => app.AdoptWindow((nint)native));
            app.ReleaseWindow(borrowed);
            Assert.Empty(app.GetWindows());
            Assert.True(borrowed.isClosed);
            Assert.Equal(0, borrowed.sdlWindowHandle);
            Assert.Throws<ObjectDisposedException>(() => borrowed.nativeHandles);
            Assert.True(SDL.Raw.GetWindowFromId(id) is not null);
            borrowed.Dispose();
            Assert.True(SDL.Raw.GetWindowFromId(id) is not null);
        }
        finally
        {
            SDL.Raw.DestroyWindow(native);
        }
        Assert.True(SDL.Raw.GetWindowFromId(id) is null);
    }

    [Fact]
    public void CrossApplicationReleaseAndCrossThreadCreationAreRejected()
    {
        if (NativeWindowTestHost.RunIfRequired(
            nameof(WindowOwnershipTests) + "." + nameof(CrossApplicationReleaseAndCrossThreadCreationAreRejected)
        )) {
            return;
        }

        using var first = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var second = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using Sdl3PlatformWindow window = first.CreateWindow(new PlatformWindowOptions
        {
            title = "Thread owner",
            width = 40,
            height = 40,
            visible = false
        });
        Assert.Throws<ArgumentException>(() => second.ReleaseWindow(window));
        Assert.Throws<InvalidOperationException>(
            () => Task.Run(() => first.CreateWindow(new PlatformWindowOptions())).GetAwaiter().GetResult()
        );
        Assert.Single(first.GetWindows());
    }
}
