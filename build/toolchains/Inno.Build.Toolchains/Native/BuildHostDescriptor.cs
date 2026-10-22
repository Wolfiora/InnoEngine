using System;

namespace Inno.Build.Toolchains;

/// <summary>
/// Identifies the tool execution machine without selecting any product target.
/// </summary>
public sealed record BuildHostDescriptor
{
    /// <summary>
    /// Captures host facts supplied by a product or command composition boundary.
    /// </summary>
    /// <param name="system">
    /// The execution operating system identity.
    /// </param>
    /// <param name="architecture">
    /// The execution processor architecture.
    /// </param>
    /// <param name="services">
    /// Explicit host execution services, or null for direct physical execution.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A host fact is unassigned.
    /// </exception>
    public BuildHostDescriptor(
        string system,
        string architecture,
        BuildHostServices? services = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(system);
        ArgumentException.ThrowIfNullOrWhiteSpace(architecture);
        this.services = services ?? new BuildHostServices(new PhysicalToolchainWorkspaceProvider());
        this.system = system;
        this.architecture = architecture;
    }

    /// <summary>
    /// Gets the operating system on which tools execute.
    /// </summary>
    public string system { get; }

    /// <summary>
    /// Gets the processor architecture on which tools execute.
    /// </summary>
    public string architecture { get; }

    /// <summary>
    /// Gets frozen host execution services.
    /// </summary>
    public BuildHostServices services { get; }
}
