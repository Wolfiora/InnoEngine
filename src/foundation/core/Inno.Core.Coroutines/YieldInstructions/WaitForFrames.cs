using System;

namespace Inno.Core.Coroutines;

/// <summary>
/// Waits for a fixed number of frames.
/// </summary>
/// <param name="frames">
/// The frames used to initialize this instance.
/// </param>
public sealed class WaitForFrames(int frames) : YieldInstruction
{
    private readonly Func<double, ulong, bool> m_wait = (
        _,
        elapsedFrames
    ) => elapsedFrames < (ulong)Math.Max(frames, 1);

    /// <summary>
    /// Gets the configured frame count.
    /// </summary>
    public int frames { get; } = frames;

    /// <inheritdoc/>
    protected internal override Func<double, ulong, bool> CreateWaiter() => m_wait;
}
