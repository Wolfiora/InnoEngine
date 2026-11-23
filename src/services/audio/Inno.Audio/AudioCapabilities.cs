using System;

namespace Inno.Audio;

/// <summary>
/// Describes backend-neutral features and limits for one device generation.
/// </summary>
public sealed record AudioCapabilities
{
    /// <summary>
    /// Creates an immutable audio capability snapshot.
    /// </summary>
    /// <param name="supportsStreaming">
    /// Whether clips can be decoded incrementally.
    /// </param>
    /// <param name="supportsScheduledPlayback">
    /// Whether voices can start against the audio clock.
    /// </param>
    /// <param name="supportsSpatialAudio">
    /// Whether listener-relative spatial playback is available.
    /// </param>
    /// <param name="maxListeners">
    /// Maximum simultaneously active listeners.
    /// </param>
    /// <param name="sampleRate">
    /// Device output sample rate in frames per second.
    /// </param>
    public AudioCapabilities(
        bool supportsStreaming,
        bool supportsScheduledPlayback,
        bool supportsSpatialAudio,
        int maxListeners,
        int sampleRate
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(maxListeners);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sampleRate);
        this.supportsStreaming = supportsStreaming;
        this.supportsScheduledPlayback = supportsScheduledPlayback;
        this.supportsSpatialAudio = supportsSpatialAudio;
        this.maxListeners = maxListeners;
        this.sampleRate = sampleRate;
    }

    /// <summary>
    /// Gets whether clips can be decoded incrementally.
    /// </summary>
    public bool supportsStreaming { get; }

    /// <summary>
    /// Gets whether voices can start against the audio clock.
    /// </summary>
    public bool supportsScheduledPlayback { get; }

    /// <summary>
    /// Gets whether listener-relative spatial playback is available.
    /// </summary>
    public bool supportsSpatialAudio { get; }

    /// <summary>
    /// Gets the maximum simultaneously active listener count.
    /// </summary>
    public int maxListeners { get; }

    /// <summary>
    /// Gets the output sample rate in frames per second.
    /// </summary>
    public int sampleRate { get; }
}
