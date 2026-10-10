using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

using Inno.Core.IO;
using Inno.Core.Serialization;
using Inno.Content;

namespace Inno.Build;

internal static class ContentPackWriter
{
    private static readonly DateTimeOffset S_ARCHIVE_TIME = new(1980, 1, 1, 0, 0, 0, TimeSpan.Zero);

    internal static async ValueTask<(string packPath, string contentHash)> WriteAsync(
        string sourceRoot,
        string contentRoot,
        SerializationGeneration serialization,
        CancellationToken cancellationToken
    ) {
        Directory.CreateDirectory(contentRoot);
        string temporary = Path.Combine(contentRoot, ".content.pack.staging-" + Guid.NewGuid().ToString("N"));
        string[] files = PathBoundary.EnumerateFiles(sourceRoot)
            .OrderBy(static value => value, StringComparer.Ordinal).ToArray();
        ContentKey[] keys = files.Select(file => new ContentKey(Path.GetRelativePath(sourceRoot, file).Replace('\\', '/'))).ToArray();
        _ = new ContentPackIndex(keys.Select(static key => new ContentEntry(key, 0, new string('0', 64))));
        try
        {
            await using (FileStream stream = new(
                             temporary,
                             FileMode.CreateNew,
                             FileAccess.Write,
                             FileShare.None,
                             128 * 1024,
                             FileOptions.Asynchronous | FileOptions.SequentialScan))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
            {
                List<ContentEntry> entries = [];
                for (int index = 0; index < files.Length; index++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    ZipArchiveEntry entry = archive.CreateEntry(keys[index].value!, CompressionLevel.Optimal);
                    entry.LastWriteTime = S_ARCHIVE_TIME;
                    await using Stream output = entry.Open();
                    await using FileStream input = new(
                        files[index],
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read,
                        128 * 1024,
                        FileOptions.Asynchronous | FileOptions.SequentialScan);
                    using IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
                    byte[] buffer = ArrayPool<byte>.Shared.Rent(128 * 1024);
                    long length = 0;
                    try
                    {
                        int read;
                        while ((read = await input.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
                        {
                            length = checked(length + read);
                            hash.AppendData(buffer.AsSpan(0, read));
                            await output.WriteAsync(buffer.AsMemory(0, read), cancellationToken).ConfigureAwait(false);
                        }
                        entries.Add(new ContentEntry(keys[index], length, Convert.ToHexString(hash.GetHashAndReset())));
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }
                }
                ContentPackIndex inventory = new(entries);
                ZipArchiveEntry indexEntry = archive.CreateEntry(ContentPackIndex.C_INDEX_KEY, CompressionLevel.Optimal);
                indexEntry.LastWriteTime = S_ARCHIVE_TIME;
                await using Stream indexOutput = indexEntry.Open();
                await indexOutput.WriteAsync(ContentPackIndexCodec.Encode(inventory, serialization), cancellationToken).ConfigureAwait(false);
            }
            cancellationToken.ThrowIfCancellationRequested();
            string contentHash;
            await using (FileStream input = new(
                             temporary,
                             FileMode.Open,
                             FileAccess.Read,
                             FileShare.Read,
                             128 * 1024,
                             FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                contentHash = Convert.ToHexString(await SHA256.HashDataAsync(input, cancellationToken).ConfigureAwait(false));
            }
            string packPath = Path.Combine(contentRoot, $"content-{contentHash}.pack");
            AtomicFile.Install(temporary, packPath);
            return (packPath, contentHash);
        }
        catch
        {
            if (File.Exists(temporary))
                File.Delete(temporary);
            throw;
        }
    }
}
