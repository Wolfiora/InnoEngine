using Inno.Core.Input;
using Inno.Editor.Interactions;

using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Logging;

[EditorAction(LoggingInteractionIDs.C_COPY_MESSAGE, LoggingInteractionIDs.C_ENTRY_AREA)]
[EditorMenu(LoggingInteractionIDs.C_ENTRY_AREA, "Copy Message", order: 100)]
[EditorShortcut(LoggingInteractionIDs.C_ENTRY_AREA, KeyCode.C, primary: true)]
internal sealed class CopyConsoleMessageCommand : EditorAction<ConsoleEntryCopyTarget>
{
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<ConsoleEntryCopyTarget> context) => Gui.SetClipboardText(context.target.message);
}
