using Inno.Core.Mathematics;
using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace Inno.Rendering.Assets;

/// <summary>
/// Identifies a contiguous triangle-index range.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("5da29d2a-b38b-5d50-b8ee-10e00503c0c5")]
public readonly record struct GeometrySection
{
    /// <summary>
    /// Creates a submesh range.
    /// </summary>
    /// <param name="firstIndex">
    /// First index in the shared index buffer.
    /// </param>
    /// <param name="indexCount">
    /// Number of indices in the range.
    /// </param>
    public GeometrySection(
        int firstIndex,
        int indexCount
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(firstIndex);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(indexCount);
        this.firstIndex = firstIndex;
        this.indexCount = indexCount;
    }

    /// <summary>
    /// Gets the first index in the shared index buffer.
    /// </summary>
    public int firstIndex { get; }

    /// <summary>
    /// Gets the number of indices in the range.
    /// </summary>
    public int indexCount { get; }
}

