using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.References;
using Inno.Rendering.Assets;
using Inno.Runtime.Contracts;
using Inno.Testing.Source;

namespace Inno.Rendering.Runtime.Tests;

public abstract class RenderRuntimeFixture : IDisposable
{
    private readonly string m_cacheDirectory = Path.Combine(
        Path.GetTempPath(),
        "InnoRenderRuntimeTests",
        Guid.NewGuid().ToString("N")
    );

    protected readonly IdentityAllocator m_identities;
    private readonly IDisposable m_identityScope;
    protected readonly ModuleHost m_modules;
    protected readonly TypeCatalog m_types;
    protected readonly SerializationRegistry m_serialization;

    protected RenderRuntimeFixture()
    {
        _ = typeof(TextureAsset);
        m_identities = new IdentityAllocator();
        m_identityScope = m_identities.EnterScope();
        m_modules = new ModuleHost(
            new ModuleHostOptions { catalogSource = new DotNetAssemblyCatalogSource(typeof(EmptyRenderingKernelTests).Assembly)}
        );
        m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
        m_serialization = new SerializationRegistry(m_types, new ReflectionSerializationMetadataSource());
        ResourceProbePipeline.action = null;
        DisposablePipeline.Reset();
        PendingFeature.Reset();
        TestRequestProvider.Reset();
        FirstTestRenderModel.Reset();
        SecondTestRenderModel.Reset();
        CompositionLayerPipeline.viewports.Clear();
        UploadPipeline.Reset();
        TexturePrewarmPipeline.texture = null;
        PendingShaderPipeline.Reset();
        ReadbackPipeline.Reset();
        PresentationPipeline.Reset();
    }

    public void Dispose()
    {
        ResourceProbePipeline.action = null;
        m_serialization.Dispose();
        m_types.Dispose();
        m_modules.Dispose();
        m_identityScope.Dispose();
        if (Directory.Exists(m_cacheDirectory))
        {
            Directory.Delete(m_cacheDirectory, recursive: true);
        }
    }

    protected DotNetModuleSource CreateRenderingPluginRequest()
        => new()
    {
        moduleName = "RenderingRuntimeReloadTests",
        artifactRootDirectory = Path.Combine(m_cacheDirectory, "Modules"),
        mainAssemblyPath = Path.Combine(
            AppContext.BaseDirectory,
            "Modules",
            "RenderingReload",
            "Inno.Rendering.Runtime.Reload.TestModule.dll"
        ),
        domain = AssemblyDomain.InnoPlugin,
        scope = AssemblyScope.Runtime
    };

    protected static void RunResourceFrame(RenderRuntime runtime)
    {
        runtime.Submit(
            CreateRequest("Resource ownership", new RenderPipelineAsset { pipelineTypeId = ResourceProbePipeline.C_EXTENSION_ID })
        );
        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
    }

    protected static RenderRequest CreateRequest(
        string name,
        RenderPipelineAsset pipeline,
        int priority = 0
    ) => new(name, RenderTarget.backbuffer, new RenderViewport(0, 0, 64, 64), pipeline, priority: priority);

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static (
        IAssemblyUnloadProbe monitor,
        IRenderRuntimeReloadTransaction transaction) RemoveRenderingPlugin(
            RenderRuntime runtime,
            ModuleHost modules,
            DotNetModuleSource plugin
        ) {
        IRenderRuntimeReloadTransaction rendering = runtime.BeginExtensionReload();
        using AssemblyReloadSession removal = modules.BeginReload([], [plugin.moduleName]);
        removal.Activate();
        rendering.Prepare();
        rendering.Activate();
        IAssemblyUnloadProbe monitor = removal.Complete();
        rendering.Complete();
        return (monitor, rendering);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static void ForceCollection()
    {
        for (int attempt = 0; attempt < 8; attempt++)
        {
            GC.Collect(2, GCCollectionMode.Forced, blocking: true, compacting: true);
            GC.WaitForPendingFinalizers();
        }
    }

    protected static void BeginRenderFrame(
        RenderRuntime runtime,
        float deltaTime
    ) {
        if (!runtime.isStarted)
        {
            runtime.Attach();
        }
        var frame = new RuntimeFrame(0, deltaTime, deltaTime, 0, 0, 1, false);
        runtime.BeginFrame(frame);
        runtime.BeforeRender(frame);
    }

    [RenderPipelineExtension(C_EXTENSION_ID)]
    protected sealed class ResourceProbePipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.resource-owner";
        internal static Action<IRenderResourceService>? action;

        public override void Build(RenderPipelineContext context)
        {
            action?.Invoke(context.resourceService);
            context.graph.AddRasterPass("Resources", new RenderPhaseID("tests.resources"), 0, static (
                _,
                _
            ) => {}).HasSideEffect();
        }
    }

