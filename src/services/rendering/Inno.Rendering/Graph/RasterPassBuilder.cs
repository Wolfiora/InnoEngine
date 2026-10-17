using System;
using System.Collections.Generic;

namespace Inno.Rendering;

/// <summary>
/// Declares raster attachments and resource access.
/// </summary>
public sealed class RasterPassBuilder : RenderPassBuilder
{
    internal RasterPassBuilder(
        RenderGraphBuilder graph,
        RenderPassRecord pass
    )
        : base(graph, pass) { }

    /// <summary>
    /// Sets backend-ready column-major transform matrices for this raster view.
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
    public RasterPassBuilder SetViewTransform(
        ReadOnlySpan<float> viewMatrix,
        ReadOnlySpan<float> projectionMatrix
    ) {
        graph.EnsurePassMutable(pass);
        pass.viewTransform = new RenderViewTransform(viewMatrix, projectionMatrix);
        return this;
    }

    /// <summary>
    /// Directs this pass to a persistent presentation surface instead of the primary backbuffer.
    /// </summary>
    /// <param name="surface">
    /// Surface owned by the active graphics-device generation.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="surface"/> is invalid.
    /// </exception>
    public RasterPassBuilder UseSurface(RenderSurfaceHandle surface)
    {
        if (!surface.isValid)
        {
            throw new ArgumentException("Presentation surface handle is invalid.", nameof(surface));
        }

        graph.EnsurePassMutable(pass);
        pass.surface = surface;
        return this;
    }

    /// <summary>
    /// Clears the primary backbuffer or detached presentation surface before this pass.
    /// </summary>
    /// <param name="clearColor">
    /// Linear color written before draw commands.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public RasterPassBuilder ClearPresentationTarget(RenderClearColor clearColor)
    {
        graph.EnsurePassMutable(pass);
        pass.clearsPresentationTarget = true;
        pass.presentationClearColor = clearColor;
        return this;
    }

    /// <summary>
    /// Attaches a color texture.
    /// </summary>
    /// <param name="texture">
    /// Color attachment texture.
    /// </param>
    /// <param name="slot">
    /// Zero-based color attachment slot.
    /// </param>
    /// <param name="loadAction">
    /// Initial content behavior.
    /// </param>
    /// <param name="storeAction">
    /// Final content behavior.
    /// </param>
    /// <param name="clearColor">
    /// Linear clear value used for <see cref="RenderLoadAction.Clear"/>.
    /// </param>
    /// <param name="mipLevel">
    /// Attached mip level.
    /// </param>
    /// <param name="arrayLayer">
    /// Attached texture-array layer.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public RasterPassBuilder UseColorAttachment(
        RenderTextureHandle texture,
        int slot,
        RenderLoadAction loadAction,
        RenderStoreAction storeAction = RenderStoreAction.Store,
        RenderClearColor clearColor = default,
        int mipLevel = 0,
        int arrayLayer = 0
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(slot);
        ArgumentOutOfRangeException.ThrowIfNegative(mipLevel);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayLayer);
        graph.AddAttachment(pass, new RenderAttachment(
            texture,
            slot,
            false,
            mipLevel,
            arrayLayer,
            loadAction,
            storeAction,
            clearColor,
            1f,
            0));
        return this;
    }

    /// <summary>
    /// Attaches a depth or depth-stencil texture.
    /// </summary>
    /// <param name="texture">
    /// Depth attachment texture.
    /// </param>
    /// <param name="loadAction">
    /// Initial content behavior.
    /// </param>
    /// <param name="storeAction">
    /// Final content behavior.
    /// </param>
    /// <param name="clearDepth">
    /// Depth clear value.
    /// </param>
    /// <param name="clearStencil">
    /// Stencil clear value.
    /// </param>
    /// <param name="mipLevel">
    /// Attached mip level.
    /// </param>
    /// <param name="arrayLayer">
    /// Attached texture-array layer.
    /// </param>
    /// <returns>
    /// This builder for fluent declarations.
    /// </returns>
    public RasterPassBuilder UseDepthAttachment(
        RenderTextureHandle texture,
        RenderLoadAction loadAction,
        RenderStoreAction storeAction = RenderStoreAction.Store,
        float clearDepth = 1f,
        byte clearStencil = 0,
        int mipLevel = 0,
        int arrayLayer = 0
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(mipLevel);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayLayer);
        graph.AddAttachment(pass, new RenderAttachment(
            texture,
            0,
            true,
            mipLevel,
            arrayLayer,
            loadAction,
            storeAction,
            default,
            clearDepth,
            clearStencil));
        return this;
    }
}

