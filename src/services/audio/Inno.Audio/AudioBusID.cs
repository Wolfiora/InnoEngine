using System;

namespace Inno.Audio;

/// <summary>
/// Identifies a mixer bus without imposing a closed bus catalog.
/// </summary>
public readonly record struct AudioBusID
{
    /// <summary>
    /// Creates an open mixer bus identifier.
    /// </summary>
    /// <param name="value">
    /// Globally stable bus protocol value.
    /// </param>
    public AudioBusID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets the mandatory root bus identifier.
    /// </summary>
    public static AudioBusID master { get; } = new("inno.audio.bus.master");

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
