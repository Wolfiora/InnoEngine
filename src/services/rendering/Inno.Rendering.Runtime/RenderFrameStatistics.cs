using System;
using Inno.Rendering;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Reports read-only statistics for the most recently completed render frame.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("f3924117-1829-5885-9f93-9010066fed48")]
public sealed class RenderFrameStatistics
{
    /// <summary>
    /// Creates an immutable frame statistics snapshot.
    /// </summary>
    /// <param name="frameIndex">
    /// Monotonic render frame index.
    /// </param>
    /// <param name="viewCount">
    /// Executed logical view count.
    /// </param>
    /// <param name="drawCount">
    /// Recorded draw count.
    /// </param>
    /// <param name="dispatchCount">
    /// Recorded compute dispatch count.
    /// </param>
    /// <param name="culledPassCount">
    /// Passes removed by graph compilation.
    /// </param>
    /// <param name="allocationCounters">
    /// Device-generation cumulative transient allocations at frame completion, or null when unavailable.
    /// </param>
    /// <param name="graphCompileCount">
    /// Complete graph compilation attempts for this frame, excluding validation-only analysis.
    /// </param>
    public RenderFrameStatistics(
        ulong frameIndex,
        int viewCount,
        int drawCount,
        int dispatchCount,
        int culledPassCount,
        RenderDeviceAllocationCounters? allocationCounters,
        int graphCompileCount
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(viewCount);
        ArgumentOutOfRangeException.ThrowIfNegative(drawCount);
        ArgumentOutOfRangeException.ThrowIfNegative(dispatchCount);
        ArgumentOutOfRangeException.ThrowIfNegative(culledPassCount);
        ArgumentOutOfRangeException.ThrowIfNegative(graphCompileCount);
        this.frameIndex = frameIndex;
        this.viewCount = viewCount;
        this.drawCount = drawCount;
        this.dispatchCount = dispatchCount;
        this.culledPassCount = culledPassCount;
        this.allocationCounters = allocationCounters;
        this.graphCompileCount = graphCompileCount;
    }

    /// <summary>
    /// Gets the monotonic render frame index.
    /// </summary>
    public ulong frameIndex { get; }

    /// <summary>
    /// Gets the executed logical view count.
    /// </summary>
    public int viewCount { get; }

    /// <summary>
    /// Gets the recorded draw count.
    /// </summary>
    public int drawCount { get; }

    /// <summary>
    /// Gets the recorded compute dispatch count.
    /// </summary>
    public int dispatchCount { get; }

    /// <summary>
    /// Gets passes removed by graph compilation.
    /// </summary>
    public int culledPassCount { get; }

    /// <summary>
    /// Gets the device-generation cumulative transient allocation snapshot at frame completion.
    /// Null means the backend does not report allocation accounting; it does not mean zero allocations.
    /// </summary>
    public RenderDeviceAllocationCounters? allocationCounters { get; }

    /// <summary>
    /// Gets complete graph compilation attempts for the frame. Validation does not increase this count.
    /// </summary>
    public int graphCompileCount { get; }
}
