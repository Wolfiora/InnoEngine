using System;
using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Editor.Assets;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/create-asset", "panel/asset.file-browser")]
internal sealed class CreateShaderAsset(
    ShaderEditorDocuments documents,
    AssetEditingModule browser
) : EditorAction<string, string>
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
    protected override EditorActionState Query(EditorActionContext<string, string> context)
    {
        AssetPath path = AssetPath.Parse(context.target);
        return documents.assets.sourceMounts.Any(mount => mount.id == path.source && !mount.isReadOnly)
               ? EditorActionState.enabled
               : EditorActionState.disabled;
    }

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void Execute(EditorActionContext<string, string> context)
    {
        AssetPath parent = AssetPath.Parse(context.target);
        AssetSourceMount mount = documents.assets.sourceMounts.Single(mount => mount.id == parent.source);
        string prefix = parent.localPath.TrimEnd('/');
        if (prefix.Length != 0)
        {
            prefix += "/";
        }
        int index = 0;
        AssetPath path;
        do
        {
            path = new(parent.source, prefix + (index++ == 0 ? "New Shader" : "New Shader " + index) + ".ishader");
        }
        while (File.Exists(mount.Resolve(path.localPath)) || File.Exists(mount.Resolve(path.localPath) + ".imeta"));
        GraphDocument graph = (documents.templates ?? throw new InvalidOperationException("Shader templates have not started.")).Create(
            context.argument,
            documents.serialization,
            documents.context
        );
        AssetFileEntry created = browser.CreateSource(path, GraphDocumentCodec.Encode(graph, documents.serialization));
        _ = context.interactions.For("panel/asset.file-browser", created).Select();
        _ = context.interactions.For("panel/asset.file-browser", created).Execute("file-browser/rename");
        context.interactions.OpenPanel("rendering.shader-editor");
    }
}
