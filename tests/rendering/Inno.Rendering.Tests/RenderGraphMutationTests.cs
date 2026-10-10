using System;
using System.Linq;
using Inno.Rendering;
using Xunit;

namespace Inno.Rendering.Tests;

public sealed class RenderGraphMutationTests
{
    [Fact]
    public void LaterScopeCannotChangeAnyPreviouslyAcceptedPassDeclaration()
    {
        var graph = CreateGraph();
        RasterPassBuilder accepted = graph.AddRasterPass("Accepted", new("first"), 0, Execute);
        accepted.HasSideEffect();
        using (RenderGraphMutationScope mutation = graph.BeginMutationScope())
        {
            Assert.Throws<InvalidOperationException>(() => accepted.Before(new("late")));
            Assert.Throws<InvalidOperationException>(() => accepted.After(new("late")));
            Assert.Throws<InvalidOperationException>(() => accepted.AllowParallelRecording());
            Assert.Throws<InvalidOperationException>(() => accepted.ClearPresentationTarget(default));
            mutation.Commit();
        }
        Assert.Equal("Accepted", Assert.Single(graph.Compile().graph!.passes).name);
    }

    [Fact]
    public void InvalidCommitRollsBackOutputsResourcesPhasesAndValidation()
    {
        var graph = CreateGraph();
        graph.AddRasterPass("Accepted", new("first"), 0, Execute).HasSideEffect();
        RenderGraphValidationResult baseline = graph.Validate();
        using (RenderGraphMutationScope mutation = graph.BeginMutationScope())
        {
            RenderTextureHandle texture = graph.CreateTexture("Missing Producer", Descriptor());
            graph.AddRasterPass("Invalid", new("bad"), 0, Execute).ReadTexture(texture).HasSideEffect();
            graph.MarkOutput(texture);
            Assert.False(graph.Validate().isValid);
            Assert.Throws<InvalidOperationException>(() => mutation.Commit());
        }
        RenderGraphValidationResult restored = graph.Validate();
        Assert.Same(baseline, restored);
        Assert.True(restored.isValid);
        Assert.Equal(baseline.passCount, restored.passCount);
        Assert.Equal(baseline.textureCount, restored.textureCount);
        Assert.Empty(restored.diagnostics);
        Assert.Equal("Accepted", Assert.Single(graph.Compile().graph!.passes).name);
    }

    [Fact]
    public void RolledBackHandlesAndPassBuildersCannotAliasNewDeclarations()
    {
        var graph = CreateGraph();
        RenderTextureHandle retired;
        RenderBufferHandle retiredBuffer;
        RasterPassBuilder retiredPass;
        using (graph.BeginMutationScope())
        {
            retired = graph.CreateTexture("Retired", Descriptor());
            retiredBuffer = graph.CreateBuffer("Retired Buffer", new(16, 4, RenderBufferUsage.Vertex));
            retiredPass = graph.AddRasterPass("Retired", new("retired"), 0, Execute);
        }
        RenderTextureHandle current = graph.CreateTexture("Current", Descriptor());
        RenderBufferHandle currentBuffer = graph.CreateBuffer("Current Buffer", new(16, 4, RenderBufferUsage.Vertex));
        Assert.NotEqual(retired, current);
        Assert.NotEqual(retiredBuffer, currentBuffer);
        Assert.Throws<ArgumentException>(() => graph.MarkOutput(retired));
        Assert.Throws<ArgumentException>(() => graph.MarkOutput(retiredBuffer));
        Assert.Throws<InvalidOperationException>(() => retiredPass.HasSideEffect());
        graph.AddRasterPass("Current", new("current"), 0, Execute).UseColorAttachment(current, 0, RenderLoadAction.Clear);
        graph.MarkOutput(current);
        Assert.True(graph.Compile().succeeded);
    }

    [Fact]
    public void NestedCommitRemainsOwnedByOuterRollbackAndScopesEnforceNesting()
    {
        var graph = CreateGraph();
        RenderGraphMutationScope outer = graph.BeginMutationScope();
        RenderGraphMutationScope inner = graph.BeginMutationScope();
        graph.AddRasterPass("Nested", new("nested"), 0, Execute).HasSideEffect();
        Assert.Throws<InvalidOperationException>(() => outer.Dispose());
        Assert.Throws<InvalidOperationException>(() => graph.Compile());
        inner.Commit();
        Assert.Throws<InvalidOperationException>(() => graph.CreateTexture("Too Late", Descriptor()));
        inner.Dispose();
        outer.Dispose();
        Assert.Empty(graph.Compile().graph!.passes);
    }

    [Fact]
    public void RollbackRemovesNameScopeWithoutLateDisposalRemovingAnotherScope()
    {
        var graph = CreateGraph();
        RenderGraphNameScope retired;
        using (graph.BeginMutationScope())
            retired = graph.BeginNameScope("Retired");
        using (graph.BeginNameScope("Current"))
        {
            retired.Dispose();
            graph.AddRasterPass("Visible", new("visible"), 0, Execute).HasSideEffect();
        }
        Assert.Equal("Current/Visible", Assert.Single(graph.Compile().graph!.passes).name);
    }

    [Fact]
    public void MutationCannotCloseAnEarlierNameScopeOrChangeItsPrefix()
    {
        var graph = CreateGraph();
        using RenderGraphNameScope name = graph.BeginNameScope("Accepted");
        using (graph.BeginMutationScope())
            Assert.Throws<InvalidOperationException>(() => name.Dispose());
        graph.AddRasterPass("Visible", new("visible"), 0, Execute).HasSideEffect();
        Assert.Equal("Accepted/Visible", Assert.Single(graph.Compile().graph!.passes).name);
    }

    private static RenderGraphBuilder CreateGraph()
        => new(1, new GraphicsCapabilities(
            GraphicsApi.Noop, GraphicsCapability.None,
            new GraphicsLimits(64, 8, 16384, 16),
            Enum.GetValues<RenderTextureFormat>(), Enum.GetValues<RenderTextureFormat>(),
            [], [], false, false));

    private static RenderTextureDescriptor Descriptor()
        => new(32, 32, RenderTextureFormat.RGBA8, RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled);

    private static void Execute(
        int payload,
        RenderPassContext context
    ) { }
}
