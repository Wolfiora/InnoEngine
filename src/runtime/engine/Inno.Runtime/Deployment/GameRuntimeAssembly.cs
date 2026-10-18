using System;
using System.IO;
using System.Linq;
using Inno.Core.Serialization;

namespace Inno.Runtime;

/// <summary>
/// Records a logical assembly identity and the content fingerprint of its build input.
/// </summary>
[GenerateSerializationConverter]
public sealed class GameRuntimeAssembly : ISerializable
{
    /// <summary>
    /// Gets or sets the exact simple assembly name, independent of deployment file layout.
    /// </summary>
    [SerializableProperty]
    public string name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the lowercase SHA-256 fingerprint of the frozen compiler output.
    /// </summary>
    [SerializableProperty]
    public string contentFingerprint { get; set; } = string.Empty;

    /// <summary>
    /// Rejects identities or fingerprints that cannot describe a frozen code input.
    /// </summary>
    /// <exception cref="InvalidDataException">
    /// The assembly name is empty or nonportable, or the fingerprint is not a SHA-256 value.
    /// </exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(name)
            || name.Any(static character => !(char.IsAsciiLetterOrDigit(character) || character is '.' or '_' or '-')))
            throw new InvalidDataException("A code assembly requires a portable simple name.");
        if (contentFingerprint is null || contentFingerprint.Length != 64
            || contentFingerprint.Any(static character => !(character is >= '0' and <= '9' or >= 'a' and <= 'f')))
            throw new InvalidDataException($"Code assembly '{name}' requires a lowercase SHA-256 fingerprint.");
    }
}
