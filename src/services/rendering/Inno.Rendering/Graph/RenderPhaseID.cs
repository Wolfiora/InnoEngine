using System;

namespace Inno.Rendering;

/// <summary>
/// Identifies an open render phase protocol value.
/// </summary>
public readonly record struct RenderPhaseID
{
    /// <summary>
    /// Creates an open render phase identifier.
    /// </summary>
    /// <param name="value">
    /// Globally stable phase value.
    /// </param>
    public RenderPhaseID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets the globally stable phase value.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Formats this value as a human-readable representation.
    /// </summary>
    /// <returns>
    /// The human-readable representation of this value.
    /// </returns>
    public override string ToString() => value;
}
