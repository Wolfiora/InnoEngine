using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets.Pipeline;
using Inno.Build.Toolchains;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains.Bgfx.Shaders;
using Inno.Core.IO;
using Inno.Integration.WindowsX64.Bgfx;
using Inno.Rendering;
using Xunit;

namespace Inno.Build.Tests;

public sealed class ShaderBuildInputsTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoShaderInputs", Guid.NewGuid().ToString("N"));

    public ShaderBuildInputsTests() => Directory.CreateDirectory(m_root);

    [Fact]
    public void LivePublicationIsNotAnAssetButPermanentImportSettingsAreBuildInputs()
    {
        string source = Path.Combine(m_root, "shader.ishader");
        File.WriteAllText(source, "source");
        File.WriteAllText(source + ".imeta", "permanent settings");
        File.WriteAllText(source + ".abin", "derived artifact");
        ShaderBuildInputs baseline = ShaderBuildInputs.Capture(m_root);
        string fingerprint = baseline.fingerprint;
        string candidate = source + ".imeta.staging-" + Guid.NewGuid().ToString("N");
        using (FileStream publication = new(candidate, FileMode.CreateNew, FileAccess.Write, FileShare.None))
        {
            publication.Write("not yet published"u8);
            Assert.True(AssetSourcePolicy.defaultPolicy.IsIgnored(Path.GetFileName(candidate), false));
            ShaderBuildInputs current = ShaderBuildInputs.Capture(m_root);
            Assert.Equal(fingerprint, current.fingerprint);
            baseline.VerifyUnchanged();
        }
        File.WriteAllText(source + ".imeta", "changed settings");
        Assert.Throws<InvalidDataException>(() => baseline.VerifyUnchanged());
    }

    [Theory]
    [InlineData("edit")]
    [InlineData("add")]
    [InlineData("delete")]
    public void FreshVerificationIncludesContentAndTheCompleteInventory(string mutation)
    {
        string source = Path.Combine(m_root, "shader.txt");
        File.WriteAllText(source, "before");
        DateTime timestamp = File.GetLastWriteTimeUtc(source);
        ShaderBuildInputs captured = ShaderBuildInputs.Capture(m_root);
        switch (mutation)
        {
            case "edit":
                File.WriteAllText(source, "edited");
                File.SetLastWriteTimeUtc(source, timestamp);
                break;
            case "add":
                File.WriteAllText(Path.Combine(m_root, "new.imeta"), "settings");
                break;
            case "delete":
                File.Delete(source);
                break;
        }
        Assert.Throws<InvalidDataException>(() => captured.VerifyUnchanged());
    }

    [Fact]
    public void SourceNoiseDoesNotWeakenExplicitNativeInputDeclarations()
    {
        string candidate = Path.Combine(m_root, "source.staging-" + Guid.NewGuid().ToString("N"));
        File.WriteAllText(candidate, "first");
        NativeBuildInput[] explicitInputs = [new("explicit-input", candidate)];
        string before = NativeBuildFingerprint.Create([], explicitInputs);
        File.WriteAllText(candidate, "other");
        Assert.NotEqual(before, NativeBuildFingerprint.Create([], explicitInputs));
        string sourceIdentity = ShaderBuildInputs.Capture(m_root).fingerprint;
        File.WriteAllText(candidate, "again");
        Assert.Equal(sourceIdentity, ShaderBuildInputs.Capture(m_root).fingerprint);
    }

    [Fact]
    public void EmptyAuthoredDirectoriesParticipateInTheFrozenInventory()
    {
        ShaderBuildInputs before = ShaderBuildInputs.Capture(m_root);
        string directory = Path.Combine(m_root, "Includes");
        Directory.CreateDirectory(directory);
        Assert.NotEqual(before.fingerprint, ShaderBuildInputs.Capture(m_root).fingerprint);
        Assert.Throws<InvalidDataException>(() => before.VerifyUnchanged());
        ShaderBuildInputs withDirectory = ShaderBuildInputs.Capture(m_root);
        Directory.Delete(directory);
        Assert.Throws<InvalidDataException>(() => withDirectory.VerifyUnchanged());
        Assert.Equal(before.fingerprint, ShaderBuildInputs.Capture(m_root).fingerprint);
    }

    [Fact]
    public async Task FailedImportDoesNotWriteTheAuthoringTreeOrReplaceAnExistingArtifact()
    {
        string sources = Path.Combine(m_root, "Assets");
        Directory.CreateDirectory(sources);
        File.WriteAllText(Path.Combine(sources, "invalid.ishader"), "invalid authored graph");
        string output = Path.Combine(m_root, "program.bin");
        File.WriteAllText(output, "last good artifact");
        string[] original = Directory.GetFiles(sources);
        byte[] bytes = File.ReadAllBytes(original.Single());
        ShaderBuildInputs inputs = ShaderBuildInputs.Capture(sources);
        using ProcessOwnedDirectory compilation = await ProcessOwnedDirectory.CreateAsync(Path.Combine(m_root, "requests"));
        Assert.Throws<InvalidDataException>(() => ShaderArtifactBuilder.CompileGraphicsProgram(
            inputs, compilation, "invalid.ishader", WindowsX64BgfxIntegration.shaderProfile, GraphicsApi.Direct3D12, output));
        Assert.Equal(original, Directory.GetFiles(sources));
        Assert.Equal(bytes, File.ReadAllBytes(original.Single()));
        Assert.Equal("last good artifact", File.ReadAllText(output));
        inputs.VerifyUnchanged();
    }

    [Fact]
    public async Task CancellationCannotPublishAnArtifact()
    {
        using var canceled = new CancellationTokenSource();
        canceled.Cancel();
        Assert.Throws<OperationCanceledException>(() => ShaderBuildInputs.Capture(m_root, canceled.Token));
        ShaderBuildInputs inputs = ShaderBuildInputs.Capture(m_root);
        string output = Path.Combine(m_root, "program.bin");
        File.WriteAllText(output, "last good artifact");
        using ProcessOwnedDirectory compilation = await ProcessOwnedDirectory.CreateAsync(Path.Combine(m_root, "requests"));
        Assert.Throws<OperationCanceledException>(() => ShaderArtifactBuilder.CompileGraphicsProgram(
            inputs, compilation, "graph.ishader", WindowsX64BgfxIntegration.shaderProfile, GraphicsApi.Direct3D12, output,
            cancellationToken: canceled.Token));
        Assert.Equal("last good artifact", File.ReadAllText(output));
    }

    [Fact]
    public async Task CompilationWorkspaceMetadataUsesTheExistingRegisteredRetirementProtocol()
    {
        string directory;
        using (ProcessOwnedDirectory workspace = await StandardBuildCaches.CreateShaderWorkspaceAsync(m_root))
        {
            directory = workspace.directory;
            Assert.True(Directory.Exists(directory));
            var registration = Assert.Single(BuildCacheOwnerCatalog.ReadOwners(m_root));
            Assert.Equal("shader-requests", registration.id);
            Assert.Equal(Path.GetDirectoryName(directory), registration.directory);
        }
        Assert.False(Directory.Exists(directory));
        string owner = Path.GetDirectoryName(directory)!;
        using FileLease exclusive = await FileLease.AcquireAsync(Path.Combine(owner, "owner.lock"), TimeSpan.FromSeconds(5));
        Assert.Equal(1, ProcessOwnedDirectory.RetireMetadataUnderOwnership(owner));
        Assert.Empty(Directory.GetFiles(owner, "*.work.json"));
        Assert.Empty(Directory.GetFiles(owner, "*.work.lock"));
        Assert.True(File.Exists(Path.Combine(owner, "owner.lock")));
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);
}
