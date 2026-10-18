using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Inno.Core.Graphs;
using Inno.Editor.ImGui;
using Inno.Native.ImGui;
using ImGuiApi = Inno.Native.ImGui.ImGui;
using Widget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;

namespace Inno.Editor.Panel.ShaderEditor;

internal sealed partial class ShaderEditorCanvas
{
    private const float C_MINI_MAP_WIDTH = 220f;
    private const float C_MINI_MAP_HEIGHT = 145f;
    private const float C_MINI_MAP_MARGIN = 14f;
    private const float C_MINI_MAP_PADDING = 9f;
    private readonly Dictionary<GraphNodeId, MiniMapNodeGeometry> m_miniMapGeometry = [];

    private readonly record struct MiniMapNodeGeometry(
        Vector2 minimum,
        Vector2 maximum
    ) {
        internal Vector2 center => (minimum + maximum) * 0.5f;
    }

    private readonly record struct MiniMapLayout(
        Vector2 worldMinimum,
        Vector2 screenOrigin,
        float scale
    ) {
        internal Vector2 ToScreen(Vector2 position) => screenOrigin + (position - worldMinimum) * scale;

        internal Vector2 ToWorld(Vector2 position) => worldMinimum + (position - screenOrigin) / scale;
    }

    private bool MiniMapContains(Vector2 point)
    {
        if (!TryGetMiniMapBounds(out Vector2 minimum, out Vector2 maximum))
            return false;
        return point.X >= minimum.X && point.X <= maximum.X &&
               point.Y >= minimum.Y && point.Y <= maximum.Y;
    }

    private bool TryGetMiniMapBounds(
        out Vector2 minimum,
        out Vector2 maximum
    ) {
        minimum = maximum = default;
        if (m_size.X < 320f || m_size.Y < 220f)
            return false;

        float zoom = Widget.style.zoom;
        Vector2 size = new(
            MathF.Min(C_MINI_MAP_WIDTH * zoom, m_size.X * 0.30f),
            MathF.Min(C_MINI_MAP_HEIGHT * zoom, m_size.Y * 0.30f));
        maximum = m_origin + new Vector2(m_size.X - C_MINI_MAP_MARGIN * zoom, C_MINI_MAP_MARGIN * zoom + size.Y);
        minimum = maximum - size;
        return true;
    }

    private void DrawMiniMap()
    {
        if (!TryGetMiniMapBounds(out Vector2 minimum, out Vector2 maximum))
            return;

        Vector2 size = maximum - minimum;
        Vector2 cursor = ImGuiApi.GetCursorScreenPos();
        ImGuiApi.SetCursorScreenPos(minimum);
        ImGuiApi.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        ImGuiApi.PushStyleColor(ImGuiCol.ChildBg, new Vector4(0.085f, 0.092f, 0.11f, 0.95f));
        bool began = false;
        try
        {
            bool visible = ImGuiApi.BeginChild(
                "##shader-mini-map",
                size,
                ImGuiChildFlags.Borders,
                ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoSavedSettings);
            began = true;
            if (!visible)
                return;

            MiniMapLayout layout = CreateMiniMapLayout(minimum, maximum);
            ImGuiApi.InvisibleButton(
                "##mini-map-navigation",
                Vector2.Max(ImGuiApi.GetContentRegionAvail(), Vector2.One),
                ImGuiButtonFlags.MouseButtonLeft);
            if (ImGuiApi.IsItemActive() && ImGuiApi.IsMouseDown(ImGuiMouseButton.Left))
            {
                Vector2 graphPoint = layout.ToWorld(Vector2.Clamp(ImGuiApi.GetMousePos(), minimum, maximum));
                Canvas.SetViewport(
                    new(m_size.X * 0.5f - graphPoint.X * Canvas.zoom,
                        m_size.Y * 0.5f - graphPoint.Y * Canvas.zoom),
                    Canvas.zoom);
            }

            DrawMiniMapContents(ImGuiApi.GetWindowDrawList(), layout);
        }
        finally
        {
            if (began)
                ImGuiApi.EndChild();
            ImGuiApi.PopStyleColor();
            ImGuiApi.PopStyleVar();
            ImGuiApi.SetCursorScreenPos(cursor);
        }
    }

