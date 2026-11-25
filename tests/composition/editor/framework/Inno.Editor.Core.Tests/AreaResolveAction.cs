using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

[EditorAction("tests.resolve", "tests/special", priority: 100)]
public sealed class AreaResolveAction : EditorAction<DerivedTarget>
{
    protected override EditorActionState Query(EditorActionContext<DerivedTarget> context) => new(true, true, displayName: "area");

    protected override void Execute(EditorActionContext<DerivedTarget> context) {}
}
