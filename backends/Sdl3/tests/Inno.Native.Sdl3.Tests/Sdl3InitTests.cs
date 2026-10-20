using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace Inno.Native.Sdl3.Tests;

public sealed class Sdl3InitTests
{
    [Fact]
    public unsafe void GeneratedFlagWidthsMatchSdlHeaders()
    {
        Assert.Equal(8, sizeof(SDLWindowFlags));
        Assert.Equal(4, sizeof(SDLMouseButtonFlags));
        Assert.Equal(2, sizeof(SDLKeymod));
    }

    private readonly ITestOutputHelper output;

    public Sdl3InitTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public unsafe void InitAndQuit_ShouldSucceed()
    {
        var initResult = SDL.Init(SDLInitFlags.Events);
        if (!initResult)
        {
            string message = Marshal.PtrToStringUTF8((nint)SDL.Raw.GetError()) ?? "SDL.Init returned false.";
            Assert.True(initResult, message);
        }

        string? platform = Marshal.PtrToStringUTF8((nint)SDL.Raw.GetPlatform());
        output.WriteLine($"SDL.GetPlatform: {platform}");
        Assert.False(string.IsNullOrWhiteSpace(platform));

        var version = SDL.GetVersion();
        var major = version / 1000000;
        var minor = (version / 1000) % 1000;
        var patch = version % 1000;
        output.WriteLine($"SDL.GetVersion: {major}.{minor}.{patch} ({version})");
        Assert.True(version > 0);

        SDL.Quit();
    }
}
