using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Freezes host execution capabilities independently of a product target SDK.
/// </summary>
public sealed class BuildHostServices
{
    /// <summary>
    /// Captures execution policy and its implementation inputs.
    /// </summary>
    /// <param name="workspaces">
    /// The host's scoped execution path provider.
    /// </param>
    /// <param name="environmentComparer">
    /// The host's environment variable name semantics.
    /// </param>
    /// <param name="inputPaths">
    /// Checkout-relative implementation files included in native recipe identity.
    /// </param>
    /// <param name="executableExtension">
    /// The host executable suffix, or an empty string for suffix-free executable names.
    /// </param>
    public BuildHostServices(
        IToolchainWorkspaceProvider workspaces,
        StringComparer? environmentComparer = null,
        IEnumerable<string>? inputPaths = null,
        string executableExtension = ""
    ) {
        ArgumentNullException.ThrowIfNull(workspaces);
        this.workspaces = workspaces;
        this.environmentComparer = environmentComparer ?? StringComparer.Ordinal;
        this.inputPaths = Array.AsReadOnly((inputPaths ?? []).ToArray());
        this.executableExtension = executableExtension;
    }

    /// <summary>
    /// Gets the host's scoped execution path provider.
    /// </summary>
    public IToolchainWorkspaceProvider workspaces { get; }
    /// <summary>
    /// Gets environment variable name semantics.
    /// </summary>
    public StringComparer environmentComparer { get; }
    /// <summary>
    /// Gets checkout-relative execution implementation inputs.
    /// </summary>
    public IReadOnlyList<string> inputPaths { get; }
    /// <summary>
    /// Gets the host-owned executable suffix.
    /// </summary>
    public string executableExtension { get; }
}
