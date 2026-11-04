using System;
using System.IO;
using System.Threading.Tasks;

using Inno.Core.IO;
using Xunit;

namespace Inno.Core.IO.Tests;

public sealed class ProcessOwnedDirectoryTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoOwnedDirectory", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task FailedRetirementCanBeRetriedAfterTheReaderReleasesItsLease()
    {
        using ProcessOwnedDirectory directory = await ProcessOwnedDirectory.CreateAsync(m_root);
        string payload = Path.Combine(directory.directory, "payload");
        await File.WriteAllTextAsync(payload, "owned");
        using CoordinatedFileLease reader = await CoordinatedFileLease.AcquireAsync(
            Path.Combine(m_root, "owner.lock"), directory.directory + ".work.lock", TimeSpan.Zero, shared: true);

        Assert.Throws<IOException>(directory.Dispose);
        Assert.True(File.Exists(payload));
        Assert.Single(ProcessOwnedDirectory.ReadClaims(m_root));
        reader.Dispose();

        directory.Dispose();
        Assert.False(Directory.Exists(directory.directory));
        directory.Dispose();
        using FileLease maintenance = await FileLease.AcquireAsync(Path.Combine(m_root, "owner.lock"), TimeSpan.Zero);
        Assert.Equal(1, ProcessOwnedDirectory.RetireMetadataUnderOwnership(m_root));
        Assert.Empty(ProcessOwnedDirectory.ReadClaims(m_root));
    }

    [Fact]
    public async Task CompletedRetirementDoesNotAdoptANewDirectoryAtTheSamePath()
    {
        using ProcessOwnedDirectory directory = await ProcessOwnedDirectory.CreateAsync(m_root);
        directory.Dispose();
        Directory.CreateDirectory(directory.directory);
        string unknown = Path.Combine(directory.directory, "unknown");
        await File.WriteAllTextAsync(unknown, "not owned by the retired operation");

        directory.Dispose();

        Assert.True(File.Exists(unknown));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
