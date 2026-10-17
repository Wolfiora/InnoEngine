using Inno.Core.Mathematics;
using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace Inno.Rendering.Assets;

/// <summary>
/// Contains normalized CPU mesh data ready for backend upload.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("9590e0d3-f8d2-58b1-9288-d0a20448ceb6")]
public sealed class GeometryData
{
    /// <summary>
    /// Creates normalized mesh data.
    /// </summary>
    /// <param name="vertices">
    /// Vertex stream.
    /// </param>
    /// <param name="indices">
    /// Triangle index stream.
    /// </param>
    /// <param name="sections">
    /// Contiguous submesh ranges.
    /// </param>
    public GeometryData(
        GeometryVertex[] vertices,
        uint[] indices,
        GeometrySection[] sections
    ) {
        ArgumentNullException.ThrowIfNull(vertices);
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(sections);
        this.vertices = Array.AsReadOnly((GeometryVertex[])vertices.Clone());
        this.indices = Array.AsReadOnly((uint[])indices.Clone());
        this.sections = Array.AsReadOnly((GeometrySection[])sections.Clone());
    }

    /// <summary>
    /// Gets the normalized vertex stream.
    /// </summary>
    public IReadOnlyList<GeometryVertex> vertices { get; }

    /// <summary>
    /// Gets the triangle index stream.
    /// </summary>
    public IReadOnlyList<uint> indices { get; }

    /// <summary>
    /// Gets contiguous submesh ranges.
    /// </summary>
    public IReadOnlyList<GeometrySection> sections { get; }
}

