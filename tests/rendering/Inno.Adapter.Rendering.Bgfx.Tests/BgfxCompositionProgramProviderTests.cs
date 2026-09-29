using System;
using Inno.Rendering;
using Xunit;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

public sealed class BgfxCompositionProgramProviderTests
{
    [Fact]
    public void EmbeddedTargetProgramMatchesTheDeviceBackendAndFullscreenLayout()
    {
        if (!OperatingSystem.IsMacOS() && !OperatingSystem.IsWindows())
            return;
        GraphicsApi api = OperatingSystem.IsMacOS() ? GraphicsApi.Metal : GraphicsApi.Direct3D11;
        var capabilities = new GraphicsCapabilities(
            api, GraphicsCapability.None, new GraphicsLimits(16, 1, 256, 0),
            [], [], [], [], originBottomLeft: false, homogeneousDepth: false);
        var layout = new RenderVertexLayout(
        [
            new(RenderVertexSemantic.Position, RenderVertexFormat.Float2),
            new(RenderVertexSemantic.TextureCoordinate0, RenderVertexFormat.Float2),
            new(RenderVertexSemantic.Color0, RenderVertexFormat.UInt8Normalized4)
        ]);

        GraphicsPipelineDescriptor descriptor = new BgfxCompositionProgramProvider()
            .CreateDescriptor(capabilities, layout);

        Assert.Same(layout, descriptor.vertexLayout);
        Assert.Equal("s_tex", Assert.Single(descriptor.bindings).id.value);
        Assert.False(descriptor.vertexShader.IsEmpty);
        Assert.False(descriptor.fragmentShader.IsEmpty);
    }
}
