using System;

namespace Inno.Core.Graphs;

/// <summary>
/// Describes one dynamically or statically resolved node port.
/// </summary>
public sealed class GraphPortDefinition
{
    /// <summary>
    /// Creates a graph port definition.
    /// </summary>
    /// <param name="id">
    /// Stable node-local port identifier.
    /// </param>
    /// <param name="displayName">
    /// Artist-facing display name.
    /// </param>
    /// <param name="valueTypeID">
    /// Stable value type identifier.
    /// </param>
    /// <param name="direction">
    /// Value-flow direction.
    /// </param>
    /// <param name="capacity">
    /// Allowed edge capacity.
    /// </param>
    /// <param name="required">
    /// Whether an input must be connected.
    /// </param>
    public GraphPortDefinition(
        GraphPortID id,
        string displayName,
        string valueTypeID,
        GraphPortDirection direction,
        GraphPortCapacity capacity = GraphPortCapacity.Single,
        bool required = false
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        ArgumentException.ThrowIfNullOrWhiteSpace(valueTypeID);
        this.id = id;
        this.displayName = displayName;
        this.valueTypeID = valueTypeID;
        this.direction = direction;
        this.capacity = capacity;
        this.required = required;
    }

    /// <summary>
    /// Gets the stable node-local port identifier.
    /// </summary>
    public GraphPortID id { get; }

    /// <summary>
    /// Gets the artist-facing display name.
    /// </summary>
    public string displayName { get; }

    /// <summary>
    /// Gets the stable value type identifier.
    /// </summary>
    public string valueTypeID { get; }

    /// <summary>
    /// Gets the value-flow direction.
    /// </summary>
    public GraphPortDirection direction { get; }

    /// <summary>
    /// Gets the allowed edge capacity.
    /// </summary>
    public GraphPortCapacity capacity { get; }

    /// <summary>
    /// Gets whether an input must be connected.
    /// </summary>
    public bool required { get; }
}
