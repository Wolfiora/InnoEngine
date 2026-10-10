using System;
using Inno.Integration.Windows.Sdl3;
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
        using var application = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        var options = new PlatformWindowOptions
        {
            title = "Shared SDL creation",
            width = 83,
            height = 47,
            visible = false,
            resizable = resizable,
            highPixelDensity = highPixelDensity
        };
        SDLWindow native = new(Sdl3WindowOperations.CreateWindow(options, requireOpenGl: false));
        uint id = SDL.GetWindowID(native);
        try
        {
            Assert.Empty(application.GetWindows());
            Assert.Equal(options.title, SDL.GetWindowTitle(native));
            int width;
            int height;
            Assert.NotEqual((byte)0, SDL.GetWindowSize(native, &width, &height));
            Assert.Equal(options.width, width);
            Assert.Equal(options.height, height);
            SDLWindowFlags flags = SDL.GetWindowFlags(native);
            Assert.True((flags & SDLWindowFlags.Hidden) != 0);
            Assert.Equal(resizable, (flags & SDLWindowFlags.Resizable) != 0);
            Assert.Equal(highPixelDensity, (flags & SDLWindowFlags.HighPixelDensity) != 0);
            Assert.Equal((flags & SDLWindowFlags.InputFocus) != 0,
                Sdl3WindowOperations.ReadFocus((nint)native.Handle));
        }
        finally
        {
            SDL.DestroyWindow(native);
        }
        Assert.True(SDL.GetWindowFromID(id).IsNull);
    }

    [Fact]
    public void InvalidInputFailsBeforeCreatingAWindow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Sdl3WindowOperations.CreateWindow(
            new PlatformWindowOptions { width = 0 }, requireOpenGl: false));
        Assert.Throws<ArgumentException>(() => Sdl3WindowOperations.ReadFocus(0));
    }
}
