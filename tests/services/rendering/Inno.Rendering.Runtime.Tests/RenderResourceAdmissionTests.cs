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
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.Input;
using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Testing.Source;

namespace Inno.Rendering.Runtime.Tests;

[Collection(nameof(RenderRuntimeFixture))]
public sealed class RenderResourceAdmissionTests : RenderRuntimeFixture
{
    [Fact]
    public void GeometryCandidatePublishesACompletePairAndPreservesOldPairAfterPartialAllocationFailure()
    {
        IRenderDevice backend = TestDeviceProxy.Create(out TestDeviceProxy device);
        using var runtime = new RenderRuntime(m_types, backend, new TestDiagnosticSink());
        var owner = new AssetRuntimeOwner();
        var geometry = new GeometryAsset(3, 3, 1, default, default);
        m_identities.Register(geometry);
        byte[] bytes = GeometryArtifact.Encode(
            new GeometryData(
                [
                    new GeometryVertex(default, default, default, default),
                    new GeometryVertex(default, default, default, default),
                    new GeometryVertex(default, default, default, default)
                ],
                [0, 1, 2],
                [new GeometrySection(0, 3)]
            )
        );
        owner.Initialize(geometry, AssetPath.Project("triangle.obj"), "first", bytes, false, 1);
        RenderGeometry? first = null;
        ResourceProbePipeline.action = resources => resources.TryResolveGeometry(geometry, out first);
        RunResourceFrame(runtime);
        Assert.NotNull(first);
        Assert.Equal(2, device.createdBuffers.Count);
        device.failBufferCreationAt = 4;
        owner.Initialize(geometry, AssetPath.Project("triangle.obj"), "second", bytes, false, 2);
        RenderGeometry? failed = null;
        ResourceProbePipeline.action = resources => resources.TryResolveGeometry(geometry, out failed);
        RunResourceFrame(runtime);
        Assert.NotNull(failed);
        Assert.Equal(first!.vertexBuffer, failed!.vertexBuffer);
        Assert.Equal(first.indexBuffer, failed.indexBuffer);
        Assert.Equal(device.createdBuffers[2], Assert.Single(device.destroyedBuffers));
        device.failBufferCreationAt = 0;
        RunResourceFrame(runtime);
        Assert.NotEqual(first.vertexBuffer, failed.vertexBuffer);
        Assert.NotEqual(first.indexBuffer, failed.indexBuffer);
        Assert.Equal(3, device.destroyedBuffers.Count);
        runtime.Dispose();
        Assert.Equal(device.createdBuffers.Count, device.destroyedBuffers.Count);
        owner.Release(geometry);
        m_identities.Unregister(geometry);
        ResourceProbePipeline.action = null;
    }

    [Fact]
    public void PersistentResourceAdmissionAndReplacementStayBoundedAcrossFrames()
    {
        IRenderDevice backend = TestDeviceProxy.Create(out TestDeviceProxy device);
        using var runtime = new RenderRuntime(
            m_types,
            backend,
            new TestDiagnosticSink(),
            resourceLimits: new RenderResourceLimits { resourcesPerKind = 1 }
        );
        var descriptor = new PersistentBufferDescriptor(new RenderBufferDescriptor(4, 4, RenderBufferUsage.Storage));
        int rejected = 0;
        for (int frame = 0; frame < 256; frame++)
        {
            long revision = frame;
            ResourceProbePipeline.action = resources =>
            {
                _ = resources.AcquireBuffer(new RenderPersistentResourceID("owned"), revision, descriptor, new byte[16], "owned");
                try
                {
                    resources.AcquireBuffer(new RenderPersistentResourceID("overflow"), revision, descriptor, new byte[16], "overflow");
                }
                catch (InvalidOperationException)
                {
                    rejected++;
                }
            };
            RunResourceFrame(runtime);
            Assert.Equal(1, runtime.resourceStatistics.activeResources);
        }
        Assert.Equal(256, rejected);
        Assert.Equal(256, runtime.resourceStatistics.rejectedResources);
        runtime.Dispose();
        Assert.Equal(256, device.createdBuffers.Count);
        Assert.Equal(256, device.destroyedBuffers.Count);
        ResourceProbePipeline.action = null;
    }