    [RenderModelExtension(C_EXTENSION_ID)]
    protected sealed class FirstTestRenderModel : IRenderModel
    {
        internal const string C_EXTENSION_ID = "tests.runtime.model.first";
        internal static bool enabled;
        internal static int buildCount;

        public bool CanRender(RenderOutputSession session) => enabled;

        public RenderModelOutput Build(RenderOutputSession session)
        {
            buildCount++;
            return new RenderModelOutput(
                "First Model",
                new RenderPipelineAsset { pipelineTypeId = CompositionLayerPipeline.C_EXTENSION_ID },
                new RenderFrameData()
            );
        }

        public void Dispose() {}

        internal static void Reset()
        {
            enabled = false;
            buildCount = 0;
        }
    }

    [RenderModelExtension(C_EXTENSION_ID)]
    protected sealed class SecondTestRenderModel : IRenderModel
    {
        internal const string C_EXTENSION_ID = "tests.runtime.model.second";
        internal static bool enabled;
        internal static int buildCount;

        public bool CanRender(RenderOutputSession session) => enabled;

        public RenderModelOutput Build(RenderOutputSession session)
        {
            buildCount++;
            return new RenderModelOutput(
                "Second Model",
                new RenderPipelineAsset { pipelineTypeId = CompositionLayerPipeline.C_EXTENSION_ID },
                new RenderFrameData()
            );
        }

        public void Dispose() {}

        internal static void Reset()
        {
            enabled = false;
            buildCount = 0;
        }
    }

    [RenderPipelineExtension(C_EXTENSION_ID)]
    protected sealed class DisposablePipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.disposable";

        public DisposablePipeline()
        {
            createdCount++;
        }

        public override void Build(RenderPipelineContext context) => _ = context;

        internal static int createdCount { get; private set; }
        internal static int disposedCount { get; private set; }
        internal static bool rejectConfiguration { get; set; }
        internal static int pendingRetirements { get; set; }
        internal static int retirementAttempts { get; private set; }
        internal static bool retirementExpired { get; set; }

        protected override void OnConfigure(
            SerializedRenderExtensionState state,
            RenderExtensionStateContext settings
        ) {
            _ = state;
            if (rejectConfiguration)
            {
                throw new InvalidOperationException("Rejected pipeline configuration candidate.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _ = disposing;
            retirementAttempts++;
            if (retirementExpired)
            {
                throw new RetirementTimeoutException("The pipeline-owned device work exceeded its deadline.");
            }
            if (pendingRetirements > 0)
            {
                pendingRetirements--;
                throw new RetirementPendingException("Pipeline work remains active.");
            }
            disposedCount++;
        }

        internal static void Reset()
        {
            createdCount = 0;
            disposedCount = 0;
            rejectConfiguration = false;
            pendingRetirements = 0;
            retirementAttempts = 0;
            retirementExpired = false;
        }
    }

    [RenderFeatureExtension(C_EXTENSION_ID)]
    protected sealed class PendingFeature : RenderPipelineFeature, IDisposable
    {
        internal const string C_EXTENSION_ID = "tests.runtime.pending-feature";

        public override void AddRenderPasses(RenderFeatureContext context) {}

        public void Dispose()
        {
            retirementAttempts++;
            Assert.Equal(0, DisposablePipeline.disposedCount);
            if (pendingRetirements > 0)
            {
                pendingRetirements--;
                throw new RetirementPendingException("Feature work remains active.");
            }
        }

        internal static int pendingRetirements { get; set; }
        internal static int retirementAttempts { get; private set; }

        internal static void Reset()
        {
            retirementAttempts = 0;
            pendingRetirements = 0;
        }
    }

    [RenderPipelineExtension(C_EXTENSION_ID)]
    protected sealed class CompositionLayerPipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.composition-layer";

        public override void Build(RenderPipelineContext context)
        {
            viewports.Add(context.request.viewport);
            RasterPassBuilder pass = context.graph.AddRasterPass(
                "Scene Color",
                new RenderPhaseID("tests.runtime.composition-layer"),
                0,
                static (
                    _,
                    _
                ) => {}
            );
            if (context.outputTexture.isValid)
            {
                pass.UseColorAttachment(
                    context.outputTexture,
                    0,
                    RenderLoadAction.Clear,
                    RenderStoreAction.Store,
                    new RenderClearColor(0f, 0f, 0f, 0f)
                );
            }
            else
            {
                pass.ClearPresentationTarget(default).HasSideEffect();
            }
        }

        internal static List<RenderViewport> viewports { get; } = [];
    }

