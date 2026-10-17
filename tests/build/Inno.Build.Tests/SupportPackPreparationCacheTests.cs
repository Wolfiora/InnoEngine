using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class SupportPackPreparationCacheTests : IDisposable
{
    private static readonly BuildTargetId S_TARGET = new("fixture-target");
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoPackPreparationCache", Guid.NewGuid().ToString("N"));
    private readonly Source m_source;

    public SupportPackPreparationCacheTests()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), "fixture");
        File.WriteAllText(Path.Combine(m_root, "input.txt"), "first");
        m_source = new Source(Path.Combine(m_root, "input.txt"));
    }

    [Fact]
    public async Task IdenticalPreparationSkipsProducerAndDamagedOutputsRequireRepair()
    {
        string output = Path.Combine(m_root, "Packs");
        var publisher = new PlayerSupportPackPublisher([m_source]);
        string directory;
        using (PlayerSupportPackPublication first = await publisher.PublishAsync(m_root, output, S_TARGET, "unused"))
            directory = first.directory;
        using (PlayerSupportPackPublication repeated = await publisher.PublishAsync(m_root, output, S_TARGET, "unused"))
            Assert.Equal(directory, repeated.directory);
        Assert.Equal(1, m_source.preparations);
        string file = Path.Combine(directory, "References", "Inno.Fixture.dll");
        DateTime timestamp = File.GetLastWriteTimeUtc(file);
        File.WriteAllText(file, "other");
        File.SetLastWriteTimeUtc(file, timestamp);
        using (PlayerSupportPackPublication repaired = await publisher.PublishAsync(m_root, output, S_TARGET, "unused"))
            Assert.Equal("first", File.ReadAllText(Path.Combine(repaired.directory, "References", "Inno.Fixture.dll")));
        Assert.Equal(2, m_source.preparations);
        File.WriteAllText(Path.Combine(directory, "References", "Extra.dll"), "extra");
        using (PlayerSupportPackPublication repaired = await publisher.PublishAsync(m_root, output, S_TARGET, "unused"))
            Assert.Single(Directory.EnumerateFiles(Path.Combine(repaired.directory, "References")));
        Assert.Equal(3, m_source.preparations);
        Assert.Empty(Directory.EnumerateDirectories(output, ".support-pack-*"));
    }

    [Fact]
    public async Task ConcurrentIdenticalRequestsPrepareOnlyOneCandidate()
    {
        string output = Path.Combine(m_root, "Packs");
        var publisher = new PlayerSupportPackPublisher([m_source]);
        PlayerSupportPackPublication[] readers = await Task.WhenAll(Enumerable.Range(0, 4)
            .Select(_ => publisher.PublishAsync(m_root, output, S_TARGET, "unused").AsTask()));
        try
        {
            Assert.All(readers, reader => Assert.Equal(readers[0].directory, reader.directory));
            Assert.Equal(1, m_source.preparations);
        }
        finally
        {
            foreach (PlayerSupportPackPublication reader in readers)
                reader.Dispose();
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task InputMutationDuringOutputValidationRejectsSelectionAndPreservesTheCurrentPack(bool cacheHit)
    {
        string output = Path.Combine(m_root, "Packs");
        var publisher = new PlayerSupportPackPublisher([m_source]);
        if (cacheHit)
        {
            using PlayerSupportPackPublication prior = await publisher.PublishAsync(m_root, output, S_TARGET, "unused");
        }
        string current = Path.Combine(output, S_TARGET.value, "current");
        string? previous = File.Exists(current) ? File.ReadAllText(current) : null;
        m_source.onValidate = () => File.WriteAllText(Path.Combine(m_root, "input.txt"), "other");
        await Assert.ThrowsAsync<InvalidOperationException>(() => publisher.PublishAsync(m_root, output, S_TARGET, "unused").AsTask());
        Assert.Equal(1, m_source.preparations);
        Assert.Equal(previous, File.Exists(current) ? File.ReadAllText(current) : null);
        Assert.Empty(Directory.EnumerateDirectories(output, ".support-pack-*"));
    }

    [Fact]
    public async Task PreparationIndexFailureReportsCommittedPublicationWithoutThrowing()
    {
        string output = Path.Combine(m_root, "Packs");
        var publisher = new PlayerSupportPackPublisher([m_source]);
        using (PlayerSupportPackPublication first = await publisher.PublishAsync(m_root, output, S_TARGET, "unused"))
            Assert.Empty(first.diagnostics);
        string current = Path.Combine(output, S_TARGET.value, "current");
        string previous = File.ReadAllText(current);
        File.WriteAllText(Path.Combine(m_root, "input.txt"), "other");
        m_source.onPrepared = _ =>
        {
            string index = Path.Combine(output, S_TARGET.value, ".preparations");
            Directory.Move(index, index + ".saved");
            File.WriteAllText(index, "blocked");
        };
        using PlayerSupportPackPublication published = await publisher.PublishAsync(m_root, output, S_TARGET, "unused");
        Assert.NotEqual(previous, File.ReadAllText(current));
        Assert.Contains(published.diagnostics, static message => message.Contains("preparation index", StringComparison.Ordinal));
        Assert.Equal("other", File.ReadAllText(Path.Combine(published.directory, "References", "Inno.Fixture.dll")));
        using PlayerSupportPackLease selected = new PlayerSupportPackCatalog(output).Acquire(S_TARGET, m_source);
        Assert.Equal(published.directory, selected.directory);
    }

    [Fact]
    public async Task PostCommitStagingCleanupFailurePreservesReadingOwnership()
    {
        if (!OperatingSystem.IsWindows())
            return;
        string output = Path.Combine(m_root, "Packs");
        FileStream? blocker = null;
        m_source.onPrepared = staging =>
        {
            string file = Path.Combine(Path.GetDirectoryName(staging)!, "owned-cleanup.bin");
            blocker = new FileStream(file, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        };
        try
        {
            using PlayerSupportPackPublication published = await new PlayerSupportPackPublisher([m_source])
                .PublishAsync(m_root, output, S_TARGET, "unused");
            Assert.Contains(published.diagnostics, static message => message.Contains("could not be removed", StringComparison.Ordinal));
            Assert.True(File.Exists(Path.Combine(published.directory, "References", "Inno.Fixture.dll")));
            ProcessOwnedDirectoryDescriptor retained = Assert.Single(ProcessOwnedDirectory.ReadClaims(output));
            Assert.True(File.Exists(Path.Combine(retained.directory, "owned-cleanup.bin")));
        }
        finally
        {
            blocker?.Dispose();
        }
    }

    [Fact]
    public async Task ChangedInputAfterOwnershipWaitNeverEntersPreparation()
    {
        string output = Path.Combine(m_root, "Packs");
        Directory.CreateDirectory(output);
        var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        m_source.onPlan = entered.SetResult;
        Task<PlayerSupportPackPublication> waiting;
        using (FileLease owner = await FileLease.AcquireAsync(Path.Combine(output, S_TARGET.value + ".lock"), TimeSpan.FromSeconds(5)))
        {
            waiting = new PlayerSupportPackPublisher([m_source]).PublishAsync(m_root, output, S_TARGET, "unused").AsTask();
            await entered.Task.WaitAsync(TimeSpan.FromSeconds(5));
            DateTime timestamp = File.GetLastWriteTimeUtc(Path.Combine(m_root, "input.txt"));
            File.WriteAllText(Path.Combine(m_root, "input.txt"), "other");
            File.SetLastWriteTimeUtc(Path.Combine(m_root, "input.txt"), timestamp);
        }
        await Assert.ThrowsAsync<InvalidOperationException>(() => waiting);
        Assert.Equal(0, m_source.preparations);
        Assert.False(File.Exists(Path.Combine(output, S_TARGET.value, "current")));
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    private sealed class Source(string input) : IPlayerSupportPackSource
    {
        private int m_preparations;
        public int preparations => m_preparations;
        public Action? onPlan { get; set; }
        public Action? onValidate { get; set; }
        public Action<string>? onPrepared { get; set; }
        public BuildTargetId target => S_TARGET;

        public ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
            PlayerSupportPackPlanningContext context,
            CancellationToken cancellationToken
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            var plan = new Plan(this, Identity(), File.ReadAllText(input));
            onPlan?.Invoke();
            return ValueTask.FromResult<PlayerSupportPackPlan>(plan);
        }

        public void Validate(string directory)
        {
            Assert.True(File.Exists(Path.Combine(directory, "References", "Inno.Fixture.dll")));
            onValidate?.Invoke();
        }

        private string Identity() => NativeBuildFingerprint.Create(["fixture-preparation"], [new NativeBuildInput("source", input)]);

        private sealed class Plan(
            Source owner,
            string identity,
            string content
        ) : PlayerSupportPackPlan(S_TARGET, identity) {
            public override ValueTask VerifyInputsAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (owner.Identity() != preparationIdentity)
                    throw new InvalidOperationException("Fixture source input changed after planning.");
                return ValueTask.CompletedTask;
            }

            public override ValueTask PrepareAsync(
                string stagingDirectory,
                CancellationToken cancellationToken
            ) {
                cancellationToken.ThrowIfCancellationRequested();
                Interlocked.Increment(ref owner.m_preparations);
                string references = Path.Combine(stagingDirectory, "References");
                Directory.CreateDirectory(references);
                File.WriteAllText(Path.Combine(references, "Inno.Fixture.dll"), content);
                owner.onPrepared?.Invoke(stagingDirectory);
                return ValueTask.CompletedTask;
            }
        }
    }
}
