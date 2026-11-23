using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Graphs;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Editor.Interactions;

namespace Inno.Editor.Graph;

/// <summary>
/// Applies neutral graph edits atomically and records every data mutation in reload-safe Editor History.
/// </summary>
public sealed class GraphDocumentController
{
    private readonly Identity m_identity;
    private readonly IEditorHistory m_history;
    private readonly SerializationRegistry m_serialization;

    internal GraphDocumentController(
        GraphDocumentSession session,
        IEditorHistory history,
        SerializationRegistry serialization
    ) {
        m_identity = session.identity;
        m_history = history;
        m_serialization = serialization;
    }

    /// <summary>
    /// Gets the stable project-relative document identity.
    /// </summary>
    public Guid documentID => m_identity.persistentID;

    /// <summary>
    /// Gets whether the current document identity and source are both available.
    /// </summary>
    public bool isAvailable => m_identity.Resolve<GraphDocumentSession>() is { available: true };

    /// <summary>
    /// Gets the mutable neutral document used by the current asset generation.
    /// </summary>
    public GraphDocument document => session.document;

    /// <summary>
    /// Gets the monotonic in-session content revision.
    /// </summary>
    public ulong revision => session.revision;

    /// <summary>
    /// Gets whether content changed after its last explicit saved marker.
    /// </summary>
    public bool isDirty => session.isDirty;

    /// <summary>
    /// Marks the current revision as saved without creating a data History entry.
    /// </summary>
    public void MarkSaved() => session.isDirty = false;

    /// <summary>
    /// Replaces the complete neutral document as one atomic, undoable authoring operation.
    /// </summary>
    /// <param name="replacement">
    /// Complete replacement document copied by the operation.
    /// </param>
    /// <param name="historyName">
    /// Concise artist-facing history entry name.
    /// </param>
    /// <param name="gestureID">
    /// Unique active gesture identity for continuous samples, or null for an independent edit.
    /// </param>
    public void ReplaceDocument(
        GraphDocument replacement,
        string historyName,
        string? gestureID = null
    ) {
        ArgumentNullException.ThrowIfNull(replacement);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        GraphDocument snapshot = replacement.Clone();
        Mutate(
            historyName,
            gestureID is null ? null : $"graph:{documentID}:document:{gestureID}",
            () => document.ReplaceContents(snapshot)
        );
    }

    /// <summary>
    /// Adds a node with a generated stable identity.
    /// </summary>
    /// <param name="definitionID">
    /// Stable node definition ID.
    /// </param>
    /// <param name="position">
    /// Initial graph-space position.
    /// </param>
    /// <param name="values">
    /// Optional neutral property values.
    /// </param>
    /// <returns>
    /// The new stable node identity.
    /// </returns>
    public GraphNodeID AddNode(
        string definitionID,
        GraphPosition position,
        IReadOnlyDictionary<string, GraphSerializedValue>? values = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(definitionID);
        GraphNodeID id = NewNodeID();
        Mutate("Create Node", null, () =>
        {
            var node = new GraphNodeRecord(id, definitionID) { position = position };
            if (values is not null)
            {
                foreach ((string propertyID, GraphSerializedValue value) in values)
                {
                    node.SetValue(propertyID, value.Clone());
                }
            }

            document.AddNode(node);
        });
        return id;
    }

    /// <summary>
    /// Removes nodes and all incident connections as one structural edit.
    /// </summary>
    /// <param name="nodeIDs">
    /// Nodes to remove.
    /// </param>
    public void RemoveNodes(IEnumerable<GraphNodeID> nodeIDs)
    {
        ArgumentNullException.ThrowIfNull(nodeIDs);
        GraphNodeID[] ids = nodeIDs.Distinct().ToArray();
        if (ids.Length == 0)
        {
            return;
        }

        foreach (GraphNodeID id in ids)
        {
            RequireNode(id);
        }

        Mutate(ids.Length == 1 ? "Delete Node" : "Delete Nodes", null, () =>
        {
            foreach (GraphNodeID id in ids)
            {
                document.RemoveNode(id);
            }
        });
    }

