using Inno.Editor.Interactions;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorMenuSource("panel/asset.file-browser")]
internal sealed class ShaderTemplateMenu(ShaderEditorDocuments documents) : EditorMenuSource
{
    /// <summary>
    /// Builds a validated result from the current immutable input snapshot.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="builder">
    /// The builder consumed by build; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void Build(
        EditorMenuContext context,
        EditorMenuBuilder builder
    ) {
        if (documents.templates is null)
        {
            return;
        }
        builder.AddGroup("Create/Rendering", order: 200, separatorBefore: true);
        builder.AddGroup("Create/Rendering/Shaders", order: 0);
        foreach (ShaderGraphTemplateInfo template in documents.templates.templates)
        {
            builder.Add("Create/Rendering/Shaders/" + template.displayName, "shader/create-asset", argument: template.id, order: 0);
        }
    }
}
