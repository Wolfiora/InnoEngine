using System;

namespace Inno.Audio;

/// <summary>
/// Contains immutable parameters used to create one playback voice.
/// </summary>
public readonly record struct AudioPlayOptions
{
    /// <summary>
    /// Creates default playback parameters.
    /// </summary>
    public AudioPlayOptions()
        : this(1f, 1f, 0f, false, 0, AudioBusID.master, AudioClipLoadMode.Automatic, null) {}

    /// <summary>
    /// Creates playback parameters.
    /// </summary>
    /// <param name="volume">
    /// Linear gain applied to the voice.
    /// </param>
    /// <param name="pitch">
    /// Playback-rate multiplier.
    /// </param>
    /// <param name="pan">
    /// Stereo pan from negative one to positive one.
    /// </param>
    /// <param name="loop">
    /// Whether playback restarts after reaching the clip end.
    /// </param>
    /// <param name="priority">
    /// Voice retention priority; larger values are retained first.
    /// </param>
    /// <param name="bus">
    /// Destination mixer bus.
    /// </param>
    /// <param name="loadMode">
    /// Clip preparation override.
    /// </param>
    /// <param name="spatial">
    /// Optional spatial playback parameters.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Gain, pitch, or pan is non-finite or outside its valid range, or the load mode is undefined.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The supplied destination bus is uninitialized.
    /// </exception>
    public AudioPlayOptions(
        float volume = 1f,
        float pitch = 1f,
        float pan = 0f,
        bool loop = false,
        int priority = 0,
        AudioBusID? bus = null,
        AudioClipLoadMode loadMode = AudioClipLoadMode.Automatic,
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
        if (!Enum.IsDefined(loadMode))
        {
            throw new ArgumentOutOfRangeException(nameof(loadMode));
        }
        AudioBusID resolvedBus = bus ?? AudioBusID.master;
        if (!resolvedBus.isValid)
        {
            throw new ArgumentException("A valid destination bus is required.", nameof(bus));
        }
        this.volume = volume;
        this.pitch = pitch;
        this.pan = pan;
        this.loop = loop;
        this.priority = priority;
        this.bus = resolvedBus;
        this.loadMode = loadMode;
        this.spatial = spatial;
    }

    /// <summary>
    /// Gets the default playback parameters.
    /// </summary>
    public static AudioPlayOptions defaultValue { get; } = new();

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
    /// Gets whether playback loops at the clip end.
    /// </summary>
    public bool loop { get; }

    /// <summary>
    /// Gets the voice retention priority.
    /// </summary>
    public int priority { get; }

    /// <summary>
    /// Gets the destination mixer bus.
    /// </summary>
    public AudioBusID bus { get; }

    /// <summary>
    /// Gets the clip preparation override.
    /// </summary>
    public AudioClipLoadMode loadMode { get; }

    /// <summary>
    /// Gets optional spatial playback parameters.
    /// </summary>
    public AudioSpatialOptions? spatial { get; }
}
