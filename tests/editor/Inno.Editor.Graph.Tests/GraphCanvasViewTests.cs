using System;
using System.Numerics;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.Graph.ImGui;
using Xunit;

namespace Inno.Editor.Graph.Tests;
public sealed class GraphCanvasViewTests
{
    [Theory]
    [InlineData(0.1f)]
    [InlineData(1f)]
    [InlineData(4f)]
    public void ViewportRoundTripPreservesTheGraphPoint(float zoom)
    {
        var state = new GraphCanvasState();
        state.SetViewport(new(-172f, 38f), zoom);
        var view = new GraphCanvasView(state, new(100f, 200f), new(800f, 600f));
        GraphPosition point = new(-21.25f, 90f);
        GraphPosition actual = view.ToGraph(view.ToScreen(point));
        Assert.Equal(point.x, actual.x, 3);
        Assert.Equal(point.y, actual.y, 3);
    }

    [Fact]
    public void ConnectionHitTestUsesTheDisplayedRouteIncludingEndpoints()
    {
        Assert.True(GraphCanvasView.HitConnection(new(0f, 0f), Vector2.Zero, new(200f, 100f)));
        Assert.True(GraphCanvasView.HitConnection(new(100f, 50f), Vector2.Zero, new(200f, 100f)));
        Assert.False(GraphCanvasView.HitConnection(new(100f, 120f), Vector2.Zero, new(200f, 100f)));
        Assert.True(GraphCanvasView.HitConnection(new(200f, 100f), Vector2.Zero, new(200f, 100f)));
    }

    [Fact]
    public void InvalidViewportAndHitToleranceAreRejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GraphCanvasView(new GraphCanvasState(), Vector2.Zero, new(float.NaN, 100f)));
        Assert.Throws<ArgumentOutOfRangeException>(() => GraphCanvasView.HitConnection(Vector2.Zero, Vector2.Zero, Vector2.One, -1f));
    }
}
