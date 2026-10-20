using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class NativeBindingCompilationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoBindingCompilation", Guid.NewGuid().ToString("N"));

    [Fact]
    public void RemappedInputsPreserveRootRelationshipsAndVerifyExactContent()
    {
        string native = Path.Combine(m_root, "published");
        Directory.CreateDirectory(native);
        NativeBindingCompilation compilation = Create();
        File.WriteAllText(Path.Combine(native, "compilation.json"), JsonSerializer.Serialize(compilation));
        compilation = NativeBindingCompilation.Load(native);
        string copied = Path.Combine(m_root, "frozen");
        Directory.CreateDirectory(Path.Combine(copied, "nested"));
        File.WriteAllText(Path.Combine(copied, "nested", "api.hpp"), "verified", new UTF8Encoding(false));
        string Resolve(string id) => id == "user" ? copied : id == "nested" ? Path.Combine(copied, "nested") : native;
        compilation.VerifyInputs(Resolve);
        Assert.Equal(Path.Combine(m_root, "source", "nested"), compilation.ResolveRoot(m_root, native, "nested"));
        Assert.Equal(new[] { "first", "second" }, compilation.defines);
        Assert.False(compilation.files[0].compile);
        Assert.True(compilation.files[1].compile);
        File.WriteAllText(Path.Combine(copied, "nested", "api.hpp"), "modified", new UTF8Encoding(false));
        Assert.Throws<InvalidDataException>(() => compilation.VerifyInputs(Resolve));
    }

    [Theory]
    [InlineData("../escape.hpp")]
    [InlineData("/rooted.hpp")]
    [InlineData("C:/rooted.hpp")]
    [InlineData("nested\\file.hpp")]
    public void UnsafeInputPathsAreRejectedWhenReadingTheDescription(string relative)
    {
        Directory.CreateDirectory(m_root);
        NativeBindingCompilation compilation = Create();
        compilation.files[0] = compilation.files[0] with { relativePath = relative };
        File.WriteAllText(Path.Combine(m_root, "compilation.json"), JsonSerializer.Serialize(compilation));
        Assert.Throws<InvalidDataException>(() => NativeBindingCompilation.Load(m_root));
    }

    [Fact]
    public void LengthMismatchAndCancellationFailBeforeAcceptingAnInput()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "nested"));
        File.WriteAllText(Path.Combine(m_root, "nested", "api.hpp"), "short", new UTF8Encoding(false));
        NativeBindingCompilation compilation = Create();
        Assert.Throws<InvalidDataException>(() => compilation.VerifyInputs(_ => m_root));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Throws<OperationCanceledException>(() => compilation.VerifyInputs(_ => m_root, cancellation.Token));
    }

    [Fact]
    public void RootIdentitiesBelongToEachDescription()
    {
        NativeBindingCompilation first = Create();
        NativeBindingCompilation second = Create();
        second.roots[0] = second.roots[0] with { directory = "other" };
        Assert.Equal(Path.Combine(m_root, "source"), first.ResolveRoot(m_root, m_root, "user"));
        Assert.Equal(Path.Combine(m_root, "other"), second.ResolveRoot(m_root, m_root, "user"));
        Assert.Throws<InvalidDataException>(() => second.ResolveRoot(m_root, m_root, "unknown"));
    }

    [Fact]
    public void MissingRootSemanticsAndUnknownFieldsAreRejected()
    {
        Directory.CreateDirectory(m_root);
        string json = JsonSerializer.Serialize(Create());
        string path = Path.Combine(m_root, "compilation.json");
        File.WriteAllText(path, json.Replace("\"frozen\":true,", string.Empty, StringComparison.Ordinal)
            .Replace(",\"frozen\":true", string.Empty, StringComparison.Ordinal));
        Assert.Throws<JsonException>(() => NativeBindingCompilation.Load(m_root));
        File.WriteAllText(path, json.Insert(1, "\"unrecognized\":true,"));
        Assert.Throws<JsonException>(() => NativeBindingCompilation.Load(m_root));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, true);
    }

    private static NativeBindingCompilation Create()
    {
        string hash = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes("verified")));
        return new NativeBindingCompilation
        {
            roots = [new("user", "source", false, true), new("nested", "source/nested", false, true), new("generated", "", true, true)],
            files = [new("user", "nested/api.hpp", 8, hash, false), new("nested", "api.hpp", 8, hash, true)],
            includeDirectories = [new("user", "", false, false), new("nested", "", false, false)],
            defines = ["first", "second"], compilerDefines = ["__COMPILER_FACT=1"], cxxStandard = 20, cStandard = 0,
            requiredExports = ["exported"], staticExportSymbols = [], staticExportAbi = "", staticExportNames = []
        };
    }
}
