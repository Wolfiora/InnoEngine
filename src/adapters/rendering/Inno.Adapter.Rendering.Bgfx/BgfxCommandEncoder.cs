using System;
using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed unsafe partial class BgfxCommandEncoder : RenderCommandEncoder
{
    private readonly BgfxDevice m_device;
    private readonly bgfx.Encoder m_encoder;
    private readonly ushort m_viewId;

    private BgfxPipelineResource? m_pipeline;
    private BgfxBufferResource? m_vertexBuffer;
    private BgfxBufferResource? m_indexBuffer;
    private RenderRasterState? m_rasterState;
    private RenderStencilState m_stencilState = RenderStencilState.disabled;
    private bool m_instanceDataBound;
    private int m_firstVertex;
    private int m_firstIndex;

    /// <summary>
    /// Creates a validated bgfx command encoder instance.
    /// </summary>
    /// <param name="device">
    /// The device consumed by bgfx command encoder; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="encoder">
    /// The encoder consumed by bgfx command encoder; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="viewId">
    /// The view id consumed by bgfx command encoder; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public BgfxCommandEncoder(
        BgfxDevice device,
        bgfx.Encoder encoder,
        ushort viewId
    ) {
        m_device = device;
        m_encoder = encoder;
        m_viewId = viewId;
    }

}
