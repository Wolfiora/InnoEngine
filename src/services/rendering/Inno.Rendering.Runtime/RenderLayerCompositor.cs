using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using Inno.Rendering;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Composites isolated model targets with one engine-owned premultiplied-alpha program.
/// </summary>
internal sealed class RenderLayerCompositor : IDisposable
{
    private static readonly RenderPhaseId S_PHASE = new("inno.runtime.model-composition");
    private static readonly RenderBindingId S_TEXTURE = new("s_tex");
    private static readonly float[] S_IDENTITY =
    [
        1f, 0f, 0f, 0f,
        0f, 1f, 0f, 0f,
        0f, 0f, 1f, 0f,
        0f, 0f, 0f, 1f
    ];
    private static readonly RenderVertexLayout S_VERTEX_LAYOUT = new(
    [
        new(RenderVertexSemantic.Position, RenderVertexFormat.Float2),
        new(RenderVertexSemantic.TextureCoordinate0, RenderVertexFormat.Float2),
        new(RenderVertexSemantic.Color0, RenderVertexFormat.UInt8Normalized4)
    ]);

    private readonly IRenderDevice m_device;
    private readonly IRenderLayerCompositionProgramProvider? m_programProvider;
    private GraphicsPipelineHandle m_pipeline;
    private GraphicsPipelineHandle m_outputTransferPipeline;
    private PersistentBufferHandle m_vertices;
    private PersistentBufferHandle m_indices;
    private bool m_disposed;

    internal RenderLayerCompositor(
        IRenderDevice device,
        IRenderLayerCompositionProgramProvider? programProvider
    ) {
        m_device = device ?? throw new ArgumentNullException(nameof(device));
        m_programProvider = programProvider;
    }

    internal void AddPasses(
        RenderGraphBuilder graph,
        string name,
        IReadOnlyList<RenderTextureHandle> layers,
        RenderTextureHandle output,
        RenderViewport viewport,
        RenderTextureFormat layerFormat
    ) {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(layers);
        if (layers.Count == 0)
            throw new ArgumentException("Composition requires at least one layer.", nameof(layers));
        EnsureResources();
        bool needsOutputTransfer = !output.isValid && !m_device.primaryPresentationEncodesSrgb;
        if (needsOutputTransfer)
            EnsureOutputTransferPipeline();
        if (needsOutputTransfer && layers.Count == 1)
        {
            AddOutputTransfer(graph, name, layers[0], viewport);
            return;
        }
        RenderTextureHandle compositionTarget = output;
        RenderViewport compositionViewport = viewport;
        if (needsOutputTransfer)
        {
            compositionTarget = graph.CreateTexture(
                $"{name}/Composition",
                new RenderTextureDescriptor(
                    viewport.width,
                    viewport.height,
                    layerFormat,
                    RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled));
            compositionViewport = new RenderViewport(0, 0, viewport.width, viewport.height);
        }
        for (int index = 0; index < layers.Count; index++)
        {
            if (!layers[index].isValid)
                throw new ArgumentException("Composition contains an invalid layer.", nameof(layers));
            var data = new PassData(m_pipeline, m_vertices, m_indices, layers[index], compositionViewport);
            RasterPassBuilder pass = graph.AddRasterPass(
                $"{name}/Layer {index + 1}", S_PHASE, data,
                static (
                    value,
                    context
                ) => DrawFullscreen(value, context));
            pass.SetViewTransform(S_IDENTITY, S_IDENTITY);
            pass.ReadTexture(layers[index]);
            if (compositionTarget.isValid)
            {
                pass.UseColorAttachment(compositionTarget, 0,
                    index == 0 ? RenderLoadAction.Clear : RenderLoadAction.Load,
                    RenderStoreAction.Store, default);
            }
            else
            {
                if (index == 0)
                    pass.ClearPresentationTarget(default);
                pass.HasSideEffect();
            }
        }
        if (needsOutputTransfer)
            AddOutputTransfer(graph, name, compositionTarget, viewport);
    }

