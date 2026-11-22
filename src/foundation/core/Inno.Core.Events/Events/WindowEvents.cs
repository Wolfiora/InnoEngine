namespace Inno.Core.Events;

/// <summary>
/// Base class for window lifecycle events.
/// </summary>
/// <param name="windowID">
/// The window id used to initialize this instance.
/// </param>
public abstract class WindowEvent(uint windowID) : Event
{
    /// <summary>
    /// Gets the source window id for this event.
    /// </summary>
    public uint windowID { get; } = windowID;
}

/// <summary>
/// Raised when the window size changes.
/// </summary>
/// <param name="windowID">
/// The window id used to initialize this instance.
/// </param>
/// <param name="width">
/// The width in logical units or pixels required by this operation.
/// </param>
/// <param name="height">
/// The height in logical units or pixels required by this operation.
/// </param>
public sealed class WindowResizeEvent(
    uint windowID,
    int width,
    int height
) : WindowEvent(windowID)
{
    /// <summary>
    /// Gets the new window width.
    /// </summary>
    public int width { get; } = width;

    /// <summary>
    /// Gets the new window height.
    /// </summary>
    public int height { get; } = height;
}

/// <summary>
/// Raised when the window requests close.
/// </summary>
/// <param name="windowID">
/// The window id used to initialize this instance.
/// </param>
public sealed class WindowCloseEvent(uint windowID) : WindowEvent(windowID);

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
