using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Adapter.Content.FileSystem.Tests;

public sealed class ContentCacheCancellationTests
{
    [Fact]
    public async Task CancellationDuringCopyDiscardsCandidateAndPreservesThePreviousPointer()
    {
        using var fixture = new ContentCacheFixture();
        using (FileContentStore first = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options))
        {
        }
        string previous = fixture.current;
        File.Delete(Path.Combine(previous, "AssetDatabase", "Catalog.snapshot"));
        using var cancellation = new CancellationTokenSource();
        fixture.source.onRead = cancellation.Cancel;
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => FileContentPreparation.PrepareAsync(
            fixture.source,
            fixture.options,
            cancellation.Token
        ).AsTask());
        Assert.Equal(previous, fixture.current);
        Assert.Empty(Directory.GetDirectories(Path.Combine(fixture.cache, "staging")));
        fixture.source.onRead = null;
        using FileContentStore repaired = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        Assert.NotEqual(previous, fixture.current);
    }

    [Fact]
    public async Task PriorCancellationCreatesNoCacheDirectory()
    {
        using var fixture = new ContentCacheFixture();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => FileContentPreparation.PrepareAsync(
            fixture.source,
            fixture.options,
            new CancellationToken(true)
        ).AsTask());
        Assert.False(Directory.Exists(fixture.root));
    }
}
