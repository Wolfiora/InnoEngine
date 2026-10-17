using System;
using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed unsafe partial class BgfxCommandEncoder

{

    /// <summary>
    /// Copies texture without transferring ownership of the source state.
    /// </summary>
    /// <param name="source">
    /// The source value or location read by this operation.
    /// </param>
    /// <param name="destination">
    /// The destination that receives the completed result.
    /// </param>
    public override void CopyTexture(
        RenderTextureHandle source,
        RenderTextureHandle destination
    ) {
        RequireTextureBlit();
        RenderTextureDescriptor sourceDescriptor = m_device.ResolveTextureDescriptor(source);
        RenderTextureDescriptor destinationDescriptor = m_device.ResolveTextureDescriptor(destination);
        ValidateCompleteTextureCopy(sourceDescriptor, destinationDescriptor);
        bgfx.TextureHandle sourceTexture = m_device.ResolveTexture(source);
        bgfx.TextureHandle destinationTexture = m_device.ResolveTexture(destination);
        for (int mipLevel = 0; mipLevel < sourceDescriptor.mipCount; mipLevel++)
        {
            ushort width = checked((ushort)Math.Max(1, sourceDescriptor.width >> mipLevel));
            ushort height = checked((ushort)Math.Max(1, sourceDescriptor.height >> mipLevel));
            int layerCount = sourceDescriptor.GetSubresourceLayerCount(mipLevel);
            for (int layer = 0; layer < layerCount; layer++)
            {
                bgfx.encoder_blit(
                    m_encoder,
                    m_viewId,
                    destinationTexture,
                    checked((byte)mipLevel),
                    0,
                    0,
                    checked((ushort)layer),
                    sourceTexture,
                    checked((byte)mipLevel),
                    0,
                    0,
                    checked((ushort)layer),
                    width,
                    height,
                    1);
            }
        }
    }

    /// <summary>
    /// Copies the requested texture region into the destination texture resource.
    /// </summary>
    /// <param name="source">
    /// The source value or location read by this operation.
    /// </param>
    /// <param name="sourceRegion">
    /// The source region consumed by blit texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="destination">
    /// The destination that receives the completed result.
    /// </param>
    /// <param name="destinationRegion">
    /// The destination region consumed by blit texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BlitTexture(
        RenderTextureHandle source,
        RenderTextureRegion sourceRegion,
        RenderTextureHandle destination,
        RenderTextureRegion destinationRegion
    ) {
        RequireTextureBlit();
        RenderTextureDescriptor sourceDescriptor = m_device.ResolveTextureDescriptor(source);
        RenderTextureDescriptor destinationDescriptor = m_device.ResolveTextureDescriptor(destination);
        ValidateTextureRegion(sourceDescriptor, sourceRegion, nameof(sourceRegion));
        ValidateTextureRegion(destinationDescriptor, destinationRegion, nameof(destinationRegion));
        ValidateTextureCopyFormats(sourceDescriptor, destinationDescriptor);
        if (sourceRegion.width != destinationRegion.width
            || sourceRegion.height != destinationRegion.height
            || sourceRegion.depth != destinationRegion.depth)
        {
            throw new ArgumentException("BGFX blit source and destination extents must match.");
        }
        bgfx.encoder_blit(
            m_encoder,
            m_viewId,
            m_device.ResolveTexture(destination),
            checked((byte)destinationRegion.mip),
            checked((ushort)destinationRegion.x),
            checked((ushort)destinationRegion.y),
            checked((ushort)destinationRegion.layer),
            m_device.ResolveTexture(source),
            checked((byte)sourceRegion.mip),
            checked((ushort)sourceRegion.x),
            checked((ushort)sourceRegion.y),
            checked((ushort)sourceRegion.layer),
            checked((ushort)sourceRegion.width),
            checked((ushort)sourceRegion.height),
            checked((ushort)sourceRegion.depth));
    }

    /// <summary>
    /// Copies buffer without transferring ownership of the source state.
    /// </summary>
    /// <param name="source">
    /// The source value or location read by this operation.
    /// </param>
    /// <param name="destination">
    /// The destination that receives the completed result.
    /// </param>
    public override void CopyBuffer(
        RenderBufferHandle source,
        RenderBufferHandle destination
    ) {
        _ = source;
        _ = destination;
        throw new NotSupportedException(
            "BGFX has no general buffer-copy command; use a Plugin compute pass on supported devices.");
    }

}
