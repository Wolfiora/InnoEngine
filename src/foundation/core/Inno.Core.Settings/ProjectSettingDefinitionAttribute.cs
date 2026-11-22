using System;

namespace Inno.Core.Settings;

/// <summary>
/// Declares a reloadable settings type under one stable protocol identity.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ProjectSettingDefinitionAttribute : Attribute
{
    /// <summary>
    /// Creates a project setting declaration.
    /// </summary>
    /// <param name="id">
    /// Globally stable setting identifier.
    /// </param>
    /// <param name="allowPluginContributions">
    /// Whether Plugin packages may contribute values to this protocol.
    /// </param>
    public ProjectSettingDefinitionAttribute(
        string id,
        bool allowPluginContributions = true
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
        this.allowPluginContributions = allowPluginContributions;
    }

    /// <summary>
    /// Gets the globally stable setting identifier.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets whether Plugin packages may contribute values to this protocol.
    /// </summary>
    public bool allowPluginContributions { get; }
}
