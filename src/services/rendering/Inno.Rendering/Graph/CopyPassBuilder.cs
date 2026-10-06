using System;
using System.Collections.Generic;

namespace Inno.Rendering;

/// <summary>
/// Declares explicit resource copy access.
/// </summary>
public sealed class CopyPassBuilder : RenderPassBuilder
{
    internal CopyPassBuilder(
        RenderGraphBuilder graph,
        RenderPassRecord pass
    )
        : base(graph, pass) { }

    /// <summary>
    /// Declares one texture copy operation.
    /// </summary>
    /// <param name="source">
    /// Copy source.
    /// </param>
    /// <param name="destination">
    /// Copy destination.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public CopyPassBuilder CopyTexture(
        RenderTextureHandle source,
        RenderTextureHandle destination
    ) {
        graph.AddUse(
            pass,
            source,
            RenderResourceAccess.Read,
            RenderResourceUseKind.CopySource);
        graph.AddUse(
            pass,
            destination,
            RenderResourceAccess.Write,
            RenderResourceUseKind.CopyDestination);
        return this;
    }

    /// <summary>
    /// Declares one buffer copy operation.
    /// </summary>
    /// <param name="source">
    /// Copy source.
    /// </param>
    /// <param name="destination">
    /// Copy destination.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public CopyPassBuilder CopyBuffer(
        RenderBufferHandle source,
        RenderBufferHandle destination
    ) {
        graph.AddUse(
            pass,
            source,
            RenderResourceAccess.Read,
            RenderResourceUseKind.CopySource);
        graph.AddUse(
            pass,
            destination,
            RenderResourceAccess.Write,
            RenderResourceUseKind.CopyDestination);
        return this;
    }
}
