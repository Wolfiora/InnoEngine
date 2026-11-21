using System;
using Xunit;
using Inno.Adapter.Platform;
using Inno.Platform;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

[Collection(BgfxDeviceCollection.C_NAME)]
[Trait("ProcessIsolation", "Required")]
public sealed class BgfxDeviceTests
{
    private static readonly RenderPhaseId FIRST = new("tests.first");
    private static readonly RenderPhaseId COMPUTE = new("tests.compute");
    private static readonly RenderPhaseId LAYERED = new("tests.layered");

    private readonly BgfxDevice m_device;

    public BgfxDeviceTests(BgfxDeviceFixture fixture)
    {
        m_device = fixture.device;
    }

    [Fact]
    public void NoopDevice_ExecutesOneGraphAndSubmitsOneFrame()
    {
        var builder = new RenderGraphBuilder(1, m_device.capabilities);
        builder
            .AddRasterPass("Noop Clear", FIRST, 0, static (
                _,
                _
            ) => {})
            .HasSideEffect();
        CompiledRenderGraph graph = builder.Compile().graph!;

        m_device.BeginFrame();
        m_device.Execute(graph, 1);
        uint submitted = m_device.EndFrame();

        Assert.Equal(GraphicsApi.Noop, m_device.capabilities.backend);
        Assert.Equal(submitted, m_device.backendFrame);
        Assert.Single(graph.passes);
    }

    [Fact]
    public void NoopDevice_ResetsPublicCommandCountersPerFrame()
    {
        m_device.BeginFrame();
        Assert.Equal(default, m_device.frameCounters);
        m_device.EndFrame();
        m_device.BeginFrame();
        Assert.Equal(default, m_device.frameCounters);
        m_device.EndFrame();
    }

    [Fact]
    public void CreateWindowSurface_RejectsUnsupportedPlatformHandlesBeforeNativeCreation()
    {
        m_device.BeginFrame();

        Assert.Throws<NotSupportedException>(() => m_device.CreateWindowSurface(
            new PlatformNativeHandles(new nint(1), handleKind: default),
            32,
            32,
            "Unsupported"
        ));

        m_device.EndFrame();
    }

    [Fact]
    public void NoopDevice_CreatesAndDefersPersistentTextureDestruction()
    {
        var descriptor = new RenderTextureDescriptor(16, 16, RenderTextureFormat.RGBA8, RenderTextureUsage.Sampled);

        m_device.BeginFrame();
        PersistentTextureHandle texture = m_device.CreateTexture(descriptor, "Persistent Test");
        m_device.DestroyTexture(texture);
        m_device.EndFrame();

        for (int index = 0; index < 4; index++)
        {
            m_device.BeginFrame();
            m_device.EndFrame();
        }

        Assert.True(texture.isValid);
    }

    [Fact]
    public void NoopDevice_UploadsCompletePersistentTextureSubresource()
    {
        var descriptor = new RenderTextureDescriptor(4, 4, RenderTextureFormat.RGBA8, RenderTextureUsage.Sampled);

        m_device.BeginFrame();
        PersistentTextureHandle texture = m_device.CreateTexture(descriptor, "Upload Test");
        m_device.UpdateTexture(texture, new byte[4 * 4 * 4]);
        m_device.DestroyTexture(texture);
        m_device.EndFrame();
    }

    [Fact]
    public void NoopDevice_CreatesAndUpdatesVolumeAndCubemapSubresources()
    {
        Assert.True(m_device.capabilities.Supports(GraphicsCapability.Texture3D));
        Assert.True(m_device.capabilities.SupportsSampled(RenderTextureFormat.RGBA8, RenderTextureDimension.Texture3D));
        Assert.True(m_device.capabilities.SupportsSampled(RenderTextureFormat.RGBA8, RenderTextureDimension.Cube));
        var volumeDescriptor = new RenderTextureDescriptor(
            4,
            4,
            RenderTextureFormat.RGBA8,
            RenderTextureUsage.Sampled,
            dimension: RenderTextureDimension.Texture3D,
            depth: 2
        );
        var cubeDescriptor = new RenderTextureDescriptor(
            4,
            4,
            RenderTextureFormat.RGBA8,
            RenderTextureUsage.Sampled,
            dimension: RenderTextureDimension.Cube
        );

        m_device.BeginFrame();
        PersistentTextureHandle volume = m_device.CreateTexture(volumeDescriptor, "Volume");
        PersistentTextureHandle cube = m_device.CreateTexture(cubeDescriptor, "Cube");
        m_device.UpdateTexture(volume, new byte[4 * 4 * 4], arrayLayer: 1);
        m_device.UpdateTexture(cube, new byte[4 * 4 * 4], arrayLayer: 5);
        m_device.DestroyTexture(volume);
        m_device.DestroyTexture(cube);
        m_device.EndFrame();
    }

