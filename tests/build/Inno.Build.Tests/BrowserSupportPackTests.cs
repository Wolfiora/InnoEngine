using System;
using System.IO;
using System.Threading.Tasks;
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
    public async Task BrowserPackRequiresItsGameLinkerAndBindingRuntime()
    {
        string pack = Path.Combine(m_root, "candidate");
        Write(pack, "References/Inno.Runtime.dll");
        Write(pack, "PlayerLink/Player.csproj");
        Write(pack, "PlayerLink/Program.cs");
        Write(pack, "PlayerLink/BrowserPlayerComposition.cs");
        Write(pack, "PlayerLink/HttpPlayerContentSource.cs");
        Write(pack, "PlayerLink/BrowserBridge.cs");
        Write(pack, "PlayerLink/global.json");
        Write(pack, "PlayerLink/Analyzers/Inno.Runtime.Generators.dll");
        Write(pack, "PlayerLink/Analyzers/Inno.Core.Serialization.Generators.dll");
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

        await Assert.ThrowsAsync<InvalidDataException>(() => catalog.PublishAsync(
            BuildTargetId.browserWasm, pack, new Inno.Build.Platform.Browser.BrowserSupportPackValidator()).AsTask());

        Write(pack, "PlayerLink/References/BGCS.Runtime.dll");
        File.Delete(Path.Combine(pack, "PlayerLink", "HttpPlayerContentSource.cs"));
        InvalidDataException missingSource = Assert.Throws<InvalidDataException>(
            () => new Inno.Build.Platform.Browser.BrowserSupportPackValidator().Validate(pack));
        Assert.Contains("HttpPlayerContentSource.cs", missingSource.Message, StringComparison.Ordinal);
        Write(pack, "PlayerLink/HttpPlayerContentSource.cs");
        pack = await catalog.PublishAsync(BuildTargetId.browserWasm, pack,
            new Inno.Build.Platform.Browser.BrowserSupportPackValidator());
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
