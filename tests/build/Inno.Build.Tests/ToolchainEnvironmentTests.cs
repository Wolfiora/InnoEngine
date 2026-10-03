using System;
using System.IO;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class ToolchainEnvironmentTests
{
    [Fact]
    public void NativeBuildRootBelongsToTheToolchainProject()
    {
        string expected = Path.Combine(ToolchainEnvironment.FindRepoRoot(),
            "build", "toolchains", "Inno.Build.Toolchains", "obj", "native");

        Assert.Equal(expected, new NativeBuildContext(ToolchainEnvironment.FindRepoRoot(), "debug").GetNativeBuildRoot(typeof(ToolchainEnvironment).Assembly));
    }

    [Fact]
    public void NativeBuildRootRejectsAnUnrelatedOwner()
    {
        Assert.Throws<InvalidOperationException>(
            () => new NativeBuildContext(ToolchainEnvironment.FindRepoRoot(), "debug").GetNativeBuildRoot(typeof(ToolchainEnvironmentTests).Assembly));
    }

    [Fact]
    public void NativeBuildRootRequiresAnOwner()
    {
        Assert.Throws<ArgumentNullException>(() => new NativeBuildContext(ToolchainEnvironment.FindRepoRoot(), "debug").GetNativeBuildRoot(null!));
    }
}
