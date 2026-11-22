using System;
using System.Text;

namespace Inno.Core.Settings;

/// <summary>
/// Identifies one project namespace used to qualify project-authored logical names.
/// </summary>
public readonly record struct ProjectID
{
    /// <summary>
    /// Creates a portable project identifier.
    /// </summary>
    /// <param name="value">
    /// Lowercase ASCII namespace segments.
    /// </param>
    public ProjectID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        string normalized = value.Trim();
        ValidatePortable(normalized, "Project ID", nameof(value));
        this.value = normalized;
    }

    /// <summary>
    /// Gets the canonical identifier text.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Gets whether the value contains a usable identifier.
    /// </summary>
    public bool isValid => !string.IsNullOrEmpty(value);

    /// <summary>
    /// Qualifies a local project name beneath this project namespace.
    /// </summary>
    /// <param name="name">
    /// The portable local name.
    /// </param>
    /// <returns>
    /// The complete project-scoped identity.
    /// </returns>
    public ProjectScopedID Qualify(ProjectLocalID name) => new(this, name);

    /// <summary>
    /// Formats the canonical identifier.
    /// </summary>
    /// <returns>
    /// The canonical identifier text.
    /// </returns>
    public override string ToString() => value ?? string.Empty;

    /// <summary>
    /// Creates a portable initial project identifier from a user-facing project name.
    /// </summary>
    /// <param name="name">
    /// The project name used to derive the initial namespace.
    /// </param>
    /// <returns>
    /// A portable initial project identifier.
    /// </returns>
    public static ProjectID FromName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var builder = new StringBuilder(name.Length);
        bool pendingSeparator = false;
        foreach (char character in name.Trim())
        {
            if (character is >= 'A' and <= 'Z')
            {
                Append((char)(character + ('a' - 'A')));
            }
            else if (character is >= 'a' and <= 'z' or >= '0' and <= '9')
            {
                Append(character);
            }
            else
            {
                pendingSeparator = builder.Length > 0;
            }
        }
        string value = builder.Length == 0 ? "inno.project" : builder.ToString();
        return new ProjectID(string.Equals(value, "project", StringComparison.Ordinal) ? "inno.project" : value);

        void Append(char character)
        {
            if (pendingSeparator)
            {
                builder.Append('-');
            }
            pendingSeparator = false;
            builder.Append(character);
        }
    }

    internal static void ValidatePortable(
        string value,
        string label,
        string parameterName
    ) {
        if (value.Length > 128)
        {
            throw new ArgumentException($"{label} cannot exceed 128 characters.", parameterName);
        }
        for (int index = 0; index < value.Length; index++)
        {
            char character = value[index];
            bool valid = character is >= 'a' and <= 'z'
                         || character is >= '0' and <= '9'
                         || character is '.' or '-' or '_';
            if (!valid)
            {
                throw new ArgumentException(
                    $"{label} must use lowercase ASCII letters, digits, dots, hyphens, or underscores.",
                    parameterName
                );
            }
        }
        if (value[0] is '.' or '-' or '_' || value[^1] is '.' or '-' or '_')
        {
            throw new ArgumentException($"{label} must begin and end with a letter or digit.", parameterName);
        }
        if (value.Contains("..", StringComparison.Ordinal))
        {
            throw new ArgumentException($"{label} cannot contain empty namespace segments.", parameterName);
        }
    }
}
