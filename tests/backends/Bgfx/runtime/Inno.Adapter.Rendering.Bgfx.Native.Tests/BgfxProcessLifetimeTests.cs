using System;
using Xunit;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx.Native.Tests;

[Trait("ProcessIsolation", "Required")]
public sealed class BgfxProcessLifetimeTests
{
    [Fact]
    public void SingleThreadedRuntimeCannotRestartWithEitherThreadingPolicy()
    {
        using (var device = new BgfxDevice(new BgfxDeviceOptions
        {
            preferredBackend = GraphicsApi.noop,
            forceSingleThreaded = true
        })) {
            device.BeginFrame();
            device.EndFrame();
        }
        foreach (bool inline in new[] { false, true })
        {
            Assert.Throws<InvalidOperationException>(() => new BgfxDevice(new BgfxDeviceOptions
            {
                preferredBackend = GraphicsApi.noop,
                forceSingleThreaded = inline
            }));
        }
    }

    [Fact]
    public void UnsupportedRendererIsRejectedBeforeTheNativeThreadingModeChanges()
    {
        Assert.Throws<NotSupportedException>(() => new BgfxDevice(new BgfxDeviceOptions
        {
            preferredBackend = new GraphicsApi("fixture.unsupported"),
            forceSingleThreaded = true
        }));
        using var device = new BgfxDevice(new BgfxDeviceOptions { preferredBackend = GraphicsApi.noop });
        device.BeginFrame();
        device.EndFrame();
    }
}
