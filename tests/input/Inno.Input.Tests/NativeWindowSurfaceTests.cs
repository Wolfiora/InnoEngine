using System;
using Inno.Adapter.Platform;
using Inno.Platform;
using Xunit;

namespace Inno.Input.Tests;

public sealed class NativeWindowSurfaceTests
{
    [Fact]
    public void NativeSurfaceIdentifiersAreOpenAndRejectWhitespace()
    {
        var thirdParty = new PlatformNativeHandleId("tests.surface.custom");
        Assert.True(thirdParty.isValid);
        Assert.Equal(new PlatformNativeHandleId("tests.surface.custom"), thirdParty);
        Assert.NotEqual(new PlatformNativeHandleId("tests.surface.Custom"), thirdParty);
        Assert.False(default(PlatformNativeHandleId).isValid);
        Assert.Throws<ArgumentException>(() => new PlatformNativeHandleId(" "));
        Assert.Throws<ArgumentException>(() => new PlatformNativeHandleId("tests. surface"));
    }

}
