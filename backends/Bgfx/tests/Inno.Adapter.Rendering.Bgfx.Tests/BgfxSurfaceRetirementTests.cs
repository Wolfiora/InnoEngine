using System;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Integration.Windows.Sdl3;
using Inno.Platform;
using Inno.Rendering;
using Xunit;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

[Collection("Windowed BGFX")]
public sealed class BgfxSurfaceRetirementTests
{
    [Fact]
    public void MultiThreadedSurfaceAcknowledgesDestructionAfterTheFollowingSubmission() => VerifyRetirement(false);

    [Fact]
    public void InlineSurfaceUsesTheSameConservativeAcknowledgment() => VerifyRetirement(true);

    [Fact]
    public void InvalidIntegrationFailsBeforeTakingTheProcessOwner()
    {
        using var application = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        using var window = application.CreateWindow(new PlatformWindowOptions
        {
            title = "Invalid integration", width = 100, height = 90, visible = false
        });
        Assert.Throws<ArgumentException>(() => new BgfxDevice(new BgfxDeviceOptions
        {
            preferredBackend = GraphicsApi.Direct3D11,
            window = window,
            surfaceIntegration = new InvalidIntegration()
        }));
        using var device = new BgfxDevice(new BgfxDeviceOptions
        {
            preferredBackend = GraphicsApi.Direct3D11,
            window = window,
            surfaceIntegration = new FixtureIntegration()
        });
        device.BeginFrame();
        device.EndFrame();
    }

    private static void VerifyRetirement(bool inline)
    {
        using var application = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        using var primary = application.CreateWindow(new PlatformWindowOptions
        {
            title = "Primary surface", width = 100, height = 90, visible = false
        });
        using var additional = application.CreateWindow(new PlatformWindowOptions
        {
            title = "Additional surface", width = 100, height = 90, visible = false
        });
        var integration = new FixtureIntegration();
        using var device = new BgfxDevice(new BgfxDeviceOptions
        {
            preferredBackend = GraphicsApi.Direct3D11,
            window = primary,
            surfaceIntegration = integration,
            forceSingleThreaded = inline,
            deferredDestroyFrames = 100
        });
        PlatformNativeHandles native = additional.nativeHandles;
        var handles = new PlatformNativeHandles(
            native.windowHandle, native.displayHandle, new PlatformNativeHandleId("fixture.custom-surface"));
        RenderSurfaceHandle surface = device.CreateWindowSurface(handles, 100, 90, "Custom surface");
        device.ResizeWindowSurface(surface, 110, 95);
        device.ResizeWindowSurface(surface, 120, 100);
        var retirement = device.RetireWindowSurface(surface);
        Assert.False(retirement.IsCompleted);
        Assert.Equal(2, integration.resolveCount);
        Assert.Throws<ArgumentException>(() => device.ResizeWindowSurface(surface, 120, 100));

        device.BeginFrame();
        Assert.Throws<InvalidOperationException>(() => device.DrainWindowSurfaceRetirements());
        device.EndFrame();
        Assert.False(retirement.IsCompleted);
        device.BeginFrame();
        device.EndFrame();
        Assert.True(retirement.IsCompletedSuccessfully);
        Assert.False(additional.isClosed);

        RenderSurfaceHandle final = device.CreateWindowSurface(handles, 100, 90, "Final surface");
        var drained = device.RetireWindowSurface(final);
        device.DrainWindowSurfaceRetirements();
        Assert.True(drained.IsCompletedSuccessfully);
        device.DrainWindowSurfaceRetirements();
    }

    private sealed class FixtureIntegration : IBgfxSurfaceIntegration
    {
        internal int resolveCount;
        public bool supportsAdditionalSurfaces => true;
        public BgfxSurfaceDescriptor Resolve(
            PlatformNativeHandles handles,
            BgfxSurfaceRole role
        ) {
            resolveCount++;
            return new BgfxSurfaceDescriptor(handles.windowHandle, handles.displayHandle, true);
        }
    }

    private sealed class InvalidIntegration : IBgfxSurfaceIntegration
    {
        public bool supportsAdditionalSurfaces => true;
        public BgfxSurfaceDescriptor Resolve(
            PlatformNativeHandles handles,
            BgfxSurfaceRole role
        ) => default;
    }
}

[CollectionDefinition("Windowed BGFX", DisableParallelization = true)]
public sealed class WindowedBgfxCollection;