    private MiniMapLayout CreateMiniMapLayout(
        Vector2 minimum,
        Vector2 maximum
    ) {
        Vector2 viewMinimum = -new Vector2(Canvas.pan.x, Canvas.pan.y) / Canvas.zoom;
        Vector2 viewMaximum = viewMinimum + m_size / Canvas.zoom;
        Vector2 worldMinimum = viewMinimum;
        Vector2 worldMaximum = viewMaximum;
        m_miniMapGeometry.Clear();
        foreach (GraphNodeRecord node in Controller.document.nodes)
        {
            GraphPosition position = draft.dragPreview.GetValueOrDefault(node.id, node.position);
            Vector2 nodeMinimum = new(position.x, position.y);
            Vector2 nodeMaximum = nodeMinimum + new Vector2(
                NodeWidth(node),
                C_HEADER + Rows(node) * C_ROW + ControlHeight(node) + 20f);
            m_miniMapGeometry.Add(node.id, new(nodeMinimum, nodeMaximum));
            worldMinimum = Vector2.Min(worldMinimum, nodeMinimum);
            worldMaximum = Vector2.Max(worldMaximum, nodeMaximum);
        }

        Vector2 innerMinimum = minimum + new Vector2(C_MINI_MAP_PADDING * Widget.style.zoom);
        Vector2 innerSize = maximum - minimum - new Vector2(C_MINI_MAP_PADDING * Widget.style.zoom * 2f);
        Vector2 worldSize = Vector2.Max(worldMaximum - worldMinimum, Vector2.One);
        float scale = MathF.Min(innerSize.X / worldSize.X, innerSize.Y / worldSize.Y);
        Vector2 screenOrigin = innerMinimum + (innerSize - worldSize * scale) * 0.5f;
        return new MiniMapLayout(worldMinimum, screenOrigin, scale);
    }

    private void DrawMiniMapContents(
        ImDrawListPtr draw,
        MiniMapLayout layout
    ) {
        foreach (GraphEdgeRecord edge in Controller.document.edges)
        {
            if (!m_miniMapGeometry.TryGetValue(edge.output.nodeId, out MiniMapNodeGeometry source) ||
                !m_miniMapGeometry.TryGetValue(edge.input.nodeId, out MiniMapNodeGeometry target))
                continue;
            draw.AddLine(layout.ToScreen(source.center), layout.ToScreen(target.center), Color(0.55f, 0.42f, 0.76f, 0.65f));
        }

        foreach (GraphNodeRecord node in Controller.document.nodes)
        {
            MiniMapNodeGeometry geometry = m_miniMapGeometry[node.id];
            uint fill = Canvas.selectedNodes.Contains(node.id)
                ? Color(0.60f, 0.44f, 0.83f)
                : Color(0.34f, 0.37f, 0.44f);
            draw.AddRectFilled(layout.ToScreen(geometry.minimum), layout.ToScreen(geometry.maximum), fill, 2f);
        }

        Vector2 viewMinimum = -new Vector2(Canvas.pan.x, Canvas.pan.y) / Canvas.zoom;
        Vector2 viewMaximum = viewMinimum + m_size / Canvas.zoom;
        Vector2 screenMinimum = layout.ToScreen(viewMinimum);
        Vector2 screenMaximum = layout.ToScreen(viewMaximum);
        draw.AddRectFilled(screenMinimum, screenMaximum, Color(0.65f, 0.47f, 0.88f, 0.10f));
        draw.AddRect(screenMinimum, screenMaximum, Color(0.76f, 0.62f, 0.95f), 0f, ImDrawFlags.None, 1.5f);
    }

}
