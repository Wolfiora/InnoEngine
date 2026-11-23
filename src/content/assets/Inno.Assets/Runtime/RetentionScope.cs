using System;
using Inno.Core.Execution;

namespace Inno.Assets;

/// <summary>
/// Owns multiple asset, artifact, or subsystem leases as one reverse-order lifetime.
/// </summary>
public sealed class RetentionScope : IDisposable
{
    private readonly LifetimeScope m_lifetime = new();

    /// <summary>
    /// Transfers one lease into this scope.
    /// </summary>
    /// <typeparam name="TLease">
    /// Disposable lease type.
    /// </typeparam>
    /// <param name="lease">
    /// Lease whose disposal ownership transfers to this scope.
    /// </param>
    /// <returns>
    /// The same lease for convenient initialization.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Retirement has begun and the scope cannot accept another lease.
    /// </exception>
    public TLease Retain<TLease>(TLease lease)
        where TLease : IDisposable => m_lifetime.Own(lease);

    /// <summary>
    /// Releases all retained leases in reverse acquisition order.
    /// </summary>
    /// <exception cref="AggregateException">
    /// Thrown after every lease is attempted when one or more releases fail.
    /// </exception>
    /// <exception cref="RetirementPendingException">
    /// A lease is not retired. The current and remaining dependencies are retained for retry.
    /// </exception>
    public void Dispose() => m_lifetime.Dispose();
}
