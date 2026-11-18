using System;
using Xunit;

namespace Inno.Native.Bgfx.Tests;

public sealed class BgfxInitTests
{
    [Fact]
    public unsafe void InitAndShutdown_ShouldSucceed()
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
            Assert.True(bgfx.Raw.get_caps() != null);
            Assert.Equal(bgfx.RendererType.Noop, bgfx.get_renderer_type());
        }
        finally
        {
            bgfx.shutdown();
        }
    }
}
