using System;
using System.Linq;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/reroute", ShaderEditorCanvas.C_AREA)]
internal sealed class InsertShaderReroute(ShaderEditorDocuments documents) : ShaderSelectionAction(documents)
{
    /// <summary>
    /// Gets whether needs selection is active for the current instance.
    /// </summary>
    protected override bool needsSelection => false;

    /// <summary>
    /// Evaluates whether the requested change can be applied to the current generation.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// The validated editor action state that represents the completed operation.
    /// </returns>
    protected override EditorActionState Query(EditorActionContext<AssetFileEntry> context)
        => base.Query(context).isEnabled
           && documents.Open(context.target).selectedEdge is not null ? EditorActionState.enabled : EditorActionState.disabled;

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<AssetFileEntry> context)
    {
        ShaderEditorDocuments.Draft draft = documents.Open(context.target);
        GraphDocumentController controller = documents.Controller(draft);
        GraphDocument graph = controller.document.Clone();
        GraphEdgeRecord edge = graph.edges.Single(value => value.id == draft.selectedEdge);
        ShaderNodePort source = draft.ports[edge.output.nodeID].Single(value => value.id == edge.output.portID.value);
        var node = new GraphNodeRecord(new GraphNodeID(Guid.NewGuid().ToString("N")), "inno.shader.reroute")
        {
            position = draft.menuPosition
        };
        node.SetValue(
            "valueType",
            ShaderGraphDocument.Encode(ShaderGraphType.Capture(source.type), documents.serialization, documents.context)
        );
        string stage = ShaderGraphDocument.Read(
            graph.FindNode(edge.output.nodeID)!,
            "stage",
            string.Empty,
            documents.serialization,
            documents.context
        );
        node.SetValue("stage", ShaderGraphDocument.Encode(stage, documents.serialization, documents.context));
        graph.AddNode(node);
        graph.RemoveEdge(edge.id);
        graph.AddEdge(
            new GraphEdgeRecord(
                new GraphEdgeID(Guid.NewGuid().ToString("N")),
                edge.output,
                new GraphEndpoint(node.id, new GraphPortID("input"))
            )
        );
        graph.AddEdge(
            new GraphEdgeRecord(
                new GraphEdgeID(Guid.NewGuid().ToString("N")),
                new GraphEndpoint(node.id, new GraphPortID("value")),
                edge.input
            )
        );
        controller.ReplaceDocument(graph, "Insert Shader Reroute");
        draft.selectedEdge = null;
        draft.canvas.SelectNodes([node.id]);
        documents.Changed(draft);
    }
}
