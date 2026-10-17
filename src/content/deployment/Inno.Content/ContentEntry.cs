using System;

namespace Inno.Content;

/// <summary>
/// Describes the exact uncompressed bytes of one immutable content entry.
/// </summary>
public sealed record ContentEntry
{
    /// <summary>
    /// Freezes a portable key, byte length, and SHA-256 identity.
    /// </summary>
    /// <param name="key">
    /// The assigned logical content key.
    /// </param>
    /// <param name="length">
    /// The exact nonnegative decoded byte count.
    /// </param>
    /// <param name="contentHash">
    /// The hexadecimal SHA-256 of decoded bytes, normalized to uppercase.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The key is unassigned or the hash is malformed.
    /// </exception>
    public ContentEntry(
        ContentKey key,
        long length,
        string contentHash
    ) {
        if (key.value is null)
            throw new ArgumentException("Content metadata requires an assigned key.", nameof(key));
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        this.key = key;
        this.length = length;
        this.contentHash = NormalizeHash(contentHash);
    }

    /// <summary>
    /// Gets the portable entry identity.
    /// </summary>
    public ContentKey key { get; }

    /// <summary>
    /// Gets the exact decoded byte count.
    /// </summary>
    public long length { get; }

    /// <summary>
    /// Gets the uppercase SHA-256 of decoded bytes.
    /// </summary>
    public string contentHash { get; }

    internal static string NormalizeHash(string value)
    {
        if (value is not { Length: 64 })
            throw new ArgumentException("Content identity requires a SHA-256 value.", nameof(value));
        foreach (char character in value)
        {
            if (!Uri.IsHexDigit(character))
                throw new ArgumentException("Content identity requires hexadecimal digits.", nameof(value));
        }
        return value.ToUpperInvariant();
    }
}
