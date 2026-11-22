using System;
using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

[EditorMenuSource("tests/failed-menu")]
public sealed class RejectedMenuSource : EditorMenuSource
{
    public override void Build(
        EditorMenuContext context,
        EditorMenuBuilder builder
    ) {
        builder.Add("Partial", "tests.menu");
        throw new InvalidOperationException("Candidate menu source failed.");
    }
}
