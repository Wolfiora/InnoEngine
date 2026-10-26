using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Tests;

public sealed class BuildCacheRetirementTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoCacheRetirement", Guid.NewGuid().ToString("N"));
    private readonly List<OwnedCache> m_owners = [];

    [Fact]
    public async Task UnprotectedProcessWorkIsRetiredWithoutEvictingReusablePayloadsUnderBudget()
    {
        OwnedCache owner = AddOwner("process-work");
        BuildCacheArtifactDescriptor process = owner.Add("exited-process", 2);
        owner.artifacts[0] = new(process.directory, process.leasePath, process.targetId,
            process.bytes, process.lastUseUtc, [], BuildCacheArtifactRole.ProcessWorkingDirectory);
        BuildCacheArtifactDescriptor reusable = owner.Add("reusable", 3);
        BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(m_root, [owner.descriptor], new(long.MaxValue, 0));
        Assert.Equal(process.directory, Assert.Single(plan.candidates).directory);
        Assert.Equal(process.directory, Assert.Single((await plan.ExecuteAsync()).retired));
        Assert.True(Directory.Exists(reusable.directory));
    }

    [Fact]
    public async Task TargetCleanupPreservesOtherTargetsSharedArtifactsAndTheirDependencies()
    {
        OwnedCache owner = AddOwner("targeted");
        BuildCacheArtifactDescriptor shared = owner.Add("shared", 2);
        BuildCacheArtifactDescriptor requiredByMac = owner.Add("windows-dependency", 3, targetId: "windows-x64");
        BuildCacheArtifactDescriptor mac = owner.Add("mac", 4, [requiredByMac.directory], "macos-arm64");
        BuildCacheArtifactDescriptor obsoleteWindows = owner.Add("windows-obsolete", 5, targetId: "windows-x64");
        BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(m_root, [owner.descriptor], new(0, 0),
            retireAll: true, targetId: "windows-x64");
        Assert.Equal(obsoleteWindows.directory, Assert.Single(plan.candidates).directory);
        BuildCacheCleanupResult result = await plan.ExecuteAsync();
        Assert.Equal(obsoleteWindows.directory, Assert.Single(result.retired));
        Assert.True(Directory.Exists(shared.directory));
        Assert.True(Directory.Exists(mac.directory));
        Assert.True(Directory.Exists(requiredByMac.directory));
    }

    [Fact]
    public async Task FreshTargetChangePreventsRetiringAnOriginalCandidate()
    {
        OwnedCache owner = AddOwner("retargeted");
        BuildCacheArtifactDescriptor original = owner.Add("candidate", 2, targetId: "windows-x64");
        BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(m_root, [owner.descriptor], new(0, 0),
            retireAll: true, targetId: "windows-x64");
        owner.artifacts[0] = new(original.directory, original.leasePath, "macos-arm64",
            original.bytes, original.lastUseUtc, original.dependencies, BuildCacheArtifactRole.ImmutableArtifact);
        BuildCacheCleanupResult result = await plan.ExecuteAsync();
        Assert.Empty(result.retired);
        Assert.True(Directory.Exists(original.directory));
    }

    [Fact]
    public async Task CurrentRootsProtectDependenciesAcrossOwners()
    {
        OwnedCache bindings = AddOwner("bindings");
        BuildCacheArtifactDescriptor binding = bindings.Add("binding", 3);
        OwnedCache native = AddOwner("native");
        BuildCacheArtifactDescriptor product = native.Add("product", 4, [binding.directory]);
        BuildCacheArtifactDescriptor obsolete = native.Add("obsolete", 5);
        native.roots.Add(product.directory);
        string unrelated = Path.Combine(m_root, "UserOutput");
        Directory.CreateDirectory(unrelated);
        File.WriteAllText(Path.Combine(unrelated, "keep.dll"), "user output");

        BuildCacheCleanupPlan plan = CreatePlan();
        Assert.Equal(7, plan.protectedBytes);
        Assert.Equal(obsolete.directory, Assert.Single(plan.candidates).directory);
        BuildCacheCleanupResult result = await plan.ExecuteAsync();

        Assert.Equal(obsolete.directory, Assert.Single(result.retired));
        Assert.Equal(5, result.retiredBytes);
        Assert.True(Directory.Exists(binding.directory));
        Assert.True(Directory.Exists(product.directory));
        Assert.True(File.Exists(Path.Combine(unrelated, "keep.dll")));
        Assert.True(File.Exists(obsolete.directory + ".read.lock"));
        Assert.True(File.Exists(native.descriptor.coordinationPath));
    }

    [Fact]
    public async Task ActiveReaderPreventsRetirementUntilItReleasesOwnership()
    {
        OwnedCache owner = AddOwner("native");
        BuildCacheArtifactDescriptor artifact = owner.Add("generation", 6);
        BuildArtifactManifest.Write(artifact.directory, "fixture", ["Outputs"]);
        BuildCacheCleanupPlan plan = CreatePlan();
        using (BuildArtifactReadLease reading = await BuildArtifactReadLease.AcquireAsync(artifact.directory, Path.GetDirectoryName(artifact.directory)!, "fixture", ["Outputs"]))
        {
            BuildCacheCleanupResult blocked = await plan.ExecuteAsync();
            Assert.Empty(blocked.retired);
            Assert.Contains(artifact.directory, blocked.skipped);
            Assert.True(Directory.Exists(artifact.directory));
        }
        BuildCacheCleanupResult completed = await plan.ExecuteAsync();
        Assert.Equal(artifact.directory, Assert.Single(completed.retired));
        Assert.False(Directory.Exists(artifact.directory));
        Assert.Empty(completed.failures);
    }

    [Fact]
    public async Task WorkspaceRetirementUsesTheActualProducerLease()
    {
        OwnedCache owner = AddOwner("workspaces");
        BuildCacheArtifactDescriptor artifact = owner.Add("workspace", 6);
        string producerLease = Path.Combine(owner.descriptor.directory, "producer.lock");
        owner.artifacts[0] = new(artifact.directory, producerLease, artifact.targetId, artifact.bytes, artifact.lastUseUtc, [], BuildCacheArtifactRole.ImmutableArtifact);
        BuildCacheCleanupPlan plan = CreatePlan();
        using (FileLease producer = await FileLease.AcquireAsync(producerLease, TimeSpan.Zero))
        {
            BuildCacheCleanupResult blocked = await plan.ExecuteAsync();
            Assert.Empty(blocked.retired);
            Assert.True(Directory.Exists(artifact.directory));
        }
        BuildCacheCleanupResult completed = await plan.ExecuteAsync();
        Assert.Equal(artifact.directory, Assert.Single(completed.retired));
        Assert.True(File.Exists(producerLease));
        Assert.False(File.Exists(artifact.directory + ".read.lock"));
    }

    [Fact]
    public void PersistentLeasesCannotBeInsideRetiredDirectoriesOrSharedByEntries()
    {
        OwnedCache owner = AddOwner("native");
        BuildCacheArtifactDescriptor first = owner.Add("first", 2);
        BuildCacheArtifactDescriptor second = owner.Add("second", 2);
        owner.artifacts[0] = new(first.directory, Path.Combine(first.directory, "inside.lock"),
            first.targetId, first.bytes, first.lastUseUtc, [], BuildCacheArtifactRole.ImmutableArtifact);
        Assert.Throws<InvalidDataException>(() => CreatePlan());
        owner.artifacts[0] = first;
        owner.artifacts[1] = new(second.directory, first.leasePath, second.targetId, second.bytes, second.lastUseUtc, [], BuildCacheArtifactRole.ImmutableArtifact);
        Assert.Throws<InvalidDataException>(() => CreatePlan());
        Assert.True(Directory.Exists(first.directory));
        Assert.True(Directory.Exists(second.directory));
    }

    [Fact]
    public async Task ActiveParentOutsideTheProposalProtectsItsDependency()
    {
        OwnedCache owner = AddOwner("native");
        BuildCacheArtifactDescriptor dependency = owner.Add("old-dependency", 6);
        BuildCacheArtifactDescriptor parent = owner.Add("new-parent", 8, [dependency.directory]);
        BuildArtifactManifest.Write(parent.directory, "parent", ["Outputs"]);
        BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(m_root, [owner.descriptor], new(13, 0));
        Assert.Equal(dependency.directory, Assert.Single(plan.candidates).directory);
        using BuildArtifactReadLease reading = await BuildArtifactReadLease.AcquireAsync(parent.directory, Path.GetDirectoryName(parent.directory)!, "parent", ["Outputs"]);

        BuildCacheCleanupResult result = await plan.ExecuteAsync();

        Assert.Empty(result.retired);
        Assert.True(Directory.Exists(parent.directory));
        Assert.True(Directory.Exists(dependency.directory));
    }

    [Fact]
    public async Task PreviewAndExecutionUseTheSameProposalAndFreshRoots()
    {
        OwnedCache owner = AddOwner("native");
        BuildCacheArtifactDescriptor artifact = owner.Add("generation", 3);
        BuildCacheCleanupPlan plan = CreatePlan();
        BuildCacheCleanupResult preview = await plan.ExecuteAsync(dryRun: true);
        Assert.Empty(preview.retired);
        Assert.Equal(0, preview.retiredBytes);
        Assert.Contains(artifact.directory, preview.skipped);
        Assert.True(Directory.Exists(artifact.directory));
        using (FileLease update = await FileLease.AcquireAsync(owner.descriptor.coordinationPath, TimeSpan.Zero))
            owner.roots.Add(artifact.directory);

        BuildCacheCleanupResult actual = await plan.ExecuteAsync();

        Assert.Empty(actual.retired);
        Assert.Contains(artifact.directory, actual.skipped);
        Assert.True(Directory.Exists(artifact.directory));
    }

    [Fact]
    public async Task BusyOwnerDefersTheWholeCrossOwnerOperation()
    {
        OwnedCache first = AddOwner("first");
        BuildCacheArtifactDescriptor dependency = first.Add("dependency", 2);
        OwnedCache second = AddOwner("second");
        BuildCacheArtifactDescriptor parent = second.Add("parent", 2, [dependency.directory]);
        BuildCacheCleanupPlan plan = CreatePlan();
        using FileLease publication = await FileLease.AcquireAsync(second.descriptor.coordinationPath, TimeSpan.Zero);

        BuildCacheCleanupResult result = await plan.ExecuteAsync();

        Assert.Empty(result.retired);
        Assert.Equal(2, result.skipped.Count);
        Assert.True(Directory.Exists(parent.directory));
        Assert.True(Directory.Exists(dependency.directory));
    }

    [Fact]
    public async Task ChangedInventoryDoesNotDeleteTheEarlierGeneration()
    {
        OwnedCache owner = AddOwner("native");
        BuildCacheArtifactDescriptor artifact = owner.Add("generation", 3);
        BuildCacheCleanupPlan plan = CreatePlan();
        owner.artifacts[0] = new(artifact.directory, artifact.leasePath, artifact.targetId, 4, artifact.lastUseUtc, [], BuildCacheArtifactRole.ImmutableArtifact);

        BuildCacheCleanupResult result = await plan.ExecuteAsync();

        Assert.Empty(result.retired);
        Assert.Contains(artifact.directory, result.skipped);
        Assert.True(Directory.Exists(artifact.directory));
    }

    [Fact]
    public async Task AlreadyMissingEntriesAreNotReportedAsRecoveredSpace()
    {
        OwnedCache owner = AddOwner("native");
        BuildCacheArtifactDescriptor artifact = owner.Add("generation", 3);
        BuildCacheCleanupPlan plan = CreatePlan();
        Directory.Delete(artifact.directory, recursive: true);
        BuildCacheCleanupResult result = await plan.ExecuteAsync();
        Assert.Empty(result.retired);
        Assert.Equal(0, result.retiredBytes);
        Assert.Contains(artifact.directory, result.skipped);
    }

    [Fact]
    public async Task CancellationAndDurationLimitPreserveUnprocessedArtifacts()
    {
        OwnedCache owner = AddOwner("native");
        BuildCacheArtifactDescriptor artifact = owner.Add("generation", 3);
        Assert.Throws<TimeoutException>(() => BuildCacheCleanupPlan.Create(m_root, [owner.descriptor],
            new(0, 0, TimeSpan.FromTicks(1)), retireAll: true));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => CreatePlan().ExecuteAsync(cancellationToken: cancellation.Token).AsTask());
        Assert.True(Directory.Exists(artifact.directory));
    }

    [Fact]
    public void InvalidOwnershipAndDependencyCyclesFailBeforeRemoval()
    {
        OwnedCache owner = AddOwner("native");
        BuildCacheArtifactDescriptor first = owner.Add("first", 2);
        BuildCacheArtifactDescriptor second = owner.Add("second", 2, [first.directory]);
        owner.artifacts[0] = new(first.directory, first.leasePath, first.targetId, first.bytes, first.lastUseUtc, [second.directory], BuildCacheArtifactRole.ImmutableArtifact);
        Assert.Throws<ArgumentException>(() => CreatePlan());
        owner.artifacts[0] = new(first.directory, first.leasePath, first.targetId, first.bytes, first.lastUseUtc, [Path.Combine(m_root, "unregistered")], BuildCacheArtifactRole.ImmutableArtifact);
        Assert.Throws<InvalidDataException>(() => CreatePlan());
        owner.artifacts[0] = new(m_root, first.leasePath, null, 2, DateTime.UtcNow, [], BuildCacheArtifactRole.ImmutableArtifact);
        Assert.Throws<IOException>(() => CreatePlan());
        Assert.True(Directory.Exists(first.directory));
        Assert.True(Directory.Exists(second.directory));
    }

    [Fact]
    public async Task UnregisteredProtectionRootsRejectPlanningAndFreshExecution()
    {
        OwnedCache owner = AddOwner("native");
        BuildCacheArtifactDescriptor artifact = owner.Add("generation", 3);
        string omitted = Path.Combine(owner.descriptor.directory, "omitted-current");
        Directory.CreateDirectory(omitted);
        BuildCacheCleanupPlan prior = CreatePlan();
        owner.roots.Add(omitted);
        Assert.Throws<InvalidDataException>(() => CreatePlan());
        await Assert.ThrowsAsync<InvalidDataException>(() => prior.ExecuteAsync().AsTask());
        Assert.True(Directory.Exists(artifact.directory));
        Assert.True(Directory.Exists(omitted));
    }

    [Fact]
    public void ProtectedArtifactsCanExceedTheBudgetWithoutBecomingCandidates()
    {
        OwnedCache owner = AddOwner("native");
        BuildCacheArtifactDescriptor artifact = owner.Add("generation", 3);
        owner.roots.Add(artifact.directory);
        BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(m_root, [owner.descriptor], new(0, 0));
        Assert.Equal(3, plan.totalBytes);
        Assert.Equal(3, plan.protectedBytes);
        Assert.Empty(plan.candidates);
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private OwnedCache AddOwner(string name)
    {
        var owner = new OwnedCache(Path.Combine(m_root, name), name);
        m_owners.Add(owner);
        return owner;
    }

    private BuildCacheCleanupPlan CreatePlan() => BuildCacheCleanupPlan.Create(m_root,
        m_owners.Select(static owner => owner.descriptor).ToArray(), new(0, 0), retireAll: true);

    private sealed class OwnedCache
    {
        internal OwnedCache(
            string directory,
            string id
        ) {
            Directory.CreateDirectory(directory);
            descriptor = new(id, directory, Path.Combine(directory, "owner.lock"),
                token =>
                {
                    token.ThrowIfCancellationRequested();
                    return artifacts.ToArray();
                },
                token =>
                {
                    token.ThrowIfCancellationRequested();
                    return roots.ToArray();
                });
        }

        internal List<BuildCacheArtifactDescriptor> artifacts { get; } = [];
        internal List<string> roots { get; } = [];
        internal BuildCacheOwnerDescriptor descriptor { get; }

        internal BuildCacheArtifactDescriptor Add(
            string name,
            int bytes,
            IReadOnlyList<string>? dependencies = null,
            string? targetId = null
        ) {
            string directory = Path.Combine(descriptor.directory, name);
            Directory.CreateDirectory(Path.Combine(directory, "Outputs"));
            File.WriteAllBytes(Path.Combine(directory, "Outputs", "payload.bin"), new byte[bytes]);
            var used = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(artifacts.Count);
            var artifact = new BuildCacheArtifactDescriptor(directory, directory + ".read.lock", targetId, bytes, used, dependencies ?? [], BuildCacheArtifactRole.ImmutableArtifact);
            artifacts.Add(artifact);
            return artifact;
        }
    }
}
