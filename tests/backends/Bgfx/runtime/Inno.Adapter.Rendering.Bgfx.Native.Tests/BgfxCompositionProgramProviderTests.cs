using System;
using System.Runtime.Versioning;
using Xunit;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx.Native.Tests;

public sealed class BgfxCompositionProgramProviderTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public void EmbeddedTargetProgramMatchesTheDeviceBackendAndFullscreenLayout(bool outputTransfer)
    {
        if (!OperatingSystem.IsMacOS() && !OperatingSystem.IsWindows())
        {
            return;
        }
        GraphicsApi[] apis = OperatingSystem.IsMacOS()
                             ? [GraphicsApi.metal, GraphicsApi.vulkan, GraphicsApi.openGL]
                             : [GraphicsApi.direct3D11, GraphicsApi.direct3D12, GraphicsApi.vulkan, GraphicsApi.openGL];
        var layout = new RenderVertexLayout(
            [
                new RenderVertexAttribute(RenderVertexSemantic.Position, RenderVertexFormat.Float2),
                new RenderVertexAttribute(RenderVertexSemantic.TextureCoordinate0, RenderVertexFormat.Float2),
                new RenderVertexAttribute(RenderVertexSemantic.Color0, RenderVertexFormat.UInt8Normalized4)
            ]
        );

        var provider = new BgfxCompositionProgramProvider();
        foreach (GraphicsApi api in apis)
        {
            var capabilities = new GraphicsCapabilities(
                api,
                GraphicsCapability.None,
                new GraphicsLimits(16, 1, 256, 0),
                [],
                [],
                [],
                [],
                originBottomLeft: false,
                homogeneousDepth: false
            );
            GraphicsPipelineDescriptor descriptor = outputTransfer
                                                    ? provider.CreateOutputTransferDescriptor(capabilities, layout)
                                                    : provider.CreateDescriptor(capabilities, layout);

            Assert.Same(layout, descriptor.vertexLayout);
            Assert.Equal("s_tex", Assert.Single(descriptor.bindings).id.value);
            Assert.False(descriptor.vertexShader.IsEmpty);
            Assert.False(descriptor.fragmentShader.IsEmpty);
            Assert.Equal(
                outputTransfer ? RenderBlendState.opaque : RenderBlendState.premultiplied,
                descriptor.rasterState.blend
            );
        }
    }
}
