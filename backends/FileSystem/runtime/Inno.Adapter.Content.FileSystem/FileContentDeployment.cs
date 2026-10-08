using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter.Content.FileSystem;
using Inno.Content;
using Inno.Core.Serialization;
using Inno.Storage;

namespace Inno.Adapter.Content.FileSystem;

/// <summary>
/// Reads explicit file deployments using the shared content protocol and verified generation cache.
/// </summary>
public sealed class FileContentDeployment
{
    private const int C_METADATA_LIMIT = 16 * 1024 * 1024;

    private readonly string m_contentDirectory;
    private readonly string m_cacheRoot;
    private readonly ContentReadLimits m_limits = new();

    /// <summary>
    /// Captures explicit deployment and application cache locations without inspecting platform layout.
    /// </summary>
    /// <param name="contentDirectory">
    /// The directory containing the current deployment's metadata and content pack.
    /// </param>
    /// <param name="cacheRoot">
    /// The host-owned root beneath which application-scoped verified generations are prepared.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A location is empty or cannot be normalized.
    /// </exception>
    public FileContentDeployment(
        string contentDirectory,
        string cacheRoot
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(cacheRoot);
        m_contentDirectory = Path.GetFullPath(contentDirectory);
        m_cacheRoot = Path.GetFullPath(cacheRoot);
    }

    /// <summary>
    /// Opens, verifies and materializes one immutable content pack into an application-scoped generation.
    /// </summary>
    /// <param name="pack">
    /// The frozen expected pack identity.
    /// </param>
    /// <param name="scope">
    /// The portable application namespace.
    /// </param>
    /// <param name="serialization">
    /// The owning serialization generation.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels acquisition and candidate preparation before publication.
    /// </param>
    /// <returns>
    /// An owned verified store; the caller must dispose it after all readers retire.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The pack or its index fails integrity validation.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Preparation was canceled.
    /// </exception>
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

    /// <summary>
    /// Acquires a bounded metadata document directly beneath the declared deployment root.
    /// </summary>
    /// <param name="fileName">
    /// A single portable document filename, without directory segments.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the owned stream read.
    /// </param>
    /// <returns>
    /// Owned document bytes; an absent or changing document fails explicitly.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The filename is not a single portable segment.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The document is empty, too large or changes during acquisition.
    /// </exception>
    public async Task<byte[]> ReadDocumentAsync(
        string fileName,
        CancellationToken cancellationToken
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        if (fileName is "." or ".." || fileName.IndexOfAny(['/', '\\', ':']) >= 0)
            throw new ArgumentException("A deployment document requires a single portable filename.", nameof(fileName));
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
