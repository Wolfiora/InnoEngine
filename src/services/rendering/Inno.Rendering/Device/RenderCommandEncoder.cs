using System;

namespace Inno.Rendering;

/// <summary>
/// Provides backend-neutral draw, dispatch and copy commands for one compiled pass.
/// </summary>
public abstract class RenderCommandEncoder
{
    private ulong m_frameIndex;
    private bool m_hasFrameIndex;

    /// <summary>
    /// Selects the graphics pipeline for subsequent draw commands.
    /// </summary>
    /// <param name="pipeline">
    /// Graphics pipeline owned by the current device generation.
    /// </param>
    public abstract void BindGraphicsPipeline(GraphicsPipelineHandle pipeline);

    /// <summary>
    /// Selects the compute pipeline for subsequent dispatch commands.
    /// </summary>
    /// <param name="pipeline">
    /// Compute pipeline owned by the current device generation.
    /// </param>
    public abstract void BindComputePipeline(ComputePipelineHandle pipeline);

    /// <summary>
    /// Binds a graph texture to a shader interface slot.
    /// </summary>
    /// <param name="binding">
    /// Stable shader binding identifier.
    /// </param>
    /// <param name="texture">
    /// Graph texture to bind.
    /// </param>
    public void BindTexture(
        RenderBindingID binding,
        RenderTextureHandle texture
    ) => BindTexture(binding, texture, RenderSamplerState.linearClamp);

    /// <summary>
    /// Binds a persistent texture to a shader interface slot.
    /// </summary>
    /// <param name="binding">
    /// Stable shader binding identifier.
    /// </param>
    /// <param name="texture">
    /// Persistent texture owned by the active device generation.
    /// </param>
    public void BindTexture(
        RenderBindingID binding,
        PersistentTextureHandle texture
    ) => BindTexture(binding, texture, RenderSamplerState.linearClamp);

    /// <summary>
    /// Binds a sampled texture and its sampler to a shader binding.
    /// </summary>
    /// <param name="binding">
    /// Stable shader binding declared by the selected pipeline.
    /// </param>
    /// <param name="texture">
    /// Frame-scoped texture owned by the current graph generation.
    /// </param>
    /// <param name="sampler">
    /// Sampling state copied by value.
    /// </param>
    public abstract void BindTexture(
        RenderBindingID binding,
        RenderTextureHandle texture,
        RenderSamplerState sampler
    );

    /// <summary>
    /// Binds a sampled texture and its sampler to a shader binding.
    /// </summary>
    /// <param name="binding">
    /// Stable shader binding declared by the selected pipeline.
    /// </param>
    /// <param name="texture">
    /// Persistent texture owned by the current device generation.
    /// </param>
    /// <param name="sampler">
    /// Sampling state copied by value.
    /// </param>
    public abstract void BindTexture(
        RenderBindingID binding,
        PersistentTextureHandle texture,
        RenderSamplerState sampler
    );

    /// <summary>
    /// Binds a texture subresource for shader storage access.
    /// </summary>
    /// <param name="binding">
    /// Stable shader binding declared by the selected pipeline.
    /// </param>
    /// <param name="texture">
    /// Frame-scoped texture owned by the current graph generation.
    /// </param>
    /// <param name="mipLevel">
    /// Zero-based mip level used for storage access.
    /// </param>
    public abstract void BindStorageTexture(
        RenderBindingID binding,
        RenderTextureHandle texture,
        int mipLevel = 0
    );

    /// <summary>
    /// Binds a texture subresource for shader storage access.
    /// </summary>
    /// <param name="binding">
    /// Stable shader binding declared by the selected pipeline.
    /// </param>
    /// <param name="texture">
    /// Persistent texture owned by the current device generation.
    /// </param>
    /// <param name="mipLevel">
    /// Zero-based mip level used for storage access.
    /// </param>
    public abstract void BindStorageTexture(
        RenderBindingID binding,
        PersistentTextureHandle texture,
        int mipLevel = 0
    );

    /// <summary>
    /// Binds a storage buffer to a shader binding.
    /// </summary>
    /// <param name="binding">
    /// Stable shader binding declared by the selected pipeline.
    /// </param>
    /// <param name="buffer">
    /// Frame-scoped buffer owned by the current graph generation.
    /// </param>
    public abstract void BindBuffer(
        RenderBindingID binding,
        RenderBufferHandle buffer
    );

