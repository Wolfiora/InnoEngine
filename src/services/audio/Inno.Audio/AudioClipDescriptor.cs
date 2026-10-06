using System;

namespace Inno.Audio;

/// <summary>
/// Describes one immutable encoded audio artifact presented to a backend device.
/// </summary>
public readonly record struct AudioClipDescriptor
{
    /// <summary>
    /// Creates a backend clip description.
    /// </summary>
    /// <param name="codec">
    /// Codec protocol used by the artifact.
    /// </param>
    /// <param name="loadMode">
    /// Required decoded or streamed storage strategy.
    /// </param>
    /// <param name="channels">
    /// Encoded channel count.
    /// </param>
    /// <param name="sampleRate">
    /// Encoded sample rate in frames per second.
    /// </param>
    /// <param name="frameCount">
    /// Total decoded frame count when known.
    /// </param>
    /// <param name="encodedByteLength">
    /// Encoded artifact length in bytes.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The load mode is undefined or a channel, rate, frame, or byte count is outside its valid range.
    /// </exception>
    public AudioClipDescriptor(
        AudioCodecId codec,
        AudioClipLoadMode loadMode,
        int channels,
        int sampleRate,
        long frameCount,
        long encodedByteLength
    ) {
        if (!codec.isValid)
            throw new ArgumentException("A valid codec identifier is required.", nameof(codec));
        if (!Enum.IsDefined(loadMode))
            throw new ArgumentOutOfRangeException(nameof(loadMode));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(channels);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sampleRate);
        ArgumentOutOfRangeException.ThrowIfNegative(frameCount);
        ArgumentOutOfRangeException.ThrowIfNegative(encodedByteLength);
        this.codec = codec;
        this.loadMode = loadMode;
        this.channels = channels;
        this.sampleRate = sampleRate;
        this.frameCount = frameCount;
        this.encodedByteLength = encodedByteLength;
    }

    /// <summary>
    /// Gets the encoded codec protocol.
    /// </summary>
    public AudioCodecId codec { get; }

    /// <summary>
    /// Gets the required storage strategy.
    /// </summary>
    public AudioClipLoadMode loadMode { get; }

    /// <summary>
    /// Gets the encoded channel count.
    /// </summary>
    public int channels { get; }

    /// <summary>
    /// Gets the encoded sample rate in frames per second.
    /// </summary>
    public int sampleRate { get; }

    /// <summary>
    /// Gets the total decoded frame count, or zero when unknown.
    /// </summary>
    public long frameCount { get; }

    /// <summary>
    /// Gets the encoded artifact length in bytes.
    /// </summary>
    public long encodedByteLength { get; }

}