    /// <summary>
    /// Moves one or more nodes as one completed gesture, independently undoable from earlier drags.
    /// </summary>
    /// <param name="positions">
    /// Complete destination positions keyed by node identity.
    /// </param>
    public void MoveNodes(IReadOnlyDictionary<GraphNodeID, GraphPosition> positions)
    {
        ArgumentNullException.ThrowIfNull(positions);
        if (positions.Count == 0)
        {
            return;
        }

        foreach (GraphNodeID id in positions.Keys)
        {
            RequireNode(id);
        }

        Mutate(
            positions.Count == 1 ? "Move Node" : "Move Nodes",
            null,
            () =>
            {
                foreach ((GraphNodeID id, GraphPosition position) in positions)
                {
                    document.FindNode(id)!.position = position;
                }
            }
        );
    }

    /// <summary>
    /// Creates or reconnects one input endpoint to an output endpoint.
    /// </summary>
    /// <param name="output">
    /// Source endpoint.
    /// </param>
    /// <param name="input">
    /// Destination endpoint.
    /// </param>
    /// <returns>
    /// The generated connection identity.
    /// </returns>
    public GraphEdgeID Connect(
        GraphEndpoint output,
        GraphEndpoint input
    ) {
        RequireNode(output.nodeID);
        RequireNode(input.nodeID);
        if (output.nodeID == input.nodeID && output.portID == input.portID)
        {
            throw new ArgumentException("A graph endpoint cannot connect to itself.", nameof(input));
        }

        GraphEdgeID edgeID = NewEdgeID();
        Mutate("Connect Nodes", null, () =>
        {
            GraphEdgeID[] replaced = document.edges
                .Where(edge => edge.input == input)
                .Select(static edge => edge.id)
                .ToArray();
            foreach (GraphEdgeID existing in replaced)
            {
                document.RemoveEdge(existing);
            }

            document.AddEdge(new GraphEdgeRecord(edgeID, output, input));
        });
        return edgeID;
    }

    /// <summary>
    /// Removes one stable connection.
    /// </summary>
    /// <param name="edgeID">
    /// Connection to remove.
    /// </param>
    /// <returns>
    /// <see langword="true"/> after a connection was removed.
    /// </returns>
    public bool Disconnect(GraphEdgeID edgeID)
    {
        if (!document.edges.Any(edge => edge.id == edgeID))
        {
            return false;
        }

        Mutate("Disconnect Nodes", null, () => document.RemoveEdge(edgeID));
        return true;
    }

