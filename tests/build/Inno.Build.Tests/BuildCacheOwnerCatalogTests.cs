using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BuildCacheOwnerCatalogTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoOwnerCatalog", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task MaintenanceUsesPublisherInventoryWithoutAdoptingUnrecordedDirectories()
    {
        string ownerRoot = Path.Combine(m_root, "artifacts", "fixture");
        var publisher = new BuildCachePublisher(ownerRoot, BuildCacheArtifactRole.ImmutableArtifact);
        BuildCacheOwnerCatalog.Register(m_root, ownerRoot, "fixture", publisher.role);
        string owned = Path.Combine(ownerRoot, "recorded");
        string unknown = Path.Combine(ownerRoot, "unrecorded");
        Directory.CreateDirectory(owned);
        Directory.CreateDirectory(unknown);
        File.WriteAllText(Path.Combine(owned, "value"), "owned");
        File.WriteAllText(Path.Combine(unknown, "value"), "user");
        using (await publisher.AcquireUseAsync())
            publisher.Record(owned, owned + ".read.lock", [], null, "windows-x64");
        Assert.Single(BuildCacheOwnerCatalog.ReadOwners(m_root));
        await BuildMaintenanceApplication.CleanAsync(m_root, new(0, 0), true, true, "windows-x64", CancellationToken.None);
        Assert.True(Directory.Exists(owned));
        await BuildMaintenanceApplication.CleanAsync(m_root, new(0, 0), true, false, "windows-x64", CancellationToken.None);
        Assert.False(Directory.Exists(owned));
        Assert.True(Directory.Exists(unknown));
    }

    [Fact]
    public async Task EmptyCheckoutAndInvalidRequestsDoNotPrepareGeneratorsOrClaimArtifacts()
    {
        Assert.Empty(BuildCacheOwnerCatalog.ReadOwners(m_root));
        Assert.Equal(0, await BuildMaintenanceApplication.Run(["clean", "--scope", "cache", "--engine-root", m_root, "--dry-run"], CancellationToken.None));
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts", "native")));
        Assert.Equal(1, await BuildMaintenanceApplication.Run(["clean", "--scope", "cache", "--scope", "cache", "--engine-root", m_root], CancellationToken.None));
        using var canceled = new CancellationTokenSource();
        canceled.Cancel();
        Assert.Equal(2, await BuildMaintenanceApplication.Run(["clean"], canceled.Token));
    }

    [Fact]
    public void CorruptRegistrationOrChangedRoleIsRejectedBeforeRetirement()
    {
        string ownerRoot = Path.Combine(m_root, "artifacts", "fixture");
        BuildCacheOwnerCatalog.Register(m_root, ownerRoot, "fixture", BuildCacheArtifactRole.ImmutableArtifact);
        Assert.Throws<InvalidDataException>(() => BuildCacheOwnerCatalog.Register(m_root, ownerRoot, "fixture", BuildCacheArtifactRole.ReusableWorkspace));
        string registration = Directory.EnumerateFiles(Path.Combine(m_root, "artifacts", "cache-owners"), "*.json").Single();
        File.WriteAllText(registration, "{}");
        Assert.Throws<InvalidDataException>(() => BuildCacheOwnerCatalog.ReadOwners(m_root));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
