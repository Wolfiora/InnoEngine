using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Owns the relationship between physical build storage and host tool execution paths.
/// </summary>
public interface IToolchainWorkspaceProvider
{
    /// <summary>
    /// Gets the stable execution namespace included in compiler workspace identity.
    /// </summary>
    string identity { get; }

    /// <summary>
    /// Acquires an execution path for an absolute build-owned directory.
    /// </summary>
    /// <param name="physicalPath">
    /// The retained physical directory.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation while waiting for exclusive alias ownership.
    /// </param>
    /// <returns>
    /// A scoped path whose processes must finish before disposal.
    /// </returns>
    ValueTask<ToolchainWorkingDirectory> OpenAsync(
        string physicalPath,
        CancellationToken cancellationToken = default
    );
}
