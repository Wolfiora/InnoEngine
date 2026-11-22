using System;

namespace Inno.Text;

/// <summary>
/// Describes immutable metadata for an imported OpenType font source.
/// </summary>
public readonly record struct FontMetadata
{
    /// <summary>
    /// Creates validated imported font metadata.
    /// </summary>
    /// <param name="faceCount">
    /// The number of faces in the source collection.
    /// </param>
    /// <param name="encodedByteLength">
    /// The encoded source length in bytes.
    /// </param>
    public FontMetadata(
        int faceCount,
        long encodedByteLength
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(faceCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(encodedByteLength);
        this.faceCount = faceCount;
        this.encodedByteLength = encodedByteLength;
    }

    /// <summary>
    /// Gets the number of independently addressable faces in the source.
    /// </summary>
    public int faceCount { get; }

    /// <summary>
    /// Gets the encoded source length in bytes.
    /// </summary>
    public long encodedByteLength { get; }
}
