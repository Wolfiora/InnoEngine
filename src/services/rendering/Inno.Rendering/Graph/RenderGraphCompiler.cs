using Inno.Core.Diagnostics;
using System;
using System.Collections.Generic;

namespace Inno.Rendering;

internal static partial class RenderGraphCompiler
{
    internal static RenderGraphCompileResult Compile(
        uint generation,
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderBufferRecord> buffers,
        IReadOnlyList<RenderPassRecord> passes,
        RenderGraphValidationState validation
    ) {
        if (!validation.result.isValid)
            return new RenderGraphCompileResult(null, validation.result.diagnostics);

        Dictionary<int, int> schedulePositions = [];
        for (int index = 0; index < validation.schedule.Length; index++)
            schedulePositions.Add(validation.schedule[index], index);
        int[] textureSlots = AllocateTextureSlots(textures, passes, schedulePositions);
        int[] bufferSlots = AllocateBufferSlots(buffers, passes, schedulePositions);
        return new RenderGraphCompileResult(
            BuildCompiledGraph(generation, textures, buffers, passes, validation.schedule, textureSlots, bufferSlots),
            validation.result.diagnostics,
            validation.result.culledPassCount);
    }

    private static CompiledRenderGraph BuildCompiledGraph(
        uint generation,
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderBufferRecord> buffers,
        IReadOnlyList<RenderPassRecord> passes,
        IReadOnlyList<int> schedule,
        IReadOnlyList<int> textureSlots,
        IReadOnlyList<int> bufferSlots
    ) {
        List<CompiledRenderPass> compiledPasses = [];
        for (int viewIndex = 0; viewIndex < schedule.Count; viewIndex++)
        {
            RenderPassRecord pass = passes[schedule[viewIndex]];
            List<CompiledRenderAttachment> attachments = [];
            foreach (RenderAttachment attachment in pass.attachments)
            {
                attachments.Add(new CompiledRenderAttachment(attachment));
            }

            compiledPasses.Add(new CompiledRenderPass(
                pass.name,
                pass.phase,
                pass.kind,
                viewIndex,
                attachments,
                pass.surface,
                pass.clearsPresentationTarget,
                pass.presentationClearColor,
                pass.viewTransform,
                pass.recordingMode,
                pass.execute));
        }

        List<CompiledRenderTexture> compiledTextures = [];
        for (int index = 0; index < textures.Count; index++)
        {
            RenderTextureRecord texture = textures[index];
            compiledTextures.Add(new CompiledRenderTexture(
                new RenderTextureHandle(index, generation, texture.allocationId),
                texture.name,
                texture.descriptor,
                texture.imported,
                texture.persistentHandle,
                textureSlots[index]));
        }

        List<CompiledRenderBuffer> compiledBuffers = [];
        for (int index = 0; index < buffers.Count; index++)
        {
            RenderBufferRecord buffer = buffers[index];
            compiledBuffers.Add(new CompiledRenderBuffer(
                new RenderBufferHandle(index, generation, buffer.allocationId),
                buffer.name,
                buffer.descriptor,
                buffer.imported,
                buffer.persistentHandle,
                bufferSlots[index]));
        }

        return new CompiledRenderGraph(generation, compiledPasses, compiledTextures, compiledBuffers);
    }
}
