using System;
using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed unsafe partial class BgfxCommandEncoder

{

    private void RequireTextureBlit()
    {
        if (!m_device.capabilities.Supports(GraphicsCapability.TextureBlit))
        {
            throw new NotSupportedException("The active backend does not support texture blits.");
        }
    }

    private static void ValidateCompleteTextureCopy(
        RenderTextureDescriptor source,
        RenderTextureDescriptor destination
    ) {
        ValidateTextureCopyFormats(source, destination);
        if (source.width != destination.width
            || source.height != destination.height
            || source.depth != destination.depth
            || source.dimension != destination.dimension
            || source.mipCount != destination.mipCount
            || source.arrayLayers != destination.arrayLayers)
        {
            throw new ArgumentException(
                "A complete texture copy requires equal dimensions, mip counts, and array layers.");
        }
    }

    private static void ValidateTextureCopyFormats(
        RenderTextureDescriptor source,
        RenderTextureDescriptor destination
    ) {
        if (source.format != destination.format || source.sampleCount != destination.sampleCount)
        {
            throw new ArgumentException("Texture copies require equal formats and sample counts.");
        }

        if (source.sampleCount != 1)
        {
            throw new NotSupportedException("BGFX texture blits do not copy multisampled resources.");
        }

        if ((source.usage & RenderTextureUsage.CopySource) == 0
            || (destination.usage & RenderTextureUsage.CopyDestination) == 0)
        {
            throw new ArgumentException(
                "Texture copies require CopySource and CopyDestination usage respectively.");
        }
    }

    private static void ValidateTextureRegion(
        RenderTextureDescriptor descriptor,
        RenderTextureRegion region,
        string parameterName
    ) {
        if (region.mip >= descriptor.mipCount)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Texture mip level is outside the descriptor.");
        }

        int mipWidth = Math.Max(1, descriptor.width >> region.mip);
        int mipHeight = Math.Max(1, descriptor.height >> region.mip);
        if (region.x > mipWidth - region.width
            || region.y > mipHeight - region.height
            || region.layer > descriptor.GetSubresourceLayerCount(region.mip) - region.depth)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Texture region is outside the descriptor.");
        }
    }

    private void ValidateIndirect(
        BgfxBufferResource buffer,
        int firstCommand,
        int commandCount
    ) {
        if (!m_device.capabilities.Supports(GraphicsCapability.Indirect))
            throw new NotSupportedException("The active graphics backend does not support indirect commands.");
        ArgumentOutOfRangeException.ThrowIfNegative(firstCommand);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(commandCount);
        ValidateRange(firstCommand, commandCount, buffer.descriptor.elementCount, nameof(commandCount));
        if (buffer.kind != BgfxBufferKind.Indirect)
            throw new ArgumentException("The buffer was not created for indirect commands.", nameof(buffer));
    }

    private BgfxPipelineResource RequireGraphicsPipeline()
    {
        if (m_pipeline is null || m_pipeline.compute)
        {
            throw new InvalidOperationException("A graphics pipeline must be bound before drawing.");
        }

        return m_pipeline;
    }

    private BgfxPipelineResource RequireComputePipeline()
    {
        if (m_pipeline is null || !m_pipeline.compute)
        {
            throw new InvalidOperationException("A compute pipeline must be bound before dispatch.");
        }

        return m_pipeline;
    }

    private static void ValidateDrawCounts(
        int primitiveCount,
        int instanceCount
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(primitiveCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(instanceCount);
    }

    private static void ValidateRange(
        int first,
        int count,
        int available,
        string parameterName
    ) {
        if (first < 0 || count <= 0 || first > available - count)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                $"Requested range [{first}, {first + count}) exceeds buffer element count {available}.");
        }
    }

}
