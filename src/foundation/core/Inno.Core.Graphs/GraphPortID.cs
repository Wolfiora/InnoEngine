using System;

namespace Inno.Core.Graphs;

/// <summary>
/// Identifies a port within its owning node definition.
/// </summary>
public readonly record struct GraphPortID
{
    /// <summary>
    /// Creates a stable graph port identifier.
    /// </summary>
    /// <param name="value">
    /// Non-empty identifier value.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is empty.
    /// </exception>
    public GraphPortID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets the serialized identifier value.
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
