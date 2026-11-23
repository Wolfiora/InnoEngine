using System;

namespace Inno.Rendering;

/// <summary>
/// Contains backend-recorded GPU command submissions for the current frame.
/// </summary>
public readonly record struct RenderDeviceFrameCounters
{
    /// <summary>
    /// Creates immutable device frame counters.
    /// </summary>
    /// <param name="drawCount">
    /// Direct and indirect draw commands submitted so far.
    /// </param>
    /// <param name="dispatchCount">
    /// Direct and indirect compute dispatches submitted so far.
    /// </param>
    public RenderDeviceFrameCounters(
        int drawCount,
        int dispatchCount
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(drawCount);
        ArgumentOutOfRangeException.ThrowIfNegative(dispatchCount);
        this.drawCount = drawCount;
        this.dispatchCount = dispatchCount;
    }

    /// <summary>
    /// Gets direct and indirect draw commands submitted during the current frame.
    /// </summary>
    public int drawCount { get; }

    /// <summary>
    /// Gets direct and indirect compute dispatches submitted during the current frame.
    /// </summary>
    public int dispatchCount { get; }
}
