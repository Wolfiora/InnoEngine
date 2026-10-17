using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter.Content.FileSystem;
using Inno.Content;
using Inno.Core.Serialization;
using Inno.Platform.MacOSArm64;
using Inno.Player.Runtime;
using Inno.Storage;

namespace Inno.Player.MacOSArm64;

internal sealed class MacOSArm64PlayerContentSource : IPlayerContentSource
{
    private readonly FileContentDeployment m_files;

    internal MacOSArm64PlayerContentSource(
        string applicationDirectory,
        string cacheRoot
    ) => m_files = new FileContentDeployment(
        MacOSArm64ApplicationLocations.GetPlayerContentDirectory(applicationDirectory), cacheRoot);

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
        byte[] manifest = await m_files.ReadDocumentAsync("runtime.manifest", cancellationToken).ConfigureAwait(false);
        byte[] catalog = await m_files.ReadDocumentAsync("catalog.inno", cancellationToken).ConfigureAwait(false);
        return new PlayerContentMetadata(manifest, catalog);
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
    public ValueTask<IRuntimeContentStore> PrepareAsync(
        ContentPackDescriptor pack,
        StorageScope scope,
        SerializationGeneration serialization,
        CancellationToken cancellationToken = default
    ) => m_files.PrepareAsync(pack, scope, serialization, cancellationToken);
}
