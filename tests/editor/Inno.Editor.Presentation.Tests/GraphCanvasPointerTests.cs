using System;
using System.Numerics;
using Xunit;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.Graph.ImGui;

namespace Inno.Editor.Presentation.Tests;

public sealed class GraphCanvasPointerTests
{
    [Fact]
    public void ShiftBoxSelectionKeepsTheExistingSelectionAndAddsIntersectingNodes()
    {
        var state = new GraphCanvasState();
        var pointer = new GraphCanvasPointerController(state);
        var layout = new GraphCanvasLayout();
        var view = new GraphCanvasView(state, Vector2.Zero, new Vector2(800f, 600f));
        var original = new GraphNodeID("workflow.original");
        var added = new GraphNodeID("workflow.added");
        _ = layout.SetNodes([
            new GraphCanvasNode(original, new GraphPosition(500f, 300f), new Vector2(100f, 80f), 24f, "Original", []),
            new GraphCanvasNode(added, new GraphPosition(50f, 50f), new Vector2(100f, 80f), 24f, "Added", [])
        ]);
        state.SelectNodes([original]);
        _ = pointer.Step(
            view,
            layout,
            new GraphCanvasPointerInput() { position = new(10f, 10f), isHovered = true, isPressed = true, isDown = true, shift = true }
        );
        _ = pointer.Step(
            view,
            layout,
            new GraphCanvasPointerInput() { position = new(200f, 200f), isHovered = true, isReleased = true, shift = true }
        );
        Assert.Equal(2, state.selectedNodes.Count);
        Assert.Contains(original, state.selectedNodes);
        Assert.Contains(added, state.selectedNodes);
        Assert.False(pointer.isBoxSelecting);
    }

    [Fact]
    public void ConnectionReleaseBehindAnotherInputOwnerDoesNotConnect()
    {
        var state = new GraphCanvasState();
        var pointer = new GraphCanvasPointerController(state);
        var layout = new GraphCanvasLayout();
        var view = new GraphCanvasView(state, Vector2.Zero, new Vector2(800f, 600f));
        var output = new GraphEndpoint(new GraphNodeID("source"), new GraphPortID("out"));
        var input = new GraphEndpoint(new GraphNodeID("target"), new GraphPortID("in"));
        _ = layout.SetNodes([
            new GraphCanvasNode(
                new GraphNodeID("source"),
                new GraphPosition(0f, 0f),
                new Vector2(100f, 100f),
                24f,
                "Source",
                [new GraphCanvasPort(output, GraphPortDirection.Output, new Vector2(100f, 50f), "Output")]
            ),
            new GraphCanvasNode(
                new GraphNodeID("target"),
                new GraphPosition(200f, 0f),
                new Vector2(100f, 100f),
                24f,
                "Target",
                [new GraphCanvasPort(input, GraphPortDirection.Input, new Vector2(0f, 50f), "Input")]
            )
        ]);
        _ = pointer.Step(
            view,
            layout,
            new GraphCanvasPointerInput() { position = new(100f, 50f), isHovered = true, isPressed = true, isDown = true }
        );
        GraphCanvasPointerResult result = pointer.Step(
            view,
            layout,
            new GraphCanvasPointerInput() { position = new(200f, 50f), isReleased = true, isHovered = false }
        );
        Assert.Null(result.output);
        Assert.Null(result.input);
        Assert.Null(state.pendingConnection);
    }

