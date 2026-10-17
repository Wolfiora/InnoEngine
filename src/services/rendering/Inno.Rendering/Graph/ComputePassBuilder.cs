using System;
using System.Collections.Generic;

namespace Inno.Rendering;

/// <summary>
/// Declares compute resource reads and unordered writes.
/// </summary>
public sealed class ComputePassBuilder : RenderPassBuilder
{
    internal ComputePassBuilder(
        RenderGraphBuilder graph,
        RenderPassRecord pass
    )
        : base(graph, pass) { }

    /// <summary>
    /// Sets backend-ready column-major transform matrices exposed to this compute view.
    /// </summary>
    /// <param name="viewMatrix">
    /// Exactly sixteen column-major world-to-view values.
    /// </param>
    /// <param name="projectionMatrix">
    /// Exactly sixteen column-major projection values.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public ComputePassBuilder SetViewTransform(
        ReadOnlySpan<float> viewMatrix,
        ReadOnlySpan<float> projectionMatrix
    ) {
        graph.EnsurePassMutable(pass);
        pass.viewTransform = new RenderViewTransform(viewMatrix, projectionMatrix);
        return this;
    }

    /// <summary>
    /// Declares an unordered texture read.
    /// </summary>
    /// <param name="texture">
    /// Storage texture read by compute work.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public ComputePassBuilder ReadStorageTexture(RenderTextureHandle texture)
    {
        graph.AddUse(
            pass,
            texture,
            RenderResourceAccess.Read,
            RenderResourceUseKind.StorageRead);
        return this;
    }

    /// <summary>
    /// Declares an unordered texture write.
    /// </summary>
    /// <param name="texture">
    /// Storage texture written by compute work.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public ComputePassBuilder WriteStorageTexture(RenderTextureHandle texture)
    {
        graph.AddUse(
            pass,
            texture,
            RenderResourceAccess.Write,
            RenderResourceUseKind.StorageWrite);
        return this;
    }

    /// <summary>
    /// Declares an unordered texture read and write.
    /// </summary>
    /// <param name="texture">
    /// Storage texture read and written by compute work.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public ComputePassBuilder ReadWriteStorageTexture(RenderTextureHandle texture)
    {
        graph.AddUse(
            pass,
            texture,
            RenderResourceAccess.ReadWrite,
            RenderResourceUseKind.StorageReadWrite);
        return this;
    }

    /// <summary>
    /// Declares an unordered buffer read.
    /// </summary>
    /// <param name="buffer">
    /// Storage buffer read by compute work.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public ComputePassBuilder ReadStorageBuffer(RenderBufferHandle buffer)
    {
        graph.AddUse(
            pass,
            buffer,
            RenderResourceAccess.Read,
            RenderResourceUseKind.StorageRead);
        return this;
    }

    /// <summary>
    /// Declares an unordered buffer write.
    /// </summary>
    /// <param name="buffer">
    /// Storage buffer written by compute work.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public ComputePassBuilder WriteStorageBuffer(RenderBufferHandle buffer)
    {
        graph.AddUse(
            pass,
            buffer,
            RenderResourceAccess.Write,
            RenderResourceUseKind.StorageWrite);
        return this;
    }

    /// <summary>
    /// Declares an unordered buffer read and write.
    /// </summary>
    /// <param name="buffer">
    /// Storage buffer read and written by compute work.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public ComputePassBuilder ReadWriteStorageBuffer(RenderBufferHandle buffer)
    {
        graph.AddUse(
            pass,
            buffer,
            RenderResourceAccess.ReadWrite,
            RenderResourceUseKind.StorageReadWrite);
        return this;
    }
}

