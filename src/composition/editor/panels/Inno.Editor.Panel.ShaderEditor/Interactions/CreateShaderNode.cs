using Inno.Assets.Pipeline;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/create-node", ShaderEditorCanvas.C_AREA)]
internal sealed class CreateShaderNode(ShaderEditorDocuments documents) : EditorAction<AssetFileEntry, ShaderNodeCreation>
{
    /// <summary>
    /// Evaluates the operation's current availability and presentation state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// The validated editor action state that represents the completed operation.
    /// </returns>
    protected override EditorActionState Query(EditorActionContext<AssetFileEntry, ShaderNodeCreation> context)
        => documents.TryGet(context.target, out ShaderEditorDocuments.Draft draft)
           && documents.CanCreate(draft, context.argument) ? EditorActionState.enabled : EditorActionState.disabled;

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void Execute(EditorActionContext<AssetFileEntry, ShaderNodeCreation> context)
        => documents.Create(documents.Open(context.target), context.argument);
}