    [Fact]
    public async Task ReadbackAdmissionIsRejectedBeforeAllocatingASecondNativeOperation()
    {
        var device = new RecordingRenderDevice(supportsReadback: true);
        using var runtime = new RenderRuntime(
            m_types,
            device,
            new TestDiagnosticSink(),
            resourceLimits: new RenderResourceLimits { pendingReadbacks = 1 }
        );
        Task<RenderTextureReadbackResult>? readback = null;
        bool rejected = false;
        ResourceProbePipeline.action = resources =>
        {
            readback = resources.ReadTextureAsync(device.textureHandle).AsTask();
            try
            {
                _ = resources.ReadTextureAsync(device.textureHandle);
            }
            catch (InvalidOperationException)
            {
                rejected = true;
            }
        };
        RunResourceFrame(runtime);
        ResourceProbePipeline.action = null;
        Assert.True(rejected);
        Assert.Equal(1, runtime.resourceStatistics.pendingReadbacks);
        Assert.Equal(1, runtime.resourceStatistics.rejectedReadbacks);
        runtime.Dispose();
        await Assert.ThrowsAsync<ObjectDisposedException>(() => readback!);
        Assert.Equal(1, device.cancelReadbackCount);
    }

    [Fact]
    public void TargetCapacityRejectsBeforeNativeAllocationAndReopensAfterRelease()
    {
        IRenderDevice backend = TestDeviceProxy.Create(out TestDeviceProxy device);
        using var targets = new RenderTargetStore(backend, capacity: 1);
        backend.BeginFrame();
        var descriptor = new RenderTextureDescriptor(
            16,
            16,
            RenderTextureFormat.RGBA8,
            RenderTextureUsage.ColorAttachment
        );
        var first = new RenderTexture("First", descriptor);
        var second = new RenderTexture("Second", descriptor);
        var graph = new RenderGraphBuilder(1, backend.capabilities);
        targets.Import(graph, first);
        Assert.Throws<InvalidOperationException>(() => targets.Import(graph, second));
        Assert.Single(device.createdTextures);
        Assert.Equal(1, targets.rejectedCount);
        targets.Release(first);
        targets.PrepareFrame();
        targets.Import(new RenderGraphBuilder(2, backend.capabilities), second);
        Assert.Equal(1, targets.count);
        targets.Dispose();
        Assert.Equal(device.createdTextures.Count, device.destroyedTextures.Count);
        backend.EndFrame();
    }

    [Fact]
    public void UploadAdmissionResetsEachFrameWithoutGrowingResidentPages()
    {
        IRenderDevice backend = TestDeviceProxy.Create(out TestDeviceProxy device);
        using var runtime = new RenderRuntime(m_types, backend, new TestDiagnosticSink(), resourceLimits:
            new RenderResourceLimits { uploadPages = 1, uploadResidentBytes = 4096, uploadBytesPerFrame = 36 });
        for (int frame = 0; frame < 256; frame++)
        {
            runtime.Submit(CreateRequest("Upload budget", new RenderPipelineAsset { pipelineTypeId = UploadPipeline.C_EXTENSION_ID }));
            BeginRenderFrame(runtime, 0f);
            runtime.AfterRender(default);
            runtime.EndFrame(default);
            Assert.Equal(1, runtime.resourceStatistics.uploadPages);
            Assert.Equal(36, runtime.resourceStatistics.uploadedFrameBytes);
        }
        Assert.Equal(256, runtime.resourceStatistics.rejectedUploads);
        Assert.Single(device.createdBuffers);
        runtime.Dispose();
        Assert.Single(device.destroyedBuffers);
    }

    [Fact]
    public void FrameUploadsReusePagesAndResetSliceOffsetsBetweenFrames()
    {
        var device = new RecordingRenderDevice();
        var asset = new RenderPipelineAsset { pipelineTypeId = UploadPipeline.C_EXTENSION_ID };
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());

        runtime.Submit(CreateRequest("Upload A", asset));
        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(1, device.createBufferCount);
        Assert.Equal(new[] { 0, 3 }, device.bufferUpdateOffsets.ToArray());
        Assert.Equal(0, UploadPipeline.firstSlice.firstElement);
        Assert.Equal(3, UploadPipeline.secondSlice.firstElement);
        Assert.Equal(3, UploadPipeline.secondSlice.elementCount);

