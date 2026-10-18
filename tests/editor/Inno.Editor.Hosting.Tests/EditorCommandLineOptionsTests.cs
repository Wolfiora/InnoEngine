using System;
using Inno.Editor.Hosting;
using Xunit;

namespace Inno.Editor.Hosting.Tests;

public sealed class EditorCommandLineOptionsTests
{
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
