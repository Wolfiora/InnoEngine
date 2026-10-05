using System;

namespace Inno.Adapter.Audio;

/// <summary>
/// Identifies a audio implementation without closing the set of supported backends.
/// </summary>
public readonly record struct AudioBackendId
{
    /// <summary>
    /// Creates an ordinal, case-sensitive implementation identifier.
    /// </summary>
    /// <param name="value">
    /// A stable nonempty identifier containing no whitespace.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identifier is empty or contains whitespace.
    /// </exception>
    public AudioBackendId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        foreach (char character in value)
            if (char.IsWhiteSpace(character))
                throw new ArgumentException("A backend identifier cannot contain whitespace.", nameof(value));
        this.value = value;
    }

    /// <summary>
    /// Gets the identifier of the bundled miniAudio implementation.
    /// </summary>
    public static AudioBackendId miniAudio { get; } = new("inno.audio.mini-audio");

    /// <summary>
    /// Gets the stable identifier; a default value is unassigned.
    /// </summary>
    public string value { get; } = string.Empty;

    /// <summary>
    /// Gets whether this value identifies an implementation.
    /// </summary>
    public bool isValid => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Returns the identifier without resolving a provider.
    /// </summary>
    /// <returns>
    /// The stable identifier, or an empty string when unassigned.
    /// </returns>
    public override string ToString() => value ?? string.Empty;
}