    /// <summary>
    /// Binds a storage buffer to a shader binding.
    /// </summary>
    /// <param name="binding">
    /// Stable shader binding declared by the selected pipeline.
    /// </param>
    /// <param name="buffer">
    /// Persistent buffer owned by the current device generation.
    /// </param>
    public abstract void BindBuffer(
        RenderBindingID binding,
        PersistentBufferHandle buffer
    );

    /// <summary>
    /// Binds a frame-uploaded storage buffer to a shader interface slot.
    /// </summary>
    /// <param name="binding">
    /// Stable shader binding identifier.
    /// </param>
    /// <param name="buffer">
    /// Current-frame storage slice.
    /// </param>
    public void BindBuffer(
        RenderBindingID binding,
        RenderBufferSlice buffer
    ) {
        ValidateSlice(buffer, RenderBufferUsage.Storage);
        if (buffer.firstElement != 0)
        {
            throw new ArgumentException(
                "Storage upload slices must begin at the first backing element.",
                nameof(buffer)
            );
        }
        BindBuffer(binding, buffer.buffer);
    }

    /// <summary>
    /// Sets the byte value of a shader uniform.
    /// </summary>
    /// <param name="binding">
    /// Stable shader binding declared by the selected pipeline.
    /// </param>
    /// <param name="value">
    /// Bytes borrowed for this call. An encoder that defers execution must copy them before returning.
    /// </param>
    public abstract void SetUniform(
        RenderBindingID binding,
        ReadOnlySpan<byte> value
    );

    /// <summary>
    /// Sets the column-major object transform for subsequent draws.
    /// </summary>
    /// <param name="columnMajorMatrix">
    /// Matrix values borrowed for this call. An encoder that defers execution must copy them before returning.
    /// </param>
    public abstract void SetTransform(ReadOnlySpan<float> columnMajorMatrix);

    /// <summary>
    /// Sets the raster configuration for subsequent draws.
    /// </summary>
    /// <param name="state">
    /// Immutable raster configuration applied to subsequent draws.
    /// </param>
    public abstract void SetRasterState(RenderRasterState state);

    /// <summary>
    /// Sets the stencil configuration for subsequent draws.
    /// </summary>
    /// <param name="state">
    /// Immutable stencil configuration applied to subsequent draws.
    /// </param>
    public abstract void SetStencil(RenderStencilState state);

    /// <summary>
    /// Sets the viewport rectangle in render-target pixels.
    /// </summary>
    /// <param name="x">
    /// Left coordinate in render-target pixels.
    /// </param>
    /// <param name="y">
    /// Top coordinate in render-target pixels.
    /// </param>
    /// <param name="width">
    /// Rectangle width in render-target pixels.
    /// </param>
    /// <param name="height">
    /// Rectangle height in render-target pixels.
    /// </param>
    public abstract void SetViewport(
        int x,
        int y,
        int width,
        int height
    );

    /// <summary>
    /// Sets the scissor rectangle in render-target pixels.
    /// </summary>
    /// <param name="x">
    /// Left coordinate in render-target pixels.
    /// </param>
    /// <param name="y">
    /// Top coordinate in render-target pixels.
    /// </param>
    /// <param name="width">
    /// Rectangle width in render-target pixels.
    /// </param>
    /// <param name="height">
    /// Rectangle height in render-target pixels.
    /// </param>
    public abstract void SetScissor(
        int x,
        int y,
        int width,
        int height
    );

    /// <summary>
    /// Binds a frame-uploaded vertex slice.
    /// </summary>
    /// <param name="buffer">
    /// Current-frame vertex slice.
    /// </param>
    public void BindVertexBuffer(RenderBufferSlice buffer)
    {
        ValidateSlice(buffer, RenderBufferUsage.Vertex);
        BindVertexBuffer(buffer.buffer, buffer.firstElement);
    }

    /// <summary>
    /// Selects a vertex buffer and its first vertex for subsequent draws.
    /// </summary>
    /// <param name="buffer">
    /// Frame-scoped buffer owned by the current graph generation.
    /// </param>
    /// <param name="firstVertex">
    /// Zero-based vertex offset in the bound buffer.
    /// </param>
    public abstract void BindVertexBuffer(
        RenderBufferHandle buffer,
        int firstVertex = 0
    );

