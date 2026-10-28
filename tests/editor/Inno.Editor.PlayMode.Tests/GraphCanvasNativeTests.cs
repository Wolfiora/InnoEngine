using System;
using System.Linq;
using System.Numerics;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.Graph.ImGui;
using Inno.Native.ImGui;
using Xunit;
using NativeGui = Inno.Native.ImGui.ImGui;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.PlayMode.Tests;

public sealed unsafe class GraphCanvasNativeTests
{
    [Fact]
    public void NonShaderPointerWorkflowUsesTheRealDocumentHistory()
    {
        using var host = new NativeProjectPanelHost();
        var document = new GraphDocument();
        var first = new GraphNodeRecord(new("workflow.first"), "workflow.step") { position = new(20f, 20f) };
        var second = new GraphNodeRecord(new("workflow.second"), "workflow.step") { position = new(300f, 20f) };
        document.AddNode(first);
        document.AddNode(second);
        var history = host.runtime.interactions.history;
        var controller = host.graphs.OpenDocument(Guid.NewGuid(), document, history);
        var state = new GraphCanvasState();
        var pointer = new GraphCanvasPointerController(state);
        var layout = new GraphCanvasLayout();
        Vector2 origin = default;
        Frame(new(0f, 0f), false);
        Frame(new(0f, 0f), false);
        Vector2 hit = origin + new Vector2(30f, 25f);
        Frame(hit, true);
        Frame(hit + new Vector2(50f, 40f), true);
        Frame(hit + new Vector2(50f, 40f), false);
        Assert.Equal(new GraphPosition(70f, 60f), controller.document.FindNode(first.id)!.position);
        Assert.True(history.Undo().succeeded);
        Assert.Equal(new GraphPosition(20f, 20f), controller.document.FindNode(first.id)!.position);
        Assert.True(history.Redo().succeeded);
        Frame(origin + new Vector2(270f, 100f), true);
        Frame(origin + new Vector2(300f, 60f), false);
        GraphEdgeRecord edge = Assert.Single(controller.document.edges);
        Assert.Equal(first.id, edge.output.nodeId);
        Assert.Equal(second.id, edge.input.nodeId);
        Assert.True(history.Undo().succeeded);
        Assert.Empty(controller.document.edges);
        Assert.True(history.Redo().succeeded);
        Assert.Single(controller.document.edges);

        void Frame(
            Vector2 mouse,
            bool down
        ) {
            var io = NativeGui.Raw.GetIO();
            NativeGui.Raw.AddMousePosEvent(io, mouse.X, mouse.Y);
            NativeGui.Raw.AddMouseButtonEvent(io, 0, down ? (byte)1 : (byte)0);
            host.native.Frame(() =>
            {
                NativeGui.SetNextWindowPos(new Vector2(20f, 20f), (int)ImGuiCond.Always);
                NativeGui.SetNextWindowSize(new Vector2(700f, 450f), (int)ImGuiCond.Always);
                bool open = true;
                _ = NativeGui.Begin("Workflow graph", ref open, (int)(ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoSavedSettings));
                try
                {
                    origin = Ui.GetCursorScreenPosition();
                    Vector2 size = Ui.GetContentRegionAvailable();
                    _ = Ui.InvisibleButton("workflow-canvas", size);
                    var view = new GraphCanvasView(state, origin, size);
                    _ = layout.SetNodes(controller.document.nodes.Select(node => new GraphCanvasNode(node.id, node.position,
                        new(200f, 100f), 24f, "Workflow step", [
                            new(new(node.id, new("previous")), GraphPortDirection.Input, new(0f, 40f), "Previous"),
                            new(new(node.id, new("next")), GraphPortDirection.Output, new(200f, 40f), "Next")
                        ])));
                    GraphCanvasPointerResult result = pointer.Step(view, layout, new()
                    {
                        position = Ui.GetMousePos(),
                        isHovered = Ui.IsItemHovered(Inno.Editor.ImGui.ImGuiHoveredFlags.AllowWhenBlockedByActiveItem),
                        isPressed = Ui.IsMouseClicked(Inno.Editor.ImGui.ImGuiMouseButton.Left),
                        isDown = Ui.IsMouseButtonDown(Inno.Editor.ImGui.ImGuiMouseButton.Left),
                        isReleased = Ui.IsMouseReleased(Inno.Editor.ImGui.ImGuiMouseButton.Left)
                    });
                    if (result.movedNodes is { } positions)
                        controller.MoveNodes(positions);
                    if (result.output is GraphEndpoint output && result.input is GraphEndpoint input)
                        _ = controller.Connect(output, input);
                    var draw = Ui.GetDrawList();
                    foreach (GraphCanvasNode node in layout.nodes)
                        _ = view.DrawNode(draw, node, 0xFF504030, state.selectedNodes.Contains(node.id),
                            previewPosition: pointer.previewPositions.TryGetValue(node.id, out GraphPosition position) ? position : null);
                }
                finally
                {
                    NativeGui.End();
                }
            });
        }
    }

    [Fact]
    public void NonShaderNodeDrawingRestoresItsClipAfterAnExtensionThrows()
    {
        using var host = new NativePanelHost();
        var state = new GraphCanvasState();
        GraphNodeId id = new("workflow.begin");
        var node = new GraphCanvasNode(id, new(10f, 10f), new(200f, 100f), 24f, "Workflow step",
            [new(new(id, new("next")), GraphPortDirection.Output, new(200f, 40f), "Continue", labelBounds: (new(0f, 28f), new(200f, 52f)))]);
        for (int frame = 0; frame < 3; frame++)
        {
            host.Frame(() =>
            {
                NativeGui.SetNextWindowPos(new Vector2(20f, 20f), (int)ImGuiCond.Always);
                NativeGui.SetNextWindowSize(new Vector2(600f, 400f), (int)ImGuiCond.Always);
                bool open = true;
                _ = NativeGui.Begin("Non-shader graph", ref open, (int)ImGuiWindowFlags.NoSavedSettings);
                try
                {
                    ImGuiWindow* window = NativeGui.Raw.GetCurrentWindow();
                    int clipDepth = window->DrawList->ClipRectStack.Size;
                    var draw = Ui.GetDrawList();
                    var view = new GraphCanvasView(state, new(40f, 60f), new(540f, 300f));
                    var failure = new InvalidOperationException("Workflow node extension failed.");
                    Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => view.DrawNode(draw, node, 0xFF504030, true,
                        () => throw failure)));
                    Assert.Equal(clipDepth, window->DrawList->ClipRectStack.Size);
                    Assert.True(view.DrawNode(draw, node, 0xFF504030, false));
                    var offscreen = new GraphCanvasNode(new("workflow.hidden"), new(5000f, 5000f), new(200f, 100f), 24f, "Hidden", []);
                    Assert.False(view.DrawNode(draw, offscreen, 0xFF504030, false, () => throw new InvalidOperationException("Culled content must not run.")));
                    var outsideWindow = new GraphCanvasNode(new("workflow.clipped-parent"), new(900f, 900f), new(200f, 100f), 24f, "Clipped by parent", []);
                    var oversized = new GraphCanvasView(state, new(40f, 60f), new(1800f, 1800f));
                    Assert.False(oversized.DrawNode(draw, outsideWindow, 0xFF504030, false, () => throw new InvalidOperationException("Parent-clipped content must not run.")));
                    Assert.Equal(clipDepth, window->DrawList->ClipRectStack.Size);
                }
                finally
                {
                    NativeGui.End();
                }
            });
        }
    }
}
