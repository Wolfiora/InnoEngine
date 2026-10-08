using System;
using System.Collections.Generic;
using System.IO;
using Inno.Content;

namespace Inno.Adapter.Content.FileSystem;

/// <summary>
/// Pins one fully verified filesystem generation while exposing only logical content reads.
/// </summary>
public sealed class FileContentStore : IRuntimeContentStore
{
    private readonly object m_sync = new();
    private ContentCacheGeneration? m_generation;

    internal FileContentStore(
        ContentPackDescriptor descriptor,
        ContentPackIndex index,
        ContentCacheGeneration generation,
        IReadOnlyList<string> retirementDiagnostics
    ) {
        this.descriptor = descriptor;
        this.index = index;
        m_generation = generation;
        this.retirementDiagnostics = retirementDiagnostics;
    }

    /// <inheritdoc />
    public ContentPackDescriptor descriptor { get; }

    /// <inheritdoc />
    public ContentPackIndex index { get; }

    /// <summary>
    /// Gets post-publication cleanup failures; these do not invalidate the verified published generation.
    /// </summary>
    public IReadOnlyList<string> retirementDiagnostics { get; }

    /// <inheritdoc />
    public ContentReadLease Acquire(ContentKey key)
    {
        lock (m_sync)
        {
            ContentCacheGeneration generation = m_generation ?? throw new ObjectDisposedException(nameof(FileContentStore));
            if (!index.TryGetEntry(key, out ContentEntry? entry))
                throw new FileNotFoundException($"Content '{key}' does not exist in pack '{descriptor.contentHash}'.");
            return generation.Acquire(entry!);
        }
    }

    /// <summary>
    /// Stops new reads and releases the store pin after existing leases and streams retain their own ownership.
    /// </summary>
    public void Dispose()
    {
        lock (m_sync)
        {
            ContentCacheGeneration? generation = m_generation;
            m_generation = null;
            generation?.ReleasePin();
        }
    }
}
