using Inno.Assets.Pipeline;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

internal abstract class ShaderSelectionAction(ShaderEditorDocuments documents) : EditorAction<AssetFileEntry>
{
    /// <summary>
    /// Gets documents presented by this Shader Editor action.
    /// </summary>
    protected ShaderEditorDocuments documents { get; } = documents;

    /// <summary>
    /// Gets whether writes is active for the current instance.
    /// </summary>
    protected virtual bool writes => true;

    /// <summary>
    /// Gets whether needs selection is active for the current instance.
    /// </summary>
    protected virtual bool needsSelection => true;

    /// <summary>
    /// Evaluates the operation's current availability and presentation state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// The validated editor action state that represents the completed operation.
    /// </returns>
    protected override EditorActionState Query(EditorActionContext<AssetFileEntry> context)
        => documents.TryGet(context.target, out ShaderEditorDocuments.Draft draft)
           && (!writes
               || !draft.readOnly)
           && (!needsSelection
               || draft.canvas.selectedNodes.Count != 0) ? EditorActionState.enabled : EditorActionState.disabled;
}
