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
    /// Creates a texture using this implementation's validated inputs.
    /// </summary>
    /// <param name="descriptor">
    /// The descriptor consumed by create texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="name">
    /// The human-readable name used for presentation and diagnostics.
    /// </param>
    /// <returns>
    /// The validated persistent texture handle that represents the completed operation.
    /// </returns>
    public PersistentTextureHandle CreateTexture(
        RenderTextureDescriptor descriptor,
        string name
    ) {
        EnsureFrameSafetyPoint();
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        bgfx.TextureHandle nativeTexture = CreateNativeTexture(descriptor);
        if (!nativeTexture.Valid)
        {
            throw new InvalidOperationException($"BGFX could not create texture '{name}'.");
        }

        bgfx.set_texture_name(nativeTexture, name, Utf8Length(name));
        ulong id = m_nextPersistentId++;
        m_persistentTextures.Add(id, nativeTexture);
        m_persistentTextureDescriptors.Add(id, descriptor);
        return CreatePersistentTextureHandle(id, generation);
    }

    /// <summary>
    /// Creates a texture using this implementation's validated inputs.
    /// </summary>
    /// <param name="container">
    /// The container consumed by create texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="data">
    /// The complete immutable byte payload consumed by this operation.
    /// </param>
    /// <param name="sRgb">
    /// Whether s rgb behavior is enabled while create texture executes.
    /// </param>
    /// <param name="name">
    /// The human-readable name used for presentation and diagnostics.
    /// </param>
    /// <returns>
    /// The validated persistent texture handle that represents the completed operation.
    /// </returns>
    public PersistentTextureHandle CreateTexture(
        RenderTextureContainer container,
        ReadOnlySpan<byte> data,
        bool sRgb,
        string name
    ) {
        EnsureFrameSafetyPoint();
        if (container != RenderTextureContainer.Ktx)
        {
            throw new NotSupportedException($"BGFX does not accept texture container '{container}'.");
        }

        if (data.IsEmpty)
        {
            throw new ArgumentException("An encoded texture container cannot be empty.", nameof(data));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        bgfx.Memory* memory;
        fixed (byte* pointer = data)
        {
            memory = bgfx.copy(pointer, checked((uint)data.Length));
        }

        bgfx.TextureInfo info = default;
        bgfx.TextureHandle nativeTexture = bgfx.create_texture(
            memory,
            sRgb ? (ulong)bgfx.TextureFlags.Srgb : 0,
            0,
            &info);
        if (!nativeTexture.Valid)
        {
            throw new InvalidOperationException($"BGFX could not create encoded texture '{name}'.");
        }

        bgfx.set_texture_name(nativeTexture, name, Utf8Length(name));
        ulong id = m_nextPersistentId++;
        m_persistentTextures.Add(id, nativeTexture);
        return CreatePersistentTextureHandle(id, generation);
    }

    /// <summary>
    /// Updates texture state from the current authoritative inputs.
    /// </summary>
    /// <param name="texture">
    /// The texture consumed by update texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="data">
    /// The complete immutable byte payload consumed by this operation.
    /// </param>
    /// <param name="mipLevel">
    /// The mip level consumed by update texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="arrayLayer">
    /// The array layer consumed by update texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void UpdateTexture(
        PersistentTextureHandle texture,
        ReadOnlySpan<byte> data,
        int mipLevel = 0,
        int arrayLayer = 0
    ) {
        EnsureFrameSafetyPoint();
        ArgumentOutOfRangeException.ThrowIfNegative(mipLevel);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayLayer);
        ValidatePersistentHandle(texture);
        if (!m_persistentTextures.TryGetValue(GetHandleIdentity(texture).value, out bgfx.TextureHandle nativeTexture)
            || !m_persistentTextureDescriptors.TryGetValue(GetHandleIdentity(texture).value, out RenderTextureDescriptor? descriptor))
        {
            throw new ArgumentException("Persistent texture is not active on this device.", nameof(texture));
        }

        if (mipLevel >= descriptor.mipCount
            || arrayLayer >= descriptor.GetSubresourceLayerCount(mipLevel))
        {
            throw new ArgumentOutOfRangeException(nameof(mipLevel), "Texture subresource is outside the descriptor.");
        }

        int width = Math.Max(1, descriptor.width >> mipLevel);
        int height = Math.Max(1, descriptor.height >> mipLevel);
        int expectedSize = checked(width * height * BytesPerPixel(descriptor.format));
        if (data.Length != expectedSize)
        {
            throw new ArgumentException(
                $"Texture update requires exactly {expectedSize} tightly packed bytes.",
                nameof(data));
        }

        bgfx.Memory* memory;
        fixed (byte* pointer = data)
        {
            memory = bgfx.copy(pointer, checked((uint)data.Length));
        }

        switch (descriptor.dimension)
        {
            case RenderTextureDimension.Texture2D:
                bgfx.update_texture_2d(
                    nativeTexture,
                    checked((ushort)arrayLayer),
                    checked((byte)mipLevel),
                    0,
                    0,
                    checked((ushort)width),
                    checked((ushort)height),
                    memory,
                    ushort.MaxValue);
                break;
            case RenderTextureDimension.Texture3D:
                bgfx.update_texture_3d(
                    nativeTexture,
                    checked((byte)mipLevel),
                    0,
                    0,
                    checked((ushort)arrayLayer),
                    checked((ushort)width),
                    checked((ushort)height),
                    1,
                    memory);
                break;
            case RenderTextureDimension.Cube:
                bgfx.update_texture_cube(
                    nativeTexture,
                    checked((ushort)(arrayLayer / 6)),
                    checked((byte)(arrayLayer % 6)),
                    checked((byte)mipLevel),
                    0,
                    0,
                    checked((ushort)width),
                    checked((ushort)height),
                    memory,
                    ushort.MaxValue);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(descriptor));
        }
    }

    /// <summary>
    /// Updates texture region state from the current authoritative inputs.
    /// </summary>
    /// <param name="texture">
    /// The texture consumed by update texture region; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="region">
    /// The region consumed by update texture region; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="data">
    /// The complete immutable byte payload consumed by this operation.
    /// </param>
    public void UpdateTextureRegion(
        PersistentTextureHandle texture,
        RenderTextureRegion region,
        ReadOnlySpan<byte> data
    ) {
        EnsureFrameSafetyPoint();
        ValidatePersistentHandle(texture);
        if (!m_persistentTextures.TryGetValue(GetHandleIdentity(texture).value, out bgfx.TextureHandle nativeTexture)
            || !m_persistentTextureDescriptors.TryGetValue(GetHandleIdentity(texture).value, out RenderTextureDescriptor? descriptor))
        {
            throw new ArgumentException("Persistent texture is not active on this device.", nameof(texture));
        }
        if (region.mip >= descriptor.mipCount)
            throw new ArgumentOutOfRangeException(nameof(region), "Texture mip is outside its descriptor.");
        int mipWidth = Math.Max(1, descriptor.width >> region.mip);
        int mipHeight = Math.Max(1, descriptor.height >> region.mip);
        int layerCount = descriptor.GetSubresourceLayerCount(region.mip);
        if (region.x + region.width > mipWidth ||
            region.y + region.height > mipHeight ||
            region.layer + region.depth > layerCount)
        {
            throw new ArgumentOutOfRangeException(nameof(region), "Texture update region is outside its descriptor.");
        }
        if (descriptor.dimension != RenderTextureDimension.Texture3D && region.depth != 1)
        {
            throw new ArgumentException(
                "Two-dimensional and cubemap updates address exactly one layer or face.",
                nameof(region));
        }
        int rowPitch = checked(region.width * BytesPerPixel(descriptor.format));
        int expectedSize = checked(rowPitch * region.height * region.depth);
        if (data.Length != expectedSize)
        {
            throw new ArgumentException(
                $"Texture region update requires exactly {expectedSize} tightly packed bytes.",
                nameof(data));
        }

        bgfx.Memory* memory;
        fixed (byte* pointer = data)
            memory = bgfx.copy(pointer, checked((uint)data.Length));
        switch (descriptor.dimension)
        {
            case RenderTextureDimension.Texture2D:
                bgfx.update_texture_2d(
                    nativeTexture,
                    checked((ushort)region.layer),
                    checked((byte)region.mip),
                    checked((ushort)region.x),
                    checked((ushort)region.y),
                    checked((ushort)region.width),
                    checked((ushort)region.height),
                    memory,
                    checked((ushort)rowPitch));
                break;
            case RenderTextureDimension.Texture3D:
                bgfx.update_texture_3d(
                    nativeTexture,
                    checked((byte)region.mip),
                    checked((ushort)region.x),
                    checked((ushort)region.y),
                    checked((ushort)region.layer),
                    checked((ushort)region.width),
                    checked((ushort)region.height),
                    checked((ushort)region.depth),
                    memory);
                break;
            case RenderTextureDimension.Cube:
                bgfx.update_texture_cube(
                    nativeTexture,
                    checked((ushort)(region.layer / 6)),
                    checked((byte)(region.layer % 6)),
                    checked((byte)region.mip),
                    checked((ushort)region.x),
                    checked((ushort)region.y),
                    checked((ushort)region.width),
                    checked((ushort)region.height),
                    memory,
                    checked((ushort)rowPitch));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(descriptor));
        }
    }

    /// <summary>
    /// Destroys the texture after all in-flight references have retired.
    /// </summary>
    /// <param name="texture">
    /// The texture consumed by destroy texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void DestroyTexture(PersistentTextureHandle texture)
    {
        EnsureFrameSafetyPoint();
        ValidatePersistentHandle(texture);
        if (!m_persistentTextures.Remove(GetHandleIdentity(texture).value, out bgfx.TextureHandle nativeTexture))
        {
            throw new ArgumentException("Persistent texture is not active on this device.", nameof(texture));
        }

        m_persistentTextureDescriptors.Remove(GetHandleIdentity(texture).value);
        RemoveCachedFrameBuffersReferencing(nativeTexture.idx);

        EnqueueDestroy(DeferredResource.ForTexture(nativeTexture));
    }

    internal bgfx.TextureHandle ResolveTexture(RenderTextureHandle texture)
    {
        if (m_activeGraph is null
            || GetHandleIdentity(texture).generation != m_activeGraph.generation
            || !m_graphTextures.TryGetValue(GetHandleIdentity(texture).index, out bgfx.TextureHandle nativeTexture))
        {
            throw new ArgumentException("Texture is not active in the current BGFX graph.", nameof(texture));
        }

        return nativeTexture;
    }

    internal RenderTextureDescriptor ResolveTextureDescriptor(RenderTextureHandle texture)
    {
        if (m_activeGraph is null || GetHandleIdentity(texture).generation != m_activeGraph.generation)
        {
            throw new ArgumentException("Texture is not active in the current BGFX graph.", nameof(texture));
        }

        return m_activeGraph.textures[GetHandleIdentity(texture).index].descriptor;
    }

    private bgfx.TextureHandle CreateNativeTexture(RenderTextureDescriptor descriptor)
    {
        if (descriptor.width > capabilities.limits.maxTextureSize
            || descriptor.height > capabilities.limits.maxTextureSize
            || descriptor.depth > capabilities.limits.maxTextureSize)
        {
            throw new NotSupportedException(
                "The texture descriptor exceeds the active backend extent limit.");
        }

        if (!capabilities.SupportsSampled(descriptor.format, descriptor.dimension)
            && (descriptor.usage & RenderTextureUsage.Sampled) != 0)
        {
            throw new NotSupportedException(
                $"The active graphics backend cannot sample {descriptor.dimension} textures in format '{descriptor.format}'.");
        }

        if ((descriptor.usage
                & (RenderTextureUsage.ColorAttachment | RenderTextureUsage.DepthStencilAttachment)) != 0
            && !capabilities.SupportsRenderTarget(descriptor.format))
        {
            throw new NotSupportedException(
                $"The active graphics backend cannot attach texture format '{descriptor.format}'.");
        }

        if ((descriptor.usage
                & (RenderTextureUsage.ColorAttachment | RenderTextureUsage.DepthStencilAttachment)) != 0
            && descriptor.sampleCount > 1
            && !capabilities.SupportsMultisampleRenderTarget(descriptor.format))
        {
            throw new NotSupportedException(
                $"The active graphics backend cannot multisample texture format '{descriptor.format}'.");
        }

        if ((descriptor.usage & RenderTextureUsage.Storage) != 0
            && (!capabilities.Supports(GraphicsCapability.Compute)
                || !capabilities.Supports(GraphicsCapability.StorageTexture)
                || (!capabilities.SupportsStorage(descriptor.format, RenderStorageAccess.Read)
                    && !capabilities.SupportsStorage(descriptor.format, RenderStorageAccess.Write))))
        {
            throw new NotSupportedException(
                $"The active graphics backend cannot use texture format '{descriptor.format}' for storage access.");
        }

        if ((descriptor.usage & RenderTextureUsage.Readback) != 0)
        {
            if (!capabilities.Supports(GraphicsCapability.TextureReadback))
                throw new NotSupportedException("The active graphics backend does not support texture readback.");
            if (descriptor.sampleCount != 1 ||
                (descriptor.usage & (RenderTextureUsage.ColorAttachment |
                                     RenderTextureUsage.DepthStencilAttachment |
                                     RenderTextureUsage.Storage)) != 0)
            {
                throw new ArgumentException(
                    "Readback textures must be single-sampled transfer resources, not attachments or storage images.",
                    nameof(descriptor));
            }
        }

        if (descriptor.dimension == RenderTextureDimension.Texture2D
            && descriptor.arrayLayers > 1
            && !capabilities.Supports(GraphicsCapability.Texture2DArray))
        {
            throw new NotSupportedException(
                "The active graphics backend does not support two-dimensional texture arrays.");
        }

        if (descriptor.dimension == RenderTextureDimension.Texture3D
            && !capabilities.Supports(GraphicsCapability.Texture3D))
        {
            throw new NotSupportedException(
                "The active graphics backend does not support three-dimensional textures.");
        }

        if (descriptor.dimension == RenderTextureDimension.Cube
            && descriptor.arrayLayers > 1
            && !capabilities.Supports(GraphicsCapability.TextureCubeArray))
        {
            throw new NotSupportedException(
                "The active graphics backend does not support cubemap texture arrays.");
        }

        bgfx.TextureFlags flags = bgfx.TextureFlags.None;
        if ((descriptor.usage
            & (RenderTextureUsage.ColorAttachment | RenderTextureUsage.DepthStencilAttachment)) != 0)
        {
            flags |= descriptor.sampleCount switch
            {
                1 => bgfx.TextureFlags.Rt,
                2 => bgfx.TextureFlags.RtMsaaX2,
                4 => bgfx.TextureFlags.RtMsaaX4,
                8 => bgfx.TextureFlags.RtMsaaX8,
                16 => bgfx.TextureFlags.RtMsaaX16,
                _ => throw new ArgumentOutOfRangeException(nameof(descriptor))
            };
        }

        if ((descriptor.usage & RenderTextureUsage.Storage) != 0)
        {
            flags |= bgfx.TextureFlags.ComputeWrite;
        }

        if ((descriptor.usage & RenderTextureUsage.CopyDestination) != 0)
        {
            flags |= bgfx.TextureFlags.BlitDst;
        }

        if ((descriptor.usage & RenderTextureUsage.Readback) != 0)
        {
            flags |= bgfx.TextureFlags.ReadBack | bgfx.TextureFlags.BlitDst;
        }

        if (descriptor.format == RenderTextureFormat.RGBA8Srgb)
        {
            flags |= bgfx.TextureFlags.Srgb;
        }

        return descriptor.dimension switch
        {
            RenderTextureDimension.Texture2D => bgfx.create_texture_2d(
                checked((ushort)descriptor.width),
                checked((ushort)descriptor.height),
                descriptor.mipCount > 1,
                checked((ushort)descriptor.arrayLayers),
                BgfxCapabilityMapper.ToNativeFormat(descriptor.format),
                (ulong)flags,
                null,
                0),
            RenderTextureDimension.Texture3D => bgfx.create_texture_3d(
                checked((ushort)descriptor.width),
                checked((ushort)descriptor.height),
                checked((ushort)descriptor.depth),
                descriptor.mipCount > 1,
                BgfxCapabilityMapper.ToNativeFormat(descriptor.format),
                (ulong)flags,
                null,
                0),
            RenderTextureDimension.Cube => bgfx.create_texture_cube(
                checked((ushort)descriptor.width),
                descriptor.mipCount > 1,
                checked((ushort)descriptor.arrayLayers),
                BgfxCapabilityMapper.ToNativeFormat(descriptor.format),
                (ulong)flags,
                null,
                0),
            _ => throw new ArgumentOutOfRangeException(nameof(descriptor))
        };
    }

    private static int BytesPerPixel(RenderTextureFormat format)
        => format switch
        {
            RenderTextureFormat.R8 => 1,
            RenderTextureFormat.RG8 => 2,
            RenderTextureFormat.RGBA8 or RenderTextureFormat.RGBA8Srgb
                or RenderTextureFormat.RGB10A2 or RenderTextureFormat.RG11B10Float
                or RenderTextureFormat.R32Float or RenderTextureFormat.Depth24Stencil8
                or RenderTextureFormat.Depth32Float => 4,
            RenderTextureFormat.RGBA16Float => 8,
            _ => throw new ArgumentOutOfRangeException(nameof(format))
        };

}
