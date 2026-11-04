using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class BuildCacheInventoryProgressTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoInventoryProgress", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task DryRunDoesNotCreateOrRewriteInventoryProgress()
    {
        await PrepareAsync("first", "first payload");
        await MaintainAsync(dryRun: true);
        string directory = Path.Combine(m_root, "artifacts", "cache-owners", "inventory");
        Assert.False(Directory.Exists(directory));
        await MaintainAsync();
        string checkpoint = Assert.Single(Directory.GetFiles(directory, "*.inventory"));
        DateTime time = File.GetLastWriteTimeUtc(checkpoint);
        string contents = File.ReadAllText(checkpoint);
        await MaintainAsync(dryRun: true);
        Assert.Equal(time, File.GetLastWriteTimeUtc(checkpoint));
        Assert.Equal(contents, File.ReadAllText(checkpoint));
        Assert.True(File.Exists(Path.Combine(m_root, "cache", "first", "payload")));
    }

    [Fact]
    public async Task ANewWriterInvalidatesItsMeasurementWithoutDiscardingOtherShards()
    {
        await PrepareAsync("first", "first payload");
        await PrepareAsync("second", "second payload");
        await MaintainAsync();
        string[] checkpoints = Directory.GetFiles(Path.Combine(m_root, "artifacts", "cache-owners", "inventory"), "*.inventory");
        var before = checkpoints.ToDictionary(static path => path, File.ReadAllText);
        await PrepareAsync("first", "a longer changed payload");
        await MaintainAsync();
        Assert.Single(checkpoints.Where(path => before[path] != File.ReadAllText(path)));
        Assert.All(checkpoints, path => Assert.True(File.Exists(path)));
    }

    [Fact]
    public async Task AnIncompleteProtectionGraphCannotRetireAnyCompletedInventoryShard()
    {
        await PrepareAsync("first", "first payload");
        await PrepareAsync("second", "second payload");
        string record = Directory.GetFiles(Path.Combine(m_root, "cache"), "*.cache.json")
            .Single(path => JsonNode.Parse(File.ReadAllText(path))!["directory"]!.GetValue<string>() == "second");
        JsonNode metadata = JsonNode.Parse(File.ReadAllText(record))!;
        metadata["dependencies"] = new JsonArray(Path.Combine(m_root, "unregistered"));
        File.WriteAllText(record, metadata.ToJsonString());
        await Assert.ThrowsAsync<InvalidDataException>(() => MaintainAsync(retireAll: true));
        string[] checkpoints = Directory.GetFiles(Path.Combine(m_root, "artifacts", "cache-owners", "inventory"), "*.inventory");
        Assert.Equal(2, checkpoints.Length);
        Assert.True(Directory.Exists(Path.Combine(m_root, "cache", "first")));
        Assert.True(Directory.Exists(Path.Combine(m_root, "cache", "second")));
        metadata["dependencies"] = new JsonArray();
        File.WriteAllText(record, metadata.ToJsonString());
        await MaintainAsync();
        Assert.Equal(2, Directory.GetFiles(Path.Combine(m_root, "artifacts", "cache-owners", "inventory"), "*.inventory").Length);
        await MaintainAsync(retireAll: true);
        Assert.False(Directory.Exists(Path.Combine(m_root, "cache", "first")));
        Assert.False(Directory.Exists(Path.Combine(m_root, "cache", "second")));
    }

    private async Task PrepareAsync(string name, string content)
    {
        string root = Path.Combine(m_root, "cache");
        var publisher = new BuildCachePublisher(root, BuildCacheArtifactRole.ReusableWorkspace);
        BuildCacheOwnerCatalog.Register(m_root, root, "fixture", publisher.role);
        using FileLease activity = await publisher.AcquireUseAsync();
        string directory = Path.Combine(root, name);
        using FileLease writer = await FileLease.AcquireAsync(directory + ".lock", TimeSpan.FromSeconds(5));
        publisher.BeginWorkingDirectory(directory, directory + ".lock", null);
        File.WriteAllText(Path.Combine(directory, "payload"), content);
    }

    private Task MaintainAsync(bool dryRun = false, bool retireAll = false)
        => BuildMaintenanceApplication.CleanAsync(m_root, new(long.MaxValue, 0), retireAll, dryRun, null, CancellationToken.None);

    public void Dispose()
    {
        string parent = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "InnoInventoryProgress")) + Path.DirectorySeparatorChar;
        if (!Path.GetFullPath(m_root).StartsWith(parent, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("The inventory fixture escaped its owner.");
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
