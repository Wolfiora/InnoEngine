using System;
using System.IO;

using Xunit;

using Inno.Build.Toolchains.Bgfx.Tools;

namespace Inno.Build.Toolchains.Bgfx.Tests;

public sealed class ToolRunnerTests
{
    [Fact]
    public void Run_Throws_WhenToolMissing()
    {
        Assert.Throws<FileNotFoundException>(() => new ToolRunner().Run((BgfxTool)999, ["--help"]));
    }
}
