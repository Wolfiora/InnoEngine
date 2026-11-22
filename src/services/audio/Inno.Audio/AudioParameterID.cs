using System;

namespace Inno.Audio;

/// <summary>
/// Identifies one processor parameter without imposing a processor-specific enum.
/// </summary>
public readonly record struct AudioParameterID
{
    /// <summary>
    /// Creates an open processor parameter identifier.
    /// </summary>
    /// <param name="value">
    /// Stable parameter name within its processor protocol.
    /// </param>
    public AudioParameterID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets the standard cutoff or center-frequency parameter in hertz.
    /// </summary>
    public static AudioParameterID frequency { get; } = new("inno.audio.parameter.frequency-hz");

    /// <summary>
    /// Gets the standard filter quality-factor parameter.
    /// </summary>
    public static AudioParameterID quality { get; } = new("inno.audio.parameter.quality");

    /// <summary>
    /// Gets the standard equalizer gain parameter in decibels.
    /// </summary>
    public static AudioParameterID gainDecibels { get; } = new("inno.audio.parameter.gain-db");

    /// <summary>
    /// Gets the standard equalizer shelf-slope parameter.
    /// </summary>
    public static AudioParameterID shelfSlope { get; } = new("inno.audio.parameter.shelf-slope");

    /// <summary>
    /// Gets the standard delay duration parameter in milliseconds.
    /// </summary>
    public static AudioParameterID delayMilliseconds { get; } = new("inno.audio.parameter.delay-ms");

    /// <summary>
    /// Gets the standard delay feedback decay factor.
    /// </summary>
    public static AudioParameterID decay { get; } = new("inno.audio.parameter.decay");

    /// <summary>
    /// Gets the stable parameter protocol value.
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
