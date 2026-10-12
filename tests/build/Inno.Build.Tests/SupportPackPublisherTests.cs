using System;
using System.IO;
using System.Linq;
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

    [Theory]
    [InlineData("missing-sdk")]
    [InlineData("wrong-target")]
    [InlineData("null-plan")]
    public async Task PreflightFailuresLeaveNoPublicationDirectory(string failure)
    {
        string output = Path.Combine(m_root, "Packs");
        var source = new PlanningSource((
            context,
            token
        ) => {
            Assert.False(Directory.Exists(output));
            if (failure == "missing-sdk")
                throw new FileNotFoundException("The selected SDK is absent.");
            return ValueTask.FromResult<PlayerSupportPackPlan>(failure == "null-plan"
                ? null! : new ForeignPlan());
        });
        var publisher = new PlayerSupportPackPublisher([source]);

        if (failure == "missing-sdk")
            await Assert.ThrowsAsync<FileNotFoundException>(() => publisher.PublishAsync(m_root, output, S_TARGET, "unused").AsTask());
        else
            await Assert.ThrowsAsync<InvalidDataException>(() => publisher.PublishAsync(m_root, output, S_TARGET, "unused").AsTask());
        Assert.False(Directory.Exists(output));
    }

    [Fact]
    public async Task CancellationDuringPlanningLeavesNoOutputAndExecutesNoPlan()
    {
        using var cancellation = new CancellationTokenSource();
        string output = Path.Combine(m_root, "Packs");
        var source = new PlanningSource((
            context,
            token
        ) => {
            cancellation.Cancel();
            return ValueTask.FromResult<PlayerSupportPackPlan>(new ForeignPlan(S_TARGET));
        });

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => new PlayerSupportPackPublisher([source])
            .PublishAsync(m_root, output, S_TARGET, "unused", cancellation.Token).AsTask());

        Assert.False(Directory.Exists(output));
    }

    [Fact]
    public async Task PreparedPlanIsExecutedOnceWithoutRepeatingDiscovery()
    {
        int planningCount = 0;
        string output = Path.Combine(m_root, "Packs");
        var source = new PlanningSource(async (
            context,
            token
        ) => {
            Assert.False(Directory.Exists(output));
            planningCount++;
            return await new TestSource(null, payload: "frozen").CreatePlanAsync(context, token);
        });

        using PlayerSupportPackLease installedLease = await new PlayerSupportPackPublisher([source]).PublishAsync(m_root, output, S_TARGET, "unused");
        string installed = installedLease.directory;

        Assert.Equal(1, planningCount);
        Assert.Equal("frozen", File.ReadAllText(Path.Combine(installed, "References", "Inno.Test.dll")));
    }

    [Fact]
    public async Task RegisteredCustomTargetInstallsWithoutCorePlatformBranches()
    {
        var publisher = new PlayerSupportPackPublisher([new TestSource(null)]);
        using PlayerSupportPackLease installedLease = await publisher.PublishAsync(m_root, Path.Combine(m_root, "Packs"), S_TARGET, "unused");
        string installed = installedLease.directory;
        Assert.True(File.Exists(Path.Combine(installed, "References", "Inno.Test.dll")));
        Assert.Single(Directory.EnumerateDirectories(Path.Combine(m_root, "Packs")));
    }

    [Fact]
    public async Task FailedPreparationPreservesInstalledPackAndRemovesStaging()
    {
        string output = Path.Combine(m_root, "Packs");
        using PlayerSupportPackLease installedLease = await new PlayerSupportPackPublisher([new TestSource(null)]).PublishAsync(
            m_root, output, S_TARGET, "unused");
        string installed = installedLease.directory;
        var publisher = new PlayerSupportPackPublisher([new TestSource(new InvalidDataException("invalid candidate"))]);
        await Assert.ThrowsAsync<InvalidDataException>(() => publisher.PublishAsync(m_root, output, S_TARGET, "unused").AsTask());
        Assert.Equal("candidate", File.ReadAllText(Path.Combine(installed, "References", "Inno.Test.dll")));
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
        Assert.False(Directory.Exists(output));
    }

    [Fact]
    public void DuplicateTargetRegistrationsFailBeforeAnyBuild()
        => Assert.Throws<ArgumentException>(() => new PlayerSupportPackPublisher([new TestSource(null), new TestSource(null)]));

    [Fact]
    public async Task CancellationDuringValidationPreservesTheInstalledPack()
    {
        using var cancellation = new CancellationTokenSource();
        string output = Path.Combine(m_root, "Packs");
        using PlayerSupportPackLease installedLease = await new PlayerSupportPackPublisher([new TestSource(null)]).PublishAsync(
            m_root, output, S_TARGET, "unused");
        string installed = installedLease.directory;
        var publisher = new PlayerSupportPackPublisher([new TestSource(null, cancellation.Cancel)]);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => publisher.PublishAsync(m_root, output, S_TARGET, "unused", cancellation.Token).AsTask());

        Assert.Equal("candidate", File.ReadAllText(Path.Combine(installed, "References", "Inno.Test.dll")));
        Assert.Single(Directory.EnumerateDirectories(output));
    }

    [Fact]
    public async Task NewPublicationRetainsTheImmutableInputsHeldByEarlierReaders()
    {
        string output = Path.Combine(m_root, "Packs");
        using PlayerSupportPackLease previousLease = await new PlayerSupportPackPublisher([new TestSource(null)]).PublishAsync(
            m_root, output, S_TARGET, "unused");
        string previous = previousLease.directory;
        using FileStream reader = File.OpenRead(Path.Combine(previous, "References", "Inno.Test.dll"));

        using PlayerSupportPackLease currentLease = await new PlayerSupportPackPublisher([new TestSource(null, payload: "updated")]).PublishAsync(
            m_root, output, S_TARGET, "unused");
        string current = currentLease.directory;

        Assert.NotEqual(previous, current);
        Assert.Equal("candidate", File.ReadAllText(Path.Combine(previous, "References", "Inno.Test.dll")));
        Assert.Equal("updated", File.ReadAllText(Path.Combine(current, "References", "Inno.Test.dll")));
        using PlayerSupportPackLease selected = new PlayerSupportPackCatalog(output).Acquire(S_TARGET, new TestSource(null));
        Assert.Equal(current, selected.directory);
        Assert.Empty(Directory.EnumerateDirectories(output, ".support-pack-*"));
    }

    [Fact]
    public async Task IdenticalPublicationReusesTheExistingImmutableDirectory()
    {
        string output = Path.Combine(m_root, "Packs");
        var publisher = new PlayerSupportPackPublisher([new TestSource(null)]);
        using PlayerSupportPackLease firstLease = await publisher.PublishAsync(m_root, output, S_TARGET, "unused");
        string first = firstLease.directory;

        using PlayerSupportPackLease second = await publisher.PublishAsync(m_root, output, S_TARGET, "unused");
        Assert.Equal(first, second.directory);
        Assert.Single(Directory.EnumerateDirectories(Path.Combine(output, S_TARGET.value))
            .Where(static directory => Path.GetFileName(directory) != ".preparations"));
        Assert.Empty(Directory.EnumerateDirectories(output, ".support-pack-*"));
    }

    [Fact]
    public async Task DamagedInstalledArtifactCannotBeSilentlyReplacedWhileReadersHoldIt()
    {
        string output = Path.Combine(m_root, "Packs");
        var publisher = new PlayerSupportPackPublisher([new TestSource(null)]);
        using PlayerSupportPackLease installedLease = await publisher.PublishAsync(m_root, output, S_TARGET, "unused");
        string installed = installedLease.directory;
        string file = Path.Combine(installed, "References", "Inno.Test.dll");
        File.WriteAllText(file, "changed externally");

        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(150));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => publisher.PublishAsync(
            m_root, output, S_TARGET, "unused", cancellation.Token).AsTask());

        Assert.Equal("changed externally", File.ReadAllText(file));
        Assert.Throws<InvalidDataException>(() => new PlayerSupportPackCatalog(output).Acquire(S_TARGET, new TestSource(null)));
        Assert.Empty(Directory.EnumerateDirectories(output, ".support-pack-*"));
        installedLease.Dispose();
        Assert.Throws<ObjectDisposedException>(() => installedLease.directory);
        using PlayerSupportPackLease repaired = await publisher.PublishAsync(m_root, output, S_TARGET, "unused");
        Assert.Equal(installed, repaired.directory);
        Assert.Equal("candidate", File.ReadAllText(Path.Combine(repaired.directory, "References", "Inno.Test.dll")));
    }

    [Fact]
    public async Task WaitingWriterCanCancelWithoutEnteringPreparationOrDisturbingTheCurrentWriter()
    {
        string output = Path.Combine(m_root, "Packs");
        var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var first = new PlayerSupportPackPublisher([new PendingSource(entered, release)]);
        Task<PlayerSupportPackLease> publishing = first.PublishAsync(m_root, output, S_TARGET, "unused").AsTask();
        await entered.Task.WaitAsync(TimeSpan.FromSeconds(5));
        using var cancellation = new CancellationTokenSource();
        var second = new PlayerSupportPackPublisher([new TestSource(new InvalidOperationException("Preparation must not run."))]);
        Task<PlayerSupportPackLease> waiting = second.PublishAsync(m_root, output, S_TARGET, "unused", cancellation.Token).AsTask();
        try
        {
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => waiting);
        }
        finally
        {
            release.SetResult();
        }
        using PlayerSupportPackLease installedLease = await publishing;
        string installed = installedLease.directory;
        using PlayerSupportPackLease selected = new PlayerSupportPackCatalog(output).Acquire(S_TARGET, new TestSource(null));
        Assert.Equal(installed, selected.directory);
        Assert.Empty(Directory.EnumerateDirectories(output, ".support-pack-*"));
    }

    private static string InputIdentity(string input) => Convert.ToHexStringLower(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(input)));

    private sealed class PendingSource(
        TaskCompletionSource entered,
        TaskCompletionSource release
    ) : IPlayerSupportPackSource {
        public BuildTargetId target => S_TARGET;

        public ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
            PlayerSupportPackPlanningContext context,
            CancellationToken cancellationToken
        ) {
            return ValueTask.FromResult<PlayerSupportPackPlan>(new PendingPlan(entered, release));
        }

        private sealed class PendingPlan(
            TaskCompletionSource entered,
            TaskCompletionSource release
        ) : PlayerSupportPackPlan(S_TARGET, InputIdentity("pending")) {
            public override ValueTask VerifyInputsAsync(CancellationToken cancellationToken) => ValueTask.CompletedTask;

            public override async ValueTask PrepareAsync(
                string stagingDirectory,
                CancellationToken cancellationToken
            ) {
                entered.SetResult();
                await release.Task.WaitAsync(cancellationToken);
                PlayerSupportPackPlan plan = await new TestSource(null).CreatePlanAsync(
                    new PlayerSupportPackPlanningContext("unused", "unused"), cancellationToken);
                await plan.PrepareAsync(stagingDirectory, cancellationToken);
            }
        }

        public void Validate(string directory) => new TestSource(null).Validate(directory);
    }

    private sealed class PlanningSource(
        Func<PlayerSupportPackPlanningContext, CancellationToken, ValueTask<PlayerSupportPackPlan>> createPlan
    ) : IPlayerSupportPackSource {
        public BuildTargetId target => S_TARGET;

        public ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
            PlayerSupportPackPlanningContext context,
            CancellationToken cancellationToken
        ) => createPlan(context, cancellationToken);

        public void Validate(string directory) => new TestSource(null).Validate(directory);
    }

    private sealed class ForeignPlan(BuildTargetId? target = null) : PlayerSupportPackPlan(target ?? new("foreign-target"), InputIdentity("foreign"))
    {
        public override ValueTask VerifyInputsAsync(CancellationToken cancellationToken) => ValueTask.CompletedTask;

        public override ValueTask PrepareAsync(
            string stagingDirectory,
            CancellationToken cancellationToken
        ) => throw new Xunit.Sdk.XunitException("An unaccepted plan must never execute.");
    }

    private sealed class TestSource(
        Exception? failure,
        Action? afterValidation = null,
        string payload = "candidate"
    ) : IPlayerSupportPackSource {
        public BuildTargetId target => S_TARGET;

        public ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
            PlayerSupportPackPlanningContext context,
            CancellationToken cancellationToken
        ) => ValueTask.FromResult<PlayerSupportPackPlan>(new TestPlan(failure, payload));

        private sealed class TestPlan(
            Exception? failure,
            string payload
        ) : PlayerSupportPackPlan(S_TARGET, InputIdentity(payload + failure?.GetType().FullName)) {
            public override ValueTask VerifyInputsAsync(CancellationToken cancellationToken) => ValueTask.CompletedTask;

            public override ValueTask PrepareAsync(
                string stagingDirectory,
                CancellationToken cancellationToken
            ) {
                cancellationToken.ThrowIfCancellationRequested();
                string references = Path.Combine(stagingDirectory, "References");
                Directory.CreateDirectory(references);
                File.WriteAllText(Path.Combine(references, "Inno.Test.dll"), payload);
                if (failure is not null)
                    throw failure;
                return ValueTask.CompletedTask;
            }

        }

        public void Validate(string directory)
        {
            if (!File.Exists(Path.Combine(directory, "References", "Inno.Test.dll")))
                throw new InvalidDataException("Candidate is incomplete.");
            afterValidation?.Invoke();
        }
    }
}
