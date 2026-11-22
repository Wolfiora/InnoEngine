namespace Inno.Core.Events;

/// <summary>
/// Raised when a window gains or loses input focus.
/// </summary>
/// <param name="windowID">
/// The window id used to initialize this instance.
/// </param>
/// <param name="isFocused">
/// The is focused used to initialize this instance.
/// </param>
public sealed class WindowFocusChangedEvent(
    uint windowID,
    bool isFocused
) : WindowEvent(windowID)
{
    /// <summary>
    /// Gets whether the window has input focus after the change.
    /// </summary>
    public bool isFocused { get; } = isFocused;
}
