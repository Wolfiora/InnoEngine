using System;

namespace Inno.Audio;

/// <summary>
/// Identifies an audio processor implementation without imposing a closed effect catalog.
/// </summary>
public readonly record struct AudioProcessorID
{
    /// <summary>
    /// Creates an open processor identifier.
    /// </summary>
    /// <param name="value">
    /// Globally stable processor protocol value.
    /// </param>
    public AudioProcessorID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets the standard low-pass filter processor identifier.
    /// </summary>
    public static AudioProcessorID lowPass { get; } = new("inno.audio.processor.low-pass");

    /// <summary>
    /// Gets the standard high-pass filter processor identifier.
    /// </summary>
    public static AudioProcessorID highPass { get; } = new("inno.audio.processor.high-pass");

    /// <summary>
    /// Gets the standard band-pass filter processor identifier.
    /// </summary>
    public static AudioProcessorID bandPass { get; } = new("inno.audio.processor.band-pass");

    /// <summary>
    /// Gets the standard notch filter processor identifier.
    /// </summary>
    public static AudioProcessorID notch { get; } = new("inno.audio.processor.notch");

    /// <summary>
    /// Gets the standard peak equalizer processor identifier.
    /// </summary>
    public static AudioProcessorID peak { get; } = new("inno.audio.processor.peak");

    /// <summary>
    /// Gets the standard low-shelf equalizer processor identifier.
    /// </summary>
    public static AudioProcessorID lowShelf { get; } = new("inno.audio.processor.low-shelf");

    /// <summary>
    /// Gets the standard high-shelf equalizer processor identifier.
    /// </summary>
    public static AudioProcessorID highShelf { get; } = new("inno.audio.processor.high-shelf");

    /// <summary>
    /// Gets the standard delay processor identifier.
    /// </summary>
    public static AudioProcessorID delay { get; } = new("inno.audio.processor.delay");

    /// <summary>
    /// Gets the globally stable protocol value.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Gets whether this identifier contains a usable protocol value.
    /// </summary>
    public bool isValid => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Formats the identifier for diagnostics and persistence.
    /// </summary>
    /// <returns>
    /// The stable protocol value.
    /// </returns>
    public override string ToString() => value;
}
