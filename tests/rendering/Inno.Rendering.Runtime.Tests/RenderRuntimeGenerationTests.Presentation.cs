using System;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Xunit;

namespace Inno.Rendering.Runtime.Tests;

public sealed partial class RenderRuntimeGenerationTests
{
    [Fact]
    public void UnavailablePrimaryOutputSkipsInputScalingAndKeepsOffscreenWork()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        proxy.presentationSize = null;
        TestRequestProvider.enabled = true;
        FirstTestRenderModel.enabled = true;
        using var runtime = new RenderRuntime(
            m_types, device, new TestDiagnosticSink(),
            primaryInputSurfaceSizeProvider: static () => throw new InvalidOperationException("There is no primary input surface."));
        var asset = new RenderPipelineAsset { pipelineTypeId = CompositionLayerPipeline.extensionId };
        var target = new RenderTexture("Offscreen", new RenderTextureDescriptor(
            32, 24, RenderTextureFormat.RGBA8, RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled));
        runtime.Submit(new RenderRequest("Offscreen", RenderTarget.FromTexture(target), new RenderViewport(0, 0, 32, 24), asset));
        runtime.Submit(new RenderRequest("Unavailable", RenderTarget.backbuffer, new RenderViewport(0, 0, 32, 24), asset));
        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Null(TestRequestProvider.lastPresentationViewport);
        Assert.False(TestRequestProvider.lastInput.interactionEnabled);
        Assert.Empty(TestRequestProvider.lastInput.buttonsPressed);
        Assert.Equal(0, FirstTestRenderModel.buildCount);
        Assert.Single(CompositionLayerPipeline.viewports);
        Assert.Equal(new RenderViewport(0, 0, 32, 24), CompositionLayerPipeline.viewports[0]);
        Assert.Equal(1, proxy.executeCount);
        Assert.True(runtime.targets.TryGetTexture(target, out _));
        proxy.ReleaseRecordedGraph();
    }

    [Fact]
    public void RestoringPrimaryOutputPublishesRealExtentToTheNextCompleteFrame()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        proxy.presentationSize = null;
        FirstTestRenderModel.enabled = true;
        TestRequestProvider.enabled = true;
        using var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Equal(0, FirstTestRenderModel.buildCount);
        device.SetPrimaryPresentationSize(new RenderPresentationSize(2560, 1440));
        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Equal(1, FirstTestRenderModel.buildCount);
        Assert.Equal(new RenderViewport(0, 0, 2560, 1440), TestRequestProvider.lastPresentationViewport);
        proxy.ReleaseRecordedGraph();
    }

    [Fact]
    public void InvalidBackendExtentFailsAndClosesTheDeviceFrame()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        proxy.presentationSize = default(RenderPresentationSize);
        using var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        Assert.Throws<InvalidOperationException>(() => BeginRenderFrame(runtime, 0f));
        Assert.False(proxy.frameOpen);
        Assert.Equal(1, proxy.endFrameCount);
    }

    [Fact]
    public void UnavailableLogicalInputSurfaceDoesNotDispatchPrimaryInteraction()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        proxy.presentationSize = new RenderPresentationSize(1280, 720);
        TestRequestProvider.enabled = true;
        using var runtime = new RenderRuntime(
            m_types, device, new TestDiagnosticSink(), primaryInputSurfaceSizeProvider: static () => null);
        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.False(TestRequestProvider.lastInput.interactionEnabled);
        Assert.Empty(TestRequestProvider.lastInput.buttonsPressed);
    }
}
