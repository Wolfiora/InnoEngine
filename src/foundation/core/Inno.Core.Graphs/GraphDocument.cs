using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inno.Core.Graphs;

/// <summary>
/// Owns neutral graph records and graph-level canvas metadata.
/// </summary>
public sealed class GraphDocument
{
    private readonly List<GraphNodeRecord> m_nodes = [];
    private readonly List<GraphEdgeRecord> m_edges = [];
    private readonly Dictionary<string, GraphSerializedValue> m_metadata = new(StringComparer.Ordinal);
    private readonly Dictionary<GraphNodeID, GraphNodeRecord> m_nodeIndex = [];
    private readonly ReadOnlyCollection<GraphNodeRecord> m_nodeView;
    private readonly ReadOnlyCollection<GraphEdgeRecord> m_edgeView;
    private readonly ReadOnlyDictionary<string, GraphSerializedValue> m_metadataView;

    /// <summary>
    /// Creates an empty document with stable, read-only collection views.
    /// </summary>
    public GraphDocument()
    {
        m_nodeView = m_nodes.AsReadOnly();
        m_edgeView = m_edges.AsReadOnly();
        m_metadataView = new(m_metadata);
    }

    /// <summary>
    /// Gets all node records in stable document order.
    /// </summary>
    public IReadOnlyList<GraphNodeRecord> nodes => m_nodeView;

    /// <summary>
    /// Gets all edge records in stable document order.
    /// </summary>
    public IReadOnlyList<GraphEdgeRecord> edges => m_edgeView;

    /// <summary>
    /// Gets graph-level neutral metadata such as groups or comments.
    /// </summary>
    public IReadOnlyDictionary<string, GraphSerializedValue> metadata => m_metadataView;

    /// <summary>
    /// Creates a deep neutral copy that shares no mutable node, edge, value, or metadata records.
    /// </summary>
    /// <returns>
    /// An independently mutable graph document with identical stable identifiers and order.
    /// </returns>
    public GraphDocument Clone()
    {
        var clone = new GraphDocument();
        clone.ReplaceContents(this);
        return clone;
    }

    /// <summary>
    /// Atomically replaces all records with deep copies from another neutral document.
    /// </summary>
    /// <param name="source">
    /// Complete source state.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="source"/> is this document.
    /// </exception>
    public void ReplaceContents(GraphDocument source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (ReferenceEquals(this, source))
        {
            throw new ArgumentException("A graph document cannot replace itself.", nameof(source));
        }

        List<GraphNodeRecord> nodes = [];
        Dictionary<GraphNodeID, GraphNodeRecord> nodeIndex = [];
        foreach (GraphNodeRecord sourceNode in source.nodes)
        {
            var node = new GraphNodeRecord(sourceNode.id, sourceNode.definitionID)
            {
                position = sourceNode.position
            };
            foreach ((string propertyID, GraphSerializedValue value) in sourceNode.values)
            {
                node.SetValue(propertyID, value.Clone());
            }

            nodes.Add(node);
            nodeIndex.Add(node.id, node);
        }

        List<GraphEdgeRecord> edges = [];
        foreach (GraphEdgeRecord sourceEdge in source.edges)
        {
            edges.Add(new GraphEdgeRecord(sourceEdge.id, sourceEdge.output, sourceEdge.input));
        }

        var metadata = new Dictionary<string, GraphSerializedValue>(StringComparer.Ordinal);
        foreach ((string key, GraphSerializedValue value) in source.metadata)
        {
            metadata.Add(key, value.Clone());
        }

        m_nodes.Clear();
        m_nodes.AddRange(nodes);
        m_nodeIndex.Clear();
        foreach ((GraphNodeID id, GraphNodeRecord node) in nodeIndex)
        {
            m_nodeIndex.Add(id, node);
        }
        m_edges.Clear();
        m_edges.AddRange(edges);
        m_metadata.Clear();
        foreach ((string key, GraphSerializedValue value) in metadata)
        {
            m_metadata.Add(key, value);
        }
    }

    /// <summary>
    /// Adds a node while preserving document order.
    /// </summary>
    /// <param name="node">
    /// Node record to add.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the node identifier already exists.
    /// </exception>
    public void AddNode(GraphNodeRecord node)
    {
        ArgumentNullException.ThrowIfNull(node);
        if (!m_nodeIndex.TryAdd(node.id, node))
        {
            throw new ArgumentException($"Node '{node.id}' already exists.", nameof(node));
        }

        m_nodes.Add(node);
    }

    /// <summary>
    /// Removes a node and every edge connected to it.
    /// </summary>
    /// <param name="nodeID">
    /// Node identifier to remove.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the node existed; otherwise <see langword="false"/>.
    /// </returns>
    public bool RemoveNode(GraphNodeID nodeID)
    {
        if (!m_nodeIndex.Remove(nodeID, out GraphNodeRecord? node))
        {
            return false;
        }

        m_nodes.Remove(node);
        m_edges.RemoveAll(edge => edge.output.nodeID == nodeID || edge.input.nodeID == nodeID);
        return true;
    }

    /// <summary>
    /// Finds a node by stable identifier.
    /// </summary>
    /// <param name="nodeID">
    /// Node identifier to find.
    /// </param>
    /// <returns>
    /// The node record, or <see langword="null"/> when absent.
    /// </returns>
    public GraphNodeRecord? FindNode(GraphNodeID nodeID) => m_nodeIndex.GetValueOrDefault(nodeID);

    /// <summary>
    /// Adds an edge while preserving document order.
    /// </summary>
    /// <param name="edge">
    /// Edge record to add.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the edge identifier already exists.
    /// </exception>
    public void AddEdge(GraphEdgeRecord edge)
    {
        ArgumentNullException.ThrowIfNull(edge);
        if (m_edges.Exists(candidate => candidate.id == edge.id))
        {
            throw new ArgumentException($"Edge '{edge.id}' already exists.", nameof(edge));
        }

        m_edges.Add(edge);
    }

    /// <summary>
    /// Removes an edge by stable identifier.
    /// </summary>
    /// <param name="edgeID">
    /// Edge identifier to remove.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the edge existed; otherwise <see langword="false"/>.
    /// </returns>
    public bool RemoveEdge(GraphEdgeID edgeID)
    {
        int index = m_edges.FindIndex(edge => edge.id == edgeID);
        if (index < 0)
        {
            return false;
        }

        m_edges.RemoveAt(index);
        return true;
    }

    /// <summary>
    /// Creates or replaces graph-level neutral metadata.
    /// </summary>
    /// <param name="key">
    /// Stable metadata key.
    /// </param>
    /// <param name="value">
    /// Serialized metadata value.
    /// </param>
    public void SetMetadata(
        string key,
        GraphSerializedValue value
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);
        m_metadata[key] = value;
    }

    /// <summary>
    /// Removes graph-level metadata by stable key.
    /// </summary>
    /// <param name="key">
    /// Stable metadata key.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when metadata existed and was removed.
    /// </returns>
    public bool RemoveMetadata(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        return m_metadata.Remove(key);
    }
}
