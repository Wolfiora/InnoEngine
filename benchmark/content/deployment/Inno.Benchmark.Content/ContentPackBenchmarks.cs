using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Inno.Benchmark.Runner;
using Inno.Content;
using Inno.Core.Serialization;
using Inno.Runtime;

namespace Inno.Benchmark.Content;

/// <summary>
/// Measures immutable content pack open, lease and read costs through the public reader boundary.
/// </summary>
/// <remarks>
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
public class ContentPackBenchmarks
{
    private readonly byte[] m_buffer = new byte[81_920];
    private EngineHost m_host = null!;
    private SerializationGeneration m_generation = null!;
    private string m_root = string.Empty;
    private string m_packPath = string.Empty;
    private ContentPackDescriptor m_descriptor = null!;
    private ContentKey[] m_keys = [];
    private PackContentStore m_store = null!;

    [Params(64, 1024)]
    public int entryCount;

    [Params(4096)]
    public int payloadBytes;

    [GlobalSetup]
    public void Setup()
    {
        m_host = CreateHost();
        m_generation = m_host.serialization.CaptureGeneration();
        m_root = BenchmarkPaths.CreateRoot("ContentPack");
        (m_packPath, m_descriptor, m_keys) = WritePack(m_root, m_generation, entryCount, payloadBytes);
        m_store = Open();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        m_store.Dispose();
        m_generation.Dispose();
        m_host.Dispose();
        BenchmarkPaths.Delete(m_root);
    }

    [Benchmark(Description = "Open pack (validate index, cold store)")]
    public int OpenPack()
    {
        using PackContentStore store = Open();
        return store.index.entries.Count;
    }

    [Benchmark(Description = "Acquire+release every lease (warm store)")]
    public int AcquireLeases()
    {
        int total = 0;
        foreach (ContentKey key in m_keys)
        {
            using ContentReadLease lease = m_store.Acquire(key);
            total += (int)lease.entry.length;
        }
        return total;
    }

    [Benchmark(Description = "Acquire+read every entry (warm store)")]
    public long ReadAll() => ReadAll(m_store);

    [Benchmark(Description = "Open+read every entry (cold store)")]
    public long OpenAndReadAll()
    {
        using PackContentStore store = Open();
        return ReadAll(store);
    }

    internal static EngineHost CreateHost()
        => new EngineHostBuilder().UseMetadataSources(
            new StaticAssemblyCatalogSource([], [typeof(object).Assembly]),
            new StaticTypeCatalogSource([]),
            new StaticSerializationMetadataSource([])
        ).Build();

    internal static (string path, ContentPackDescriptor descriptor, ContentKey[] keys) WritePack(
        string root,
        SerializationGeneration generation,
        int count,
        int payloadBytes
    ) {
        var random = new Random(17);
        var entries = new List<ContentEntry>(count);
        var keys = new ContentKey[count];
        var output = new MemoryStream();
        using (var archive = new ZipArchive(output, ZipArchiveMode.Create, true))
        {
            for (int index = 0; index < count; index++)
            {
                var payload = new byte[payloadBytes];
                random.NextBytes(payload);
                string name = $"content/{index:D5}.bin";
                keys[index] = new ContentKey(name);
                entries.Add(new ContentEntry(keys[index], payload.Length, Convert.ToHexString(SHA256.HashData(payload))));
                using Stream destination = archive.CreateEntry(name, CompressionLevel.Optimal).Open();
                destination.Write(payload);
            }
            using Stream indexStream = archive.CreateEntry(ContentPackIndex.C_INDEX_KEY).Open();
            indexStream.Write(ContentPackIndexCodec.Encode(new ContentPackIndex(entries), generation));
        }
        byte[] bytes = output.ToArray();
        string hash = Convert.ToHexString(SHA256.HashData(bytes));
        var descriptor = new ContentPackDescriptor(hash, $"content-{hash}.pack");
        string path = Path.Combine(root, descriptor.fileName);
        File.WriteAllBytes(path, bytes);
        return (path, descriptor, keys);
    }

    private PackContentStore Open() => ContentPackReader.Open(File.OpenRead(m_packPath), m_descriptor, m_generation);

    private long ReadAll(PackContentStore store)
    {
        long total = 0;
        foreach (ContentKey key in m_keys)
        {
            using ContentReadLease lease = store.Acquire(key);
            using Stream input = lease.OpenRead();
            int read;
            while ((read = input.Read(m_buffer)) > 0)
            {
                total += read;
            }
        }
        return total;
    }
}
