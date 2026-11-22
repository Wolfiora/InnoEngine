using System;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.ImGui;
using Inno.Editor.Inspection;
using Inno.Editor.Rendering;
using Inno.Rendering.Assets;

namespace Inno.Editor.Panel.Inspector;

[InspectionDrawer(typeof(RenderPipelineAsset))]
internal sealed class PipelineInspectionDrawer(IInspectionIconProvider<AssetFileEntry> icons) : InspectionDrawer<RenderPipelineAsset>
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
        RenderPipelineAsset target
    ) => context.interactions.TryGetModule<PipelineDocuments>(out PipelineDocuments? documents)
         && documents is not null
         && documents.assets.TryGetInfo(target.identity.persistentID, out AssetInfo? info)
         && info is not null
         && documents.assets.TryGetFileSystemEntry(
             info.assetPath,
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
        RenderPipelineAsset target
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
        RenderPipelineAsset target
    ) {
        if (context.interactions.TryGetModule<PipelineDocuments>(out PipelineDocuments? documents)
            && documents is not null
            && documents.assets.TryGetInfo(target.identity.persistentID, out AssetInfo? info)
            && info is not null
        ) {
            PipelineInspector.DrawHeader(context, documents, documents.Open(info.assetPath));
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
        RenderPipelineAsset target
    ) {
        if (context.interactions.TryGetModule<PipelineDocuments>(out PipelineDocuments? documents)
            && documents is not null
            && documents.assets.TryGetInfo(target.identity.persistentID, out AssetInfo? info)
            && info is not null
        ) {
            PipelineInspector.Draw(context, documents, documents.Open(info.assetPath));
        }
    }
}