    /// <summary>
    /// Selects a vertex buffer and its first vertex for subsequent draws.
    /// </summary>
    /// <param name="buffer">
    /// Persistent buffer owned by the current device generation.
    /// </param>
    /// <param name="firstVertex">
    /// Zero-based vertex offset in the bound buffer.
    /// </param>
    public abstract void BindVertexBuffer(
        PersistentBufferHandle buffer,
        int firstVertex = 0
    );

    /// <summary>
    /// Binds a frame-uploaded index slice.
    /// </summary>
    /// <param name="buffer">
    /// Current-frame index slice.
    /// </param>
    public void BindIndexBuffer(RenderBufferSlice buffer)
    {
        ValidateSlice(buffer, RenderBufferUsage.Index);
        BindIndexBuffer(buffer.buffer, buffer.firstElement);
    }

    /// <summary>
    /// Selects an index buffer and its first index for subsequent indexed draws.
    /// </summary>
    /// <param name="buffer">
    /// Frame-scoped buffer owned by the current graph generation.
    /// </param>
    /// <param name="firstIndex">
    /// Zero-based index offset in the bound buffer.
    /// </param>
    public abstract void BindIndexBuffer(
        RenderBufferHandle buffer,
        int firstIndex = 0
    );

    /// <summary>
    /// Selects an index buffer and its first index for subsequent indexed draws.
    /// </summary>
    /// <param name="buffer">
    /// Persistent buffer owned by the current device generation.
    /// </param>
    /// <param name="firstIndex">
    /// Zero-based index offset in the bound buffer.
    /// </param>
    public abstract void BindIndexBuffer(
        PersistentBufferHandle buffer,
        int firstIndex = 0
    );

    /// <summary>
    /// Binds a frame-uploaded vertex slice as per-instance input.
    /// </summary>
    /// <param name="buffer">
    /// Current-frame instance records.
    /// </param>
    public void BindInstanceBuffer(RenderBufferSlice buffer)
    {
        ValidateSlice(buffer, RenderBufferUsage.Vertex);
        BindInstanceBuffer(buffer.buffer, buffer.firstElement, buffer.elementCount);
    }

    /// <summary>
    /// Selects the buffer range supplying per-instance attributes.
    /// </summary>
    /// <param name="buffer">
    /// Frame-scoped buffer owned by the current graph generation.
    /// </param>
    /// <param name="firstInstance">
    /// Zero-based offset in the instance data.
    /// </param>
    /// <param name="instanceCount">
    /// Number of instances to submit.
    /// </param>
    public abstract void BindInstanceBuffer(
        RenderBufferHandle buffer,
        int firstInstance,
        int instanceCount
    );

    /// <summary>
    /// Selects the buffer range supplying per-instance attributes.
    /// </summary>
    /// <param name="buffer">
    /// Persistent buffer owned by the current device generation.
    /// </param>
    /// <param name="firstInstance">
    /// Zero-based offset in the instance data.
    /// </param>
    /// <param name="instanceCount">
    /// Number of instances to submit.
    /// </param>
    public abstract void BindInstanceBuffer(
        PersistentBufferHandle buffer,
        int firstInstance,
        int instanceCount
    );

    /// <summary>
    /// Submits a draw using the currently bound pipeline and resources.
    /// </summary>
    /// <param name="vertexCount">
    /// Number of vertices to submit.
    /// </param>
    /// <param name="instanceCount">
    /// Number of instances to submit.
    /// </param>
    public abstract void Draw(
        int vertexCount,
        int instanceCount = 1
    );

    /// <summary>
    /// Submits a draw whose vertices are produced without a vertex buffer.
    /// </summary>
    /// <param name="vertexCount">
    /// Number of vertices to submit.
    /// </param>
    /// <param name="instanceCount">
    /// Number of instances to submit.
    /// </param>
    public abstract void DrawProcedural(
        int vertexCount,
        int instanceCount = 1
    );

    /// <summary>
    /// Submits an indexed draw using the currently bound index buffer.
    /// </summary>
    /// <param name="indexCount">
    /// Number of indices to submit.
    /// </param>
    /// <param name="instanceCount">
    /// Number of instances to submit.
    /// </param>
    public abstract void DrawIndexed(
        int indexCount,
        int instanceCount = 1
    );

    /// <summary>
    /// Submits draws described by consecutive commands in an indirect buffer.
    /// </summary>
    /// <param name="buffer">
    /// Frame-scoped buffer owned by the current graph generation.
    /// </param>
    /// <param name="firstCommand">
    /// Zero-based command offset in the indirect buffer.
    /// </param>
    /// <param name="commandCount">
    /// Number of consecutive indirect commands to submit.
    /// </param>
    public abstract void DrawIndirect(
        RenderBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    );

