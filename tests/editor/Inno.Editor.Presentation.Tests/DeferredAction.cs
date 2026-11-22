using Inno.Editor.Interactions;

namespace Inno.Editor.Presentation.Tests;

[EditorAction("tests.deferred")]
public sealed class DeferredAction : EditorAction
{
    public static int executeCount;

    protected override void Execute(EditorActionContext context) => executeCount++;
}
