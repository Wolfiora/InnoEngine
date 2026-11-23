using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

[EditorAction("tests.menu")]
[EditorMenu("tests/menu", "Tools/Create/Asset", order: 100)]
public sealed class MenuAction : EditorAction
{
    protected override void Execute(EditorActionContext context) {}
}
