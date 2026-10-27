using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Inno.Core.Graphs;

namespace Inno.Editor.Graph.ImGui;

/// <summary>
/// Freezes one node's geometric and textual presentation without owning its domain record.
/// </summary>
public sealed class GraphCanvasNode
{
    /// <summary>
    /// Creates immutable display data that can safely outlive the extension that described it.
    /// </summary>
    /// <param name="id">
    /// The existing graph node identity.
    /// </param>
    /// <param name="position">
    /// The node's document position in graph coordinates.
    /// </param>
    /// <param name="size">
    /// Finite positive width and height in graph coordinates.
    /// </param>
    /// <param name="headerHeight">
    /// The header extent, within the node height.
    /// </param>
    /// <param name="title">
    /// A display title already interpreted by its domain owner.
    /// </param>
    /// <param name="ports">
    /// Immutable neutral endpoint descriptions; the collection is copied.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The title or port collection is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A node or port identity is invalid, duplicated or owned by a different node.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The node's geometry is nonfinite or has an invalid extent.
    /// </exception>
    public GraphCanvasNode(
        GraphNodeId id,
        GraphPosition position,
        Vector2 size,
        float headerHeight,
        string title,
        IEnumerable<GraphCanvasPort> ports
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id.value);
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(ports);
        if (!float.IsFinite(position.x) || !float.IsFinite(position.y))
            throw new ArgumentOutOfRangeException(nameof(position));
        if (!float.IsFinite(size.X) || !float.IsFinite(size.Y) || size.X <= 0f || size.Y <= 0f)
            throw new ArgumentOutOfRangeException(nameof(size));
        if (!float.IsFinite(headerHeight) || headerHeight < 0f || headerHeight > size.Y)
            throw new ArgumentOutOfRangeException(nameof(headerHeight));
        GraphCanvasPort[] frozen = ports.ToArray();
        if (frozen.Any(port => port.endpoint.nodeId != id)
            || frozen.Select(static port => (port.endpoint, port.direction)).Distinct().Count() != frozen.Length)
            throw new ArgumentException("Node ports require unique endpoint/direction pairs owned by the same node.", nameof(ports));
        this.id = id;
        this.position = position;
        this.size = size;
        this.headerHeight = headerHeight;
        this.title = title;
        this.ports = Array.AsReadOnly(frozen);
    }

    /// <summary>
    /// Gets the stable graph identity represented by this display snapshot.
    /// </summary>
    public GraphNodeId id { get; }

    /// <summary>
    /// Gets the document position, before transient gesture previews.
    /// </summary>
    public GraphPosition position { get; }

    /// <summary>
    /// Gets the unscaled node extent.
    /// </summary>
    public Vector2 size { get; }

    /// <summary>
    /// Gets the unscaled header extent used for node movement hit testing.
    /// </summary>
    public float headerHeight { get; }

    /// <summary>
    /// Gets the domain-supplied display title.
    /// </summary>
    public string title { get; }

    /// <summary>
    /// Gets copied neutral port descriptions; no callback or domain object is retained.
    /// </summary>
    public IReadOnlyList<GraphCanvasPort> ports { get; }
}
