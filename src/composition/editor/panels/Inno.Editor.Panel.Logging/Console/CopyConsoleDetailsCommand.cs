using Inno.Core.Input;
using Inno.Editor.Interactions;

using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Logging;

[EditorAction(LoggingInteractionIDs.C_COPY_DETAILS, LoggingInteractionIDs.C_ENTRY_AREA)]
[EditorMenu(LoggingInteractionIDs.C_ENTRY_AREA, "Copy Full Entry", order: 110)]
[EditorShortcut(LoggingInteractionIDs.C_ENTRY_AREA, KeyCode.C, KeyModifier.Shift, primary: true)]
internal sealed class CopyConsoleDetailsCommand : EditorAction<ConsoleEntryCopyTarget>
{
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<ConsoleEntryCopyTarget> context) => Gui.SetClipboardText(context.target.fullText);
}
