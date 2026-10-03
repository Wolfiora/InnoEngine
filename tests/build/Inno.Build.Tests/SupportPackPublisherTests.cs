using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.SupportPacks;
using Xunit;

namespace Inno.Build.Tests;

public sealed class SupportPackPublisherTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoPackTransactionTests", Guid.NewGuid().ToString("N"));
    private static readonly BuildTargetId S_TARGET = new("tests-target");

    public SupportPackPublisherTests()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    [Fact]
    public async Task RegisteredCustomTargetInstallsWithoutCorePlatformBranches()
    {
        var publisher = new PlayerSupportPackPublisher([new TestSource(null)]);
        string installed = await publisher.PublishAsync(m_root, Path.Combine(m_root, "Packs"), S_TARGET, "unused");
        Assert.True(File.Exists(Path.Combine(installed, "References", "Inno.Test.dll")));
        Assert.Single(Directory.EnumerateDirectories(Path.Combine(m_root, "Packs")));
    }

    [Fact]
    public async Task FailedPreparationPreservesInstalledPackAndRemovesStaging()
    {
        string output = Path.Combine(m_root, "Packs");
        string installed = Path.Combine(output, S_TARGET.value);
        Directory.CreateDirectory(installed);
        File.WriteAllText(Path.Combine(installed, "last-good"), "preserve");
        var publisher = new PlayerSupportPackPublisher([new TestSource(new InvalidDataException("invalid candidate"))]);
        await Assert.ThrowsAsync<InvalidDataException>(() => publisher.PublishAsync(m_root, output, S_TARGET, "unused").AsTask());
        Assert.Equal("preserve", File.ReadAllText(Path.Combine(installed, "last-good")));
        Assert.Single(Directory.EnumerateDirectories(output));
    }

    [Fact]
    public async Task CancellationRejectsCandidateBeforeInstallation()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var publisher = new PlayerSupportPackPublisher([new TestSource(null)]);
        string output = Path.Combine(m_root, "Packs");
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => publisher.PublishAsync(m_root, output, S_TARGET, "unused", cancellation.Token).AsTask());
        Assert.Empty(Directory.EnumerateDirectories(output));
    }

    [Fact]
    public void DuplicateTargetRegistrationsFailBeforeAnyBuild()
        => Assert.Throws<ArgumentException>(() => new PlayerSupportPackPublisher([new TestSource(null), new TestSource(null)]));

    [Fact]
    public async Task CancellationDuringValidationPreservesTheInstalledPack()
    {
        using var cancellation = new CancellationTokenSource();
        string output = Path.Combine(m_root, "Packs");
        string installed = Path.Combine(output, S_TARGET.value);
        Directory.CreateDirectory(installed);
        File.WriteAllText(Path.Combine(installed, "last-good"), "preserve");
        var publisher = new PlayerSupportPackPublisher([new TestSource(null, cancellation.Cancel)]);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => publisher.PublishAsync(m_root, output, S_TARGET, "unused", cancellation.Token).AsTask());

        Assert.Equal("preserve", File.ReadAllText(Path.Combine(installed, "last-good")));
        Assert.Single(Directory.EnumerateDirectories(output));
    }

    [Fact]
    public async Task SuccessfulReplacementUsesTheSharedDirectoryCommitAndRemovesItsBackup()
    {
        string output = Path.Combine(m_root, "Packs");
        string installed = Path.Combine(output, S_TARGET.value);
        Directory.CreateDirectory(installed);
        File.WriteAllText(Path.Combine(installed, "last-good"), "previous");
        var publisher = new PlayerSupportPackPublisher([new TestSource(null)]);

        Assert.Equal(installed, await publisher.PublishAsync(m_root, output, S_TARGET, "unused"));

        Assert.False(File.Exists(Path.Combine(installed, "last-good")));
        Assert.Equal("candidate", File.ReadAllText(Path.Combine(installed, "References", "Inno.Test.dll")));
        Assert.Single(Directory.EnumerateDirectories(output));
    }

    [Fact]
    public async Task BackupCleanupFailureExplicitlyPreservesTheInstalledSupportPack()
    {
        if (!OperatingSystem.IsWindows())
            return;

        string output = Path.Combine(m_root, "Packs");
        string installed = Path.Combine(output, S_TARGET.value);
        Directory.CreateDirectory(installed);
        string retained = Path.Combine(installed, "locked.txt");
        File.WriteAllText(retained, "previous");
        File.SetAttributes(retained, FileAttributes.ReadOnly);
        var publisher = new PlayerSupportPackPublisher([new TestSource(null)]);
        try
        {
            IOException failure = await Assert.ThrowsAsync<IOException>(
                () => publisher.PublishAsync(m_root, output, S_TARGET, "unused").AsTask());

            Assert.Contains("installed", failure.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Equal("candidate", File.ReadAllText(Path.Combine(installed, "References", "Inno.Test.dll")));
            string backup = Assert.Single(Directory.EnumerateDirectories(output, S_TARGET.value + ".backup-*"));
            Assert.Equal("previous", File.ReadAllText(Path.Combine(backup, "locked.txt")));
            Assert.Empty(Directory.EnumerateDirectories(output, ".support-pack-*"));
        }
        finally
        {
            foreach (string file in Directory.EnumerateFiles(output, "locked.txt", SearchOption.AllDirectories))
                File.SetAttributes(file, FileAttributes.Normal);
        }
    }

    private sealed class TestSource(
        Exception? failure,
        Action? afterValidation = null
    ) : IPlayerSupportPackSource {
        public BuildTargetId target => S_TARGET;

        public ValueTask PrepareAsync(
            PlayerSupportPackBuildContext context,
            CancellationToken cancellationToken
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            string references = Path.Combine(context.stagingDirectory, "References");
            Directory.CreateDirectory(references);
            File.WriteAllText(Path.Combine(references, "Inno.Test.dll"), "candidate");
            if (failure is not null)
                throw failure;
            return ValueTask.CompletedTask;
        }

        public void Validate(string directory)
        {
            if (!File.Exists(Path.Combine(directory, "References", "Inno.Test.dll")))
                throw new InvalidDataException("Candidate is incomplete.");
            afterValidation?.Invoke();
        }
    }
}
