using Inno.Editor.Interactions;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Logging;
[EditorAction(LoggingInteractionIds.C_COPY_MESSAGE, LoggingInteractionIds.C_ENTRY_AREA)]
[EditorMenu(LoggingInteractionIds.C_ENTRY_AREA, "Copy Message", order: 100)]
internal sealed class CopyConsoleMessageCommand : EditorAction<ConsoleEntryCopyTarget>
{
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<ConsoleEntryCopyTarget> context) => Ui.SetClipboardText(context.target.message);
}
