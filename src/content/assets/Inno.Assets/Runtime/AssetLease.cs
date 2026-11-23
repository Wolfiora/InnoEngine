using System;
using Inno.Core.Execution;

namespace Inno.Assets;

/// <summary>
/// Keeps one canonical asset generation resident for an explicit lifetime.
/// </summary>
/// <typeparam name="TAsset">
/// Concrete retained asset type.
/// </typeparam>
public sealed class AssetLease<TAsset> : IDisposable
    where TAsset : AssetObject
{
    private readonly ResidencyLease<TAsset> m_residency;

    internal AssetLease(
        TAsset asset,
        Action release
    ) {
        m_residency = new ResidencyLease<TAsset>(asset, release);
    }

    /// <summary>
    /// Gets the retained canonical asset while this lease is active.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after the lease has been released.
    /// </exception>
    public TAsset asset => m_residency.value;

    /// <summary>
    /// Releases this caller's residency ownership, retaining its value and callback while the provider reports Pending.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// Provider retirement is unfinished or a concurrent release is in progress; retain this lease and retry at a safe point.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The provider reports a wrapped failure, or release completes with ordinary errors retained from earlier pending attempts.
    /// </exception>
    /// <remarks>
    /// Pending may be wrapped in another exception; classify the complete failure with RetirementPendingException.Find.
    /// Ordinary sibling failures remain observable after a later release attempt succeeds.
    /// </remarks>
    public void Dispose() => m_residency.Dispose();
}
