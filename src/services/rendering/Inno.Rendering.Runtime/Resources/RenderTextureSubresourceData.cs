using Inno.Core.Diagnostics;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Stores one complete texture mip and addressable layer, slice, or cubemap-face upload.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("22ed6418-30e6-511e-8af3-5d830d3b0ce5")]
public sealed class RenderTextureSubresourceData
{
    private readonly byte[] m_data;

    /// <summary>
    /// Creates one immutable texture subresource upload.
    /// </summary>
    /// <param name="mipLevel">
    /// Zero-based mip level.
    /// </param>
    /// <param name="arrayLayer">
    /// Zero-based 2D array layer, 3D Z slice, or flattened cubemap face using cube-layer * 6 + face.
    /// </param>
    /// <param name="data">
    /// Tightly packed complete subresource bytes.
    /// </param>
    public RenderTextureSubresourceData(
        int mipLevel,
        int arrayLayer,
        ReadOnlySpan<byte> data
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(mipLevel);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayLayer);
        if (data.IsEmpty)
            throw new ArgumentException("A texture subresource upload cannot be empty.", nameof(data));
        this.mipLevel = mipLevel;
        this.arrayLayer = arrayLayer;
        m_data = data.ToArray();
    }

    /// <summary>
    /// Gets the zero-based mip level.
    /// </summary>
    public int mipLevel { get; }

    /// <summary>
    /// Gets the zero-based array layer, volume slice, or flattened cubemap face.
    /// </summary>
    public int arrayLayer { get; }

    /// <summary>
    /// Gets immutable tightly packed bytes.
    /// </summary>
    public ReadOnlyMemory<byte> data => m_data;
}

