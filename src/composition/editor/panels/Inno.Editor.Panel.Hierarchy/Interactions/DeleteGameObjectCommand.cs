using System.Collections.Generic;
using System.Linq;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Core.Input;
using Inno.Scene;

namespace Inno.Editor.Panel.Hierarchy;
[EditorAction(HierarchyInteractionIds.C_DELETE_GAME_OBJECT, priority: 100)]
[EditorMenu(HierarchyInteractionIds.C_AREA, "Delete", order: 300)]
[EditorShortcut(HierarchyInteractionIds.C_AREA, KeyCode.Delete)]
internal sealed class DeleteGameObjectCommand(SceneEdits edits) : EditorAction<GameObject>
{
    /// <summary>
    /// Evaluates whether the requested change can be applied to the current generation.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// The validated editor action state that represents the completed operation.
    /// </returns>
    protected override EditorActionState Query(EditorActionContext<GameObject> context)
    {
        if (!context.target.isRuntimeValid)
            return EditorActionState.hidden;
        GameObject[] targets = Targets(context);
        return targets.Length != 0
            && targets.All(edits.CanEdit) ? EditorActionState.enabled : new EditorActionState(
            isVisible: true,
            isEnabled: false,
            unavailableReason: "Every selected object must belong to an editable scene."
        );
    }

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<GameObject> context)
    {
        edits.DeleteGameObjects(Targets(context));
        context.interactions.SetSelection((object? )null);
    }

    private static GameObject[] Targets(EditorActionContext<GameObject> context)
    {
        if (!context.interactions.selection.Contains(context.target))
            return[context.target];
        var values = new List<GameObject>();
        context.interactions.selection.CopyTargets(values);
        return values.Count == context.interactions.selection.count ? values.ToArray() : [];
    }
}
