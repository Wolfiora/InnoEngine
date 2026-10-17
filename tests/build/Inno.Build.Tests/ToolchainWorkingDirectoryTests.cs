using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class ToolchainWorkingDirectoryTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoToolchainDirectoryTests", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task BoundedExecutionRetainsFilesAndStableIdentityAcrossOwners()
    {
        string physical = Path.Combine(m_root, new string('p', 70), new string('p', 70));
        string execution;
        using (ToolchainWorkingDirectory owner = await ToolchainWorkingDirectory.OpenAsync(physical))
        {
            execution = owner.toolPath;
            File.WriteAllText(Path.Combine(execution, "product.txt"), "completed");
            Assert.Equal("completed", File.ReadAllText(Path.Combine(physical, "product.txt")));
            if (OperatingSystem.IsWindows())
            {
                Assert.True(execution.Length < physical.Length);
                Assert.NotNull(new DirectoryInfo(execution).LinkTarget);
            }
            else
            {
                Assert.Equal(physical, execution);
            }
        }
        Assert.True(File.Exists(Path.Combine(physical, "product.txt")));
        using ToolchainWorkingDirectory next = await ToolchainWorkingDirectory.OpenAsync(physical);
        Assert.Equal(execution, next.toolPath);
        Assert.Equal("completed", File.ReadAllText(Path.Combine(next.toolPath, "product.txt")));
        next.Dispose();
        next.Dispose();
        if (OperatingSystem.IsWindows())
            Assert.False(Directory.Exists(execution));
    }

    [Fact]
    public async Task CanceledWaitDoesNotRemoveAnotherOperationsExecutionPath()
    {
        string physical = Path.Combine(m_root, "Owned");
        using ToolchainWorkingDirectory owner = await ToolchainWorkingDirectory.OpenAsync(physical);
        using CancellationTokenSource cancellation = new(TimeSpan.FromMilliseconds(150));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await ToolchainWorkingDirectory.OpenAsync(physical, cancellation.Token));
        Assert.True(Directory.Exists(owner.toolPath));
        File.WriteAllText(Path.Combine(owner.toolPath, "retained.txt"), "owner");
        Assert.Equal("owner", File.ReadAllText(Path.Combine(physical, "retained.txt")));
    }

    [Fact]
    public async Task OccupiedAliasFailsWithoutRemovingTheOccupantAndReleasesOwnership()
    {
        if (!OperatingSystem.IsWindows())
            return;
        string physical = Path.Combine(m_root, "Occupied");
        string alias;
        using (ToolchainWorkingDirectory owner = await ToolchainWorkingDirectory.OpenAsync(physical))
            alias = owner.toolPath;
        Directory.CreateDirectory(alias);
        try
        {
            await Assert.ThrowsAsync<IOException>(async () =>
                await ToolchainWorkingDirectory.OpenAsync(physical));
            Assert.Null(new DirectoryInfo(alias).LinkTarget);
        }
        finally
        {
            Directory.Delete(alias, recursive: false);
        }
        using ToolchainWorkingDirectory restored = await ToolchainWorkingDirectory.OpenAsync(physical);
        Assert.Equal(alias, restored.toolPath);
    }

    [Fact]
    public async Task RelativePathsAreRejectedBeforeCreatingAnAlias()
        => await Assert.ThrowsAsync<ArgumentException>(async () =>
            await ToolchainWorkingDirectory.OpenAsync("relative-build-owner"));

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
