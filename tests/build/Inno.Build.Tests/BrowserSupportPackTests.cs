using System;
using System.IO;
using Inno.Build;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BrowserSupportPackTests : IDisposable
{
    private readonly string m_root = Path.Combine(
        Path.GetTempPath(), "InnoBrowserPackTests", Guid.NewGuid().ToString("N"));

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    [Fact]
    public void BrowserPackRequiresItsGameLinkerAndBindingRuntime()
    {
        string pack = Path.Combine(m_root, BuildTargetId.browserWasm.value);
        Write(pack, "References/Inno.Runtime.dll");
        Write(pack, "PlayerLink/BrowserPlayer.csproj");
        Write(pack, "PlayerLink/Program.cs");
        Write(pack, "PlayerLink/References/Inno.Player.Runtime.dll");
        Write(pack, "PlayerLink/wwwroot/index.html");
        Write(pack, "PlayerLink/wwwroot/main.js");
        Write(pack, "PlayerLink/Native/wasm_sjlj_shim.c");
        foreach (string binding in new[]
        {
            "Inno.Native.Bgfx.dll", "Inno.Native.MiniAudio.dll",
            "Inno.Native.Sdl3.dll", "Inno.Native.Text.dll", "Inno.Native.UI.dll"
        })
            Write(pack, "PlayerLink/References/" + binding);
        foreach (string archive in new[]
        {
            "bgfxRelease.a", "bxRelease.a", "bimgRelease.a", "bimg_decodeRelease.a",
            "libSDL3.a", "libminiaudio.a", "libinno-text.a", "libinno-ui.a",
            "librmlui.a", "libfreetype.a", "libharfbuzz.a"
        })
            Write(pack, "PlayerLink/Native/" + archive);
        var catalog = new PlayerSupportPackCatalog(m_root);

        Assert.Throws<InvalidDataException>(() => catalog.Resolve(BuildTargetId.browserWasm, new Inno.Build.Platform.Browser.BrowserSupportPackValidator()));

        Write(pack, "PlayerLink/References/BGCS.Runtime.dll");
        Assert.Equal(pack, catalog.Resolve(BuildTargetId.browserWasm, new Inno.Build.Platform.Browser.BrowserSupportPackValidator()));

        File.Delete(Path.Combine(pack, "PlayerLink", "Native", "libSDL3.a"));
        InvalidDataException missingArchive = Assert.Throws<InvalidDataException>(
            () => catalog.Resolve(BuildTargetId.browserWasm, new Inno.Build.Platform.Browser.BrowserSupportPackValidator()));
        Assert.Contains("libSDL3.a", missingArchive.Message, StringComparison.Ordinal);
    }

    private static void Write(
        string pack,
        string relativePath
    ) {
        string path = Path.Combine(pack, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "fixture");
    }
}
