using System;

namespace Inno.Content;

/// <summary>
/// Identifies one content entry with case-sensitive, portable, slash-separated segments.
/// </summary>
public readonly record struct ContentKey
{
    /// <summary>
    /// Validates a logical key without interpreting it as a host filesystem path.
    /// </summary>
    /// <param name="value">
    /// A nonempty relative key with no traversal, ambiguous segments, or platform-reserved names.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The key cannot be represented consistently by supported content sources.
    /// </exception>
    public ContentKey(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        foreach (string segment in value.Split('/'))
        {
            if (segment.Length == 0 || segment is "." or ".." || segment.EndsWith('.')
                || segment.EndsWith(' ') || IsReserved(segment))
                throw new ArgumentException("Content keys require portable relative segments.", nameof(value));
            foreach (char character in segment)
            {
                if (char.IsControl(character) || character is '\\' or ':' or '*' or '?' or '"' or '<' or '>' or '|')
                    throw new ArgumentException("Content keys contain a nonportable character.", nameof(value));
            }
        }
        this.value = value;
    }

    /// <summary>
    /// Gets the exact logical key; a default value is unassigned and cannot be read from a store.
    /// </summary>
    public string? value { get; }

    /// <inheritdoc />
    public override string ToString() => value ?? string.Empty;

    private static bool IsReserved(string segment)
    {
        ReadOnlySpan<char> stem = segment.AsSpan();
        int extension = stem.IndexOf('.');
        if (extension >= 0)
            stem = stem[..extension];
        return stem.Equals("CON", StringComparison.OrdinalIgnoreCase)
            || stem.Equals("PRN", StringComparison.OrdinalIgnoreCase)
            || stem.Equals("AUX", StringComparison.OrdinalIgnoreCase)
            || stem.Equals("NUL", StringComparison.OrdinalIgnoreCase)
            || (stem.Length == 4 && stem[3] is >= '1' and <= '9'
                && (stem[..3].Equals("COM", StringComparison.OrdinalIgnoreCase)
                    || stem[..3].Equals("LPT", StringComparison.OrdinalIgnoreCase)));
    }
}
