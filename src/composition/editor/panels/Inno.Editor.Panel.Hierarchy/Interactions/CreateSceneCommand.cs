using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Scene;

namespace Inno.Editor.Panel.Hierarchy;

[EditorAction(HierarchyInteractionIDs.C_CREATE_SCENE)]
[EditorMenu(HierarchyInteractionIDs.C_AREA, "Create/Scene", order: 300)]
internal sealed class CreateSceneCommand(SceneEdits edits) : EditorAction
{
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext context)
    {
        GameScene scene = edits.CreateScene();
        EditorInteraction interaction = context.interactions.For(HierarchyInteractionIDs.C_AREA, scene);
        _ = interaction.Select();
        _ = interaction.Execute(HierarchyInteractionIDs.C_RENAME);
    }
}
