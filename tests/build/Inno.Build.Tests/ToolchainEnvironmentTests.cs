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

        Assert.Equal(expected, new NativeBuildContext(ToolchainEnvironment.FindRepoRoot(), "debug").GetNativeBuildRoot(new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj")));
    }

    [Fact]
    public void NativeBuildRootRejectsAnUnrelatedOwner()
    {
        Assert.Throws<InvalidOperationException>(
            () => new NativeBuildContext(ToolchainEnvironment.FindRepoRoot(), "debug").GetNativeBuildRoot(new NativeComponentDescriptor("missing", "missing/Native.csproj", "missing/Build.csproj")));
    }

    [Fact]
    public void NativeBuildRootRequiresAnOwner()
    {
        Assert.Throws<ArgumentNullException>(() => new NativeBuildContext(ToolchainEnvironment.FindRepoRoot(), "debug").GetNativeBuildRoot(null!));
    }
}
