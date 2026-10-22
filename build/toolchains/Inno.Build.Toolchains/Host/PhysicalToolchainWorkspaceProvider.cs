using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Executes tools directly in their physical owner directory.
/// </summary>
public sealed class PhysicalToolchainWorkspaceProvider : IToolchainWorkspaceProvider
{
    /// <inheritdoc />
    public string identity => "physical";

    /// <inheritdoc />
    public ValueTask<ToolchainWorkingDirectory> OpenAsync(
        string physicalPath,
        CancellationToken cancellationToken = default
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        if (!Path.IsPathFullyQualified(physicalPath))
            throw new ArgumentException("Tool execution requires an absolute physical owner path.", nameof(physicalPath));
        string physical = Path.GetFullPath(physicalPath);
        Directory.CreateDirectory(physical);
        return ValueTask.FromResult(new ToolchainWorkingDirectory(physical));
    }
}
