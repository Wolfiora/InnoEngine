using System;
using System.IO;
using Xunit;
using Inno.Build;
using Inno.Testing.SupportPacks;

namespace Inno.Build.WindowsX64.Tests;

public sealed class SupportPackValidationTests : PlayerSupportPackFixture
{
    [Fact]
    public void PlatformPackRejectsAMissingContentSource()
    {
        var target = new BuildTargetId("windows-x64");
        string pack = GetSupportPackDirectory(target);
        string source = "WindowsX64PlayerContentSource.cs";
        File.Delete(Path.Combine(pack, "PlayerLink", source));
        IPlayerSupportPackValidator validator = new WindowsX64SupportPackValidator();

        InvalidDataException missing = Assert.Throws<InvalidDataException>(() => validator.Validate(pack));

        Assert.Contains(source, missing.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void PlatformValidatorRejectsForeignAbiDespiteTheExpectedFilename()
    {
        var target = new BuildTargetId("windows-x64");
        string directory = GetSupportPackDirectory(target);
        string library = "miniaudio-release.dll";
        File.WriteAllBytes(
            Path.Combine(directory, "native", library),
            CreateNativeImage(new BuildTargetId("macos-arm64"))
        );
        IPlayerSupportPackValidator validator = new WindowsX64SupportPackValidator();
        InvalidDataException failure = Assert.Throws<InvalidDataException>(() => validator.Validate(directory));
        Assert.Contains("PE header", failure.Message);
    }
}
