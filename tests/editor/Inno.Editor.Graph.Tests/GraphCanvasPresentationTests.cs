using System;
using System.Linq;
using System.Numerics;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.Graph.ImGui;
using Xunit;

namespace Inno.Editor.Graph.Tests;

public sealed class GraphCanvasPresentationTests
{
    [Fact]
    public void NonShaderNodesUseTheSameOrderedHitAndBoxGeometry()
    {
        var state = new GraphCanvasState();
        state.SetViewport(new(17f, -9f), 1.5f);
        var view = new GraphCanvasView(state, new(100f, 200f), new(800f, 600f));
        GraphCanvasNode first = Node("workflow.start", new(0f, 0f));
        GraphCanvasNode second = Node("workflow.finish", new(50f, 0f));
        var layout = new GraphCanvasLayout();
        Assert.Equal(2, layout.SetNodes([first, second]));
        Assert.Same(second, view.HitNode(layout, view.ToScreen(new(60f, 10f))));
        Assert.Equal(new[] { first.id, second.id }, view.BoxNodes(layout, view.ToScreen(new(-5f, -5f)), view.ToScreen(new(300f, 100f))));
        Assert.Equal(0, layout.SetNodes([second, first]));
        Assert.Same(first, view.HitNode(layout, view.ToScreen(new(60f, 10f))));
    }

    [Fact]
    public void PortHitGeometryPreservesMissingAndExplicitLabelBounds()
    {
        var state = new GraphCanvasState();
        var view = new GraphCanvasView(state, Vector2.Zero, new(800f, 600f));
        GraphNodeId id = new("workflow.output");
        var port = new GraphCanvasPort(new(id, new("next")), GraphPortDirection.Output, new(200f, 40f), "Continue",
            isMissing: true, labelBounds: (new(100f, 28f), new(200f, 52f)));
        var node = new GraphCanvasNode(id, new(10f, 10f), new(200f, 100f), 24f, "Flow step", [port]);
        var layout = new GraphCanvasLayout();
        _ = layout.SetNodes([node]);
        Assert.Equal(port, view.HitPort(layout, new(210f, 50f)));
        Assert.Equal(port, view.HitPort(layout, new(150f, 50f)));
        Assert.Null(view.HitPort(layout, new(150f, 90f)));
        Assert.Throws<ArgumentOutOfRangeException>(() => view.HitPort(layout, Vector2.Zero, -1f));
    }

    [Fact]
    public void ChangingOneOfTwoThousandNodesPreservesOtherGeometry()
    {
        GraphCanvasNode[] nodes = Enumerable.Range(0, 2000)
            .Select(index => Node("workflow." + index, new(index * 220f, 0f))).ToArray();
        var layout = new GraphCanvasLayout();
        Assert.Equal(2000, layout.SetNodes(nodes));
        GraphCanvasNode retained = nodes[999];
        nodes[1000] = Node(nodes[1000].id.value, new(100f, 200f));
        Assert.Equal(1, layout.SetNodes(nodes));
        Assert.True(layout.TryGetNode(retained.id, out GraphCanvasNode? current));
        Assert.Same(retained, current);
        Assert.Equal(0, layout.SetNodes(nodes));
    }

    [Fact]
    public void InvalidCandidatesDoNotReplaceTheCurrentSnapshot()
    {
        var layout = new GraphCanvasLayout();
        GraphCanvasNode node = Node("workflow.valid", default);
        _ = layout.SetNodes([node]);
        Assert.Throws<ArgumentException>(() => layout.SetNodes([node, node]));
        Assert.Same(node, Assert.Single(layout.nodes));
        Assert.Throws<ArgumentOutOfRangeException>(() => new GraphCanvasNode(node.id, default, new(float.NaN, 10f), 2f, "Invalid", []));
        Assert.Throws<ArgumentException>(() => new GraphCanvasNode(node.id, default, new(200f, 100f), 24f, "Invalid",
            [new GraphCanvasPort(new(new("foreign"), new("port")), GraphPortDirection.Input, Vector2.Zero, "Foreign")]));
    }

    private static GraphCanvasNode Node(
        string id,
        GraphPosition position
    ) => new(new(id), position, new(200f, 100f), 24f, id, []);
}
