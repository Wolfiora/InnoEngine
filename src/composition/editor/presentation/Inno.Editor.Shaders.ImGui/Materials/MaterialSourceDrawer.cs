using System;
using Inno.Assets.Pipeline;
using Inno.Editor.ImGui;
using Inno.Editor.Inspection;

namespace Inno.Editor.Shaders.ImGui;

[InspectionDrawer(typeof(AssetFileEntry), priority: 100, conditional: true)]
internal sealed class MaterialSourceDrawer(IInspectionIconProvider<AssetFileEntry> icons) : InspectionDrawer<AssetFileEntry>
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
        AssetFileEntry target
    ) => icons.GetIcon(target);

    /// <summary>
    /// Checks whether this drawer supports the selected Inspector target.
    /// </summary>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the documented condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    protected override bool CanInspect(AssetFileEntry target)
        => !target.isDirectory && target.extension.Equals(".imaterial", StringComparison.OrdinalIgnoreCase);

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
        AssetFileEntry target
    ) => (target.nameWithoutExtension, null);

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
        AssetFileEntry target
    ) {
        if (context.interactions.TryGetModule<MaterialDocuments>(out MaterialDocuments? documents) && documents is not null)
        {
            MaterialInspector.DrawHeader(context, documents, documents.OpenDraft(target.assetPath));
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
        AssetFileEntry target
    ) {
        if (context.interactions.TryGetModule<MaterialDocuments>(out MaterialDocuments? documents) && documents is not null)
        {
            MaterialInspector.Draw(context, documents, documents.OpenDraft(target.assetPath));
        }
    }
}
