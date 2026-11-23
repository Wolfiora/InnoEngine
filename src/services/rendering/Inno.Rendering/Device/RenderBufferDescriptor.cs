using System;

namespace Inno.Rendering;

/// <summary>
/// Describes a render-graph buffer independently from a graphics backend.
/// </summary>
public sealed class RenderBufferDescriptor : IEquatable<RenderBufferDescriptor>
{
    /// <summary>
    /// Creates a buffer descriptor.
    /// </summary>
    /// <param name="elementCount">
    /// Number of addressable elements.
    /// </param>
    /// <param name="elementStride">
    /// Element size in bytes.
    /// </param>
    /// <param name="usage">
    /// Permitted operations.
    /// </param>
    public RenderBufferDescriptor(
        int elementCount,
        int elementStride,
        RenderBufferUsage usage
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(elementCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(elementStride);
        this.elementCount = elementCount;
        this.elementStride = elementStride;
        this.usage = usage;
    }

    /// <summary>
    /// Gets the number of addressable elements.
    /// </summary>
    public int elementCount { get; }

    /// <summary>
    /// Gets the element size in bytes.
    /// </summary>
    public int elementStride { get; }

    /// <summary>
    /// Gets permitted operations.
    /// </summary>
    public RenderBufferUsage usage { get; }

    /// <summary>
    /// Determines whether this instance and the supplied value represent the same logical state.
    /// </summary>
    /// <param name="other">
    /// The value to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when both values represent the same logical state; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(RenderBufferDescriptor? other)
        => other is not null
           && elementCount == other.elementCount
           && elementStride == other.elementStride
           && usage == other.usage;

    /// <summary>
    /// Determines whether this instance and the supplied value represent the same logical state.
    /// </summary>
    /// <param name="obj">
    /// The object to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when both values represent the same logical state; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj) => Equals(obj as RenderBufferDescriptor);

    /// <summary>
    /// Computes a hash code from the fields that participate in logical equality.
    /// </summary>
    /// <returns>
    /// A hash code consistent with the implemented equality contract.
    /// </returns>
    public override int GetHashCode() => HashCode.Combine(elementCount, elementStride, usage);
}
