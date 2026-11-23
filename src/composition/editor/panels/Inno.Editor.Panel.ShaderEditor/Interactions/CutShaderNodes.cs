using Inno.Assets.Pipeline;
using Inno.Core.Input;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/cut", ShaderEditorCanvas.C_AREA)]
[EditorShortcut(ShaderEditorCanvas.C_AREA, KeyCode.X, primary: true)]
internal sealed class CutShaderNodes(ShaderEditorDocuments documents) : ShaderSelectionAction(documents)
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
        documents.clipboard = documents.Copy(draft);
        documents.RemoveNodes(draft);
        draft.canvas.ClearSelection();
        documents.Changed(draft);
    }
}
