using Inno.Editor.Interactions;

namespace Inno.Editor.Workbench;

[EditorAction(ExportInteractionIDs.C_EXPORT_GAME)]
[EditorMenu(ExportInteractionIDs.C_MAIN_MENU_AREA, "File/Export as Game...", order: 810)]
internal sealed class ExportGameAction(ExportWindowModule window) : EditorAction
{
    /// <summary>
    /// Applies the editor action to the supplied interaction context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void Execute(EditorActionContext context) => window.OpenGame();
}
