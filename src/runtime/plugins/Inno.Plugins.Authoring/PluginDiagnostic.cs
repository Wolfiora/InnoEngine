namespace Inno.Plugins.Authoring;

/// <summary>
/// Reports one Plugin discovery, validation, or dependency problem.
/// </summary>
public sealed class PluginDiagnostic
{
    /// <summary>
    /// Creates a Plugin source diagnostic.
    /// </summary>
    /// <param name="sourcePath">
    /// Related <c>.iplugin</c> package path or invalid installation entry.
    /// </param>
    /// <param name="message">
    /// Actionable problem description.
    /// </param>
    public PluginDiagnostic(
        string sourcePath,
        string message
    ) {
        this.sourcePath = sourcePath ?? string.Empty;
        this.message = message ?? string.Empty;
    }

    /// <summary>
    /// Gets the related <c>.iplugin</c> package path or invalid installation entry.
    /// </summary>
    public string sourcePath { get; }

    /// <summary>
    /// Gets the actionable problem description.
    /// </summary>
    public string message { get; }
}
