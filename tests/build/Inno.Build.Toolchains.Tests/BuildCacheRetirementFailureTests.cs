using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class BuildCacheRetirementFailureTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoCacheRetirementFailures", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task UnverifiableMetadataPreservesClaimsAndReportsCompletedRetirement()
    {
        string invalidRoot = Path.Combine(m_root, "a-invalid");
        var invalid = new BuildCachePublisher(invalidRoot, BuildCacheArtifactRole.ProcessWorkingDirectory);
        using (await invalid.AcquireUseAsync())
            using (await ProcessOwnedDirectory.CreateAsync(invalidRoot)) { }
        string claim = Assert.Single(Directory.GetFiles(invalidRoot, "*.work.json"));
        MarkOwnerExited(claim);
        string marker = claim + ".invalid.process";
        File.WriteAllText(marker, "unverifiable process metadata");
        byte[] original = File.ReadAllBytes(claim);

        string healthyRoot = Path.Combine(m_root, "z-healthy");
        var healthy = new BuildCachePublisher(healthyRoot, BuildCacheArtifactRole.ImmutableArtifact);
        string output = Path.Combine(healthyRoot, "generation");
        Directory.CreateDirectory(output);
        File.WriteAllBytes(Path.Combine(output, "payload"), [1, 2, 3]);
        using (await healthy.AcquireUseAsync())
            healthy.Record(output, output + ".read.lock", [], null, null);

        BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(m_root,
            [invalid.Describe("invalid"), healthy.Describe("healthy")], new(0, 0), retireAll: true);
        BuildCacheCleanupResult result = await plan.ExecuteAsync();

        Assert.Equal(3, result.retiredBytes);
        Assert.False(Directory.Exists(output));
        Assert.Empty(Directory.GetFiles(healthyRoot, "*.cache.json"));
        Assert.Single(result.failures);
        Assert.Contains("unverifiable process owner", result.failures[invalidRoot]);
        Assert.Equal(original, File.ReadAllBytes(claim));
        Assert.Equal("unverifiable process metadata", File.ReadAllText(marker));
        Assert.Empty(result.skipped);
    }

    [Fact]
    public async Task ExplicitMaintenanceStillFailsWhenMetadataCannotBeCertified()
    {
        string cache = Path.Combine(m_root, "cache");
        var publisher = new BuildCachePublisher(cache, BuildCacheArtifactRole.ProcessWorkingDirectory);
        BuildCacheOwnerCatalog.Register(m_root, cache, "fixture", BuildCacheArtifactRole.ProcessWorkingDirectory);
        using (await publisher.AcquireUseAsync())
            using (await ProcessOwnedDirectory.CreateAsync(cache)) { }
        string claim = Directory.GetFiles(cache, "*.work.json").Single();
        MarkOwnerExited(claim);
        File.WriteAllText(claim + ".invalid.process", "unverifiable");

        await Assert.ThrowsAsync<InvalidOperationException>(() => BuildMaintenanceApplication.CleanAsync(
            m_root, new(timeBudget: TimeSpan.FromSeconds(30)), true, false, null, default));
        Assert.True(File.Exists(claim));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private static void MarkOwnerExited(string claim)
    {
        // The fixture models a former lifetime of this PID, not the still-running test host.
        JsonNode metadata = JsonNode.Parse(File.ReadAllText(claim))!;
        metadata["startedUtcTicks"] = 1;
        File.WriteAllText(claim, metadata.ToJsonString());
    }
}
