using System;

namespace Inno.Content;

/// <summary>
/// Provides immutable content leases without exposing deployment paths or writable authoring sources.
/// </summary>
public interface IRuntimeContentStore : IDisposable
{
    /// <summary>
    /// Gets the immutable complete-pack identity.
    /// </summary>
    ContentPackDescriptor descriptor { get; }

    /// <summary>
    /// Gets the immutable complete payload inventory.
    /// </summary>
    ContentPackIndex index { get; }

    /// <summary>
    /// Pins an entry against source retirement and supplies independent read streams.
    /// </summary>
    /// <param name="key">
    /// The exact assigned logical key.
    /// </param>
    /// <returns>
    /// A caller-owned lease whose streams remain usable after this store closes.
    /// </returns>
    /// <exception cref="System.IO.FileNotFoundException">
    /// The exact content key is not present.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The store has closed and cannot issue new leases.
    /// </exception>
    ContentReadLease Acquire(ContentKey key);
}
