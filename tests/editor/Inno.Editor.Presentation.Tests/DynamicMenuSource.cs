using Inno.Editor.Interactions;

namespace Inno.Editor.Presentation.Tests;

[EditorMenuSource("tests/menu")]
public sealed class DynamicMenuSource : EditorMenuSource
{
    public override void Build(
        EditorMenuContext context,
        EditorMenuBuilder builder
    ) {
        builder.AddGroup("Tools/Create/Libraries", order: 150, separatorBefore: true);
        builder.Add("Tools/Create/Libraries/Function", "tests.menu", order: 150);
        builder.Add("Tools/Create/Generated", "tests.menu", order: 200);
    }
}
