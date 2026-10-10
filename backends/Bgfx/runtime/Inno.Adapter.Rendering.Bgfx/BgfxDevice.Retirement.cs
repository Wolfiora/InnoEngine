using Inno.Adapter.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Inno.Native.Bgfx;
using Inno.Platform;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

sealed unsafe partial class BgfxDevice
{
    /// <summary>
    /// Shuts down BGFX after releasing all active and queued backend resources.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The caller is not on the owner thread, rendering is active, or retirement cannot complete.
    /// A failed retirement retains the device and its dependencies for diagnosis and a safe retry.
    /// </exception>
    public void Dispose()
    {
        if (m_disposed)
        {
            return;
        }

        EnsureApiThread();
        if (!m_activeEncoder.IsNull || m_activeGraph is not null)
        {
            throw new InvalidOperationException("Cannot dispose BGFX while a render graph or encoder is active.");
        }

        if (m_frameOpen)
        {
            EndFrame();
        }

        ResetPreviousViews();

        foreach (BgfxWindowSurfaceResource surface in m_windowSurfaces.Values)
        {
            surface.retirement.pending.Add(surface.frameBuffer);
            surface.retirement.closing = true;
            if (!m_surfaceRetirements.Contains(surface.retirement))
                m_surfaceRetirements.Add(surface.retirement);
        }
        m_windowSurfaces.Clear();
        DrainWindowSurfaceRetirements();

        foreach (bgfx.TextureHandle texture in m_persistentTextures.Values)
        {
            EnqueueDestroy(DeferredResource.ForTexture(texture));
        }

        foreach (BgfxBufferResource buffer in m_persistentBuffers.Values)
        {
            EnqueueDestroy(DeferredResource.ForBuffer(buffer));
        }

        foreach (BgfxPipelineResource pipeline in m_graphicsPipelines.Values)
        {
            EnqueuePipelineDestroy(pipeline);
            if (pipeline.vertexLayoutHandle.Valid)
            {
                EnqueueDestroy(DeferredResource.ForVertexLayout(pipeline.vertexLayoutHandle));
            }
        }

        foreach (BgfxPipelineResource pipeline in m_computePipelines.Values)
        {
            EnqueuePipelineDestroy(pipeline);
        }

        foreach (CachedGraphFrameBuffer cached in m_graphFrameBufferCache)
        {
            EnqueueDestroy(DeferredResource.ForFrameBuffer(cached.handle));
        }

        foreach (PooledTransientTexture pooled in m_transientTexturePool)
        {
            EnqueueDestroy(DeferredResource.ForTexture(pooled.handle));
        }

        foreach (PooledTransientBuffer pooled in m_transientBufferPool)
        {
            EnqueueDestroy(DeferredResource.ForBuffer(pooled.resource));
        }

        m_persistentTextures.Clear();
        m_persistentTextureDescriptors.Clear();
        m_persistentBuffers.Clear();
        m_graphicsPipelines.Clear();
        m_computePipelines.Clear();
        m_windowSurfaces.Clear();
        m_graphFrameBufferCache.Clear();
        m_transientTexturePool.Clear();
        m_transientBufferPool.Clear();
        DrainDeferredResourcesForShutdown();
        string? closureFailure = GetManagedResourceClosureFailure();
        try
        {
            bgfx.shutdown();
        }
        finally
        {
            foreach (PendingTextureReadback pending in m_textureReadbacks.Values)
                NativeMemory.Free((void*)pending.data);
            m_textureReadbacks.Clear();
            m_disposed = true;
            generation = 0;
            m_processLease.Dispose();
        }

        if (closureFailure is not null)
            throw new InvalidOperationException(closureFailure);
    }

    private void EnqueueDestroy(DeferredResource resource)
        => m_deferredResources.Add(resource with
        {
            eligibleFrame = m_backendFrame
                + checked((uint)m_deferredDestroyFrames)
        });

    private void EnqueuePipelineDestroy(BgfxPipelineResource pipeline) => EnqueueDestroy(DeferredResource.ForProgram(pipeline.program));

