using System;
using System.IO;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeArtifactIdentityTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativeArtifactTests", Guid.NewGuid().ToString("N"));

    [Fact]
    public void FingerprintIsOrderIndependentAndChangesForSourceOrSdkIdentity()
    {
        string header = Write("include/api.h", "int sample(void);");
        string compiler = Write("tools/clang", "compiler");
        string first = NativeBuildFingerprint.Create(["sdk-a", "wasm32"], [header, compiler]);
        Assert.Equal(first, NativeBuildFingerprint.Create(["wasm32", "sdk-a"], [compiler, header, header]));
        Assert.NotEqual(first, NativeBuildFingerprint.Create(["sdk-b", "wasm32"], [header, compiler]));
        File.WriteAllText(header, "long sample(void);");
        Assert.NotEqual(first, NativeBuildFingerprint.Create(["sdk-a", "wasm32"], [header, compiler]));
        Assert.Throws<ArgumentException>(() => NativeBuildFingerprint.Create([], ["relative.h"]));
    }

    [Fact]
    public void ManifestChecksIdentityBytesAndTheCompleteOutputSet()
    {
        string source = Write("Generated/Bindings.cs", "generated");
        Write("Native/api.h", "bridge");
        BuildArtifactManifest.Write(m_root, "identity", ["Generated", "Native"]);
        Assert.True(BuildArtifactManifest.IsComplete(m_root, "identity", ["Generated", "Native"]));
        Assert.False(BuildArtifactManifest.IsComplete(m_root, "other", ["Generated", "Native"]));
        File.WriteAllText(source, "tampered");
        Assert.False(BuildArtifactManifest.IsComplete(m_root, "identity", ["Generated", "Native"]));
        File.WriteAllText(source, "generated");
        string extra = Write("Generated/Extra.cs", "extra");
        Assert.False(BuildArtifactManifest.IsComplete(m_root, "identity", ["Generated", "Native"]));
        File.Delete(extra);
        Assert.True(BuildArtifactManifest.IsComplete(m_root, "identity", ["Generated", "Native"]));
        File.Delete(source);
        Assert.False(BuildArtifactManifest.IsComplete(m_root, "identity", ["Generated", "Native"]));
    }

    [Fact]
    public void ManifestRejectsEscapingDirectoriesAndMalformedRecords()
    {
        Write("Generated/Bindings.cs", "generated");
        Assert.Throws<ArgumentException>(() => BuildArtifactManifest.Write(m_root, "identity", [".."]));
        Write("generation-manifest.json", "{ broken");
        Assert.False(BuildArtifactManifest.IsComplete(m_root, "identity", ["Generated"]));
    }

    [Fact]
    public void RequestDescriptorRejectsUnavailableOutputs()
    {
        string bindings = Write("Generated/Bindings.cs", "generated");
        string descriptor = Path.Combine(m_root, "request.json");
        new NativeBindingGenerationDescriptor {
            fingerprint = "identity", bindingsPath = bindings, bridgeDirectory = string.Empty
        }.Write(descriptor);
        Assert.Equal(bindings, NativeBindingGenerationDescriptor.Load(descriptor).bindingsPath);
        File.Delete(bindings);
        Assert.Throws<InvalidDataException>(() => NativeBindingGenerationDescriptor.Load(descriptor));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private string Write(
        string relative,
        string contents
    ) {
        string path = Path.Combine(m_root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, contents);
        return path;
    }
}
