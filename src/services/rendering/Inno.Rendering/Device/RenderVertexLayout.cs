using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Identifies one backend-neutral vertex attribute semantic.
/// </summary>
public enum RenderVertexSemantic
{
    /// <summary>
    /// Object-space position.
    /// </summary>
    Position,
    /// <summary>
    /// Object-space normal.
    /// </summary>
    Normal,
    /// <summary>
    /// Object-space tangent and handedness.
    /// </summary>
    Tangent,
    /// <summary>
    /// Object-space bitangent.
    /// </summary>
    Bitangent,
    /// <summary>
    /// Primary vertex color.
    /// </summary>
    Color0,
    /// <summary>
    /// Secondary vertex color.
    /// </summary>
    Color1,
    /// <summary>
    /// Third vertex color channel.
    /// </summary>
    Color2,
    /// <summary>
    /// Fourth vertex color channel.
    /// </summary>
    Color3,
    /// <summary>
    /// Primary texture coordinate.
    /// </summary>
    TextureCoordinate0,
    /// <summary>
    /// Secondary texture coordinate.
    /// </summary>
    TextureCoordinate1,
    /// <summary>
    /// Third texture coordinate.
    /// </summary>
    TextureCoordinate2,
    /// <summary>
    /// Fourth texture coordinate.
    /// </summary>
    TextureCoordinate3,
    /// <summary>
    /// Fifth texture coordinate.
    /// </summary>
    TextureCoordinate4,
    /// <summary>
    /// Sixth texture coordinate.
    /// </summary>
    TextureCoordinate5,
    /// <summary>
    /// Seventh texture coordinate.
    /// </summary>
    TextureCoordinate6,
    /// <summary>
    /// Eighth texture coordinate.
    /// </summary>
    TextureCoordinate7,
    /// <summary>
    /// Skinning indices.
    /// </summary>
    BlendIndices,
    /// <summary>
    /// Skinning weights.
    /// </summary>
    BlendWeights
}

/// <summary>
/// Identifies one packed vertex attribute representation.
/// </summary>
public enum RenderVertexFormat
{
    /// <summary>
    /// One 32-bit floating-point component.
    /// </summary>
    Float1,
    /// <summary>
    /// Two 32-bit floating-point components.
    /// </summary>
    Float2,
    /// <summary>
    /// Three 32-bit floating-point components.
    /// </summary>
    Float3,
    /// <summary>
    /// Four 32-bit floating-point components.
    /// </summary>
    Float4,
    /// <summary>
    /// Two 16-bit floating-point components.
    /// </summary>
    Half2,
    /// <summary>
    /// Four 16-bit floating-point components.
    /// </summary>
    Half4,
    /// <summary>
    /// Four normalized unsigned bytes.
    /// </summary>
    UInt8Normalized4,
    /// <summary>
    /// Two normalized unsigned bytes.
    /// </summary>
    UInt8Normalized2,
    /// <summary>
    /// Four unsigned bytes interpreted as integers.
    /// </summary>
    UInt8Integer4,
    /// <summary>
    /// Two unsigned bytes interpreted as integers.
    /// </summary>
    UInt8Integer2,
    /// <summary>
    /// Four normalized unsigned components packed into 10:10:10:2 bits.
    /// </summary>
    UInt10Normalized4,
    /// <summary>
    /// Two normalized signed 16-bit components.
    /// </summary>
    Int16Normalized2,
    /// <summary>
    /// Four normalized signed 16-bit components.
    /// </summary>
    Int16Normalized4,
    /// <summary>
    /// Two signed 16-bit components interpreted as integers.
    /// </summary>
    Int16Integer2,
    /// <summary>
    /// Four signed 16-bit components interpreted as integers.
    /// </summary>
    Int16Integer4
}

/// <summary>
/// Describes one vertex attribute in stream order.
/// </summary>
public readonly record struct RenderVertexAttribute
{
    /// <summary>
    /// Creates a vertex attribute declaration.
    /// </summary>
    /// <param name="semantic">
    /// Shader input semantic.
    /// </param>
    /// <param name="format">
    /// Packed component representation.
    /// </param>
    /// <param name="byteOffset">
    /// Explicit byte offset in the stream, or -1 to place the attribute directly after the preceding attribute.
    /// </param>
    public RenderVertexAttribute(
        RenderVertexSemantic semantic,
        RenderVertexFormat format,
        int byteOffset = -1
    ) {
        if (byteOffset < -1)
            throw new ArgumentOutOfRangeException(nameof(byteOffset));
        this.semantic = semantic;
        this.format = format;
        this.byteOffset = byteOffset;
    }

    /// <summary>
    /// Gets the shader input semantic.
    /// </summary>
    public RenderVertexSemantic semantic { get; }

    /// <summary>
    /// Gets the packed component representation.
    /// </summary>
    public RenderVertexFormat format { get; }

    /// <summary>
    /// Gets the explicit byte offset in the resolved stream layout, or -1 before a layout resolves automatic placement.
    /// </summary>
    public int byteOffset { get; }

    /// <summary>
    /// Gets the packed byte size.
    /// </summary>
    public int byteSize => format switch
    {
        RenderVertexFormat.Float1 => 4,
        RenderVertexFormat.Float2 => 8,
        RenderVertexFormat.Float3 => 12,
        RenderVertexFormat.Float4 => 16,
        RenderVertexFormat.Half2 => 4,
        RenderVertexFormat.Half4 => 8,
        RenderVertexFormat.UInt8Normalized2 => 2,
        RenderVertexFormat.UInt8Normalized4 => 4,
        RenderVertexFormat.UInt8Integer2 => 2,
        RenderVertexFormat.UInt8Integer4 => 4,
        RenderVertexFormat.UInt10Normalized4 => 4,
        RenderVertexFormat.Int16Normalized2 => 4,
        RenderVertexFormat.Int16Normalized4 => 8,
        RenderVertexFormat.Int16Integer2 => 4,
        RenderVertexFormat.Int16Integer4 => 8,
        _ => throw new ArgumentOutOfRangeException(nameof(format))
    };
}

