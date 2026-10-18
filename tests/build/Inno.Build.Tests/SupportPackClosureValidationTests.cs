using System;
using System.IO;
using System.Text;
using Inno.Build.SupportPacks;
using Xunit;

namespace Inno.Build.Tests;

public sealed class SupportPackClosureValidationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoPackClosureTests", Guid.NewGuid().ToString("N"));

    [Fact]
    public void ACustomBackendUsesItsDeclaredGroupWithoutStandardLibraryNames()
    {
        Write("PlayerLink/References/Custom.Runtime.dll");
        Write("native/custom/fixture-target/arbitrary-name.fixture");
        var requirements = new PlayerSupportPackRequirements(new("fixture-target"),
            ["PlayerLink/References/Custom.Runtime.dll"], "native", ["custom/fixture-target"], ".fixture");
        var validator = new PlayerSupportPackClosureValidator(requirements, new LayoutValidator(), null);
        validator.Validate(m_root);
        Write("native/unselected/fixture-target/another.fixture");
        Assert.Throws<InvalidDataException>(() => validator.Validate(m_root));
    }

    [Fact]
    public void MissingDeclaredArchiveFailsEvenWhenItsGroupContainsAnotherArchive()
    {
        Write("PlayerLink/Native/custom/fixture-target/other.a");
        var requirements = new PlayerSupportPackRequirements(new("fixture-target"),
            ["PlayerLink/Native/custom/fixture-target/required.a"], "PlayerLink/Native", ["custom/fixture-target"], ".a");
        var validator = new PlayerSupportPackClosureValidator(requirements, new LayoutValidator(), null);
        InvalidDataException missing = Assert.Throws<InvalidDataException>(() => validator.Validate(m_root));
        Assert.Contains("required.a", missing.Message);
    }

    [Theory]
    [InlineData("custom/fixture-target/extra.a")]
    [InlineData("unselected/fixture-target/extra.a")]
    public void StaticClosureRejectsAnAdditionalArchive(string extra)
    {
        Write("PlayerLink/Native/custom/fixture-target/required.a");
        var requirements = new PlayerSupportPackRequirements(new("fixture-target"),
            ["PlayerLink/Native/custom/fixture-target/required.a"], "PlayerLink/Native",
            ["custom/fixture-target"], ".a", ["custom/fixture-target/required.a"]);
        var validator = new PlayerSupportPackClosureValidator(requirements, new LayoutValidator(), null);
        validator.Validate(m_root);
        Write("PlayerLink/Native/" + extra);
        Assert.Throws<InvalidDataException>(() => validator.Validate(m_root));
    }

    [Theory]
    [InlineData("../outside")]
    [InlineData("native/../outside")]
    [InlineData("C:/outside")]
    public void RequirementPathsCannotEscapeTheirOwner(string path)
        => Assert.Throws<ArgumentException>(() => new PlayerSupportPackRequirements(new("fixture-target"),
            [path], "native", ["custom"], ".fixture"));

    [Fact]
    public void BrowserPlatformChecksObjectAbiWithoutKnowingTheSelectedBackend()
    {
        foreach (string input in new[]
        {
            "Player.csproj", "Program.cs", "BrowserWasm32PlayerComposition.cs", "HttpPlayerContentSource.cs",
            "BrowserWasm32Bridge.cs", "global.json", "Analyzers/Inno.Runtime.Generators.dll",
            "Analyzers/Inno.Core.Serialization.Generators.dll", "wwwroot/index.html", "wwwroot/main.js",
            "Native/wasm_sjlj_shim.c", "References/Inno.Player.Runtime.dll"
        })
            Write("PlayerLink/" + input);
        string archive = Path.Combine(m_root, "PlayerLink", "Native", "custom.a");
        string header = "object.o/".PadRight(16) + "0".PadRight(12) + "0".PadRight(6)
            + "0".PadRight(6) + "644".PadRight(8) + "8".PadRight(10) + "`\n";
        byte[] bytes = [.. Encoding.ASCII.GetBytes("!<arch>\n" + header), 0, 97, 115, 109, 1, 0, 0, 0];
        File.WriteAllBytes(archive, bytes);
        var validator = new Inno.Build.BrowserWasm32.BrowserWasm32SupportPackValidator();
        validator.Validate(m_root);
        bytes[^8] = 0x7f;
        File.WriteAllBytes(archive, bytes);
        Assert.Contains("ABI", Assert.Throws<InvalidDataException>(() => validator.Validate(m_root)).Message);
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private void Write(string relative)
    {
        string path = Path.Combine(m_root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "fixture");
    }

    private sealed class LayoutValidator : IPlayerSupportPackValidator
    {
        public void Validate(string directory) => Assert.True(Directory.Exists(directory));
    }
}
