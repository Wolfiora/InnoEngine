using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace Inno.Native.Sdl3.Tests;

public sealed class Sdl3InitTests
{
    private readonly ITestOutputHelper m_output;

    public Sdl3InitTests(ITestOutputHelper output)
    {
        m_output = output;
    }

    [Fact]
    public unsafe void GeneratedFlagWidthsMatchSdlHeaders()
    {
        Assert.Equal(8, sizeof(SDLWindowFlags));
        Assert.Equal(4, sizeof(SDLMouseButtonFlags));
        Assert.Equal(2, sizeof(SDLKeymod));
    }

    [Fact]
    public unsafe void InitAndQuitShouldSucceed()
    {
        bool initResult = SDL.Init(SDLInitFlags.Events);
        if (!initResult)
        {
            string message = Marshal.PtrToStringUTF8((nint)SDL.Raw.GetError()) ?? "SDL.Init returned false.";
            Assert.True(initResult, message);
        }

        string? platform = Marshal.PtrToStringUTF8((nint)SDL.Raw.GetPlatform());
        m_output.WriteLine($"SDL.GetPlatform: {platform}");
        Assert.False(string.IsNullOrWhiteSpace(platform));

        int version = SDL.GetVersion();
        int major = version / 1_000_000;
        int minor = (version / 1000) % 1000;
        int patch = version % 1000;
        m_output.WriteLine($"SDL.GetVersion: {major}.{minor}.{patch} ({version})");
        Assert.True(version > 0);

        SDL.Quit();
    }
}
