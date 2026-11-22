using System;

namespace Inno.Core.Settings;

/// <summary>
/// Identifies one host-neutral project setting protocol.
/// </summary>
public record struct ProjectSettingID
{
    /// <summary>
    /// Creates a project setting identifier.
    /// </summary>
    /// <param name="value">
    /// Globally stable setting value.
    /// </param>
    public ProjectSettingID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value.Trim();
    }

    /// <summary>
    /// Gets or sets the globally stable setting value.
    /// </summary>
    public string value { get; set; }

    /// <summary>
    /// Gets whether the identifier has a usable value.
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
