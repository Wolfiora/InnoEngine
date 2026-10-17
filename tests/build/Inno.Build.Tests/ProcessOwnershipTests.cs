using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class ProcessOwnershipTests
{
    [Theory]
    [InlineData("{\"directory\":\"valid\",\"processId\":\"wrong\",\"startedUtcTicks\":1}")]
    [InlineData("{\"directory\":\"valid\",\"processId\":1,\"startedUtcTicks\":null}")]
    public async Task MalformedSubprocessClaimsPreventRetirement(string malformed)
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoMalformedClaim", Guid.NewGuid().ToString("N"));
        try
        {
            using ProcessOwnedDirectory working = await ProcessOwnedDirectory.CreateAsync(root);
            string record = Assert.Single(Directory.EnumerateFiles(root, "*.work.json"));
            File.WriteAllText(record + ".fixture.process", malformed);
            Assert.NotEqual(ProcessOwnerState.Exited, Assert.Single(ProcessOwnedDirectory.ReadClaims(root)).ownerState);
            Assert.Throws<IOException>(working.Dispose);
            Assert.True(Directory.Exists(working.directory));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task RegistrationAndRetirementCoordinateThroughTheSamePersistentLease()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoClaimHandoff", Guid.NewGuid().ToString("N"));
        try
        {
            using ProcessOwnedDirectory working = await ProcessOwnedDirectory.CreateAsync(root);
            string path = working.directory;
            string leasePath = Assert.Single(ProcessOwnedDirectory.ReadClaims(root)).leasePath;
            using FileLease registration = await FileLease.AcquireSharedAsync(leasePath, TimeSpan.FromSeconds(5));
            Task retiring = Task.Run(working.Dispose);
            await Task.Delay(100);
            Assert.False(retiring.IsCompleted);
            Assert.True(Directory.Exists(path));
            ProcessOwnedDirectory.RegisterCurrentProcess(root, path);
            registration.Dispose();
            await retiring;
            Assert.False(Directory.Exists(path));
            Assert.Empty(ProcessOwnedDirectory.ReadClaims(root));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task ProcessParticipantsReuseOnlyTheSameActiveDeclaredOperation()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoProcessClaim", Guid.NewGuid().ToString("N"));
        try
        {
            string identity = Guid.NewGuid().ToString("N");
            string first = await ProcessOwnedDirectory.CreateForProcessAsync(root, identity);
            File.WriteAllText(Path.Combine(first, "payload"), "retain");
            Assert.Equal(first, await ProcessOwnedDirectory.CreateForProcessAsync(root, identity));
            Assert.Equal("retain", File.ReadAllText(Path.Combine(first, "payload")));
            Assert.Single(ProcessOwnedDirectory.ReadClaims(root));
            string unknown = Guid.NewGuid().ToString("N");
            Directory.CreateDirectory(Path.Combine(root, unknown));
            await Assert.ThrowsAsync<InvalidDataException>(() => ProcessOwnedDirectory.CreateForProcessAsync(root, unknown).AsTask());
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }
    [Fact]
    public void ProcessIdentityIncludesItsStartTimeAndInvalidIdentityIsConservative()
    {
        ProcessOwnerStamp current = ProcessOwnerStamp.CaptureCurrent();
        Assert.Equal(ProcessOwnerState.Active, current.Query());
        Assert.Equal(ProcessOwnerState.Exited, new ProcessOwnerStamp(current.processId, current.startedUtcTicks - 1).Query());
        Assert.Equal(ProcessOwnerState.Unknown, default(ProcessOwnerStamp).Query());
    }

    [Fact]
    public async Task WorkingClaimPrecedesPayloadAndNeverAdoptsUnrecordedDirectories()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoClaimTests", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(root);
            string unowned = Path.Combine(root, "unowned");
            Directory.CreateDirectory(unowned);
            using ProcessOwnedDirectory working = await ProcessOwnedDirectory.CreateAsync(root);
            ProcessOwnedDirectoryDescriptor claim = Assert.Single(ProcessOwnedDirectory.ReadClaims(root));
            Assert.Equal(working.directory, claim.directory);
            Assert.Equal(ProcessOwnerState.Active, claim.ownerState);
            Assert.True(File.Exists(claim.leasePath));
            await Assert.ThrowsAsync<TimeoutException>(() => FileLease.AcquireAsync(claim.leasePath, TimeSpan.Zero).AsTask());
            File.WriteAllText(Path.Combine(working.directory, "payload"), "owned");
            working.Dispose();
            Assert.False(Directory.Exists(working.directory));
            Assert.Empty(ProcessOwnedDirectory.ReadClaims(root));
            Assert.True(Directory.Exists(unowned));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task WorkspacePreferenceDoesNotPermanentlyPinAnIdleWorkspace()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoWorkspaceRole", Guid.NewGuid().ToString("N"));
        try
        {
            string cache = Path.Combine(root, "cache");
            string workspace = Path.Combine(cache, "workspace");
            Directory.CreateDirectory(workspace);
            File.WriteAllText(Path.Combine(workspace, "payload"), "owned");
            var publisher = new BuildCachePublisher(cache, BuildCacheArtifactRole.ReusableWorkspace);
            using (await publisher.AcquireUseAsync())
                publisher.Record(workspace, workspace + ".lock", [], "preferred", "fixture");
            BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(root, [publisher.Describe("workspace")],
                new BuildCachePolicy(0, 0, TimeSpan.FromSeconds(10)), retireAll: true);
            Assert.Single(plan.candidates);
            BuildCacheCleanupResult result = await plan.ExecuteAsync();
            Assert.Single(result.retired);
            Assert.False(Directory.Exists(workspace));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task WorkingRegistrationRefusesToAdoptUnknownExistingPayload()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoUnownedWorkspace", Guid.NewGuid().ToString("N"));
        try
        {
            string cache = Path.Combine(root, "cache");
            string workspace = Path.Combine(cache, "unregistered");
            Directory.CreateDirectory(workspace);
            string payload = Path.Combine(workspace, "payload");
            File.WriteAllText(payload, "preserve");
            var publisher = new BuildCachePublisher(cache, BuildCacheArtifactRole.ReusableWorkspace);
            using FileLease activity = await publisher.AcquireUseAsync();
            using FileLease writing = await FileLease.AcquireAsync(workspace + ".lock", TimeSpan.FromSeconds(10));
            Assert.Throws<InvalidOperationException>(() => publisher.BeginWorkingDirectory(workspace, workspace + ".lock", null));
            Assert.Equal("preserve", File.ReadAllText(payload));
            Assert.Empty(Directory.EnumerateFiles(cache, "*.cache.json"));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }
}
