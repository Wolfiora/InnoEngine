using System;
using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Interactions;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/reveal-source", ShaderEditorCanvas.C_AREA)]
internal sealed class RevealShaderFunction(
    ShaderEditorDocuments documents,
    AssetBrowserModule browser
) : ShaderSelectionAction(documents)
{
    /// <summary>
    /// Gets whether writes is active for the current instance.
    /// </summary>
    protected override bool writes => false;

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
        if (!base.Query(context).isEnabled)
        {
            return EditorActionState.disabled;
        }
        ShaderEditorDocuments.Draft draft = documents.Open(context.target);
        string? definition = draft.canvas.selectedNodes.Count == 1
                             ? documents.Controller(draft).document.FindNode(draft.canvas.selectedNodes.First())?.definitionID
                             : null;
        return definition is "inno.shader.source" or ShaderGraphNodes.C_CALL_DEFINITION_ID
               ? EditorActionState.enabled
               : EditorActionState.disabled;
    }

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<AssetFileEntry> context)
    {
        ShaderEditorDocuments.Draft draft = documents.Open(context.target);
        GraphNodeRecord node = documents.Controller(draft).document.FindNode(draft.canvas.selectedNodes.First())!;
        Reveal(documents, browser, ShaderGraphDocument.Read(node, "sourceId", Guid.Empty, documents.serialization, documents.context));
    }

    internal static void Reveal(
        ShaderEditorDocuments documents,
        AssetBrowserModule browser,
        Guid sourceID
    ) {
        if (!documents.assets.TryGetInfo(sourceID, out AssetInfo? info) || info is null)
        {
            throw new IOException("Referenced Shader authoring asset is missing. Select a replacement in the node.");
        }
        if (!documents.assets.TryGetFileSystemEntry(info.assetPath, out AssetFileEntry entry))
        {
            throw new FileNotFoundException(
                "Referenced Shader authoring asset is missing from the Asset Browser.",
                info.assetPath.ToString()
            );
        }
        string parent = Path.GetDirectoryName(info.assetPath.localPath)?.Replace('\\', '/') ?? string.Empty;
        browser.browser.SetCurrentDirectory(new AssetPath(info.assetPath.source, parent).ToString());
        documents.interactions.SetSelection(entry);
        documents.interactions.OpenPanel("asset.file-browser");
    }
}
