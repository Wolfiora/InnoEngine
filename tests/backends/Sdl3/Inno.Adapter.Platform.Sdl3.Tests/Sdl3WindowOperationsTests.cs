using System;
using System.Runtime.InteropServices;
using Inno.Integration.WindowsX64.Sdl3;
using Inno.Native.Sdl3;
using Inno.Platform;
using Xunit;

namespace Inno.Adapter.Platform.Sdl3.Tests;

public sealed unsafe class Sdl3WindowOperationsTests
{
    [Theory]
    [InlineData(false, false)]
    [InlineData(true, true)]
    public void CreationPreservesOptionsAndLeavesRegistrationToTheOwner(
        bool resizable,
        bool highPixelDensity
    ) {
        if (NativeWindowTestHost.RunIfRequired(nameof(Sdl3WindowOperationsTests) + "." + nameof(CreationPreservesOptionsAndLeavesRegistrationToTheOwner) + "." + resizable + "." + highPixelDensity))
            return;

        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        var options = new PlatformWindowOptions
        {
            title = "Shared SDL creation",
            width = 83,
            height = 47,
            visible = false,
            resizable = resizable,
            highPixelDensity = highPixelDensity
        };
        SDLWindow* native = (SDLWindow*)Sdl3WindowOperations.CreateWindow(options, requireOpenGl: false);
        uint id = SDL.Raw.GetWindowId(native);
        try
        {
            Assert.Empty(application.GetWindows());
            Assert.Equal(options.title, Marshal.PtrToStringUTF8((nint)SDL.Raw.GetWindowTitle(native)));
            int width;
            int height;
            Assert.NotEqual((byte)0, SDL.Raw.GetWindowSize(native, &width, &height));
            Assert.Equal(options.width, width);
            Assert.Equal(options.height, height);
            SDLWindowFlags flags = (SDLWindowFlags)SDL.Raw.GetWindowFlags(native);
            Assert.True((flags & SDLWindowFlags.Hidden) != 0);
            Assert.Equal(resizable, (flags & SDLWindowFlags.Resizable) != 0);
            Assert.Equal(highPixelDensity, (flags & SDLWindowFlags.HighPixelDensity) != 0);
            Assert.Equal((flags & SDLWindowFlags.InputFocus) != 0,
                Sdl3WindowOperations.ReadFocus((nint)native));
        }
        finally
        {
            SDL.Raw.DestroyWindow(native);
        }
        Assert.True(SDL.Raw.GetWindowFromId(id) == null);
    }

    [Fact]
    public void InvalidInputFailsBeforeCreatingAWindow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Sdl3WindowOperations.CreateWindow(
            new PlatformWindowOptions { width = 0 }, requireOpenGl: false));
        Assert.Throws<ArgumentException>(() => Sdl3WindowOperations.ReadFocus(0));
    }
}
