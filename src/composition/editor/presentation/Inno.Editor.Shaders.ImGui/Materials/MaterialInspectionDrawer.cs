using System;
using Inno.Assets.Pipeline;
using Inno.Editor.ImGui;
using Inno.Editor.Inspection;
using Inno.Rendering.Assets;

namespace Inno.Editor.Shaders.ImGui;

[InspectionDrawer(typeof(MaterialAsset))]
internal sealed class MaterialInspectionDrawer(IInspectionIconProvider<AssetFileEntry> icons) : InspectionDrawer<MaterialAsset>
{
    /// <summary>
    /// Gets the icon glyph used to represent this item in the editor.
    /// </summary>
    public override string icon => ImGuiIcon.C_FILE;

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
        MaterialAsset target
    ) => context.interactions.TryGetModule<MaterialDocuments>(out MaterialDocuments? documents)
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
        MaterialAsset target
    ) => (target.name, null);

    /// <summary>
    /// Renders the header presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    protected override void DrawHeader(
        InspectionDrawContext context,
        MaterialAsset target
    ) {
        if (context.interactions.TryGetModule<MaterialDocuments>(out MaterialDocuments? documents) && documents is not null)
        {
            MaterialInspector.DrawHeader(context, documents, documents.Open(target));
        }
    }

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
        MaterialAsset target
    ) {
        if (context.interactions.TryGetModule<MaterialDocuments>(out MaterialDocuments? documents) && documents is not null)
        {
            MaterialInspector.Draw(context, documents, documents.Open(target));
        }
    }
}
