using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Inno.Build.Toolchains;

/// <summary>
/// Registers a publisher's exact cache boundary and its current artifact ownership inventory.
/// </summary>
public sealed class BuildCacheOwnerDescriptor
{
    private readonly Func<CancellationToken, IReadOnlyList<BuildCacheArtifactDescriptor>> m_readArtifacts;
    private readonly Func<CancellationToken, IReadOnlyList<string>> m_readProtectedArtifacts;
    private readonly Action<CancellationToken>? m_retireMetadata;

    /// <summary>
    /// Borrows publisher callbacks without discovering artifacts from directory names or file extensions.
    /// </summary>
    /// <param name="id">
    /// The unique registration identity within this cleanup operation.
    /// </param>
    /// <param name="directory">
    /// The absolute cache root exclusively managed by this publisher.
    /// </param>
    /// <param name="coordinationPath">
    /// The persistent owner lease also acquired by publication and protected-root updates.
    /// It must remain within the cache root and is never deleted by cleanup.
    /// </param>
    /// <param name="readArtifacts">
    /// Returns only generations proven to belong to this owner, with their dependency metadata.
    /// The callback must observe cancellation and must not create, repair or publish artifacts.
    /// </param>
    /// <param name="readProtectedArtifacts">
    /// Returns current product and Pack roots; retirement follows their complete dependency closure.
    /// Root updates must hold coordinationPath and retain each referenced artifact's reading lease.
    /// </param>
    /// <param name="retireMetadata">
    /// Optional owner-specific metadata retirement, called only under exclusive owner coordination.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An identity is blank or a path is relative.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// An inventory callback is absent.
    /// </exception>
    public BuildCacheOwnerDescriptor(
        string id,
        string directory,
        string coordinationPath,
        Func<CancellationToken, IReadOnlyList<BuildCacheArtifactDescriptor>> readArtifacts,
        Func<CancellationToken, IReadOnlyList<string>> readProtectedArtifacts,
        Action<CancellationToken>? retireMetadata = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        ArgumentException.ThrowIfNullOrWhiteSpace(coordinationPath);
        ArgumentNullException.ThrowIfNull(readArtifacts);
        ArgumentNullException.ThrowIfNull(readProtectedArtifacts);
        if (!Path.IsPathFullyQualified(directory) || !Path.IsPathFullyQualified(coordinationPath))
            throw new ArgumentException("Cache registration requires absolute owner paths.");
        this.id = id;
        this.directory = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory));
        this.coordinationPath = Path.GetFullPath(coordinationPath);
        m_readArtifacts = readArtifacts;
        m_retireMetadata = retireMetadata;
        m_readProtectedArtifacts = readProtectedArtifacts;
    }

    /// <summary>
    /// Gets the unique publisher identity registered for this cleanup operation.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the exact registered cache root; its parent directories are never managed implicitly.
    /// </summary>
    public string directory { get; }

    /// <summary>
    /// Gets the persistent lease shared with publication and protected-root changes.
    /// </summary>
    public string coordinationPath { get; }

    internal void RetireMetadata(CancellationToken cancellation) => m_retireMetadata?.Invoke(cancellation);

    internal IReadOnlyList<BuildCacheArtifactDescriptor> ReadArtifacts(CancellationToken cancellation)
        => m_readArtifacts(cancellation) ?? throw new InvalidDataException($"Cache owner '{id}' returned no inventory.");

    internal IReadOnlyList<string> ReadProtectedArtifacts(CancellationToken cancellation)
        => m_readProtectedArtifacts(cancellation) ?? throw new InvalidDataException($"Cache owner '{id}' returned no protection inventory.");
}
