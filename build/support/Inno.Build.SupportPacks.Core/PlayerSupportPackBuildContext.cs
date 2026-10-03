using System;

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
    /// <param name="dotnetHost">
    /// The host-selected SDK executable.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A required input is blank.
    /// </exception>
    public PlayerSupportPackBuildContext(
        string engineRoot,
        string stagingDirectory,
        string dotnetHost
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(stagingDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        this.engineRoot = engineRoot;
        this.stagingDirectory = stagingDirectory;
        this.dotnetHost = dotnetHost;
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
    /// Gets the SDK executable selected by the composition host.
    /// </summary>
    public string dotnetHost { get; }
}
