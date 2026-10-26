using Inno.Scene;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;

namespace Inno.Editor.Panel.Inspector;
[EditorAction(InspectorInteractionIds.C_REMOVE_SYSTEM, priority: 100)]
[EditorMenu(InspectorInteractionIds.C_SYSTEM_AREA, "Remove System", order: 200)]
internal sealed class RemoveSystemCommand(SceneEdits edits) : EditorAction<GameSystem>
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
    protected override EditorActionState Query(EditorActionContext<GameSystem> context) => !context.target.isDestroyed ? EditorActionState.enabled : EditorActionState.disabled;
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<GameSystem> context) => _ = edits.RemoveSystem(context.target);
}
