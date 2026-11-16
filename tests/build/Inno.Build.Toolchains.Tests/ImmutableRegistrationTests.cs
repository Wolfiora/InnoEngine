using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class ImmutableRegistrationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoImmutableRegistration", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task ConfirmationRetainsPublicationMetadataAndCurrentDependencyProtection()
    {
        var publisher = new BuildCachePublisher(m_root, BuildCacheArtifactRole.ImmutableArtifact);
        string page = Create("page");
        string generation = Create("generation");
        using (await publisher.AcquireUseAsync())
        using (await FileLease.AcquireAsync(page + ".lock", TimeSpan.FromSeconds(5)))
        using (await FileLease.AcquireAsync(generation + ".lock", TimeSpan.FromSeconds(5)))
        {
            publisher.EnsureImmutableRegistration(page, page + ".lock", [], null);
            string record = Assert.Single(Directory.GetFiles(m_root, "*.cache.json"));
            byte[] bytes = File.ReadAllBytes(record);
            DateTime time = File.GetLastWriteTimeUtc(record);
            publisher.EnsureImmutableRegistration(page, page + ".lock", [], null);
            Assert.Equal(bytes, File.ReadAllBytes(record));
            Assert.Equal(time, File.GetLastWriteTimeUtc(record));
            publisher.Record(generation, generation + ".lock", [page], "selected", null);
        }
        BuildCacheCleanupPlan protectedPlan = BuildCacheCleanupPlan.Create(Path.GetDirectoryName(m_root)!,
            [publisher.Describe("documentation")], new(0, 0), retireAll: true);
        Assert.Empty((await protectedPlan.ExecuteAsync()).retired);
        Assert.True(Directory.Exists(page));
    }

    [Theory]
    [InlineData("lease")]
    [InlineData("dependency")]
    [InlineData("target")]
    [InlineData("payload")]
    public async Task ConfirmationRejectsChangedOwnershipWithoutRepairingItsRecord(string change)
    {
        var publisher = new BuildCachePublisher(m_root, BuildCacheArtifactRole.ImmutableArtifact);
        string page = Create("page");
        using (await publisher.AcquireUseAsync())
        using (await FileLease.AcquireAsync(page + ".lock", TimeSpan.FromSeconds(5)))
        {
            publisher.EnsureImmutableRegistration(page, page + ".lock", [], null);
            string record = Assert.Single(Directory.GetFiles(m_root, "*.cache.json"));
            byte[] bytes = File.ReadAllBytes(record);
            if (change == "payload")
                File.WriteAllText(Path.Combine(page, "extra"), "unaccounted bytes");
            Assert.Throws<InvalidDataException>(() => publisher.EnsureImmutableRegistration(page,
                change == "lease" ? page + ".other.lock" : page + ".lock",
                change == "dependency" ? [Path.Combine(m_root, "dependency")] : [],
                change == "target" ? "another-target" : null));
            Assert.Equal(bytes, File.ReadAllBytes(record));
        }
    }

    [Fact]
    public async Task CanceledRegistrationDoesNotCreateOwnershipAndWorkspacesCannotOptIn()
    {
        var publisher = new BuildCachePublisher(m_root, BuildCacheArtifactRole.ImmutableArtifact);
        string page = Create("page");
        using (await publisher.AcquireUseAsync())
        using (await FileLease.AcquireAsync(page + ".lock", TimeSpan.FromSeconds(5)))
        {
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();
            Assert.Throws<OperationCanceledException>(() => publisher.EnsureImmutableRegistration(
                page, page + ".lock", [], null, cancellation.Token));
            Assert.Empty(Directory.GetFiles(m_root, "*.cache.json"));
            var workspace = new BuildCachePublisher(m_root, BuildCacheArtifactRole.ReusableWorkspace);
            Assert.Throws<InvalidOperationException>(() => workspace.EnsureImmutableRegistration(page, page + ".lock", [], null));
            Assert.Throws<IOException>(() => publisher.EnsureImmutableRegistration(
                Path.Combine(m_root, "..", "foreign"), page + ".lock", [], null));
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private string Create(string name)
    {
        string path = Path.Combine(m_root, name);
        Directory.CreateDirectory(path);
        File.WriteAllText(Path.Combine(path, "payload"), name);
        return path;
    }
}
