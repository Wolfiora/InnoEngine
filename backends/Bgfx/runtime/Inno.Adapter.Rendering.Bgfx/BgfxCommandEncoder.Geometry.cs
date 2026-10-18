using System;
using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed unsafe partial class BgfxCommandEncoder

{

    /// <summary>
    /// Binds a vertex buffer and its first vertex for subsequent draws.
    /// </summary>
    /// <param name="buffer">
    /// The buffer consumed by bind vertex buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="firstVertex">
    /// The first vertex consumed by bind vertex buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindVertexBuffer(
        RenderBufferHandle buffer,
        int firstVertex = 0
    )
        => BindVertexBuffer(m_device.ResolveBuffer(buffer), firstVertex);

    /// <summary>
    /// Binds a vertex buffer and its first vertex for subsequent draws.
    /// </summary>
    /// <param name="buffer">
    /// The buffer consumed by bind vertex buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="firstVertex">
    /// The first vertex consumed by bind vertex buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindVertexBuffer(
        PersistentBufferHandle buffer,
        int firstVertex = 0
    )
        => BindVertexBuffer(m_device.ResolveBuffer(buffer), firstVertex);

    /// <summary>
    /// Binds an index buffer and its first index for subsequent indexed draws.
    /// </summary>
    /// <param name="buffer">
    /// The buffer consumed by bind index buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="firstIndex">
    /// The first index consumed by bind index buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindIndexBuffer(
        RenderBufferHandle buffer,
        int firstIndex = 0
    )
        => BindIndexBuffer(m_device.ResolveBuffer(buffer), firstIndex);

    /// <summary>
    /// Binds an index buffer and its first index for subsequent indexed draws.
    /// </summary>
    /// <param name="buffer">
    /// The buffer consumed by bind index buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="firstIndex">
    /// The first index consumed by bind index buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindIndexBuffer(
        PersistentBufferHandle buffer,
        int firstIndex = 0
    )
        => BindIndexBuffer(m_device.ResolveBuffer(buffer), firstIndex);

    /// <summary>
    /// Binds per-instance data for subsequent instanced draws.
    /// </summary>
    /// <param name="buffer">
    /// The buffer consumed by bind instance buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="firstInstance">
    /// The first instance consumed by bind instance buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="instanceCount">
    /// The instance count consumed by bind instance buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindInstanceBuffer(
        RenderBufferHandle buffer,
        int firstInstance,
        int instanceCount
    )
        => BindInstanceBuffer(m_device.ResolveBuffer(buffer), firstInstance, instanceCount);

    /// <summary>
    /// Binds per-instance data for subsequent instanced draws.
    /// </summary>
    /// <param name="buffer">
    /// The buffer consumed by bind instance buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="firstInstance">
    /// The first instance consumed by bind instance buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="instanceCount">
    /// The instance count consumed by bind instance buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindInstanceBuffer(
        PersistentBufferHandle buffer,
        int firstInstance,
        int instanceCount
    )
        => BindInstanceBuffer(m_device.ResolveBuffer(buffer), firstInstance, instanceCount);

    private void BindVertexBuffer(
        BgfxBufferResource buffer,
        int firstVertex
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(firstVertex);
        if ((buffer.descriptor.usage & RenderBufferUsage.Vertex) == 0)
        {
            throw new ArgumentException("The buffer was not created for vertex input.", nameof(buffer));
        }

        if (firstVertex >= buffer.descriptor.elementCount)
        {
            throw new ArgumentOutOfRangeException(nameof(firstVertex));
        }

        m_vertexBuffer = buffer;
        m_firstVertex = firstVertex;
    }

    private void BindIndexBuffer(
        BgfxBufferResource buffer,
        int firstIndex
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(firstIndex);
        if ((buffer.descriptor.usage & RenderBufferUsage.Index) == 0)
        {
            throw new ArgumentException("The buffer was not created for index input.", nameof(buffer));
        }

        if (firstIndex >= buffer.descriptor.elementCount)
        {
            throw new ArgumentOutOfRangeException(nameof(firstIndex));
        }

        m_indexBuffer = buffer;
        m_firstIndex = firstIndex;
    }

    private void BindInstanceBuffer(
        BgfxBufferResource buffer,
        int firstInstance,
        int instanceCount
    ) {
        if (!m_device.capabilities.Supports(GraphicsCapability.Instancing))
            throw new NotSupportedException("The active graphics backend does not support instancing.");
        ArgumentOutOfRangeException.ThrowIfNegative(firstInstance);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(instanceCount);
        ValidateRange(firstInstance, instanceCount, buffer.descriptor.elementCount, nameof(instanceCount));
        switch (buffer.kind)
        {
            case BgfxBufferKind.Vertex:
                bgfx.encoder_set_instance_data_from_vertex_buffer(
                    m_encoder,
                    new bgfx.VertexBufferHandle { idx = buffer.nativeIndex },
                    checked((uint)firstInstance),
                    checked((uint)instanceCount));
                break;
            case BgfxBufferKind.DynamicVertex:
                bgfx.encoder_set_instance_data_from_dynamic_vertex_buffer(
                    m_encoder,
                    new bgfx.DynamicVertexBufferHandle { idx = buffer.nativeIndex },
                    checked((uint)firstInstance),
                    checked((uint)instanceCount));
                break;
            default:
                throw new ArgumentException("Instance data requires a vertex-compatible buffer.", nameof(buffer));
        }
        m_instanceDataBound = true;
    }

    private void SetVertexBuffer(
        BgfxPipelineResource pipeline,
        BgfxBufferResource buffer,
        int firstVertex,
        int vertexCount
    ) {
        ValidateRange(firstVertex, vertexCount, buffer.descriptor.elementCount, nameof(vertexCount));
        if (pipeline.vertexLayout is null || !pipeline.vertexLayoutHandle.Valid)
        {
            throw new InvalidOperationException(
                "A graphics pipeline with procedural vertices cannot bind a vertex buffer.");
        }

        if (buffer.vertexLayout is not null && !buffer.vertexLayout.Equals(pipeline.vertexLayout))
        {
            throw new InvalidOperationException("Vertex buffer layout does not match the bound graphics pipeline.");
        }

        uint first = checked((uint)firstVertex);
        uint count = checked((uint)vertexCount);
        if (buffer.kind == BgfxBufferKind.Vertex)
        {
            bgfx.VertexBufferHandle handle = new() { idx = buffer.nativeIndex };
            if (buffer.vertexLayout is null)
            {
                bgfx.encoder_set_vertex_buffer_with_layout(
                    m_encoder,
                    0,
                    handle,
                    first,
                    count,
                    pipeline.vertexLayoutHandle);
            }
            else
            {
                bgfx.encoder_set_vertex_buffer(m_encoder, 0, handle, first, count);
            }

            return;
        }

        if (buffer.kind == BgfxBufferKind.DynamicVertex)
        {
            bgfx.DynamicVertexBufferHandle handle = new() { idx = buffer.nativeIndex };
            if (buffer.vertexLayout is null)
            {
                bgfx.encoder_set_dynamic_vertex_buffer_with_layout(
                    m_encoder,
                    0,
                    handle,
                    first,
                    count,
                    pipeline.vertexLayoutHandle);
            }
            else
            {
                bgfx.encoder_set_dynamic_vertex_buffer(m_encoder, 0, handle, first, count);
            }

            return;
        }

        throw new InvalidOperationException("The bound resource is not a vertex buffer.");
    }

    private void SetIndexBuffer(
        BgfxBufferResource buffer,
        int firstIndex,
        int indexCount
    ) {
        ValidateRange(firstIndex, indexCount, buffer.descriptor.elementCount, nameof(indexCount));
        uint first = checked((uint)firstIndex);
        uint count = checked((uint)indexCount);
        if (buffer.kind == BgfxBufferKind.Index)
        {
            bgfx.encoder_set_index_buffer(
                m_encoder,
                new bgfx.IndexBufferHandle { idx = buffer.nativeIndex },
                first,
                count);
            return;
        }

        if (buffer.kind == BgfxBufferKind.DynamicIndex)
        {
            bgfx.encoder_set_dynamic_index_buffer(
                m_encoder,
                new bgfx.DynamicIndexBufferHandle { idx = buffer.nativeIndex },
                first,
                count);
            return;
        }

        throw new InvalidOperationException("The bound resource is not an index buffer.");
    }

}
