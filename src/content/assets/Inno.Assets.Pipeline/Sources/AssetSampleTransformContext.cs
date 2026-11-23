using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Gives a source-language extension access to a staged sample clone before serialized references are remapped.
/// </summary>
public sealed class AssetSampleTransformContext
{
    private readonly Dictionary<Guid, Guid> m_identityMap;
    private readonly IReadOnlyDictionary<string, (Guid oldID, Guid newID)> m_sourceIdentities;

    internal AssetSampleTransformContext(
        string stagedRoot,
        AssetPath source,
        AssetPath target,
        Dictionary<Guid, Guid> identityMap,
        IReadOnlyDictionary<string, (Guid oldID, Guid newID)> sourceIdentities,
        CancellationToken cancellationToken
    ) {
        this.stagedRoot = stagedRoot;
        this.source = source;
        this.target = target;
        m_identityMap = identityMap;
        m_sourceIdentities = sourceIdentities;
        this.cancellationToken = cancellationToken;
    }

    /// <summary>
    /// Gets the absolute private staging directory; files here are committed only after all transforms succeed.
    /// </summary>
    public string stagedRoot { get; }

    /// <summary>
    /// Gets the installed sample source path.
    /// </summary>
    public AssetPath source { get; }

    /// <summary>
    /// Gets the writable project destination path.
    /// </summary>
    public AssetPath target { get; }

    /// <summary>
    /// Gets cancellation for this private, generation-pinned transformation.
    /// </summary>
    public CancellationToken cancellationToken { get; }

    /// <summary>
    /// Gets cloned source identities, including directory and file metadata.
    /// </summary>
    public IReadOnlyDictionary<Guid, Guid> identityMap => m_identityMap;

    /// <summary>
    /// Resolves the source and clone identity of one file relative to the sample directory.
    /// </summary>
    /// <param name="relativePath">
    /// Slash-separated source-local file path below the sample directory.
    /// </param>
    /// <param name="oldID">
    /// Receives the installed source identity.
    /// </param>
    /// <param name="newID">
    /// Receives the writable clone identity.
    /// </param>
    /// <returns>
    /// Whether both identities were recorded from sample metadata.
    /// </returns>
    public bool TryGetSourceIdentity(
        string relativePath,
        out Guid oldID,
        out Guid newID
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        if (m_sourceIdentities.TryGetValue(relativePath.Replace('\\', '/'), out (Guid oldID, Guid newID) pair))
        {
            oldID = pair.oldID;
            newID = pair.newID;
            return true;
        }
        oldID = default;
        newID = default;
        return false;
    }

    /// <summary>
    /// Registers one source-language type replacement for subsequent structured asset rewriting.
    /// </summary>
    /// <param name="oldID">
    /// The type identity stored in installed sample assets.
    /// </param>
    /// <param name="newID">
    /// The type identity declared by the cloned source.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// A source identity is reused inconsistently.
    /// </exception>
    public void MapType(
        Guid oldID,
        Guid newID
    ) {
        if (oldID == Guid.Empty || newID == Guid.Empty)
        {
            throw new ArgumentException("Sample type identities must be non-empty.");
        }
        if (m_identityMap.TryGetValue(oldID, out Guid existing) && existing != newID)
        {
            throw new InvalidDataException($"Sample type identity '{oldID:D}' has conflicting clone mappings.");
        }
        m_identityMap[oldID] = newID;
    }
}
