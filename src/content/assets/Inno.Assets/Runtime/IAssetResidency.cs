using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets;

/// <summary>
/// Exposes explicit asynchronous asset and artifact residency ownership.
/// </summary>
public interface IAssetResidency
{
    /// <summary>
    /// Acquires a canonical asset by logical path and keeps it resident until the lease is released.
    /// </summary>
    /// <typeparam name="TAsset">
    /// Required asset contract.
    /// </typeparam>
    /// <param name="path">
    /// Mount-qualified logical asset path.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation for this caller's asynchronous wait.
    /// </param>
    /// <returns>
    /// A lease that owns residency for the canonical asset.
    /// </returns>
    ValueTask<AssetLease<TAsset>> AcquireAsync<TAsset>(
        AssetPath path,
        CancellationToken cancellationToken = default
    ) where TAsset : AssetObject;

    /// <summary>
    /// Acquires a canonical asset by persistent identity and keeps it resident until release.
    /// </summary>
    /// <typeparam name="TAsset">
    /// Required asset contract.
    /// </typeparam>
    /// <param name="persistentID">
    /// Non-empty persistent asset identity.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation for this caller's asynchronous wait.
    /// </param>
    /// <returns>
    /// A lease that owns residency for the canonical asset.
    /// </returns>
    ValueTask<AssetLease<TAsset>> AcquireAsync<TAsset>(
        Guid persistentID,
        CancellationToken cancellationToken = default
    ) where TAsset : AssetObject;

    /// <summary>
    /// Acquires one verified immutable artifact output.
    /// </summary>
    /// <param name="persistentID">
    /// Persistent identity of the artifact owner.
    /// </param>
    /// <param name="outputName">
    /// Stable artifact output name.
    /// </param>
    /// <returns>
    /// A lease that keeps the artifact generation available to the caller.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the requested artifact does not exist or fails verification.
    /// </exception>
    ArtifactLease AcquireArtifact(
        Guid persistentID,
        string outputName
    );
}
