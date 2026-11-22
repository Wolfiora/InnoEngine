namespace Inno.Plugins.Authoring;

/// <summary>
/// Identifies how an <c>.iplugin</c> package participates in one installation.
/// </summary>
public enum PluginSourceKind
{
    /// <summary>
    /// The Plugin is installed as one top-level <c>.iplugin</c> package.
    /// </summary>
    Package,
    /// <summary>
    /// The Plugin is embedded as one complete <c>.iplugin</c> dependency package.
    /// </summary>
    EmbeddedPackage
}
