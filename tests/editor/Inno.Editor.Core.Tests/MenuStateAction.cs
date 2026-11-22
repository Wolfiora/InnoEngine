using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

[EditorAction("tests.menu-state", "tests/static-menu")]
[EditorMenu("tests/static-menu", "Tools/State")]
public sealed class MenuStateAction : EditorAction
{
    protected override EditorActionState Query(EditorActionContext context)
        => context.target is MenuStateTarget target
           ? new EditorActionState(target.visible, target.enabled, target.isChecked, target.label, "Current owner is read-only.")
           : EditorActionState.hidden;

    protected override void Execute(EditorActionContext context) {}
}
