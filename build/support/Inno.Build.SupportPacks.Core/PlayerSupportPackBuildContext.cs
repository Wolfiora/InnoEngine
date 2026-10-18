using System;
using Inno.Build.Toolchains;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Supplies preparation with explicit paths and a selected SDK, excluding installed output.
/// </summary>
public sealed class PlayerSupportPackBuildContext
{
    /// <summary>
    /// Creates immutable inputs for an isolated preparation operation.
    /// </summary>
    /// <param name="engineRoot">
    /// The complete engine source checkout.
    /// </param>
    /// <param name="stagingDirectory">
    /// The empty transaction directory to populate.
    /// </param>
    /// <param name="sdk">
    /// The project-selected SDK frozen during read-only preflight.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A required input is blank.
    /// </exception>
    public PlayerSupportPackBuildContext(
        string engineRoot,
        string stagingDirectory,
        DotNetSdkDescriptor sdk
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(stagingDirectory);
        ArgumentNullException.ThrowIfNull(sdk);
        this.engineRoot = engineRoot;
        this.stagingDirectory = stagingDirectory;
        this.sdk = sdk;
    }

    /// <summary>
    /// Gets the engine source checkout.
    /// </summary>
    public string engineRoot { get; }

    /// <summary>
    /// Gets the isolated directory that the source must populate.
    /// </summary>
    public string stagingDirectory { get; }

    /// <summary>
    /// Gets the frozen SDK host and CLI entry used without resolving another SDK.
    /// </summary>
    public DotNetSdkDescriptor sdk { get; }
}
