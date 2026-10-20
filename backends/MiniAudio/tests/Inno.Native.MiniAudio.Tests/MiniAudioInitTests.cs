using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace Inno.Native.MiniAudio.Tests;

public sealed class MiniAudioInitTests
{
    private readonly ITestOutputHelper m_output;

    public MiniAudioInitTests(ITestOutputHelper output)
    {
        m_output = output;
    }

    [Fact]
    public unsafe void VersionLoadsTheCompleteNativeFunctionTable()
    {
        uint major = 0;
        uint minor = 0;
        uint revision = 0;

        MiniAudio.Raw.Version(&major, &minor, &revision);
        string? version = Marshal.PtrToStringUTF8((nint)MiniAudio.Raw.VersionString());

        m_output.WriteLine($"miniaudio version: {version}");
        Assert.Equal((uint)0, major);
        Assert.Equal((uint)11, minor);
        Assert.Equal((uint)25, revision);
        Assert.Equal("0.11.25", version);
    }

    [Fact]
    public unsafe void EngineInitializesWithoutAnAudioDevice()
    {
        MaEngineConfig config = MiniAudio.EngineConfigInit();
        config.NoDevice = 1;
        config.Channels = 2;
        config.SampleRate = 48_000;
        MaEngine engine = default;

        MaResult result = MiniAudio.Raw.EngineInit(&config, &engine);
        Assert.Equal(MaResult.MA_SUCCESS, result);

        MiniAudio.Raw.EngineUninit(&engine);
    }
}
