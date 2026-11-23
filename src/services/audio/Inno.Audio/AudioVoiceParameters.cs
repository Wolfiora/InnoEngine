using System;

namespace Inno.Audio;

/// <summary>
/// Contains mutable voice parameters that can be changed after playback begins.
/// </summary>
public readonly record struct AudioVoiceParameters
{
    /// <summary>
    /// Creates a voice parameter snapshot.
    /// </summary>
    /// <param name="volume">
    /// Linear voice gain.
    /// </param>
    /// <param name="pitch">
    /// Playback-rate multiplier.
    /// </param>
    /// <param name="pan">
    /// Stereo pan from negative one to positive one.
    /// </param>
    /// <param name="spatial">
    /// Optional spatial playback parameters.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Gain, pitch, or pan is non-finite or outside its valid range.
    /// </exception>
    public AudioVoiceParameters(
        float volume,
        float pitch,
        float pan,
        AudioSpatialOptions? spatial = null
    ) {
        if (!float.IsFinite(volume) || volume < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(volume));
        }
        if (!float.IsFinite(pitch) || pitch <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(pitch));
        }
        if (!float.IsFinite(pan) || pan is < -1f or > 1f)
        {
            throw new ArgumentOutOfRangeException(nameof(pan));
        }
        this.volume = volume;
        this.pitch = pitch;
        this.pan = pan;
        this.spatial = spatial;
    }

    /// <summary>
    /// Gets the linear voice gain.
    /// </summary>
    public float volume { get; }

    /// <summary>
    /// Gets the playback-rate multiplier.
    /// </summary>
    public float pitch { get; }

    /// <summary>
    /// Gets the stereo pan value.
    /// </summary>
    public float pan { get; }

    /// <summary>
    /// Gets optional spatial playback parameters.
    /// </summary>
    public AudioSpatialOptions? spatial { get; }
}
