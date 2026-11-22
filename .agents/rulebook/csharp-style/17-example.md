---
paths: ["**/*.cs"]
tags: [csharp-style]
---
# 17. Reference example

One file that applies the rules together.

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.Diagnostics;
using Inno.Core.Identity;

using AssetEvent = Inno.Assets.Pipeline.AssetChangedEvent;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Caches loaded assets and reloads them when their sources change.
/// </summary>
public sealed class AssetCache : IDisposable
{
    private const int C_INITIAL_CAPACITY = 64;
    private const int C_MAX_ATTEMPT_COUNT = 3;

    private static readonly TimeSpan RETRY_DELAY = TimeSpan.FromMilliseconds(50);

    private readonly Dictionary<AssetID, Entry> m_entries = new(C_INITIAL_CAPACITY);
    private readonly IAssetStore m_store;
    private readonly ILogSink m_log;
    private int m_generation;
    private bool m_disposed;

    /// <summary>
    /// Creates a cache that reads asset content from the given store.
    /// </summary>
    /// <param name="store">
    /// The store that provides asset content.
    /// </param>
    /// <param name="log">
    /// The sink that receives reload diagnostics.
    /// </param>
    /// <param name="name">
    /// The diagnostic name of the cache.
    /// </param>
    /// <exception cref="ArgumentException">
    /// <paramref name="name"/> is empty or consists only of white space.
    /// </exception>
    public AssetCache(
        IAssetStore store,
        ILogSink log,
        string name
    ) {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(log);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        m_store = store;
        m_log = log;
        this.name = name;
        this.isEnabled = true;
    }

    /// <summary>
    /// Gets the diagnostic name of the cache.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets a value indicating whether the cache accepts source changes.
    /// </summary>
    public bool isEnabled { get; private set; }

    /// <summary>
    /// Gets the number of cached assets.
    /// </summary>
    public int count => m_entries.Count;

    /// <summary>
    /// Occurs when a cached asset has been reloaded from its source.
    /// </summary>
    public event Action<AssetID>? assetReloaded;

    /// <summary>
    /// Attempts to find a cached asset that is still current.
    /// </summary>
    /// <param name="id">
    /// The persistent identity of the asset.
    /// </param>
    /// <param name="handle">
    /// Receives the cached handle when this method returns <see langword="true"/>;
    /// otherwise <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a current entry exists; otherwise <see langword="false"/>.
    /// </returns>
    public bool TryResolve(
        AssetID id,
        [NotNullWhen(true)] out AssetHandle? handle
    ) {
        ObjectDisposedException.ThrowIf(m_disposed, this);

        if (m_entries.TryGetValue(id, out Entry? entry)
            && entry.generation == m_generation
            && !entry.handle.isMissing
        ) {
            handle = entry.handle;
            return true;
        }

        handle = null;
        return false;
    }

    /// <summary>
    /// Loads an asset, reusing the cached handle when it is still current.
    /// </summary>
    /// <param name="id">
    /// The persistent identity of the asset.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that cancels the load.
    /// </param>
    /// <returns>
    /// The loaded asset handle.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The store could not provide the asset after every attempt.
    /// </exception>
    public async Task<AssetHandle> LoadAsync(
        AssetID id,
        CancellationToken cancellationToken
    ) {
        if (TryResolve(id, out AssetHandle? cached)) return cached;

        for (int attempt = 1; attempt <= C_MAX_ATTEMPT_COUNT; attempt++)
        {
            try
            {
                AssetHandle handle = await m_store.LoadAsync(id, cancellationToken).ConfigureAwait(false);
                m_entries[id] = new Entry(handle, m_generation);
                return handle;
            }
            catch (IOException exception) when (attempt < C_MAX_ATTEMPT_COUNT)
            {
                m_log.Warning($"Loading asset '{id}' failed on attempt {attempt}: {exception.Message}");
                await Task.Delay(RETRY_DELAY, cancellationToken).ConfigureAwait(false);
            }
        }

        throw new InvalidDataException(
            $"The asset '{id}' could not be loaded from store '{m_store.name}' after {C_MAX_ATTEMPT_COUNT} attempts."
        );
    }

    /// <summary>
    /// Applies source changes and invalidates the affected entries.
    /// </summary>
    /// <param name="changes">
    /// The source changes reported by the watcher.
    /// </param>
    public void ApplySourceChanges(IReadOnlyList<AssetEvent> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);
        if (!isEnabled) return;

        m_generation++;
        foreach (AssetEvent change in changes)
        {
            if (!m_store.TryGetID(change.path, out AssetID id)) continue;

            string action = change.change switch
            {
                WatcherChangeTypes.Deleted => "removed",
                WatcherChangeTypes.Renamed => "renamed",
                _ => "changed"
            };
            m_log.Info($"Asset '{id}' was {action}.");

            m_entries.Remove(id);
            assetReloaded?.Invoke(id);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (m_disposed) return;

        // Entries only borrow their handles, so the store releases the content.
        m_entries.Clear();
        m_disposed = true;
    }

    internal bool IsCurrent(AssetID id)
        => m_entries.TryGetValue(id, out Entry? entry)
           && entry.generation == m_generation;

    private sealed record Entry(
        AssetHandle handle,
        int generation
    );
}
```
