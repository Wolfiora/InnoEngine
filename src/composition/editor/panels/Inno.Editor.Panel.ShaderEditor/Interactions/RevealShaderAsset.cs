using System.IO;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/reveal-shader", ShaderEditorCanvas.C_AREA)]
internal sealed class RevealShaderAsset(
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
        string parent = Path.GetDirectoryName(draft.path.localPath)?.Replace('\\', '/') ?? string.Empty;
        browser.browser.SetCurrentDirectory(new AssetPath(draft.path.source, parent).ToString());
        if (!documents.assets.TryGetFileSystemEntry(draft.path, out AssetFileEntry entry))
        {
            throw new FileNotFoundException("Shader source is missing from the Asset Browser.", draft.path.ToString());
        }
        documents.interactions.SetSelection(entry);
        documents.interactions.OpenPanel("asset.file-browser");
    }
}
