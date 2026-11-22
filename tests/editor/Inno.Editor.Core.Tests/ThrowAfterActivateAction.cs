using System;
using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

[EditorAction("tests.throw-after-activate", "tests/throw-after-activate")]
public sealed class ThrowAfterActivateAction : EditorAction
{
    public static int cancelCount;

    protected override void Execute(EditorActionContext context)
    {
        Activate(context);
        throw new InvalidOperationException("Injected action execution failure.");
    }

    protected override void OnCancelled() => cancelCount++;
}
