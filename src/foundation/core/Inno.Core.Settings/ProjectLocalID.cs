using System;
using System.Text;

namespace Inno.Core.Settings;

/// <summary>
/// Stores the project-independent portion of one project-scoped identity.
/// </summary>
public readonly record struct ProjectLocalID
{
    /// <summary>
    /// Creates a portable local identity.
    /// </summary>
    /// <param name="value">
    /// Lowercase ASCII local namespace segments.
    /// </param>
    public ProjectLocalID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        string normalized = value.Trim();
        ProjectID.ValidatePortable(normalized, "Project-local ID", nameof(value));
        this.value = normalized;
    }

    /// <summary>
    /// Gets the canonical local identity text.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Formats the canonical local identity.
    /// </summary>
    /// <returns>
    /// The canonical local identity text.
    /// </returns>
    public override string ToString() => value ?? string.Empty;

    /// <summary>
    /// Creates a deterministic portable local identity from a display name.
    /// </summary>
    /// <param name="name">
    /// The user-facing name.
    /// </param>
    /// <returns>
    /// The normalized local identity.
    /// </returns>
    public static ProjectLocalID FromName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var builder = new StringBuilder(name.Length);
        bool pendingSeparator = false;
        foreach (char character in name.Trim())
        {
            if (character is >= 'A' and <= 'Z')
            {
                AppendSeparator(builder, ref pendingSeparator);
                builder.Append((char)(character + ('a' - 'A')));
            }
            else if (character is >= 'a' and <= 'z' or >= '0' and <= '9')
            {
                AppendSeparator(builder, ref pendingSeparator);
                builder.Append(character);
            }
            else if (character is ' ' or '\t' or '.' or '-' or '_')
            {
                pendingSeparator = builder.Length > 0;
            }
            else
            {
                throw new ArgumentException(
                    "A project-scoped name must use ASCII letters, digits, spaces, dots, hyphens, or underscores.",
                    nameof(name)
                );
            }
        }
        if (builder.Length == 0)
        {
            throw new ArgumentException("A project-scoped name must contain a letter or digit.", nameof(name));
        }
        return new ProjectLocalID(builder.ToString());
    }

    private static void AppendSeparator(
        StringBuilder builder,
        ref bool pendingSeparator
    ) {
        if (pendingSeparator && builder.Length > 0)
        {
            builder.Append('-');
        }
        pendingSeparator = false;
    }
}
