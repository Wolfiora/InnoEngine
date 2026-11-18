using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Adapter.Content.FileSystem.Tests;

public sealed class ContentCacheConcurrencyTests
{
    [Fact]
    public async Task ConcurrentPreparationPublishesOneCompleteGeneration()
    {
        using ContentCacheFixture fixture = new();
        FileContentStore[] readers = await Task.WhenAll(Enumerable.Range(0, 8).Select(_ =>
            FileContentPreparation.PrepareAsync(fixture.source, fixture.options).AsTask()));
        try
        {
            Assert.Single(Directory.GetDirectories(Path.Combine(fixture.cache, "generations")));
            Assert.Empty(Directory.GetDirectories(Path.Combine(fixture.cache, "staging")));
            Assert.All(readers, static reader => Assert.Equal(2, reader.index.entries.Count));
        }
        finally
        {
            foreach (FileContentStore reader in readers)
                reader.Dispose();
        }
    }
}
