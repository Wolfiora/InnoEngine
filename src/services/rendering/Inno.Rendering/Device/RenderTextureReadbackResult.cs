using System;

namespace Inno.Rendering;

/// <summary>
/// Contains immutable bytes copied from one complete texture mip.
/// </summary>
public sealed class RenderTextureReadbackResult
{
    private readonly byte[] m_data;

    /// <summary>
    /// Creates one immutable texture readback result.
    /// </summary>
    /// <param name="descriptor">
    /// Source texture descriptor.
    /// </param>
    /// <param name="mipLevel">
    /// Source mip level.
    /// </param>
    /// <param name="rowPitch">
    /// Number of bytes between adjacent texel rows.
    /// </param>
    /// <param name="data">
    /// Complete tightly packed mip bytes for every addressable layer.
    /// </param>
    public RenderTextureReadbackResult(
        RenderTextureDescriptor descriptor,
        int mipLevel,
        int rowPitch,
        ReadOnlySpan<byte> data
    ) {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentOutOfRangeException.ThrowIfNegative(mipLevel);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rowPitch);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(mipLevel, descriptor.mipCount);
        if (data.IsEmpty)
        {
            throw new ArgumentException("Texture readback data cannot be empty.", nameof(data));
        }
        this.descriptor = descriptor;
        this.mipLevel = mipLevel;
        this.rowPitch = rowPitch;
        m_data = data.ToArray();
    }

    /// <summary>
    /// Gets the source texture descriptor.
    /// </summary>
    public RenderTextureDescriptor descriptor { get; }

    /// <summary>
    /// Gets the source mip level.
    /// </summary>
    public int mipLevel { get; }

    /// <summary>
    /// Gets the byte distance between adjacent rows.
    /// </summary>
    public int rowPitch { get; }

    /// <summary>
    /// Gets complete tightly packed mip bytes for every addressable layer.
    /// </summary>
    public ReadOnlyMemory<byte> data => m_data;
}
