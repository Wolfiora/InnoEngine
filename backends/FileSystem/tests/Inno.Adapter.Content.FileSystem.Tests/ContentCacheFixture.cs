using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Inno.Content;

namespace Inno.Adapter.Content.FileSystem.Tests;

internal sealed class ContentCacheFixture : IDisposable
{
    internal readonly string root = Path.Combine(Path.GetTempPath(), "inno-content-cache-" + Guid.NewGuid().ToString("N"));
    internal readonly MemoryContentStore source = new(new Dictionary<string, byte[]>
    {
        ["AssetDatabase/Catalog.snapshot"] = [1, 2, 3],
        ["Artifacts/aa/bb/data"] = [4, 5, 6]
    });

    internal string cache => Path.Combine(root, "Content", source.descriptor.contentHash);
    internal string current => Path.Combine(cache, "generations", File.ReadAllText(Path.Combine(cache, "current")));
    internal FileContentCacheOptions options => new(root);

    public void Dispose()
    {
        source.Dispose();
        if (Directory.Exists(root))
            Directory.Delete(root, true);
    }

    internal sealed class MemoryContentStore : IRuntimeContentStore
    {
        private readonly Dictionary<ContentKey, byte[]> m_content;
        private bool m_closed;
        internal Action? onRead;

        internal MemoryContentStore(Dictionary<string, byte[]> content)
        {
            m_content = content.ToDictionary(static pair => new ContentKey(pair.Key), static pair => (byte[])pair.Value.Clone());
            index = new ContentPackIndex(m_content.Select(static pair => new ContentEntry(pair.Key, pair.Value.Length,
                Convert.ToHexString(SHA256.HashData(pair.Value)))));
            string hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join(";",
                index.entries.Select(static entry => entry.key.value + entry.contentHash)))));
            descriptor = new ContentPackDescriptor(hash, $"content-{hash}.pack");
        }

        public ContentPackDescriptor descriptor { get; }
        public ContentPackIndex index { get; }
        public ContentReadLease Acquire(ContentKey key)
        {
            ObjectDisposedException.ThrowIf(m_closed, this);
            if (!index.TryGetEntry(key, out ContentEntry? entry))
                throw new FileNotFoundException();
            byte[] bytes = m_content[key];
            return new ContentReadLease(entry!, () =>
            {
                onRead?.Invoke();
                return new MemoryStream(bytes, false);
            }, static () => { });
        }
        public void Dispose() => m_closed = true;
    }
}
