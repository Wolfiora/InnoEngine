using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Inno.Rendering;

internal sealed partial class RecordedRenderCommandEncoder : RenderCommandEncoder, IDisposable
{
    private readonly RecordedRenderCommandBuffer m_buffer = new();
    private bool m_sealed;

    /// <summary>
    /// Releases all command snapshots and references after recording or replay ends.
    /// </summary>
    public void Dispose()
    {
        m_sealed = true;
        m_buffer.Dispose();
    }

    /// <inheritdoc />
    public override void BindGraphicsPipeline(GraphicsPipelineHandle pipeline)
    {
        Begin(Operation.BindGraphicsPipeline);
        m_buffer.Write(pipeline);
    }

    /// <inheritdoc />
    public override void BindComputePipeline(ComputePipelineHandle pipeline)
    {
        Begin(Operation.BindComputePipeline);
        m_buffer.Write(pipeline);
    }

    /// <inheritdoc />
    public override void BindTexture(
        RenderBindingId binding,
        RenderTextureHandle texture,
        RenderSamplerState sampler
    ) {
        Begin(Operation.BindTextureGraph);
        m_buffer.WriteReference(binding.value);
        m_buffer.Write(texture);
        m_buffer.Write(sampler);
    }

    /// <inheritdoc />
    public override void BindTexture(
        RenderBindingId binding,
        PersistentTextureHandle texture,
        RenderSamplerState sampler
    ) {
        Begin(Operation.BindTexturePersistent);
        m_buffer.WriteReference(binding.value);
        m_buffer.Write(texture);
        m_buffer.Write(sampler);
    }

    /// <inheritdoc />
    public override void BindStorageTexture(
        RenderBindingId binding,
        RenderTextureHandle texture,
        int mipLevel = 0
    ) {
        Begin(Operation.BindStorageTextureGraph);
        m_buffer.WriteReference(binding.value);
        m_buffer.Write(texture);
        m_buffer.Write(mipLevel);
    }

    /// <inheritdoc />
    public override void BindStorageTexture(
        RenderBindingId binding,
        PersistentTextureHandle texture,
        int mipLevel = 0
    ) {
        Begin(Operation.BindStorageTexturePersistent);
        m_buffer.WriteReference(binding.value);
        m_buffer.Write(texture);
        m_buffer.Write(mipLevel);
    }

    /// <inheritdoc />
    public override void BindBuffer(
        RenderBindingId binding,
        RenderBufferHandle buffer
    ) {
        Begin(Operation.BindBufferGraph);
        m_buffer.WriteReference(binding.value);
        m_buffer.Write(buffer);
    }

