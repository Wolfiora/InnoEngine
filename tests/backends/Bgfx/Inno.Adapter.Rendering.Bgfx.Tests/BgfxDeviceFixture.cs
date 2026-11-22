using System;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

public sealed class BgfxDeviceFixture : IDisposable
{
    public BgfxDeviceFixture()
    {
        this.device = CreateDevice();
    }

    public BgfxDevice device { get; }

    public void Dispose() => device.Dispose();

    private static BgfxDevice CreateDevice()
        => new(new BgfxDeviceOptions
        {
            preferredBackend = GraphicsApi.noop,
            backbufferWidth = 1,
            backbufferHeight = 1,
            verticalSync = false,
            sRgbBackbuffer = false,
            forceSingleThreaded = true,
            deferredDestroyFrames = 2
        });
}
