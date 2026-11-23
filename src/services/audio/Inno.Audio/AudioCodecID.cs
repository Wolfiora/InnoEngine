using System;

namespace Inno.Audio;

/// <summary>
/// Identifies encoded audio data without constraining codec providers to a closed enum.
/// </summary>
public readonly record struct AudioCodecID
{
    /// <summary>
    /// Creates an open codec identifier.
    /// </summary>
    /// <param name="value">
    /// Globally stable codec protocol value.
    /// </param>
    public AudioCodecID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets the standard Waveform Audio codec identifier.
    /// </summary>
    public static AudioCodecID wav { get; } = new("audio/wav");

    /// <summary>
    /// Gets the standard Free Lossless Audio Codec identifier.
    /// </summary>
    public static AudioCodecID flac { get; } = new("audio/flac");

    /// <summary>
    /// Gets the standard MPEG Layer III codec identifier.
    /// </summary>
    public static AudioCodecID mp3 { get; } = new("audio/mpeg");

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
