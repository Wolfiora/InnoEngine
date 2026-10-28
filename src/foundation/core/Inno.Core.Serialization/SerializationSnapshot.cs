using System;
using System.IO;

namespace Inno.Core.Serialization;

/// <summary>
/// Holds an immutable, neutral capture of serialized values without retaining the source object or its generation.
/// </summary>
/// <remarks>
/// Capture again through <see cref="SerializationRegistry.CaptureSnapshot{T}"/> to reuse unchanged values.
/// Readers may share a snapshot across threads. It owns only scalar values, strings and copied binary data,
/// never source instances, metadata, converters or delegates. It is transient and is not a persistence format.
/// </remarks>
public sealed class SerializationSnapshot
{
    internal SerializationSnapshot(SerializationNode root) => this.root = root;

    /// <summary>
    /// Encodes this capture using the same deterministic binary format as ordinary serialization.
    /// </summary>
    /// <returns>
    /// A new caller-owned byte array; changing it cannot modify this snapshot.
    /// </returns>
    public byte[] ToArray() => BinarySerializationFormat.Encode(root);

    /// <summary>
    /// Streams this capture in the ordinary deterministic binary format without allocating the complete output payload.
    /// </summary>
    /// <param name="destination">
    /// The writable caller-owned stream, which remains open.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The destination is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The destination cannot be written.
    /// </exception>
    /// <exception cref="IOException">
    /// The destination rejects a write; its partial bytes remain the caller's responsibility.
    /// </exception>
    public void WriteTo(Stream destination)
    {
        ArgumentNullException.ThrowIfNull(destination);
        if (!destination.CanWrite)
            throw new ArgumentException("A writable serialization destination is required.", nameof(destination));
        BinarySerializationFormat.WriteTo(destination, root);
    }

    internal SerializationNode root { get; }
}
