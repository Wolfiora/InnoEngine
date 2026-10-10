using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;
using Inno.Core.Serialization;

namespace Inno.Runtime;

/// <summary>
/// Frames the serialized runtime manifest with the application identity required before engine startup.
/// </summary>
public static class RuntimeManifestEnvelope
{
    private static ReadOnlySpan<byte> magic => "INNORTM\0"u8;

    /// <summary>
    /// Encodes a validated runtime manifest into the strict deployment envelope.
    /// </summary>
    /// <param name="manifest">
    /// The manifest whose application identity and serialized payload must agree.
    /// </param>
    /// <param name="serialization">
    /// The immutable converter generation captured for the surrounding build transaction.
    /// </param>
    /// <returns>
    /// The complete deterministic deployment envelope.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="manifest"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// Thrown when the manifest is not valid for Player startup.
    /// </exception>
    public static byte[] Encode(
        GameRuntimeManifest manifest,
        SerializationGeneration serialization
    ) {
        ArgumentNullException.ThrowIfNull(manifest);
        ArgumentNullException.ThrowIfNull(serialization);
        manifest.Validate();
        byte[] applicationId = Encoding.UTF8.GetBytes(manifest.applicationId);
        byte[] persistentDataPath = Encoding.UTF8.GetBytes(
            manifest.persistentDataPath.Length == 0 ? manifest.applicationId : manifest.persistentDataPath);
        byte[] payload = serialization.Serialize(manifest);
        byte[] result = new byte[checked(magic.Length + sizeof(int) * 3 +
            applicationId.Length + persistentDataPath.Length + payload.Length)];
        int offset = 0;
        magic.CopyTo(result);
        offset += magic.Length;
        BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan(offset), applicationId.Length);
        offset += sizeof(int);
        applicationId.CopyTo(result, offset);
        offset += applicationId.Length;
        BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan(offset), persistentDataPath.Length);
        offset += sizeof(int);
        persistentDataPath.CopyTo(result, offset);
        offset += persistentDataPath.Length;
        BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan(offset), payload.Length);
        offset += sizeof(int);
        payload.CopyTo(result, offset);
        return result;
    }

    /// <summary>
    /// Reads and validates the application identity without requiring serialization services to be initialized.
    /// </summary>
    /// <param name="data">
    /// The complete runtime manifest envelope.
    /// </param>
    /// <returns>
    /// The stable application identity declared by the Player build.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// Thrown when the envelope is truncated, malformed, or contains an invalid application identity.
    /// </exception>
    public static string ReadApplicationId(ReadOnlySpan<byte> data)
    {
        Parse(data, out string applicationId, out _, out _);
        ValidateApplicationId(applicationId);
        return applicationId;
    }

    /// <summary>
    /// Reads the validated writable data folder before engine serialization is initialized.
    /// </summary>
    /// <param name="data">
    /// The complete runtime manifest envelope.
    /// </param>
    /// <returns>
    /// A portable folder path relative to the operating system's local application data directory.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// Thrown when the envelope or persistent data path is malformed.
    /// </exception>
    public static string ReadPersistentDataPath(ReadOnlySpan<byte> data)
    {
        Parse(data, out string applicationId, out string persistentDataPath, out _);
        ValidateApplicationId(applicationId);
        ValidatePersistentDataPath(persistentDataPath);
        return persistentDataPath;
    }

    /// <summary>
    /// Deserializes and validates the complete runtime manifest after engine serialization is available.
    /// </summary>
    /// <param name="data">
    /// The complete runtime manifest envelope.
    /// </param>
    /// <param name="serialization">
    /// The immutable converter generation selected for Player startup.
    /// </param>
    /// <returns>
    /// The validated runtime manifest.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// Thrown when the envelope or manifest is malformed, or when the framed identity disagrees with the payload.
    /// </exception>
    public static GameRuntimeManifest Decode(
        ReadOnlySpan<byte> data,
        SerializationGeneration serialization
    ) {
        ArgumentNullException.ThrowIfNull(serialization);
        Parse(data, out string applicationId, out string persistentDataPath, out ReadOnlySpan<byte> payload);
        ValidateApplicationId(applicationId);
        ValidatePersistentDataPath(persistentDataPath);
        GameRuntimeManifest manifest = serialization.Deserialize<GameRuntimeManifest>(payload);
        manifest.Validate();
        if (!string.Equals(applicationId, manifest.applicationId, StringComparison.Ordinal))
            throw new InvalidDataException("Runtime manifest application identities do not match.");
        string expectedPath = manifest.persistentDataPath.Length == 0
            ? manifest.applicationId : manifest.persistentDataPath;
        if (!string.Equals(persistentDataPath, expectedPath, StringComparison.Ordinal))
            throw new InvalidDataException("Runtime manifest persistent data paths do not match.");
        return manifest;
    }

    private static void Parse(
        ReadOnlySpan<byte> data,
        out string applicationId,
        out string persistentDataPath,
        out ReadOnlySpan<byte> payload
    ) {
        int minimumLength = magic.Length + sizeof(int) * 3;
        if (data.Length < minimumLength || !data[..magic.Length].SequenceEqual(magic))
            throw new InvalidDataException("Runtime manifest envelope has an invalid header.");
        int offset = magic.Length;
        int applicationIdLength = BinaryPrimitives.ReadInt32LittleEndian(data[offset..]);
        offset += sizeof(int);
        if (applicationIdLength <= 0 || applicationIdLength > 255 || applicationIdLength > data.Length - offset - sizeof(int) * 2)
            throw new InvalidDataException("Runtime manifest envelope has an invalid application identity length.");
        try
        {
            applicationId = new UTF8Encoding(false, true).GetString(data.Slice(offset, applicationIdLength));
        }
        catch (DecoderFallbackException exception)
        {
            throw new InvalidDataException("Runtime manifest application identity is not valid UTF-8.", exception);
        }
        offset += applicationIdLength;
        int persistentDataPathLength = BinaryPrimitives.ReadInt32LittleEndian(data[offset..]);
        offset += sizeof(int);
        if (persistentDataPathLength <= 0 || persistentDataPathLength > 1024 ||
            persistentDataPathLength > data.Length - offset - sizeof(int))
        {
            throw new InvalidDataException("Runtime manifest envelope has an invalid persistent data path length.");
        }
        try
        {
            persistentDataPath = new UTF8Encoding(false, true).GetString(data.Slice(offset, persistentDataPathLength));
        }
        catch (DecoderFallbackException exception)
        {
            throw new InvalidDataException("Runtime manifest persistent data path is not valid UTF-8.", exception);
        }
        offset += persistentDataPathLength;
        int payloadLength = BinaryPrimitives.ReadInt32LittleEndian(data[offset..]);
        offset += sizeof(int);
        if (payloadLength <= 0 || payloadLength != data.Length - offset)
            throw new InvalidDataException("Runtime manifest envelope has an invalid payload length.");
        payload = data[offset..];
    }

    private static void ValidateApplicationId(string applicationId)
    {
        if (string.IsNullOrWhiteSpace(applicationId))
            throw new InvalidDataException("Runtime manifest requires an application identity.");
        foreach (char character in applicationId)
        {
            if (!(character is >= 'a' and <= 'z'
                  || character is >= '0' and <= '9'
                  || character is '.' or '_' or '-'))
            {
                throw new InvalidDataException("Runtime manifest contains an invalid application identity.");
            }
        }
    }

    internal static void ValidatePersistentDataPath(string persistentDataPath)
    {
        if (persistentDataPath is null)
            throw new InvalidDataException("Runtime manifest requires a persistent data path value.");
        if (persistentDataPath.Length == 0)
            return;
        foreach (string segment in persistentDataPath.Split('/'))
        {
            string stem = segment.Split('.', 2)[0];
            if (segment is "." or ".." || string.IsNullOrEmpty(segment) || segment.EndsWith('.')
                || stem is "con" or "prn" or "aux" or "nul"
                || (stem.Length == 4 && (stem.StartsWith("com", StringComparison.Ordinal)
                    || stem.StartsWith("lpt", StringComparison.Ordinal)) && stem[3] is >= '1' and <= '9'))
                throw new InvalidDataException("Runtime manifest contains an invalid persistent data folder.");
            ValidateApplicationId(segment);
        }
    }
}