    /// <summary>
    /// Releases the engine-owned shader pipeline and fullscreen quad buffers.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
            return;
        if (m_indices.isValid)
            m_device.DestroyBuffer(m_indices);
        if (m_vertices.isValid)
            m_device.DestroyBuffer(m_vertices);
        if (m_pipeline.isValid)
            m_device.DestroyGraphicsPipeline(m_pipeline);
        if (m_outputTransferPipeline.isValid)
            m_device.DestroyGraphicsPipeline(m_outputTransferPipeline);
        m_indices = default;
        m_vertices = default;
        m_pipeline = default;
        m_outputTransferPipeline = default;
        m_disposed = true;
    }

    private void EnsureResources()
    {
        if (m_pipeline.isValid)
            return;
        IRenderLayerCompositionProgramProvider provider = m_programProvider
            ?? throw new InvalidOperationException("The rendering host has no layer composition program provider.");
        GraphicsPipelineDescriptor descriptor = provider.CreateDescriptor(m_device.capabilities, S_VERTEX_LAYOUT);
        GraphicsPipelineHandle pipeline = m_device.CreateGraphicsPipeline(
            descriptor, "Render Model Composition");
        PersistentBufferHandle vertices = default;
        PersistentBufferHandle indices = default;
        try
        {
            vertices = m_device.CreateBuffer(
                new PersistentBufferDescriptor(new RenderBufferDescriptor(4, 20, RenderBufferUsage.Vertex), S_VERTEX_LAYOUT),
                CreateVertices(m_device.capabilities.originBottomLeft),
                "Render Model Composition Vertices");
            indices = m_device.CreateBuffer(
                new PersistentBufferDescriptor(
                    new RenderBufferDescriptor(6, sizeof(ushort), RenderBufferUsage.Index),
                    indexFormat: RenderIndexFormat.UInt16),
                [0, 0, 1, 0, 2, 0, 0, 0, 2, 0, 3, 0],
                "Render Model Composition Indices");
        }
        catch
        {
            if (indices.isValid)
                m_device.DestroyBuffer(indices);
            if (vertices.isValid)
                m_device.DestroyBuffer(vertices);
            m_device.DestroyGraphicsPipeline(pipeline);
            throw;
        }
        m_pipeline = pipeline;
        m_vertices = vertices;
        m_indices = indices;
    }

    private void EnsureOutputTransferPipeline()
    {
        if (m_outputTransferPipeline.isValid)
            return;
        IRenderLayerCompositionProgramProvider provider = m_programProvider
            ?? throw new InvalidOperationException("The rendering host has no layer composition program provider.");
        GraphicsPipelineDescriptor descriptor = provider.CreateOutputTransferDescriptor(
            m_device.capabilities,
            S_VERTEX_LAYOUT);
        m_outputTransferPipeline = m_device.CreateGraphicsPipeline(
            descriptor,
            "Render Model Output Transfer");
    }

    private void AddOutputTransfer(
        RenderGraphBuilder graph,
        string name,
        RenderTextureHandle source,
        RenderViewport viewport
    ) {
        var data = new PassData(m_outputTransferPipeline, m_vertices, m_indices, source, viewport);
        RasterPassBuilder transfer = graph.AddRasterPass(
            $"{name}/Output Transfer",
            S_PHASE,
            data,
            static (
                value,
                context
            ) => DrawFullscreen(value, context));
        transfer.SetViewTransform(S_IDENTITY, S_IDENTITY);
        transfer.ReadTexture(source);
        transfer.ClearPresentationTarget(default);
        transfer.HasSideEffect();
    }

    private static void DrawFullscreen(
        PassData value,
        RenderPassContext context
    ) {
        context.commands.SetViewport(
            value.viewport.x,
            value.viewport.y,
            value.viewport.width,
            value.viewport.height);
        context.commands.SetScissor(
            value.viewport.x,
            value.viewport.y,
            value.viewport.width,
            value.viewport.height);
        context.commands.BindGraphicsPipeline(value.pipeline);
        context.commands.BindVertexBuffer(value.vertices);
        context.commands.BindIndexBuffer(value.indices);
        context.commands.BindTexture(S_TEXTURE, value.source);
        context.commands.DrawIndexed(6);
    }

    private static byte[] CreateVertices(bool originBottomLeft)
    {
        byte[] bytes = new byte[4 * 20];
        float top = originBottomLeft ? 1f : 0f;
        float bottom = originBottomLeft ? 0f : 1f;
        WriteVertex(0, -1f, 1f, 0f, top);
        WriteVertex(1, 1f, 1f, 1f, top);
        WriteVertex(2, 1f, -1f, 1f, bottom);
        WriteVertex(3, -1f, -1f, 0f, bottom);
        return bytes;

        void WriteVertex(
            int index,
            float x,
            float y,
            float u,
            float v
        ) {
            Span<byte> value = bytes.AsSpan(index * 20, 20);
            BinaryPrimitives.WriteSingleLittleEndian(value, x);
            BinaryPrimitives.WriteSingleLittleEndian(value[4..], y);
            BinaryPrimitives.WriteSingleLittleEndian(value[8..], u);
            BinaryPrimitives.WriteSingleLittleEndian(value[12..], v);
            BinaryPrimitives.WriteUInt32LittleEndian(value[16..], uint.MaxValue);
        }
    }

    private sealed record PassData(
        GraphicsPipelineHandle pipeline,
        PersistentBufferHandle vertices,
        PersistentBufferHandle indices,
        RenderTextureHandle source,
        RenderViewport viewport
    );
}
