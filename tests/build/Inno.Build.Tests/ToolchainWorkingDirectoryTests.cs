using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Build.Windows.Host;

namespace Inno.Build.Tests;

public sealed class ToolchainWorkingDirectoryTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoToolchainDirectoryTests", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task BoundedExecutionRetainsFilesAndStableIdentityAcrossOwners()
    {
        string physical = Path.Combine(m_root, new string('p', 70), new string('p', 70));
        string execution;
        using (ToolchainWorkingDirectory owner = await CreateProvider().OpenAsync(physical))
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
        using ToolchainWorkingDirectory next = await CreateProvider().OpenAsync(physical);
        Assert.Equal(execution, next.toolPath);
        Assert.Equal("completed", File.ReadAllText(Path.Combine(next.toolPath, "product.txt")));
        next.Dispose();
        next.Dispose();
        if (OperatingSystem.IsWindows())
            Assert.False(Directory.Exists(execution));
    }

    [Fact]
    public async Task OpeningExistingToolOutputsPreservesThePublishedArtifactFileSet()
    {
        string artifact = Path.Combine(m_root, "Artifact");
        string outputs = Path.Combine(artifact, "Outputs");
        Directory.CreateDirectory(outputs);
        File.WriteAllText(Path.Combine(outputs, "tool.bin"), "immutable");
        string fingerprint = new('c', 64);
        BuildArtifactManifest.Write(artifact, fingerprint, ["Outputs"]);
        Assert.True(BuildArtifactManifest.IsComplete(artifact, fingerprint, ["Outputs"]));
        using (ToolchainWorkingDirectory owner = await CreateProvider().OpenAsync(outputs))
        {
            Assert.Equal("immutable", File.ReadAllText(Path.Combine(owner.toolPath, "tool.bin")));
            Assert.True(BuildArtifactManifest.IsComplete(artifact, fingerprint, ["Outputs"]));
        }
        Assert.True(BuildArtifactManifest.IsComplete(artifact, fingerprint, ["Outputs"]));
    }

    [Fact]
    public async Task CanceledWaitDoesNotRemoveAnotherOperationsExecutionPath()
    {
        if (!OperatingSystem.IsWindows())
        {
            await Assert.ThrowsAsync<PlatformNotSupportedException>(async () =>
                await new WindowsToolchainWorkspaceProvider().OpenAsync(m_root));
            return;
        }
        string physical = Path.Combine(m_root, "Owned");
        using ToolchainWorkingDirectory owner = await CreateProvider().OpenAsync(physical);
        using CancellationTokenSource cancellation = new(TimeSpan.FromMilliseconds(150));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await new WindowsToolchainWorkspaceProvider().OpenAsync(physical, cancellation.Token));
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
        using (ToolchainWorkingDirectory owner = await CreateProvider().OpenAsync(physical))
            alias = owner.toolPath;
        Directory.CreateDirectory(alias);
        try
        {
            await Assert.ThrowsAsync<IOException>(async () =>
                await CreateProvider().OpenAsync(physical));
            Assert.Null(new DirectoryInfo(alias).LinkTarget);
        }
        finally
        {
            Directory.Delete(alias, recursive: false);
        }
        using ToolchainWorkingDirectory restored = await CreateProvider().OpenAsync(physical);
        Assert.Equal(alias, restored.toolPath);
    }

    [Fact]
    public async Task RelativePathsAreRejectedBeforeCreatingAnAlias()
        => await Assert.ThrowsAsync<ArgumentException>(async () =>
            await new PhysicalToolchainWorkspaceProvider().OpenAsync("relative-build-owner"));

    [Fact]
    public async Task PhysicalProviderPreservesTheExactOwnerPath()
    {
        string physical = Path.Combine(m_root, "Physical workspace");
        using ToolchainWorkingDirectory execution = await new PhysicalToolchainWorkspaceProvider().OpenAsync(physical);
        Assert.Equal(physical, execution.toolPath);
        File.WriteAllText(Path.Combine(execution.toolPath, "output.txt"), "retained");
        Assert.Equal("retained", File.ReadAllText(Path.Combine(physical, "output.txt")));
    }

    private static IToolchainWorkspaceProvider CreateProvider() => OperatingSystem.IsWindows()
        ? new WindowsToolchainWorkspaceProvider() : new PhysicalToolchainWorkspaceProvider();

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
