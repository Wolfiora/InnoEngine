using System;
using System.IO;

namespace Inno.Assets;

/// <summary>
/// Provides controlled lease construction to concrete asset residency implementations.
/// </summary>
public abstract class AssetResidencyProvider
{
    /// <summary>
    /// Creates one strongly typed asset residency lease.
    /// </summary>
    /// <typeparam name="TAsset">
    /// Concrete canonical asset type.
    /// </typeparam>
    /// <param name="asset">
    /// Canonical asset retained by the lease.
    /// </param>
    /// <param name="release">
    /// Provider-owned release callback. Pending retirement must be retryable without repeating completed effects.
    /// </param>
    /// <returns>
    /// A lease that retains its value until release completes; a Pending callback is retried by the owning lifecycle.
    /// </returns>
    protected static AssetLease<TAsset> CreateAssetLease<TAsset>(
        TAsset asset,
        Action release
    ) where TAsset : AssetObject
        => new(asset, release);

    /// <summary>
    /// Creates one immutable artifact residency lease.
    /// </summary>
    /// <param name="artifact">
    /// Verified artifact metadata retained by the lease.
    /// </param>
    /// <param name="openRead">
    /// Opens immutable bytes with an independent provider pin owned by the returned stream.
    /// </param>
    /// <param name="release">
    /// Provider-owned release callback. Pending retirement must be retryable without repeating completed effects.
    /// </param>
    /// <returns>
    /// A lease that retains its value until release completes; a Pending callback is retried by the owning lifecycle.
    /// </returns>
    protected static ArtifactLease CreateArtifactLease(
        AssetArtifactInfo artifact,
        Func<Stream> openRead,
        Action release
    ) => new(artifact, openRead, release);
}