    private void ProcessDeferredResources(bool force)
    {
        if (m_deferredResources.Count == 0)
        {
            return;
        }

        List<DeferredResource> pending = [];
        foreach (DeferredResource resource in m_deferredResources)
        {
            if (!force && resource.eligibleFrame > m_backendFrame)
            {
                pending.Add(resource);
                continue;
            }

            switch (resource.kind)
            {
                case DeferredResourceKind.Texture:
                    bgfx.destroy_texture(new bgfx.TextureHandle { idx = resource.index });
                    break;
                case DeferredResourceKind.FrameBuffer:
                    bgfx.destroy_frame_buffer(new bgfx.FrameBufferHandle { idx = resource.index });
                    break;
                case DeferredResourceKind.VertexBuffer:
                    bgfx.destroy_vertex_buffer(new bgfx.VertexBufferHandle { idx = resource.index });
                    break;
                case DeferredResourceKind.IndexBuffer:
                    bgfx.destroy_index_buffer(new bgfx.IndexBufferHandle { idx = resource.index });
                    break;
                case DeferredResourceKind.DynamicVertexBuffer:
                    bgfx.destroy_dynamic_vertex_buffer(new bgfx.DynamicVertexBufferHandle { idx = resource.index });
                    break;
                case DeferredResourceKind.DynamicIndexBuffer:
                    bgfx.destroy_dynamic_index_buffer(new bgfx.DynamicIndexBufferHandle { idx = resource.index });
                    break;
                case DeferredResourceKind.IndirectBuffer:
                    bgfx.destroy_indirect_buffer(new bgfx.IndirectBufferHandle { idx = resource.index });
                    break;
                case DeferredResourceKind.Program:
                    bgfx.destroy_program(new bgfx.ProgramHandle { idx = resource.index });
                    break;
                case DeferredResourceKind.VertexLayout:
                    bgfx.destroy_vertex_layout(new bgfx.VertexLayoutHandle { idx = resource.index });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resource));
            }
        }

        m_deferredResources.Clear();
        m_deferredResources.AddRange(pending);
    }

    private void DrainDeferredResourcesForShutdown()
    {
        while (m_deferredResources.Count != 0)
        {
            bgfx.touch(0);
            m_backendFrame = SubmitNativeFrame(bgfx.FrameFlags.Flush);
            ProcessDeferredResources(force: false);
        }

        for (int frame = 0; frame < m_deferredDestroyFrames; frame++)
        {
            bgfx.touch(0);
            m_backendFrame = SubmitNativeFrame(bgfx.FrameFlags.Flush);
        }
    }

    private string? GetManagedResourceClosureFailure()
    {
        if (!m_activeEncoder.IsNull
            || m_activeGraph is not null
            || m_graphFrameBuffers.Count != 0
            || m_graphTextures.Count != 0
            || m_transientTextureSlots.Count != 0
            || m_transientTextureSlotDescriptors.Count != 0
            || m_transientTexturePool.Count != 0
            || m_graphFrameBufferCache.Count != 0
            || m_graphBuffers.Count != 0
            || m_transientBufferSlots.Count != 0
            || m_transientBufferPool.Count != 0
            || m_persistentTextures.Count != 0
            || m_persistentTextureDescriptors.Count != 0
            || m_persistentBuffers.Count != 0
            || m_graphicsPipelines.Count != 0
            || m_computePipelines.Count != 0
            || m_windowSurfaces.Count != 0
            || m_surfaceRetirements.Count != 0
            || m_deferredResources.Count != 0)
        {
            return "BGFX shutdown detected a non-empty managed resource ownership graph.";
        }

        return null;
    }

    private enum DeferredResourceKind
    {
        Texture,
        FrameBuffer,
        VertexBuffer,
        IndexBuffer,
        DynamicVertexBuffer,
        DynamicIndexBuffer,
        IndirectBuffer,
        Program,
        VertexLayout
    }

    private readonly record struct DeferredResource(
        DeferredResourceKind kind,
        ushort index,
        uint eligibleFrame
    ) {
        /// <summary>
        /// Creates a deferred resource record for the supplied texture handle.
        /// </summary>
        /// <param name="texture">
        /// The texture consumed by for texture; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        /// <returns>
        /// The validated deferred resource that represents the completed operation.
        /// </returns>
        public static DeferredResource ForTexture(bgfx.TextureHandle texture) => new(DeferredResourceKind.Texture, texture.idx, 0);

        /// <summary>
        /// Creates a deferred resource record for the supplied frame buffer handle.
        /// </summary>
        /// <param name="frameBuffer">
        /// The frame buffer consumed by for frame buffer; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        /// <returns>
        /// The validated deferred resource that represents the completed operation.
        /// </returns>
        public static DeferredResource ForFrameBuffer(bgfx.FrameBufferHandle frameBuffer)
            => new(DeferredResourceKind.FrameBuffer, frameBuffer.idx, 0);

        /// <summary>
        /// Creates a deferred resource record for the supplied buffer handle.
        /// </summary>
        /// <param name="buffer">
        /// The buffer consumed by for buffer; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        /// <returns>
        /// The validated deferred resource that represents the completed operation.
        /// </returns>
        public static DeferredResource ForBuffer(BgfxBufferResource buffer)
            => new(buffer.kind switch
            {
                BgfxBufferKind.Vertex => DeferredResourceKind.VertexBuffer,
                BgfxBufferKind.Index => DeferredResourceKind.IndexBuffer,
                BgfxBufferKind.DynamicVertex => DeferredResourceKind.DynamicVertexBuffer,
                BgfxBufferKind.DynamicIndex => DeferredResourceKind.DynamicIndexBuffer,
                BgfxBufferKind.Indirect => DeferredResourceKind.IndirectBuffer,
                _ => throw new ArgumentOutOfRangeException(nameof(buffer))
            }, buffer.nativeIndex, 0);

        /// <summary>
        /// Creates a deferred resource record for the supplied program handle.
        /// </summary>
        /// <param name="program">
        /// The program consumed by for program; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        /// <returns>
        /// The validated deferred resource that represents the completed operation.
        /// </returns>
        public static DeferredResource ForProgram(bgfx.ProgramHandle program) => new(DeferredResourceKind.Program, program.idx, 0);

        /// <summary>
        /// Creates a deferred resource record for the supplied vertex layout handle.
        /// </summary>
        /// <param name="layout">
        /// The layout consumed by for vertex layout; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        /// <returns>
        /// The validated deferred resource that represents the completed operation.
        /// </returns>
        public static DeferredResource ForVertexLayout(bgfx.VertexLayoutHandle layout)
            => new(DeferredResourceKind.VertexLayout, layout.idx, 0);
    }

}