/// <summary>
/// Defines one interleaved vertex stream independently from a graphics backend.
/// </summary>
public sealed class RenderVertexLayout : IEquatable<RenderVertexLayout>
{
    private readonly IReadOnlyList<RenderVertexAttribute> m_attributes;

    /// <summary>
    /// Creates an interleaved vertex layout.
    /// </summary>
    /// <param name="attributes">
    /// Unique attributes in ascending byte order. Attributes with offset -1 are packed after the preceding attribute.
    /// </param>
    /// <param name="stride">
    /// Explicit positive stream stride, or zero to use the end of the final attribute. A larger stride preserves
    /// trailing application-defined padding.
    /// </param>
    public RenderVertexLayout(
        IReadOnlyList<RenderVertexAttribute> attributes,
        int stride = 0
    ) {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentOutOfRangeException.ThrowIfNegative(stride);
        if (attributes.Count == 0)
        {
            throw new ArgumentException("A vertex layout requires at least one attribute.", nameof(attributes));
        }

        if (attributes.Select(static value => value.semantic).Distinct().Count() != attributes.Count)
        {
            throw new ArgumentException("A vertex layout cannot repeat a semantic.", nameof(attributes));
        }

        var resolved = new RenderVertexAttribute[attributes.Count];
        int occupiedEnd = 0;
        for (int index = 0; index < attributes.Count; index++)
        {
            RenderVertexAttribute attribute = attributes[index];
            int offset = attribute.byteOffset < 0 ? occupiedEnd : attribute.byteOffset;
            if (offset < occupiedEnd)
            {
                throw new ArgumentException(
                    $"Vertex attribute '{attribute.semantic}' overlaps a preceding attribute.",
                    nameof(attributes));
            }

            resolved[index] = new RenderVertexAttribute(attribute.semantic, attribute.format, offset);
            occupiedEnd = checked(offset + attribute.byteSize);
        }

        if (stride != 0 && stride < occupiedEnd)
        {
            throw new ArgumentException(
                "Vertex stride cannot end before the final attribute.",
                nameof(stride));
        }

        m_attributes = Array.AsReadOnly(resolved);
        this.stride = stride == 0 ? occupiedEnd : stride;
    }

    /// <summary>
    /// Gets attributes in byte-stream order.
    /// </summary>
    public IReadOnlyList<RenderVertexAttribute> attributes => m_attributes;

    /// <summary>
    /// Gets the interleaved vertex stride in bytes.
    /// </summary>
    public int stride { get; }

    /// <summary>
    /// Determines whether this instance and the supplied value represent the same logical state.
    /// </summary>
    /// <param name="other">
    /// The value to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when both values represent the same logical state; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(RenderVertexLayout? other)
        => other is not null
            && stride == other.stride
            && m_attributes.SequenceEqual(other.m_attributes);

    /// <summary>
    /// Determines whether this instance and the supplied value represent the same logical state.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when both values represent the same logical state; otherwise, <see langword="false"/>.
    /// </returns>
    /// <param name="obj">
    /// The object to compare with this instance.
    /// </param>
    public override bool Equals(object? obj) => Equals(obj as RenderVertexLayout);

    /// <summary>
    /// Computes a hash code from the fields that participate in logical equality.
    /// </summary>
    /// <returns>
    /// A hash code consistent with the implemented equality contract.
    /// </returns>
    public override int GetHashCode()
    {
        HashCode hash = new();
        foreach (RenderVertexAttribute attribute in m_attributes)
        {
            hash.Add(attribute);
        }

        hash.Add(stride);

        return hash.ToHashCode();
    }
}

/// <summary>
/// Selects the integer representation of an index buffer.
/// </summary>
public enum RenderIndexFormat
{
    /// <summary>
    /// Unsigned 16-bit indices.
    /// </summary>
    UInt16,
    /// <summary>
    /// Unsigned 32-bit indices.
    /// </summary>
    UInt32
}

/// <summary>
/// Selects the primitive assembly used by raster draw commands.
/// </summary>
public enum RenderPrimitiveTopology
{
    /// <summary>
    /// Independent triangle triplets.
    /// </summary>
    TriangleList,
    /// <summary>
    /// Connected triangle strip.
    /// </summary>
    TriangleStrip,
    /// <summary>
    /// Independent line pairs.
    /// </summary>
    LineList,
    /// <summary>
    /// Connected line strip.
    /// </summary>
    LineStrip,
    /// <summary>
    /// Independent points.
    /// </summary>
    PointList
}

