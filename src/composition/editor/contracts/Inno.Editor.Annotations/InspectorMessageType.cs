namespace Inno.Editor.Annotations;

/// <summary>
/// Selects the visual severity of an Inspector help message.
/// </summary>
public enum InspectorMessageType
{
    /// <summary>
    /// Neutral explanatory information.
    /// </summary>
    Info,
    /// <summary>
    /// A non-blocking configuration warning.
    /// </summary>
    Warning,
    /// <summary>
    /// An invalid or unusable configuration.
    /// </summary>
    Error
}
