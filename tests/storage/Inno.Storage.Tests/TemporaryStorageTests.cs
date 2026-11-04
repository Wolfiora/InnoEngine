using System;
using System.IO;
using System.Threading.Tasks;

using Inno.Adapter.Storage;
using Inno.Adapter.Storage.FileSystem;
using Inno.Core.IO;
using Inno.Storage.Runtime;
using Xunit;

namespace Inno.Storage.Tests;

public sealed class TemporaryStorageTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoTemporaryStorage", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task TemporarySandboxesDoNotReadOrReplacePersistentOrSiblingValues()
    {
        var catalog = new StorageBackendCatalog([new FileSystemStorageBackendProvider(m_root)]);
        var scope = new StorageScope("editor.preview");
        var key = new StorageKey("save");
        using var persistent = Assert.IsType<FileSystemApplicationStorage>(catalog.CreateStorage(StorageBackendId.fileSystem, scope));
        using var first = Assert.IsType<FileSystemApplicationStorage>(catalog.CreateTemporaryStorage(StorageBackendId.fileSystem, scope));
        using var second = Assert.IsType<FileSystemApplicationStorage>(catalog.CreateTemporaryStorage(StorageBackendId.fileSystem, scope));
        await persistent.WriteAsync(key, new byte[] { 1 });
        Assert.Null(await first.ReadAsync(key));
        Assert.Null(await second.ReadAsync(key));
        await first.WriteAsync(key, new byte[] { 2 });
        await second.WriteAsync(key, new byte[] { 3 });
        Assert.NotEqual(first.rootDirectory, second.rootDirectory);

        first.Dispose();
        Assert.False(Directory.Exists(first.rootDirectory));
        Assert.Equal(new byte[] { 1 }, await persistent.ReadAsync(key));
        Assert.Equal(new byte[] { 3 }, await second.ReadAsync(key));
        second.Dispose();
        Assert.False(Directory.Exists(second.rootDirectory));
        Assert.Empty(Directory.GetFiles(Path.GetDirectoryName(second.rootDirectory)!, "*.work.*"));
        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await first.ReadAsync(key));
    }

    [Fact]
    public async Task RuntimeRetirementRemovesTemporaryPayloadAndMetadataAcrossOneHundredSessions()
    {
        var provider = new FileSystemStorageBackendProvider(m_root);
        var scope = new StorageScope("editor.preview");
        for (int i = 0; i < 100; i++)
        {
            var storage = Assert.IsType<FileSystemApplicationStorage>(provider.CreateTemporaryStorage(scope));
            using var runtime = new StorageRuntime(storage);
            await runtime.WriteAsync(new StorageKey("nested/save"), new byte[] { 7 });
            runtime.Dispose();
            Assert.False(Directory.Exists(storage.rootDirectory));
        }
        string temporaryRoot = Path.Combine(m_root, scope.value!, "Temporary");
        Assert.Empty(Directory.GetDirectories(temporaryRoot));
        Assert.Equal(new[] { "owner.lock" }, Array.ConvertAll(Directory.GetFiles(temporaryRoot), Path.GetFileName));
        Assert.False(Directory.Exists(Path.Combine(m_root, scope.value!, "Storage")));
    }

    [Fact]
    public async Task FailedRetirementStopsAdmissionAndCanBeRetriedWithoutDeletingASibling()
    {
        var provider = new FileSystemStorageBackendProvider(m_root);
        using var storage = Assert.IsType<FileSystemApplicationStorage>(provider.CreateTemporaryStorage(new("editor.preview")));
        using var sibling = Assert.IsType<FileSystemApplicationStorage>(provider.CreateTemporaryStorage(new("editor.preview")));
        string producer = Path.GetDirectoryName(storage.rootDirectory)!;
        using CoordinatedFileLease reader = await CoordinatedFileLease.AcquireAsync(
            Path.Combine(producer, "owner.lock"), storage.rootDirectory + ".work.lock", TimeSpan.Zero, shared: true);
        Assert.Throws<IOException>(storage.Dispose);
        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await storage.WriteAsync(new("late"), new byte[] { 1 }));
        Assert.True(Directory.Exists(storage.rootDirectory));
        reader.Dispose();

        storage.Dispose();
        Assert.False(Directory.Exists(storage.rootDirectory));
        Assert.True(Directory.Exists(sibling.rootDirectory));
        sibling.Dispose();
        Assert.Empty(Directory.GetFiles(producer, "*.work.*"));
    }

    [Fact]
    public void UnsupportedProviderDoesNotCreatePersistentStorageAsAFallback()
    {
        var provider = new PersistentOnlyProvider();
        var catalog = new StorageBackendCatalog([provider]);
        Assert.Throws<NotSupportedException>(() => catalog.CreateTemporaryStorage(provider.id, new("preview")));
        Assert.Equal(0, provider.persistentRequests);
        Assert.Throws<ArgumentException>(() => catalog.CreateTemporaryStorage(provider.id, default));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private sealed class PersistentOnlyProvider() : StorageBackendProvider(new("tests.persistent"))
    {
        internal int persistentRequests;

        public override IApplicationStorage CreateStorage(StorageScope scope)
        {
            persistentRequests++;
            throw new InvalidOperationException("Persistent storage must not be requested by temporary admission.");
        }
    }
}
