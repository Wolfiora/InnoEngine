using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Core.Input;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/delete", ShaderEditorCanvas.C_AREA)]
[EditorShortcut(ShaderEditorCanvas.C_AREA, KeyCode.Delete)]
[EditorShortcut(ShaderEditorCanvas.C_AREA, KeyCode.Backspace)]
internal sealed class DeleteShaderNodes(ShaderEditorDocuments documents) : ShaderSelectionAction(documents)
{
    /// <summary>
    /// Gets whether needs selection is active for the current instance.
    /// </summary>
    protected override bool needsSelection => false;

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
        using EditorHistoryTransaction transaction = context.history.BeginTransaction("Delete Shader Selection");
        if (draft.selectedEdge is GraphEdgeID edge)
        {
            controller.Disconnect(edge);
        }
        documents.RemoveNodes(draft);
        transaction.Commit();
        draft.selectedEdge = null;
        draft.canvas.ClearSelection();
        documents.Changed(draft);
    }
}
