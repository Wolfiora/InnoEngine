using System;
using System.IO;
using Xunit;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Native.LibraryLoading;

namespace Inno.Build.Toolchains.Bgfx.Native.Tests;

public sealed class ToolRunnerTests
{
    [Fact]
    public void RunUsesExplicitlyDeployedTools()
    {
        foreach (BgfxTool tool in Enum.GetValues<BgfxTool>())
        {
            string toolName = tool.ToString().ToLowerInvariant();
            ToolRunResult result = new ToolRunner().Run(tool, ["--help"]);
            Assert.InRange(result.exitCode, 0, 1);
            Assert.False(string.IsNullOrWhiteSpace(result.standardOutput + result.standardError));
            AssertToolExists(toolName);
        }
    }

    private static void AssertToolExists(string toolName)
    {
        string path = NativeDllLoader.FindNativeFile(toolName);
        Assert.True(File.Exists(path), $"The selected tool '{toolName}' is unavailable at '{path}'.");
    }
}
