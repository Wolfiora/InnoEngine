using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Inno.Storage;

namespace Inno.Adapter.Storage.Browser;

/// <summary>
/// Stores application values atomically in the browser origin's durable key-value store.
/// </summary>
public sealed partial class BrowserApplicationStorage : IApplicationStorage
{
    private readonly string m_prefix;

    /// <summary>
    /// Creates a namespace sandbox within the current browser origin.
    /// </summary>
    /// <param name="scope">
    /// The application namespace used to isolate keys within the browser origin.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The namespace is unassigned.
    /// </exception>
    public BrowserApplicationStorage(StorageScope scope)
    {
        if (!scope.isValid)
            throw new ArgumentException("Browser storage requires an assigned namespace.", nameof(scope));
        m_prefix = scope.value + ":";
    }

    /// <summary>
    /// Checks whether a key has a value in this browser origin.
    /// </summary>
    /// <param name="key">
    /// The application key to check.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the operation before accessing storage.
    /// </param>
    /// <returns>
    /// True when a value exists for the key.
    /// </returns>
    public ValueTask<bool> ExistsAsync(
        StorageKey key,
        CancellationToken cancellationToken = default
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        return ValueTask.FromResult(GetItem(Resolve(key)) is not null);
    }

    /// <summary>
    /// Reads a stored value from this browser origin.
    /// </summary>
    /// <param name="key">
    /// The application key to read.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the operation before accessing storage.
    /// </param>
    /// <returns>
    /// The stored bytes, or null when the key is absent.
    /// </returns>
    public ValueTask<byte[]?> ReadAsync(
        StorageKey key,
        CancellationToken cancellationToken = default
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        string? value = GetItem(Resolve(key));
        return ValueTask.FromResult(value is null ? null : Convert.FromBase64String(value));
    }

    /// <summary>
    /// Replaces a value in this browser origin.
    /// </summary>
    /// <param name="key">
    /// The application key to write.
    /// </param>
    /// <param name="value">
    /// The bytes to store.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the operation before accessing storage.
    /// </param>
    /// <returns>
    /// An operation that completes when the browser has stored the value.
    /// </returns>
    public ValueTask WriteAsync(
        StorageKey key,
        ReadOnlyMemory<byte> value,
        CancellationToken cancellationToken = default
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        SetItem(Resolve(key), Convert.ToBase64String(value.Span));
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Removes a value from this browser origin.
    /// </summary>
    /// <param name="key">
    /// The application key to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the operation before accessing storage.
    /// </param>
    /// <returns>
    /// True when the key existed and was removed.
    /// </returns>
    public ValueTask<bool> DeleteAsync(
        StorageKey key,
        CancellationToken cancellationToken = default
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        string storageKey = Resolve(key);
        if (GetItem(storageKey) is null)
            return ValueTask.FromResult(false);
        RemoveItem(storageKey);
        return ValueTask.FromResult(true);
    }

    /// <summary>
    /// Lists keys in this application sandbox, optionally under a prefix.
    /// </summary>
    /// <param name="prefix">
    /// The optional key prefix to filter.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the operation before accessing storage.
    /// </param>
    /// <returns>
    /// The matching keys in ordinal order.
    /// </returns>
    public ValueTask<IReadOnlyList<StorageKey>> ListAsync(
        StorageKey? prefix = null,
        CancellationToken cancellationToken = default
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        var keys = new List<StorageKey>();
        for (int index = 0; index < GetItemCount(); index++)
        {
            string? browserKey = GetKeyAt(index);
            if (browserKey is null || !browserKey.StartsWith(m_prefix, StringComparison.Ordinal))
                continue;
            var key = new StorageKey(browserKey[m_prefix.Length..]);
            if (prefix is { } requested
                && key != requested
                && !key.value.StartsWith(requested.value + "/", StringComparison.Ordinal))
            {
                continue;
            }
            keys.Add(key);
        }
        return ValueTask.FromResult<IReadOnlyList<StorageKey>>(
            keys.OrderBy(static key => key.value, StringComparer.Ordinal).ToArray());
    }

    private string Resolve(StorageKey key)
    {
        if (!key.isValid)
            throw new ArgumentException("A valid storage key is required.", nameof(key));
        return m_prefix + key.value;
    }

    [JSImport("storage.get", "browser-player.js")]
    private static partial string? GetItem(string key);

    [JSImport("storage.set", "browser-player.js")]
    private static partial void SetItem(
        string key,
        string value
    );

    [JSImport("storage.remove", "browser-player.js")]
    private static partial void RemoveItem(string key);

    [JSImport("storage.count", "browser-player.js")]
    private static partial int GetItemCount();

    [JSImport("storage.keyAt", "browser-player.js")]
    private static partial string? GetKeyAt(int index);
}
