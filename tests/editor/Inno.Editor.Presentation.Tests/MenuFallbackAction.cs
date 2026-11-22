using Inno.Editor.Interactions;

namespace Inno.Editor.Presentation.Tests;

[EditorAction("tests.menu-fallback", "tests/shared-menu")]
[EditorMenu("tests/shared-menu", "Shared", order: 100)]
public sealed class MenuFallbackAction : EditorAction
{
    protected override void Execute(EditorActionContext context) {}
}
