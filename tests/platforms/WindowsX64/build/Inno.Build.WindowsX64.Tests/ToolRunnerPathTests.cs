using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Build.WindowsX64;

namespace Inno.Build.WindowsX64.Tests;

public sealed class ToolRunnerPathTests
{
    [Fact]
    [SupportedOSPlatform("windows")]
    public async Task DeepOwnedExecutableStartsAndRemainsAfterItsAliasIsReleased()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }
        string root = Path.Combine(Path.GetTempPath(), "InnoToolRunner", Guid.NewGuid().ToString("N"));
        string directory = Path.Combine(root, new string('a', 90), new string('b', 90), new string('c', 50));
        Directory.CreateDirectory(directory);
        string executable = Path.Combine(directory, "shaderc.exe");
        try
        {
            File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"), executable);
            Assert.True(executable.Length > 260);
            var runner = new ToolRunner(
                new Dictionary<BgfxTool, string> { [BgfxTool.Shaderc] = executable },
                directory,
                new WindowsToolchainWorkspaceProvider()
            );
            ToolRunResult result = await runner.RunAsync(BgfxTool.Shaderc, ["/c", "exit", "0"], root);
            Assert.True(result.exitCode == 0, $"exit={result.exitCode}; stdout={result.standardOutput}; stderr={result.standardError}");
            Assert.True(File.Exists(executable));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
