namespace Inno.Build.Toolchains;

/// <summary>
/// Discovers one explicitly registered host implementation.
/// </summary>
public interface IBuildHostProvider
{
    /// <summary>
    /// Captures the current execution machine when this provider supports it.
    /// </summary>
    /// <returns>
    /// The immutable host, or null when the current machine is outside this provider's capabilities.
    /// </returns>
    BuildHostDescriptor? TryCapture();
}
