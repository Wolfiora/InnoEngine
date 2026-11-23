using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.Input;
using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Testing.Source;

namespace Inno.Rendering.Runtime.Tests;

[Collection(nameof(RenderRuntimeFixture))]
public sealed class RenderFramePresentationTests : RenderRuntimeFixture
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FrameStatisticsReportBackendCommandsAndGraphCulling(bool reportsAllocations)
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        proxy.frameCounters = new RenderDeviceFrameCounters(3, 2);
        proxy.allocationCounters = reportsAllocations
                                   ? new RenderDeviceAllocationCounters(device.generation, 17, 23, 11)
                                   : null;
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        using IDisposable executionScope = runtime.EnterExecutionScope();
        var asset = new RenderPipelineAsset { pipelineTypeId = StatisticsPipeline.C_EXTENSION_ID };
        runtime.Attach();
        Assert.True(runtime.TryActivateDefaultPipeline(asset));
        runtime.Submit(
            new RenderRequest("Statistics", RenderTarget.backbuffer, new RenderViewport(0, 0, 64, 64), asset)
        );

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        RenderFrameStatistics statistics = Assert.IsType<RenderFrameStatistics>(GraphicsSettings.frameStatistics);
        Assert.Equal(1, statistics.viewCount);
        Assert.Equal(3, statistics.drawCount);
        Assert.Equal(2, statistics.dispatchCount);
        Assert.Equal(1, statistics.culledPassCount);
        Assert.Equal(proxy.allocationCounters, statistics.allocationCounters);
        proxy.allocationCounters = new RenderDeviceAllocationCounters(device.generation, 19, 25, 13);
        Assert.NotEqual(proxy.allocationCounters, statistics.allocationCounters);
        runtime.Detach();
    }

    [Fact]
    public void DeviceSynchronizationIsRequiredAndUnsupportedDiagnosticsAreExplicitlyUnavailable()
    {
        Assert.True(typeof(IRenderDevice).GetMethod(nameof(IRenderDevice.SetVerticalSync))!.IsAbstract);
        using var backend = new RecordingRenderDevice();
        IRenderDevice device = backend;
        device.SetVerticalSync(true);
        Assert.True(backend.verticalSync);
        device.SetVerticalSync(false);
        Assert.False(backend.verticalSync);
        Assert.Null(device.allocationCounters);
        Assert.Throws<ArgumentOutOfRangeException>(() => new RenderDeviceAllocationCounters(0, 0, 0, 0));
    }

    [Fact]
    public void GraphicsFacadeResolvesTheCurrentlyBoundRenderingRuntime()
    {
        IRenderDevice firstDevice = TestDeviceProxy.Create(out TestDeviceProxy firstProxy);
        IRenderDevice secondDevice = TestDeviceProxy.Create(out TestDeviceProxy secondProxy);
        firstProxy.frameCounters = new RenderDeviceFrameCounters(2, 0);
        secondProxy.frameCounters = new RenderDeviceFrameCounters(7, 1);
        using var first = new RenderRuntime(m_types, firstDevice, new TestDiagnosticSink());
        using var second = new RenderRuntime(m_types, secondDevice, new TestDiagnosticSink());
        var asset = new RenderPipelineAsset { pipelineTypeId = StatisticsPipeline.C_EXTENSION_ID };

        using (first.EnterExecutionScope())
        {
            first.Attach();
            Assert.True(first.TryActivateDefaultPipeline(asset));
            first.Submit(CreateRequest("First Runtime", asset));
            BeginRenderFrame(first, 0f);
            first.AfterRender(default);
        first.EndFrame(default);
            Assert.Equal(2, Assert.IsType<RenderFrameStatistics>(GraphicsSettings.frameStatistics).drawCount);

            using (second.EnterExecutionScope())
            {
                second.Attach();
                Assert.True(second.TryActivateDefaultPipeline(asset));
                second.Submit(CreateRequest("Second Runtime", asset));
                BeginRenderFrame(second, 0f);
                second.AfterRender(default);
        second.EndFrame(default);
                Assert.Equal(7, Assert.IsType<RenderFrameStatistics>(GraphicsSettings.frameStatistics).drawCount);
            }

            Assert.Equal(2, Assert.IsType<RenderFrameStatistics>(GraphicsSettings.frameStatistics).drawCount);
        }

        Assert.Null(GraphicsSettings.frameStatistics);
        Assert.Throws<InvalidOperationException>(() => GraphicsSettings.defaultPipeline = asset);
        second.Detach();
        first.Detach();
    }

    [Fact]
    public void RequestProviderProducesCurrentFrameWorkThroughPublicPluginApi()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        var asset = new RenderPipelineAsset { pipelineTypeId = SideEffectPipeline.C_EXTENSION_ID };
        TestRequestProvider.pipeline = asset;
        TestRequestProvider.enabled = true;
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        runtime.Attach();

        BeginRenderFrame(runtime, 1f / 60f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(1, TestRequestProvider.submitCount);
        Assert.Equal(1, proxy.executeCount);
        CompiledRenderPass pass = Assert.Single(Assert.IsType<CompiledRenderGraph>(proxy.lastGraph).passes);
        Assert.Equal("Request[0] Provider Request/Visible", pass.name);
        runtime.Detach();
    }

    [Fact]
    public void RequestProviderReceivesExplicitHostContentScope()
    {
        IRenderDevice device = TestDeviceProxy.Create(out _);
        Guid contentID = Guid.Parse("3aee0ced-b598-4366-ab45-a6ef8e0feb30");
        var content = new TestContent();
        var identities = new IdentityAllocator();
        identities.Register(content, contentID);
        var scope = new ContentReadScope([content.identity], contentID);
        Assert.True(scope.TryGetValue(contentID, out TestContent? resolved));
        Assert.Same(content, resolved);
        TestRequestProvider.enabled = true;
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink(), contentScopeProvider: () => scope);
        runtime.Attach();

        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Same(scope, TestRequestProvider.lastContent);
        Assert.Throws<ObjectDisposedException>(() => scope.TryGetValue(contentID, out TestContent? _));
        runtime.Detach();
    }

    [Fact]
    public void PrimaryPresentationViewportIsSharedAndLetterboxSurfaceIsCleared()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        proxy.presentationSize = new RenderPresentationSize(1000, 1000);
        TestRequestProvider.enabled = true;
        var runtime = new RenderRuntime(
            m_types,
            device,
            new TestDiagnosticSink(),
            primaryPresentationViewportProvider: static size => new RenderViewport(
                0,
                (size.height - 562) / 2,
                size.width,
                562
            )
        );
        runtime.Attach();

        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(new RenderViewport(0, 219, 1000, 562), TestRequestProvider.lastPresentationViewport);
        CompiledRenderPass pass = Assert.Single(Assert.IsType<CompiledRenderGraph>(proxy.lastGraph).passes);
        Assert.Equal("Primary Presentation Background", pass.name);
        Assert.True(pass.clearsPresentationTarget);
        Assert.Equal(new RenderClearColor(0f, 0f, 0f, 1f), pass.presentationClearColor);
        runtime.Detach();
    }

    [Fact]
    public void PrimaryOutputScalesLogicalWindowPointerToPhysicalPixels()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        proxy.presentationSize = new RenderPresentationSize(2560, 1440);
        TestRequestProvider.enabled = true;
        var snapshot = new InputSnapshot(
            1,
            mousePosition: new Vector2(400f, 274f),
            mouseButtonsPressed: [MouseButton.Left]
        );
        using var runtime = new RenderRuntime(
            m_types,
            device,
            new TestDiagnosticSink(),
            inputSnapshotProvider: () => snapshot,
            primaryInputSurfaceSizeProvider: static () => new RenderPresentationSize(1280, 720)
        );

        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(new Vector2(800f, 548f), TestRequestProvider.lastInput.pointerPosition);
        Assert.Single(TestRequestProvider.lastInput.buttonsPressed);
    }

    [Fact]
    public void CompetingModelsDoNotShareATargetWhenAnExactRouteExists()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        var diagnostics = new TestDiagnosticSink();
        proxy.capabilities = new GraphicsCapabilities(
            GraphicsApi.metal,
            GraphicsCapability.None,
            new GraphicsLimits(64, 4, 4096, 8),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            [],
            [],
            originBottomLeft: false,
            homogeneousDepth: false
        );
        proxy.presentationSize = new RenderPresentationSize(1000, 1000);
        FirstTestRenderModel.enabled = true;
        SecondTestRenderModel.enabled = true;
        using var runtime = new RenderRuntime(
            m_types,
            device,
            diagnostics,
            primaryPresentationViewportProvider: static _ => new RenderViewport(100, 200, 800, 600),
            compositionProgramProvider: new TestCompositionProgramProvider()
        );

        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Equal(0, FirstTestRenderModel.buildCount);
        Assert.Equal(0, SecondTestRenderModel.buildCount);
        Assert.Contains(diagnostics.items, item => (
            item.code == "RENDER_OUTPUT_MODEL_UNAVAILABLE"
            && item.message.Contains("Multiple rendering models", StringComparison.Ordinal)
        ));

        runtime.SetPrimaryRoute(
            new RenderOutputRoute(
                [
                    new RenderOutputLayer(SecondTestRenderModel.C_EXTENSION_ID, []),
                    new RenderOutputLayer(FirstTestRenderModel.C_EXTENSION_ID, [])
                ]
            )
        );
        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Equal(1, FirstTestRenderModel.buildCount);
        Assert.Equal(1, SecondTestRenderModel.buildCount);
        Assert.Equal(
            [new RenderViewport(0, 0, 800, 600), new RenderViewport(0, 0, 800, 600)],
            CompositionLayerPipeline.viewports
        );
        Assert.DoesNotContain(diagnostics.items, item => item.code == "RENDER_OUTPUT_MODEL_UNAVAILABLE");
        Assert.DoesNotContain(diagnostics.items, item => (
            item.code == "RENDER_OUTPUT_MODEL_UNAVAILABLE"
            && item.message.Contains("independent model targets", StringComparison.Ordinal)
        ));
        Assert.True(
            diagnostics.items.All(item => item.code != "RENDER_OUTPUT_COMPOSITION_FAILED"),
            string.Join("\n", diagnostics.items.Select(item => item.message))
        );
        Assert.True(proxy.lastGraph is not null, string.Join("\n", diagnostics.items.Select(item => item.message)));
        Assert.Contains(proxy.lastGraph.passes, pass => pass.name.Contains("Layer 2", StringComparison.Ordinal));
        runtime.Detach();
    }

    [Fact]
    public void OutputWithoutAnApplicableModelReportsItsMissingRenderer()
    {
        var diagnostics = new TestDiagnosticSink();
        using var runtime = new RenderRuntime(m_types, new RecordingRenderDevice(), diagnostics);
        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Contains(diagnostics.items, item => (
            item.code == "RENDER_OUTPUT_MODEL_UNAVAILABLE"
            && item.message.Contains("No rendering model", StringComparison.Ordinal)
        ));
        runtime.Detach();
    }

    [Fact]
    public void LinearPresentationReceivesOneTransferAfterAllModelLayers()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        proxy.primaryPresentationEncodesSrgb = false;
        proxy.presentationSize = new RenderPresentationSize(800, 600);
        FirstTestRenderModel.enabled = true;
        SecondTestRenderModel.enabled = true;
        var diagnostics = new TestDiagnosticSink();
        using var runtime = new RenderRuntime(
            m_types,
            device,
            diagnostics,
            compositionProgramProvider: new TestCompositionProgramProvider()
        );
        runtime.SetPrimaryRoute(
            new RenderOutputRoute(
                [
                    new RenderOutputLayer(FirstTestRenderModel.C_EXTENSION_ID, []),
                    new RenderOutputLayer(SecondTestRenderModel.C_EXTENSION_ID, [])
                ]
            )
        );

        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.DoesNotContain(diagnostics.items, item => item.code == "RENDER_OUTPUT_COMPOSITION_FAILED");
        Assert.NotNull(proxy.lastGraph);
        Assert.Single(proxy.lastGraph.passes.Where(pass => pass.name.Contains("Output Transfer", StringComparison.Ordinal)));
        Assert.Contains("Output Transfer", proxy.lastGraph.passes[^1].name, StringComparison.Ordinal);
        Assert.Contains(proxy.lastGraph.textures, texture => (
            texture.name.EndsWith("/Composition", StringComparison.Ordinal)
            && texture.descriptor.width == 800
            && texture.descriptor.height == 600
            && texture.descriptor.format == RenderTextureFormat.RGBA8Srgb
        ));
        runtime.Detach();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SingleModelPresentationEncodesColorExactlyOnce(bool presentationEncodesSrgb)
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        proxy.primaryPresentationEncodesSrgb = presentationEncodesSrgb;
        proxy.presentationSize = new RenderPresentationSize(800, 600);
        FirstTestRenderModel.enabled = true;
        var diagnostics = new TestDiagnosticSink();
        using var runtime = new RenderRuntime(
            m_types,
            device,
            diagnostics,
            compositionProgramProvider: new TestCompositionProgramProvider()
        );

        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(1, FirstTestRenderModel.buildCount);
        Assert.DoesNotContain(diagnostics.items, item => item.code == "RENDER_OUTPUT_COMPOSITION_FAILED");
        Assert.NotNull(proxy.lastGraph);
        Assert.Equal(
            presentationEncodesSrgb ? 0 : 1,
            proxy.lastGraph.passes.Count(pass => pass.name.Contains("Output Transfer", StringComparison.Ordinal))
        );
        Assert.DoesNotContain(
            proxy.lastGraph.textures,
            texture => texture.name.EndsWith("/Composition", StringComparison.Ordinal)
        );
        if (!presentationEncodesSrgb)
        {
            Assert.Contains("Output Transfer", proxy.lastGraph.passes[^1].name, StringComparison.Ordinal);
            Assert.Contains(proxy.lastGraph.textures, texture => (
                texture.name.Contains("Model Layer 1", StringComparison.Ordinal)
                && texture.descriptor.format == RenderTextureFormat.RGBA8Srgb
            ));
        }
        runtime.Detach();
    }

    [Fact]
    public void EditorOwnedOffscreenOutputsDoNotReportAnUnusedPrimaryModel()
    {
        var diagnostics = new TestDiagnosticSink();
        using var runtime = new RenderRuntime(m_types, new RecordingRenderDevice(), diagnostics);
        runtime.SetPrimaryModelOutputEnabled(false);
        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.DoesNotContain(diagnostics.items, item => item.code == "RENDER_OUTPUT_MODEL_UNAVAILABLE");
        runtime.Detach();
    }

    [Fact]
    public void MultipleRequestsAndContributorsCompileAndExecuteAsOneFrameGraph()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        var asset = new RenderPipelineAsset { pipelineTypeId = SideEffectPipeline.C_EXTENSION_ID };
        var contributor = new SideEffectContributor();
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink(), [contributor]);
        runtime.Submit(CreateRequest("Second", asset, priority: 10));
        runtime.Submit(CreateRequest("First", asset, priority: -10));

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(1, proxy.executeCount);
        string[] names = Assert.IsType<CompiledRenderGraph>(proxy.lastGraph).passes
            .Select(static pass => pass.name)
            .ToArray();
        string[] expectedNames =
        [
            "Request[0] First/Visible",
            "Request[1] Second/Visible",
            "Contributor[0] SideEffectContributor/Overlay"
        ];
        Assert.Equal(expectedNames, names);
        runtime.Detach();
    }

    [Fact]
    public void OverlappingRequestsPreserveEarlierPresentationLayersInSchedulingOrder()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        var asset = new RenderPipelineAsset { pipelineTypeId = PresentationPipeline.C_EXTENSION_ID };
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        runtime.Submit(CreateRequest("Overlay", asset, priority: 100));
        runtime.Submit(CreateRequest("Base", asset, priority: 0));

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(
            new[]
            {
                new PresentationObservation("Base", false),
                new PresentationObservation("Overlay", true)
            },
            PresentationPipeline.observations
        );
        Assert.Equal(1, proxy.executeCount);
        runtime.Detach();
    }

    [Fact]
    public void DisjointViewportsCanInitializeTheSamePresentationTargetIndependently()
    {
        IRenderDevice device = TestDeviceProxy.Create(out _);
        var asset = new RenderPipelineAsset { pipelineTypeId = PresentationPipeline.C_EXTENSION_ID };
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        runtime.Submit(new RenderRequest("Left", RenderTarget.backbuffer, new RenderViewport(0, 0, 32, 64), asset));
        runtime.Submit(new RenderRequest("Right", RenderTarget.backbuffer, new RenderViewport(32, 0, 32, 64), asset));

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(
            new[]
            {
                new PresentationObservation("Left", false),
                new PresentationObservation("Right", false)
            },
            PresentationPipeline.observations
        );
        runtime.Detach();
    }

    [Fact]
    public void FailedRequestDoesNotClaimThePresentationTarget()
    {
        IRenderDevice device = TestDeviceProxy.Create(out _);
        var failed = new RenderPipelineAsset { pipelineTypeId = ThrowingPipeline.C_EXTENSION_ID };
        var valid = new RenderPipelineAsset { pipelineTypeId = PresentationPipeline.C_EXTENSION_ID };
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        runtime.Submit(CreateRequest("Failed", failed, priority: 0));
        runtime.Submit(CreateRequest("Recovery", valid, priority: 100));

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(new[] { new PresentationObservation("Recovery", false) }, PresentationPipeline.observations);
        runtime.Detach();
    }

    [Fact]
    public void FailedRequestRollsBackWithoutDiscardingOtherRequests()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        var diagnostics = new TestDiagnosticSink();
        var failed = new RenderPipelineAsset { pipelineTypeId = ThrowingPipeline.C_EXTENSION_ID };
        var valid = new RenderPipelineAsset { pipelineTypeId = SideEffectPipeline.C_EXTENSION_ID };
        var runtime = new RenderRuntime(m_types, device, diagnostics);
        runtime.Submit(CreateRequest("A Failed", failed));
        runtime.Submit(CreateRequest("B Valid", valid));

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(1, proxy.executeCount);
        CompiledRenderPass pass = Assert.Single(Assert.IsType<CompiledRenderGraph>(proxy.lastGraph).passes);
        Assert.Equal("Request[1] B Valid/Visible", pass.name);
        Assert.Contains(diagnostics.items, diagnostic => diagnostic.code == "RENDER_REQUEST_FAILED");

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.DoesNotContain(diagnostics.items, diagnostic => diagnostic.code == "RENDER_REQUEST_FAILED");
        runtime.Detach();
    }

    [Fact]
    public void PendingShaderCompilationDoesNotPublishAnUnavailableArtifactError()
    {
        var contract = new ShaderContractID("tests.pending.contract");
        var role = new ShaderPassRoleID("draw");
        var pass = new ShaderPassDefinition("draw", ShaderProgramKind.Raster);
        var shader = new ShaderAsset();
        Assert.True(m_identities.Register(shader));
        shader.SetDefinition(
            new ShaderDefinition(
                "Pending Shader",
                [],
                [],
                [pass],
                [new ShaderTechniqueDefinition(
                    new ShaderTechniqueID("default"),
                    contract,
                    [new ShaderTechniquePass(role, pass.name)]
                )]
            ),
            m_serialization,
            SerializationContext.empty
        );
        PendingShaderPipeline.material = new MaterialAsset { shader = shader };
        PendingShaderPipeline.contract = contract;
        PendingShaderPipeline.role = role;

        var diagnostics = new TestDiagnosticSink();
        var compiler = new DelayedTextureArtifactSource();
        var runtime = new RenderRuntime(
            m_types,
            new RecordingRenderDevice(),
            diagnostics,
            targetArtifacts: new DelayedArtifactProvider(compiler)
        );
        var asset = new RenderPipelineAsset { pipelineTypeId = PendingShaderPipeline.C_EXTENSION_ID };
        runtime.Submit(CreateRequest("Pending Shader", asset));

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.False(PendingShaderPipeline.resolved);
        Assert.DoesNotContain(
            diagnostics.items,
            static diagnostic => diagnostic.code == "RENDER_SHADER_TARGET_UNAVAILABLE"
        );
        runtime.Detach();
    }

    [Fact]
    public void UnavailablePrimaryOutputSkipsInputScalingAndKeepsOffscreenWork()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        proxy.presentationSize = null;
        TestRequestProvider.enabled = true;
        FirstTestRenderModel.enabled = true;
        using var runtime = new RenderRuntime(
            m_types,
            device,
            new TestDiagnosticSink(),
            primaryInputSurfaceSizeProvider: static () => throw new InvalidOperationException("There is no primary input surface.")
        );
        var asset = new RenderPipelineAsset { pipelineTypeId = CompositionLayerPipeline.C_EXTENSION_ID };
        var target = new RenderTexture(
            "Offscreen",
            new RenderTextureDescriptor(
                32,
                24,
                RenderTextureFormat.RGBA8,
                RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled
            )
        );
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
            m_types,
            device,
            new TestDiagnosticSink(),
            primaryInputSurfaceSizeProvider: static () => null
        );
        BeginRenderFrame(runtime, 0f);
        runtime.Render(default);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.False(TestRequestProvider.lastInput.interactionEnabled);
        Assert.Empty(TestRequestProvider.lastInput.buttonsPressed);
    }

    private sealed class TestContent : IdentityObject;

    [RenderPipelineExtension(C_EXTENSION_ID)]
    private sealed class StatisticsPipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.statistics";

        public override void Build(RenderPipelineContext context)
        {
            context.graph
                .AddRasterPass(
                    "Visible",
                    new RenderPhaseID("tests.statistics.visible"),
                    0,
                    static (
                        _,
                        _
                    ) => {}
                )
                .HasSideEffect();
            context.graph.AddRasterPass(
                "Culled",
                new RenderPhaseID("tests.statistics.culled"),
                0,
                static (
                    _,
                    _
                ) => {}
            );
        }
    }

    [RenderPipelineExtension(C_EXTENSION_ID)]
    private sealed class ThrowingPipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.throwing";

        public override void Build(RenderPipelineContext context)
        {
            context.graph
                .AddRasterPass(
                    "Rolled Back",
                    new RenderPhaseID("tests.runtime.rolled-back"),
                    0,
                    static (
                        _,
                        _
                    ) => {}
                )
                .HasSideEffect();
            throw new InvalidOperationException("Expected request failure.");
        }
    }

    private sealed class TestCompositionProgramProvider : IRenderLayerCompositionProgramProvider
    {
        public GraphicsPipelineDescriptor CreateDescriptor(
            GraphicsCapabilities capabilities,
            RenderVertexLayout vertexLayout
        ) {
            _ = capabilities;
            return new GraphicsPipelineDescriptor(
                [1],
                [1],
                [new RenderShaderBindingDescriptor(
                    new RenderBindingID("s_tex"),
                    RenderShaderBindingKind.Texture,
                    0,
                    nativeName: "s_tex"
                )],
                vertexLayout
            );
        }

        public GraphicsPipelineDescriptor CreateOutputTransferDescriptor(
            GraphicsCapabilities capabilities,
            RenderVertexLayout vertexLayout
        ) => CreateDescriptor(capabilities, vertexLayout);
    }
}
