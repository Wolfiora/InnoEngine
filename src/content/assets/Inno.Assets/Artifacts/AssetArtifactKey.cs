using System;

namespace Inno.Assets;

/// <summary>
/// Identifies one immutable content-addressed artifact bundle.
/// </summary>
public readonly struct AssetArtifactKey : IEquatable<AssetArtifactKey>
{
    private const int C_SHA256_HEX_LENGTH = 64;

    /// <summary>
    /// Creates an artifact key from a hexadecimal content fingerprint.
    /// </summary>
    /// <param name="value">
    /// The hexadecimal content fingerprint.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A non-empty value is not exactly 64 hexadecimal digits.
    /// </exception>
    public AssetArtifactKey(string value)
    {
        string normalized = string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim().ToUpperInvariant();
        if (normalized.Length != 0)
        {
            if (normalized.Length != C_SHA256_HEX_LENGTH)
                throw new ArgumentException("An artifact key must be a complete SHA-256 hexadecimal fingerprint.", nameof(value));
            foreach (char digit in normalized)
                if (digit is not (>= '0' and <= '9') and not (>= 'A' and <= 'F'))
                    throw new ArgumentException("An artifact key cannot contain non-hexadecimal characters.", nameof(value));
        }
        this.value = normalized;
    }

    /// <summary>
    /// Gets an empty artifact key.
    /// </summary>
    public static AssetArtifactKey empty => default;

    /// <summary>
    /// Gets the normalized SHA-256 hexadecimal fingerprint, or an empty value for an unassigned key.
    /// </summary>
    public string value { get; } = string.Empty;

    /// <summary>
    /// Gets whether the key is empty.
    /// </summary>
    public bool isEmpty => string.IsNullOrEmpty(value);

    /// <summary>
    /// Determines whether this instance and the supplied value represent the same logical state.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when both values represent the same logical state; otherwise, <see langword="false"/>.
    /// </returns>
    /// <param name="other">
    /// The value to compare with this instance.
    /// </param>
    public bool Equals(AssetArtifactKey other) => string.Equals(value, other.value, StringComparison.Ordinal);

    /// <summary>
    /// Determines whether this instance and the supplied value represent the same logical state.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when both values represent the same logical state; otherwise, <see langword="false"/>.
    /// </returns>
    /// <param name="obj">
    /// The object to compare with this instance.
    /// </param>
    public override bool Equals(object? obj) => obj is AssetArtifactKey other && Equals(other);

    /// <summary>
    /// Computes a hash code from the fields that participate in logical equality.
    /// </summary>
    /// <returns>
    /// A hash code consistent with the implemented equality contract.
    /// </returns>
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(value ?? string.Empty);

    /// <summary>
    /// Formats this value as a human-readable representation.
    /// </summary>
    /// <returns>
    /// The human-readable representation of this value.
    /// </returns>
    public override string ToString() => value ?? string.Empty;

    /// <summary>
    /// Determines whether two artifact keys are equal.
    /// </summary>
    /// <param name="left">
    /// The first artifact key to compare.
    /// </param>
    /// <param name="right">
    /// The second artifact key to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when both keys contain the same normalized fingerprint; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(
        AssetArtifactKey left,
        AssetArtifactKey right
    ) => left.Equals(right);

    /// <summary>
    /// Determines whether two artifact keys differ.
    /// </summary>
    /// <param name="left">
    /// The first artifact key to compare.
    /// </param>
    /// <param name="right">
    /// The second artifact key to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the normalized fingerprints differ; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(
        AssetArtifactKey left,
        AssetArtifactKey right
    ) => !left.Equals(right);
}
