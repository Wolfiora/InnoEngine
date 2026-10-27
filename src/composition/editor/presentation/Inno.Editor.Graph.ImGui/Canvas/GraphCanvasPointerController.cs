using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Inno.Core.Graphs;
using Inno.Editor.Graph;

namespace Inno.Editor.Graph.ImGui;

/// <summary>
/// Coordinates neutral selection, movement previews, box selection and connection intents for one canvas.
/// </summary>
public sealed class GraphCanvasPointerController
{
    private readonly GraphCanvasState m_state;
    private readonly Dictionary<GraphNodeId, GraphPosition> m_starts = [];
    private readonly Dictionary<GraphNodeId, GraphPosition> m_preview = [];
    private readonly IReadOnlyDictionary<GraphNodeId, GraphPosition> m_previewView;
    private Vector2 m_pointerStart;
    private bool m_dragging;
    private bool m_boxSelecting;
    private GraphNodeId[] m_boxSelection = [];

    /// <summary>
    /// Creates a pointer owner over existing document navigation and selection state.
    /// </summary>
    /// <param name="state">
    /// The existing canvas state, which remains the selection and pending-connection authority.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The state is null.
    /// </exception>
    public GraphCanvasPointerController(GraphCanvasState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        m_state = state;
        m_previewView = new ReadOnlyDictionary<GraphNodeId, GraphPosition>(m_preview);
    }

    /// <summary>
    /// Gets frame-local movement previews; callers must not persist this changing view.
    /// </summary>
    public IReadOnlyDictionary<GraphNodeId, GraphPosition> previewPositions => m_previewView;

    /// <summary>
    /// Gets whether a node movement preview is active.
    /// </summary>
    public bool isMoving => m_dragging;

    /// <summary>
    /// Gets whether a selection box is active.
    /// </summary>
    public bool isBoxSelecting => m_boxSelecting;

    /// <summary>
    /// Gets the initial screen-space corner of the current selection box.
    /// </summary>
    public Vector2 boxStart => m_pointerStart;

    /// <summary>
    /// Cancels transient presentation state without applying or undoing document changes.
    /// </summary>
    public void Cancel()
    {
        m_dragging = m_boxSelecting = false;
        m_starts.Clear();
        m_preview.Clear();
        m_boxSelection = [];
        m_state.CancelConnection();
    }

    /// <summary>
    /// Starts movement for explicit node IDs, supporting domain-owned group-header composition.
    /// </summary>
    /// <param name="layout">
    /// The neutral snapshot supplying initial positions.
    /// </param>
    /// <param name="nodes">
    /// Selected stable graph identities; absent nodes remain selected but are not moved.
    /// </param>
    /// <param name="pointer">
    /// The screen-space gesture start.
    /// </param>
    public void BeginMove(
        GraphCanvasLayout layout,
        IEnumerable<GraphNodeId> nodes,
        Vector2 pointer
    ) {
        ArgumentNullException.ThrowIfNull(layout);
        ArgumentNullException.ThrowIfNull(nodes);
        var starts = new Dictionary<GraphNodeId, GraphPosition>();
        foreach (GraphNodeId id in nodes)
            if (layout.TryGetNode(id, out GraphCanvasNode? node))
                starts.TryAdd(id, node.position);
        Cancel();
        foreach (var entry in starts)
            m_starts.Add(entry.Key, entry.Value);
        m_dragging = m_starts.Count != 0;
        m_pointerStart = pointer;
    }

    /// <summary>
    /// Consumes one routed frame and returns data-editing intents without invoking domain code.
    /// </summary>
    /// <param name="view">
    /// The current canvas projection.
    /// </param>
    /// <param name="layout">
    /// The immutable neutral node snapshot used by drawing and hit testing.
    /// </param>
    /// <param name="input">
    /// Current pointer input after the caller's focus, navigation and overlay routing.
    /// </param>
    /// <returns>
    /// Completed neutral intents. The feature owns validation, atomic mutation and History recording.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The pointer position is nonfinite.
    /// </exception>
    public GraphCanvasPointerResult Step(
        GraphCanvasView view,
        GraphCanvasLayout layout,
        GraphCanvasPointerInput input
    ) {
        ArgumentNullException.ThrowIfNull(layout);
        if (!float.IsFinite(input.position.X) || !float.IsFinite(input.position.Y))
            throw new ArgumentOutOfRangeException(nameof(input));
        if (input.cancel)
        {
            Cancel();
            return default;
        }
        if (input.readOnly && (m_dragging || m_state.pendingConnection is not null))
            Cancel();
        GraphNodeId? pressed = null;
        if (input.isPressed && input.isHovered)
        {
            GraphCanvasPort? port = view.HitPort(layout, input.position);
            if (!input.readOnly && port is { isMissing: false } endpoint)
                m_state.BeginConnection(endpoint.endpoint, endpoint.direction);
            else
            {
                GraphCanvasNode? node = view.HitNode(layout, input.position);
                if (node is not null && input.position.Y <= view.NodeBounds(node).minimum.Y + node.headerHeight * view.zoom)
                {
                    pressed = node.id;
                    if (input.shift)
                        m_state.ToggleNode(node.id);
                    else if (!m_state.selectedNodes.Contains(node.id))
                        m_state.SelectNodes([node.id]);
                    if (!input.readOnly)
                        BeginMove(layout, m_state.selectedNodes, input.position);
                }
                else if (node is null)
                {
                    if (!input.shift)
                        m_state.ClearSelection();
                    m_boxSelecting = true;
                    m_boxSelection = input.shift ? m_state.selectedNodes.ToArray() : [];
                    m_pointerStart = input.position;
                }
            }
        }
        if (m_dragging && input.isDown)
        {
            GraphPosition start = view.ToGraph(m_pointerStart);
            GraphPosition current = view.ToGraph(input.position);
            foreach ((GraphNodeId id, GraphPosition position) in m_starts)
                m_preview[id] = new(position.x + current.x - start.x, position.y + current.y - start.y);
        }
        if (!input.isReleased)
            return new(pressed, null, null, null, null);

        IReadOnlyDictionary<GraphNodeId, GraphPosition>? moved = null;
        if (m_dragging && m_preview.Any(entry => m_starts[entry.Key] != entry.Value))
            moved = new ReadOnlyDictionary<GraphNodeId, GraphPosition>(new Dictionary<GraphNodeId, GraphPosition>(m_preview));
        if (m_boxSelecting)
            m_state.SelectNodes(m_boxSelection.Concat(view.BoxNodes(layout, m_pointerStart, input.position)).Distinct());
        GraphEndpoint? output = null;
        GraphEndpoint? targetInput = null;
        GraphEndpoint? released = null;
        if (!input.readOnly && input.isHovered && m_state.pendingConnection is GraphEndpoint source
            && m_state.pendingConnectionDirection is GraphPortDirection direction)
        {
            GraphCanvasPort? destination = view.HitPort(layout, input.position);
            if (destination is { isMissing: false } target && target.direction != direction)
            {
                output = direction == GraphPortDirection.Output ? source : target.endpoint;
                targetInput = direction == GraphPortDirection.Input ? source : target.endpoint;
            }
            else if (destination is null && direction == GraphPortDirection.Output
                && input.isHovered && view.HitNode(layout, input.position) is null)
                released = source;
        }
        Cancel();
        return new(pressed, moved, output, targetInput, released);
    }
}
