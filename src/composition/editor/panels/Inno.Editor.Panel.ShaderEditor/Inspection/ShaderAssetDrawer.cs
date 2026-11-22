using System;
using Inno.Assets.Pipeline;
using Inno.Editor.ImGui;
using Inno.Editor.Inspection;
using Inno.Rendering.Assets;

using EditorUI = Inno.Editor.ImGui.EditorUISession;

namespace Inno.Editor.Panel.ShaderEditor;

[InspectionDrawer(typeof(ShaderAsset))]
internal sealed class ShaderAssetDrawer(IInspectionIconProvider<AssetFileEntry> icons) : InspectionDrawer<ShaderAsset>
{
    /// <summary>
    /// Gets the icon glyph used to represent this item in the editor.
    /// </summary>
    public override string icon => "S";

    /// <summary>
    /// Retrieves the current icon from authoritative state.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    /// <returns>
    /// The validated text representation owned by the caller.
    /// </returns>
    protected override string GetIcon(
        InspectionDrawContext context,
        ShaderAsset target
    ) => context.interactions.TryGetModule<ShaderEditorDocuments>(out ShaderEditorDocuments? documents)
         && documents is not null
         && documents.assets.TryGetFileSystemEntry(
             target.assetPath,
             out AssetFileEntry entry
         ) ? icons.GetIcon(entry) : icon;

    /// <summary>
    /// Binds a caller-visible label to the current inspection target.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    /// <returns>
    /// The validated (string name, actionstring? setter) that represents the completed operation.
    /// </returns>
    protected override (string name, Action<string>? setter) BindName(
        InspectionDrawContext context,
        ShaderAsset target
    ) => (target.name, null);

    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    protected override void Draw(
        InspectionDrawContext context,
        ShaderAsset target
    ) {
        if (!context.interactions.TryGetModule<ShaderEditorDocuments>(out ShaderEditorDocuments? documents) || documents is null)
        {
            return;
        }
        if (!documents.assets.TryGetFileSystemEntry(target.assetPath, out AssetFileEntry entry))
        {
            return;
        }
        if (documents.TryOpen(entry, out ShaderEditorDocuments.Draft draft))
        {
            new ShaderEditorCanvas(documents, draft).DrawInspector(context, []);
        }
        else
        {
            EditorUI.current.widgets.ColoredText(EditorPalette.textDisabled, "Waiting for Shader asset import…");
        }
    }
}
