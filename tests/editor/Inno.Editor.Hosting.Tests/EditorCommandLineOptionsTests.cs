using System;
using System.IO;
using Inno.Editor.Hosting;
using Xunit;

namespace Inno.Editor.Hosting.Tests;

public sealed class EditorCommandLineOptionsTests
{
    [Fact]
    public void DependencySelectionRemainsExplicitAndCannotFallBackToSourcePreparation()
    {
        string selection = Path.Combine(Path.GetTempPath(), "native-artifacts.json");
        Assert.Equal(selection, EditorCommandLineOptions.Parse(["project", "--native-dependencies", selection]).nativeDependencies);
        Assert.Null(EditorCommandLineOptions.Parse(["project"]).nativeDependencies);
        Assert.Throws<ArgumentException>(() => EditorCommandLineOptions.Parse(["project", "--native-dependencies", "relative.json"]));
        Assert.Throws<ArgumentException>(() => EditorCommandLineOptions.Parse(["project", "--native-dependencies", ""]));
        Assert.Throws<ArgumentException>(() => EditorCommandLineOptions.Parse(["project", "--native-dependencies", selection, "--native-dependencies", selection]));
    }

    [Fact]
    public void HiddenStartupUsesTheSameBoundedProductOptions()
    {
        EditorCommandLineOptions options = EditorCommandLineOptions.Parse(["project", "--window-visible", "false", "--smoke-frames", "60"]);
        Assert.False(options.windowVisible);
        Assert.Equal(60, options.smokeFrameLimit);
        Assert.True(EditorCommandLineOptions.Parse(["project"]).windowVisible);
    }

    [Theory]
    [InlineData("maybe")]
    [InlineData("")]
    public void InvalidVisibilityIsRejectedBeforeComposition(string value)
        => Assert.Throws<ArgumentException>(() => EditorCommandLineOptions.Parse(["project", "--window-visible", value]));

    [Fact]
    public void RepeatedVisibilityIsRejectedBeforeComposition()
        => Assert.Throws<ArgumentException>(() => EditorCommandLineOptions.Parse(["project", "--window-visible", "false", "--window-visible", "true"]));
}
