using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/copy-to-project", ShaderEditorCanvas.C_AREA)]
internal sealed class CopyShaderToProject(
    ShaderEditorDocuments documents,
    AssetBrowserModule browser
) : ShaderSelectionAction(documents)
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
    protected override void Execute(EditorActionContext<AssetFileEntry> context)
    {
        ShaderEditorDocuments.Draft draft = documents.Open(context.target);
        AssetSourceMount mount = documents.assets.sourceMounts.Single(value => value.id == AssetSourceID.project);
        string name = Path.GetFileNameWithoutExtension(draft.path.localPath) + " Copy";
        AssetPath destination = AssetPath.Project(name + ".ishader");
        for (int index = 2; File.Exists(mount.Resolve(destination.localPath))
                            || File.Exists(mount.Resolve(destination.localPath) + ".imeta"); index++
        ) {
            destination = AssetPath.Project(name + " " + index + ".ishader");
        }
        AssetFileEntry entry = browser.edits.CreateSource(
            destination,
            GraphDocumentCodec.Encode(documents.Controller(draft).document, documents.serialization)
        );
        _ = context.interactions.For("panel/asset.file-browser", entry).Select();
        _ = context.interactions.For("panel/asset.file-browser", entry).Execute("file-browser/rename");
        context.interactions.OpenPanel("rendering.shader-editor");
    }
}
