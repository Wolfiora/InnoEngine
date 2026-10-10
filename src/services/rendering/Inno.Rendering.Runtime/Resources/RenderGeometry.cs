using Inno.Core.Diagnostics;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Provides generation-scoped GPU buffers for an imported geometry helper asset.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("6ad78a05-1eae-5e6f-a5d2-ab40b6c35075")]
public sealed class RenderGeometry
{
    private readonly IReadOnlyList<RenderGeometrySection> m_sections;

    internal RenderGeometry(
        PersistentBufferHandle vertexBuffer,
        PersistentBufferHandle indexBuffer,
        RenderVertexLayout vertexLayout,
        int vertexCount,
        int indexCount,
        IReadOnlyList<RenderGeometrySection> sections
    ) {
        this.vertexBuffer = vertexBuffer;
        this.indexBuffer = indexBuffer;
        this.vertexLayout = vertexLayout;
        this.vertexCount = vertexCount;
        this.indexCount = indexCount;
        m_sections = Array.AsReadOnly(sections.ToArray());
    }

    /// <summary>
    /// Gets the persistent vertex buffer.
    /// </summary>
    public PersistentBufferHandle vertexBuffer { get; }

    /// <summary>
    /// Gets the persistent index buffer.
    /// </summary>
    public PersistentBufferHandle indexBuffer { get; }

    /// <summary>
    /// Gets the imported interleaved vertex layout.
    /// </summary>
    public RenderVertexLayout vertexLayout { get; }

    /// <summary>
    /// Gets the number of vertices.
    /// </summary>
    public int vertexCount { get; }

    /// <summary>
    /// Gets the total index count.
    /// </summary>
    public int indexCount { get; }

    /// <summary>
    /// Gets independently drawable indexed ranges.
    /// </summary>
    public IReadOnlyList<RenderGeometrySection> sections => m_sections;

    /// <summary>
    /// Binds both geometry streams at their first element.
    /// </summary>
    /// <param name="commands">
    /// Current pass command encoder.
    /// </param>
    public void Bind(RenderCommandEncoder commands)
    {
        ArgumentNullException.ThrowIfNull(commands);
        commands.BindVertexBuffer(vertexBuffer);
        commands.BindIndexBuffer(indexBuffer);
    }

    /// <summary>
    /// Binds and draws one indexed section.
    /// </summary>
    /// <param name="commands">
    /// Current raster pass command encoder.
    /// </param>
    /// <param name="sectionIndex">
    /// Zero-based section index.
    /// </param>
    /// <param name="instanceCount">
    /// Positive instance count.
    /// </param>
    public void DrawSection(
        RenderCommandEncoder commands,
        int sectionIndex,
        int instanceCount = 1
    ) {
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(instanceCount);
        if ((uint)sectionIndex >= (uint)m_sections.Count)
            throw new ArgumentOutOfRangeException(nameof(sectionIndex));
        RenderGeometrySection section = m_sections[sectionIndex];
        commands.BindVertexBuffer(vertexBuffer);
        commands.BindIndexBuffer(indexBuffer, section.firstIndex);
        commands.DrawIndexed(section.indexCount, instanceCount);
    }
}

