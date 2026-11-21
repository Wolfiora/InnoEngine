using System;
using System.IO;
using Xunit;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Native.LibraryLoading;

namespace Inno.Build.Toolchains.Bgfx.Integration.Tests;

public sealed class ToolRunnerTests
{
    [Fact]
    public void Run_UsesExplicitlyDeployedTools()
    {
        foreach (var tool in Enum.GetValues<BgfxTool>())
        {
            var toolName = tool.ToString().ToLowerInvariant();
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
