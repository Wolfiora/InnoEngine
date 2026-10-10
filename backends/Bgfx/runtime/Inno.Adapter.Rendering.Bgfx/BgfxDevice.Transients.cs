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
    private bgfx.TextureHandle AcquireTransientTexture(
        RenderTextureDescriptor descriptor,
        string name,
        int physicalSlot
    ) {
        for (int index = m_transientTexturePool.Count - 1; index >= 0; index--)
        {
            PooledTransientTexture pooled = m_transientTexturePool[index];
            if (pooled.physicalSlot != physicalSlot || !pooled.descriptor.Equals(descriptor))
                continue;
            m_transientTexturePool.RemoveAt(index);
            return pooled.handle;
        }

        for (int index = m_transientTexturePool.Count - 1; index >= 0; index--)
        {
            PooledTransientTexture pooled = m_transientTexturePool[index];
            if (!pooled.descriptor.Equals(descriptor))
                continue;
            m_transientTexturePool.RemoveAt(index);
            return pooled.handle;
        }

        bgfx.TextureHandle texture = CreateNativeTexture(descriptor);
        if (!texture.Valid)
            throw new InvalidOperationException($"BGFX could not allocate transient texture '{name}'.");
        bgfx.set_texture_name(texture, name, Utf8Length(name));
        m_transientTextureAllocationCount++;
        return texture;
    }

    private void ReturnTransientGraphResources()
    {
        foreach ((int slot, bgfx.TextureHandle texture) in m_transientTextureSlots)
        {
            m_transientTexturePool.Add(new PooledTransientTexture(
                m_transientTextureSlotDescriptors[slot],
                texture,
                m_backendFrame,
                slot));
        }

        foreach ((int slot, BgfxBufferResource buffer) in m_transientBufferSlots)
            m_transientBufferPool.Add(new PooledTransientBuffer(buffer, m_backendFrame, slot));

        m_transientTextureSlots.Clear();
        m_transientTextureSlotDescriptors.Clear();
        m_transientBufferSlots.Clear();
    }

    private void TrimTransientResourceCaches()
    {
        for (int index = m_graphFrameBufferCache.Count - 1; index >= 0; index--)
        {
            CachedGraphFrameBuffer cached = m_graphFrameBufferCache[index];
            if (!CacheEntryExpired(cached.lastUsedFrame))
                continue;
            EnqueueDestroy(DeferredResource.ForFrameBuffer(cached.handle));
            m_graphFrameBufferCache.RemoveAt(index);
        }

        for (int index = m_transientTexturePool.Count - 1; index >= 0; index--)
        {
            PooledTransientTexture pooled = m_transientTexturePool[index];
            if (!CacheEntryExpired(pooled.lastUsedFrame))
                continue;
            RemoveCachedFrameBuffersReferencing(pooled.handle.idx);
            EnqueueDestroy(DeferredResource.ForTexture(pooled.handle));
            m_transientTexturePool.RemoveAt(index);
        }

        for (int index = m_transientBufferPool.Count - 1; index >= 0; index--)
        {
            PooledTransientBuffer pooled = m_transientBufferPool[index];
            if (!CacheEntryExpired(pooled.lastUsedFrame))
                continue;
            EnqueueDestroy(DeferredResource.ForBuffer(pooled.resource));
            m_transientBufferPool.RemoveAt(index);
        }
    }

    private bool CacheEntryExpired(uint lastUsedFrame) => unchecked(m_backendFrame - lastUsedFrame) > C_TRANSIENT_CACHE_RETENTION_FRAMES;

    private readonly record struct PooledTransientTexture(
        RenderTextureDescriptor descriptor,
        bgfx.TextureHandle handle,
        uint lastUsedFrame,
        int physicalSlot
    );

    private readonly record struct PooledTransientBuffer(
        BgfxBufferResource resource,
        uint lastUsedFrame,
        int physicalSlot
    );

}
