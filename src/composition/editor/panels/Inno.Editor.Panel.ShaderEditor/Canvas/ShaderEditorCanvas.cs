using Inno.Editor.Core;
using Inno.Editor.Graph.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.ImGui;
using Inno.Editor.Interactions;
using Inno.Editor.Rendering;
using Inno.Editor.Shaders;
using Inno.Editor.Shaders.ImGui;
using Inno.Rendering.Assets;
using Inno.Rendering.Shaders;
using ImGuiApi = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.ShaderEditor;
internal sealed partial class ShaderEditorCanvas(
    ShaderEditorDocuments owner,
    ShaderEditorDocuments.Draft draft
) {
    private readonly record struct PortHandle(
        GraphEndpoint endpoint,
        GraphPortDirection direction
    );
    internal const string C_AREA = "panel/rendering.shader-editor";
    private static readonly Vector4 C_EMPTY_GROUP_HEADER = new(0.16f, 0.17f, 0.20f, EditorPalette.opacityOpaque);
    private const string C_NODE_WIDTH = "editor.width";
    private const float C_DEFAULT_WIDTH = 270f;
    private const float C_MINIMUM_WIDTH = 180f;
    private const float C_MAXIMUM_WIDTH = 800f;
    private const float C_HEADER = 32f;
    private const float C_ROW = 24f;
    private const float C_RESIZE_HANDLE = 14f;
    private GraphDocumentController Controller => owner.Controller(draft);
    private GraphCanvasState Canvas => draft.canvas;
    private GraphCanvasView View => new(Canvas, m_origin, m_size);

    private Vector2 m_origin;
    private Vector2 m_size;
    internal void Draw()
    {
        DrawHeader(owner, draft);
        m_origin = ImGuiApi.GetCursorScreenPosition();
        m_size = Vector2.Max(ImGuiApi.GetContentRegionAvailable(), Vector2.One);
        RefreshPorts();
        RefreshGeometry();
        owner.RefreshCompilation(draft);
        if (draft.frameRequested)
        {
            owner.CompleteCanvasEdit(draft);
            Frame();
            draft.frameRequested = false;
        }

        ImGuiApi.SetNextItemAllowOverlap();
        _ = ImGuiApi.InvisibleButton("##shader-canvas", m_size, (ImGuiButtonFlags.MouseButtonLeft | ImGuiButtonFlags.MouseButtonMiddle | ImGuiButtonFlags.MouseButtonRight));
        bool hovered = ImGuiApi.IsItemHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem);
        Vector2 mouse = ImGuiApi.GetMousePos();
        bool miniMapHovered = MiniMapContains(mouse);
        if (hovered && !miniMapHovered && ImGuiApi.IsMouseClicked(ImGuiMouseButton.Right))
        {
            draft.menuPosition = ToGraph(mouse);
            draft.createFromPort = null;
            ShaderCanvasGroup? hitGroup = HitGroupHeader(mouse);
            if (hitGroup is ShaderCanvasGroup group)
            {
                draft.selectedGroupId = group.id;
                Canvas.SelectNodes(group.nodes.Select(static id => new GraphNodeId(id)));
            }

            GraphNodeRecord? hit = hitGroup is null ? HitNode(mouse) : null;
            draft.selectedEdge = hit is null ? HitEdge(mouse) : null;
            if (draft.selectedEdge is not null)
                Canvas.ClearSelection();
            if (hit is not null && !Canvas.selectedNodes.Contains(hit.id))
                Canvas.SelectNodes([hit.id]);
            SetStage(hit);
        }

        if (owner.assets.TryGetFileSystemEntry(draft.path, out AssetFileEntry entry))
        {
            EditorInteraction interaction = owner.interactions.For(C_AREA, entry);
            if (ImGuiApi.IsWindowFocused(ImGuiFocusedFlags.RootAndChildWindows))
                interaction.Focus();
            _ = EditorUI.current.menus.ContextMenu("##shader-menu", interaction);
        }

        Navigate(hovered && !miniMapHovered);
        EditorDrawList draw = ImGuiApi.GetDrawList();
        draw.PushClip(m_origin, m_origin + m_size, true);
        try
        {
            draw.FilledRectangle(m_origin, m_origin + m_size, Color(0.065f, 0.071f, 0.083f), 0, false);
            Grid(draw);
            Groups(draw);
            Edges(draw);
            foreach (GraphNodeRecord node in Controller.document.nodes)
                Node(draw, node, mouse);
            Pointer(hovered && !miniMapHovered);
            if (draft.pointer.isBoxSelecting)
            {
                Vector2 min = Vector2.Min(draft.pointer.boxStart, mouse), max = Vector2.Max(draft.pointer.boxStart, mouse);
                draw.FilledRectangle(min, max, Color(0.52f, 0.37f, 0.78f, EditorPalette.opacitySubtle), 0, false);
                draw.Rectangle(min, max, Color(0.65f, 0.47f, 0.88f), 0, 1f);
            }

            string status = draft.readOnly ? "Read-only · copy to project to edit" : Controller.isDirty ? "Unsaved changes · Save to apply" : draft.status;
            status += " · Saved asset: " + draft.compilationStatus;
            {
                draw.Text(m_origin + new Vector2(12, 10), ImGuiApi.GetStyleColor(ImGuiCol.TextDisabled), status, 1f);
            }

            if (draft.compilationDiagnostics.Length != 0 && mouse.Y < m_origin.Y + 32 && hovered)
                EditorUI.current.widgets.DrawTooltip(draft.compilationDiagnostics);
            if (draft.error.Length != 0)
            {
                draw.Text(m_origin + new Vector2(12, 34), Color(1f, 0.47f, 0.44f), draft.error, 1f);
            }
        }
        finally
        {
            draw.PopClip();
        }

        DrawMiniMap();
        // Re-submit the canvas footprint after absolutely positioned node controls. A cursor
        // move alone can exceed ImGui's pixel-rounded item bounds at fractional UI scales.
        ImGuiApi.SetCursorScreenPosition(m_origin);
        ImGuiApi.Dummy(m_size);
        SynchronizeInspection();
    }

    private void SynchronizeInspection()
    {
        if (Canvas.selectedNodes.SequenceEqual(draft.inspectedNodes))
            return;
        draft.inspectedNodes = Canvas.selectedNodes.ToArray();
        draft.inspectionSelection = new ShaderInspectionSelection(draft.id, draft.inspectedNodes);
        owner.interactions.SetSelection(draft.inspectionSelection);
    }

    internal void DrawInspector(
        Inno.Editor.Inspection.InspectionDrawContext inspection,
        IReadOnlyList<GraphNodeId> selected
    ) {
        m_inspection = inspection;
        try
        {
            DrawInspectorContents(selected);
        }
        finally
        {
            m_inspection = null;
        }
    }

    private void DrawInspectorContents(IReadOnlyList<GraphNodeId> selected)
    {
        RefreshPorts();
        bool graphNode = ShaderGraphNodes.IsNodeGraph(Controller.document);
        if (!graphNode)
        {
            if (EditorUI.current.widgets.SectionHeader("Preview", "The preview uses the current draft without publishing it to Scene or Game views."))
            {
                var preview = owner.Preview(draft);
                if (preview is null)
                {
                    EditorUI.current.widgets.Hint("Run Check to build a preview for the current draft.");
                }
                else if (preview.state == EditorShaderCompilationState.Failed)
                {
                    EditorUI.current.widgets.Hint("Shader check failed. See Console.");
                }
                else if (preview.state == EditorShaderCompilationState.Succeeded
                    && preview.artifact is not null
                    && owner.interactions.TryGetModule<Inno.Editor.Shaders.ImGui.ShaderPreviews>(out var images)
                    && images is not null)
                {
                    var shader = owner.assets.Load<Inno.Rendering.Assets.ShaderAsset>(draft.path);
                    images.Draw(
                        draft.id,
                        new Inno.Rendering.Assets.MaterialAsset { shader = shader },
                        preview,
                        MathF.Max(1f, MathF.Min(256f, ImGuiApi.GetContentRegionAvailable().X))
                    );
                }
            }
        }
        else
        {
            if (EditorUI.current.widgets.SectionHeader(
                "Graph Node",
                "Function Inputs and Function Outputs independently define the reusable typed interface. Check validates the interface; callers validate the inlined computation."
            ))
            {
                try
                {
                    ShaderGraphNodeInterface nodeInterface = ShaderGraphNodes.ReadInterface(Controller.document, owner.serialization, owner.context);
                    InspectorRow(
                        "graph-node.summary",
                        "Interface",
                        () => EditorUI.current.widgets.WrappedText($"{nodeInterface.inputs.Length} input(s) · {nodeInterface.outputs.Length} output(s) · {nodeInterface.kind}")
                    );
                }
                catch (Exception failure) when (failure is InvalidOperationException or ArgumentException or FormatException or NotSupportedException)
                {
                    EditorUI.current.widgets.Hint(failure.Message);
                }
            }
        }

        if (selected.Count == 0)
        {
            if (EditorUI.current.widgets.SectionHeader(
                graphNode ? "Node Definition" : "Shader",
                graphNode ? "Select Function Inputs or Function Outputs to edit this reusable node's public interface." : "Target and public interface belong to this Shader, not to any Material override."
            ))
            {
                if (!graphNode)
                    DrawTarget();
                if (Controller.document.metadata.ContainsKey(ShaderGraphDocument.definitionKey))
                {
                    var definition = ShaderGraphDocument.ReadDefinition(Controller.document, owner.serialization, owner.context);
                    foreach (var property in definition.properties)
                        InspectorRow(
                            "definition." + property.id.value,
                            property.displayName,
                            () => EditorUI.current.widgets.WrappedText(property.type + " · " + property.bindingOwner)
                        );
                }

                owner.RefreshCompilation(draft);
                InspectorRow("compilation", "Compilation", () => EditorUI.current.widgets.WrappedText(draft.compilationStatus));
            }

            return;
        }

        GraphNodeRecord[] nodes = selected.Select(Controller.document.FindNode).OfType<GraphNodeRecord>().ToArray();
        if (nodes.Length != selected.Count)
            EditorUI.current.widgets.Hint("Some selected nodes are unavailable. Stable identities are retained.");
        if (nodes.Length == 0)
            return;
        if (nodes.Any(node => node.definitionId != nodes[0].definitionId))
        {
            EditorUI.current.widgets.Hint("Select nodes of the same kind to edit common settings.");
            return;
        }

        m_inspectionNodes = nodes;
        ImGuiApi.PushId(draft.id.ToString("N"));
        ImGuiApi.PushId(nodes[0].id.value);
        ImGuiApi.BeginDisabled(draft.readOnly);
        try
        {
            if (EditorUI.current.widgets.SectionHeader(
                Title(nodes[0]),
                "Inputs, defaults and output configuration are edited here. Changes remain in the Shader draft until Save."
            ))
            {
                if (nodes.Length > 1)
                    EditorUI.current.widgets.Hint("Editing " + nodes.Length + " nodes · differing values are replaced together");
                Controls(nodes[0]);
            }

            ShaderNodePort[] inputs = draft.ports[nodes[0].id].Where(port => port.direction == GraphPortDirection.Input).ToArray();
            if (inputs.Length != 0
                && EditorUI.current.widgets.SectionHeader(
                "Inputs",
                "Connected inputs show their source. Optional inputs use an automatic zero; required inputs must be connected in the graph."
            ))
            {
                foreach (ShaderNodePort port in inputs)
                    DrawInputDefault(nodes[0], port);
            }
        }
        finally
        {
            ImGuiApi.EndDisabled();
            ImGuiApi.PopId();
            ImGuiApi.PopId();
            m_inspectionNodes = null;
        }
    }

    private GraphNodeRecord[]? m_inspectionNodes;
    private Inno.Editor.Inspection.InspectionDrawContext? m_inspection;
    private static void InspectorRow(
        string id,
        string label,
        Action draw
    ) => EditorUI.current.widgets.PropertyRow("shader." + id, label, draw);
    internal static void DrawHeader(
        ShaderEditorDocuments owner,
        ShaderEditorDocuments.Draft? draft
    ) {
        ArgumentNullException.ThrowIfNull(owner);
        EditorUI.current.widgets.HeaderSurface("##shader-header", () =>
        {
            ImGuiApi.SetNextItemWidth(-1f);
            string title = draft is null ? "Select Shader" : System.IO.Path.GetFileName(draft.path.localPath) + (owner.Controller(draft).isDirty ? " *" : "");
            if (EditorUI.current.widgets.BeginBoundedCombo("##shader-header-select", title))
            {
                try
                {
                    foreach (AssetFileEntry candidate in owner.assets.GetFileSystemEntries(includeDirectories: false).Where(static candidate => candidate.extension.Equals(".ishader", StringComparison.OrdinalIgnoreCase)).OrderBy(
                        static candidate => candidate.assetPath.ToString(),
                        StringComparer.Ordinal
                    ))
                    {
                        bool selected = draft is not null && candidate.assetPath == draft.path;
                        if (ImGuiApi.Selectable(candidate.assetPath.ToString(), selected))
                        {
                            owner.interactions.SetSelection(candidate);
                            _ = owner.Open(candidate);
                        }

                        if (selected)
                            ImGuiApi.SetItemDefaultFocus();
                    }
                }
                finally
                {
                    EditorUI.current.widgets.EndBoundedCombo();
                }
            }

            bool noDocument = draft is null;
            ImGuiApi.BeginDisabled(noDocument || draft!.readOnly);
            if (ImGuiApi.Button("Save") && draft is not null && owner.assets.TryGetFileSystemEntry(draft.path, out AssetFileEntry entry))
                _ = owner.interactions.For(C_AREA, entry).Execute("shader/save");
            EditorUI.current.widgets.DrawItemTooltip("Save this shader and apply its changes (Command/Ctrl + S). Invalid graphs can be saved; rendering retains the last successful programs.");
            ImGuiApi.SameLine(0f, -1f);
            if (ImGuiApi.Button("Revert") && draft is not null)
                _ = owner.interactions.documents.Revert(draft.documentId);
            EditorUI.current.widgets.DrawItemTooltip("Restore the saved shader. This draft change can be undone.");
            ImGuiApi.EndDisabled();
            ImGuiApi.SameLine(0f, -1f);
            ImGuiApi.BeginDisabled(noDocument);
            if (ImGuiApi.Button("Format") && draft is not null && owner.assets.TryGetFileSystemEntry(draft.path, out AssetFileEntry formatEntry))
                _ = owner.interactions.For(C_AREA, formatEntry).Execute("shader/format");
            EditorUI.current.widgets.DrawItemTooltip("Arrange the graph from inputs on the left to outputs on the right. This changes only authoring positions and is undoable.");
            ImGuiApi.SameLine(0f, -1f);
            if (ImGuiApi.Button("Check") && draft is not null && owner.assets.TryGetFileSystemEntry(draft.path, out AssetFileEntry checkEntry))
                _ = owner.interactions.For(C_AREA, checkEntry).Execute("shader/check");
            EditorUI.current.widgets.DrawItemTooltip("Compile-check the current draft without saving or publishing it, and show diagnostics with source locations.");
            ImGuiApi.EndDisabled();
        }, spanWindowPadding: true);
    }

    private void RefreshPorts()
    {
        if (draft.portRevision == Controller.revision && draft.assetRevision == owner.assets.revision && draft.typeRevision == owner.typeVersion)
            return;
        draft.ports.Clear();
        draft.missingPorts.Clear();
        draft.nodeErrors.Clear();
        foreach (GraphNodeRecord node in Controller.document.nodes)
        {
            if (!draft.portSnapshots.ContainsKey(node.id))
                draft.portSnapshots[node.id] = Read(node, ShaderEditorDocuments.C_PORT_SNAPSHOT, Array.Empty<ShaderPortSnapshot>());
            var ports = new List<ShaderNodePort>();
            try
            {
                if (node.definitionId == ShaderGraphDocument.outputDefinitionId)
                {
                    ShaderGraphStageSettings settings = Read(node, "settings", new ShaderGraphStageSettings());
                    ports.AddRange(settings.outputs.Select(static output => new ShaderNodePort(output.id, ShaderSourceType.Atomic(output.kind == ShaderIrOutputKind.Depth ? "float" : output.kind == ShaderIrOutputKind.Varying ? "any" : "float4"), GraphPortDirection.Input)));
                }
                else
                    ports.AddRange(owner.Describe(node));
            }
            catch (Exception failure) when ((failure is InvalidOperationException or ArgumentException or FormatException)
                && Inno.Core.Execution.RetirementPendingException.Find(failure) is null)
            {
                draft.nodeErrors[node.id] = failure.Message;
            }

            foreach (ShaderPortSnapshot old in draft.portSnapshots[node.id])
                if (!ports.Any(port => port.id == old.id))
                {
                    ports.Add(new(old.id, old.type.CreateType(), old.direction, old.required));
                    draft.missingPorts.Add(new(node.id, new(old.id)));
                }

            foreach (GraphEdgeRecord edge in Controller.document.edges)
            {
                if (edge.input.nodeId == node.id)
                    KeepEndpoint(edge.input, GraphPortDirection.Input);
                if (edge.output.nodeId == node.id)
                    KeepEndpoint(edge.output, GraphPortDirection.Output);
            }

            draft.portSnapshots[node.id] = ports.Select(static port => new ShaderPortSnapshot { id = port.id, type = ShaderGraphType.Capture(port.type), direction = port.direction, required = port.required }).ToArray();
            draft.ports.Add(node.id, ports.ToArray());
            void KeepEndpoint(
                GraphEndpoint endpoint,
                GraphPortDirection direction
            ) {
                if (ports.Any(port => port.id == endpoint.portId.value))
                    return;
                ports.Add(new(endpoint.portId.value, ShaderSourceType.Atomic("missing"), direction, false));
                draft.missingPorts.Add(endpoint);
            }
        }

        draft.portRevision = Controller.revision;
        draft.groups = GroupShaderNodes.Read(owner, Controller.document);
        draft.assetRevision = owner.assets.revision;
        draft.typeRevision = owner.typeVersion;
        owner.PublishNodeDiagnostics(draft);
    }

    private void Navigate(bool hovered)
    {
        if (ImGuiApi.IsAnyPopupOpen())
        {
            owner.CompleteCanvasEdit(draft);
            draft.navigation.Cancel();
            return;
        }
        EditorUiInput io = ImGuiApi.GetInput();
        draft.navigation.Update(
            hovered,
            ImGuiApi.IsMouseClicked(ImGuiMouseButton.Left),
            ImGuiApi.IsMouseClicked(ImGuiMouseButton.Middle),
            ImGuiApi.IsMouseButtonDown(ImGuiMouseButton.Left),
            ImGuiApi.IsMouseButtonDown(ImGuiMouseButton.Middle),
            io.alt
        );
        if (draft.navigation.isPanning)
        {
            Canvas.PanBy(io.mouseDelta.X, io.mouseDelta.Y);
            ImGuiApi.SetMouseCursor(ImGuiMouseCursor.ResizeAll);
        }

        if (hovered && io.wheel != 0)
        {
            owner.CompleteCanvasEdit(draft);
            Vector2 pivot = ImGuiApi.GetMousePos() - m_origin;
            Canvas.ZoomAt(EditorPlanarNavigation.WheelFactor(io.wheel), pivot.X, pivot.Y);
        }

        if (hovered && !io.wantsText && ImGuiApi.IsKeyPressed(ImGuiKey.F, false))
        {
            owner.CompleteCanvasEdit(draft);
            Frame();
        }
        if (!io.wantsText && ImGuiApi.IsKeyPressed(ImGuiKey.Escape, false))
        {
            ShaderEditorDocuments.CancelCanvasEdit(draft);
            draft.navigation.Cancel();
            draft.createFromPort = null;
            RefreshGeometry();
        }
    }

    private void Pointer(bool hovered)
    {
        if (ImGuiApi.IsAnyPopupOpen())
            return;
        if (draft.navigation.isPanning || ImGuiApi.GetInput().alt)
        {
            owner.CompleteCanvasEdit(draft);
            return;
        }
        if (draft.pendingMove is not null)
        {
            owner.CompleteCanvasEdit(draft);
            return;
        }
        Vector2 mouse = ImGuiApi.GetMousePos();
        if (draft.resizingNode is GraphNodeId resizing)
        {
            ImGuiApi.SetMouseCursor(ImGuiMouseCursor.ResizeNwse);
            if (ImGuiApi.IsMouseButtonDown(ImGuiMouseButton.Left))
            {
                draft.resizePreviewWidth = Math.Clamp(draft.resizeStartWidth + (mouse.X - draft.resizePointerStart.X) / Canvas.zoom, C_MINIMUM_WIDTH, C_MAXIMUM_WIDTH);
            }
            else
            {
                owner.CompleteCanvasEdit(draft);
            }

            return;
        }

        GraphNodeRecord? resizeTarget = !draft.readOnly && hovered ? HitResizeHandle(mouse) : null;
        if (resizeTarget is not null)
            ImGuiApi.SetMouseCursor(ImGuiMouseCursor.ResizeNwse);
        if (hovered && ImGuiApi.IsMouseClicked(ImGuiMouseButton.Left))
        {
            if (resizeTarget is not null)
            {
                draft.selectedGroupId = "";
                draft.selectedEdge = null;
                Canvas.SelectNodes([resizeTarget.id]);
                draft.resizingNode = resizeTarget.id;
                draft.resizeStartWidth = NodeWidth(resizeTarget, includePreview: false);
                draft.resizePreviewWidth = draft.resizeStartWidth;
                draft.resizePointerStart = mouse;
                return;
            }

            GraphNodeRecord? hitNode = HitNode(mouse);
            ShaderCanvasGroup? group = hitNode is null ? HitGroupHeader(mouse) : null;
            if (group is ShaderCanvasGroup selectedGroup)
            {
                draft.selectedEdge = null;
                draft.selectedGroupId = selectedGroup.id;
                Canvas.SelectNodes(selectedGroup.nodes.Select(static id => new GraphNodeId(id)));
                SynchronizeInspection();
                if (!draft.readOnly)
                    draft.pointer.BeginMove(draft.layout, Canvas.selectedNodes, mouse);
                return;
            }
            if (hitNode is null && HitPort(mouse) is null)
            {
                draft.selectedGroupId = "";
                draft.selectedEdge = HitEdge(mouse);
                if (draft.selectedEdge is not null)
                {
                    if (!ImGuiApi.GetInput().shift)
                        Canvas.ClearSelection();
                    return;
                }
            }
        }

        GraphCanvasPointerResult intent = draft.pointer.Step(View, draft.layout, new GraphCanvasPointerInput
        {
            position = mouse,
            isHovered = hovered,
            isPressed = ImGuiApi.IsMouseClicked(ImGuiMouseButton.Left),
            isDown = ImGuiApi.IsMouseButtonDown(ImGuiMouseButton.Left),
            isReleased = ImGuiApi.IsMouseReleased(ImGuiMouseButton.Left),
            shift = ImGuiApi.GetInput().shift,
            readOnly = draft.readOnly
        });
        if (intent.nodePressed is GraphNodeId pressed && Controller.document.FindNode(pressed) is { } node)
        {
            draft.selectedGroupId = "";
            draft.selectedEdge = null;
            SetStage(node);
            SynchronizeInspection();
            if (!draft.readOnly && !draft.pointer.isMoving)
                draft.pointer.BeginMove(draft.layout, Canvas.selectedNodes, mouse);
        }
        if (intent.movedNodes is { } positions)
        {
            draft.pendingMove = positions;
            owner.CompleteCanvasEdit(draft);
        }
        if (intent.output is GraphEndpoint output && intent.input is GraphEndpoint input)
        {
            ShaderNodePort outputPort = Port(output);
            ShaderNodePort inputPort = Port(input);
            if (outputPort.type.IsEquivalentTo(inputPort.type) || inputPort.type.id == "any")
            {
                Controller.Connect(output, input);
                owner.Changed(draft);
            }
        }
        if (intent.releasedOutput is GraphEndpoint released)
        {
            draft.createFromPort = released;
            draft.menuPosition = ToGraph(mouse);
            ImGuiApi.OpenPopup("##shader-menu");
        }
    }

    private void Node(
        EditorDrawList draw,
        GraphNodeRecord node,
        Vector2 mouse
    ) {
        var rect = Rect(node);
        if (rect.max.X < m_origin.X || rect.min.X > m_origin.X + m_size.X || rect.max.Y < m_origin.Y || rect.min.Y > m_origin.Y + m_size.Y)
            return;
        float zoom = Canvas.zoom;
        if (!draft.layout.TryGetNode(node.id, out GraphCanvasNode? presentation))
            return;
        _ = View.DrawNode(draw, presentation, ImGuiApi.ToPackedColor(NodeHeaderColor(node)), Canvas.selectedNodes.Contains(node.id),
            previewPosition: draft.pointer.previewPositions.TryGetValue(node.id, out GraphPosition preview) ? preview : null);
        ShaderNodePort[] nodePorts = draft.ports[node.id];
        if (UsesSplitPortLayout(nodePorts))
        {
            float divider = (rect.min.X + rect.max.X) * 0.5f;
            float padding = 7f * zoom;
            draw.Line(new(divider, rect.min.Y + C_HEADER * zoom + padding), new(divider, rect.max.Y - padding),
                Color(0.62f, 0.63f, 0.69f, EditorPalette.opacityFaint), 1f);
        }
        foreach (ShaderNodePort port in nodePorts)
        {
            var endpoint = new GraphEndpoint(node.id, new GraphPortId(port.id));
            if (!TryPortPosition(endpoint, port.direction, out Vector2 point))
                continue;
            if (Vector2.DistanceSquared(point, mouse) <= 64f)
            {
                bool missing = draft.missingPorts.Contains(endpoint);
                bool optional = port.direction == GraphPortDirection.Input && !port.required;
                EditorUI.current.widgets.DrawTooltip(port.type.id + (missing ? " · missing port; reconnect explicitly"
                    : optional ? " · optional input" : port.direction == GraphPortDirection.Input ? " · required input" : ""));
            }
        }

        if (!draft.readOnly)
        {
            bool resizeHovered = ContainsResizeHandle(rect, ImGuiApi.GetMousePos());
            uint gripColor = Color(0.62f, 0.63f, 0.69f, resizeHovered || draft.resizingNode == node.id ? EditorPalette.opacityEmphasized : EditorPalette.opacityMedium);
            const float inset = 4f;
            draw.Line(rect.max - new Vector2(C_RESIZE_HANDLE, inset) * zoom, rect.max - new Vector2(inset, C_RESIZE_HANDLE) * zoom, gripColor, 1f);
            draw.Line(rect.max - new Vector2(C_RESIZE_HANDLE * 0.62f, inset) * zoom, rect.max - new Vector2(inset, C_RESIZE_HANDLE * 0.62f) * zoom, gripColor, 1f);
        }

        if (draft.ports[node.id].Length == 0 && node.definitionId == ShaderGraphNodes.inputDefinitionId)
        {
            draw.Text(
                rect.min + new Vector2(12, C_HEADER + 8) * zoom,
                ImGuiApi.GetStyleColor(ImGuiCol.TextDisabled),
                "No caller inputs",
                (ImGuiApi.GetFontSize() * zoom) / ImGuiApi.GetFontSize()
            );
        }

        if (draft.nodeErrors.TryGetValue(node.id, out string? error))
        {
            draw.FilledCircle(new(rect.max.X - 14 * zoom, rect.min.Y + 16 * zoom), 4 * zoom, Color(1, 0.4f, 0.35f));
            if (Contains(rect, ImGuiApi.GetMousePos()))
                EditorUI.current.widgets.DrawTooltip(error);
        }
    }

    private void Grid(EditorDrawList draw) => View.DrawGrid(Color(0.105f, 0.115f, 0.135f));
    private void Groups(EditorDrawList draw)
    {
        foreach (ShaderCanvasGroup group in draft.groups)
        {
            if (GroupRect(group) is not { } bounds)
                continue;
            Vector2 min = bounds.min, max = bounds.max;
            bool selected = draft.selectedGroupId == group.id;
            Vector4 headerColor = GroupHeaderColor(group);
            Vector4 bodyColor = EditorPalette.WithOpacity(headerColor, EditorPalette.opacitySoft);
            Vector4 borderColor = selected ? new(
                0.65f,
                0.47f,
                0.88f,
                EditorPalette.opacityOpaque
            ) : new(
                MathF.Min(1f, headerColor.X * 1.22f),
                MathF.Min(1f, headerColor.Y * 1.22f),
                MathF.Min(1f, headerColor.Z * 1.22f),
                EditorPalette.opacityEmphasized
            );
            float headerHeight = C_HEADER * Canvas.zoom;
            draw.FilledRectangle(min, max, ImGuiApi.ToPackedColor(bodyColor), 8 * Canvas.zoom, false);
            draw.FilledRectangle(min, new(max.X, min.Y + headerHeight), ImGuiApi.ToPackedColor(headerColor), 8 * Canvas.zoom, true);
            draw.Line(new(min.X, min.Y + headerHeight), new(max.X, min.Y + headerHeight), ImGuiApi.ToPackedColor(borderColor), 1f);
            draw.Rectangle(min, max, ImGuiApi.ToPackedColor(borderColor), 8 * Canvas.zoom, selected ? 2f : 1f);
            {
                draw.Text(
                    min + new Vector2(12, 7) * Canvas.zoom,
                    ImGuiApi.GetStyleColor(ImGuiCol.Text),
                    group.title,
                    (ImGuiApi.GetFontSize() * Canvas.zoom) / ImGuiApi.GetFontSize()
                );
            }
        }
    }

    private ShaderCanvasGroup? HitGroupHeader(Vector2 point)
    {
        foreach (ShaderCanvasGroup group in draft.groups.Reverse())
            if (GroupRect(group) is { } bounds && point.X >= bounds.min.X && point.X <= bounds.max.X && point.Y >= bounds.min.Y && point.Y <= bounds.min.Y + 32f * Canvas.zoom)
                return group;
        return null;
    }

    private (Vector2 min, Vector2 max)? GroupRect(ShaderCanvasGroup group)
    {
        Vector2 min = new(float.MaxValue), max = new(float.MinValue);
        foreach (string id in group.nodes)
            if (Controller.document.FindNode(new(id)) is GraphNodeRecord node)
            {
                var rect = Rect(node);
                min = Vector2.Min(min, rect.min);
                max = Vector2.Max(max, rect.max);
            }

        if (min.X == float.MaxValue)
            return null;
        return (min - new Vector2(20, 40) * Canvas.zoom, max + new Vector2(20) * Canvas.zoom);
    }

    private void RefreshGeometry()
    {
        bool changed = draft.layoutRevision != Controller.revision
            || draft.layoutAssetRevision != owner.assets.revision || draft.layoutTypeRevision != owner.typeVersion;
        if (!changed && draft.resizingNode is GraphNodeId resizing
            && draft.layout.TryGetNode(resizing, out GraphCanvasNode? current)
            && Controller.document.FindNode(resizing) is { } resizedNode)
        {
            if (current.size.X != NodeWidth(resizedNode))
                _ = draft.layout.SetNodes(draft.layout.nodes.Select(node => node.id == resizing ? DescribeNode(resizedNode) : node));
            return;
        }
        if (!changed)
            return;
        var nodes = new List<GraphCanvasNode>(Controller.document.nodes.Count);
        foreach (GraphNodeRecord node in Controller.document.nodes)
            nodes.Add(DescribeNode(node));
        _ = draft.layout.SetNodes(nodes);
        draft.layoutRevision = Controller.revision;
        draft.layoutAssetRevision = owner.assets.revision;
        draft.layoutTypeRevision = owner.typeVersion;

        GraphCanvasNode DescribeNode(GraphNodeRecord node)
        {
            float width = NodeWidth(node);
            bool split = UsesSplitPortLayout(draft.ports[node.id]);
            int left = 0;
            int right = 0;
            var ports = new List<GraphCanvasPort>();
            foreach (ShaderNodePort port in draft.ports[node.id])
            {
                bool input = port.direction == GraphPortDirection.Input;
                int row = input ? left++ : right++;
                float y = C_HEADER + C_ROW * (row + 0.5f);
                var endpoint = new GraphEndpoint(node.id, new GraphPortId(port.id));
                ports.Add(new GraphCanvasPort(endpoint, port.direction, new(input ? 0f : width, y), port.id,
                    input && !port.required, draft.missingPorts.Contains(endpoint),
                    (new(split && !input ? width * 0.5f : 0f, y - C_ROW * 0.5f),
                        new(split && input ? width * 0.5f : width, y + C_ROW * 0.5f))));
            }
            return new GraphCanvasNode(node.id, node.position,
                new(width, C_HEADER + Rows(node) * C_ROW + 20f), C_HEADER, Title(node), ports);
        }
    }

    private bool TryPortPosition(
        GraphEndpoint endpoint,
        GraphPortDirection direction,
        out Vector2 position
    ) {
        if (draft.layout.TryGetNode(endpoint.nodeId, out GraphCanvasNode? node))
            foreach (GraphCanvasPort port in node.ports)
                if (port.endpoint == endpoint && port.direction == direction)
                {
                    position = View.ToScreen(draft.pointer.previewPositions.GetValueOrDefault(node.id, node.position)) + port.position * Canvas.zoom;
                    return true;
                }
        position = default;
        return false;
    }

    private void Edges(EditorDrawList draw)
    {
        foreach (GraphEdgeRecord edge in Controller.document.edges)
            if (TryPortPosition(edge.output, GraphPortDirection.Output, out Vector2 a) && TryPortPosition(edge.input, GraphPortDirection.Input, out Vector2 b))
                Curve(a, b, edge.id == draft.selectedEdge);
        if (Canvas.pendingConnection is GraphEndpoint pending
            && Canvas.pendingConnectionDirection is GraphPortDirection direction
            && TryPortPosition(
            pending,
            direction,
            out Vector2 start
        ))
        {
            if (direction == GraphPortDirection.Output)
                Curve(start, ImGuiApi.GetMousePos());
            else
                Curve(ImGuiApi.GetMousePos(), start);
        }

        void Curve(
            Vector2 a,
            Vector2 b,
            bool selected = false
        ) {
            View.DrawConnection(draw, a, b, selected ? Color(0.87f, 0.72f, 1f) : Color(0.59f, 0.46f, 0.82f), selected ? 3f : 2f);
        }
    }

    private GraphEdgeId? HitEdge(Vector2 mouse)
    {
        foreach (GraphEdgeRecord edge in Controller.document.edges)
        {
            if (!TryPortPosition(edge.output, GraphPortDirection.Output, out Vector2 a) || !TryPortPosition(edge.input, GraphPortDirection.Input, out Vector2 b))
                continue;
            if (GraphCanvasView.HitConnection(mouse, a, b))
                return edge.id;
        }

        return null;
    }

    private (Vector2 min, Vector2 max) Rect(GraphNodeRecord node)
    {
        if (draft.layout.TryGetNode(node.id, out GraphCanvasNode? presentation))
            return View.NodeBounds(presentation, draft.pointer.previewPositions.TryGetValue(node.id, out GraphPosition preview) ? preview : null);
        Vector2 minimum = View.ToScreen(node.position);
        return (minimum, minimum + new Vector2(NodeWidth(node), C_HEADER + Rows(node) * C_ROW + 20f) * Canvas.zoom);
    }

    private int Rows(GraphNodeRecord node)
    {
        int rows = Math.Max(
            draft.ports[node.id].Count(static port => port.direction == GraphPortDirection.Input),
            draft.ports[node.id].Count(static port => port.direction == GraphPortDirection.Output)
        );
        return rows == 0 && node.definitionId is ShaderGraphNodes.inputDefinitionId or ShaderGraphNodes.outputDefinitionId ? 1 : rows;
    }

    private static bool UsesSplitPortLayout(IEnumerable<ShaderNodePort> ports) => ports.Any(static port => port.direction == GraphPortDirection.Input)
        && ports.Any(static port => port.direction == GraphPortDirection.Output);
    private GraphNodeRecord? HitNode(Vector2 point)
        => View.HitNode(draft.layout, point) is { } node ? Controller.document.FindNode(node.id) : null;

    private PortHandle? HitPort(Vector2 point) => View.HitPort(draft.layout, point) is { } port ? new PortHandle(port.endpoint, port.direction) : null;

    private GraphNodeRecord? HitResizeHandle(Vector2 point)
    {
        foreach (GraphNodeRecord node in Controller.document.nodes.Reverse())
        {
            (Vector2 min, Vector2 max) rect = Rect(node);
            if (ContainsResizeHandle(rect, point))
                return node;
        }

        return null;
    }

    private bool ContainsResizeHandle(
        (Vector2 min, Vector2 max) rect,
        Vector2 point
    ) {
        float extent = MathF.Max(10f, C_RESIZE_HANDLE * Canvas.zoom);
        return point.X >= rect.max.X - extent && point.X <= rect.max.X + 2f && point.Y >= rect.max.Y - extent && point.Y <= rect.max.Y + 2f;
    }

    private float NodeWidth(
        GraphNodeRecord node,
        bool includePreview = true
    ) {
        if (includePreview && draft.resizingNode == node.id)
            return draft.resizePreviewWidth;
        float width = Read(node, C_NODE_WIDTH, C_DEFAULT_WIDTH);
        return float.IsFinite(width) ? Math.Clamp(width, C_MINIMUM_WIDTH, C_MAXIMUM_WIDTH) : C_DEFAULT_WIDTH;
    }

    private ShaderNodePort Port(PortHandle handle) => draft.ports[handle.endpoint.nodeId].Single(port => port.id == handle.endpoint.portId.value && port.direction == handle.direction);
    private ShaderNodePort Port(GraphEndpoint endpoint) => draft.ports[endpoint.nodeId].Single(port => port.id == endpoint.portId.value);
    private GraphPosition ToGraph(Vector2 point) => View.ToGraph(point);
    private static bool Contains(
        (Vector2 min, Vector2 max) rect,
        Vector2 point
    ) => point.X >= rect.min.X && point.Y >= rect.min.Y && point.X <= rect.max.X && point.Y <= rect.max.Y;
    private static uint Color(
        float r,
        float g,
        float b,
        float a = EditorPalette.opacityOpaque
    ) => ImGuiApi.ToPackedColor(new(r, g, b, a));
    private Vector4 NodeHeaderColor(GraphNodeRecord node)
    {
        string key = node.definitionId + "/" + Title(node);
        return node.definitionId switch
        {
            ShaderGraphDocument.outputDefinitionId => StableHeaderColor(key, 258f, 34f),
            ShaderGraphNodes.outputDefinitionId => StableHeaderColor(key, 258f, 34f),
            "inno.shader.stage-input" or "inno.shader.constant" or ShaderGraphNodes.inputDefinitionId => StableHeaderColor(key, 22f, 24f),
            "inno.shader.sample" => StableHeaderColor(key, 158f, 30f),
            "inno.shader.source" => StableHeaderColor(key, 198f, 42f),
            "inno.shader.binary" => StableHeaderColor(key, 205f, 45f),
            "inno.shader.construct" or "inno.shader.extract" or "inno.shader.select" or "inno.shader.reroute" => StableHeaderColor(key, 178f, 58f),
            "inno.shader.storage-load" or "inno.shader.storage-store" or "inno.shader.storage-atomic-add" => StableHeaderColor(key, 105f, 42f),
            "inno.shader.discard" => StableHeaderColor(key, 350f, 22f),
            _ => StableHeaderColor(key, 0f, 360f)};
    }

    private Vector4 GroupHeaderColor(ShaderCanvasGroup group)
    {
        Vector4 total = Vector4.Zero;
        int count = 0;
        foreach (string id in group.nodes)
        {
            if (Controller.document.FindNode(new(id)) is not GraphNodeRecord node)
                continue;
            total += NodeHeaderColor(node);
            count++;
        }

        if (count == 0)
            return C_EMPTY_GROUP_HEADER;
        Vector4 average = total / count;
        return EditorPalette.WithOpacity(average, EditorPalette.opacityNearOpaque);
    }

    private static Vector4 StableHeaderColor(
        string key,
        float startHue,
        float hueRange
    ) {
        uint hash = 2166136261;
        foreach (char value in key)
        {
            hash ^= value;
            hash *= 16777619;
        }

        float hue = (startHue + hueRange * (hash & 0xffff) / 65535f) % 360f;
        float saturation = 0.52f + 0.12f * ((hash >> 16) & 0xff) / 255f;
        float brightness = 0.34f + 0.08f * ((hash >> 24) & 0xff) / 255f;
        float chroma = brightness * saturation;
        float sector = hue / 60f;
        float secondary = chroma * (1f - MathF.Abs(sector % 2f - 1f));
        (float red, float green, float blue) = sector switch
        {
            < 1f => (chroma, secondary, 0f),
            < 2f => (secondary, chroma, 0f),
            < 3f => (0f, chroma, secondary),
            < 4f => (0f, secondary, chroma),
            < 5f => (secondary, 0f, chroma),
            _ => (chroma, 0f, secondary)};
        float match = brightness - chroma;
        return new(red + match, green + match, blue + match, EditorPalette.opacityOpaque);
    }

    private static bool CenteredAddButton(string label) => EditorUI.current.widgets.CenteredButton(label, EditorUI.current.widgets.style.addButtonTopPadding);
    private T Read<T>(
        GraphNodeRecord node,
        string key,
        T value
    ) => ShaderGraphDocument.Read(node, key, value, owner.serialization, owner.context);
    private void Set<T>(
        GraphNodeRecord node,
        string key,
        T value,
        bool continuous = false
    ) => SetEncoded(node, key, ShaderGraphDocument.Encode(value, owner.serialization, owner.context), continuous ? draft.valueGesture : null);
    private void SetEncoded(
        GraphNodeRecord node,
        string key,
        GraphSerializedValue value,
        string? mergeKey
    ) {
        if (m_inspectionNodes is { Length: > 1 } selected)
        {
            GraphDocument candidate = Controller.document.Clone();
            foreach (GraphNodeRecord target in selected)
            {
                if (key == "settings" && target.definitionId == "inno.shader.stage-input")
                    candidate = ShaderGraphBindings.ChangeInput(
                        candidate,
                        target.id,
                        ShaderGraphDocument.Decode<ShaderGraphInputSettings>(value, owner.serialization, owner.context),
                        owner.serialization,
                        owner.context
                    );
                else
                    candidate.FindNode(target.id)!.SetValue(key, value.Clone());
            }

            Controller.ReplaceDocument(candidate, "Edit Shader Nodes", mergeKey);
        }
        else if (key == "settings" && node.definitionId == "inno.shader.stage-input")
            Controller.ReplaceDocument(
                ShaderGraphBindings.ChangeInput(
                    Controller.document,
                    node.id,
                    ShaderGraphDocument.Decode<ShaderGraphInputSettings>(value, owner.serialization, owner.context),
                    owner.serialization,
                    owner.context
                ),
                "Edit Shader Input",
                mergeKey
            );
        else
            Controller.SetNodeValue(node.id, key, value, mergeKey);
        owner.Changed(draft);
    }

    private void SetStage(GraphNodeRecord? node)
    {
        if (node is null)
            return;
        string stage = node.definitionId == ShaderGraphDocument.outputDefinitionId ? node.id.value : Read(node, "stage", "");
        if (stage.Length != 0)
            draft.activeStage = new(stage);
    }

    private void Frame()
    {
        GraphNodeRecord[] selection = Controller.document.nodes.Where(node => Canvas.selectedNodes.Count == 0 || Canvas.selectedNodes.Contains(node.id)).ToArray();
        if (selection.Length == 0)
            return;
        float minX = selection.Min(static node => node.position.x), minY = selection.Min(static node => node.position.y);
        float maxX = selection.Max(node => node.position.x + NodeWidth(node));
        float maxY = selection.Max(node => node.position.y + C_HEADER + Rows(node) * C_ROW + 20);
        float scale = Math.Clamp(MathF.Min(MathF.Max(1, m_size.X - 80) / MathF.Max(1, maxX - minX), MathF.Max(1, m_size.Y - 80) / MathF.Max(1, maxY - minY)), 0.1f, 1f);
        Canvas.SetViewport(new(m_size.X / 2 - (minX + maxX) / 2 * scale, m_size.Y / 2 - (minY + maxY) / 2 * scale), scale);
    }

    internal string Title(GraphNodeRecord node)
    {
        if (owner.drawers?.GetDisplayName(node.definitionId) is { } displayName)
            return displayName;
        if (node.definitionId == ShaderGraphDocument.outputDefinitionId)
            return Read(node, "settings", new ShaderGraphStageSettings()).stage + " Output";
        if (node.definitionId == "inno.shader.binary")
            return EditorDisplayNames.Nicify(Read(node, "operation", "add").Replace('-', ' '));
        if (node.definitionId == "inno.shader.stage-input")
        {
            ShaderGraphInputSettings input = Read(node, "settings", new ShaderGraphInputSettings());
            return input.id.Length != 0 ? EditorDisplayNames.Nicify(input.id) : "Stage Input";
        }

        if (node.definitionId == "inno.shader.source")
        {
            string function = Read(node, "function", "");
            return function.Length == 0 ? "Source Function" : function;
        }

        if (node.definitionId == ShaderGraphNodes.callDefinitionId)
            return Read(node, ShaderGraphNodes.interfaceKey, new ShaderGraphNodeInterface()).displayName;
        if (node.definitionId == ShaderGraphNodes.inputDefinitionId)
            return ShaderGraphNodes.ReadSettings(
                Controller.document,
                owner.serialization,
                owner.context
            ).kind == ShaderGraphNodeKind.DomainOutput ? "Domain Inputs" : "Function Inputs";
        if (node.definitionId == ShaderGraphNodes.outputDefinitionId)
            return "Function Outputs";
        return EditorDisplayNames.Nicify(node.definitionId.Replace("inno.shader.", "", StringComparison.Ordinal).Replace('-', ' '));
    }
}
