using System;
using System.Buffers.Binary;

namespace Inno.Text;

/// <summary>
/// Encodes the compact runtime payload shared by the font importer and text runtime.
/// </summary>
public static class FontMetadataCodec
{
    private const uint C_MAGIC = 0x544E_4649;
    private const int C_SIZE = 16;

    /// <summary>
    /// Encodes validated metadata into a deterministic runtime payload.
    /// </summary>
    /// <param name="metadata">
    /// The metadata to encode.
    /// </param>
    /// <returns>
    /// The compact runtime payload.
    /// </returns>
    public static byte[] Encode(FontMetadata metadata)
    {
        var output = new byte[C_SIZE];
        BinaryPrimitives.WriteUInt32LittleEndian(output, C_MAGIC);
        BinaryPrimitives.WriteInt32LittleEndian(output.AsSpan(4), metadata.faceCount);
        BinaryPrimitives.WriteInt64LittleEndian(output.AsSpan(8), metadata.encodedByteLength);
        return output;
    }

    /// <summary>
    /// Decodes and validates a compact font runtime payload.
    /// </summary>
    /// <param name="payload">
    /// The complete runtime payload.
    /// </param>
    /// <returns>
    /// The decoded metadata.
    /// </returns>
    public static FontMetadata Decode(ReadOnlySpan<byte> payload)
    {
        if (payload.Length != C_SIZE || BinaryPrimitives.ReadUInt32LittleEndian(payload) != C_MAGIC)
        {
            throw new InvalidOperationException("The font runtime payload is invalid.");
        }
        try
        {
            return new FontMetadata(
                BinaryPrimitives.ReadInt32LittleEndian(payload[4..]),
                BinaryPrimitives.ReadInt64LittleEndian(payload[8..])
            );
        }
        catch (ArgumentOutOfRangeException exception)
        {
            throw new InvalidOperationException("The font runtime payload contains invalid metadata.", exception);
        }
    }
}
