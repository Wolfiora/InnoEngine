using System;
using System.Buffers;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Inno.Audio;
using Inno.Core.IO;

namespace Inno.Adapter.Audio.MiniAudio;

internal sealed class MiniAudioClipPreparation : IDisposable
{
    private readonly CancellationTokenSource m_cancellation = new();

    internal MiniAudioClipPreparation(
        IAudioClipSource source,
        string path,
        SemaphoreSlim slots
    ) {
        contentHash = source.contentHash.ToUpperInvariant();
        length = source.length;
        this.path = path;
        try
        {
            using AsyncFlowControl? suppression = ExecutionContext.IsFlowSuppressed()
                ? null : ExecutionContext.SuppressFlow();
            completion = Task.Run(() => PrepareAsync(source, slots), CancellationToken.None);
        }
        catch
        {
            m_cancellation.Dispose();
            throw;
        }
    }

    internal string contentHash { get; }
    internal long length { get; }
    internal string path { get; }
    internal Task completion { get; }
    internal int references { get; set; }

    internal void Cancel() => m_cancellation.Cancel();

    /// <inheritdoc />
    public void Dispose()
    {
        if (!completion.IsCompleted)
            throw new InvalidOperationException("Encoded preparation must drain before releasing its owner.");
        _ = completion.Exception;
        if (File.Exists(path))
            File.Delete(path);
        m_cancellation.Dispose();
    }

    private async Task PrepareAsync(
        IAudioClipSource source,
        SemaphoreSlim slots
    ) {
        CancellationToken cancellationToken = m_cancellation.Token;
        await slots.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            using Stream reader = source.OpenRead();
            byte[] buffer = ArrayPool<byte>.Shared.Rent(64 * 1024);
            string candidate = path + ".staging-" + Guid.NewGuid().ToString("N");
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                using IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
                long copied = 0;
                using (var output = new FileStream(candidate, FileMode.CreateNew, FileAccess.Write,
                           FileShare.None, buffer.Length, FileOptions.Asynchronous))
                {
                    int count;
                    while ((count = await reader.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
                    {
                        copied = checked(copied + count);
                        if (copied > length)
                            throw new InvalidDataException("Encoded audio exceeds its declared length.");
                        hash.AppendData(buffer, 0, count);
                        await output.WriteAsync(buffer.AsMemory(0, count), cancellationToken).ConfigureAwait(false);
                    }
                    if (copied != length || !string.Equals(Convert.ToHexString(hash.GetHashAndReset()),
                            contentHash, StringComparison.Ordinal))
                    {
                        throw new InvalidDataException("Encoded audio does not match its content identity.");
                    }
                    await output.FlushAsync(cancellationToken).ConfigureAwait(false);
                }
                cancellationToken.ThrowIfCancellationRequested();
                AtomicFile.Install(candidate, path, overwrite: false);
            }
            finally
            {
                try
                {
                    if (File.Exists(candidate))
                        File.Delete(candidate);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
        }
        finally
        {
            slots.Release();
        }
    }
}
