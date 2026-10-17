using System;
using Inno.Rendering;
using Xunit;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

[Trait("ProcessIsolation", "Required")]
public sealed class BgfxProcessLifetimeTests
{
    [Fact]
    public void SingleThreadedRuntimeCannotRestartWithEitherThreadingPolicy()
    {
        using (var device = new BgfxDevice(new BgfxDeviceOptions
        {
            preferredBackend = GraphicsApi.Noop, forceSingleThreaded = true
        }))
        {
            device.BeginFrame();
            device.EndFrame();
        }
        foreach (bool inline in new[] { false, true })
            Assert.Throws<InvalidOperationException>(() => new BgfxDevice(new BgfxDeviceOptions
            {
                preferredBackend = GraphicsApi.Noop, forceSingleThreaded = inline
            }));
    }

    [Fact]
    public void UnsupportedRendererIsRejectedBeforeTheNativeThreadingModeChanges()
    {
        Assert.Throws<NotSupportedException>(() => new BgfxDevice(new BgfxDeviceOptions
        {
            preferredBackend = new GraphicsApi("fixture.unsupported"), forceSingleThreaded = true
        }));
        using var device = new BgfxDevice(new BgfxDeviceOptions { preferredBackend = GraphicsApi.Noop });
        device.BeginFrame();
        device.EndFrame();
    }

}
