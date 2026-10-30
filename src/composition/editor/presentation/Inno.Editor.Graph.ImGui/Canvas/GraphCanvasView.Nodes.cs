using System;
using System.Collections.Generic;
using System.Numerics;
using Inno.Core.Graphs;
using Inno.Editor.ImGui;
using CanvasDrawing = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Graph.ImGui;

/// <summary>
/// Draws and hit-tests neutral graph nodes using the frame's frozen viewport projection.
/// </summary>
public readonly partial struct GraphCanvasView
{
    /// <summary>
    /// Projects neutral node geometry, optionally substituting a transient drag position.
    /// </summary>
    /// <param name="node">
    /// The immutable display node.
    /// </param>
    /// <param name="previewPosition">
    /// An uncommitted gesture position, or null to use the document position.
    /// </param>
    /// <returns>
    /// The screen-space node bounds.
    /// </returns>
    public (Vector2 minimum, Vector2 maximum) NodeBounds(
        GraphCanvasNode node,
        GraphPosition? previewPosition = null
    ) {
        ArgumentNullException.ThrowIfNull(node);
        Vector2 minimum = ToScreen(previewPosition ?? node.position);
        return (minimum, minimum + node.size * zoom);
    }

    /// <summary>
    /// Finds the frontmost node beneath a pointer using the same geometry as drawing.
    /// </summary>
    /// <param name="layout">
    /// The current document presentation snapshot.
    /// </param>
    /// <param name="point">
    /// The screen-space pointer.
    /// </param>
    /// <returns>
    /// The frontmost neutral node, or null over empty canvas.
    /// </returns>
    public GraphCanvasNode? HitNode(
        GraphCanvasLayout layout,
        Vector2 point
    ) {
        ArgumentNullException.ThrowIfNull(layout);
        for (int index = layout.nodes.Count - 1; index >= 0; index--)
        {
            GraphCanvasNode node = layout.nodes[index];
            var bounds = NodeBounds(node);
            if (Contains(bounds, point))
                return node;
        }
        return null;
    }

    /// <summary>
    /// Finds a port by its displayed socket or label row, without interpreting its domain type.
    /// </summary>
    /// <param name="layout">
    /// The current document presentation snapshot.
    /// </param>
    /// <param name="point">
    /// The screen-space pointer.
    /// </param>
    /// <param name="radius">
    /// The nonnegative minimum socket hit radius in screen pixels.
    /// </param>
    /// <returns>
    /// The hit neutral port, including Missing state, or null over non-port content.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The radius is negative or nonfinite.
    /// </exception>
    public GraphCanvasPort? HitPort(
        GraphCanvasLayout layout,
        Vector2 point,
        float radius = 8f
    ) {
        ArgumentNullException.ThrowIfNull(layout);
        if (!float.IsFinite(radius) || radius < 0f)
            throw new ArgumentOutOfRangeException(nameof(radius));
        float hitRadius = MathF.Max(radius, 7f * zoom);
        for (int index = layout.nodes.Count - 1; index >= 0; index--)
        {
            GraphCanvasNode node = layout.nodes[index];
            Vector2 minimum = ToScreen(node.position);
            foreach (GraphCanvasPort port in node.ports)
                if (Vector2.DistanceSquared(point, minimum + port.position * m_zoom) <= hitRadius * hitRadius)
                    return port;
            if (!Contains(NodeBounds(node), point))
                continue;
            foreach (GraphCanvasPort port in node.ports)
                if (port.labelBounds is { } label && Contains(
                    (ToScreen(node.position) + label.minimum * m_zoom, ToScreen(node.position) + label.maximum * m_zoom), point))
                    return port;
            // Covered nodes must not receive interaction through the frontmost node's body.
            return null;
        }
        return null;
    }

    /// <summary>
    /// Finds nodes intersecting a screen-space selection rectangle in document draw order.
    /// </summary>
    /// <param name="layout">
    /// The current neutral display snapshot.
    /// </param>
    /// <param name="first">
    /// One corner of the selection box.
    /// </param>
    /// <param name="second">
    /// The opposite corner, in any drag direction.
    /// </param>
    /// <returns>
    /// Stable graph node IDs; no live domain objects are returned.
    /// </returns>
    public IReadOnlyList<GraphNodeId> BoxNodes(
        GraphCanvasLayout layout,
        Vector2 first,
        Vector2 second
    ) {
        ArgumentNullException.ThrowIfNull(layout);
        Vector2 minimum = Vector2.Min(first, second);
        Vector2 maximum = Vector2.Max(first, second);
        var nodes = new List<GraphNodeId>();
        foreach (GraphCanvasNode node in layout.nodes)
        {
            var bounds = NodeBounds(node);
            if (bounds.maximum.X >= minimum.X && bounds.minimum.X <= maximum.X
                && bounds.maximum.Y >= minimum.Y && bounds.minimum.Y <= maximum.Y)
                nodes.Add(node.id);
        }
        return nodes.AsReadOnly();
    }

    /// <summary>
    /// Draws a visible neutral node and its ports, borrowing optional content only for this call.
    /// </summary>
    /// <param name="draw">
    /// The current window's frame-local drawing borrow; it is neither retained nor released by this call.
    /// </param>
    /// <param name="node">
    /// The immutable geometric and textual node input.
    /// </param>
    /// <param name="headerColor">
    /// The domain-selected packed header color; the presenter does not classify node types.
    /// </param>
    /// <param name="selected">
    /// Whether to display the current selection outline.
    /// </param>
    /// <param name="drawContent">
    /// Optional frame-local custom drawing. Exceptions propagate after restoring the node clip.
    /// </param>
    /// <param name="previewPosition">
    /// An uncommitted movement position, or null to draw the document position.
    /// </param>
    /// <returns>
    /// True when the node intersects the canvas and effective window clip and was submitted; false when culled.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The node is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The canvas is uninitialized or a visible node uses an expired or mismatched drawing borrow.
    /// </exception>
    public bool DrawNode(
        EditorDrawList draw,
        GraphCanvasNode node,
        uint headerColor,
        bool selected,
        Action? drawContent = null,
        GraphPosition? previewPosition = null
    ) {
        var bounds = NodeBounds(node, previewPosition);
        Vector2 minimum = bounds.minimum;
        Vector2 maximum = bounds.maximum;
        if (m_size.X == 0f || m_size.Y == 0f || maximum.X < m_origin.X || minimum.X > m_origin.X + m_size.X
            || maximum.Y < m_origin.Y || minimum.Y > m_origin.Y + m_size.Y)
            return false;
        var clip = CanvasDrawing.GetClipBounds();
        if (maximum.X < clip.minimum.X || minimum.X > clip.maximum.X || maximum.Y < clip.minimum.Y || minimum.Y > clip.maximum.Y)
            return false;
        float zoom = this.zoom;
        draw.PushClip(Vector2.Max(m_origin, minimum - new Vector2(6f * zoom)),
            Vector2.Min(m_origin + m_size, maximum + new Vector2(6f * zoom)), true);
        try
        {
            draw.FilledRectangle(minimum + new Vector2(3, 5), maximum + new Vector2(3, 5), Pack(0f, 0f, 0f, EditorPalette.opacityMuted), 6f * zoom, false);
            draw.FilledRectangle(minimum, maximum, Pack(0.115f, 0.12f, 0.14f), 6f * zoom, false);
            draw.FilledRectangle(minimum, new(maximum.X, minimum.Y + node.headerHeight * zoom), headerColor, 6f * zoom, true);
            draw.Rectangle(minimum, maximum, selected ? Pack(0.65f, 0.47f, 0.88f) : Pack(0.26f, 0.27f, 0.31f), 6f * zoom, selected ? 2f : 1f);
            draw.Text(minimum + new Vector2(10f, 7f) * zoom, CanvasDrawing.GetStyleColor(ImGuiCol.Text), node.title, zoom);
            foreach (GraphCanvasPort port in node.ports)
            {
                Vector2 point = minimum + port.position * zoom;
                uint color = port.isMissing ? Pack(0.95f, 0.35f, 0.3f) : Pack(0.60f, 0.48f, 0.84f);
                draw.FilledCircle(point, 5f * zoom, color);
                if (port.isOptional && !port.isMissing)
                    draw.FilledCircle(point, 2.75f * zoom, Pack(0.115f, 0.12f, 0.14f));
                if (port.labelBounds is not { } labelBounds)
                    continue;
                Vector2 labelMinimum = minimum + labelBounds.minimum * zoom;
                Vector2 labelMaximum = minimum + labelBounds.maximum * zoom;
                float padding = 12f * zoom;
                var label = FitLabel(port.label, labelMaximum.X - labelMinimum.X - padding * 2f, zoom);
                float textX = port.direction == GraphPortDirection.Input ? labelMinimum.X + padding
                    : labelMaximum.X - padding - label.width;
                draw.PushClip(labelMinimum, labelMaximum, true);
                try
                {
                    draw.Text(new Vector2(textX, point.Y - 8f * zoom),
                        CanvasDrawing.ToPackedColor(port.isOptional ? EditorPalette.textDisabled : EditorPalette.text), label.text, zoom);
                }
                finally
                {
                    draw.PopClip();
                }
            }
            drawContent?.Invoke();
        }
        finally
        {
            draw.PopClip();
        }
        return true;
    }

    private static (string text, float width) FitLabel(
        string label,
        float width,
        float zoom
    ) {
        float measuredWidth = CanvasDrawing.MeasureText(label).X * zoom;
        if (measuredWidth <= width)
            return (label, measuredWidth);
        const string ellipsis = "…";
        if (CanvasDrawing.MeasureText(ellipsis).X * zoom > width)
            return (string.Empty, 0f);
        int low = 0;
        int high = label.Length;
        while (low < high)
        {
            int length = (low + high + 1) / 2;
            if (CanvasDrawing.MeasureText(label[..length] + ellipsis).X * zoom <= width)
                low = length;
            else
                high = length - 1;
        }
        // Avoid splitting a UTF-16 surrogate pair when a label contains supplementary characters.
        if (low > 0 && low < label.Length && char.IsHighSurrogate(label[low - 1]))
            low--;
        string text = label[..low] + ellipsis;
        return (text, CanvasDrawing.MeasureText(text).X * zoom);
    }

    private static bool Contains(
        (Vector2 minimum, Vector2 maximum) bounds,
        Vector2 point
    ) => point.X >= bounds.minimum.X && point.X <= bounds.maximum.X
        && point.Y >= bounds.minimum.Y && point.Y <= bounds.maximum.Y;

    private static uint Pack(
        float red,
        float green,
        float blue,
        float alpha = 1f
    ) => CanvasDrawing.ToPackedColor(new Vector4(red, green, blue, alpha));
}
