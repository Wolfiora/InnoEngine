using System;
using System.IO;
using System.Text;
using Xunit;

namespace Inno.Build.BrowserWasm32.Integration.Tests;

public sealed class BrowserSupportPackAbiTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoBrowserPack", Guid.NewGuid().ToString("N"));

    [Fact]
    public void BrowserPlatformChecksObjectAbiWithoutKnowingTheSelectedBackend()
    {
        foreach (string input in new[]
        {
            "Player.csproj", "Program.cs", "BrowserWasm32PlayerComposition.cs", "HttpPlayerContentSource.cs",
            "BrowserWasm32Bridge.cs", "global.json", "Analyzers/Inno.Runtime.Generators.dll",
            "Analyzers/Inno.Core.Serialization.Generators.dll", "wwwroot/index.html", "wwwroot/main.js",
            "Native/wasm_sjlj_shim.c", "References/Inno.Player.Runtime.dll"
        }) {
            Write("PlayerLink/" + input);
        }
        string archive = Path.Combine(m_root, "PlayerLink", "Native", "custom.a");
        string header = "object.o/".PadRight(16) + "0".PadRight(12) + "0".PadRight(6)
                        + "0".PadRight(6) + "644".PadRight(8) + "8".PadRight(10) + "`\n";
        byte[] bytes = [.. Encoding.ASCII.GetBytes("!<arch>\n" + header), 0, 97, 115, 109, 1, 0, 0, 0];
        File.WriteAllBytes(archive, bytes);
        var validator = new BrowserWasm32SupportPackValidator();
        validator.Validate(m_root);
        bytes[^8] = 0x7F;
        File.WriteAllBytes(archive, bytes);
        Assert.Contains("ABI", Assert.Throws<InvalidDataException>(() => validator.Validate(m_root)).Message);
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root)) Directory.Delete(m_root, recursive: true);
    }

    private void Write(string relative)
    {
        string path = Path.Combine(m_root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "fixture");
    }
}
