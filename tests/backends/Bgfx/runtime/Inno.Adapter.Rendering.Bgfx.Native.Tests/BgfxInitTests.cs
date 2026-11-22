using System;
using Xunit;
using Inno.Native.Bgfx;

namespace Inno.Adapter.Rendering.Bgfx.Native.Tests;

public sealed class BgfxInitTests
{
    [Fact]
    public unsafe void InitAndShutdownShouldSucceed()
    {
        bgfx.Init init;
        bgfx.Raw.init_ctor(&init);
        init.type = bgfx.RendererType.Noop;
        init.resolution.width = 1;
        init.resolution.height = 1;

        bool ok = bgfx.Raw.init(&init) != 0;
        Assert.True(ok);
        try
        {
            Assert.True(bgfx.Raw.get_caps() is not null);
            Assert.Equal(bgfx.RendererType.Noop, bgfx.get_renderer_type());
        }
        finally
        {
            bgfx.shutdown();
        }
    }
}
