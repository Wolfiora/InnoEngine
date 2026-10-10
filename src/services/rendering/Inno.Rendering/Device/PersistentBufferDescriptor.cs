using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Describes a persistent buffer and any vertex/index interpretation required at creation.
/// </summary>
public sealed class PersistentBufferDescriptor
{
    /// <summary>
    /// Creates a persistent buffer descriptor.
    /// </summary>
    /// <param name="buffer">
    /// Capacity and usage.
    /// </param>
    /// <param name="vertexLayout">
    /// Required interleaved layout for vertex buffers.
    /// </param>
    /// <param name="indexFormat">
    /// Index representation for index buffers.
    /// </param>
    public PersistentBufferDescriptor(
        RenderBufferDescriptor buffer,
        RenderVertexLayout? vertexLayout = null,
        RenderIndexFormat indexFormat = RenderIndexFormat.UInt32
    ) {
        ArgumentNullException.ThrowIfNull(buffer);
        if (buffer.usage == 0)
        {
            throw new ArgumentException("A persistent buffer requires at least one usage.", nameof(buffer));
        }

        if ((buffer.usage & (RenderBufferUsage.Vertex | RenderBufferUsage.Index))
            == (RenderBufferUsage.Vertex | RenderBufferUsage.Index))
        {
            throw new ArgumentException("A buffer cannot be both vertex and index input.", nameof(buffer));
        }

        if ((buffer.usage & RenderBufferUsage.Vertex) != 0 && vertexLayout is null)
        {
            throw new ArgumentException("A vertex buffer requires an interleaved layout.", nameof(vertexLayout));
        }

        if (vertexLayout is not null && vertexLayout.stride != buffer.elementStride)
        {
            throw new ArgumentException("Vertex layout stride must match buffer element stride.", nameof(vertexLayout));
        }

        int expectedIndexStride = indexFormat == RenderIndexFormat.UInt16 ? 2 : 4;
        if ((buffer.usage & RenderBufferUsage.Index) != 0
            && buffer.elementStride != expectedIndexStride)
        {
            throw new ArgumentException("Index format must match buffer element stride.", nameof(indexFormat));
        }

        this.buffer = buffer;
        this.vertexLayout = vertexLayout;
        this.indexFormat = indexFormat;
    }

    /// <summary>
    /// Gets buffer capacity and usage.
    /// </summary>
    public RenderBufferDescriptor buffer { get; }

    /// <summary>
    /// Gets the vertex layout when this is a vertex buffer.
    /// </summary>
    public RenderVertexLayout? vertexLayout { get; }

    /// <summary>
    /// Gets the index representation when this is an index buffer.
    /// </summary>
    public RenderIndexFormat indexFormat { get; }
}

