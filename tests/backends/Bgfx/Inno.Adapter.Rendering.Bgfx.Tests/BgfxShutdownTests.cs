using System;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Platform;
using Inno.Rendering;
using Xunit;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

[Collection("Windowed BGFX")]
[Trait("ProcessIsolation", "Required")]
public sealed class BgfxShutdownTests
{
    [Fact]
    public void ShutdownAfterAnOffscreenGraphReleasesTheProcessOwner()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(BgfxShutdownTests) + "." + nameof(ShutdownAfterAnOffscreenGraphReleasesTheProcessOwner)))
            return;

        string? requestedBackend = Environment.GetEnvironmentVariable("INNO_TEST_GRAPHICS_API");
        GraphicsApi backend = requestedBackend is null
            ? OperatingSystem.IsWindows() ? GraphicsApi.Direct3D11 : GraphicsApi.Metal
            : new GraphicsApi(requestedBackend);
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions
        {
            title = "Resource retirement", width = 100, height = 90, visible = false
        });
        var options = new BgfxDeviceOptions
        {
            preferredBackend = backend,
            window = window,
            surfaceIntegration = new FixtureIntegration()
        };

        using (var device = new BgfxDevice(options))
        {
            Assert.Equal(backend, device.capabilities.backend);
            RenderGraphBuilder builder = new(1, device.capabilities);
            RenderTextureDescriptor descriptor = new(
                32,
                32,
                RenderTextureFormat.RGBA8,
                RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled);
            RenderTextureHandle target = builder.CreateTexture("Retired target", descriptor);
            builder.AddRasterPass("Offscreen clear", new RenderPhaseId("tests.shutdown"), 0, static (
                _,
                _
            ) => { })
                .UseColorAttachment(target, 0, RenderLoadAction.Clear)
                .HasSideEffect();
            RenderGraphCompileResult result = builder.Compile();
            Assert.NotNull(result.graph);

            device.BeginFrame();
            device.CreateTexture(descriptor, "Device-owned texture");
            device.Execute(result.graph!, 1);
            device.EndFrame();
            device.Dispose();
            Assert.Equal(0u, device.generation);
            Assert.False(window.isClosed);
        }

        // A completed shutdown must release the process lease while retaining the borrowed window.
        using var successor = new BgfxDevice(options);
        Assert.Equal(backend, successor.capabilities.backend);
    }

    private sealed class FixtureIntegration : IBgfxSurfaceIntegration
    {
        public bool supportsAdditionalSurfaces => true;

        public BgfxSurfaceDescriptor Resolve(
            PlatformNativeHandles handles,
            BgfxSurfaceRole role
        ) => new(handles.windowHandle, handles.displayHandle, true);
    }
}
