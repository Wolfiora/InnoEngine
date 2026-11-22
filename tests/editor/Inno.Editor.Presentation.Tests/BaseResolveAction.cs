using Inno.Editor.Interactions;

namespace Inno.Editor.Presentation.Tests;

[EditorAction("tests.resolve")]
public sealed class BaseResolveAction : EditorAction<BaseTarget>
{
    protected override EditorActionState Query(EditorActionContext<BaseTarget> context) => new(true, true, displayName: "base");

    protected override void Execute(EditorActionContext<BaseTarget> context) {}
}