    [Fact]
    public void NoopDevice_UpdatesDynamicVertexBuffer()
    {
        var layout = new RenderVertexLayout(
            [
                new RenderVertexAttribute(RenderVertexSemantic.Position, RenderVertexFormat.Float3)
            ]
        );
        var descriptor = new PersistentBufferDescriptor(
            new RenderBufferDescriptor(3, layout.stride, RenderBufferUsage.Vertex | RenderBufferUsage.Dynamic),
            layout
        );

        m_device.BeginFrame();
        PersistentBufferHandle buffer = m_device.CreateBuffer(descriptor, [], "Dynamic");
        m_device.UpdateBuffer(buffer, new byte[layout.stride * 3]);
        m_device.DestroyBuffer(buffer);
        m_device.EndFrame();
    }

    [Fact]
    public void NoopDevice_CreatesVertexBufferWithExplicitPadding()
    {
        var layout = new RenderVertexLayout(
            [
                new RenderVertexAttribute(RenderVertexSemantic.Position, RenderVertexFormat.Float3, 0),
                new RenderVertexAttribute(RenderVertexSemantic.Color0, RenderVertexFormat.UInt8Normalized4, 16)
            ],
            stride: 32
        );
        var descriptor = new PersistentBufferDescriptor(
            new RenderBufferDescriptor(3, layout.stride, RenderBufferUsage.Vertex),
            layout
        );

        m_device.BeginFrame();
        PersistentBufferHandle buffer = m_device.CreateBuffer(
            descriptor,
            new byte[layout.stride * 3],
            "Padded Vertices"
        );
        m_device.DestroyBuffer(buffer);
        m_device.EndFrame();

        Assert.True(buffer.isValid);
    }

    [Fact]
    public void NoopDevice_CreatesVertexAndIndexBuffersAtFrameSafetyPoint()
    {
        var layout = new RenderVertexLayout(
            [
                new RenderVertexAttribute(RenderVertexSemantic.Position, RenderVertexFormat.Float3)
            ]
        );
        var vertices = new PersistentBufferDescriptor(
            new RenderBufferDescriptor(3, layout.stride, RenderBufferUsage.Vertex),
            layout
        );
        var indices = new PersistentBufferDescriptor(
            new RenderBufferDescriptor(3, sizeof(ushort), RenderBufferUsage.Index),
            indexFormat: RenderIndexFormat.UInt16
        );

        m_device.BeginFrame();
        PersistentBufferHandle vertexBuffer = m_device.CreateBuffer(vertices, new byte[36], "Test Vertices");
        PersistentBufferHandle indexBuffer = m_device.CreateBuffer(indices, new byte[6], "Test Indices");
        m_device.DestroyBuffer(vertexBuffer);
        m_device.DestroyBuffer(indexBuffer);
        m_device.EndFrame();

        Assert.True(vertexBuffer.isValid);
        Assert.True(indexBuffer.isValid);
    }

    [Fact]
    public void NoopDevice_AllocatesTransientStorageBufferForCompiledGraph()
    {
        var builder = new RenderGraphBuilder(8, m_device.capabilities);
        RenderBufferHandle buffer = builder.CreateBuffer(
            "Cluster Light List",
            new RenderBufferDescriptor(64, 16, RenderBufferUsage.Storage)
        );
        builder
            .AddComputePass("Build Data", COMPUTE, 0, static (
                _,
                _
            ) => {})
            .WriteStorageBuffer(buffer)
            .HasSideEffect();
        CompiledRenderGraph graph = builder.Compile().graph!;

        m_device.BeginFrame();
        m_device.Execute(graph, 8);
        m_device.EndFrame();

        Assert.Single(graph.buffers);
    }

