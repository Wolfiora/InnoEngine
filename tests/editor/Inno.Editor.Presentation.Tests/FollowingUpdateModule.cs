using Inno.Editor.Core;

namespace Inno.Editor.Presentation.Tests;

[EditorModule("tests.following-update", order: -199)]
public sealed class FollowingUpdateModule : EditorModule
{
    public static int updateCount;

    protected override void OnUpdate(EditorContext context) => updateCount++;
}
