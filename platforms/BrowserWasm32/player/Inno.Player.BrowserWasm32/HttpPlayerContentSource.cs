using System;
using System.Buffers;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Inno.Content;
using Inno.Core.Serialization;
using Inno.Player.Runtime;
using Inno.Storage;

namespace Inno.Player.BrowserWasm32;

internal sealed class HttpPlayerContentSource : IPlayerContentSource
{
    private const int C_METADATA_LIMIT = 16 * 1024 * 1024;
    private readonly HttpClient m_client;
    private readonly ContentReadLimits m_limits = new(packBytes: 512L * 1024 * 1024);

    internal HttpPlayerContentSource(HttpClient client)
    {
        m_client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <summary>
    /// Reads independently owned deployment metadata from the platform's explicit content source.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels acquisition before content preparation starts.
    /// </param>
    /// <returns>
    /// Owned manifest and catalog bytes; missing, unreadable or oversized documents fail the operation.
    /// </returns>
    /// <exception cref="System.OperationCanceledException">
    /// Metadata acquisition was canceled.
    /// </exception>
    public async ValueTask<PlayerContentMetadata> ReadMetadataAsync(CancellationToken cancellationToken = default)
    {
        using MemoryStream manifest = await DownloadAsync("runtime.manifest", C_METADATA_LIMIT, cancellationToken);
        using MemoryStream catalog = await DownloadAsync("catalog.inno", C_METADATA_LIMIT, cancellationToken);
        return new PlayerContentMetadata(manifest.GetBuffer().AsSpan(0, checked((int)manifest.Length)),
            catalog.GetBuffer().AsSpan(0, checked((int)catalog.Length)));
    }

    /// <summary>
    /// Opens the catalog's exact pack through this platform's verified content reader.
    /// </summary>
    /// <param name="pack">
    /// The exact validated deployment identity; no other pack is substituted.
    /// </param>
    /// <param name="scope">
    /// The required application namespace for cache isolation.
    /// </param>
    /// <param name="serialization">
    /// The active serialization generation borrowed during preparation.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels acquisition before store ownership transfers.
    /// </param>
    /// <returns>
    /// A caller-owned immutable content store; dispose it after all consumers and leases finish.
    /// </returns>
    /// <exception cref="System.IO.InvalidDataException">
    /// Pack inventory, lengths or content hashes fail validation.
    /// </exception>
    /// <exception cref="System.OperationCanceledException">
    /// Preparation was canceled.
    /// </exception>
    public async ValueTask<IRuntimeContentStore> PrepareAsync(
        ContentPackDescriptor pack,
        StorageScope scope,
        SerializationGeneration serialization,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(pack);
        if (!scope.isValid)
            throw new ArgumentException("Content preparation requires an application namespace.", nameof(scope));
        MemoryStream stream = await DownloadAsync(pack.fileName, m_limits.packBytes, cancellationToken);
        return ContentPackReader.Open(stream, pack, serialization, m_limits, cancellationToken);
    }

    private async Task<MemoryStream> DownloadAsync(
        string fileName,
        long budget,
        CancellationToken cancellationToken
    ) {
        using HttpResponseMessage response = await m_client.GetAsync("Content/" + fileName,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        long? length = response.Content.Headers.ContentLength;
        if (length is < 0 || length > budget)
            throw new InvalidDataException($"Deployment content '{fileName}' exceeds its download budget.");
        var output = new MemoryStream();
        byte[] buffer = ArrayPool<byte>.Shared.Rent(64 * 1024);
        try
        {
            using Stream input = await response.Content.ReadAsStreamAsync(cancellationToken);
            int count;
            while ((count = await input.ReadAsync(buffer, cancellationToken)) != 0)
            {
                if (output.Length + count > budget)
                    throw new InvalidDataException($"Deployment content '{fileName}' exceeds its download budget.");
                await output.WriteAsync(buffer.AsMemory(0, count), cancellationToken);
            }
            if (length is not null && output.Length != length)
                throw new InvalidDataException($"Deployment content '{fileName}' has a truncated HTTP body.");
            cancellationToken.ThrowIfCancellationRequested();
            output.Position = 0;
            return output;
        }
        catch
        {
            output.Dispose();
            throw;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}
