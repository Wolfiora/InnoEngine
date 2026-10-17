using System.IO;
using System.Threading.Tasks;
using Inno.Content;
using Xunit;

namespace Inno.Adapter.Content.FileSystem.Tests;

public sealed class ContentCacheRetirementTests
{
    [Fact]
    public async Task RepairKeepsAnOldReaderGenerationUntilItsLastStreamCloses()
    {
        using ContentCacheFixture fixture = new();
        FileContentStore old = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        string oldDirectory = fixture.current;
        ContentReadLease lease = old.Acquire(new ContentKey("Artifacts/aa/bb/data"));
        using Stream input = lease.OpenRead();
        lease.Dispose();
        old.Dispose();
        File.WriteAllText(Path.Combine(oldDirectory, "unindexed"), "invalidates generation");
        using FileContentStore replacement = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        Assert.NotEqual(oldDirectory, fixture.current);
        Assert.True(Directory.Exists(oldDirectory));
        byte[] bytes = new byte[3];
        input.ReadExactly(bytes);
        Assert.Equal(new byte[] { 4, 5, 6 }, bytes);
        input.Dispose();
        using FileContentStore afterRetirement = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        Assert.False(Directory.Exists(oldDirectory));
    }
}
