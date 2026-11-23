namespace Inno.Core.Events;

/// <summary>
/// Reports whether a platform window can present visible content.
/// </summary>
/// <param name="windowID">
/// The application-local identity of the affected window.
/// </param>
/// <param name="isVisible">
/// Whether the window is shown and not minimized.
/// </param>
public sealed class WindowVisibilityChangedEvent(
    uint windowID,
    bool isVisible
) : WindowEvent(windowID)
{
    /// <summary>
    /// Gets whether the window can present visible content after this notification.
    /// </summary>
    public bool isVisible { get; } = isVisible;
}
