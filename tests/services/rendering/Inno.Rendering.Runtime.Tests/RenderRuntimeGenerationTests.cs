using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Assets;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Testing.Source;

namespace Inno.Rendering.Runtime.Tests;

[Collection(nameof(RenderRuntimeFixture))]
public sealed class RenderRuntimeGenerationTests : RenderRuntimeFixture
{
    [Fact]
    public void SuccessfulTypeCacheChangeRetiresUnrequestedPipelineAtFrameBoundary()
    {
        IRenderDevice device = TestDeviceProxy.Create(out _);
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        var asset = new RenderPipelineAsset { pipelineTypeId = DisposablePipeline.C_EXTENSION_ID };

        Assert.True(runtime.TryActivateDefaultPipeline(asset));
        Assert.Equal(1, DisposablePipeline.createdCount);
        Assert.Equal(0, DisposablePipeline.disposedCount);

        m_types.Rebuild();
        BeginRenderFrame(runtime, 0f);

        Assert.Equal(1, DisposablePipeline.disposedCount);

        runtime.AfterRender(default);
        runtime.EndFrame(default);
        runtime.Detach();
    }

    [Fact]
    public void PendingProviderRetirementIsRetriedBeforeTheSnapshotIsReleased()
    {
        using var runtime = new RenderRuntime(m_types, TestDeviceProxy.Create(out _), new TestDiagnosticSink());
        Assert.True(runtime.TryActivateDefaultPipeline(new RenderPipelineAsset { pipelineTypeId = DisposablePipeline.C_EXTENSION_ID }));
        TestRequestProvider.pendingRetirements = 2;

        m_types.Rebuild();

        Assert.Equal(3, TestRequestProvider.retirementAttempts);
        Assert.True(runtime.TryActivateDefaultPipeline(new RenderPipelineAsset { pipelineTypeId = DisposablePipeline.C_EXTENSION_ID }));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void InvalidInitialContributorsFailBeforeRegisteringAnExtensionOwner(bool duplicate)
    {
        var contributor = new SideEffectContributor();
        IRenderFrameGraphContributor[] contributors = duplicate ? [contributor, contributor] : [null!];
        Assert.Throws<ArgumentException>(() => new RenderRuntime(
            m_types,
            TestDeviceProxy.Create(out _),
            new TestDiagnosticSink(),
            contributors
        ));
        m_types.Rebuild();
        Assert.Equal(0, TestRequestProvider.retirementAttempts);
    }

    [Fact]
    public void PendingFeatureRetirementKeepsItsPipelineAliveUntilQuiescent()
    {
        var runtime = new RenderRuntime(m_types, TestDeviceProxy.Create(out TestDeviceProxy device), new TestDiagnosticSink());
        Assert.True(
            runtime.TryActivateDefaultPipeline(new RenderPipelineAsset
            {
                pipelineTypeId = DisposablePipeline.C_EXTENSION_ID,
                features = [new RenderFeatureConfiguration(PendingFeature.C_EXTENSION_ID)]
            })
        );
        PendingFeature.pendingRetirements = 2;
        DisposablePipeline.pendingRetirements = 1;

        runtime.Dispose();
        runtime.Dispose();

        Assert.Equal(3, PendingFeature.retirementAttempts);
        Assert.Equal(2, DisposablePipeline.retirementAttempts);
        Assert.Equal(1, DisposablePipeline.disposedCount);
        Assert.Equal(1, device.endFrameCount);
        Assert.Throws<ObjectDisposedException>(() => runtime.Submit(CreateRequest("Late", new RenderPipelineAsset())));
    }

    [Fact]
    public void RejectedPipelineWaitsForItsCandidateToRetireBeforeKeepingLastGood()
    {
        using var runtime = new RenderRuntime(m_types, TestDeviceProxy.Create(out _), new TestDiagnosticSink());
        var asset = new RenderPipelineAsset { pipelineTypeId = DisposablePipeline.C_EXTENSION_ID };
        Assert.True(runtime.TryActivateDefaultPipeline(asset));
        DisposablePipeline.rejectConfiguration = true;
        DisposablePipeline.pendingRetirements = 2;
        asset.pipelineState = new SerializedRenderExtensionState(Guid.NewGuid(), [1]);

        Assert.True(runtime.TryActivateDefaultPipeline(asset));

        Assert.Equal(3, DisposablePipeline.retirementAttempts);
        Assert.Equal(1, DisposablePipeline.disposedCount);
        DisposablePipeline.rejectConfiguration = false;
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ReloadRetirementDrainsPendingBeforeFinishingTheTransaction(bool commit)
    {
        using var runtime = new RenderRuntime(m_types, TestDeviceProxy.Create(out _), new TestDiagnosticSink());
        Assert.True(runtime.TryActivateDefaultPipeline(new RenderPipelineAsset { pipelineTypeId = DisposablePipeline.C_EXTENSION_ID }));
        IRenderRuntimeReloadTransaction transaction = runtime.BeginExtensionReload();
        transaction.Prepare();
        transaction.Activate();
        DisposablePipeline.pendingRetirements = 2;

        if (commit)
        {
            transaction.Complete();
        }
        else
        {
            transaction.Rollback();
        }

        Assert.Equal(3, DisposablePipeline.retirementAttempts);
        Assert.Equal(1, DisposablePipeline.disposedCount);
        runtime.BeginExtensionReload().Rollback();
    }

    [Fact]
    public void TargetRetirementRetainsProgressAndOrdinaryErrorsAcrossPendingRetries()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        var targets = new RenderTargetStore(device);
        device.BeginFrame();
        var descriptor = new RenderTextureDescriptor(
            16,
            16,
            RenderTextureFormat.RGBA8,
            RenderTextureUsage.ColorAttachment
        );
        var graph = new RenderGraphBuilder(1, device.capabilities);
        targets.Import(graph, new RenderTexture("First", descriptor));
        targets.Import(graph, new RenderTexture("Second", descriptor));
        targets.Import(graph, new RenderTexture("Third", descriptor));
        proxy.failFirstTextureRetirement = true;
        proxy.pendingTextureRetirement = proxy.createdTextures[1];

        Assert.Throws<RetirementPendingException>(targets.Dispose);
        Assert.Equal(2, proxy.textureRetirementAttempts);
        Assert.True(proxy.frameOpen);
        Assert.Throws<ObjectDisposedException>(() => targets.Import(graph, new RenderTexture("Late", descriptor)));
        Assert.Throws<AggregateException>(targets.Dispose);
        Assert.Equal(4, proxy.textureRetirementAttempts);
        Assert.Equal(proxy.createdTextures.Skip(1), proxy.destroyedTextures);
        targets.Dispose();
        Assert.Equal(4, proxy.textureRetirementAttempts);
        device.EndFrame();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TerminalPipelineRetirementBlocksAdmissionAndPreservesLowerOwners(bool duringFrame)
    {
        var modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(EmptyRenderingKernelTests).Assembly)
        });
        var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        var runtime = new RenderRuntime(types, TestDeviceProxy.Create(out TestDeviceProxy device), new TestDiagnosticSink());
        var asset = new RenderPipelineAsset { pipelineTypeId = DisposablePipeline.C_EXTENSION_ID };
        Assert.True(runtime.TryActivateDefaultPipeline(asset));
        DisposablePipeline.retirementExpired = true;
        try
        {
            if (duringFrame)
            {
                asset.pipelineState = new SerializedRenderExtensionState(Guid.NewGuid(), [1]);
                DisposablePipeline.rejectConfiguration = true;
                runtime.Submit(CreateRequest("Faulting candidate", asset));
                BeginRenderFrame(runtime, 0f);
                Assert.Throws<RetirementTimeoutException>(() => runtime.AfterRender(default));
                Assert.True(device.frameOpen);
            }
            else
            {
                Assert.Throws<RetirementTimeoutException>(runtime.Dispose);
            }
            Assert.Throws<InvalidOperationException>(() => runtime.Submit(CreateRequest("Late", new RenderPipelineAsset())));
            Assert.Throws<InvalidOperationException>(() => runtime.RegisterContributor(new SideEffectContributor()));
            Assert.Throws<InvalidOperationException>(() => runtime.BeginExtensionReload());
            Assert.ThrowsAny<Exception>(() => types.Rebuild());
            Assert.Equal(1, DisposablePipeline.retirementAttempts);
            Assert.Equal(0, TestRequestProvider.retirementAttempts);
            Assert.Equal(0, device.endFrameCount);

            DisposablePipeline.retirementExpired = false;
            Assert.ThrowsAny<RetirementPendingException>(runtime.Dispose);
            Assert.ThrowsAny<RetirementPendingException>(types.Dispose);
            Assert.ThrowsAny<RetirementPendingException>(modules.Dispose);
            Assert.Equal(1, DisposablePipeline.retirementAttempts);
            Assert.Equal(0, device.endFrameCount);
        }
        finally
        {
            DisposablePipeline.retirementExpired = false;
            DisposablePipeline.rejectConfiguration = false;
        }
    }

    [Fact]
    public void RuntimeKeepsItsMaintenanceFrameOpenUntilTargetsFinishRetiring()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        var target = new RenderTexture(
            "Pending Target",
            new RenderTextureDescriptor(16, 16, RenderTextureFormat.RGBA8, RenderTextureUsage.ColorAttachment)
        );
        runtime.Submit(
            new RenderRequest(
                "Target",
                RenderTarget.FromTexture(target),
                new RenderViewport(0, 0, 16, 16),
                new RenderPipelineAsset { pipelineTypeId = SideEffectPipeline.C_EXTENSION_ID }
            )
        );
        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        proxy.pendingTextureRetirement = Assert.Single(proxy.createdTextures);

        runtime.Dispose();

        Assert.Equal(2, proxy.textureRetirementAttempts);
        Assert.Single(proxy.destroyedTextures);
        Assert.False(proxy.frameOpen);
        Assert.Equal(2, proxy.endFrameCount);
    }

    [Fact]
    public void RemovingAndRestoringPluginRenderingCommitsTheSameUnavailableStateAsColdStart()
    {
        const string C_EXTENSION_ID = "tests.runtime.reloadable-plugin";
        DotNetModuleSource plugin = CreateRenderingPluginRequest();
        AssemblyModuleHandle activeModule = m_modules.Load(plugin);
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        var diagnostics = new TestDiagnosticSink();
        using var runtime = new RenderRuntime(m_types, device, diagnostics);
        var asset = new RenderPipelineAsset { pipelineTypeId = C_EXTENSION_ID };
        var featureAsset = new RenderPipelineAsset
        {
            pipelineTypeId = SideEffectPipeline.C_EXTENSION_ID,
            features = [new RenderFeatureConfiguration("tests.runtime.reloadable-plugin-feature")]
        };

        runtime.Submit(CreateRequest("Before Removal", asset));
        runtime.Submit(CreateRequest("Feature Before Removal", featureAsset));
        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Equal(1, proxy.executeCount);
        proxy.ReleaseRecordedGraph();

        QueueCollectiblePayload(runtime, m_types, asset);
        (IAssemblyUnloadProbe removalMonitor, IRenderRuntimeReloadTransaction renderingRemoval) = RemoveRenderingPlugin(
            runtime,
            m_modules,
            plugin
        );

        ForceCollection();
        Assert.True(removalMonitor.isCompleted);
        GC.KeepAlive(renderingRemoval);

        runtime.Submit(CreateRequest("While Unavailable", asset));
        runtime.Submit(CreateRequest("Feature While Unavailable", featureAsset));
        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Equal(1, proxy.executeCount);
        Assert.DoesNotContain(
            diagnostics.items,
            static diagnostic => diagnostic.code == "RENDER_EXTENSION_RELOAD_REJECTED"
        );

        IRenderRuntimeReloadTransaction renderingRecovery = runtime.BeginExtensionReload();
        using (AssemblyReloadSession recovery = m_modules.BeginReload([plugin]))
        {
            activeModule = recovery.context.module;
            recovery.Activate();
            renderingRecovery.Prepare();
            renderingRecovery.Activate();
            _ = recovery.Complete();
            renderingRecovery.Complete();
        }

        runtime.Submit(CreateRequest("After Recovery", asset));
        runtime.Submit(CreateRequest("Feature After Recovery", featureAsset));
        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Equal(2, proxy.executeCount);

        proxy.ReleaseRecordedGraph();
        runtime.Detach();
        IAssemblyUnloadProbe recoveryMonitor = m_modules.Unload(activeModule);
        ForceCollection();
        Assert.True(recoveryMonitor.isCompleted);
    }

    [Fact]
    public void RemovingUnusedRenderingPluginReleasesItsAssemblyContext()
    {
        DotNetModuleSource plugin = CreateRenderingPluginRequest();
        _ = m_modules.Load(plugin);
        IAssemblyUnloadProbe monitor = RemoveUnusedPlugin(m_modules, plugin);

        ForceCollection();

        Assert.True(monitor.isCompleted);
    }

    [Fact]
    public void ReplacedAndReleasedTargetsRemainValidAcrossPreparedPresentationFrames()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        var registry = new RenderTargetStore(device);
        var target = new RenderTexture(
            "Deferred Target",
            new RenderTextureDescriptor(
                32,
                32,
                RenderTextureFormat.RGBA8,
                RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled
            )
        );

        device.BeginFrame();
        registry.PrepareFrame();
        _ = registry.Import(new RenderGraphBuilder(1, device.capabilities), target);
        _ = device.EndFrame();
        PersistentTextureHandle first = Assert.Single(proxy.createdTextures);

        target.Resize(
            new RenderTextureDescriptor(
                64,
                64,
                RenderTextureFormat.RGBA8,
                RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled
            )
        );
        device.BeginFrame();
        registry.PrepareFrame();
        _ = registry.Import(new RenderGraphBuilder(2, device.capabilities), target);
        _ = device.EndFrame();
        PersistentTextureHandle second = proxy.createdTextures[1];
        Assert.Empty(proxy.destroyedTextures);

        device.BeginFrame();
        registry.PrepareFrame();
        _ = device.EndFrame();
        Assert.Empty(proxy.destroyedTextures);

        device.BeginFrame();
        registry.PrepareFrame();
        _ = device.EndFrame();
        Assert.Equal(new[] { first }, proxy.destroyedTextures);

        registry.Release(target);
        for (int frame = 0; frame < 2; frame++)
        {
            device.BeginFrame();
            registry.PrepareFrame();
            _ = device.EndFrame();
        }
        Assert.Equal(new[] { first }, proxy.destroyedTextures);

        device.BeginFrame();
        registry.PrepareFrame();
        registry.Dispose();
        _ = device.EndFrame();
        Assert.Equal(new[] { first, second }, proxy.destroyedTextures);
    }

    [Fact]
    public void DetachUsesADeviceSafetyFrameToReleaseResidentTargets()
    {
        IRenderDevice device = TestDeviceProxy.Create(out TestDeviceProxy proxy);
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        var asset = new RenderPipelineAsset { pipelineTypeId = SideEffectPipeline.C_EXTENSION_ID };
        var target = new RenderTexture(
            "Detach Target",
            new RenderTextureDescriptor(16, 16, RenderTextureFormat.RGBA8, RenderTextureUsage.ColorAttachment)
        );
        runtime.Submit(
            new RenderRequest("Detach", RenderTarget.FromTexture(target), new RenderViewport(0, 0, 16, 16), asset)
        );

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        PersistentTextureHandle resident = Assert.Single(proxy.createdTextures);

        runtime.Detach();

        Assert.Equal(new[] { resident }, proxy.destroyedTextures);
        Assert.False(proxy.frameOpen);
        Assert.Equal(2, proxy.endFrameCount);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static IAssemblyUnloadProbe RemoveUnusedPlugin(
        ModuleHost modules,
        DotNetModuleSource plugin
    ) {
        using AssemblyReloadSession removal = modules.BeginReload([], [plugin.moduleName]);
        removal.Activate();
        return removal.Complete();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void QueueCollectiblePayload(
        RenderRuntime runtime,
        TypeCatalog types,
        RenderPipelineAsset pipeline
    ) {
        Type payloadType = types.current.types
            .Select(type => type.Resolve(types))
            .Single(type => string.Equals(
                type.FullName,
                "Inno.Rendering.Runtime.Reload.TestModule.ReloadableFramePayload",
                StringComparison.Ordinal
            ));
        object payload = Activator.CreateInstance(payloadType)
                         ?? throw new InvalidOperationException("The collectible frame payload could not be created.");
        var data = new RenderFrameData();
        data.Set(new RenderDataChannelID("tests.runtime.collectible-payload"), payload);
        runtime.Submit(
            new RenderRequest(
                "Collectible Pending Request",
                RenderTarget.backbuffer,
                new RenderViewport(0, 0, 64, 64),
                pipeline,
                data
            )
        );
    }
}
