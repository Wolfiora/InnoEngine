using System;
using Inno.Adapter.Platform;
using Inno.Integration.BrowserWasm32.Bgfx.Runtime;
using Inno.Integration.MacOSArm64.Bgfx.Runtime;
using Inno.Integration.WindowsX64.Bgfx.Runtime;
using Xunit;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

public sealed class BgfxSurfaceIntegrationTests
{
    [Fact]
    public void EachIntegrationValidatesItsOwnAbiAndRole()
    {
        var windows = new WindowsX64BgfxSurfaceIntegration();
        var macOS = new MacOSArm64BgfxSurfaceIntegration();
        var browser = new BrowserWasm32BgfxSurfaceIntegration();
        Assert.Equal((nint)42, windows.Resolve(new((nint)42, handleKind: PlatformNativeHandleId.win32),
            BgfxSurfaceRole.Primary).windowHandle);
        Assert.True(windows.supportsAdditionalSurfaces);
        Assert.True(macOS.Resolve(new((nint)42, handleKind: PlatformNativeHandleId.cocoa),
            BgfxSurfaceRole.Additional).supportsSrgbReset);
        Assert.False(browser.Resolve(new((nint)42, handleKind: PlatformNativeHandleId.browserCanvas),
            BgfxSurfaceRole.Primary).supportsSrgbReset);
        Assert.False(browser.supportsAdditionalSurfaces);
        Assert.Throws<NotSupportedException>(() => windows.Resolve(
            new((nint)42, handleKind: PlatformNativeHandleId.cocoa), BgfxSurfaceRole.Primary));
        Assert.Throws<NotSupportedException>(() => browser.Resolve(
            new((nint)42, handleKind: PlatformNativeHandleId.browserCanvas), BgfxSurfaceRole.Additional));
        Assert.Throws<ArgumentOutOfRangeException>(() => windows.Resolve(
            new((nint)42, handleKind: PlatformNativeHandleId.win32), (BgfxSurfaceRole)99));
        Assert.Throws<ArgumentException>(() => windows.Resolve(
            new(0, handleKind: PlatformNativeHandleId.win32), BgfxSurfaceRole.Primary));
    }

    [Fact]
    public void ProvidersRequireAnExplicitBorrowedIntegration()
    {
        Assert.Throws<ArgumentNullException>(() => new BgfxRenderingBackendProvider(null!));
        Assert.Throws<ArgumentException>(() => new BgfxSurfaceDescriptor(0, 0, false));
    }
}
