using System;
using Inno.Rendering;
using Xunit;

namespace Inno.Rendering.Tests;

public sealed class GraphicsApiTests
{
    [Fact]
    public void CustomBackendIdentityCanFlowThroughDeviceCapabilities()
    {
        var api = new GraphicsApi("studio.renderer-v1");
        Assert.True(GraphicsApi.TryParse(api.ToString(), out GraphicsApi parsed));
        Assert.Equal(api, parsed);
        Assert.True(api.isValid);
        Assert.False(default(GraphicsApi).isValid);
        Assert.Throws<ArgumentException>(() => new GraphicsApi(".."));
        Assert.False(GraphicsApi.TryParse("../other", out _));
        Assert.Throws<ArgumentException>(() => new GraphicsCapabilities(
            default, GraphicsCapability.None, new GraphicsLimits(16, 1, 256, 0),
            [], [], [], [], originBottomLeft: false, homogeneousDepth: false));
        var capabilities = new GraphicsCapabilities(
            api, GraphicsCapability.None, new GraphicsLimits(16, 1, 256, 0),
            [], [], [], [], originBottomLeft: false, homogeneousDepth: false);
        Assert.Equal(api, capabilities.backend);
    }
}
