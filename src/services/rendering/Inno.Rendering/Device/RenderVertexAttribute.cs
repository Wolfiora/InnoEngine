using System;

namespace Inno.Rendering;

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
        ArgumentOutOfRangeException.ThrowIfLessThan(byteOffset, -1);
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
    public int byteSize
        => format switch
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
