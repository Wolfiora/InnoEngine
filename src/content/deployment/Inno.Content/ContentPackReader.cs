using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading;
using Inno.Core.Serialization;

namespace Inno.Content;

/// <summary>
/// Verifies complete encoded and decoded identities before transferring a pack into an immutable store.
/// </summary>
public static class ContentPackReader
{
    /// <summary>
    /// Takes ownership of a readable seekable pack and validates all bytes, entries, and budgets.
    /// </summary>
    /// <param name="pack">
    /// The owned stream; callers must not access it after this call. Failure also disposes it.
    /// </param>
    /// <param name="descriptor">
    /// The expected complete-pack identity received through validated deployment metadata.
    /// </param>
    /// <param name="serialization">
    /// The borrowed converter generation used to decode the common content index.
    /// </param>
    /// <param name="limits">
    /// Explicit finite reading and expansion budgets; null selects the documented defaults.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels validation before a store is returned.
    /// </param>
    /// <returns>
    /// A caller-owned store that retains the stream until every lease and stream has retired.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The encoded hash, index, entry inventory, decoded bytes, or reading budgets are invalid.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Validation was canceled; the input stream is disposed.
    /// </exception>
    public static PackContentStore Open(
        Stream pack,
        ContentPackDescriptor descriptor,
        SerializationGeneration serialization,
        ContentReadLimits? limits = null,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(pack);
        ZipArchive? archive = null;
        try
        {
            ArgumentNullException.ThrowIfNull(descriptor);
            ArgumentNullException.ThrowIfNull(serialization);
            limits ??= new ContentReadLimits();
            cancellationToken.ThrowIfCancellationRequested();
            if (!pack.CanRead || !pack.CanSeek)
                throw new ArgumentException("A content pack requires an owned readable seekable stream.", nameof(pack));
            if (pack.Length > limits.packBytes)
                throw new InvalidDataException("The encoded content pack exceeds its reading budget.");
            pack.Position = 0;
            string actualHash = Hash(pack, pack.Length, cancellationToken);
            if (!string.Equals(actualHash, descriptor.contentHash, StringComparison.Ordinal))
                throw new InvalidDataException("The complete pack bytes do not match their content identity.");
            pack.Position = 0;
            archive = new ZipArchive(pack, ZipArchiveMode.Read, leaveOpen: false);
            Dictionary<ContentKey, ZipArchiveEntry> payloads = ReadInventory(archive, limits);
            ContentKey indexKey = new(ContentPackIndex.C_INDEX_KEY);
            if (!payloads.Remove(indexKey, out ZipArchiveEntry? indexEntry))
                throw new InvalidDataException("The content pack has no inventory document.");
            if (indexEntry.Length > limits.indexBytes)
                throw new InvalidDataException("The content inventory exceeds its reading budget.");
            byte[] indexBytes = new byte[checked((int)indexEntry.Length)];
            using (Stream indexStream = indexEntry.Open())
            {
                indexStream.ReadExactly(indexBytes);
                if (indexStream.ReadByte() != -1)
                    throw new InvalidDataException("The content inventory exceeds its declared length.");
            }
            ContentPackIndex index = ContentPackIndexCodec.Decode(indexBytes, serialization);
            if (index.entries.Count != payloads.Count || index.entries.Count > limits.entryCount
                || index.totalLength > limits.totalBytes)
                throw new InvalidDataException("The content inventory is incomplete or exceeds its expansion budget.");
            foreach (ContentEntry entry in index.entries)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!payloads.TryGetValue(entry.key, out ZipArchiveEntry? payload)
                    || payload.Length != entry.length || entry.length > limits.entryBytes)
                    throw new InvalidDataException($"Content entry '{entry.key}' is missing or has an invalid length.");
                using Stream input = payload.Open();
                if (!string.Equals(Hash(input, entry.length, cancellationToken), entry.contentHash, StringComparison.Ordinal))
                    throw new InvalidDataException($"Content entry '{entry.key}' does not match its decoded identity.");
            }
            cancellationToken.ThrowIfCancellationRequested();
            return new PackContentStore(archive, descriptor, index, payloads);
        }
        catch
        {
            if (archive is not null)
                archive.Dispose();
            else
                pack.Dispose();
            throw;
        }
    }

    private static Dictionary<ContentKey, ZipArchiveEntry> ReadInventory(
        ZipArchive archive,
        ContentReadLimits limits
    ) {
        if (archive.Entries.Count > limits.entryCount + 1L)
            throw new InvalidDataException("The archive contains too many entries.");
        Dictionary<ContentKey, ZipArchiveEntry> entries = new();
        HashSet<string> folded = new(StringComparer.OrdinalIgnoreCase);
        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            int mode = (entry.ExternalAttributes >> 16) & 0xF000;
            if ((mode != 0 && mode != 0x8000) || (entry.ExternalAttributes & 0x10) != 0)
                throw new InvalidDataException($"Archive entry '{entry.FullName}' is not a regular file.");
            ContentKey key;
            try
            {
                key = new ContentKey(entry.FullName);
            }
            catch (ArgumentException failure)
            {
                throw new InvalidDataException($"Archive entry '{entry.FullName}' is not portable.", failure);
            }
            if (!folded.Add(key.value!) || !entries.TryAdd(key, entry))
                throw new InvalidDataException($"The archive repeats an ambiguous content key '{key}'.");
        }
        return entries;
    }

    private static string Hash(
        Stream input,
        long expectedLength,
        CancellationToken cancellationToken
    ) {
        using IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        byte[] buffer = ArrayPool<byte>.Shared.Rent(128 * 1024);
        try
        {
            long remaining = expectedLength;
            while (remaining != 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                int read = input.Read(buffer, 0, (int)Math.Min(buffer.Length, remaining));
                if (read == 0)
                    throw new InvalidDataException("Content ended before its declared length.");
                hash.AppendData(buffer.AsSpan(0, read));
                remaining -= read;
            }
            cancellationToken.ThrowIfCancellationRequested();
            if (input.ReadByte() != -1)
                throw new InvalidDataException("Content exceeds its declared length.");
            return Convert.ToHexString(hash.GetHashAndReset());
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}
