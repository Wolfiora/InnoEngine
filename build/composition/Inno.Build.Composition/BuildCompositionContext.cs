using System;
using System.IO;

namespace Inno.Build.Composition;

/// <summary>
/// Captures host-owned SDK selection and application location without probing global process state.
/// </summary>
public sealed class BuildCompositionContext
{
    /// <summary>
    /// Captures the host dependencies used by deployment and Support Pack preparation.
    /// </summary>
    /// <param name="dotnetHost">
    /// The SDK executable selected by the host, either an executable path or a command name.
    /// </param>
    /// <param name="applicationDirectory">
    /// The absolute directory from which source-based Support Pack provisioning can be resolved.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The SDK command is blank or the application directory is not absolute.
    /// </exception>
    public BuildCompositionContext(
        string dotnetHost,
        string applicationDirectory
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationDirectory);
        if (!Path.IsPathFullyQualified(applicationDirectory))
            throw new ArgumentException("The build host requires an absolute application directory.", nameof(applicationDirectory));
        this.dotnetHost = dotnetHost;
        this.applicationDirectory = Path.GetFullPath(applicationDirectory);
    }

    /// <summary>
    /// Gets the explicitly selected SDK executable.
    /// </summary>
    public string dotnetHost { get; }

    /// <summary>
    /// Gets the host location used for source provisioning discovery.
    /// </summary>
    public string applicationDirectory { get; }
}