    [RenderPipelineExtension(C_EXTENSION_ID)]
    protected sealed class SideEffectPipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.side-effect";

        public override void Build(RenderPipelineContext context)
            => context.graph
                .AddRasterPass(
                    "Visible",
                    new RenderPhaseID("tests.runtime.visible"),
                    0,
                    static (
                        _,
                        _
                    ) => {}
                )
                .HasSideEffect();
    }

    [RenderPipelineExtension(C_EXTENSION_ID)]
    protected sealed class PresentationPipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.presentation";

        public override void Build(RenderPipelineContext context)
        {
            observations.Add(new PresentationObservation(context.request.name, context.preservePresentationTarget));
            context.graph
                .AddRasterPass(
                    "Presentation",
                    new RenderPhaseID("tests.runtime.presentation"),
                    0,
                    static (
                        _,
                        _
                    ) => {}
                )
                .HasSideEffect();
        }

        internal static List<PresentationObservation> observations { get; } = [];

        internal static void Reset() => observations.Clear();
    }

    protected readonly record struct PresentationObservation(
        string requestName,
        bool preserveTarget
    );

    [RenderPipelineExtension(C_EXTENSION_ID)]
    protected sealed class UploadPipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.upload";

        public override void Build(RenderPipelineContext context)
        {
            var layout = new RenderVertexLayout(
                [
                    new RenderVertexAttribute(RenderVertexSemantic.Position, RenderVertexFormat.Float3)
                ]
            );
            var descriptor = new RenderBufferUploadDescriptor(layout.stride, RenderBufferUsage.Vertex, layout);
            firstSlice = context.uploads.UploadBuffer(descriptor, new byte[layout.stride * 3], "Vertices");
            if (!singleUpload)
            {
                secondSlice = context.uploads.UploadBuffer(descriptor, new byte[layout.stride * 3], "Vertices");
            }
        }

        internal static RenderBufferSlice firstSlice { get; private set; }
        internal static RenderBufferSlice secondSlice { get; private set; }
        internal static bool singleUpload { get; set; }

        internal static void Reset()
        {
            firstSlice = default;
            secondSlice = default;
            singleUpload = false;
        }
    }

    [RenderPipelineExtension(C_EXTENSION_ID)]
    protected sealed class TexturePrewarmPipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.texture-prewarm";

        public override void Build(RenderPipelineContext context)
        {
            context.resourceService.PrewarmTexture(
                texture ?? throw new InvalidOperationException("A test texture is required.")
            );
            context.graph
                .AddRasterPass(
                    "Submit While Prewarming",
                    new RenderPhaseID("tests.runtime.prewarm"),
                    0,
                    static (
                        _,
                        _
                    ) => {}
                )
                .HasSideEffect();
        }

        internal static TextureAsset? texture { get; set; }
    }

    [RenderPipelineExtension(C_EXTENSION_ID)]
    protected sealed class PendingShaderPipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.pending-shader";

        public override void Build(RenderPipelineContext context)
        {
            resolved = context.resourceService.TryResolveGraphicsMaterial(
                material ?? throw new InvalidOperationException("A test material is required."),
                contract,
                role,
                null,
                null,
                out _
            );
            context.graph
                .AddRasterPass(
                    "Submit While Compiling",
                    new RenderPhaseID("tests.runtime.pending-shader"),
                    0,
                    static (
                        _,
                        _
                    ) => {}
                )
                .HasSideEffect();
        }

        internal static MaterialAsset? material { get; set; }
        internal static ShaderContractID contract { get; set; }
        internal static ShaderPassRoleID role { get; set; }
        internal static bool resolved { get; private set; }

        internal static void Reset()
        {
            material = null;
            contract = default;
            role = default;
            resolved = false;
        }
    }

    [RenderPipelineExtension(C_EXTENSION_ID)]
    protected sealed class ReadbackPipeline : RenderPipeline
    {
        internal const string C_EXTENSION_ID = "tests.runtime.readback";

        public override void Build(RenderPipelineContext context)
        {
            byte[] update = Enumerable.Range(0, 16).Select(static value => (byte)value).ToArray();
            context.resourceService.UpdateTexture(texture, new RenderTextureRegion(0, 1, 1, 0, 2, 2), update);
            readback ??= context.resourceService
                .ReadTextureAsync(texture, cancellationToken: cancellationToken)
                .AsTask();
            context.graph
                .AddRasterPass(
                    "Readback Submission",
                    new RenderPhaseID("tests.runtime.readback"),
                    0,
                    static (
                        _,
                        _
                    ) => {}
                )
                .HasSideEffect();
        }

        internal static PersistentTextureHandle texture { get; set; }
        internal static CancellationToken cancellationToken { get; set; }
        internal static Task<RenderTextureReadbackResult>? readback { get; private set; }

        internal static void Reset()
        {
            texture = default;
            cancellationToken = default;
            readback = null;
        }
    }

    [RenderRequestProviderExtension(C_EXTENSION_ID)]
    protected sealed class TestRequestProvider : RenderRequestProvider
    {
        internal const string C_EXTENSION_ID = "tests.runtime.request-provider";

        public override void Submit(RenderRequestProviderContext context)
        {
            if (!enabled)
            {
                return;
            }
            submitCount++;
            lastContent = context.content;
            lastPresentationViewport = context.primaryPresentationViewport;
            lastInput = context.input;
            if (pipeline is null)
            {
                return;
            }
            context.requests.Submit(CreateRequest("Provider Request", pipeline));
        }

        internal static bool enabled { get; set; }
        internal static RenderPipelineAsset? pipeline { get; set; }
        internal static int submitCount { get; private set; }
        internal static int pendingRetirements { get; set; }
        internal static int retirementAttempts { get; private set; }
        internal static ContentReadScope? lastContent { get; private set; }
        internal static RenderViewport? lastPresentationViewport { get; private set; }
        internal static RenderOutputInput lastInput { get; private set; } = RenderOutputInput.empty;

        protected override void Dispose(bool disposing)
        {
            retirementAttempts++;
            if (pendingRetirements > 0)
            {
                pendingRetirements--;
                throw new RetirementPendingException("Provider work remains active.");
            }
        }

        internal static void Reset()
        {
            enabled = false;
            pipeline = null;
            submitCount = 0;
            pendingRetirements = 0;
            retirementAttempts = 0;
            lastContent = null;
            lastPresentationViewport = default;
            lastInput = RenderOutputInput.empty;
        }
    }

    protected sealed class SideEffectContributor : IRenderFrameGraphContributor
    {
        public void PrepareFrame(ulong frameIndex) => _ = frameIndex;

        public void AddRenderPasses(
            RenderGraphBuilder graph,
            ulong frameIndex
        ) {
            _ = frameIndex;
            graph
                .AddRasterPass(
                    "Overlay",
                    new RenderPhaseID("tests.runtime.overlay"),
                    0,
                    static (
                        _,
                        _
                    ) => {}
                )
                .HasSideEffect();
        }
    }

    protected sealed class TestDiagnosticSink : IDiagnosticReporter
    {
        public void Publish(Diagnostic diagnostic) => items.Add(diagnostic);

        public void Resolve(
            string code,
            string? semanticID = null,
            Guid? objectID = null
        ) => items.RemoveAll(diagnostic => (
            string.Equals(diagnostic.code, code, StringComparison.Ordinal)
            && string.Equals(diagnostic.semanticID, semanticID, StringComparison.Ordinal) && diagnostic.objectID == objectID
        ));

        public void Replace(IEnumerable<Diagnostic> diagnostics)
        {
            items.Clear();
            items.AddRange(diagnostics);
        }

        internal List<Diagnostic> items { get; } = [];
    }

    protected sealed class TestDeviceProxy : RenderDevice, IRenderDevice
    {
        private static readonly GraphicsCapabilities CAPABILITIES = new(
            GraphicsApi.noop,
            GraphicsCapability.None,
            new GraphicsLimits(64, 4, 4096, 8),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            [],
            [],
            originBottomLeft: false,
            homogeneousDepth: false
        );

        private int m_bufferCreationCount;

        public RenderDeviceFrameCounters frameCounters { get; internal set; }
        public RenderDeviceAllocationCounters? allocationCounters { get; internal set; }

        public GraphicsCapabilities capabilities { get; internal set; } = CAPABILITIES;
        public uint generation => 1;
        public RenderPresentationSize? presentationSize { get; set; } = new(1, 1);

        public RenderPresentationSize? primaryPresentationSize
        {
            get => presentationSize;
            private set => presentationSize = value;
        }

        public bool primaryPresentationEncodesSrgb { get; set; } = true;

        public void SetVerticalSync(bool enabled) => verticalSync = enabled;

        public void BeginFrame()
        {
            Assert.False(frameOpen);
            frameOpen = true;
        }

        public void Execute(
            CompiledRenderGraph graph,
            ulong frameIndex
        ) {
            Assert.True(frameOpen);
            _ = frameIndex;
            executeCount++;
            lastGraph = graph;
        }

        public uint EndFrame()
        {
            Assert.True(frameOpen);
            frameOpen = false;
            endFrameCount++;
            return checked((uint)endFrameCount);
        }

        public void SetPrimaryPresentationSize(RenderPresentationSize? size) => primaryPresentationSize = size;

        public PersistentTextureHandle CreateTexture(
            RenderTextureDescriptor descriptor,
            string name
        ) {
            Assert.True(frameOpen);
            _ = descriptor;
            _ = name;
            PersistentTextureHandle texture = CreatePersistentTextureHandle(
                checked((ulong)createdTextures.Count + 1),
                generation
            );
            createdTextures.Add(texture);
            return texture;
        }

        public void UpdateTexture(
            PersistentTextureHandle texture,
            ReadOnlySpan<byte> data,
            int mipLevel = 0,
            int arrayLayer = 0
        ) {
            _ = texture;
            _ = data;
            _ = mipLevel;
            _ = arrayLayer;
        }

        public void UpdateTextureRegion(
            PersistentTextureHandle texture,
            RenderTextureRegion region,
            ReadOnlySpan<byte> data
        ) {
            _ = texture;
            _ = region;
            _ = data;
        }

        public RenderTextureReadbackHandle BeginTextureReadback(
            PersistentTextureHandle texture,
            int mipLevel = 0
        ) {
            _ = texture;
            _ = mipLevel;
            return default;
        }

        public bool TryGetTextureReadback(
            RenderTextureReadbackHandle readback,
            out RenderTextureReadbackResult? result
        ) {
            _ = readback;
            result = null;
            return false;
        }

        public void CancelTextureReadback(RenderTextureReadbackHandle readback) => _ = readback;

        public void DestroyTexture(PersistentTextureHandle texture)
        {
            Assert.True(frameOpen);
            textureRetirementAttempts++;
            if (failFirstTextureRetirement)
            {
                failFirstTextureRetirement = false;
                throw new InvalidOperationException("Texture retirement failed.");
            }
            if (texture == pendingTextureRetirement)
            {
                pendingTextureRetirement = default;
                throw new RetirementPendingException("Texture still has pending work.");
            }
            destroyedTextures.Add(texture);
        }

        public PersistentBufferHandle CreateBuffer(
            PersistentBufferDescriptor descriptor,
            ReadOnlySpan<byte> initialData,
            string name
        ) {
            m_bufferCreationCount++;
            if (m_bufferCreationCount == failBufferCreationAt)
            {
                throw new InvalidOperationException("Native allocation rejected.");
            }
            PersistentBufferHandle handle = CreatePersistentBufferHandle((ulong)m_bufferCreationCount, generation);
            createdBuffers.Add(handle);
            return handle;
        }

        public void UpdateBuffer(
            PersistentBufferHandle buffer,
            ReadOnlySpan<byte> data,
            int startElement = 0
        ) {
            _ = buffer;
            _ = data;
            _ = startElement;
        }

        public void DestroyBuffer(PersistentBufferHandle buffer)
        {
            Assert.DoesNotContain(buffer, destroyedBuffers);
            destroyedBuffers.Add(buffer);
        }

        public GraphicsPipelineHandle CreateGraphicsPipeline(
            GraphicsPipelineDescriptor descriptor,
            string name
        ) {
            _ = descriptor;
            _ = name;
            return default;
        }

        public void DestroyGraphicsPipeline(GraphicsPipelineHandle pipeline) => _ = pipeline;

        public ComputePipelineHandle CreateComputePipeline(
            ComputePipelineDescriptor descriptor,
            string name
        ) {
            _ = descriptor;
            _ = name;
            return default;
        }

        public void DestroyComputePipeline(ComputePipelineHandle pipeline) => _ = pipeline;

        public void Dispose() {}

        internal bool verticalSync { get; private set; }

        internal List<PersistentBufferHandle> createdBuffers { get; } = [];
        internal List<PersistentBufferHandle> destroyedBuffers { get; } = [];
        internal int failBufferCreationAt { get; set; }

        internal List<PersistentTextureHandle> createdTextures { get; } = [];
        internal List<PersistentTextureHandle> destroyedTextures { get; } = [];
        internal bool frameOpen { get; private set; }
        internal int endFrameCount { get; private set; }
        internal int executeCount { get; private set; }
        internal int textureRetirementAttempts { get; private set; }
        internal bool failFirstTextureRetirement { get; set; }
        internal PersistentTextureHandle pendingTextureRetirement { get; set; }
        internal CompiledRenderGraph? lastGraph { get; private set; }

        internal void ReleaseRecordedGraph() => lastGraph = null;

        internal static IRenderDevice Create(out TestDeviceProxy proxy)
        {
            proxy = new TestDeviceProxy();
            return proxy;
        }
    }

    protected sealed class RecordingRenderDevice : RenderDevice, IRenderDevice
    {
        private readonly RenderTextureDescriptor m_readbackDescriptor = new(
            4,
            4,
            RenderTextureFormat.RGBA8,
            RenderTextureUsage.Readback
        );

        private readonly RenderTextureReadbackHandle m_readbackHandle;

        internal RecordingRenderDevice(bool supportsReadback = false)
        {
            this.capabilities = new GraphicsCapabilities(
                GraphicsApi.noop,
                supportsReadback ? GraphicsCapability.TextureReadback : GraphicsCapability.None,
                new GraphicsLimits(64, 4, 4096, 8),
                Enum.GetValues<RenderTextureFormat>(),
                Enum.GetValues<RenderTextureFormat>(),
                [],
                [],
                originBottomLeft: false,
                homogeneousDepth: false
            );
            this.textureHandle = CreatePersistentTextureHandle(1UL, generation);
            m_readbackHandle = CreateRenderTextureReadbackHandle(1UL, generation);
        }

        public GraphicsCapabilities capabilities { get; }

        public uint generation => 1;
        public RenderPresentationSize? primaryPresentationSize { get; private set; } = new(1, 1);
        public bool primaryPresentationEncodesSrgb => true;

        public void SetVerticalSync(bool enabled) => verticalSync = enabled;

        public void BeginFrame() {}

        public void Execute(
            CompiledRenderGraph graph,
            ulong frameIndex
        ) {
            _ = graph;
            _ = frameIndex;
            executeCount++;
        }

        public uint EndFrame()
        {
            endFrameCount++;
            return checked((uint)endFrameCount);
        }

        public void SetPrimaryPresentationSize(RenderPresentationSize? size) => primaryPresentationSize = size;

        public PersistentTextureHandle CreateTexture(
            RenderTextureDescriptor descriptor,
            string name
        ) {
            _ = descriptor;
            _ = name;
            return default;
        }

        public void UpdateTexture(
            PersistentTextureHandle texture,
            ReadOnlySpan<byte> data,
            int mipLevel = 0,
            int arrayLayer = 0
        ) {
            _ = texture;
            _ = data;
            _ = mipLevel;
            _ = arrayLayer;
        }

        public void UpdateTextureRegion(
            PersistentTextureHandle texture,
            RenderTextureRegion region,
            ReadOnlySpan<byte> data
        ) {
            Assert.Equal(textureHandle, texture);
            updatedRegion = region;
            updatedBytes = data.ToArray();
        }

        public RenderTextureReadbackHandle BeginTextureReadback(
            PersistentTextureHandle texture,
            int mipLevel = 0
        ) {
            Assert.Equal(textureHandle, texture);
            _ = mipLevel;
            return m_readbackHandle;
        }

        public bool TryGetTextureReadback(
            RenderTextureReadbackHandle readback,
            out RenderTextureReadbackResult? result
        ) {
            Assert.Equal(m_readbackHandle, readback);
            result = readbackReady
                     ? new RenderTextureReadbackResult(
                         m_readbackDescriptor,
                         0,
                         16,
                         Enumerable.Repeat((byte)37, 64).ToArray()
                     )
                     : null;
            return result is not null;
        }

        public void CancelTextureReadback(RenderTextureReadbackHandle readback)
        {
            Assert.Equal(m_readbackHandle, readback);
            cancelReadbackCount++;
            if (pendingReadbackRetirements > 0)
            {
                pendingReadbackRetirements--;
                Assert.False(ReadbackPipeline.readback!.IsCompleted);
                Assert.Equal(1, endFrameCount);
                throw new RetirementPendingException("Readback cancellation remains active.");
            }
        }

        public void DestroyTexture(PersistentTextureHandle texture) => _ = texture;

        public PersistentBufferHandle CreateBuffer(
            PersistentBufferDescriptor descriptor,
            ReadOnlySpan<byte> initialData,
            string name
        ) {
            _ = descriptor;
            _ = initialData;
            _ = name;
            createBufferCount++;
            return default;
        }

        public void UpdateBuffer(
            PersistentBufferHandle buffer,
            ReadOnlySpan<byte> data,
            int startElement = 0
        ) {
            _ = buffer;
            _ = data;
            bufferUpdateOffsets.Add(startElement);
        }

        public void DestroyBuffer(PersistentBufferHandle buffer)
        {
            bufferRetirementAttempts++;
            if (pendingBufferRetirements > 0)
            {
                pendingBufferRetirements--;
                Assert.Equal(0, cancelReadbackCount);
                Assert.False(ReadbackPipeline.readback!.IsCompleted);
                Assert.Equal(1, endFrameCount);
                throw new RetirementPendingException("Upload work remains active.");
            }
        }

        public GraphicsPipelineHandle CreateGraphicsPipeline(
            GraphicsPipelineDescriptor descriptor,
            string name
        ) {
            if (name == failProgramName) throw new InvalidOperationException("Injected program allocation failure.");
            GraphicsPipelineHandle handle = CreateGraphicsPipelineHandle(checked((ulong)createdPrograms.Count + 1), generation);
            createdPrograms.Add(handle);
            programDescriptors.Add(descriptor);
            return handle;
        }

        public void DestroyGraphicsPipeline(GraphicsPipelineHandle pipeline)
        {
            programRetirementAttempts++;
            if (pendingProgramRetirements > 0)
            {
                pendingProgramRetirements--;
                throw new RetirementPendingException("Injected unfinished program retirement.");
            }
            Assert.DoesNotContain(pipeline, destroyedPrograms);
            destroyedPrograms.Add(pipeline);
            if (failedProgramRetirements > 0)
            {
                failedProgramRetirements--;
                throw new InvalidOperationException("Injected completed program retirement failure.");
            }
        }

        public ComputePipelineHandle CreateComputePipeline(
            ComputePipelineDescriptor descriptor,
            string name
        ) {
            _ = descriptor;
            _ = name;
            return default;
        }

        public void DestroyComputePipeline(ComputePipelineHandle pipeline) => _ = pipeline;

        public void Dispose() {}

        internal List<GraphicsPipelineHandle> createdPrograms { get; } = [];
        internal List<GraphicsPipelineHandle> destroyedPrograms { get; } = [];
        internal List<GraphicsPipelineDescriptor> programDescriptors { get; } = [];
        internal string? failProgramName { get; set; }
        internal int pendingProgramRetirements { get; set; }
        internal int failedProgramRetirements { get; set; }
        internal int programRetirementAttempts { get; private set; }
        internal bool verticalSync { get; private set; }

        internal List<int> bufferUpdateOffsets { get; } = [];
        internal PersistentTextureHandle textureHandle { get; }
        internal RenderTextureRegion? updatedRegion { get; private set; }
        internal byte[] updatedBytes { get; private set; } = [];
        internal bool readbackReady { get; set; }
        internal int cancelReadbackCount { get; private set; }
        internal int pendingReadbackRetirements { get; set; }
        internal int pendingBufferRetirements { get; set; }
        internal int bufferRetirementAttempts { get; private set; }
        internal int createBufferCount { get; private set; }
        internal int endFrameCount { get; private set; }
        internal int executeCount { get; private set; }
    }

    protected sealed class DelayedTextureArtifactSource
    {
        private readonly TaskCompletionSource<byte[]> m_completion = new(
            TaskCreationOptions.RunContinuationsAsynchronously
        );

        internal ManualResetEventSlim started { get; } = new(initialState: false);
        internal bool isCompleted => m_completion.Task.IsCompleted;

        internal Task<byte[]> LoadAsync(CancellationToken cancellationToken = default)
        {
            started.Set();
            return m_completion.Task.WaitAsync(cancellationToken);
        }

        internal void Complete(byte[] data) => m_completion.TrySetResult(data);
    }

    protected sealed class DelayedArtifactProvider(DelayedTextureArtifactSource compiler) : IRenderTargetArtifactProvider
    {
        private Task<byte[]>? m_texture;

        public ShaderDefinition ReadShaderDefinition(RenderShaderArtifact artifact)
            => throw new InvalidOperationException("This pending provider never publishes a shader artifact.");

        public RenderTargetArtifactStatus GetShaderArtifact(
            ShaderAsset shader,
            RenderShaderVariant variant,
            GraphicsCapabilities capabilities,
            out RenderShaderArtifact? artifact
        ) {
            _ = shader;
            _ = variant;
            _ = capabilities;
            artifact = null;
            return RenderTargetArtifactStatus.Pending;
        }

        public RenderTargetArtifactStatus GetTextureArtifact(
            RenderTextureArtifactReference texture,
            out ReadOnlyMemory<byte> artifact
        ) {
            _ = texture;
            m_texture ??= compiler.LoadAsync();
            if (m_texture.IsCompletedSuccessfully)
            {
                artifact = m_texture.Result;
                return RenderTargetArtifactStatus.Ready;
            }
            artifact = ReadOnlyMemory<byte>.Empty;
            return RenderTargetArtifactStatus.Pending;
        }
    }
}
