using Inno.Core.Input;
using Inno.Editor.Interactions;

namespace Inno.Editor.Workbench;
[EditorAction(GlobalInteractionIds.C_FIND_COMMAND)]
[EditorMenu(GlobalInteractionIds.C_MAIN_MENU_AREA, "Tools/Find Command...", order: 0)]
[EditorShortcut(KeyCode.P, KeyModifier.Shift, primary: true)]
internal sealed class CommandSearchAction(CommandSearchModule commands) : EditorAction
{
    /// <inheritdoc/>
    protected override EditorActionState Query(EditorActionContext context) => commands.isOpen ? EditorActionState.disabled : EditorActionState.enabled;
    /// <inheritdoc/>
    protected override void Execute(EditorActionContext context) => commands.Open();
}
