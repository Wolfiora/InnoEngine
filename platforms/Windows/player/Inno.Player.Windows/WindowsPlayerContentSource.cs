using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter.Content.FileSystem;
using Inno.Content;
using Inno.Core.Serialization;
using Inno.Platform.Windows;
using Inno.Player.Runtime;
using Inno.Storage;

namespace Inno.Player.Windows;

internal sealed class WindowsPlayerContentSource : IPlayerContentSource
{
    private readonly FileContentDeployment m_files;

    internal WindowsPlayerContentSource(
        string applicationDirectory,
        string cacheRoot
    ) => m_files = new FileContentDeployment(
        WindowsApplicationLocations.GetPlayerContentDirectory(applicationDirectory), cacheRoot);

    /// <inheritdoc />
    public async ValueTask<PlayerContentMetadata> ReadMetadataAsync(CancellationToken cancellationToken = default)
    {
        byte[] manifest = await m_files.ReadDocumentAsync("runtime.manifest", cancellationToken).ConfigureAwait(false);
        byte[] catalog = await m_files.ReadDocumentAsync("catalog.inno", cancellationToken).ConfigureAwait(false);
        return new PlayerContentMetadata(manifest, catalog);
    }

    /// <inheritdoc />
    public ValueTask<IRuntimeContentStore> PrepareAsync(
        ContentPackDescriptor pack,
        StorageScope scope,
        SerializationGeneration serialization,
        CancellationToken cancellationToken = default
    ) => m_files.PrepareAsync(pack, scope, serialization, cancellationToken);
}
