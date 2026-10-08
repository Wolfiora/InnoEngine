using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Inno.Audio;
using Inno.Core.Execution;

namespace Inno.Adapter.Audio.MiniAudio;

internal sealed class MiniAudioEncodedArtifactCache : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "AudioEncoded-" + Guid.NewGuid().ToString("N"));
    private readonly Dictionary<string, MiniAudioClipPreparation> m_entries = new(StringComparer.Ordinal);
    private readonly HashSet<MiniAudioClipPreparation> m_retiring = [];
    private readonly SemaphoreSlim m_slots = new(4);
    private bool m_disposed;

    internal MiniAudioClipSourceLease Acquire(IAudioClipSource source)
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        ArgumentNullException.ThrowIfNull(source);
        if (source.length < 0 || source.contentHash is not { Length: 64 }
            || source.contentHash.Any(static character => !Uri.IsHexDigit(character)))
        {
            throw new ArgumentException("Encoded audio requires an exact length and SHA-256 identity.", nameof(source));
        }
        string hash = source.contentHash.ToUpperInvariant();
        if (!m_entries.TryGetValue(hash, out MiniAudioClipPreparation? entry))
        {
            entry = new MiniAudioClipPreparation(source,
                Path.Combine(m_root, hash + "-" + Guid.NewGuid().ToString("N") + ".audio"), m_slots);
            m_entries.Add(hash, entry);
        }
        else if (entry.length != source.length)
        {
            throw new InvalidDataException("The same encoded audio identity declares different lengths.");
        }
        entry.references++;
        return new MiniAudioClipSourceLease(this, entry);
    }

    internal void Release(MiniAudioClipPreparation entry)
    {
        if (entry.references > 1)
        {
            if (!entry.completion.IsCompleted)
                throw new RetirementPendingException("Shared encoded audio preparation still borrows its first clip source.");
            entry.references--;
            return;
        }
        entry.Cancel();
        if (!entry.completion.IsCompleted)
        {
            RemoveActiveEntry(entry);
            m_retiring.Add(entry);
            throw new RetirementPendingException("Encoded audio preparation is still releasing its source reader.");
        }
        entry.Dispose();
        RemoveActiveEntry(entry);
        m_retiring.Remove(entry);
        entry.references = 0;
    }

    internal void CancelPreparation()
    {
        foreach (MiniAudioClipPreparation entry in m_entries.Values)
            entry.Cancel();
        foreach (MiniAudioClipPreparation entry in m_retiring)
            entry.Cancel();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (m_disposed)
            return;
        CancelPreparation();
        if (m_entries.Count != 0 || m_retiring.Count != 0)
            throw new RetirementPendingException("Encoded audio still has clip owners awaiting retirement.");
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: false);
        m_slots.Dispose();
        m_disposed = true;
    }

    private void RemoveActiveEntry(MiniAudioClipPreparation entry)
    {
        if (m_entries.TryGetValue(entry.contentHash, out MiniAudioClipPreparation? current)
            && ReferenceEquals(current, entry))
        {
            m_entries.Remove(entry.contentHash);
        }
    }
}
