using System.IO;
using System.Threading.Tasks;
using Inno.Content;
using Xunit;

namespace Inno.Adapter.Content.FileSystem.Tests;

public sealed class ContentCacheIntegrityTests
{
    [Theory]
    [InlineData("missing")]
    [InlineData("extra")]
    [InlineData("empty-directory")]
    [InlineData("same-length-and-time")]
    [InlineData("bad-pointer")]
    [InlineData("missing-lease")]
    public async Task CompleteValidationRepairsEveryKindOfCacheCorruption(string fault)
    {
        using ContentCacheFixture fixture = new();
        using (FileContentStore first = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options))
        {
        }
        string original = fixture.current;
        string file = Path.Combine(original, "AssetDatabase", "Catalog.snapshot");
        switch (fault)
        {
            case "missing":
                File.Delete(file);
                break;
            case "extra":
                File.WriteAllText(Path.Combine(original, "unindexed"), "bad");
                break;
            case "empty-directory":
                Directory.CreateDirectory(Path.Combine(original, "unindexed"));
                break;
            case "same-length-and-time":
                var time = File.GetLastWriteTimeUtc(file);
                File.WriteAllBytes(file, [3, 2, 1]);
                File.SetLastWriteTimeUtc(file, time);
                break;
            case "bad-pointer":
                File.WriteAllText(Path.Combine(fixture.cache, "current"), "../wrong");
                break;
            case "missing-lease":
                File.Delete(Path.Combine(fixture.cache, "leases", Path.GetFileName(original) + ".lock"));
                break;
        }
        using FileContentStore repaired = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        Assert.NotEqual(original, fixture.current);
        using ContentReadLease lease = repaired.Acquire(new ContentKey("AssetDatabase/Catalog.snapshot"));
        using Stream input = lease.OpenRead();
        byte[] bytes = new byte[3];
        input.ReadExactly(bytes);
        Assert.Equal(new byte[] { 1, 2, 3 }, bytes);
        Assert.Equal(-1, input.ReadByte());
        if (fault == "missing-lease")
        {
            Assert.True(Directory.Exists(original));
            Assert.NotEmpty(repaired.retirementDiagnostics);
        }
        else
        {
            Assert.Single(Directory.GetDirectories(Path.Combine(fixture.cache, "generations")));
            Assert.Empty(repaired.retirementDiagnostics);
        }
    }

    [Fact]
    public async Task ContentChangedAfterPreparationFailsReadingAndReleasesTheFailedStreamPin()
    {
        using ContentCacheFixture fixture = new();
        using FileContentStore store = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        string original = fixture.current;
        string file = Path.Combine(original, "AssetDatabase", "Catalog.snapshot");
        var time = File.GetLastWriteTimeUtc(file);
        File.WriteAllBytes(file, [3, 2, 1]);
        File.SetLastWriteTimeUtc(file, time);
        using (ContentReadLease lease = store.Acquire(new ContentKey("AssetDatabase/Catalog.snapshot")))
            Assert.Throws<InvalidDataException>(() => lease.OpenRead());
        store.Dispose();
        using FileContentStore repaired = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        Assert.False(Directory.Exists(original));
        Assert.Empty(repaired.retirementDiagnostics);
    }

    [Fact]
    public async Task ValidCacheReusePreservesGenerationAndDoesNotReadSourcePayloads()
    {
        using ContentCacheFixture fixture = new();
        using FileContentStore first = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        string generation = fixture.current;
        fixture.source.onRead = () => throw new Xunit.Sdk.XunitException("A verified cache must not recopy payloads.");
        using FileContentStore second = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        Assert.Equal(generation, fixture.current);
    }
}
