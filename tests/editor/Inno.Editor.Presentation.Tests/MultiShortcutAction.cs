using Inno.Core.Input;
using Inno.Editor.Interactions;

namespace Inno.Editor.Presentation.Tests;

[EditorAction("tests.multiple-shortcuts", "tests/multiple-shortcuts")]
[EditorShortcut("tests/multiple-shortcuts", KeyCode.F3)]
[EditorShortcut("tests/multiple-shortcuts", KeyCode.F4)]
public sealed class MultiShortcutAction : EditorAction
{
    public static int executeCount;

    protected override void Execute(EditorActionContext context) => executeCount++;
}
