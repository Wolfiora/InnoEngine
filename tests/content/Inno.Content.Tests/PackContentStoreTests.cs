using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Inno.Core.Serialization;
using Inno.Runtime;
using Xunit;

namespace Inno.Content.Tests;

public sealed class PackContentStoreTests
{
    [Fact]
    public async Task IndependentReadersRetainPackAfterStoreAndLeaseClose()
    {
        using EngineHost host = CreateHost();
        using SerializationGeneration generation = host.serialization.CaptureGeneration();
        byte[] firstBytes = Enumerable.Range(0, 100_000).Select(static value => (byte)value).ToArray();
        byte[] secondBytes = Enumerable.Range(0, 90_000).Select(static value => (byte)(value * 7)).ToArray();
        (MemoryStream pack, ContentPackDescriptor descriptor) = CreatePack(generation,
            new Dictionary<string, byte[]> { ["a"] = firstBytes, ["b"] = secondBytes });
        PackContentStore store = ContentPackReader.Open(pack, descriptor, generation);
        ContentReadLease first = store.Acquire(new ContentKey("a"));
        ContentReadLease second = store.Acquire(new ContentKey("b"));
        using Stream inputA = first.OpenRead();
        using Stream inputB = second.OpenRead();
        first.Dispose();
        store.Dispose();
        Assert.Throws<ObjectDisposedException>(() => store.Acquire(new ContentKey("a")));
        Assert.Throws<ObjectDisposedException>(() => first.OpenRead());
        await Task.WhenAll(Task.Run(() => Assert.Equal(firstBytes, Read(inputA))),
            Task.Run(() => Assert.Equal(secondBytes, Read(inputB))));
        using Stream anotherB = second.OpenRead();
        Assert.Equal(secondBytes, Read(anotherB));
        second.Dispose();
        inputA.Dispose();
        inputB.Dispose();
        anotherB.Dispose();
        Assert.False(pack.CanRead);
    }

    [Theory]
    [InlineData("hash")]
    [InlineData("extra")]
    [InlineData("missing")]
    [InlineData("length")]
    [InlineData("payload")]
    [InlineData("duplicate")]
    [InlineData("link")]
    [InlineData("traversal")]
    public void MalformedPacksAreRejectedAndTheirInputIsDisposed(string fault)
    {
        using EngineHost host = CreateHost();
        using SerializationGeneration generation = host.serialization.CaptureGeneration();
        (MemoryStream pack, ContentPackDescriptor descriptor) = CreatePack(generation,
            new Dictionary<string, byte[]> { ["a"] = [1, 2, 3] }, fault);
        Assert.Throws<InvalidDataException>(() => ContentPackReader.Open(pack, descriptor, generation));
        Assert.False(pack.CanRead);
    }

    [Fact]
    public void BudgetsAndCancellationPrecedeStorePublication()
    {
        using EngineHost host = CreateHost();
        using SerializationGeneration generation = host.serialization.CaptureGeneration();
        var (pack, descriptor) = CreatePack(generation, new Dictionary<string, byte[]> { ["a"] = [1, 2, 3] });
        Assert.Throws<InvalidDataException>(() => ContentPackReader.Open(pack, descriptor, generation,
            new ContentReadLimits(entryBytes: 2)));
        var (canceled, canceledDescriptor) = CreatePack(generation, new Dictionary<string, byte[]> { ["a"] = [] });
        Assert.Throws<OperationCanceledException>(() => ContentPackReader.Open(canceled, canceledDescriptor, generation,
            cancellationToken: new System.Threading.CancellationToken(true)));
        Assert.False(canceled.CanRead);
    }

    internal static EngineHost CreateHost() => new EngineHostBuilder().UseMetadataSources(
        new StaticAssemblyCatalogSource([], [typeof(object).Assembly]), new StaticTypeCatalogSource([]),
        new StaticSerializationMetadataSource([])).Build();

    internal static (
        MemoryStream pack,
        ContentPackDescriptor descriptor
    ) CreatePack(
        SerializationGeneration generation,
        Dictionary<string, byte[]> payloads,
        string fault = ""
    ) {
        List<ContentEntry> entries = payloads.Select(pair => new ContentEntry(new ContentKey(pair.Key),
            pair.Value.Length + (fault == "length" ? 1 : 0),
            fault == "payload" ? new string('A', 64) : Convert.ToHexString(SHA256.HashData(pair.Value)))).ToList();
        MemoryStream output = new();
        using (ZipArchive archive = new(output, ZipArchiveMode.Create, true))
        {
            foreach (var pair in payloads)
            {
                if (fault == "missing")
                    continue;
                string name = fault == "traversal" ? "../a" : pair.Key;
                ZipArchiveEntry entry = archive.CreateEntry(name);
                if (fault == "link")
                    entry.ExternalAttributes = 0xA000 << 16;
                using Stream destination = entry.Open();
                destination.Write(pair.Value);
            }
            if (fault is "extra" or "duplicate")
            {
                using Stream extra = archive.CreateEntry(fault == "extra" ? "other" : "A").Open();
            }
            using Stream index = archive.CreateEntry(ContentPackIndex.C_INDEX_KEY).Open();
            index.Write(ContentPackIndexCodec.Encode(new ContentPackIndex(entries), generation));
        }
        byte[] bytes = output.ToArray();
        string hash = fault == "hash" ? new string('F', 64) : Convert.ToHexString(SHA256.HashData(bytes));
        output.Position = 0;
        return (output, new ContentPackDescriptor(hash, $"content-{hash}.pack"));
    }

    private static byte[] Read(Stream input)
    {
        using MemoryStream output = new();
        input.CopyTo(output, 137);
        return output.ToArray();
    }
}