        UploadPipeline.singleUpload = true;
        runtime.Submit(CreateRequest("Upload B", asset));
        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(1, device.createBufferCount);
        Assert.Equal(new[] { 0, 3, 0 }, device.bufferUpdateOffsets.ToArray());
        Assert.Equal(0, UploadPipeline.firstSlice.firstElement);
        runtime.Detach();
    }

    [Fact]
    public void TexturePrewarmRunsOutsideTheRenderFrameAndDoesNotDelaySubmission()
    {
        var texture = new TextureAsset(1, 1, TextureColorSpace.Srgb, "png");
        Assert.True(m_identities.Register(texture));
        TexturePrewarmPipeline.texture = texture;

        var compiler = new DelayedTextureArtifactSource();
        var device = new RecordingRenderDevice();
        var asset = new RenderPipelineAsset { pipelineTypeId = TexturePrewarmPipeline.C_EXTENSION_ID };
        var artifacts = new DelayedArtifactProvider(compiler);
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink(), targetArtifacts: artifacts);
        runtime.Submit(CreateRequest("Prewarm", asset));

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(1, device.endFrameCount);
        Assert.True(compiler.started.Wait(TimeSpan.FromSeconds(2)));
        Assert.False(compiler.isCompleted);
        compiler.Complete([1, 2, 3, 4]);
        runtime.Detach();
    }

    [Fact]
    public async Task PersistentTextureRegionUpdateAndReadbackCompleteAcrossFrameBoundaries()
    {
        var device = new RecordingRenderDevice(supportsReadback: true);
        ReadbackPipeline.texture = device.textureHandle;
        var asset = new RenderPipelineAsset { pipelineTypeId = ReadbackPipeline.C_EXTENSION_ID };
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        runtime.Submit(CreateRequest("Readback", asset));

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(new RenderTextureRegion(0, 1, 1, 0, 2, 2), device.updatedRegion);
        Assert.Equal(Enumerable.Range(0, 16).Select(static value => (byte)value), device.updatedBytes);
        Task<RenderTextureReadbackResult> readback = Assert.IsType<Task<RenderTextureReadbackResult>>(
            ReadbackPipeline.readback
        );
        Assert.False(readback.IsCompleted);

        device.readbackReady = true;
        BeginRenderFrame(runtime, 0f);
        RenderTextureReadbackResult result = await readback;
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(16, result.rowPitch);
        Assert.Equal(64, result.data.Length);
        Assert.All(result.data.ToArray(), static value => Assert.Equal((byte)37, value));
        runtime.Detach();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task PendingUploadOrReadbackRetirementPreservesDependentWork(bool pendingReadback)
    {
        var device = new RecordingRenderDevice(supportsReadback: true);
        ReadbackPipeline.texture = device.textureHandle;
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        runtime.Submit(CreateRequest("Upload", new RenderPipelineAsset { pipelineTypeId = UploadPipeline.C_EXTENSION_ID }));
        runtime.Submit(CreateRequest("Readback", new RenderPipelineAsset { pipelineTypeId = ReadbackPipeline.C_EXTENSION_ID }));
        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Task<RenderTextureReadbackResult> readback = Assert.IsType<Task<RenderTextureReadbackResult>>(ReadbackPipeline.readback);
        device.pendingBufferRetirements = pendingReadback ? 0 : 1;
        device.pendingReadbackRetirements = pendingReadback ? 1 : 0;

        runtime.Dispose();

        Assert.Equal(pendingReadback ? 1 : 2, device.bufferRetirementAttempts);
        Assert.Equal(pendingReadback ? 2 : 1, device.cancelReadbackCount);
        Assert.Equal(2, device.endFrameCount);
        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await readback);
        runtime.Dispose();
        Assert.Equal(2, device.endFrameCount);
    }

    [Fact]
    public async Task CancelingReadbackReleasesTheDeviceOperationAtTheNextFrameBoundary()
    {
        var device = new RecordingRenderDevice(supportsReadback: true);
        using var cancellation = new CancellationTokenSource();
        ReadbackPipeline.texture = device.textureHandle;
        ReadbackPipeline.cancellationToken = cancellation.Token;
        var asset = new RenderPipelineAsset { pipelineTypeId = ReadbackPipeline.C_EXTENSION_ID };
        var runtime = new RenderRuntime(m_types, device, new TestDiagnosticSink());
        runtime.Submit(CreateRequest("Canceled Readback", asset));

        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        cancellation.Cancel();
        Task<RenderTextureReadbackResult> readback = Assert.IsType<Task<RenderTextureReadbackResult>>(
            ReadbackPipeline.readback
        );
        Assert.False(readback.IsCompleted);
        BeginRenderFrame(runtime, 0f);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await readback);
        runtime.AfterRender(default);
        runtime.EndFrame(default);

        Assert.Equal(1, device.cancelReadbackCount);
        runtime.Detach();
    }
}
