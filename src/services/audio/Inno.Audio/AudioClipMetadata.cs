using System;

namespace Inno.Audio;

/// <summary>
/// Describes imported encoded audio independently from its runtime storage path.
/// </summary>
public readonly record struct AudioClipMetadata
{
    /// <summary>
    /// Creates imported audio metadata.
    /// </summary>
    /// <param name="codec">
    /// Codec protocol of the encoded artifact.
    /// </param>
    /// <param name="channels">
    /// Encoded channel count.
    /// </param>
    /// <param name="sampleRate">
    /// Sample rate in frames per second.
    /// </param>
    /// <param name="frameCount">
    /// Total decoded frame count.
    /// </param>
    /// <param name="encodedByteLength">
    /// Encoded artifact length in bytes.
    /// </param>
    public AudioClipMetadata(
        AudioCodecID codec,
        int channels,
        int sampleRate,
        long frameCount,
        long encodedByteLength
    ) {
        if (!codec.isValid)
        {
            throw new ArgumentException("A valid codec identifier is required.", nameof(codec));
        }
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(channels);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sampleRate);
        ArgumentOutOfRangeException.ThrowIfNegative(frameCount);
        ArgumentOutOfRangeException.ThrowIfNegative(encodedByteLength);
        this.codec = codec;
        this.channels = channels;
        this.sampleRate = sampleRate;
        this.frameCount = frameCount;
        this.encodedByteLength = encodedByteLength;
    }

    /// <summary>
    /// Gets the codec protocol of the encoded artifact.
    /// </summary>
    public AudioCodecID codec { get; }

    /// <summary>
    /// Gets the encoded channel count.
    /// </summary>
    public int channels { get; }

    /// <summary>
    /// Gets the sample rate in frames per second.
    /// </summary>
    public int sampleRate { get; }

    /// <summary>
    /// Gets the total decoded frame count.
    /// </summary>
    public long frameCount { get; }

    /// <summary>
    /// Gets the encoded artifact length in bytes.
    /// </summary>
    public long encodedByteLength { get; }

    /// <summary>
    /// Gets the clip duration derived from frame count and sample rate.
    /// </summary>
    public TimeSpan duration => TimeSpan.FromSeconds((double)frameCount / sampleRate);
}
