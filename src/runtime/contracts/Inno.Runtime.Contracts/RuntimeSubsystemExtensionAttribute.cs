using System;

namespace Inno.Runtime.Contracts;

/// <summary>
/// Declares the stable identity, owner and dependencies of a discoverable subsystem activator.
/// </summary>
/// <param name="id">
/// The unique subsystem ID within the owner pipeline.
/// </param>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RuntimeSubsystemExtensionAttribute(string id) : Attribute
{
    /// <summary>
    /// Gets the subsystem ID used by dependencies and diagnostics.
    /// </summary>
    public string id { get; } = new RuntimeSubsystemID(id).value;

    /// <summary>
    /// Gets or sets the host or session that owns each instance.
    /// </summary>
    public RuntimeSubsystemLifetime lifetime { get; set; } = RuntimeSubsystemLifetime.Session;

    /// <summary>
    /// Gets or sets subsystem IDs that must attach before this instance.
    /// </summary>
    public string[] dependencies { get; set; } = [];

    /// <summary>
    /// Gets or sets the capabilities required from product composition.
    /// </summary>
    public string[] requiredCapabilities { get; set; } = [];

    /// <summary>
    /// Gets or sets whether replacement requires releasing an exclusive resource first.
    /// An active instance with this requirement must be replaced by restarting the host.
    /// </summary>
    public bool requiresRestart { get; set; }
}
