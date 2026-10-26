using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Tests;

public sealed class BuildCacheMetadataRetirementTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoCacheMetadata", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task OwnerCatalogWaitParticipatesInTheCompleteMaintenanceBudget()
    {
        BuildCacheOwnerCatalog.Register(m_root, Path.Combine(m_root, "cache"), "fixture", BuildCacheArtifactRole.ImmutableArtifact);
        string coordination = Path.Combine(m_root, "artifacts", "cache-owners", "owner.lock");
        using FileLease writer = await FileLease.AcquireAsync(coordination, TimeSpan.FromSeconds(5));
        Stopwatch elapsed = Stopwatch.StartNew();
        await Assert.ThrowsAsync<TimeoutException>(() => BuildMaintenanceApplication.CleanAsync(m_root,
            new(timeBudget: TimeSpan.FromMilliseconds(100)), false, false, null, default));
        Assert.True(elapsed.Elapsed < TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task RetiredPublicationsReleaseTheirMetadataAndLeafWhilePreservingOwnerAndUnknownFiles()
    {
        string cache = Path.Combine(m_root, "cache");
        var publisher = new BuildCachePublisher(cache, BuildCacheArtifactRole.ImmutableArtifact);
        string output = Path.Combine(cache, "generation");
        Directory.CreateDirectory(Path.Combine(output, "Outputs"));
        File.WriteAllText(Path.Combine(output, "Outputs", "file"), "payload");
        using (await publisher.AcquireUseAsync())
            publisher.Record(output, output + ".read.lock", [], null, null);
        File.WriteAllText(Path.Combine(cache, "unknown.lock"), "user-owned");
        BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(m_root, [publisher.Describe("fixture")], new(0, 0), retireAll: true);
        Assert.Empty((await plan.ExecuteAsync()).failures);
        Assert.False(Directory.Exists(output));
        Assert.False(File.Exists(output + ".read.lock"));
        Assert.Empty(Directory.GetFiles(cache, "*.cache.json"));
        Assert.True(File.Exists(Path.Combine(cache, "owner.lock")));
        Assert.True(File.Exists(Path.Combine(cache, "unknown.lock")));
    }

    [Fact]
    public async Task MetadataConvergesWithoutBytePressureAndDryRunDoesNotRetireClaims()
    {
        string cache = Path.Combine(m_root, "cache");
        var publisher = new BuildCachePublisher(cache, BuildCacheArtifactRole.ProcessWorkingDirectory);
        using (await publisher.AcquireUseAsync())
            using (await ProcessOwnedDirectory.CreateAsync(cache)) { }
        Assert.Single(Directory.GetFiles(cache, "*.work.json"));
        string[] before = Directory.GetFiles(cache).Order(StringComparer.Ordinal).ToArray();
        await Plan().ExecuteAsync(dryRun: true);
        Assert.Equal(before, Directory.GetFiles(cache).Order(StringComparer.Ordinal));
        Assert.Empty((await Plan().ExecuteAsync()).failures);
        Assert.Empty(Directory.GetFiles(cache, "*.work.json"));
        Assert.Empty(Directory.GetFiles(cache, "*.work.lock"));

        BuildCacheCleanupPlan Plan() => BuildCacheCleanupPlan.Create(m_root, [publisher.Describe("fixture")], new());
    }

    [Fact]
    public async Task StableCurrentPointersAreNotRewrittenAndCorruptionIsReported()
    {
        string cache = Path.Combine(m_root, "cache");
        var publisher = new BuildCachePublisher(cache, BuildCacheArtifactRole.ImmutableArtifact);
        string output = Path.Combine(cache, "generation");
        Directory.CreateDirectory(output);
        File.WriteAllText(Path.Combine(output, "file"), "payload");
        using (await publisher.AcquireUseAsync())
        {
            publisher.Record(output, output + ".read.lock", [], "current", null);
            string pointer = Assert.Single(Directory.GetFiles(cache, "*.current"));
            DateTime stamp = DateTime.UtcNow.AddDays(-1);
            File.SetLastWriteTimeUtc(pointer, stamp);
            stamp = File.GetLastWriteTimeUtc(pointer);
            publisher.Record(output, output + ".read.lock", [], "current", null);
            Assert.Equal(stamp, File.GetLastWriteTimeUtc(pointer));
            File.WriteAllText(pointer, "../escaped");
        }
        Assert.ThrowsAny<IOException>(() => BuildCacheCleanupPlan.Create(m_root, [publisher.Describe("fixture")], new()));
        Assert.True(Directory.Exists(output));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
