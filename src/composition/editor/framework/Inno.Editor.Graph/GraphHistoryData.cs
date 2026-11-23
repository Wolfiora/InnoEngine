using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.IO;
using System.Text;
using Inno.Core.Serialization;
using Inno.Editor.Interactions;

namespace Inno.Editor.Graph;

internal sealed record GraphHistoryData(
    Guid documentID,
    byte[] before,
    byte[] after,
    long timestamp
) {
    /// <summary>
    /// The c kind value used as part of this type's public representation.
    /// </summary>
    public const string C_KIND = "editor.graph.document";

    private static readonly UTF8Encoding UTF8 = new(false, true);

    /// <summary>
    /// Encodes the supplied value into deterministic structured data.
    /// </summary>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public byte[] Encode()
    {
        byte[] id = UTF8.GetBytes(documentID.ToString("D"));
        var result = new byte[checked((sizeof(int) * 3) + sizeof(long) + id.Length + before.Length + after.Length)];
        Span<byte> span = result;
        BinaryPrimitives.WriteInt32LittleEndian(span, id.Length);
        id.CopyTo(span[sizeof(int)..]);
        int offset = sizeof(int) + id.Length;
        BinaryPrimitives.WriteInt32LittleEndian(span[offset..], before.Length);
        offset += sizeof(int);
        before.CopyTo(span[offset..]);
        offset += before.Length;
        BinaryPrimitives.WriteInt32LittleEndian(span[offset..], after.Length);
        offset += sizeof(int);
        after.CopyTo(span[offset..]);
        offset += after.Length;
        BinaryPrimitives.WriteInt64LittleEndian(span[offset..], timestamp);
        return result;
    }

    /// <summary>
    /// Creates and validates a caller-owned change value.
    /// </summary>
    /// <param name="documentID">
    /// The document id text validated by the create change operation.
    /// </param>
    /// <param name="before">
    /// The before consumed by create change; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="after">
    /// The after consumed by create change; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="mergeKey">
    /// The merge key text validated by the create change operation.
    /// </param>
    /// <returns>
    /// The validated editor history change that represents the completed operation.
    /// </returns>
    public static EditorHistoryChange CreateChange(
        Guid documentID,
        ReadOnlySpan<byte> before,
        ReadOnlySpan<byte> after,
        string? mergeKey
    ) {
        var data = new GraphHistoryData(documentID, before.ToArray(), after.ToArray(), Stopwatch.GetTimestamp());
        return new EditorHistoryChange(C_KIND, EditorHistoryPayload.FromBytes(data.Encode()), mergeKey);
    }

    /// <summary>
    /// Decodes deterministic structured data into a validated value.
    /// </summary>
    /// <param name="payload">
    /// The payload consumed by decode; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="serialization">
    /// The serialization consumed by decode; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// The validated graph history data that represents the completed operation.
    /// </returns>
    public static GraphHistoryData Decode(
        ReadOnlySpan<byte> payload,
        SerializationRegistry serialization
    ) {
        int offset = 0;
        int idLength = ReadLength(payload, ref offset);
        Guid id = Guid.ParseExact(UTF8.GetString(ReadSlice(payload, ref offset, idLength)), "D");
        if (id == Guid.Empty)
        {
            throw new InvalidDataException("Graph history requires a persistent document identity.");
        }
        int beforeLength = ReadLength(payload, ref offset);
        byte[] before = ReadSlice(payload, ref offset, beforeLength).ToArray();
        int afterLength = ReadLength(payload, ref offset);
        byte[] after = ReadSlice(payload, ref offset, afterLength).ToArray();
        if (payload.Length - offset != sizeof(long))
        {
            throw new InvalidDataException("Graph History payload has an invalid timestamp boundary.");
        }

        long timestamp = BinaryPrimitives.ReadInt64LittleEndian(payload[offset..]);
        _ = GraphHistoryDocumentCodec.Decode(before, serialization);
        _ = GraphHistoryDocumentCodec.Decode(after, serialization);
        return new GraphHistoryData(id, before, after, timestamp);
    }

    private static int ReadLength(
        ReadOnlySpan<byte> payload,
        ref int offset
    ) {
        if (payload.Length - offset < sizeof(int))
        {
            throw new InvalidDataException("Graph History payload is truncated.");
        }

        int length = BinaryPrimitives.ReadInt32LittleEndian(payload[offset..]);
        offset += sizeof(int);
        if (length < 0 || length > payload.Length - offset)
        {
            throw new InvalidDataException("Graph History payload contains an invalid length.");
        }

        return length;
    }

    private static ReadOnlySpan<byte> ReadSlice(
        ReadOnlySpan<byte> payload,
        ref int offset,
        int length
    ) {
        ReadOnlySpan<byte> result = payload.Slice(offset, length);
        offset += length;
        return result;
    }
}
