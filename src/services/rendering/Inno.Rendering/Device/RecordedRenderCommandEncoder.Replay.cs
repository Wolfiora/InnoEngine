using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Inno.Rendering;

internal sealed partial class RecordedRenderCommandEncoder
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    internal void Replay(RenderCommandEncoder destination)
    {
        ArgumentNullException.ThrowIfNull(destination);
        if (!m_sealed)
            throw new InvalidOperationException("A render command list must be sealed before replay.");
        RecordedRenderCommandBuffer.Reader reader = m_buffer.GetReader();
        while (reader.hasCommands)
        {
            switch (reader.Read<Operation>())
            {
                case Operation.BindGraphicsPipeline:
                    destination.BindGraphicsPipeline(reader.Read<GraphicsPipelineHandle>());
                    break;
                case Operation.BindComputePipeline:
                    destination.BindComputePipeline(reader.Read<ComputePipelineHandle>());
                    break;
                case Operation.BindTextureGraph:
                    destination.BindTexture(
                        reader.ReadBinding(),
                        reader.Read<RenderTextureHandle>(),
                        reader.Read<RenderSamplerState>());
                    break;
                case Operation.BindTexturePersistent:
                    destination.BindTexture(
                        reader.ReadBinding(),
                        reader.Read<PersistentTextureHandle>(),
                        reader.Read<RenderSamplerState>());
                    break;
                case Operation.BindStorageTextureGraph:
                    destination.BindStorageTexture(
                        reader.ReadBinding(),
                        reader.Read<RenderTextureHandle>(),
                        reader.Read<int>());
                    break;
                case Operation.BindStorageTexturePersistent:
                    destination.BindStorageTexture(
                        reader.ReadBinding(),
                        reader.Read<PersistentTextureHandle>(),
                        reader.Read<int>());
                    break;
                case Operation.BindBufferGraph:
                    destination.BindBuffer(
                        reader.ReadBinding(),
                        reader.Read<RenderBufferHandle>());
                    break;
                case Operation.BindBufferPersistent:
                    destination.BindBuffer(
                        reader.ReadBinding(),
                        reader.Read<PersistentBufferHandle>());
                    break;
                case Operation.SetUniform:
                    destination.SetUniform(
                        reader.ReadBinding(),
                        reader.ReadSnapshot());
                    break;
                case Operation.SetTransform:
                    destination.SetTransform(MemoryMarshal.Cast<byte, float>(reader.ReadSnapshot()));
                    break;
                case Operation.SetRasterState:
                    destination.SetRasterState(reader.ReadReference<RenderRasterState>()!);
                    break;
                case Operation.SetStencil:
                    destination.SetStencil(reader.ReadReference<RenderStencilState>()!);
                    break;
                case Operation.SetViewport:
                    destination.SetViewport(
                        reader.Read<int>(),
                        reader.Read<int>(),
                        reader.Read<int>(),
                        reader.Read<int>());
                    break;
                case Operation.SetScissor:
                    destination.SetScissor(
                        reader.Read<int>(),
                        reader.Read<int>(),
                        reader.Read<int>(),
                        reader.Read<int>());
                    break;
                case Operation.BindVertexBufferGraph:
                    destination.BindVertexBuffer(
                        reader.Read<RenderBufferHandle>(),
                        reader.Read<int>());
                    break;
                case Operation.BindVertexBufferPersistent:
                    destination.BindVertexBuffer(
                        reader.Read<PersistentBufferHandle>(),
                        reader.Read<int>());
                    break;
                case Operation.BindIndexBufferGraph:
                    destination.BindIndexBuffer(
                        reader.Read<RenderBufferHandle>(),
                        reader.Read<int>());
                    break;
                case Operation.BindIndexBufferPersistent:
                    destination.BindIndexBuffer(
                        reader.Read<PersistentBufferHandle>(),
                        reader.Read<int>());
                    break;
                case Operation.BindInstanceBufferGraph:
                    destination.BindInstanceBuffer(
                        reader.Read<RenderBufferHandle>(),
                        reader.Read<int>(),
                        reader.Read<int>());
                    break;
                case Operation.BindInstanceBufferPersistent:
                    destination.BindInstanceBuffer(
                        reader.Read<PersistentBufferHandle>(),
                        reader.Read<int>(),
                        reader.Read<int>());
                    break;
                case Operation.Draw:
                    DrawArguments draw = reader.Read<DrawArguments>();
                    destination.Draw(
                        draw.count,
                        draw.instanceCount);
                    break;
                case Operation.DrawProcedural:
                    DrawArguments procedural = reader.Read<DrawArguments>();
                    destination.DrawProcedural(
                        procedural.count,
                        procedural.instanceCount);
                    break;
                case Operation.DrawIndexed:
                    DrawArguments indexed = reader.Read<DrawArguments>();
                    destination.DrawIndexed(
                        indexed.count,
                        indexed.instanceCount);
                    break;
                case Operation.DrawIndirectGraph:
                    destination.DrawIndirect(
                        reader.Read<RenderBufferHandle>(),
                        reader.Read<int>(),
                        reader.Read<int>());
                    break;
                case Operation.DrawIndirectPersistent:
                    destination.DrawIndirect(
                        reader.Read<PersistentBufferHandle>(),
                        reader.Read<int>(),
                        reader.Read<int>());
                    break;
                case Operation.Dispatch:
                    destination.Dispatch(
                        reader.Read<int>(),
                        reader.Read<int>(),
                        reader.Read<int>());
                    break;
                case Operation.DispatchIndirectGraph:
                    destination.DispatchIndirect(
                        reader.Read<RenderBufferHandle>(),
                        reader.Read<int>(),
                        reader.Read<int>());
                    break;
                case Operation.DispatchIndirectPersistent:
                    destination.DispatchIndirect(
                        reader.Read<PersistentBufferHandle>(),
                        reader.Read<int>(),
                        reader.Read<int>());
                    break;
                case Operation.CopyTextureGraph:
                    destination.CopyTexture(
                        reader.Read<RenderTextureHandle>(),
                        reader.Read<RenderTextureHandle>());
                    break;
                case Operation.BlitTextureGraph:
                    destination.BlitTexture(
                        reader.Read<RenderTextureHandle>(),
                        reader.Read<RenderTextureRegion>(),
                        reader.Read<RenderTextureHandle>(),
                        reader.Read<RenderTextureRegion>());
                    break;
                case Operation.CopyBufferGraph:
                    destination.CopyBuffer(
                        reader.Read<RenderBufferHandle>(),
                        reader.Read<RenderBufferHandle>());
                    break;
                default:
                    throw new InvalidOperationException("A recorded render command has an invalid operation.");
            }
        }
    }

    private readonly record struct DrawArguments(
        int count,
        int instanceCount
    );

    private enum Operation : byte
    {
        BindGraphicsPipeline,
        BindComputePipeline,
        BindTextureGraph,
        BindTexturePersistent,
        BindStorageTextureGraph,
        BindStorageTexturePersistent,
        BindBufferGraph,
        BindBufferPersistent,
        SetUniform,
        SetTransform,
        SetRasterState,
        SetStencil,
        SetViewport,
        SetScissor,
        BindVertexBufferGraph,
        BindVertexBufferPersistent,
        BindIndexBufferGraph,
        BindIndexBufferPersistent,
        BindInstanceBufferGraph,
        BindInstanceBufferPersistent,
        Draw,
        DrawProcedural,
        DrawIndexed,
        DrawIndirectGraph,
        DrawIndirectPersistent,
        Dispatch,
        DispatchIndirectGraph,
        DispatchIndirectPersistent,
        CopyTextureGraph,
        BlitTextureGraph,
        CopyBufferGraph
    }
}
