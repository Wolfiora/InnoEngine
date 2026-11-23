using System;
using System.IO;
using Inno.Build.Toolchains;

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
    /// <param name="host">
    /// The explicitly declared tool execution machine, independent of any publication target.
    /// </param>
    /// <param name="toolsTarget">
    /// The explicit native target of tools executed by this composition, independent of game publication.
    /// </param>
    /// <param name="nativeDependencies">
    /// An absolute dependency selection forbidding third-party production, or null for local source builds.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The SDK command is blank or the application directory is not absolute.
    /// </exception>
    public BuildCompositionContext(
        string dotnetHost,
        string applicationDirectory,
        BuildHostDescriptor host,
        BuildTargetID toolsTarget,
        string? nativeDependencies = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationDirectory);
        ArgumentNullException.ThrowIfNull(host);
        if (string.IsNullOrWhiteSpace(toolsTarget.value))
        {
            throw new ArgumentException("A build composition must declare its executable native tools target.", nameof(toolsTarget));
        }
        if (!Path.IsPathFullyQualified(applicationDirectory))
        {
            throw new ArgumentException("The build host requires an absolute application directory.", nameof(applicationDirectory));
        }
        if (nativeDependencies is not null && !Path.IsPathFullyQualified(nativeDependencies))
        {
            throw new ArgumentException("A native dependency selection must be absolute.", nameof(nativeDependencies));
        }
        this.dotnetHost = dotnetHost;
        this.applicationDirectory = Path.GetFullPath(applicationDirectory);
        this.host = host;
        this.toolsTarget = toolsTarget;
        this.nativeDependencies = nativeDependencies;
    }

    /// <summary>
    /// Gets the explicitly selected SDK executable.
    /// </summary>
    public string dotnetHost { get; }

    /// <summary>
    /// Gets the host location used for source provisioning discovery.
    /// </summary>
    public string applicationDirectory { get; }

    /// <summary>
    /// Gets the declared execution host without selecting a product target.
    /// </summary>
    public BuildHostDescriptor host { get; }

    /// <summary>
    /// Gets the independently declared native target of build tools executed by this host.
    /// </summary>
    public BuildTargetID toolsTarget { get; }

    /// <summary>
    /// Gets the explicit consume-only selection propagated to product and Support Pack preparation.
    /// </summary>
    public string? nativeDependencies { get; }
}
