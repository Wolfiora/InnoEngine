using System.Linq;
using Inno.Assets.Pipeline;
using Inno.Core.Input;
using Inno.Editor.Interactions;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/duplicate", ShaderEditorCanvas.C_AREA)]
[EditorShortcut(ShaderEditorCanvas.C_AREA, KeyCode.D, primary: true)]
internal sealed class DuplicateShaderNodes(ShaderEditorDocuments documents) : ShaderSelectionAction(documents)
{
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
    {
        if (!base.Query(context).isEnabled || !documents.TryGet(context.target, out ShaderEditorDocuments.Draft draft))
        {
            return EditorActionState.disabled;
        }
        return draft.canvas.selectedNodes.Any(
            id => documents.Controller(draft).document.FindNode(id)?.definitionID == ShaderGraphDocument.C_OUTPUT_DEFINITION_ID
        )
               ? EditorActionState.disabled
               : EditorActionState.enabled;
    }

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void Execute(EditorActionContext<AssetFileEntry> context)
    {
        ShaderEditorDocuments.Draft draft = documents.Open(context.target);
        documents.Paste(draft, documents.Copy(draft));
    }
}
