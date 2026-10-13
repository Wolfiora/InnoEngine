using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BuildCachePublisherTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoCachePublisher", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task StandardOwnersPreserveReadersAndIgnoreUnregisteredDirectories()
    {
        var publisher = new BuildCachePublisher(Path.Combine(m_root, "artifacts", "native"));
        string old = Create("native", "old");
        string current = Create("native", "current");
        string unrelated = Create("native", "unregistered");
        using (await publisher.AcquireUseAsync())
        {
            publisher.Record(old, old + ".read.lock", [], "editor", null);
            publisher.Record(current, current + ".read.lock", [], "editor", null);
        }
        using (BuildArtifactReadLease reading = await BuildArtifactReadLease.AcquireAsync(old, "old", ["Outputs"]))
        {
            BuildCacheCleanupResult busy = await Plan().ExecuteAsync();
            Assert.Empty(busy.retired);
            Assert.Contains(old, busy.skipped);
        }
        BuildCacheCleanupResult complete = await Plan().ExecuteAsync();
        Assert.Equal(old, Assert.Single(complete.retired));
        Assert.True(Directory.Exists(current));
        Assert.True(Directory.Exists(unrelated));
        Assert.Empty((await Plan().ExecuteAsync()).retired);
    }

    [Fact]
    public async Task NativeCurrentRetainsItsExactOlderBindingGeneration()
    {
        var bindings = new BuildCachePublisher(Path.Combine(m_root, "artifacts", "bindings"));
        var native = new BuildCachePublisher(Path.Combine(m_root, "artifacts", "native"));
        string oldBinding = Create("bindings", "old");
        string newBinding = Create("bindings", "current");
        string oldNative = Create("native", "old");
        string newNative = Create("native", "current");
        using (await bindings.AcquireUseAsync())
        {
            bindings.Record(oldBinding, oldBinding + ".read.lock", [], "component", null);
            bindings.Record(newBinding, newBinding + ".read.lock", [], "component", null);
        }
        using (await native.AcquireUseAsync())
            native.Record(oldNative, oldNative + ".read.lock", [oldBinding], "component", null);
        Assert.Empty(Plan().candidates);
        using (await native.AcquireUseAsync())
            native.Record(newNative, newNative + ".read.lock", [newBinding], "component", null);
        BuildCacheCleanupResult result = await Plan().ExecuteAsync();
        Assert.Equal(2, result.retired.Count);
        Assert.Contains(oldNative, result.retired);
        Assert.Contains(oldBinding, result.retired);
        Assert.True(Directory.Exists(newNative));
        Assert.True(Directory.Exists(newBinding));
    }

    [Fact]
    public async Task ActivePublisherPreventsRetirementDuringReadingHandoff()
    {
        var publisher = new BuildCachePublisher(Path.Combine(m_root, "artifacts", "native"));
        string artifact = Create("native", "candidate");
        using var activity = await publisher.AcquireUseAsync();
        publisher.Record(artifact, artifact + ".read.lock", [], null, null);
        BuildCacheCleanupResult result = await Plan().ExecuteAsync();
        Assert.Empty(result.retired);
        Assert.True(Directory.Exists(artifact));
    }

    [Fact]
    public async Task ProcessOwnedLoadsProtectTheirExactHostDependency()
    {
        string host = Path.Combine(m_root, "artifacts", "build-tools", "tasks", "hosts", "host");
        string load = Path.Combine(m_root, "artifacts", "build-tools", "tasks", "loads", "load");
        Directory.CreateDirectory(host);
        Directory.CreateDirectory(load);
        File.WriteAllText(Path.Combine(host, "runtime.dll"), "host bytes");
        File.WriteAllText(Path.Combine(load, "runtime.dll"), "private bytes");
        var hosts = new BuildCachePublisher(Path.GetDirectoryName(host)!);
        using (await hosts.AcquireUseAsync())
            hosts.Record(host, host + ".lock", [], null, null);
        new BuildCachePublisher(Path.GetDirectoryName(load)!).RecordProcessUse(load, load + ".lock", [host]);
        Assert.Empty(Plan().candidates);
        Assert.Empty((await Plan().ExecuteAsync()).retired);
        string marker = Directory.GetFiles(Path.Combine(load, "owners"), "*.pid").Single();
        File.WriteAllText(marker, "-1");
        Assert.Empty(Plan().candidates);
    }

    [Fact]
    public async Task SizeAccountingIsRecordedByTheProducerWithoutServingAsIntegrityValidation()
    {
        var publisher = new BuildCachePublisher(Path.Combine(m_root, "artifacts", "native"));
        string artifact = Create("native", "owned");
        using (await publisher.AcquireUseAsync())
            publisher.Record(artifact, artifact + ".read.lock", [], null, null);
        long recorded = Plan().totalBytes;
        File.WriteAllText(Path.Combine(artifact, "Outputs", "payload"), "corrupted and larger");
        Assert.Equal(recorded, Plan().totalBytes);
        Assert.False(BuildArtifactManifest.IsComplete(artifact, "owned", ["Outputs"]));
        using (await publisher.AcquireUseAsync())
            publisher.Record(artifact, artifact + ".read.lock", [], null, null);
        Assert.True(Plan().totalBytes > recorded);
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private BuildCacheCleanupPlan Plan()
        => BuildCacheCleanupPlan.Create(m_root, StandardBuildCaches.CreateOwners(m_root, []), new(0, 0), retireAll: true);

    private string Create(
        string owner,
        string name
    ) {
        string directory = Path.Combine(m_root, "artifacts", owner, name);
        Directory.CreateDirectory(Path.Combine(directory, "Outputs"));
        File.WriteAllText(Path.Combine(directory, "Outputs", "payload"), name);
        BuildArtifactManifest.Write(directory, name, ["Outputs"]);
        return directory;
    }
}
