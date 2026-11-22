using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Numerics;
using Inno.Core.Graphs;
using Inno.Editor.Interactions;

namespace Inno.Editor.Graph;

/// <summary>
/// Stores transient pan, zoom, selection and connection interaction state without ImGui dependencies.
/// </summary>
public sealed class GraphCanvasState
{
    private readonly HashSet<GraphNodeID> m_selectedNodes = [];
    private readonly HashSet<GraphEdgeID> m_selectedEdges = [];
    private FrozenSet<GraphNodeID>? m_nodeSelection;
    private FrozenSet<GraphEdgeID>? m_edgeSelection;

    /// <summary>
    /// Gets the graph-space origin mapped to canvas screen origin.
    /// </summary>
    public GraphPosition pan { get; private set; }

    /// <summary>
    /// Gets the current graph-to-screen scale.
    /// </summary>
    public float zoom { get; private set; } = 1f;

    /// <summary>
    /// Gets selected node identities.
    /// </summary>
    public IReadOnlyCollection<GraphNodeID> selectedNodes => m_nodeSelection ??= m_selectedNodes.ToFrozenSet();

    /// <summary>
    /// Gets selected edge identities.
    /// </summary>
    public IReadOnlyCollection<GraphEdgeID> selectedEdges => m_edgeSelection ??= m_selectedEdges.ToFrozenSet();

    /// <summary>
    /// Gets the endpoint currently being connected, or <see langword="null"/>.
    /// </summary>
    public GraphEndpoint? pendingConnection { get; private set; }

    /// <summary>
    /// Gets the direction of <see cref="pendingConnection"/>.
    /// </summary>
    public GraphPortDirection? pendingConnectionDirection { get; private set; }

    /// <summary>
    /// Restores persistent canvas navigation state with bounded zoom.
    /// </summary>
    /// <param name="pan">
    /// Graph-space pan offset.
    /// </param>
    /// <param name="zoom">
    /// Requested zoom in the inclusive range 0.1 to 4.
    /// </param>
    public void SetViewport(
        GraphPosition pan,
        float zoom
    ) {
        if (!float.IsFinite(pan.x) || !float.IsFinite(pan.y))
        {
            throw new ArgumentOutOfRangeException(nameof(pan));
        }
        if (!float.IsFinite(zoom))
        {
            throw new ArgumentOutOfRangeException(nameof(zoom));
        }

        this.pan = pan;
        this.zoom = Math.Clamp(zoom, 0.1f, 4f);
    }

    /// <summary>
    /// Moves the canvas origin by screen-space pixels.
    /// </summary>
    /// <param name="deltaX">
    /// Horizontal screen-space delta.
    /// </param>
    /// <param name="deltaY">
    /// Vertical screen-space delta.
    /// </param>
    public void PanBy(
        float deltaX,
        float deltaY
    ) => SetViewport(new GraphPosition(pan.x + deltaX, pan.y + deltaY), zoom);

    /// <summary>
    /// Changes zoom while preserving the graph point beneath a screen-space pivot.
    /// </summary>
    /// <param name="factor">
    /// Positive multiplicative zoom factor.
    /// </param>
    /// <param name="pivotX">
    /// Screen-space pivot X.
    /// </param>
    /// <param name="pivotY">
    /// Screen-space pivot Y.
    /// </param>
    public void ZoomAt(
        float factor,
        float pivotX,
        float pivotY
    ) {
        if (!float.IsFinite(factor) || factor <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(factor));
        }

        float next = Math.Clamp(zoom * factor, 0.1f, 4f);
        Vector2 origin = EditorPlanarNavigation.ZoomOrigin(new(pan.x, pan.y), new(pivotX, pivotY), zoom, next);
        pan = new GraphPosition(origin.X, origin.Y);
        zoom = next;
    }

    /// <summary>
    /// Replaces node selection.
    /// </summary>
    /// <param name="nodes">
    /// Complete selected-node set.
    /// </param>
    public void SelectNodes(IEnumerable<GraphNodeID> nodes)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        m_selectedNodes.Clear();
        m_selectedNodes.UnionWith(nodes);
        m_nodeSelection = null;
    }

    /// <summary>
    /// Toggles one node while preserving the remaining selection.
    /// </summary>
    /// <param name="nodeID">
    /// Node to toggle.
    /// </param>
    public void ToggleNode(GraphNodeID nodeID)
    {
        m_nodeSelection = null;
        if (!m_selectedNodes.Remove(nodeID))
        {
            m_selectedNodes.Add(nodeID);
        }
    }

    /// <summary>
    /// Clears all transient node and edge selection.
    /// </summary>
    public void ClearSelection()
    {
        m_selectedNodes.Clear();
        m_selectedEdges.Clear();
        m_nodeSelection = null;
        m_edgeSelection = null;
    }

    /// <summary>
    /// Begins a connection drag from one output endpoint.
    /// </summary>
    /// <param name="output">
    /// Stable output endpoint.
    /// </param>
    public void BeginConnection(GraphEndpoint output) => BeginConnection(output, GraphPortDirection.Output);

    /// <summary>
    /// Begins a connection drag from one endpoint.
    /// </summary>
    /// <param name="endpoint">
    /// Stable endpoint.
    /// </param>
    /// <param name="direction">
    /// Direction of the endpoint within its node interface.
    /// </param>
    public void BeginConnection(
        GraphEndpoint endpoint,
        GraphPortDirection direction
    ) {
        pendingConnection = endpoint;
        pendingConnectionDirection = direction;
    }

    /// <summary>
    /// Cancels any active connection drag.
    /// </summary>
    public void CancelConnection()
    {
        pendingConnection = null;
        pendingConnectionDirection = null;
    }
}
