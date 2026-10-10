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
/// Describes one indexed range in resolved backend-neutral geometry.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("fc4ae91b-3a0b-59d3-b65e-4c0c3533d715")]
public readonly record struct RenderGeometrySection
{
    /// <summary>
    /// Creates one indexed geometry range.
    /// </summary>
    /// <param name="firstIndex">
    /// First index in the shared index buffer.
    /// </param>
    /// <param name="indexCount">
    /// Positive index count.
    /// </param>
    public RenderGeometrySection(
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
    /// Gets the number of indices in this range.
    /// </summary>
    public int indexCount { get; }
}

