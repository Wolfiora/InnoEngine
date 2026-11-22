using Inno.Assets.Pipeline;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/focus", ShaderEditorCanvas.C_AREA)]
internal sealed class FocusShaderNodes(ShaderEditorDocuments documents) : ShaderSelectionAction(documents)
{
    /// <summary>
    /// Gets whether writes is active for the current instance.
    /// </summary>
    protected override bool writes => false;

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
    protected override void Execute(EditorActionContext<AssetFileEntry> context) => documents.Open(context.target).frameRequested = true;
}
