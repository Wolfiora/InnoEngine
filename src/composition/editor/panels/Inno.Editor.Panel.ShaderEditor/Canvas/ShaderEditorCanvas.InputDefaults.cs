using System.Linq;
using Inno.Core.Graphs;
using Inno.Editor.Core;
using Inno.Rendering.Shaders;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using ImGuiApi = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.ShaderEditor;

internal sealed partial class ShaderEditorCanvas
{
    private void DrawInputDefault(
        GraphNodeRecord node,
        ShaderNodePort port
    ) {
        var endpoint = new GraphEndpoint(node.id, new GraphPortID(port.id));
        GraphEdgeRecord? edge = controller.document.edges.FirstOrDefault(value => value.input == endpoint);
        ImGuiApi.PushID(port.id);
        try
        {
            if (edge is not null)
            {
                InspectorPortStatusRow("port." + port.id, port, null, "From " + edge.output.nodeID.value + "." + edge.output.portID.value);
                return;
            }

            if (node.definitionID == ShaderGraphDocument.C_OUTPUT_DEFINITION_ID || draft.missingPorts.Contains(endpoint))
            {
                InspectorPortStatusRow("port." + port.id, port, null, "Connect an available output");
                return;
            }

            if (m_inspectionNodes is { Length: > 1 } selected
                && selected.Any(
                    value => (
                        !draft.ports[value.id].Any(candidate => candidate.id == port.id && candidate.type.IsEquivalentTo(port.type))
                        || controller.document.edges.Any(
                            connection => connection.input == new GraphEndpoint(value.id, new GraphPortID(port.id))
                        )
                    )
                )
            ) {
                InspectorPortStatusRow("port." + port.id, port, null, "Mixed connections or types");
                return;
            }

            InspectorPortStatusRow(
                "port." + port.id,
                port,
                null,
                port.required ? "Required · Connect a Constant or compatible output" : "Optional · Zero when unconnected"
            );
        }
        finally
        {
            ImGuiApi.PopID();
        }
    }

    private static void InspectorPortStatusRow(
        string id,
        ShaderNodePort port,
        string? component,
        string status
    ) => EditorUI.current.widgets.PropertyRow(
        "shader." + id,
        PortLabel(port, component),
        () => EditorUI.current.widgets.MetadataValue(DisplayType(port.type.id), status, "Shader value type · " + port.type.id)
    );

    private static string PortLabel(
        ShaderNodePort port,
        string? component
    ) => component is null ? port.id : port.id + " " + component;

    private static string DisplayType(string typeID) => EditorDisplayNames.Nicify(typeID);
}
