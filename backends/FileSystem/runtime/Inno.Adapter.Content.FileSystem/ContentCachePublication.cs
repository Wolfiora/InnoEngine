using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Content;
using Inno.Core.IO;

namespace Inno.Adapter.Content.FileSystem;

internal static class ContentCachePublication
{
    internal static async ValueTask<string> PublishAsync(
        string cache,
        IRuntimeContentStore source,
        CancellationToken cancellationToken
    ) {
        string generation = Guid.NewGuid().ToString("N");
        string candidate = Path.Combine(cache, "staging", generation);
        string target = Path.Combine(cache, "generations", generation);
        Directory.CreateDirectory(candidate);
        try
        {
            foreach (ContentEntry entry in source.index.entries)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string output = ContentCacheValidator.Resolve(candidate, entry.key);
                Directory.CreateDirectory(Path.GetDirectoryName(output)!);
                using ContentReadLease lease = source.Acquire(entry.key);
                using Stream input = lease.OpenRead();
                await using FileStream destination = new(output, FileMode.CreateNew, FileAccess.Write,
                    FileShare.None, 128 * 1024, FileOptions.Asynchronous | FileOptions.SequentialScan);
                long remaining = entry.length;
                byte[] buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(128 * 1024);
                try
                {
                    while (remaining > 0)
                    {
                        int read = await input.ReadAsync(buffer.AsMemory(0, (int)Math.Min(buffer.Length, remaining)),
                            cancellationToken).ConfigureAwait(false);
                        if (read == 0)
                            throw new InvalidDataException($"Content '{entry.key}' ended before its indexed length.");
                        await destination.WriteAsync(buffer.AsMemory(0, read), cancellationToken).ConfigureAwait(false);
                        remaining -= read;
                    }
                    if (input.ReadByte() != -1)
                        throw new InvalidDataException($"Content '{entry.key}' exceeded its indexed length.");
                    await destination.FlushAsync(cancellationToken).ConfigureAwait(false);
                    destination.Flush(flushToDisk: true);
                }
                finally
                {
                    System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
                }
            }
            if (!ContentCacheValidator.Validate(candidate, source.index, cancellationToken))
                throw new InvalidDataException("The prepared content generation failed complete validation.");
            using (await FileLease.AcquireAsync(Path.Combine(cache, "leases", generation + ".lock"),
                       TimeSpan.Zero, cancellationToken).ConfigureAwait(false))
            {
                AtomicDirectory.Publish(candidate, target, cancellationToken);
            }
            cancellationToken.ThrowIfCancellationRequested();
            AtomicFile.WriteAllBytes(Path.Combine(cache, "current"), Encoding.ASCII.GetBytes(generation));
            return generation;
        }
        finally
        {
            if (Directory.Exists(candidate))
                Directory.Delete(candidate, recursive: true);
        }
    }
}
