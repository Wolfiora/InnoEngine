using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Inno.Content.Testing;

/// <summary>
/// Supplies controlled immutable content through the real public provider boundary for contract tests.
/// </summary>
public sealed class ContentTestStore : IRuntimeContentStore
{
    private readonly Dictionary<ContentKey, byte[]> m_payloads;
    private bool m_disposed;

    /// <summary>
    /// Copies payloads and freezes their metadata before any fault is injected.
    /// </summary>
    /// <param name="payloads">
    /// The logical content inventory to capture independently of the test's source buffers.
    /// </param>
    public ContentTestStore(IEnumerable<KeyValuePair<ContentKey, byte[]>> payloads)
    {
        m_payloads = payloads.ToDictionary(static pair => pair.Key, static pair => (byte[])pair.Value.Clone());
        index = new ContentPackIndex(m_payloads.Select(static pair => new ContentEntry(
            pair.Key, pair.Value.LongLength, Convert.ToHexString(SHA256.HashData(pair.Value)))));
        string identity = string.Join('\n', index.entries.Select(static entry =>
            entry.key + ":" + entry.length + ":" + entry.contentHash));
        string hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(identity)));
        descriptor = new ContentPackDescriptor(hash, $"content-{hash}.pack");
    }

    /// <inheritdoc />
    public ContentPackDescriptor descriptor { get; }

    /// <inheritdoc />
    public ContentPackIndex index { get; }

    /// <summary>
    /// Captures a test's exported directory as independently owned memory content.
    /// </summary>
    /// <param name="directory">
    /// The fixture output directory; the returned store does not retain its location.
    /// </param>
    /// <returns>
    /// A caller-owned memory provider usable after the directory has been removed.
    /// </returns>
    public static ContentTestStore FromDirectory(string directory)
        => new(Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).Select(path =>
            new KeyValuePair<ContentKey, byte[]>(new ContentKey(Path.GetRelativePath(directory, path)
                .Replace('\\', '/')), File.ReadAllBytes(path))));

    /// <summary>
    /// Injects a provider corruption while preserving its previously declared inventory.
    /// </summary>
    /// <param name="key">
    /// An existing content identity.
    /// </param>
    /// <param name="bytes">
    /// The replacement body; independent readers opened earlier remain pinned to their original bytes.
    /// </param>
    public void CorruptPayload(
        ContentKey key,
        ReadOnlySpan<byte> bytes
    ) {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (!m_payloads.ContainsKey(key))
            throw new KeyNotFoundException(key.ToString());
        m_payloads[key] = bytes.ToArray();
    }

    /// <inheritdoc />
    public ContentReadLease Acquire(ContentKey key)
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (!index.TryGetEntry(key, out ContentEntry? entry))
            throw new KeyNotFoundException(key.ToString());
        byte[] bytes = m_payloads[key];
        return new ContentReadLease(entry!, () => new MemoryStream(bytes, writable: false), static () => { });
    }

    /// <inheritdoc />
    public void Dispose()
    {
        m_disposed = true;
        m_payloads.Clear();
    }
}
