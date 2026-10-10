using System;
using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed unsafe partial class BgfxCommandEncoder

{

    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="vertexCount">
    /// The vertex count consumed by draw; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="instanceCount">
    /// The instance count consumed by draw; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void Draw(
        int vertexCount,
        int instanceCount = 1
    ) {
        BgfxPipelineResource pipeline = RequireGraphicsPipeline();
        ValidateDrawCounts(vertexCount, instanceCount);
        if (m_vertexBuffer is null)
        {
            throw new InvalidOperationException(
                "A direct non-procedural draw requires a bound vertex buffer. Use DrawProcedural explicitly otherwise.");
        }

        SetVertexBuffer(pipeline, m_vertexBuffer, m_firstVertex, vertexCount);
        Submit(pipeline, instanceCount);
    }

    /// <summary>
    /// Renders the procedural presentation for the current editor frame.
    /// </summary>
    /// <param name="vertexCount">
    /// The vertex count consumed by draw procedural; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="instanceCount">
    /// The instance count consumed by draw procedural; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void DrawProcedural(
        int vertexCount,
        int instanceCount = 1
    ) {
        BgfxPipelineResource pipeline = RequireGraphicsPipeline();
        if (!m_device.capabilities.Supports(GraphicsCapability.ProceduralDraw))
        {
            throw new NotSupportedException(
                "The active graphics backend does not support procedural vertex-ID draws.");
        }
        ValidateDrawCounts(vertexCount, instanceCount);
        bgfx.encoder_set_vertex_count(m_encoder, checked((uint)vertexCount));
        Submit(pipeline, instanceCount);
    }

    /// <summary>
    /// Renders the indexed presentation for the current editor frame.
    /// </summary>
    /// <param name="indexCount">
    /// The index count consumed by draw indexed; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="instanceCount">
    /// The instance count consumed by draw indexed; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void DrawIndexed(
        int indexCount,
        int instanceCount = 1
    ) {
        BgfxPipelineResource pipeline = RequireGraphicsPipeline();
        ValidateDrawCounts(indexCount, instanceCount);
        if (m_vertexBuffer is null)
        {
            throw new InvalidOperationException("An indexed draw requires a bound vertex buffer.");
        }

        if (m_indexBuffer is null)
        {
            throw new InvalidOperationException("An indexed draw requires a bound index buffer.");
        }

        SetVertexBuffer(
            pipeline,
            m_vertexBuffer,
            m_firstVertex,
            m_vertexBuffer.descriptor.elementCount - m_firstVertex);
        SetIndexBuffer(m_indexBuffer, m_firstIndex, indexCount);
        Submit(pipeline, instanceCount);
    }

    /// <summary>
    /// Renders the indirect presentation for the current editor frame.
    /// </summary>
    /// <param name="buffer">
    /// The buffer consumed by draw indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="firstCommand">
    /// The first command consumed by draw indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="commandCount">
    /// The command count consumed by draw indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void DrawIndirect(
        RenderBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    )
        => DrawIndirect(m_device.ResolveBuffer(buffer), firstCommand, commandCount);

    /// <summary>
    /// Renders the indirect presentation for the current editor frame.
    /// </summary>
    /// <param name="buffer">
    /// The buffer consumed by draw indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="firstCommand">
    /// The first command consumed by draw indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="commandCount">
    /// The command count consumed by draw indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void DrawIndirect(
        PersistentBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    )
        => DrawIndirect(m_device.ResolveBuffer(buffer), firstCommand, commandCount);

    private void DrawIndirect(
        BgfxBufferResource buffer,
        int firstCommand,
        int commandCount
    ) {
        BgfxPipelineResource pipeline = RequireGraphicsPipeline();
        ValidateIndirect(buffer, firstCommand, commandCount);
        if (m_vertexBuffer is not null)
        {
            SetVertexBuffer(
                pipeline,
                m_vertexBuffer,
                m_firstVertex,
                m_vertexBuffer.descriptor.elementCount - m_firstVertex);
        }
        else if (!m_device.capabilities.Supports(GraphicsCapability.ProceduralDraw))
        {
            throw new NotSupportedException(
                "An indirect draw without a vertex buffer requires procedural draw capability.");
        }
        if (m_indexBuffer is not null)
        {
            SetIndexBuffer(
                m_indexBuffer,
                m_firstIndex,
                m_indexBuffer.descriptor.elementCount - m_firstIndex);
        }
        SetDrawState(pipeline);
        bgfx.encoder_submit_indirect(
            m_encoder,
            m_viewId,
            pipeline.program,
            new bgfx.IndirectBufferHandle { idx = buffer.nativeIndex },
            checked((uint)firstCommand),
            checked((uint)commandCount),
            0,
            checked((byte)bgfx.DiscardFlags.All));
        m_device.RecordDraw(commandCount);
        m_instanceDataBound = false;
    }

    private void Submit(
        BgfxPipelineResource pipeline,
        int instanceCount
    ) {
        if (instanceCount > 1 && !m_device.capabilities.Supports(GraphicsCapability.Instancing))
            throw new NotSupportedException("The active graphics backend does not support instancing.");
        if (m_instanceDataBound && instanceCount != 1)
        {
            throw new ArgumentException(
                "A draw with bound instance data must use the bound instance count.",
                nameof(instanceCount));
        }
        if (!m_instanceDataBound && instanceCount > 1)
        {
            bgfx.encoder_set_instance_count(m_encoder, checked((uint)instanceCount));
        }

        SetDrawState(pipeline);
        bgfx.encoder_submit(
            m_encoder,
            m_viewId,
            pipeline.program,
            0,
            checked((byte)bgfx.DiscardFlags.All));
        m_device.RecordDraw();
        m_instanceDataBound = false;
    }

    private void SetDrawState(BgfxPipelineResource pipeline)
    {
        RenderRasterState state = m_rasterState ?? pipeline.rasterState!;
        bgfx.encoder_set_state(m_encoder, RasterState(state), state.blend.constantRgba);
        if (!m_stencilState.enabled)
        {
            bgfx.encoder_set_stencil(m_encoder, 0, 0);
            return;
        }
        bgfx.encoder_set_stencil(
            m_encoder,
            StencilFlags(m_stencilState, m_stencilState.front),
            StencilFlags(m_stencilState, m_stencilState.back));
    }

}
