using System;

namespace Inno.Editor.Rendering;

/// <summary>
/// Identifies a provider-defined navigation profile across reload generations.
/// </summary>
public readonly record struct EditorViewportNavigationProfileID
{
    /// <summary>
    /// Creates a stable profile identifier.
    /// </summary>
    /// <param name="value">
    /// Globally stable profile identity.
    /// </param>
    public EditorViewportNavigationProfileID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value.Trim();
    }

    /// <summary>
    /// Gets the stable profile identity.
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
