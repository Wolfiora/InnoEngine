using System;

namespace Inno.Editor.Core;

/// <summary>
/// Identifies one editor statistic across panels, modules, and reloadable extensions.
/// </summary>
public readonly record struct EditorStatisticID
{
    /// <summary>
    /// Creates a globally stable statistic identifier.
    /// </summary>
    /// <param name="value">
    /// Non-empty globally stable identity.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is empty.
    /// </exception>
    public EditorStatisticID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value.Trim();
    }

    /// <summary>
    /// Gets the globally stable identity.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Gets whether this identifier contains a usable value.
    /// </summary>
    public bool isValid => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Formats this value as a human-readable representation.
    /// </summary>
    /// <returns>
    /// The human-readable representation of this value.
    /// </returns>
    public override string ToString() => value ?? string.Empty;
}
