using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Inno.Core.IO;

namespace Inno.Assets;

/// <summary>
/// Counts explicit leases over immutable artifact keys independently of asset object residency.
/// </summary>
public sealed class ArtifactRetention : AssetResidencyProvider
{
    private readonly object m_sync = new();
    private readonly Dictionary<AssetArtifactKey, int> m_counts = [];

    /// <summary>
    /// Retains a verified output before its owner permits cache collection.
    /// </summary>
    /// <param name="artifact">
    /// Verified immutable metadata; the provider must serialize acquisition with collection.
    /// </param>
    /// <returns>
    /// A lease that releases exactly one reference, including when disposed from another thread.
    /// </returns>
    /// <param name="openRead">
    /// Opens immutable bytes; this retention owner adds an independent stream pin before invoking it.
    /// </param>
    public ArtifactLease Retain(
        AssetArtifactInfo artifact,
        Func<Stream> openRead
    )
    {
        ArgumentNullException.ThrowIfNull(artifact);
        ArgumentNullException.ThrowIfNull(openRead);
        AddPin(artifact.key);
        return CreateArtifactLease(artifact, () => OpenPinned(artifact.key, openRead), () => Release(artifact.key));
    }

    private void AddPin(AssetArtifactKey key)
    {
        lock (m_sync)
        {
            m_counts.TryGetValue(key, out int count);
            m_counts[key] = checked(count + 1);
        }
    }

    /// <summary>
    /// Captures the keys a collector must preserve regardless of catalog reachability.
    /// </summary>
    /// <returns>
    /// An immutable snapshot; the provider serializes collection with new acquisitions.
    /// </returns>
    public IReadOnlyList<AssetArtifactKey> GetRetainedKeys()
    {
        lock (m_sync)
            return Array.AsReadOnly(m_counts.Keys.ToArray());
    }

    private Stream OpenPinned(
        AssetArtifactKey key,
        Func<Stream> openRead
    ) {
        AddPin(key);
        try
        {
            return new OwnedReadStream(openRead(), new ArtifactPin(this, key));
        }
        catch
        {
            Release(key);
            throw;
        }
    }

    private void Release(AssetArtifactKey key)
    {
        lock (m_sync)
        {
            int remaining = m_counts[key] - 1;
            if (remaining == 0)
                m_counts.Remove(key);
            else
                m_counts[key] = remaining;
        }
    }

    private sealed class ArtifactPin(
        ArtifactRetention owner,
        AssetArtifactKey key
    ) : IDisposable
    {
        private ArtifactRetention? m_owner = owner;

        public void Dispose() => System.Threading.Interlocked.Exchange(ref m_owner, null)?.Release(key);
    }
}
