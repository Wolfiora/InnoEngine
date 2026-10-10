using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Owns a stable execution path for tools whose Windows filesystem APIs require bounded paths.
/// The physical directory remains the caller's build-owned storage on every host.
/// </summary>
public sealed class ToolchainWorkingDirectory : IDisposable
{
    private readonly WindowsToolchainDirectory m_directory;
    private readonly FileLease m_ownership;
    private bool m_disposed;

    private ToolchainWorkingDirectory(
        WindowsToolchainDirectory directory,
        FileLease ownership
    ) {
        m_directory = directory;
        m_ownership = ownership;
    }

    /// <summary>
    /// Gets the execution directory, which is a temporary Windows junction or the physical path.
    /// All processes using this path must stop before this owner is disposed.
    /// </summary>
    public string toolPath => m_directory.toolPath;

    /// <summary>
    /// Waits for exclusive use of the directory's execution alias and creates the physical directory.
    /// The alias has a stable identity so tool caches can be reused across completed operations.
    /// </summary>
    /// <param name="physicalPath">
    /// The absolute build-owned directory. Relative inputs used by tools must resolve within it.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels waiting without disturbing another operation's execution path.
    /// </param>
    /// <returns>
    /// An owner that removes only its verified alias on disposal and preserves physical files.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The path is empty or is not absolute.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Ownership acquisition was canceled.
    /// </exception>
    /// <exception cref="IOException">
    /// The execution alias is occupied by another directory or ownership cannot be obtained.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// The host denies access to the physical directory or execution alias.
    /// </exception>
    /// <exception cref="System.ComponentModel.Win32Exception">
    /// Windows cannot create the execution junction.
    /// </exception>
    public static async ValueTask<ToolchainWorkingDirectory> OpenAsync(
        string physicalPath,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(physicalPath);
        if (!Path.IsPathFullyQualified(physicalPath))
            throw new ArgumentException("Toolchain working directories require an absolute owner path.", nameof(physicalPath));
        string physical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(physicalPath));
        FileLease ownership = await FileLease.AcquireAsync(GetAliasPath(physical) + ".lock",
            Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            return new(WindowsToolchainDirectory.Create(physical), ownership);
        }
        catch
        {
            ownership.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Removes the verified execution alias and releases ownership after tool processes have stopped.
    /// Physical files are retained; repeated disposal has no effect.
    /// </summary>
    /// <exception cref="IOException">
    /// The alias was replaced with a different target; that target is not removed.
    /// </exception>
    public void Dispose()
    {
        if (m_disposed)
            return;
        m_disposed = true;
        try
        {
            m_directory.Dispose();
        }
        finally
        {
            m_ownership.Dispose();
        }
    }

    internal static string GetAliasPath(string physicalPath)
    {
        string identityPath = OperatingSystem.IsWindows() ? physicalPath.ToUpperInvariant() : physicalPath;
        string identity = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(identityPath)))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        return Path.Combine(Path.GetTempPath(), "InnoTools", identity);
    }
}
