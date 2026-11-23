using Inno.Editor.Interactions;

namespace Inno.Editor.Workbench;

[EditorAction(GlobalInteractionIDs.C_SELECT)]
internal sealed class SelectEditorTargetAction : EditorAction<object>
{
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<object> context) => context.interactions.SetSelection(context.target);
}
