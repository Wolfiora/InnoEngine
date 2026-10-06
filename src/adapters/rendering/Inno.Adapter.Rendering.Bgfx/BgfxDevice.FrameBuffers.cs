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
    private void ConfigureViewTarget(
        ushort viewId,
        CompiledRenderPass pass
    ) {
        int width = m_backbufferWidth;
        int height = m_backbufferHeight;
        bgfx.ClearFlags clearFlags = 0;
        uint clearColor = 0;
        float clearDepth = 1f;
        byte clearStencil = 0;

        if (pass.surface.isValid)
        {
            BgfxWindowSurfaceResource surface = ResolveSurface(pass.surface);
            width = surface.width;
            height = surface.height;
            bgfx.set_view_frame_buffer(viewId, surface.frameBuffer);
        }
        else if (pass.attachments.Count != 0)
        {
            bgfx.FrameBufferHandle cachedFrameBuffer = FindCachedFrameBuffer(pass);
            if (cachedFrameBuffer.Valid)
            {
                bgfx.set_view_frame_buffer(viewId, cachedFrameBuffer);
            }
            else
            {
                RetireSupersededFrameBuffer(pass.name);
                bgfx.Attachment* attachments = stackalloc bgfx.Attachment[pass.attachments.Count];
                GraphAttachmentSignature[] signature = new GraphAttachmentSignature[pass.attachments.Count];
                for (int index = 0; index < pass.attachments.Count; index++)
                {
                    CompiledRenderAttachment attachment = pass.attachments[index];
                    bgfx.TextureHandle nativeTexture = ResolveTexture(attachment.texture);
                    signature[index] = new GraphAttachmentSignature(
                        nativeTexture.idx,
                        attachment.slot,
                        attachment.isDepth,
                        attachment.mipLevel,
                        attachment.arrayLayer);
                    bgfx.attachment_init(
                        &attachments[index],
                        nativeTexture,
                        bgfx.Access.Write,
                        checked((ushort)attachment.arrayLayer),
                        1,
                        checked((ushort)attachment.mipLevel),
                        (byte)bgfx.ResolveFlags.None);
                }

                bgfx.FrameBufferHandle frameBuffer = bgfx.create_frame_buffer_from_attachment(
                    checked((byte)pass.attachments.Count),
                    attachments,
                    false);
                if (!frameBuffer.Valid)
                {
                    throw new InvalidOperationException($"BGFX could not create framebuffer for pass '{pass.name}'.");
                }

                m_transientFrameBufferAllocationCount++;
                m_graphFrameBufferCache.Add(new CachedGraphFrameBuffer(
                    frameBuffer,
                    pass.name,
                    signature,
                    m_backendFrame));
                m_graphFrameBuffers.Add(frameBuffer);
                bgfx.set_view_frame_buffer(viewId, frameBuffer);
            }

            ApplyAttachmentState(
                pass,
                ref width,
                ref height,
                ref clearFlags,
                ref clearColor,
                ref clearDepth,
                ref clearStencil);
        }
        else
        {
            bgfx.set_view_frame_buffer(viewId, new bgfx.FrameBufferHandle { idx = ushort.MaxValue });
        }


        if (pass.clearsPresentationTarget)
        {
            clearFlags |= bgfx.ClearFlags.Color;
            clearColor = PackColor(pass.presentationClearColor);
        }

        bgfx.set_view_rect(
            viewId,
            0,
            0,
            checked((ushort)width),
            checked((ushort)height));
        bgfx.set_view_clear(viewId, (ushort)clearFlags, clearColor, clearDepth, clearStencil);
    }

    private bgfx.FrameBufferHandle FindCachedFrameBuffer(CompiledRenderPass pass)
    {
        for (int cacheIndex = m_graphFrameBufferCache.Count - 1; cacheIndex >= 0; cacheIndex--)
        {
            CachedGraphFrameBuffer cached = m_graphFrameBufferCache[cacheIndex];
            if (cached.attachments.Length != pass.attachments.Count)
                continue;

            bool matches = true;
            for (int attachmentIndex = 0; attachmentIndex < pass.attachments.Count; attachmentIndex++)
            {
                CompiledRenderAttachment attachment = pass.attachments[attachmentIndex];
                bgfx.TextureHandle texture = ResolveTexture(attachment.texture);
                GraphAttachmentSignature signature = cached.attachments[attachmentIndex];
                if (signature.textureIndex != texture.idx
                    || signature.slot != attachment.slot
                    || signature.isDepth != attachment.isDepth
                    || signature.mipLevel != attachment.mipLevel
                    || signature.arrayLayer != attachment.arrayLayer)
                {
                    matches = false;
                    break;
                }
            }

            if (!matches)
                continue;

            cached.lastUsedFrame = m_backendFrame;
            return cached.handle;
        }

        return new bgfx.FrameBufferHandle { idx = ushort.MaxValue };
    }

    private void RetireSupersededFrameBuffer(string passName)
    {
        for (int index = m_graphFrameBufferCache.Count - 1; index >= 0; index--)
        {
            CachedGraphFrameBuffer cached = m_graphFrameBufferCache[index];
            if (!string.Equals(cached.passName, passName, StringComparison.Ordinal)
                || cached.lastUsedFrame == m_backendFrame)
            {
                continue;
            }

            // BGFX owns the native command retirement and releases handles after frame
            // advancement. Avoid adding our persistent-resource delay to obsolete bindings.
            bgfx.destroy_frame_buffer(cached.handle);
            m_graphFrameBufferCache.RemoveAt(index);
        }
    }

    private void ApplyAttachmentState(
        CompiledRenderPass pass,
        ref int width,
        ref int height,
        ref bgfx.ClearFlags clearFlags,
        ref uint clearColor,
        ref float clearDepth,
        ref byte clearStencil
    ) {
        for (int index = 0; index < pass.attachments.Count; index++)
        {
            CompiledRenderAttachment attachment = pass.attachments[index];
            RenderTextureDescriptor descriptor = ResolveTextureDescriptor(attachment.texture);
            width = Math.Max(1, descriptor.width >> attachment.mipLevel);
            height = Math.Max(1, descriptor.height >> attachment.mipLevel);

            if (attachment.loadAction == RenderLoadAction.Clear)
            {
                if (attachment.isDepth)
                {
                    clearFlags |= bgfx.ClearFlags.Depth;
                    if (descriptor.format == RenderTextureFormat.Depth24Stencil8)
                        clearFlags |= bgfx.ClearFlags.Stencil;
                    clearDepth = attachment.clearDepth;
                    clearStencil = attachment.clearStencil;
                }
                else
                {
                    clearFlags |= bgfx.ClearFlags.Color;
                    clearColor = PackColor(attachment.clearColor);
                }
            }

            if (attachment.storeAction == RenderStoreAction.Discard)
            {
                clearFlags |= attachment.isDepth
                    ? bgfx.ClearFlags.DiscardDepth
                    : ColorDiscardFlag(attachment.slot);
            }
        }
    }

    private void RemoveCachedFrameBuffersReferencing(ushort textureIndex)
    {
        for (int index = m_graphFrameBufferCache.Count - 1; index >= 0; index--)
        {
            CachedGraphFrameBuffer cached = m_graphFrameBufferCache[index];
            if (!cached.ContainsTexture(textureIndex))
                continue;
            EnqueueDestroy(DeferredResource.ForFrameBuffer(cached.handle));
            m_graphFrameBufferCache.RemoveAt(index);
        }
    }

    private readonly record struct GraphAttachmentSignature(
        ushort textureIndex,
        int slot,
        bool isDepth,
        int mipLevel,
        int arrayLayer
    );

    private sealed class CachedGraphFrameBuffer(
        bgfx.FrameBufferHandle handle,
        string passName,
        GraphAttachmentSignature[] attachments,
        uint lastUsedFrame
    ) {
        internal bgfx.FrameBufferHandle handle { get; } = handle;
        internal string passName { get; } = passName;
        internal GraphAttachmentSignature[] attachments { get; } = attachments;
        internal uint lastUsedFrame { get; set; } = lastUsedFrame;

        internal bool ContainsTexture(ushort textureIndex)
        {
            foreach (GraphAttachmentSignature attachment in attachments)
            {
                if (attachment.textureIndex == textureIndex)
                    return true;
            }

            return false;
        }
    }

    private static bgfx.ClearFlags ColorDiscardFlag(int slot)
        => slot switch
        {
            0 => bgfx.ClearFlags.DiscardColor0,
            1 => bgfx.ClearFlags.DiscardColor1,
            2 => bgfx.ClearFlags.DiscardColor2,
            3 => bgfx.ClearFlags.DiscardColor3,
            4 => bgfx.ClearFlags.DiscardColor4,
            5 => bgfx.ClearFlags.DiscardColor5,
            6 => bgfx.ClearFlags.DiscardColor6,
            7 => bgfx.ClearFlags.DiscardColor7,
            _ => throw new ArgumentOutOfRangeException(nameof(slot))
        };

}
