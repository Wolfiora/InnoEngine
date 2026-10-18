using System;
using System.IO;
using System.Linq;
using System.Text;
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
        ProductNativeBuildPlan plan = StandardNativeBuildPlans.CreateStaticPlayer(Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxIntegration.nativeOptions);
        IPlayerSupportPackValidator validator = StandardBuildDistribution.Create(
            StandardBuildEnvironment.Capture(AppContext.BaseDirectory, BuildTargetId.windowsX64)).build.platforms
            .Single(static platform => platform.descriptor.id == BuildTargetId.browserWasm).supportPackSource;
        string pack = Path.Combine(m_root, "candidate");
        Write(pack, "References/Inno.Runtime.dll");
        Write(pack, "PlayerLink/Player.csproj");
        Write(pack, "PlayerLink/Program.cs");
        Write(pack, "PlayerLink/BrowserWasm32PlayerComposition.cs");
        Write(pack, "PlayerLink/HttpPlayerContentSource.cs");
        Write(pack, "PlayerLink/BrowserWasm32Bridge.cs");
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
        {
            Write(pack, "PlayerLink/References/" + binding);
            Write(pack, "References/" + binding);
        }
        Write(pack, "PlayerLink/References/BGCS.Runtime.dll");
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
        using PlayerSupportPackLease published = await catalog.PublishAsync(BuildTargetId.browserWasm, pack, validator);
        pack = published.directory;
        Assert.True(File.Exists(Path.Combine(pack, "PlayerLink", "References", "BGCS.Runtime.dll")));
        using PlayerSupportPackLease selected = catalog.Acquire(BuildTargetId.browserWasm, validator);
        Assert.Equal(pack, selected.directory);

        File.Delete(Path.Combine(pack, "PlayerLink", "Native", "sdl3", "browser-wasm", "libSDL3.a"));
        InvalidDataException missingArchive = Assert.Throws<InvalidDataException>(
            () => catalog.Acquire(BuildTargetId.browserWasm, validator));
        Assert.Contains("libSDL3.a", missingArchive.Message, StringComparison.Ordinal);
    }

    private static void Write(
        string pack,
        string relativePath
    ) {
        string path = Path.Combine(pack, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        if (relativePath.EndsWith(".a", StringComparison.Ordinal))
        {
            string header = "fixture.o/".PadRight(16) + "0".PadRight(12) + "0".PadRight(6)
                + "0".PadRight(6) + "100644".PadRight(8) + "8".PadRight(10) + "`\n";
            File.WriteAllBytes(path, [.. Encoding.ASCII.GetBytes("!<arch>\n" + header), .. "\0asm\u0001\0\0\0"u8]);
        }
        else
            File.WriteAllText(path, "fixture");
    }
}
