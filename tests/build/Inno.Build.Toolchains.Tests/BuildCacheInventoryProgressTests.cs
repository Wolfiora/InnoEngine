using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Core.IO;

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
        Dictionary<string, string> before = checkpoints.ToDictionary(static path => path, File.ReadAllText);
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
        Assert.Empty(Directory.GetFiles(Path.Combine(m_root, "artifacts", "cache-owners", "inventory"), "*.inventory"));
    }

    [Fact]
    public async Task ActiveReadersKeepTheirInventoryUntilRetirementActuallyCompletes()
    {
        await PrepareAsync("first", "first payload");
        await MaintainAsync();
        string directory = Path.Combine(m_root, "cache", "first");
        string checkpoint = Assert.Single(
            Directory.GetFiles(Path.Combine(m_root, "artifacts", "cache-owners", "inventory"), "*.inventory")
        );
        using (FileLease reader = await FileLease.AcquireSharedAsync(directory + ".lock", TimeSpan.FromSeconds(5)))
        {
            await MaintainAsync(retireAll: true);
            Assert.True(Directory.Exists(directory));
            Assert.True(File.Exists(checkpoint));
        }
        await MaintainAsync(retireAll: true);
        Assert.False(Directory.Exists(directory));
        Assert.False(File.Exists(checkpoint));
    }

    [Fact]
    public async Task InterruptedInventoryResumesCompletedShardsBeforeRetiringAnyArtifact()
    {
        const int C_COUNT = 64;
        for (int index = 0; index < C_COUNT; index++)
        {
            await PrepareAsync("shard-" + index.ToString("D3"), new string('x', 4096));
        }
        string directory = Path.Combine(m_root, "artifacts", "cache-owners", "inventory");
        Directory.CreateDirectory(directory);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        using var stopObserving = new CancellationTokenSource();
        using var observing = new ManualResetEventSlim();
        // Inventory is synchronous work. Observe its public checkpoint on a dedicated thread
        // so a busy test worker pool cannot postpone cancellation until after retirement.
        Task observer = Task.Factory.StartNew(() =>
        {
            observing.Set();
            while (!stopObserving.IsCancellationRequested)
            {
                if (Directory.EnumerateFiles(directory, "*.inventory").Any())
                {
                    cancellation.Cancel();
                    return;
                }
                Thread.Sleep(1);
            }
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        try
        {
            Assert.True(observing.Wait(TimeSpan.FromSeconds(30)), "The checkpoint observer did not start.");
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Task.Run(() =>
                BuildMaintenanceApplication.CleanAsync(
                    m_root,
                    new BuildCachePolicy(long.MaxValue, 0, TimeSpan.FromMinutes(2)),
                    true,
                    false,
                    null,
                    cancellation.Token
                )));
        }
        finally
        {
            stopObserving.Cancel();
            await observer;
        }
        string[] completed = Directory.GetFiles(directory, "*.inventory");
        Assert.NotEmpty(completed);
        Assert.True(
            completed.Length < C_COUNT,
            "The first checkpoint must interrupt discovery before the complete protection graph exists."
        );
        Dictionary<string, (string, DateTime)> snapshots = completed.ToDictionary(
            static path => path,
            static path => (File.ReadAllText(path), File.GetLastWriteTimeUtc(path))
        );
        Assert.Equal(C_COUNT, Directory.GetDirectories(Path.Combine(m_root, "cache")).Length);
        await MaintainAsync();
        Assert.Equal(C_COUNT, Directory.GetFiles(directory, "*.inventory").Length);
        foreach (KeyValuePair<string, (string, DateTime)> snapshot in snapshots)
        {
            Assert.Equal(snapshot.Value.Item1, File.ReadAllText(snapshot.Key));
            Assert.Equal(snapshot.Value.Item2, File.GetLastWriteTimeUtc(snapshot.Key));
        }
        await MaintainAsync(retireAll: true);
        Assert.Empty(Directory.GetDirectories(Path.Combine(m_root, "cache")));
        Assert.Empty(Directory.GetFiles(directory, "*.inventory"));
    }

    public void Dispose()
    {
        string parent = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "InnoInventoryProgress")) + Path.DirectorySeparatorChar;
        if (!Path.GetFullPath(m_root).StartsWith(parent, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The inventory fixture escaped its owner.");
        }
        if (Directory.Exists(m_root))
        {
            Directory.Delete(m_root, recursive: true);
        }
    }

    private async Task PrepareAsync(
        string name,
        string content
    ) {
        string root = Path.Combine(m_root, "cache");
        var publisher = new BuildCachePublisher(root, BuildCacheArtifactRole.ReusableWorkspace);
        BuildCacheOwnerCatalog.Register(m_root, root, "fixture", publisher.role);
        using FileLease activity = await publisher.AcquireUseAsync();
        string directory = Path.Combine(root, name);
        using FileLease writer = await FileLease.AcquireAsync(directory + ".lock", TimeSpan.FromSeconds(5));
        publisher.BeginWorkingDirectory(directory, directory + ".lock", null);
        File.WriteAllText(Path.Combine(directory, "payload"), content);
    }

    private Task MaintainAsync(
        bool dryRun = false,
        bool retireAll = false
    ) =>
        // Full inventory and retirement assertions are foreground work, not a five-second automatic maintenance slice.
        BuildMaintenanceApplication.CleanAsync(
            m_root,
            new BuildCachePolicy(long.MaxValue, 0, TimeSpan.FromMinutes(2)),
            retireAll,
            dryRun,
            null,
            CancellationToken.None
        );
}
