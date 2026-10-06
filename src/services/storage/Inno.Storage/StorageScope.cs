using System;

namespace Inno.Storage;

/// <summary>
/// Identifies an application data namespace independently of its physical storage location.
/// </summary>
public readonly record struct StorageScope
{
    /// <summary>
    /// Captures a portable, case-sensitive application namespace.
    /// </summary>
    /// <param name="value">
    /// A nonempty namespace containing ASCII letters, digits, dots, underscores, or hyphens.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The namespace is empty, contains path syntax, has a leading or trailing dot,
    /// or contains a reserved portable device name.
    /// </exception>
    public StorageScope(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (value[0] == '.' || value[^1] == '.')
            throw new ArgumentException("A storage namespace cannot begin or end with a dot.", nameof(value));
        foreach (char character in value)
        {
            if (!char.IsAsciiLetterOrDigit(character) && character is not '.' and not '_' and not '-')
                throw new ArgumentException("A storage namespace requires portable identifier characters.", nameof(value));
        }
        ReadOnlySpan<char> name = value.AsSpan();
        int extension = name.IndexOf('.');
        if (extension >= 0)
            name = name[..extension];
        if (name.Equals("CON", StringComparison.OrdinalIgnoreCase)
            || name.Equals("PRN", StringComparison.OrdinalIgnoreCase)
            || name.Equals("AUX", StringComparison.OrdinalIgnoreCase)
            || name.Equals("NUL", StringComparison.OrdinalIgnoreCase)
            || (name.Length == 4 && name[3] is >= '1' and <= '9'
                && (name[..3].Equals("COM", StringComparison.OrdinalIgnoreCase)
                    || name[..3].Equals("LPT", StringComparison.OrdinalIgnoreCase))))
        {
            throw new ArgumentException("A storage namespace cannot use a reserved portable device name.", nameof(value));
        }
        this.value = value;
    }

    /// <summary>
    /// Gets the logical namespace, or null for an unassigned value.
    /// </summary>
    public string? value { get; }

    /// <summary>
    /// Gets whether this value identifies a storage namespace.
    /// </summary>
    public bool isValid => value is not null;

    /// <inheritdoc />
    public override string ToString() => value ?? string.Empty;
}
