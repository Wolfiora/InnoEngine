using System;

namespace Inno.Audio;

/// <summary>
/// Reports an immutable snapshot of runtime and backend resource usage.
/// </summary>
public readonly record struct AudioStatistics
{
    /// <summary>
    /// Creates an audio statistics snapshot.
    /// </summary>
    /// <param name="activeVoices">
    /// Voices that currently consume runtime budget.
    /// </param>
    /// <param name="loadedClips">
    /// Clip allocations held by the backend.
    /// </param>
    /// <param name="decodedBytes">
    /// Approximate decoded clip bytes held in memory.
    /// </param>
    /// <param name="stolenVoiceCount">
    /// Voices reclaimed since runtime creation.
    /// </param>
    public AudioStatistics(
        int activeVoices,
        int loadedClips,
        long decodedBytes,
        long stolenVoiceCount
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(activeVoices);
        ArgumentOutOfRangeException.ThrowIfNegative(loadedClips);
        ArgumentOutOfRangeException.ThrowIfNegative(decodedBytes);
        ArgumentOutOfRangeException.ThrowIfNegative(stolenVoiceCount);
        this.activeVoices = activeVoices;
        this.loadedClips = loadedClips;
        this.decodedBytes = decodedBytes;
        this.stolenVoiceCount = stolenVoiceCount;
    }

    /// <summary>
    /// Gets voices that currently consume runtime budget.
    /// </summary>
    public int activeVoices { get; }

    /// <summary>
    /// Gets clip allocations held by the backend.
    /// </summary>
    public int loadedClips { get; }

    /// <summary>
    /// Gets approximate decoded clip bytes held in memory.
    /// </summary>
    public long decodedBytes { get; }

    /// <summary>
    /// Gets voices reclaimed since runtime creation.
    /// </summary>
    public long stolenVoiceCount { get; }
}
