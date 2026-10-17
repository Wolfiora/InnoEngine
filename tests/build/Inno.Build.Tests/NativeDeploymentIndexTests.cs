using System;
using System.Collections.Generic;
using System.IO;
using Inno.Native.LibraryLoading;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeDeploymentIndexTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoDeploymentIndex", Guid.NewGuid().ToString("N"));

    public NativeDeploymentIndexTests() => Directory.CreateDirectory(m_root);

    [Fact]
    public void UnknownSystemAndSuffixResolveThroughExplicitMappings()
    {
        string payload = Path.Combine(m_root, "custom", "renderer.target-module");
        Directory.CreateDirectory(Path.GetDirectoryName(payload)!);
        File.WriteAllText(payload, "fixture");
        var libraries = new Dictionary<string, string> { ["renderer"] = "custom/renderer.target-module" };
        var files = new Dictionary<string, string> { ["compiler"] = "custom/renderer.target-module" };
        var source = new NativeDeploymentIndex("new-system-custom-abi", libraries, files);
        libraries.Clear();
        files.Clear();
        NativeDeploymentIndex decoded = NativeDeploymentIndex.Decode(source.Encode());
        Assert.Equal("new-system-custom-abi", decoded.targetId);
        Assert.Equal(payload, decoded.ResolveLibrary(m_root, "renderer"));
        Assert.Equal(payload, decoded.ResolveFile(m_root, "compiler"));
        Assert.Throws<FileNotFoundException>(() => decoded.ResolveLibrary(m_root, "not-registered"));
        File.Delete(payload);
        Assert.Throws<FileNotFoundException>(() => decoded.ResolveFile(m_root, "compiler"));
    }

    [Theory]
    [InlineData("../outside.bin")]
    [InlineData("/absolute.bin")]
    [InlineData("C:/outside.bin")]
    [InlineData("part\\wrong.bin")]
    public void InvalidDeploymentPathsFailBeforeResolution(string path)
        => Assert.Throws<ArgumentException>(() => new NativeDeploymentIndex("fixture",
            new Dictionary<string, string> { ["library"] = path }, new Dictionary<string, string>()));

    [Fact]
    public void MissingAndUnknownIndexFieldsAreRejected()
    {
        Assert.Throws<InvalidDataException>(() => NativeDeploymentIndex.Decode("{}"u8));
        Assert.Throws<InvalidDataException>(() => NativeDeploymentIndex.Decode(
            "{\"targetId\":\"fixture\",\"libraries\":{},\"files\":{},\"oldLayout\":true}"u8));
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);
}
