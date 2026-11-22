using Inno.Editor.Interactions;

namespace Inno.Editor.Presentation.Tests;

[EditorAction("tests.toolbar", "tests/toolbar")]
[EditorToolbarItem("tests/toolbar", EditorToolbarIcon.Play, "Start Runtime", activeIcon: EditorToolbarIcon.Stop)]
public sealed class ToolbarAction : EditorAction
{
    public static bool isChecked;
    public static int executeCount;

    protected override EditorActionState Query(EditorActionContext context)
        => new(true, true, isChecked, isChecked ? "Stop Runtime" : "Start Runtime");

    protected override void Execute(EditorActionContext context) => executeCount++;
}
