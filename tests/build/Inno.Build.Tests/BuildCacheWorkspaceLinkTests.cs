using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BuildCacheWorkspaceLinkTests
{
    [UnixFileLinksFact]
    public async Task WorkspaceLibraryAliasesAreCountedOnceAndRetiredWithoutFollowingLinks()
    {
        using ProcessOwnedDirectory owner = await ProcessOwnedDirectory.CreateAsync(Path.Combine(Path.GetTempPath(), "InnoCacheLinks"));
        string root = owner.directory;
        var publisher = new BuildCachePublisher(Path.Combine(root, "cache"), BuildCacheArtifactRole.ReusableWorkspace);
        string workspace = Path.Combine(root, "cache", "workspace");
        string producer = workspace + ".lock";
        using (FileLease activity = await publisher.AcquireUseAsync())
        using (FileLease writing = await FileLease.AcquireAsync(producer, TimeSpan.FromSeconds(5)))
        {
            publisher.BeginWorkingDirectory(workspace, producer, "macos-arm64");
            string library = Path.Combine(workspace, "libsample.1.0.dylib");
            File.WriteAllBytes(library, new byte[19]);
            File.CreateSymbolicLink(Path.Combine(workspace, "libsample.1.dylib"), "libsample.1.0.dylib");
            File.CreateSymbolicLink(Path.Combine(workspace, "libsample.dylib"), "libsample.1.dylib");
            long bytes = Directory.EnumerateFiles(workspace, "*", SearchOption.AllDirectories)
                .Where(path => new FileInfo(path).LinkTarget is null).Sum(path => new FileInfo(path).Length);
            publisher.Record(workspace, producer, [], null, "macos-arm64");
            BuildCacheCleanupPlan measured = BuildCacheCleanupPlan.Create(root, [publisher.Describe("fixture")], new(0, 0), retireAll: true);
            Assert.Equal(bytes, measured.totalBytes);
        }
        BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(root, [publisher.Describe("fixture")], new(0, 0), retireAll: true);
        BuildCacheCleanupResult result = await plan.ExecuteAsync();
        Assert.Equal(workspace, Assert.Single(result.retired));
        Assert.Empty(result.failures);
        Assert.False(Directory.Exists(workspace));
        Assert.False(File.Exists(producer));
        Assert.True(File.Exists(publisher.Describe("fixture").coordinationPath));
    }

    [UnixFileLinksFact]
    public async Task FileAliasesCannotEscapeCycleDangleOrEnterImmutableArtifacts()
    {
        using ProcessOwnedDirectory owner = await ProcessOwnedDirectory.CreateAsync(Path.Combine(Path.GetTempPath(), "InnoCacheLinkBoundary"));
        string root = owner.directory;
        string cache = Path.Combine(root, "cache");
        string workspace = Path.Combine(cache, "workspace");
        Directory.CreateDirectory(workspace);
        File.WriteAllText(Path.Combine(root, "outside"), "keep");
        var publisher = new BuildCachePublisher(cache, BuildCacheArtifactRole.ReusableWorkspace);
        string link = Path.Combine(workspace, "alias");
        using FileLease activity = await publisher.AcquireUseAsync();
        foreach (string target in new[] { "../../outside", "alias", "missing" })
        {
            File.CreateSymbolicLink(link, target);
            Assert.ThrowsAny<IOException>(() => publisher.Record(workspace, workspace + ".lock", [], null, null));
            File.Delete(link);
        }
        File.WriteAllText(Path.Combine(workspace, "regular"), "owned");
        File.CreateSymbolicLink(link, "regular");
        var immutable = new BuildCachePublisher(cache, BuildCacheArtifactRole.ImmutableArtifact);
        Assert.Throws<IOException>(() => immutable.Record(workspace, workspace + ".lock", [], null, null));
        Assert.Equal("keep", File.ReadAllText(Path.Combine(root, "outside")));
    }

    [Fact]
    public async Task WorkspacesRejectDirectoryLinksEvenWhenTheTargetIsOwned()
    {
        using ProcessOwnedDirectory owner = await ProcessOwnedDirectory.CreateAsync(Path.Combine(Path.GetTempPath(), "InnoCacheDirectoryLinks"));
        string root = owner.directory;
        string cache = Path.Combine(root, "cache");
        string workspace = Path.Combine(cache, "workspace");
        string target = Path.Combine(workspace, "target");
        Directory.CreateDirectory(target);
        string link = Path.Combine(workspace, "alias");
        if (OperatingSystem.IsWindows())
        {
            string escapedLink = link.Replace("'", "''", StringComparison.Ordinal);
            string escapedTarget = target.Replace("'", "''", StringComparison.Ordinal);
            await ToolchainEnvironment.RunAsync("powershell.exe",
                ["-NoProfile", "-NonInteractive", "-Command", "New-Item -ItemType Junction -Path '" + escapedLink + "' -Target '" + escapedTarget + "' | Out-Null"],
                root, CancellationToken.None);
        }
        else
            Directory.CreateSymbolicLink(link, target);
        try
        {
            var publisher = new BuildCachePublisher(cache, BuildCacheArtifactRole.ReusableWorkspace);
            using FileLease activity = await publisher.AcquireUseAsync();
            Assert.Throws<IOException>(() => publisher.Record(workspace, workspace + ".lock", [], null, null));
        }
        finally
        {
            Directory.Delete(link, recursive: false);
        }
    }
}
