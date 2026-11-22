using System;
using Xunit;
using Inno.Adapter.Platform;
using Inno.Platform;

namespace Inno.Shell.Tests;

public sealed class NativeWindowSurfaceTests
{
    [Fact]
    public void NativeSurfaceIdentifiersAreOpenAndRejectWhitespace()
    {
        var thirdParty = new PlatformNativeHandleID("tests.surface.custom");
        Assert.True(thirdParty.isValid);
        Assert.Equal(new PlatformNativeHandleID("tests.surface.custom"), thirdParty);
        Assert.NotEqual(new PlatformNativeHandleID("tests.surface.Custom"), thirdParty);
        Assert.False(default(PlatformNativeHandleID).isValid);
        Assert.Throws<ArgumentException>(() => new PlatformNativeHandleID(" "));
        Assert.Throws<ArgumentException>(() => new PlatformNativeHandleID("tests. surface"));
    }
}
