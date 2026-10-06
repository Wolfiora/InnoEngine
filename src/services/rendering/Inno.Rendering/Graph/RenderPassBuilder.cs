using System;
using System.Collections.Generic;

namespace Inno.Rendering;

/// <summary>
/// Provides ordering and common resource declarations for one pass.
/// </summary>
public abstract class RenderPassBuilder
{
    private readonly RenderGraphBuilder m_graph;
    private readonly RenderPassRecord m_pass;

    internal RenderPassBuilder(
        RenderGraphBuilder graph,
        RenderPassRecord pass
    ) {
        m_graph = graph;
        m_pass = pass;
    }

    internal RenderGraphBuilder graph => m_graph;
    internal RenderPassRecord pass => m_pass;

    /// <summary>
    /// Orders this pass before every pass in a target phase.
    /// </summary>
    /// <param name="phase">
    /// Target phase.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public RenderPassBuilder Before(RenderPhaseId phase)
    {
        m_graph.EnsurePassMutable(m_pass);
        m_pass.before.Add(phase);
        return this;
    }

    /// <summary>
    /// Orders this pass after every pass in a target phase.
    /// </summary>
    /// <param name="phase">
    /// Target phase.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public RenderPassBuilder After(RenderPhaseId phase)
    {
        m_graph.EnsurePassMutable(m_pass);
        m_pass.after.Add(phase);
        return this;
    }

    /// <summary>
    /// Prevents pass culling because execution has an externally observable effect.
    /// </summary>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public RenderPassBuilder HasSideEffect()
    {
        m_graph.EnsurePassMutable(m_pass);
        m_pass.hasSideEffect = true;
        return this;
    }

    /// <summary>
    /// Opts this pass into worker-thread recording through an isolated backend-neutral command list.
    /// </summary>
    /// <remarks>
    /// Pass data and code reached by the callback must be safe for worker-thread access. GPU execution
    /// and backend view order remain identical to the compiled graph order.
    /// </remarks>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public RenderPassBuilder AllowParallelRecording()
    {
        m_graph.EnsurePassMutable(m_pass);
        m_pass.recordingMode = RenderPassRecordingMode.Parallel;
        return this;
    }

    /// <summary>
    /// Declares a shader read from a texture.
    /// </summary>
    /// <param name="texture">
    /// Texture read by this pass.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public RenderPassBuilder ReadTexture(RenderTextureHandle texture)
    {
        m_graph.AddUse(
            m_pass,
            texture,
            RenderResourceAccess.Read,
            RenderResourceUseKind.GenericRead);
        return this;
    }

    /// <summary>
    /// Declares a shader read from a buffer.
    /// </summary>
    /// <param name="buffer">
    /// Buffer read by this pass.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public RenderPassBuilder ReadBuffer(RenderBufferHandle buffer)
    {
        m_graph.AddUse(
            m_pass,
            buffer,
            RenderResourceAccess.Read,
            RenderResourceUseKind.GenericRead);
        return this;
    }
}

