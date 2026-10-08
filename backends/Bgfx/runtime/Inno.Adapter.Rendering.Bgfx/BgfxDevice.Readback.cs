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
    /// Schedules asynchronous texture readback into caller-provided destination storage.
    /// </summary>
    /// <param name="texture">
    /// The texture consumed by begin texture readback; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="mipLevel">
    /// The mip level consumed by begin texture readback; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// The validated render texture readback handle that represents the completed operation.
    /// </returns>
    public RenderTextureReadbackHandle BeginTextureReadback(
        PersistentTextureHandle texture,
        int mipLevel = 0
    ) {
        EnsureFrameSafetyPoint();
        ArgumentOutOfRangeException.ThrowIfNegative(mipLevel);
        ValidatePersistentHandle(texture);
        if (!capabilities.Supports(GraphicsCapability.TextureReadback))
            throw new NotSupportedException("The active BGFX renderer does not support texture readback.");
        if (!m_persistentTextures.TryGetValue(GetHandleIdentity(texture).value, out bgfx.TextureHandle nativeTexture)
            || !m_persistentTextureDescriptors.TryGetValue(GetHandleIdentity(texture).value, out RenderTextureDescriptor? descriptor))
        {
            throw new ArgumentException("Persistent texture is not active on this device.", nameof(texture));
        }
        if ((descriptor.usage & RenderTextureUsage.Readback) == 0)
            throw new ArgumentException("Texture was not created for readback.", nameof(texture));
        if (mipLevel >= descriptor.mipCount)
            throw new ArgumentOutOfRangeException(nameof(mipLevel));
        int width = Math.Max(1, descriptor.width >> mipLevel);
        int height = Math.Max(1, descriptor.height >> mipLevel);
        int layers = descriptor.GetSubresourceLayerCount(mipLevel);
        int rowPitch = checked(width * BytesPerPixel(descriptor.format));
        int byteCount = checked(rowPitch * height * layers);
        void* data = NativeMemory.Alloc(checked((nuint)byteCount));
        uint readyFrame;
        try
        {
            readyFrame = bgfx.read_texture(nativeTexture, data, checked((byte)mipLevel));
        }
        catch
        {
            NativeMemory.Free(data);
            throw;
        }
        ulong id = m_nextReadbackId++;
        m_textureReadbacks.Add(
            id,
            new PendingTextureReadback(
                descriptor,
                mipLevel,
                rowPitch,
                byteCount,
                (nint)data,
                readyFrame));
        return CreateRenderTextureReadbackHandle(id, generation);
    }

    /// <summary>
    /// Attempts to get texture readback without changing state when the operation cannot complete.
    /// </summary>
    /// <param name="readback">
    /// The readback consumed by try get texture readback; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="result">
    /// The result produced or completed by the preceding operation.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetTextureReadback(
        RenderTextureReadbackHandle readback,
        out RenderTextureReadbackResult? result
    ) {
        EnsureFrameSafetyPoint();
        ValidateReadbackHandle(readback);
        if (!m_textureReadbacks.TryGetValue(GetHandleIdentity(readback).value, out PendingTextureReadback? pending))
            throw new ArgumentException("Texture readback is not active on this device.", nameof(readback));
        result = null;
        if (m_backendFrame < pending.readyFrame)
            return false;
        m_textureReadbacks.Remove(GetHandleIdentity(readback).value);
        try
        {
            if (pending.canceled)
                return false;
            byte[] bytes = new byte[pending.byteCount];
            Marshal.Copy(pending.data, bytes, 0, bytes.Length);
            result = new RenderTextureReadbackResult(
                pending.descriptor,
                pending.mipLevel,
                pending.rowPitch,
                bytes);
            return true;
        }
        finally
        {
            NativeMemory.Free((void*)pending.data);
        }
    }

    /// <summary>
    /// Cancels a pending texture readback and releases its retained state.
    /// </summary>
    /// <param name="readback">
    /// The readback consumed by cancel texture readback; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void CancelTextureReadback(RenderTextureReadbackHandle readback)
    {
        EnsureFrameSafetyPoint();
        ValidateReadbackHandle(readback);
        if (m_textureReadbacks.TryGetValue(GetHandleIdentity(readback).value, out PendingTextureReadback? pending))
            pending.canceled = true;
    }

    private void ProcessCanceledReadbacks()
    {
        foreach ((ulong id, PendingTextureReadback pending) in m_textureReadbacks.ToArray())
        {
            if (!pending.canceled || m_backendFrame < pending.readyFrame)
                continue;
            NativeMemory.Free((void*)pending.data);
            m_textureReadbacks.Remove(id);
        }
    }

    private void ValidateReadbackHandle(RenderTextureReadbackHandle readback)
    {
        if (!readback.isValid || GetHandleIdentity(readback).generation != generation)
            throw new ArgumentException("Texture readback belongs to another device generation.", nameof(readback));
    }

    private sealed class PendingTextureReadback(
        RenderTextureDescriptor descriptor,
        int mipLevel,
        int rowPitch,
        int byteCount,
        nint data,
        uint readyFrame
    ) {
        internal RenderTextureDescriptor descriptor { get; } = descriptor;
        internal int mipLevel { get; } = mipLevel;
        internal int rowPitch { get; } = rowPitch;
        internal int byteCount { get; } = byteCount;
        internal nint data { get; } = data;
        internal uint readyFrame { get; } = readyFrame;
        internal bool canceled { get; set; }
    }

}
