using System;
using System.Buffers.Binary;
using System.Text;

namespace Inno.Audio;

/// <summary>
/// Encodes the compact runtime header shared by audio importers and runtime asset loading.
/// </summary>
public static class AudioClipMetadataCodec
{
    private const uint C_MAGIC = 0x4455_4149;
    private const int C_FIXED_SIZE = 32;

    /// <summary>
    /// Encodes audio metadata into a compact deterministic runtime header.
    /// </summary>
    /// <param name="metadata">
    /// Valid metadata to encode.
    /// </param>
    /// <returns>
    /// Newly allocated runtime header bytes.
    /// </returns>
    public static byte[] Encode(AudioClipMetadata metadata)
    {
        byte[] codec = Encoding.UTF8.GetBytes(metadata.codec.value);
        if (codec.Length > byte.MaxValue)
        {
            throw new ArgumentException("The codec identifier is too long.", nameof(metadata));
        }
        var output = new byte[C_FIXED_SIZE + codec.Length];
        BinaryPrimitives.WriteUInt32LittleEndian(output.AsSpan(0, 4), C_MAGIC);
        output[4] = (byte)codec.Length;
        BinaryPrimitives.WriteInt32LittleEndian(output.AsSpan(8, 4), metadata.channels);
        BinaryPrimitives.WriteInt32LittleEndian(output.AsSpan(12, 4), metadata.sampleRate);
        BinaryPrimitives.WriteInt64LittleEndian(output.AsSpan(16, 8), metadata.frameCount);
        BinaryPrimitives.WriteInt64LittleEndian(output.AsSpan(24, 8), metadata.encodedByteLength);
        codec.CopyTo(output.AsSpan(C_FIXED_SIZE));
        return output;
    }

    /// <summary>
    /// Decodes and validates one compact audio runtime header.
    /// </summary>
    /// <param name="payload">
    /// Complete runtime header bytes.
    /// </param>
    /// <returns>
    /// Valid imported audio metadata.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the header is truncated, corrupt, or semantically invalid.
    /// </exception>
    public static AudioClipMetadata Decode(ReadOnlySpan<byte> payload)
    {
        if (payload.Length < C_FIXED_SIZE || BinaryPrimitives.ReadUInt32LittleEndian(payload) != C_MAGIC)
        {
            throw new InvalidOperationException("The audio runtime header is invalid.");
        }
        int codecLength = payload[4];
        if (payload.Length != C_FIXED_SIZE + codecLength || codecLength == 0)
        {
            throw new InvalidOperationException("The audio runtime header length is invalid.");
        }
        try
        {
            return new AudioClipMetadata(
                new AudioCodecID(Encoding.UTF8.GetString(payload[C_FIXED_SIZE..])),
                BinaryPrimitives.ReadInt32LittleEndian(payload[8..12]),
                BinaryPrimitives.ReadInt32LittleEndian(payload[12..16]),
                BinaryPrimitives.ReadInt64LittleEndian(payload[16..24]),
                BinaryPrimitives.ReadInt64LittleEndian(payload[24..32])
            );
        }
        catch (ArgumentException exception)
        {
            throw new InvalidOperationException("The audio runtime header contains invalid metadata.", exception);
        }
    }
}
