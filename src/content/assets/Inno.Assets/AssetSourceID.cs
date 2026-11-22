using System;
using System.Linq;

namespace Inno.Assets;

/// <summary>
/// Identifies one isolated asset source mount.
/// </summary>
public record struct AssetSourceID
{
    /// <summary>
    /// Creates a globally stable source identifier.
    /// </summary>
    /// <param name="value">
    /// Lowercase project or Plugin source identity.
    /// </param>
    public AssetSourceID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        string normalized = value.Trim();
        if (normalized.Any(static character =>
                !(character is >= 'a' and <= 'z'
                  || character is >= '0' and <= '9'
                  || character is '.' or '_' or '-'))
        ) {
            throw new ArgumentException(
                "An asset source ID may contain lowercase ASCII letters, digits, '.', '_', and '-' only.",
                nameof(value)
            );
        }
        this.value = normalized;
    }

    /// <summary>
    /// Gets the writable project source identifier.
    /// </summary>
    public static AssetSourceID project => new("project");

    /// <summary>
    /// Gets or sets the globally stable source value.
    /// </summary>
    public string value { get; set; }

    /// <summary>
    /// Gets whether the source identity has a usable value.
    /// </summary>
    public readonly bool isValid => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Formats this value as a human-readable representation.
    /// </summary>
    /// <returns>
    /// The human-readable representation of this value.
    /// </returns>
    public override readonly string ToString() => value ?? string.Empty;
}
