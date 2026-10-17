namespace Inno.Core.Events;

/// <summary>
/// Reports whether the application must stop advancing frames until the platform resumes it.
/// </summary>
/// <param name="isSuspended">
/// Whether the platform has suspended application execution.
/// </param>
public sealed class ApplicationSuspensionChangedEvent(bool isSuspended) : ApplicationEvent
{
    /// <summary>
    /// Gets whether application execution is suspended after this notification.
    /// </summary>
    public bool isSuspended { get; } = isSuspended;
}
