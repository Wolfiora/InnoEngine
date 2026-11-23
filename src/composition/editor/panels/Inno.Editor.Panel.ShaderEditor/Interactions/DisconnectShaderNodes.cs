using System;
using System.Linq;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/disconnect", ShaderEditorCanvas.C_AREA)]
internal sealed class DisconnectShaderNodes(ShaderEditorDocuments documents) : ShaderSelectionAction(documents)
{
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void Execute(EditorActionContext<AssetFileEntry> context)
    {
        ShaderEditorDocuments.Draft draft = documents.Open(context.target);
        GraphDocumentController controller = documents.Controller(draft);
        using EditorHistoryTransaction transaction = context.history.BeginTransaction("Disconnect Nodes");
        foreach (GraphEdgeRecord edge in controller.document.edges
            .Where(
                edge => draft.canvas.selectedNodes.Contains(edge.input.nodeID) || draft.canvas.selectedNodes.Contains(edge.output.nodeID)
            )
            .ToArray()
        ) {
            controller.Disconnect(edge.id);
        }
        transaction.Commit();
        documents.Changed(draft);
    }
}
