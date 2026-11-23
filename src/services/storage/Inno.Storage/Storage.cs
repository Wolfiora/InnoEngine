using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Storage;

/// <summary>
/// Provides script-friendly access to the current application storage sandbox.
/// </summary>
public static class Storage
{
    /// <summary>
    /// Determines whether a value exists.
    /// </summary>
    /// <param name="key">
    /// The sandbox-relative storage key.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation requested by the caller.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the value exists.
    /// </returns>
    public static ValueTask<bool> ExistsAsync(
        StorageKey key,
        CancellationToken cancellationToken = default
    ) => StorageExecutionContext.current.ExistsAsync(key, cancellationToken);

    /// <summary>
    /// Reads a complete value.
    /// </summary>
    /// <param name="key">
    /// The sandbox-relative storage key.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation requested by the caller.
    /// </param>
    /// <returns>
    /// The stored bytes, or <see langword="null"/> when absent.
    /// </returns>
    public static ValueTask<byte[]?> ReadAsync(
        StorageKey key,
        CancellationToken cancellationToken = default
    ) => StorageExecutionContext.current.ReadAsync(key, cancellationToken);

    /// <summary>
    /// Atomically replaces a complete value.
    /// </summary>
    /// <param name="key">
    /// The sandbox-relative storage key.
    /// </param>
    /// <param name="value">
    /// The complete value to commit.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation requested by the caller.
    /// </param>
    /// <returns>
    /// A task that completes after commit.
    /// </returns>
    public static ValueTask WriteAsync(
        StorageKey key,
        ReadOnlyMemory<byte> value,
        CancellationToken cancellationToken = default
    ) => StorageExecutionContext.current.WriteAsync(key, value, cancellationToken);

    /// <summary>
    /// Deletes one value.
    /// </summary>
    /// <param name="key">
    /// The sandbox-relative storage key.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation requested by the caller.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a value was deleted.
    /// </returns>
    public static ValueTask<bool> DeleteAsync(
        StorageKey key,
        CancellationToken cancellationToken = default
    ) => StorageExecutionContext.current.DeleteAsync(key, cancellationToken);

    /// <summary>
    /// Lists keys below an optional prefix.
    /// </summary>
    /// <param name="prefix">
    /// An optional normalized directory prefix.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation requested by the caller.
    /// </param>
    /// <returns>
    /// Keys in deterministic ordinal order.
    /// </returns>
    public static ValueTask<IReadOnlyList<StorageKey>> ListAsync(
        StorageKey? prefix = null,
        CancellationToken cancellationToken = default
    ) => StorageExecutionContext.current.ListAsync(prefix, cancellationToken);
}
