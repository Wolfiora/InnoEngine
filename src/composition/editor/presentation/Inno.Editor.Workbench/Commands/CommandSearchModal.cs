using System.Numerics;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Workbench;
[EditorModal(GlobalInteractionIds.C_COMMAND_SEARCH_MODAL, "Find Command", order: 50)]
internal sealed class CommandSearchModal(
    CommandSearchModule commands,
    EditorUiSession ui
) : EditorModal
{
    /// <inheritdoc/>
    public override bool isVisible => commands.isOpen;
    /// <inheritdoc/>
    public override bool allowScrolling => false;
    /// <inheritdoc/>
    public override Vector2 initialSize => new(620f, 440f);

    /// <inheritdoc/>
    protected override void OnDraw(EditorContext context)
    {
        if (commands.requestsFocus)
        {
            Ui.SetKeyboardFocusHere();
            commands.requestsFocus = false;
        }

        string search = commands.search;
        _ = ui.widgets.SearchInput("commands", "Search commands...", ref search);
        commands.search = search;
        bool opened = Ui.BeginChild("##command-results", new Vector2(0f, -Ui.GetFrameHeightWithSpacing()), 0, ImGuiWindowFlags.NoSavedSettings);
        bool chosen = false;
        try
        {
            if (opened)
                chosen = commands.Draw(ui);
        }
        finally
        {
            Ui.EndChild();
        }

        if (chosen || ui.widgets.CenteredButton("Close") || Ui.IsKeyPressed(ImGuiKey.Escape, false))
            commands.Close();
    }
}
