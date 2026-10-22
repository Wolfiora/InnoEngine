using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Core.IO;

namespace Inno.Core.IO.Tests;

public sealed class CoordinatedFileLeaseTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoCoordinatedLease", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task WaitingAndHoldingParticipantsKeepTheOwnerGateUntilTheLeafIsReleased()
    {
        string owner = Path.Combine(m_root, "owner.lock");
        string leaf = Path.Combine(m_root, "generation.lock");
        using FileLease producer = await FileLease.AcquireAsync(leaf, TimeSpan.Zero);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        Task<CoordinatedFileLease> waiting = CoordinatedFileLease.AcquireAsync(owner, leaf,
            Timeout.InfiniteTimeSpan, cancellationToken: cancellation.Token).AsTask();
        Assert.False(waiting.IsCompleted);
        await Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            using FileLease retirement = await FileLease.AcquireAsync(owner, TimeSpan.Zero);
        });
        producer.Dispose();
        using CoordinatedFileLease acquired = await waiting;
        await Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            using FileLease retirement = await FileLease.AcquireAsync(owner, TimeSpan.Zero);
        });
        acquired.Dispose();
        using (FileLease retirement = await FileLease.AcquireAsync(owner, TimeSpan.Zero))
            File.Delete(leaf);
        Assert.True(File.Exists(owner));
        using CoordinatedFileLease next = await CoordinatedFileLease.AcquireAsync(owner, leaf, TimeSpan.Zero);
    }

    [Fact]
    public async Task CancellationReleasesTheOwnerAndPreservesPersistentFiles()
    {
        string owner = Path.Combine(m_root, "owner.lock");
        string leaf = Path.Combine(m_root, "generation.lock");
        using FileLease producer = await FileLease.AcquireAsync(leaf, TimeSpan.Zero);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await CoordinatedFileLease.AcquireAsync(owner, leaf, Timeout.InfiniteTimeSpan, cancellationToken: cancellation.Token));
        using FileLease retirement = await FileLease.AcquireAsync(owner, TimeSpan.Zero);
        Assert.True(File.Exists(leaf));
    }

    [Fact]
    public async Task SharedReadersPreventOwnerRetirementAndExclusiveLeafAccess()
    {
        string owner = Path.Combine(m_root, "owner.lock");
        string leaf = Path.Combine(m_root, "generation.lock");
        using CoordinatedFileLease first = await CoordinatedFileLease.AcquireAsync(owner, leaf, TimeSpan.Zero, shared: true);
        using CoordinatedFileLease second = await CoordinatedFileLease.AcquireAsync(owner, leaf, TimeSpan.Zero, shared: true);
        await Assert.ThrowsAsync<TimeoutException>(async () =>
            await CoordinatedFileLease.AcquireAsync(owner, leaf, TimeSpan.Zero));
        await Assert.ThrowsAsync<TimeoutException>(async () =>
            await FileLease.AcquireAsync(owner, TimeSpan.Zero));
    }

    [Fact]
    public async Task EscapedAndAliasedOwnerPathsDoNotCreateLeafFiles()
    {
        string owner = Path.Combine(m_root, "owner.lock");
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await CoordinatedFileLease.AcquireAsync(owner, owner, TimeSpan.Zero));
        await Assert.ThrowsAnyAsync<Exception>(async () =>
            await CoordinatedFileLease.AcquireAsync(owner, Path.GetFullPath("../outside.lock", m_root), TimeSpan.Zero));
        Assert.False(Directory.Exists(m_root));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
