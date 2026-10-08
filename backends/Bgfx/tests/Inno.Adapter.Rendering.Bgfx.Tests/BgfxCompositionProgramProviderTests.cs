using System;
using Inno.Rendering;
using Xunit;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

public sealed class BgfxCompositionProgramProviderTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void EmbeddedTargetProgramMatchesTheDeviceBackendAndFullscreenLayout(bool outputTransfer)
    {
        if (!OperatingSystem.IsMacOS() && !OperatingSystem.IsWindows())
            return;
        GraphicsApi[] apis = OperatingSystem.IsMacOS()
            ? [GraphicsApi.Metal, GraphicsApi.Vulkan, GraphicsApi.OpenGL]
            : [GraphicsApi.Direct3D11, GraphicsApi.Direct3D12, GraphicsApi.Vulkan, GraphicsApi.OpenGL];
        var layout = new RenderVertexLayout(
        [
            new(RenderVertexSemantic.Position, RenderVertexFormat.Float2),
            new(RenderVertexSemantic.TextureCoordinate0, RenderVertexFormat.Float2),
            new(RenderVertexSemantic.Color0, RenderVertexFormat.UInt8Normalized4)
        ]);

        var provider = new BgfxCompositionProgramProvider();
        foreach (GraphicsApi api in apis)
        {
            var capabilities = new GraphicsCapabilities(
                api, GraphicsCapability.None, new GraphicsLimits(16, 1, 256, 0),
                [], [], [], [], originBottomLeft: false, homogeneousDepth: false);
            GraphicsPipelineDescriptor descriptor = outputTransfer
                ? provider.CreateOutputTransferDescriptor(capabilities, layout)
                : provider.CreateDescriptor(capabilities, layout);

            Assert.Same(layout, descriptor.vertexLayout);
            Assert.Equal("s_tex", Assert.Single(descriptor.bindings).id.value);
            Assert.False(descriptor.vertexShader.IsEmpty);
            Assert.False(descriptor.fragmentShader.IsEmpty);
            Assert.Equal(outputTransfer ? RenderBlendState.opaque : RenderBlendState.premultiplied,
                descriptor.rasterState.blend);
        }
    }
}
