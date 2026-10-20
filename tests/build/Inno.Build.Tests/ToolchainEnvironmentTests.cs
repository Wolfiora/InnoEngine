using Inno.Testing.Source;
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
        string engine = TestRepository.GetRoot(typeof(ToolchainEnvironmentTests).Assembly);
        string expected = Path.Combine(engine,
            "build", "toolchains", "Inno.Build.Toolchains", "obj", "native");
        var component = new NativeComponentDescriptor(
            "fixture",
            "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
            "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj");
        var context = new NativeBuildContext(engine, "debug");

        Assert.Equal(expected, context.GetNativeBuildRoot(component));
    }

    [Fact]
    public void NativeBuildRootRejectsAnUnrelatedOwner()
    {
        var context = new NativeBuildContext(TestRepository.GetRoot(typeof(ToolchainEnvironmentTests).Assembly), "debug");
        var component = new NativeComponentDescriptor("missing", "missing/Native.csproj", "missing/Build.csproj");

        Assert.Throws<InvalidOperationException>(() => context.GetNativeBuildRoot(component));
    }

    [Fact]
    public void NativeBuildRootRequiresAnOwner()
    {
        var context = new NativeBuildContext(TestRepository.GetRoot(typeof(ToolchainEnvironmentTests).Assembly), "debug");

        Assert.Throws<ArgumentNullException>(() => context.GetNativeBuildRoot(null!));
    }
}