    [Fact]
    public void DragReturnsOneNeutralEditAndCancelLeavesTheDocumentUntouched()
    {
        var state = new GraphCanvasState();
        var pointer = new GraphCanvasPointerController(state);
        var layout = new GraphCanvasLayout();
        var view = new GraphCanvasView(state, Vector2.Zero, new Vector2(800f, 600f));
        var node = new GraphCanvasNode(
            new GraphNodeID("flow.step"),
            new GraphPosition(10f, 10f),
            new Vector2(200f, 100f),
            24f,
            "Workflow",
            []
        );
        _ = layout.SetNodes([node]);
        Assert.Equal(
            node.id,
            pointer.Step(
                view,
                layout,
                new GraphCanvasPointerInput() { position = new(20f, 15f), isHovered = true, isPressed = true, isDown = true }
            ).nodePressed
        );
        _ = pointer.Step(view, layout, new GraphCanvasPointerInput() { position = new(70f, 45f), isDown = true });
        Assert.Equal(new GraphPosition(60f, 40f), pointer.previewPositions[node.id]);
        GraphCanvasPointerResult completed = pointer.Step(
            view,
            layout,
            new GraphCanvasPointerInput() { position = new(70f, 45f), isReleased = true }
        );
        Assert.Equal(new GraphPosition(60f, 40f), completed.movedNodes![node.id]);
        Assert.False(pointer.isMoving);
        Assert.Empty(pointer.previewPositions);
        Assert.Equal(new GraphPosition(10f, 10f), node.position);
        _ = pointer.Step(
            view,
            layout,
            new GraphCanvasPointerInput() { position = new(20f, 15f), isHovered = true, isPressed = true, isDown = true }
        );
        _ = pointer.Step(view, layout, new GraphCanvasPointerInput() { position = new(90f, 90f), isDown = true });
        Assert.Null(pointer.Step(view, layout, new GraphCanvasPointerInput() { cancel = true }).movedNodes);
        Assert.Empty(pointer.previewPositions);
        Assert.Equal(new GraphPosition(60f, 40f), completed.movedNodes[node.id]);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ConnectionsAreNeutralIntentsAndMissingEndpointsCannotBeConnected(bool missing)
    {
        var state = new GraphCanvasState();
        var pointer = new GraphCanvasPointerController(state);
        var layout = new GraphCanvasLayout();
        var view = new GraphCanvasView(state, Vector2.Zero, new Vector2(800f, 600f));
        var output = new GraphEndpoint(new GraphNodeID("flow.source"), new GraphPortID("next"));
        var input = new GraphEndpoint(new GraphNodeID("flow.target"), new GraphPortID("previous"));
        _ = layout.SetNodes([
            new GraphCanvasNode(
                output.nodeID,
                default,
                new Vector2(200f, 100f),
                24f,
                "Source",
                [new GraphCanvasPort(output, GraphPortDirection.Output, new Vector2(200f, 40f), "Next")]
            ),
            new GraphCanvasNode(
                input.nodeID,
                new GraphPosition(300f, 0f),
                new Vector2(200f, 100f),
                24f,
                "Target",
                [new GraphCanvasPort(input, GraphPortDirection.Input, new Vector2(0f, 40f), "Previous", isMissing: missing)]
            )
        ]);
        _ = pointer.Step(
            view,
            layout,
            new GraphCanvasPointerInput() { position = new(200f, 40f), isHovered = true, isPressed = true, isDown = true }
        );
        Assert.Equal(output, state.pendingConnection);
        GraphCanvasPointerResult result = pointer.Step(
            view,
            layout,
            new GraphCanvasPointerInput() { position = new(300f, 40f), isHovered = true, isReleased = true }
        );
        Assert.Equal(missing ? null : output, result.output);
        Assert.Equal(missing ? null : input, result.input);
        Assert.Null(state.pendingConnection);
    }

    [Fact]
    public void ReadOnlyChangesCancelEditsButStillAllowBoxSelection()
    {
        var state = new GraphCanvasState();
        var pointer = new GraphCanvasPointerController(state);
        var layout = new GraphCanvasLayout();
        var view = new GraphCanvasView(state, Vector2.Zero, new Vector2(800f, 600f));
        var id = new GraphNodeID("flow.read-only");
        _ = layout.SetNodes([new GraphCanvasNode(id, new GraphPosition(20f, 20f), new Vector2(200f, 100f), 24f, "Read only", [])]);
        _ = pointer.Step(
            view,
            layout,
            new GraphCanvasPointerInput() { position = new(30f, 25f), isHovered = true, isPressed = true, isDown = true }
        );
        _ = pointer.Step(view, layout, new GraphCanvasPointerInput() { position = new(100f, 100f), isDown = true });
        Assert.Null(
            pointer.Step(
                view,
                layout,
                new GraphCanvasPointerInput() { position = new(100f, 100f), isReleased = true, readOnly = true }
            ).movedNodes
        );
        Assert.False(pointer.isMoving);
        _ = pointer.Step(
            view,
            layout,
            new GraphCanvasPointerInput() { position = new(5f, 150f), isHovered = true, isPressed = true, isDown = true, readOnly = true }
        );
        Assert.True(pointer.isBoxSelecting);
        _ = pointer.Step(view, layout, new GraphCanvasPointerInput() { position = new(240f, 10f), isReleased = true, readOnly = true });
        Assert.Equal(id, Assert.Single(state.selectedNodes));
        Assert.False(pointer.isBoxSelecting);
        Assert.Throws<ArgumentOutOfRangeException>(
            () => pointer.Step(view, layout, new GraphCanvasPointerInput() { position = new(float.NaN, 0f) })
        );
    }
}