    /// <summary>
    /// Submits draws described by consecutive commands in an indirect buffer.
    /// </summary>
    /// <param name="buffer">
    /// Persistent buffer owned by the current device generation.
    /// </param>
    /// <param name="firstCommand">
    /// Zero-based command offset in the indirect buffer.
    /// </param>
    /// <param name="commandCount">
    /// Number of consecutive indirect commands to submit.
    /// </param>
    public abstract void DrawIndirect(
        PersistentBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    );

    /// <summary>
    /// Submits compute work with the specified workgroup counts.
    /// </summary>
    /// <param name="groupCountX">
    /// Number of workgroups along the X axis.
    /// </param>
    /// <param name="groupCountY">
    /// Number of workgroups along the Y axis.
    /// </param>
    /// <param name="groupCountZ">
    /// Number of workgroups along the Z axis.
    /// </param>
    public abstract void Dispatch(
        int groupCountX,
        int groupCountY = 1,
        int groupCountZ = 1
    );

    /// <summary>
    /// Submits compute work described by consecutive commands in an indirect buffer.
    /// </summary>
    /// <param name="buffer">
    /// Frame-scoped buffer owned by the current graph generation.
    /// </param>
    /// <param name="firstCommand">
    /// Zero-based command offset in the indirect buffer.
    /// </param>
    /// <param name="commandCount">
    /// Number of consecutive indirect commands to submit.
    /// </param>
    public abstract void DispatchIndirect(
        RenderBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    );

    /// <summary>
    /// Submits compute work described by consecutive commands in an indirect buffer.
    /// </summary>
    /// <param name="buffer">
    /// Persistent buffer owned by the current device generation.
    /// </param>
    /// <param name="firstCommand">
    /// Zero-based command offset in the indirect buffer.
    /// </param>
    /// <param name="commandCount">
    /// Number of consecutive indirect commands to submit.
    /// </param>
    public abstract void DispatchIndirect(
        PersistentBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    );

    /// <summary>
    /// Copies texture content without transferring resource ownership.
    /// </summary>
    /// <param name="source">
    /// Frame-scoped source texture.
    /// </param>
    /// <param name="destination">
    /// Frame-scoped destination texture.
    /// </param>
    public abstract void CopyTexture(
        RenderTextureHandle source,
        RenderTextureHandle destination
    );

    /// <summary>
    /// Copies a source texture region into a destination texture region.
    /// </summary>
    /// <param name="source">
    /// Frame-scoped source texture.
    /// </param>
    /// <param name="sourceRegion">
    /// Source texture subresource and texel box.
    /// </param>
    /// <param name="destination">
    /// Frame-scoped destination texture.
    /// </param>
    /// <param name="destinationRegion">
    /// Destination texture subresource and texel box.
    /// </param>
    public abstract void BlitTexture(
        RenderTextureHandle source,
        RenderTextureRegion sourceRegion,
        RenderTextureHandle destination,
        RenderTextureRegion destinationRegion
    );

    /// <summary>
    /// Copies buffer content without transferring resource ownership.
    /// </summary>
    /// <param name="source">
    /// Frame-scoped source buffer.
    /// </param>
    /// <param name="destination">
    /// Frame-scoped destination buffer.
    /// </param>
    public abstract void CopyBuffer(
        RenderBufferHandle source,
        RenderBufferHandle destination
    );

    internal void SetFrameIndex(ulong frameIndex)
    {
        m_frameIndex = frameIndex;
        m_hasFrameIndex = true;
    }

    private void ValidateSlice(
        RenderBufferSlice buffer,
        RenderBufferUsage requiredUsage
    ) {
        if (!buffer.isValid)
        {
            throw new ArgumentException("A frame buffer slice must be valid.", nameof(buffer));
        }
        if (!m_hasFrameIndex || buffer.frameIndex != m_frameIndex)
        {
            throw new ArgumentException(
                "A frame buffer slice cannot be used outside the frame that uploaded it.",
                nameof(buffer)
            );
        }
        if ((buffer.usage & requiredUsage) == 0)
        {
            throw new ArgumentException(
                $"The frame buffer slice does not declare {requiredUsage} usage.",
                nameof(buffer)
            );
        }
    }
}
