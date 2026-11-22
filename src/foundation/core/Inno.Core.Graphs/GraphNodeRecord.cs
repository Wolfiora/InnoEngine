using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inno.Core.Graphs;

/// <summary>
/// Stores one node without retaining a CLR type or runtime extension instance.
/// </summary>
public sealed class GraphNodeRecord
{
    private readonly Dictionary<string, GraphSerializedValue> m_values = new(StringComparer.Ordinal);
    private readonly ReadOnlyDictionary<string, GraphSerializedValue> m_valueView;

    /// <summary>
    /// Creates a graph node record.
    /// </summary>
    /// <param name="id">
    /// Stable identifier within the document.
    /// </param>
    /// <param name="definitionID">
    /// Stable node definition identifier.
    /// </param>
    public GraphNodeRecord(
        GraphNodeID id,
        string definitionID
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(definitionID);
        this.id = id;
        this.definitionID = definitionID;
        m_valueView = new(m_values);
    }

    /// <summary>
    /// Gets the stable node identifier.
    /// </summary>
    public GraphNodeID id { get; }

    /// <summary>
    /// Gets the stable node definition identifier.
    /// </summary>
    public string definitionID { get; }

    /// <summary>
    /// Gets or sets the graph-space node position.
    /// </summary>
    public GraphPosition position { get; set; }

    /// <summary>
    /// Gets neutral serialized property values keyed by stable property identifier.
    /// </summary>
    public IReadOnlyDictionary<string, GraphSerializedValue> values => m_valueView;

    /// <summary>
    /// Creates or replaces a neutral serialized property value.
    /// </summary>
    /// <param name="propertyID">
    /// Stable property identifier.
    /// </param>
    /// <param name="value">
    /// Serialized value.
    /// </param>
    public void SetValue(
        string propertyID,
        GraphSerializedValue value
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyID);
        ArgumentNullException.ThrowIfNull(value);
        m_values[propertyID] = value;
    }

    /// <summary>
    /// Tries to read a neutral serialized property value.
    /// </summary>
    /// <param name="propertyID">
    /// Stable property identifier.
    /// </param>
    /// <param name="value">
    /// Receives the value when present.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the property exists; otherwise <see langword="false"/>.
    /// </returns>
    public bool TryGetValue(
        string propertyID,
        out GraphSerializedValue? value
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyID);
        return m_values.TryGetValue(propertyID, out value);
    }

    /// <summary>
    /// Removes a neutral serialized property value.
    /// </summary>
    /// <param name="propertyID">
    /// Stable property identifier.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a property was removed; otherwise <see langword="false"/>.
    /// </returns>
    public bool RemoveValue(string propertyID)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyID);
        return m_values.Remove(propertyID);
    }
}
