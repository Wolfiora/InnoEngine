using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

[EditorAction("tests.menu-conditional", "tests/shared-menu")]
[EditorMenu("tests/shared-menu", "Shared", order: 200)]
public sealed class MenuConditionalAction : EditorAction
{
    protected override EditorActionState Query(EditorActionContext context)
        => context.target is MenuStateTarget { visible: true } ? EditorActionState.enabled : EditorActionState.hidden;

    protected override void Execute(EditorActionContext context) {}
}
