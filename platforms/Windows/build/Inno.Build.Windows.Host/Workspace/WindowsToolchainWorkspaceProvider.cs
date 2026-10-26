using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Windows.Host;

/// <summary>
/// Owns stable bounded Windows junctions for native compiler workspaces.
/// </summary>
public sealed class WindowsToolchainWorkspaceProvider : IToolchainWorkspaceProvider
{
    /// <inheritdoc />
    public string identity => Path.Combine(Path.GetTempPath(), "InnoTools");

    /// <inheritdoc />
    public async ValueTask<ToolchainWorkingDirectory> OpenAsync(
        string physicalPath,
        CancellationToken cancellationToken = default
    ) {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("Windows execution workspaces require a Windows host.");
        if (!Path.IsPathFullyQualified(physicalPath))
            throw new ArgumentException("Toolchain workspaces require absolute owner paths.", nameof(physicalPath));
        string physical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(physicalPath));
        string leaf = GetAliasPath(physical) + ".lock";
        string owner = Path.Combine(Path.GetDirectoryName(leaf)!, "owner.lock");
        CoordinatedFileLease lease = await CoordinatedFileLease.AcquireAsync(owner, leaf,
            Timeout.InfiniteTimeSpan, cancellationToken: cancellationToken).ConfigureAwait(false);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            WindowsToolchainDirectory directory = WindowsToolchainDirectory.Create(physical);
            return new ToolchainWorkingDirectory(directory.toolPath, new Ownership(directory, lease, owner, leaf));
        }
        catch
        {
            lease.Dispose();
            throw;
        }
    }

    internal static string GetAliasPath(string physicalPath)
    {
        string identity = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(physicalPath.ToUpperInvariant())))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        return Path.Combine(Path.GetTempPath(), "InnoTools", identity);
    }

    private sealed class Ownership(
        WindowsToolchainDirectory directory,
        CoordinatedFileLease lease,
        string owner,
        string leaf
    ) : IDisposable
    {
        /// <inheritdoc />
        public void Dispose()
        {
            try
            {
                directory.Dispose();
            }
            finally
            {
                lease.Dispose();
            }
            try
            {
                using FileLease retirement = FileLease.AcquireAsync(owner, TimeSpan.Zero).AsTask().GetAwaiter().GetResult();
                File.Delete(leaf);
            }
            catch (TimeoutException)
            {
                // A waiter retains the same coordination domain and retires the leaf when it finishes.
            }
        }
    }
}
