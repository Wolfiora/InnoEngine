using Inno.Editor.Interactions;

namespace Inno.Editor.Presentation.Tests;

public sealed class TestViewportTool(IEditorHistory history) : EditorViewportTool
{
    public int downCount { get; private set; }
    public int upCount { get; private set; }
    public override string id => "tests.viewport.paint";
    public override EditorViewportCursor cursor => EditorViewportCursor.Crosshair;

    public override void OnPointerDown(
        EditorViewportToolContext context,
        EditorViewportPointerEvent pointer
    ) {
        downCount++;
        context.CapturePointer(pointer.pointerID);
        context.BeginHistoryGesture("Paint Tile");
        NeutralHistoryHandler.value = 1;
        history.RecordApplied("Paint Tile Cell", NeutralHistoryHandler.CreateChange(before: 0, after: 1));
    }

    public override void OnPointerUp(
        EditorViewportToolContext context,
        EditorViewportPointerEvent pointer
    ) {
        upCount++;
        context.CompleteHistoryGesture(commit: true);
        context.ReleasePointer();
    }
}
