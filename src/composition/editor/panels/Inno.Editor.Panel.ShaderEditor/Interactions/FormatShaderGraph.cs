using Inno.Assets.Pipeline;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/format", ShaderEditorCanvas.C_AREA)]
internal sealed class FormatShaderGraph(ShaderEditorDocuments documents) : ShaderSelectionAction(documents)
{
    /// <summary>
    /// Gets whether needs selection is active for the current instance.
    /// </summary>
    protected override bool needsSelection => false;

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
        controller.ReplaceDocument(
            ShaderGraphAutoLayout.Apply(controller.document, documents.serialization, documents.context, documents.Describe),
            "Format Shader Graph"
        );
        draft.frameRequested = true;
        documents.Changed(draft);
    }
}
