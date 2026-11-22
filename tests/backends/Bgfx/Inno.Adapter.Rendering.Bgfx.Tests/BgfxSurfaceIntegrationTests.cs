using System;
using Xunit;
using Inno.Adapter.Platform;
using Inno.Integration.BrowserWasm32.Bgfx.Runtime;
using Inno.Integration.MacOSArm64.Bgfx.Runtime;
using Inno.Integration.WindowsX64.Bgfx.Runtime;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

public sealed class BgfxSurfaceIntegrationTests
{
    [Fact]
    public void EachIntegrationValidatesItsOwnAbiAndRole()
    {
        var windows = new WindowsX64BgfxSurfaceIntegration();
        var macOS = new MacOSArm64BgfxSurfaceIntegration();
        var browser = new BrowserWasm32BgfxSurfaceIntegration();
        Assert.Equal(
            (nint)42,
            windows.Resolve(
                new PlatformNativeHandles((nint)42, handleKind: PlatformNativeHandleID.win32),
                BgfxSurfaceRole.Primary
            ).windowHandle
        );
        Assert.True(windows.supportsAdditionalSurfaces);
        Assert.True(
            macOS.Resolve(
                new PlatformNativeHandles((nint)42, handleKind: PlatformNativeHandleID.cocoa),
                BgfxSurfaceRole.Additional
            ).supportsSrgbReset
        );
        Assert.False(
            browser.Resolve(
                new PlatformNativeHandles((nint)42, handleKind: PlatformNativeHandleID.browserCanvas),
                BgfxSurfaceRole.Primary
            ).supportsSrgbReset
        );
        Assert.False(browser.supportsAdditionalSurfaces);
        Assert.Throws<NotSupportedException>(() => windows.Resolve(
            new PlatformNativeHandles((nint)42, handleKind: PlatformNativeHandleID.cocoa),
            BgfxSurfaceRole.Primary
        ));
        Assert.Throws<NotSupportedException>(() => browser.Resolve(
            new PlatformNativeHandles((nint)42, handleKind: PlatformNativeHandleID.browserCanvas),
            BgfxSurfaceRole.Additional
        ));
        Assert.Throws<ArgumentOutOfRangeException>(() => windows.Resolve(
            new PlatformNativeHandles((nint)42, handleKind: PlatformNativeHandleID.win32),
            (BgfxSurfaceRole)99
        ));
        Assert.Throws<ArgumentException>(() => windows.Resolve(
            new PlatformNativeHandles(0, handleKind: PlatformNativeHandleID.win32),
            BgfxSurfaceRole.Primary
        ));
    }

    [Fact]
    public void ProvidersRequireAnExplicitBorrowedIntegration()
    {
        Assert.Throws<ArgumentNullException>(() => new BgfxRenderingBackendProvider(null!));
        Assert.Throws<ArgumentException>(() => new BgfxSurfaceDescriptor(0, 0, false));
    }
}
