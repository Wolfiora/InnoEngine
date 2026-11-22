using Inno.Core.Input;
using Inno.Editor.Interactions;

namespace Inno.Editor.Workbench;

[EditorAction(GlobalInteractionIDs.C_UNDO, priority: 1000)]
[EditorMenu(GlobalInteractionIDs.C_MAIN_MENU_AREA, "Edit/Undo", order: 100)]
[EditorShortcut(KeyCode.Z, primary: true)]
internal sealed class UndoEditorAction : EditorAction
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
    protected override EditorActionState Query(EditorActionContext context)
        => new(
            isVisible: true,
            isEnabled: context.history.canUndo,
            displayName: context.history.undoName is { } name ? $"Undo {name}" : "Undo",
            unavailableReason: context.history.undoUnavailableReason
        );

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext context) => _ = context.history.Undo();
}
