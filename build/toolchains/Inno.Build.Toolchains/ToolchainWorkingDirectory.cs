using System;
using System.IO;
using System.Threading;

namespace Inno.Build.Toolchains;

/// <summary>
/// Retains a provider-owned tool execution path until all dependent processes finish.
/// </summary>
public sealed class ToolchainWorkingDirectory : IDisposable
{
    private IDisposable? m_ownership;

    /// <summary>
    /// Captures an absolute execution path and its optional release protocol.
    /// </summary>
    /// <param name="toolPath">
    /// The execution directory selected by the host provider.
    /// </param>
    /// <param name="ownership">
    /// The alias and lease owner, or null for direct physical execution.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The execution path is not absolute.
    /// </exception>
    public ToolchainWorkingDirectory(
        string toolPath,
        IDisposable? ownership = null
    ) {
        if (!Path.IsPathFullyQualified(toolPath))
            throw new ArgumentException("Tool execution paths must be absolute.", nameof(toolPath));
        this.toolPath = toolPath;
        m_ownership = ownership;
    }

    /// <summary>
    /// Gets the execution path, valid until disposal.
    /// </summary>
    public string toolPath { get; }

    /// <summary>
    /// Releases provider resources after all dependent processes have stopped.
    /// </summary>
    public void Dispose() => Interlocked.Exchange(ref m_ownership, null)?.Dispose();
}
