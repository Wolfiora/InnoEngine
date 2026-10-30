using Inno.Editor.Interactions;

namespace Inno.Editor.Workbench;
[EditorAction(GlobalInteractionIds.C_TOGGLE_PANEL, GlobalInteractionIds.C_MAIN_MENU_AREA)]
internal sealed class TogglePanelAction : EditorArgumentAction<string>
{
    /// <inheritdoc/>
    protected override EditorActionState Query(EditorActionArgumentContext<string> context) => context.interactions.TryGetPanelState(
        context.argument,
        out bool isOpen
    ) ? new EditorActionState(
        true,
        true,
        isChecked: isOpen
    ) : EditorActionState.hidden;
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionArgumentContext<string> context) => _ = context.interactions.TogglePanel(context.argument);
}
