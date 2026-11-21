using System;
using System.IO;
using Xunit;
using Inno.Build;
using Inno.Testing.SupportPacks;

namespace Inno.Build.MacOSArm64.Tests;

public sealed class SupportPackValidationTests : PlayerSupportPackFixture
{
    [Fact]
    public void PlatformPackRejectsAMissingContentSource()
    {
        BuildTargetId target = new BuildTargetId("macos-arm64");
        string pack = GetSupportPackDirectory(target);
        string source = "MacOSArm64PlayerContentSource.cs";
        File.Delete(Path.Combine(pack, "PlayerLink", source));
        IPlayerSupportPackValidator validator = new MacOSArm64SupportPackValidator();

        InvalidDataException missing = Assert.Throws<InvalidDataException>(() => validator.Validate(pack));

        Assert.Contains(source, missing.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void SupportPackRejectsAuthoringAssetPipelineAssemblies()
    {
        string supportPack = GetSupportPackDirectory(new BuildTargetId("macos-arm64"));
        File.WriteAllBytes(Path.Combine(supportPack, "Inno.Assets.Pipeline.dll"), [0x49, 0x4E, 0x4E, 0x4F]);
        var catalog = new PlayerSupportPackCatalog(m_supportPackRoot);

        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => catalog.Acquire(new BuildTargetId("macos-arm64"), new Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator())
        );

        Assert.Contains("forbidden build-time file", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SupportPackRejectsMissingMiniAudioRuntime()
    {
        string supportPack = GetSupportPackDirectory(new BuildTargetId("macos-arm64"));
        File.Delete(Path.Combine(supportPack, "native", "libminiaudio-release.dylib"));
        var catalog = new PlayerSupportPackCatalog(m_supportPackRoot);

        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => catalog.Acquire(new BuildTargetId("macos-arm64"), new Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator())
        );

        Assert.Contains("no longer matches", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void PlatformValidatorRejectsForeignAbiDespiteTheExpectedFilename()
    {
        BuildTargetId target = new BuildTargetId("macos-arm64");
        string directory = GetSupportPackDirectory(target);
        string library = "libminiaudio-release.dylib";
        File.WriteAllBytes(
            Path.Combine(directory, "native", library),
            CreateNativeImage(new BuildTargetId("windows-x64"))
        );
        IPlayerSupportPackValidator validator = new MacOSArm64SupportPackValidator();
        InvalidDataException failure = Assert.Throws<InvalidDataException>(() => validator.Validate(directory));
        Assert.Contains("ARM64 ABI", failure.Message);
    }

    [Fact]
    public void SupportPackRejectsForeignNativeRuntime()
    {
        string supportPack = GetSupportPackDirectory(new BuildTargetId("macos-arm64"));
        File.WriteAllBytes(Path.Combine(supportPack, "native", "SDL3-release.so"), [0x49, 0x4E, 0x4E, 0x4F]);
        var catalog = new PlayerSupportPackCatalog(m_supportPackRoot);

        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => catalog.Acquire(new BuildTargetId("macos-arm64"), new Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator())
        );

        Assert.Contains("foreign native runtime", exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("libinno-text-release.dylib")]
    [InlineData("libinno-ui-release.dylib")]
    public void SupportPackRejectsMissingTextOrUiRuntime(string nativeRuntime)
    {
        string supportPack = GetSupportPackDirectory(new BuildTargetId("macos-arm64"));
        File.Delete(Path.Combine(supportPack, "native", nativeRuntime));
        var catalog = new PlayerSupportPackCatalog(m_supportPackRoot);

        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => catalog.Acquire(new BuildTargetId("macos-arm64"), new Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator())
        );

        Assert.Contains("no longer matches", exception.Message, StringComparison.Ordinal);
    }
}
