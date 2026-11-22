using System;
using System.Numerics;
using Xunit;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.Graph.ImGui;

namespace Inno.Editor.Presentation.Tests;

public sealed class GraphCanvasViewTests
{
    [Fact]
    public void FrameProjectionDoesNotChangeWhenLaterNavigationMutatesItsSource()
    {
        var state = new GraphCanvasState();
        var view = new GraphCanvasView(state, new Vector2(100f, 200f), new Vector2(800f, 600f));
        state.SetViewport(new GraphPosition(50f, 75f), 2f);
        Assert.Equal(new Vector2(110f, 220f), view.ToScreen(new GraphPosition(10f, 20f)));
        Assert.Equal(1f, view.zoom);
        var next = new GraphCanvasView(state, new Vector2(100f, 200f), new Vector2(800f, 600f));
        Assert.Equal(new Vector2(170f, 315f), next.ToScreen(new GraphPosition(10f, 20f)));
        Assert.Equal(2f, next.zoom);
    }

    [Theory]
    [InlineData(0.1f)]
    [InlineData(1f)]
    [InlineData(4f)]
    public void ViewportRoundTripPreservesTheGraphPoint(float zoom)
    {
        var state = new GraphCanvasState();
        state.SetViewport(new GraphPosition(-172f, 38f), zoom);
        var view = new GraphCanvasView(state, new Vector2(100f, 200f), new Vector2(800f, 600f));
        var point = new GraphPosition(-21.25f, 90f);
        GraphPosition actual = view.ToGraph(view.ToScreen(point));
        Assert.Equal(point.x, actual.x, 3);
        Assert.Equal(point.y, actual.y, 3);
    }

    [Fact]
    public void ConnectionHitTestUsesTheDisplayedRouteIncludingEndpoints()
    {
        Assert.True(GraphCanvasView.HitConnection(new Vector2(0f, 0f), Vector2.Zero, new Vector2(200f, 100f)));
        Assert.True(GraphCanvasView.HitConnection(new Vector2(100f, 50f), Vector2.Zero, new Vector2(200f, 100f)));
        Assert.False(GraphCanvasView.HitConnection(new Vector2(100f, 120f), Vector2.Zero, new Vector2(200f, 100f)));
        Assert.True(GraphCanvasView.HitConnection(new Vector2(200f, 100f), Vector2.Zero, new Vector2(200f, 100f)));
    }

    [Fact]
    public void InvalidViewportAndHitToleranceAreRejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new GraphCanvasView(new GraphCanvasState(), Vector2.Zero, new Vector2(float.NaN, 100f))
        );
        Assert.Throws<ArgumentOutOfRangeException>(() => GraphCanvasView.HitConnection(Vector2.Zero, Vector2.Zero, Vector2.One, -1f));
    }
}