    [Fact]
    public void NoopDevice_AllocatesTransientStorageTextureForCompiledGraph()
    {
        Assert.True(m_device.capabilities.Supports(GraphicsCapability.StorageTexture));
        Assert.True(m_device.capabilities.SupportsStorage(RenderTextureFormat.RGBA8, RenderStorageAccess.Write));
        var builder = new RenderGraphBuilder(9, m_device.capabilities);
        RenderTextureHandle texture = builder.CreateTexture(
            "Compute Output",
            new RenderTextureDescriptor(8, 8, RenderTextureFormat.RGBA8, RenderTextureUsage.Storage)
        );
        builder
            .AddComputePass("Write Image", COMPUTE, 0, static (
                _,
                _
            ) => {})
            .WriteStorageTexture(texture)
            .HasSideEffect();
        CompiledRenderGraph graph = builder.Compile().graph!;

        m_device.BeginFrame();
        m_device.Execute(graph, 9);
        m_device.EndFrame();

        Assert.Single(graph.textures);
    }

    [Fact]
    public void NoopDevice_ReusesSteadyStateTransientTextureAndFramebufferAcrossGraphs()
    {
        CompiledRenderGraph first = BuildColorTargetGraph(101);
        m_device.BeginFrame();
        m_device.Execute(first, 101);
        m_device.EndFrame();

        IRenderDevice device = m_device;
        RenderDeviceAllocationCounters allocations = Assert.IsType<RenderDeviceAllocationCounters>(device.allocationCounters);
        Assert.Equal(device.generation, allocations.deviceGeneration);

        CompiledRenderGraph second = BuildColorTargetGraph(102, "Request[8] Renamed Color Pass");
        m_device.BeginFrame();
        m_device.Execute(second, 102);
        m_device.EndFrame();

        Assert.Equal(allocations, device.allocationCounters);
    }

    [Fact]
    public void NoopDevice_AcceptsBothSynchronizationPoliciesAndKeepsResizeFrameBoundaries()
    {
        IRenderDevice device = m_device;
        uint generation = device.generation;
        foreach (bool enabled in new[] { true, true, false, false, true, false })
        {
            device.SetVerticalSync(enabled);
            device.SetPrimaryPresentationSize(null);
            device.BeginFrame();
            Assert.Null(device.primaryPresentationSize);
            device.EndFrame();
        }
        Assert.Equal(generation, device.generation);
    }

    [Fact]
    public void NoopDevice_RetiresSupersededNamedFramebuffersDuringResizeChurn()
    {
        const int C_PASS_COUNT = 16;
        for (uint frame = 0; frame < 32; frame++)
        {
            var builder = new RenderGraphBuilder(1000 + frame, m_device.capabilities);
            int extent = 64 + checked((int)frame);
            for (int passIndex = 0; passIndex < C_PASS_COUNT; passIndex++)
            {
                RenderTextureHandle texture = builder.CreateTexture(
                    $"Resize Target {passIndex}",
                    new RenderTextureDescriptor(
                        extent,
                        extent,
                        RenderTextureFormat.RGBA8,
                        RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled
                    )
                );
                builder
                    .AddRasterPass(
                        $"Resize Pass {passIndex}",
                        FIRST,
                        passIndex,
                        static (
                            _,
                            _
                        ) => {}
                    )
                    .UseColorAttachment(texture, 0, RenderLoadAction.Clear)
                    .HasSideEffect();
            }

            CompiledRenderGraph graph = builder.Compile().graph!;
            m_device.BeginFrame();
            m_device.Execute(graph, 1000 + frame);
            m_device.EndFrame();
        }
    }