    /// <summary>
    /// Creates or replaces one neutral node property; only samples carrying the same explicit gesture identity merge.
    /// </summary>
    /// <param name="nodeID">
    /// Owning node.
    /// </param>
    /// <param name="propertyID">
    /// Stable property ID.
    /// </param>
    /// <param name="value">
    /// New neutral value.
    /// </param>
    /// <param name="gestureID">
    /// Unique current text/numeric gesture identity, or null for an independent edit.
    /// </param>
    public void SetNodeValue(
        GraphNodeID nodeID,
        string propertyID,
        GraphSerializedValue value,
        string? gestureID = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyID);
        ArgumentNullException.ThrowIfNull(value);
        GraphNodeRecord node = RequireNode(nodeID);
        Mutate(
            "Edit Node Property",
            gestureID is null ? null : $"graph:{documentID}:value:{nodeID.value}:{propertyID}:{gestureID}",
            () => node.SetValue(propertyID, value.Clone())
        );
    }

    /// <summary>
    /// Removes one neutral node property as a structural edit.
    /// </summary>
    /// <param name="nodeID">
    /// Owning node.
    /// </param>
    /// <param name="propertyID">
    /// Stable property ID.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the property existed.
    /// </returns>
    public bool RemoveNodeValue(
        GraphNodeID nodeID,
        string propertyID
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyID);
        GraphNodeRecord node = RequireNode(nodeID);
        if (!node.values.ContainsKey(propertyID))
        {
            return false;
        }

        Mutate("Remove Node Property", null, () => node.RemoveValue(propertyID));
        return true;
    }

    /// <summary>
    /// Copies selected nodes and only their internal connections.
    /// </summary>
    /// <param name="nodeIDs">
    /// Nodes to copy.
    /// </param>
    /// <returns>
    /// A detached neutral clipboard fragment.
    /// </returns>
    public GraphClipboardData Copy(IEnumerable<GraphNodeID> nodeIDs)
    {
        ArgumentNullException.ThrowIfNull(nodeIDs);
        HashSet<GraphNodeID> selected = [.. nodeIDs];
        var fragment = new GraphDocument();
        foreach (GraphNodeRecord source in document.nodes.Where(node => selected.Contains(node.id)))
        {
            fragment.AddNode(CloneNode(source, source.id, source.position));
        }

        foreach (GraphEdgeRecord edge in document.edges.Where(
            edge => selected.Contains(edge.output.nodeID) && selected.Contains(edge.input.nodeID)
        )) {
            fragment.AddEdge(new GraphEdgeRecord(edge.id, edge.output, edge.input));
        }

        return new GraphClipboardData(fragment);
    }

    /// <summary>
    /// Pastes a clipboard fragment with new stable identities and a graph-space offset.
    /// </summary>
    /// <param name="clipboard">
    /// Detached neutral fragment.
    /// </param>
    /// <param name="offset">
    /// Offset added to every copied node position.
    /// </param>
    /// <param name="remapNode">
    /// Optional synchronous domain remapping of neutral node values after identity allocation. The callback is never stored in History.
    /// </param>
    /// <returns>
    /// New node identities in clipboard document order.
    /// </returns>
    public IReadOnlyList<GraphNodeID> Paste(
        GraphClipboardData clipboard,
        GraphPosition offset,
        Action<GraphNodeRecord, IReadOnlyDictionary<GraphNodeID, GraphNodeID>>? remapNode = null
    ) {
        ArgumentNullException.ThrowIfNull(clipboard);
        GraphDocument fragment = clipboard.CloneFragment();
        Dictionary<GraphNodeID, GraphNodeID> remap = [];
        foreach (GraphNodeRecord node in fragment.nodes)
        {
            remap.Add(node.id, NewNodeID());
        }

        Mutate("Paste Nodes", null, () =>
        {
            foreach (GraphNodeRecord source in fragment.nodes)
            {
                GraphNodeRecord clone = CloneNode(
                    source,
                    remap[source.id],
                    new GraphPosition(source.position.x + offset.x, source.position.y + offset.y)
                );
                remapNode?.Invoke(clone, remap);
                document.AddNode(clone);
            }

            foreach (GraphEdgeRecord edge in fragment.edges)
            {
                document.AddEdge(
                    new GraphEdgeRecord(
                        NewEdgeID(),
                        new GraphEndpoint(remap[edge.output.nodeID], edge.output.portID),
                        new GraphEndpoint(remap[edge.input.nodeID], edge.input.portID)
                    )
                );
            }
        });
        return fragment.nodes.Select(node => remap[node.id]).ToArray();
    }

    private GraphDocumentSession session
        => m_identity.Resolve<GraphDocumentSession>()
           ?? throw new InvalidOperationException("The Graph document controller belongs to a closed identity generation.");

    private void Mutate(
        string name,
        string? mergeKey,
        Action mutation
    ) {
        if (!session.available)
        {
            throw new InvalidOperationException($"Graph document '{documentID}' is closed.");
        }

        byte[] before = GraphHistoryDocumentCodec.Encode(document, m_serialization);
        try
        {
            mutation();
            byte[] after = GraphHistoryDocumentCodec.Encode(document, m_serialization);
            if (before.AsSpan().SequenceEqual(after))
            {
                return;
            }

            EditorHistoryChange change = GraphHistoryData.CreateChange(documentID, before, after, mergeKey);
            try
            {
                m_history.RecordApplied(name, change);
            }
            catch
            {
                change.Dispose();
                throw;
            }

            session.revision++;
            session.isDirty = true;
        }
        catch
        {
            document.ReplaceContents(GraphHistoryDocumentCodec.Decode(before, m_serialization));
            throw;
        }
    }

    private GraphNodeRecord RequireNode(GraphNodeID nodeID)
        => document.FindNode(nodeID)
           ?? throw new ArgumentException($"Node '{nodeID}' is not present in '{documentID}'.", nameof(nodeID));

    private static GraphNodeRecord CloneNode(
        GraphNodeRecord source,
        GraphNodeID id,
        GraphPosition position
    ) {
        var clone = new GraphNodeRecord(id, source.definitionID) { position = position };
        foreach ((string propertyID, GraphSerializedValue value) in source.values)
        {
            clone.SetValue(propertyID, value.Clone());
        }

        return clone;
    }

    private static GraphNodeID NewNodeID() => new(Guid.NewGuid().ToString("N"));

    private static GraphEdgeID NewEdgeID() => new(Guid.NewGuid().ToString("N"));
}
