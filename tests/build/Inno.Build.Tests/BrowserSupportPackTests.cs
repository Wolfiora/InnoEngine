using System;
using System.IO;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
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
    public async Task BrowserPackRequiresItsStaticGameLinkerClosure()
    {
        ProductNativeBuildPlan plan = StandardNativeBuildPlans.CreateStaticPlayer(Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration.nativeOptions);
        var validator = new Inno.Build.Browser.BrowserSupportPackValidator(plan);
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
        foreach (ProductNativeBuildStep step in plan.steps)
            foreach (string archive in step.component.staticBuild!.archiveNames)
                Write(pack, $"PlayerLink/Native/{step.id}/browser-wasm/{archive}");
        var catalog = new PlayerSupportPackCatalog(m_root);

        Write(pack, "PlayerLink/Native/foreign.dll");
        InvalidDataException foreignBinary = Assert.Throws<InvalidDataException>(
            () => validator.Validate(pack));
        Assert.Contains("foreign.dll", foreignBinary.Message, StringComparison.Ordinal);
        File.Delete(Path.Combine(pack, "PlayerLink", "Native", "foreign.dll"));
        File.Delete(Path.Combine(pack, "PlayerLink", "HttpPlayerContentSource.cs"));
        InvalidDataException missingSource = Assert.Throws<InvalidDataException>(
            () => validator.Validate(pack));
        Assert.Contains("HttpPlayerContentSource.cs", missingSource.Message, StringComparison.Ordinal);
        Write(pack, "PlayerLink/HttpPlayerContentSource.cs");
        Write(pack, "PlayerLink/Native/extra.a");
        Assert.Throws<InvalidDataException>(() => validator.Validate(pack));
        File.Delete(Path.Combine(pack, "PlayerLink", "Native", "extra.a"));
        pack = await catalog.PublishAsync(BuildTargetId.browserWasm, pack, validator);
        Assert.False(File.Exists(Path.Combine(pack, "PlayerLink", "References", "BGCS.Runtime.dll")));
        Assert.Equal(pack, catalog.Resolve(BuildTargetId.browserWasm, validator));

        File.Delete(Path.Combine(pack, "PlayerLink", "Native", "sdl3", "browser-wasm", "libSDL3.a"));
        InvalidDataException missingArchive = Assert.Throws<InvalidDataException>(
            () => catalog.Resolve(BuildTargetId.browserWasm, validator));
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
