using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter.Content.FileSystem;
using Inno.Content;
using Inno.Core.Serialization;
using Inno.Player.Runtime;
using Inno.Storage;

namespace Inno.Player;

internal sealed class FilePlayerContentSource : IPlayerContentSource
{
    private const int C_METADATA_LIMIT = 16 * 1024 * 1024;

    private readonly string m_contentDirectory;
    private readonly string m_cacheRoot;
    private readonly ContentReadLimits m_limits = new();

    internal FilePlayerContentSource(
        string contentDirectory,
        string cacheRoot
    ) {
        m_contentDirectory = Path.GetFullPath(contentDirectory);
        m_cacheRoot = Path.GetFullPath(cacheRoot);
    }

    /// <inheritdoc />
    public async ValueTask<PlayerContentMetadata> ReadMetadataAsync(CancellationToken cancellationToken = default)
    {
        byte[] manifest = await ReadMetadataFileAsync("runtime.manifest", cancellationToken);
        byte[] catalog = await ReadMetadataFileAsync("catalog.inno", cancellationToken);
        return new PlayerContentMetadata(manifest, catalog);
    }

    /// <inheritdoc />
    public async ValueTask<IRuntimeContentStore> PrepareAsync(
        ContentPackDescriptor pack,
        StorageScope scope,
        SerializationGeneration serialization,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(pack);
        ArgumentNullException.ThrowIfNull(serialization);
        if (!scope.isValid)
            throw new ArgumentException("Content preparation requires an application namespace.", nameof(scope));
        cancellationToken.ThrowIfCancellationRequested();
        var stream = new FileStream(Path.Combine(m_contentDirectory, pack.fileName),
            FileMode.Open, FileAccess.Read, FileShare.Read);
        PackContentStore? source = null;
        try
        {
            source = await Task.Run(() => ContentPackReader.Open(stream, pack, serialization,
                m_limits, cancellationToken), CancellationToken.None).ConfigureAwait(false);
            return await FileContentPreparation.PrepareAsync(source,
                new FileContentCacheOptions(Path.Combine(m_cacheRoot, scope.value!)), cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            if (source is not null)
                source.Dispose();
            else
                stream.Dispose();
        }
    }

    private async Task<byte[]> ReadMetadataFileAsync(
        string fileName,
        CancellationToken cancellationToken
    ) {
        using var stream = new FileStream(Path.Combine(m_contentDirectory, fileName),
            FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);
        if (stream.Length is <= 0 or > C_METADATA_LIMIT)
            throw new InvalidDataException($"Deployment metadata '{fileName}' exceeds its document budget.");
        byte[] bytes = new byte[checked((int)stream.Length)];
        await stream.ReadExactlyAsync(bytes, cancellationToken).ConfigureAwait(false);
        if (stream.ReadByte() != -1)
            throw new InvalidDataException($"Deployment metadata '{fileName}' changed during acquisition.");
        return bytes;
    }
}
