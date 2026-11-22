using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

[EditorDrop("tests/drop")]
public sealed class TestDrop : EditorDrop<DragSource, DropTarget>
{
    protected override EditorDropStatus Query(EditorDropContext<DragSource, DropTarget> context) => EditorDropStatus.Accept();

    protected override EditorDropResult Drop(EditorDropContext<DragSource, DropTarget> context)
    {
        context.target.wasDropped = true;
        return EditorDropResult.Accepted();
    }
}
