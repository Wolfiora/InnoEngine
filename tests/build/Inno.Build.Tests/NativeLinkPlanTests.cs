using System;
using Xunit;
using Inno.Build.Toolchains;

namespace Inno.Build.Tests;

public sealed class NativeLinkPlanTests
{
    [Fact]
    public void StaticOnlyTargetDoesNotAdvertiseSharedLibraries()
    {
        var plan = new NativeLinkPlan(staticLibraryExtension: ".archive");
        Assert.Null(plan.sharedLibraryExtension);
        Assert.Equal(".archive", plan.GetFileExtension(NativeLibraryKind.Static));
        Assert.Equal("component.archive", plan.GetStaticFileName("component"));
        Assert.Throws<NotSupportedException>(() => plan.GetFileExtension(NativeLibraryKind.Shared));
    }

    [Fact]
    public void InvalidOrUnassignedFormatsFailDuringSelection()
    {
        Assert.Throws<ArgumentException>(() => new NativeLinkPlan());
        Assert.Throws<ArgumentException>(() => new NativeLinkPlan(importLibraryExtension: ".import", staticLibraryExtension: ".archive"));
        Assert.Throws<ArgumentException>(() => new NativeLinkPlan("../escape"));
        Assert.Throws<ArgumentException>(() => new NativeLinkPlan(".shared", cmakeArguments: [null!]));
        Assert.Throws<ArgumentOutOfRangeException>(() => new NativeLinkPlan(".shared").GetFileExtension((NativeLibraryKind)99));
    }

    [Fact]
    public void OrderedLinkerInputsAreFrozen()
    {
        string[] arguments = ["-DFIRST=1", "-DSECOND=2"];
        var plan = new NativeLinkPlan(".shared", cmakeArguments: arguments);
        arguments[0] = "changed";
        Assert.Equal(new[] { "-DFIRST=1", "-DSECOND=2" }, plan.cmakeArguments);
    }
}
