using System;
using System.IO;
using Inno.Core.Execution;

namespace Inno.Assets;

/// <summary>
/// Keeps one verified immutable artifact generation available for an explicit lifetime.
/// </summary>
public sealed class ArtifactLease : IDisposable
{
    private readonly ResidencyLease<AssetArtifactInfo> m_residency;
    private readonly object m_sync = new();
    private Func<Stream>? m_openRead;

    internal ArtifactLease(
        AssetArtifactInfo artifact,
        Func<Stream> openRead,
        Action release
    ) {
        ArgumentNullException.ThrowIfNull(openRead);
        m_residency = new ResidencyLease<AssetArtifactInfo>(artifact, release);
        m_openRead = openRead;
    }

    /// <summary>
    /// Gets verified metadata for the retained artifact.
    /// </summary>
    public AssetArtifactInfo info => m_residency.value;

    /// <summary>
    /// Opens a read-only stream over the retained immutable artifact.
    /// </summary>
    /// <returns>
    /// A new independently owned read stream.
    /// </returns>
    public Stream OpenRead()
    {
        lock (m_sync)
        {
            _ = m_residency.value;
            return (m_openRead ?? throw new ObjectDisposedException(nameof(ArtifactLease)))();
        }
    }

    /// <summary>
    /// Reads one indexed payload through this lease's independently pinned stream.
    /// </summary>
    /// <returns>
    /// Newly owned bytes with the exact indexed length; oversized or truncated payloads fail explicitly.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The payload exceeds the supported array size or does not have its indexed length.
    /// </exception>
    public byte[] ReadAllBytes()
    {
        long length = info.length;
        if (length < 0 || length > Array.MaxLength)
        {
            throw new InvalidDataException("The artifact payload exceeds the supported array budget.");
        }
        using Stream input = OpenRead();
        var bytes = new byte[(int)length];
        input.ReadExactly(bytes);
        if (input.ReadByte() != -1)
        {
            throw new InvalidDataException("The artifact payload exceeds its indexed length.");
        }
        return bytes;
    }

    /// <summary>
    /// Releases artifact ownership, retaining metadata and callback while the provider reports Pending.
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
    /// New reads stop when release begins; already opened streams retain their independent provider pins.
    /// </remarks>
    public void Dispose()
    {
        lock (m_sync)
        {
            m_openRead = null;
        }
        m_residency.Dispose();
    }
}
