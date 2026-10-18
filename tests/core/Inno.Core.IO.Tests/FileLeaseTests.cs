using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Xunit;

namespace Inno.Core.IO.Tests;

public sealed class FileLeaseTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoFileLeaseTests", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task ReadersShareOwnershipAndExcludeWritersUntilEveryReaderCloses()
    {
        string path = Path.Combine(m_root, "generation.lock");
        (await FileLease.AcquireAsync(path, TimeSpan.Zero)).Dispose();
        FileLease first = await FileLease.AcquireSharedAsync(path, TimeSpan.Zero);
        using FileLease second = await FileLease.AcquireSharedAsync(path, TimeSpan.Zero);
        await Assert.ThrowsAsync<TimeoutException>(() => FileLease.AcquireAsync(path, TimeSpan.Zero).AsTask());
        first.Dispose();
        await Assert.ThrowsAsync<TimeoutException>(() => FileLease.AcquireAsync(path, TimeSpan.Zero).AsTask());
        second.Dispose();
        using FileLease writer = await FileLease.AcquireAsync(path, TimeSpan.Zero);
        await Assert.ThrowsAsync<TimeoutException>(() => FileLease.AcquireSharedAsync(path, TimeSpan.Zero).AsTask());
    }

    [Fact]
    public async Task CompetingOwnershipWaitsForDisposalAndRetainsTheSameLeaseFile()
    {
        string path = Path.Combine(m_root, "output.lock");
        FileLease owner = await FileLease.AcquireAsync(path, TimeSpan.Zero);
        Task<FileLease> waiting = FileLease.AcquireAsync(path, TimeSpan.FromSeconds(5)).AsTask();
        Assert.False(waiting.IsCompleted);
        owner.Dispose();
        owner.Dispose();
        using FileLease next = await waiting;
        Assert.True(File.Exists(path));
    }

    [Fact]
    public async Task TimeoutAndCancellationDoNotReleaseAnotherOwner()
    {
        string path = Path.Combine(m_root, "output.lock");
        using FileLease owner = await FileLease.AcquireAsync(path, TimeSpan.Zero);
        await Assert.ThrowsAsync<TimeoutException>(() => FileLease.AcquireAsync(path, TimeSpan.Zero).AsTask());
        using CancellationTokenSource cancellation = new();
        Task<FileLease> waiting = FileLease.AcquireAsync(path, Timeout.InfiniteTimeSpan, cancellation.Token).AsTask();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => waiting);
        await Assert.ThrowsAsync<TimeoutException>(() => FileLease.AcquireAsync(path, TimeSpan.Zero).AsTask());
    }

    [Fact]
    public async Task InvalidRequestsAndPriorCancellationHaveNoFilesystemEffects()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => FileLease.AcquireAsync("relative.lock", TimeSpan.Zero).AsTask());
        string path = Path.Combine(m_root, "output.lock");
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => FileLease.AcquireAsync(path, TimeSpan.FromSeconds(-2)).AsTask());
        using CancellationTokenSource cancellation = new();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => FileLease.AcquireAsync(path, TimeSpan.Zero, cancellation.Token).AsTask());
        Assert.False(Directory.Exists(m_root));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
