using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Inno.Core.Graphs;

namespace Inno.Editor.Graph.ImGui;

/// <summary>
/// Owns a document's neutral node display snapshot, independently of viewport transforms and domain instances.
/// </summary>
public sealed class GraphCanvasLayout
{
    private IReadOnlyList<GraphCanvasNode> m_nodes = Array.Empty<GraphCanvasNode>();
    private Dictionary<GraphNodeId, GraphCanvasNode> m_byId = [];

    /// <summary>
    /// Gets nodes in draw order; reverse order determines overlapping-node hit priority.
    /// </summary>
    public IReadOnlyList<GraphCanvasNode> nodes => m_nodes;

    /// <summary>
    /// Atomically replaces a complete display snapshot, reusing unchanged node geometry by value.
    /// </summary>
    /// <param name="nodes">
    /// The current neutral node descriptions in draw order.
    /// </param>
    /// <returns>
    /// The number of inserted, changed or removed nodes. Ordering changes do not rebuild geometry.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The collection or one of its nodes is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A node identity occurs more than once; the previous snapshot remains active.
    /// </exception>
    public int SetNodes(IEnumerable<GraphCanvasNode> nodes)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        GraphCanvasNode[] candidate = nodes.ToArray();
        var byId = new Dictionary<GraphNodeId, GraphCanvasNode>(candidate.Length);
        int changes = 0;
        for (int index = 0; index < candidate.Length; index++)
        {
            GraphCanvasNode node = candidate[index];
            ArgumentNullException.ThrowIfNull(node);
            if (m_byId.TryGetValue(node.id, out GraphCanvasNode? current) && Equivalent(current, node))
                candidate[index] = node = current;
            else
                changes++;
            if (!byId.TryAdd(node.id, node))
                throw new ArgumentException("A graph layout cannot contain duplicate node identities.", nameof(nodes));
        }
        changes += m_byId.Keys.Count(id => !byId.ContainsKey(id));
        if (changes == 0 && m_nodes.SequenceEqual(candidate))
            return 0;
        m_nodes = Array.AsReadOnly(candidate);
        m_byId = byId;
        return changes;
    }

    /// <summary>
    /// Resolves a node in this display snapshot without consulting domain or live-object registries.
    /// </summary>
    /// <param name="id">
    /// The graph node identity to resolve.
    /// </param>
    /// <param name="node">
    /// The current neutral node, or null when it is absent.
    /// </param>
    /// <returns>
    /// True when the current snapshot contains the node.
    /// </returns>
    public bool TryGetNode(
        GraphNodeId id,
        [NotNullWhen(true)] out GraphCanvasNode? node
    ) => m_byId.TryGetValue(id, out node);

    private static bool Equivalent(
        GraphCanvasNode left,
        GraphCanvasNode right
    ) => ReferenceEquals(left, right) || left.position == right.position && left.size == right.size
        && left.headerHeight == right.headerHeight && left.title == right.title && left.ports.SequenceEqual(right.ports);
}
