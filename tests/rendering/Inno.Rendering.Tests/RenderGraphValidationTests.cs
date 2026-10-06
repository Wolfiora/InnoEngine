using System;
using System.Linq;
using Inno.Rendering;
using Xunit;

namespace Inno.Rendering.Tests;

public sealed class RenderGraphValidationTests
{
    [Fact]
    public void ValidationIsFrozenAndReusedUntilAnyPassDeclarationChanges()
    {
        var graph = CreateGraph();
        RasterPassBuilder pass = graph.AddRasterPass("Visible", new("visible"), 0, Execute);
        RenderGraphValidationResult culled = graph.Validate();
        Assert.True(culled.isValid);
        Assert.Equal(1, culled.culledPassCount);
        Assert.Same(culled, graph.Validate());

        pass.HasSideEffect();
        RenderGraphValidationResult visible = graph.Validate();
        Assert.NotSame(culled, visible);
        Assert.Equal(1, culled.culledPassCount);
        Assert.Equal(1, visible.scheduledPassCount);
        Assert.Same(visible, graph.Validate());

        RenderGraphCompileResult compiled = graph.Compile();
        Assert.True(compiled.succeeded);
        Assert.Equal(visible.scheduledPassCount, compiled.graph!.passes.Count);
        Assert.Equal(visible.diagnostics, compiled.diagnostics);
        Assert.Throws<InvalidOperationException>(() => graph.Compile());
        Assert.Throws<InvalidOperationException>(() => pass.Before(new("late")));
    }

    [Fact]
    public void LaterPhaseDeclarationInvalidatesCachedScheduleAndDetectsCycle()
    {
        var graph = CreateGraph();
        graph.AddRasterPass("First", new("first"), 0, Execute).HasSideEffect().After(new("second"));
        Assert.True(graph.Validate().isValid);
        graph.AddRasterPass("Second", new("second"), 0, Execute).HasSideEffect().After(new("first"));

        RenderGraphValidationResult invalid = graph.Validate();
        Assert.False(invalid.isValid);
        Assert.Contains(invalid.diagnostics, static item => item.code == "RENDER_GRAPH_CYCLE");
        RenderGraphCompileResult compiled = graph.Compile();
        Assert.False(compiled.succeeded);
        Assert.Equal(invalid.diagnostics, compiled.diagnostics);
    }

    [Fact]
    public void ValidationAndCompilationApplyTheSameViewLimit()
    {
        var graph = CreateGraph(1);
        graph.AddRasterPass("First", new("first"), 0, Execute).HasSideEffect();
        graph.AddRasterPass("Second", new("second"), 0, Execute).HasSideEffect();
        RenderGraphValidationResult validation = graph.Validate();
        Assert.False(validation.isValid);
        Assert.Contains(validation.diagnostics, static item => item.code == "RENDER_GRAPH_VIEW_LIMIT");
        Assert.Equal(validation.diagnostics, graph.Compile().diagnostics);
    }

    [Fact]
    public void ValidationDoesNotInvokeCommandCallbacks()
    {
        var graph = CreateGraph();
        int calls = 0;
        graph.AddRasterPass("Visible", new("visible"), 0, (
            payload,
            context
        ) => calls++).HasSideEffect();
        for (int index = 0; index < 32; index++)
            Assert.True(graph.Validate().isValid);
        Assert.True(graph.Compile().succeeded);
        Assert.Equal(0, calls);
    }

    private static RenderGraphBuilder CreateGraph(int maxViews = 64)
        => new(1, new GraphicsCapabilities(
            GraphicsApi.Noop,
            GraphicsCapability.None,
            new GraphicsLimits(maxViews, 8, 16384, 16),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            [], [], false, false));

    private static void Execute(
        int payload,
        RenderPassContext context
    ) { }
}
