using System;
using System.IO;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeArtifactIdentityTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativeArtifactTests", Guid.NewGuid().ToString("N"));

    [Fact]
    public void MultipurposeRecipeOwnerIncludesOnlyItsDeclaredImplementationClosure()
    {
        Write("InnoEngine.sln", "fixture");
        string project = Write("platforms/Fixture/Inno.Build.Fixture.csproj", "<Project />");
        string builder = Write("platforms/Fixture/NativeBuilder.cs", "builder");
        string packaging = Write("platforms/Fixture/Packaging.cs", "packaging");
        var owner = new NativeComponentDescriptor("fixture",
            Path.GetRelativePath(m_root, project), Path.GetRelativePath(m_root, project));
        var context = new NativeBuildContext(m_root, "release").WithComponentOptions(owner, new(NativeLibraryKind.Shared));
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(
            context, owner, "fixture", "fixture-target", [], [], [builder]);
        foreach (NativeBuildInput input in recipe.inputs)
            if (!File.Exists(input.physicalPath) && !Directory.Exists(input.physicalPath))
                Write(Path.GetRelativePath(m_root, input.physicalPath), "common executor");
        string identity = NativeBuildFingerprint.Create(recipe.declarations, recipe.inputs);
        File.WriteAllText(packaging, "unrelated packaging changed");
        Assert.Equal(identity, NativeBuildFingerprint.Create(recipe.declarations, recipe.inputs));
        File.WriteAllText(builder, "native recipe changed");
        Assert.NotEqual(identity, NativeBuildFingerprint.Create(recipe.declarations, recipe.inputs));
    }

    [Fact]
    public void FingerprintPreservesDeclarationOrderAndChangesForSourceOrSdkIdentity()
    {
        string header = Write("include/api.h", "int sample(void);");
        string compiler = Write("tools/clang", "compiler");
        string first = NativeBuildFingerprint.Create(["sdk-a", "wasm32"], [new NativeBuildInput("include/api.h", header), new NativeBuildInput("tools/clang", compiler)]);
        Assert.Equal(first, NativeBuildFingerprint.Create(["sdk-a", "wasm32"], [new NativeBuildInput("tools/clang", compiler), new NativeBuildInput("include/api.h", header), new NativeBuildInput("include/api.h", header)]));
        Assert.NotEqual(first, NativeBuildFingerprint.Create(["sdk-b", "wasm32"], [new NativeBuildInput("include/api.h", header), new NativeBuildInput("tools/clang", compiler)]));
        Assert.NotEqual(first, NativeBuildFingerprint.Create(["wasm32", "sdk-a"], [new NativeBuildInput("include/api.h", header), new NativeBuildInput("tools/clang", compiler)]));
        File.WriteAllText(header, "long sample(void);");
        Assert.NotEqual(first, NativeBuildFingerprint.Create(["sdk-a", "wasm32"], [new NativeBuildInput("include/api.h", header), new NativeBuildInput("tools/clang", compiler)]));
        Assert.Throws<ArgumentException>(() => new NativeBuildInput("include/api.h", "relative.h"));
    }

    [Fact]
    public void LogicalInputsCanMoveWithoutChangingIdentityAndSameTimestampTamperingInvalidatesIt()
    {
        string first = Write("checkout-a/include/api.h", "int one(void);");
        string relocated = Write("checkout-b/include/api.h", "int one(void);");
        string fingerprint = NativeBuildFingerprint.Create(["target"], [new NativeBuildInput("include/api.h", first)]);
        Assert.Equal(fingerprint,
            NativeBuildFingerprint.Create(["target"], [new NativeBuildInput("include/api.h", relocated)]));
        DateTime timestamp = File.GetLastWriteTimeUtc(relocated);
        long length = new FileInfo(relocated).Length;
        File.WriteAllText(relocated, "int two(void);");
        File.SetLastWriteTimeUtc(relocated, timestamp);
        Assert.Equal(length, new FileInfo(relocated).Length);
        Assert.NotEqual(fingerprint,
            NativeBuildFingerprint.Create(["target"], [new NativeBuildInput("include/api.h", relocated)]));
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
