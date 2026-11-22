using System;
using System.IO;
using System.Text;
using Xunit;
using Inno.Build.SupportPacks;

namespace Inno.Build.SupportPacks.Tests;

public sealed class SupportPackClosureValidationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoPackClosureTests", Guid.NewGuid().ToString("N"));

    [Fact]
    public void ACustomBackendUsesItsDeclaredGroupWithoutStandardLibraryNames()
    {
        Write("PlayerLink/References/Custom.Runtime.dll");
        Write("native/custom/fixture-target/arbitrary-name.fixture");
        var requirements = new PlayerSupportPackRequirements(
            new BuildTargetID("fixture-target"),
            ["PlayerLink/References/Custom.Runtime.dll"],
            "native",
            ["custom/fixture-target"],
            ".fixture"
        );
        var validator = new PlayerSupportPackClosureValidator(requirements, new LayoutValidator(), null);
        validator.Validate(m_root);
        Write("native/unselected/fixture-target/another.fixture");
        Assert.Throws<InvalidDataException>(() => validator.Validate(m_root));
    }

    [Fact]
    public void MissingDeclaredArchiveFailsEvenWhenItsGroupContainsAnotherArchive()
    {
        Write("PlayerLink/Native/custom/fixture-target/other.a");
        var requirements = new PlayerSupportPackRequirements(
            new BuildTargetID("fixture-target"),
            ["PlayerLink/Native/custom/fixture-target/required.a"],
            "PlayerLink/Native",
            ["custom/fixture-target"],
            ".a"
        );
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
        var requirements = new PlayerSupportPackRequirements(
            new BuildTargetID("fixture-target"),
            ["PlayerLink/Native/custom/fixture-target/required.a"],
            "PlayerLink/Native",
            ["custom/fixture-target"],
            ".a",
            ["custom/fixture-target/required.a"]
        );
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
        => Assert.Throws<ArgumentException>(() => new PlayerSupportPackRequirements(
            new BuildTargetID("fixture-target"),
            [path],
            "native",
            ["custom"],
            ".fixture"
        ));

    public void Dispose()
    {
        if (Directory.Exists(m_root))
        {
            Directory.Delete(m_root, recursive: true);
        }
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
