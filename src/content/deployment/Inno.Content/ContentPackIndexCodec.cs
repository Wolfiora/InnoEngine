using System;
using System.IO;
using System.Linq;
using Inno.Core.Serialization;

namespace Inno.Content;

/// <summary>
/// Encodes the current immutable content inventory through the common serialization protocol.
/// </summary>
public static class ContentPackIndexCodec
{
    /// <summary>
    /// Captures the entire index with the operation's pinned converter generation.
    /// </summary>
    /// <param name="index">
    /// The validated immutable inventory.
    /// </param>
    /// <param name="serialization">
    /// The borrowed generation shared by the enclosing build transaction.
    /// </param>
    /// <returns>
    /// Newly owned deterministic index bytes.
    /// </returns>
    public static byte[] Encode(
        ContentPackIndex index,
        SerializationGeneration serialization
    ) {
        ArgumentNullException.ThrowIfNull(index);
        ArgumentNullException.ThrowIfNull(serialization);
        return serialization.Encode(writer => writer.WriteObjectArray("entries", index.entries,
            static (
                entryWriter,
                entry
            ) => {
                entryWriter.Write("key", entry.key.value!);
                entryWriter.Write("length", entry.length);
                entryWriter.Write("hash", entry.contentHash);
            }));
    }

    /// <summary>
    /// Restores and validates one current-format payload inventory.
    /// </summary>
    /// <param name="data">
    /// The complete structured index bytes.
    /// </param>
    /// <param name="serialization">
    /// The borrowed converter generation pinned by the enclosing preparation operation.
    /// </param>
    /// <returns>
    /// A newly owned immutable inventory with no duplicate or ambiguous identities.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The bytes or inventory are malformed; no partial index is returned.
    /// </exception>
    public static ContentPackIndex Decode(
        ReadOnlySpan<byte> data,
        SerializationGeneration serialization
    ) {
        ArgumentNullException.ThrowIfNull(serialization);
        try
        {
            return serialization.Decode(data, reader => new ContentPackIndex(reader.ReadObjectArray("entries")
                .Select(static entry => new ContentEntry(new ContentKey(entry.Read<string>("key")),
                    entry.Read<long>("length"), entry.Read<string>("hash")))));
        }
        catch (Exception failure) when (failure is ArgumentException or InvalidOperationException or OverflowException)
        {
            throw new InvalidDataException("The content inventory is malformed.", failure);
        }
    }
}
