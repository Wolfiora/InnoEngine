using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Content;
using Inno.Core.IO;
using Xunit;

namespace Inno.Adapter.Content.FileSystem.Tests;

public sealed class ContentCachePublicationTests
{
    [Fact]
    public async Task SourceFailurePreservesThePreviousPointerAndRemovesIncompleteStaging()
    {
        using var fixture = new ContentCacheFixture();
        using (FileContentStore initial = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options))
        {
        }
        string previous = fixture.current;
        File.Delete(Path.Combine(previous, "Artifacts", "aa", "bb", "data"));
        fixture.source.onRead = () => throw new IOException("Expected source failure.");
        await Assert.ThrowsAsync<IOException>(() =>
            FileContentPreparation.PrepareAsync(fixture.source, fixture.options).AsTask());
        Assert.Equal(previous, fixture.current);
        Assert.Empty(Directory.GetDirectories(Path.Combine(fixture.cache, "staging")));
        fixture.source.onRead = null;
        using FileContentStore repaired = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        Assert.NotEqual(previous, fixture.current);
    }

    [Fact]
    public async Task AWriterLeaseEnforcesBoundedWaitingAndCancellationWithoutChangingContent()
    {
        using var fixture = new ContentCacheFixture();
        using FileLease writer = await FileLease.AcquireAsync(Path.Combine(fixture.cache, "install.lock"),
            TimeSpan.Zero);
        await Assert.ThrowsAsync<TimeoutException>(() => FileContentPreparation.PrepareAsync(fixture.source,
            new FileContentCacheOptions(fixture.root, TimeSpan.FromMilliseconds(40))).AsTask());
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(40));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            FileContentPreparation.PrepareAsync(fixture.source, fixture.options, cancellation.Token).AsTask());
        Assert.False(File.Exists(Path.Combine(fixture.cache, "current")));
    }

    [Fact]
    public async Task UnknownOldGenerationOwnershipIsReportedAfterTheNewContentHasCommitted()
    {
        using var fixture = new ContentCacheFixture();
        using (FileContentStore initial = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options))
        {
        }
        string previous = fixture.current;
        File.Delete(Path.Combine(fixture.cache, "leases", Path.GetFileName(previous) + ".lock"));
        using FileContentStore repaired = await FileContentPreparation.PrepareAsync(fixture.source, fixture.options);
        Assert.NotEqual(previous, fixture.current);
        Assert.NotEmpty(repaired.retirementDiagnostics);
        using ContentReadLease lease = repaired.Acquire(new ContentKey("Artifacts/aa/bb/data"));
        using Stream input = lease.OpenRead();
        Assert.Equal(4, input.ReadByte());
        Assert.True(Directory.Exists(previous));
    }
}