    [Fact]
    public void CreateBuffer_WithPartialInitialData_FailsWithoutClosingFrame()
    {
        var layout = new RenderVertexLayout(
            [
                new RenderVertexAttribute(RenderVertexSemantic.Position, RenderVertexFormat.Float3)
            ]
        );
        var descriptor = new PersistentBufferDescriptor(
            new RenderBufferDescriptor(3, layout.stride, RenderBufferUsage.Vertex),
            layout
        );

        m_device.BeginFrame();
        Assert.Throws<ArgumentException>(() => m_device.CreateBuffer(descriptor, new byte[12], "Partial"));
        m_device.EndFrame();
    }

    [Fact]
    public void NoopDevice_BindsRequestedTextureArrayLayer()
    {
        var builder = new RenderGraphBuilder(4, m_device.capabilities);
        RenderTextureHandle texture = builder.CreateTexture(
            "Layered Target",
            new RenderTextureDescriptor(
                8,
                8,
                RenderTextureFormat.RGBA8,
                RenderTextureUsage.ColorAttachment,
                arrayLayers: 2
            )
        );
        builder
            .AddRasterPass("Layer One", LAYERED, 0, static (
                _,
                _
            ) => {})
            .UseColorAttachment(texture, 0, RenderLoadAction.Clear, arrayLayer: 1);
        builder.MarkOutput(texture);
        CompiledRenderGraph graph = builder.Compile().graph!;

        m_device.BeginFrame();
        m_device.Execute(graph, 4);
        m_device.EndFrame();

        Assert.Equal(1, Assert.Single(Assert.Single(graph.passes).attachments).arrayLayer);
    }

    [Fact]
    public void EndFrame_WithoutBeginFrame_Throws() => Assert.Throws<InvalidOperationException>(() => m_device.EndFrame());

    [Fact]
    public void WindowlessDeviceDoesNotAdvertiseAnInternalBackbufferAsPresentation()
    {
        Assert.Null(m_device.primaryPresentationSize);
        Assert.Throws<ArgumentException>(() => m_device.SetPrimaryPresentationSize(default(RenderPresentationSize)));
        Assert.Throws<NotSupportedException>(() => m_device.SetPrimaryPresentationSize(new RenderPresentationSize(32, 24)));
        m_device.SetPrimaryPresentationSize(null);
        m_device.BeginFrame();
        m_device.EndFrame();
    }

    [Fact]
    public void SecondDeviceIsRejectedWhileTheProcessRuntimeIsOwned()
    {
        var options = new BgfxDeviceOptions
        {
            preferredBackend = GraphicsApi.Noop,
            backbufferWidth = 1,
            backbufferHeight = 1,
            verticalSync = false,
            sRgbBackbuffer = false,
            forceSingleThreaded = false
        };

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => new BgfxDevice(options));

        Assert.Contains("Only one BGFX device", exception.Message, StringComparison.Ordinal);
    }

    private CompiledRenderGraph BuildColorTargetGraph(
        uint generation,
        string passName = "Stable Color Pass"
    ) {
        var builder = new RenderGraphBuilder(generation, m_device.capabilities);
        RenderTextureHandle texture = builder.CreateTexture(
            "Stable Color Target",
            new RenderTextureDescriptor(
                32,
                32,
                RenderTextureFormat.RGBA8,
                RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled
            )
        );
        builder
            .AddRasterPass(passName, FIRST, 0, static (
                _,
                _
            ) => {})
            .UseColorAttachment(texture, 0, RenderLoadAction.Clear);
        builder.MarkOutput(texture);
        return builder.Compile().graph!;
    }
}

[CollectionDefinition(C_NAME, DisableParallelization = true)]
public sealed class BgfxDeviceCollection : ICollectionFixture<BgfxDeviceFixture>
{
    public const string C_NAME = "BGFX device";
}

public sealed class BgfxDeviceFixture : IDisposable
{
    public BgfxDeviceFixture()
    {
        this.device = CreateDevice();
    }

    public BgfxDevice device { get; }

    public void Dispose() => device.Dispose();

    private static BgfxDevice CreateDevice()
        => new(new BgfxDeviceOptions
        {
            preferredBackend = GraphicsApi.Noop,
            backbufferWidth = 1,
            backbufferHeight = 1,
            verticalSync = false,
            sRgbBackbuffer = false,
            forceSingleThreaded = true,
            deferredDestroyFrames = 2
        });
}
