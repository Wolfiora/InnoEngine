using System;

namespace Inno.Core.Graphs;

/// <summary>
/// Identifies an edge within one graph document using a stable serialized value.
/// </summary>
public readonly record struct GraphEdgeID
{
    /// <summary>
    /// Creates a stable graph edge identifier.
    /// </summary>
    /// <param name="value">
    /// Non-empty identifier value.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is empty.
    /// </exception>
    public GraphEdgeID(string value)
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
