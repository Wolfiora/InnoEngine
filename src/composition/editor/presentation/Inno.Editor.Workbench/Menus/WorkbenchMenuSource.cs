using Inno.Editor.Core;
using Inno.Editor.Interactions;

namespace Inno.Editor.Workbench;
[EditorMenuSource(GlobalInteractionIds.C_MAIN_MENU_AREA)]
internal sealed class WorkbenchMenuSource : EditorMenuSource
{
    /// <inheritdoc/>
    public override void Build(
        EditorMenuContext context,
        EditorMenuBuilder builder
    ) {
        builder.AddGroup("File", order: 0);
        builder.AddGroup("Edit", order: 100);
        builder.AddGroup("Create", order: 200);
        builder.AddGroup("View", order: 300);
        builder.AddGroup("Window", order: 400);
        builder.AddGroup("Tools", order: 500);
        builder.AddGroup("Help", order: 600);
        foreach (EditorPanelAttribute panel in context.interactions.GetPanelDeclarations())
        {
            string path = string.IsNullOrEmpty(panel.menuPath) ? "Window/" + panel.title : "Window/" + panel.menuPath + "/" + panel.title;
            builder.Add(path, GlobalInteractionIds.C_TOGGLE_PANEL, panel.order, panel.separatorBefore, panel.id);
        }
    }
}
