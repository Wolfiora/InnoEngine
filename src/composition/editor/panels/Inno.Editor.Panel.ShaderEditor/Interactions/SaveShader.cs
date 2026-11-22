using Inno.Assets.Pipeline;
using Inno.Core.Input;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/save", ShaderEditorCanvas.C_AREA)]
[EditorShortcut(ShaderEditorCanvas.C_AREA, KeyCode.S, primary: true)]
internal sealed class SaveShader(ShaderEditorDocuments documents) : ShaderSelectionAction(documents)
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
        => context.interactions.documents.Save(documents.Open(context.target).documentID);
}
