using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Inno.Build.Toolchains.Bgfx.Tools;
using Xunit;

namespace Inno.Build.Tests;

public sealed class ToolRunnerPathTests
{
    [Fact]
    public async Task DeepOwnedExecutableStartsAndRemainsAfterItsAliasIsReleased()
    {
        if (!OperatingSystem.IsWindows())
            return;
        string root = Path.Combine(Path.GetTempPath(), "InnoToolRunner", Guid.NewGuid().ToString("N"));
        string directory = Path.Combine(root, new string('a', 90), new string('b', 90), new string('c', 50));
        Directory.CreateDirectory(directory);
        string executable = Path.Combine(directory, "shaderc.exe");
        try
        {
            File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "where.exe"), executable);
            Assert.True(executable.Length > 260);
            var runner = new ToolRunner(new Dictionary<BgfxTool, string> { [BgfxTool.Shaderc] = executable }, directory);
            ToolRunResult result = await runner.RunAsync(BgfxTool.Shaderc, ["/?"], root);
            Assert.Equal(0, result.exitCode);
            Assert.True(File.Exists(executable));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
