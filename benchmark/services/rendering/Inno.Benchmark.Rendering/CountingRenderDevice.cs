using System;
using Inno.Benchmark.Rendering.Fixtures;
using Inno.Rendering;

namespace Inno.Benchmark.Rendering;

internal sealed class CountingRenderDevice : RenderDevice, IRenderDevice
{
    public int creates { get; private set; }
    public int uploads { get; private set; }
    public GraphicsCapabilities capabilities { get; } = BenchmarkGraphicsCapabilities.Create();
    public uint generation => 1;
    public RenderPresentationSize? primaryPresentationSize { get; private set; } = new(64, 64);
    public bool primaryPresentationEncodesSrgb => true;

    public void SetPrimaryPresentationSize(RenderPresentationSize? size) => primaryPresentationSize = size;

    public void SetVerticalSync(bool enabled) {}

    public void BeginFrame() {}

    public uint EndFrame() => 1;

    public void Execute(
        CompiledRenderGraph graph,
        ulong frameIndex
    ) => throw new NotSupportedException();

    public PersistentTextureHandle CreateTexture(
        RenderTextureDescriptor descriptor,
        string name
    ) => CreatePersistentTextureHandle((ulong)++creates, generation);

    public void UpdateTexture(
        PersistentTextureHandle texture,
        ReadOnlySpan<byte> data,
        int mipLevel = 0,
        int arrayLayer = 0
    ) => uploads++;

    public void DestroyTexture(PersistentTextureHandle texture) {}

    public void Dispose() {}

    public void UpdateTextureRegion(
        PersistentTextureHandle texture,
        RenderTextureRegion region,
        ReadOnlySpan<byte> data
    ) => throw new NotSupportedException();

    public RenderTextureReadbackHandle BeginTextureReadback(
        PersistentTextureHandle texture,
        int mipLevel = 0
    ) => throw new NotSupportedException();

    public bool TryGetTextureReadback(
        RenderTextureReadbackHandle readback,
        out RenderTextureReadbackResult? result
    ) => throw new NotSupportedException();

    public void CancelTextureReadback(RenderTextureReadbackHandle readback) => throw new NotSupportedException();

    public PersistentBufferHandle CreateBuffer(
        PersistentBufferDescriptor descriptor,
        ReadOnlySpan<byte> initialData,
        string name
    ) => throw new NotSupportedException();

    public void UpdateBuffer(
        PersistentBufferHandle buffer,
        ReadOnlySpan<byte> data,
        int startElement = 0
    ) => throw new NotSupportedException();

    public void DestroyBuffer(PersistentBufferHandle buffer) => throw new NotSupportedException();

    public GraphicsPipelineHandle CreateGraphicsPipeline(
        GraphicsPipelineDescriptor descriptor,
        string name
    ) => throw new NotSupportedException();

    public void DestroyGraphicsPipeline(GraphicsPipelineHandle pipeline) => throw new NotSupportedException();

    public ComputePipelineHandle CreateComputePipeline(
        ComputePipelineDescriptor descriptor,
        string name
    ) => throw new NotSupportedException();

    public void DestroyComputePipeline(ComputePipelineHandle pipeline) => throw new NotSupportedException();
}
