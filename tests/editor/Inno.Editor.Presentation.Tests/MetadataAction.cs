using System;
using Inno.Editor.Interactions;

namespace Inno.Editor.Presentation.Tests;

[EditorAction("tests.action-metadata", "tests/action-metadata")]
public sealed class MetadataAction : EditorAction
{
    public static int targetTypeReadCount;
    public static bool throwWhenRead;

    public override Type? targetType
    {
        get
        {
            targetTypeReadCount++;
            return throwWhenRead ? throw new InvalidOperationException("Action metadata was read at runtime.") : typeof(BaseTarget);
        }
    }

    protected override void Execute(EditorActionContext context) {}
}