    /// <inheritdoc />
    public override void BindBuffer(
        RenderBindingId binding,
        PersistentBufferHandle buffer
    ) {
        Begin(Operation.BindBufferPersistent);
        m_buffer.WriteReference(binding.value);
        m_buffer.Write(buffer);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Copies the borrowed uniform bytes into this recording before returning.
    /// </remarks>
    public override void SetUniform(
        RenderBindingId binding,
        ReadOnlySpan<byte> value
    ) {
        Begin(Operation.SetUniform);
        m_buffer.WriteReference(binding.value);
        m_buffer.WriteSnapshot(value);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Copies the borrowed matrix values into this recording before returning.
    /// </remarks>
    public override void SetTransform(ReadOnlySpan<float> columnMajorMatrix)
    {
        Begin(Operation.SetTransform);
        m_buffer.WriteSnapshot(MemoryMarshal.AsBytes(columnMajorMatrix));
    }

    /// <inheritdoc />
    public override void SetRasterState(RenderRasterState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        Begin(Operation.SetRasterState);
        m_buffer.WriteReference(state);
    }

    /// <inheritdoc />
    public override void SetStencil(RenderStencilState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        Begin(Operation.SetStencil);
        m_buffer.WriteReference(state);
    }

    /// <inheritdoc />
    public override void SetViewport(
        int x,
        int y,
        int width,
        int height
    ) {
        Begin(Operation.SetViewport);
        m_buffer.Write(x);
        m_buffer.Write(y);
        m_buffer.Write(width);
        m_buffer.Write(height);
    }

    /// <inheritdoc />
    public override void SetScissor(
        int x,
        int y,
        int width,
        int height
    ) {
        Begin(Operation.SetScissor);
        m_buffer.Write(x);
        m_buffer.Write(y);
        m_buffer.Write(width);
        m_buffer.Write(height);
    }

    /// <inheritdoc />
    public override void BindVertexBuffer(
        RenderBufferHandle buffer,
        int firstVertex = 0
    ) {
        Begin(Operation.BindVertexBufferGraph);
        m_buffer.Write(buffer);
        m_buffer.Write(firstVertex);
    }

    /// <inheritdoc />
    public override void BindVertexBuffer(
        PersistentBufferHandle buffer,
        int firstVertex = 0
    ) {
        Begin(Operation.BindVertexBufferPersistent);
        m_buffer.Write(buffer);
        m_buffer.Write(firstVertex);
    }

    /// <inheritdoc />
    public override void BindIndexBuffer(
        RenderBufferHandle buffer,
        int firstIndex = 0
    ) {
        Begin(Operation.BindIndexBufferGraph);
        m_buffer.Write(buffer);
        m_buffer.Write(firstIndex);
    }

    /// <inheritdoc />
    public override void BindIndexBuffer(
        PersistentBufferHandle buffer,
        int firstIndex = 0
    ) {
        Begin(Operation.BindIndexBufferPersistent);
        m_buffer.Write(buffer);
        m_buffer.Write(firstIndex);
    }

    /// <inheritdoc />
    public override void BindInstanceBuffer(
        RenderBufferHandle buffer,
        int firstInstance,
        int instanceCount
    ) {
        Begin(Operation.BindInstanceBufferGraph);
        m_buffer.Write(buffer);
        m_buffer.Write(firstInstance);
        m_buffer.Write(instanceCount);
    }

    /// <inheritdoc />
    public override void BindInstanceBuffer(
        PersistentBufferHandle buffer,
        int firstInstance,
        int instanceCount
    ) {
        Begin(Operation.BindInstanceBufferPersistent);
        m_buffer.Write(buffer);
        m_buffer.Write(firstInstance);
        m_buffer.Write(instanceCount);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void Draw(
        int vertexCount,
        int instanceCount = 1
    ) {
        RecordDraw(Operation.Draw, vertexCount, instanceCount);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void DrawProcedural(
        int vertexCount,
        int instanceCount = 1
    ) {
        RecordDraw(Operation.DrawProcedural, vertexCount, instanceCount);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void DrawIndexed(
        int indexCount,
        int instanceCount = 1
    ) {
        RecordDraw(Operation.DrawIndexed, indexCount, instanceCount);
    }

    /// <inheritdoc />
    public override void DrawIndirect(
        RenderBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    ) {
        Begin(Operation.DrawIndirectGraph);
        m_buffer.Write(buffer);
        m_buffer.Write(firstCommand);
        m_buffer.Write(commandCount);
    }

    /// <inheritdoc />
    public override void DrawIndirect(
        PersistentBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    ) {
        Begin(Operation.DrawIndirectPersistent);
        m_buffer.Write(buffer);
        m_buffer.Write(firstCommand);
        m_buffer.Write(commandCount);
    }

    /// <inheritdoc />
    public override void Dispatch(
        int groupCountX,
        int groupCountY = 1,
        int groupCountZ = 1
    ) {
        Begin(Operation.Dispatch);
        m_buffer.Write(groupCountX);
        m_buffer.Write(groupCountY);
        m_buffer.Write(groupCountZ);
    }

    /// <inheritdoc />
    public override void DispatchIndirect(
        RenderBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    ) {
        Begin(Operation.DispatchIndirectGraph);
        m_buffer.Write(buffer);
        m_buffer.Write(firstCommand);
        m_buffer.Write(commandCount);
    }

    /// <inheritdoc />
    public override void DispatchIndirect(
        PersistentBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    ) {
        Begin(Operation.DispatchIndirectPersistent);
        m_buffer.Write(buffer);
        m_buffer.Write(firstCommand);
        m_buffer.Write(commandCount);
    }

    /// <inheritdoc />
    public override void CopyTexture(
        RenderTextureHandle source,
        RenderTextureHandle destination
    ) {
        Begin(Operation.CopyTextureGraph);
        m_buffer.Write(source);
        m_buffer.Write(destination);
    }

    /// <inheritdoc />
    public override void BlitTexture(
        RenderTextureHandle source,
        RenderTextureRegion sourceRegion,
        RenderTextureHandle destination,
        RenderTextureRegion destinationRegion
    ) {
        Begin(Operation.BlitTextureGraph);
        m_buffer.Write(source);
        m_buffer.Write(sourceRegion);
        m_buffer.Write(destination);
        m_buffer.Write(destinationRegion);
    }

    /// <inheritdoc />
    public override void CopyBuffer(
        RenderBufferHandle source,
        RenderBufferHandle destination
    ) {
        Begin(Operation.CopyBufferGraph);
        m_buffer.Write(source);
        m_buffer.Write(destination);
    }

    internal void Seal() => m_sealed = true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Begin(Operation operation)
    {
        EnsureWritable();
        m_buffer.Write(operation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RecordDraw(
        Operation operation,
        int count,
        int instanceCount
    ) {
        EnsureWritable();
        m_buffer.WriteInstruction((byte)operation, new DrawArguments(count, instanceCount));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureWritable()
    {
        if (m_sealed)
            throw new InvalidOperationException("A recorded render command list is already sealed.");
    }

}
