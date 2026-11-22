using Inno.Core.Input;

namespace Inno.Core.Events;

/// <summary>
/// Base class for mouse button events.
/// </summary>
/// <param name="windowID">
/// The window id used to initialize this instance.
/// </param>
/// <param name="button">
/// The button used to initialize this instance.
/// </param>
public abstract class MouseButtonEvent(
    uint windowID,
    MouseButton button
) : MouseEvent(windowID)
{
    /// <summary>
    /// Gets the mouse button for this event.
    /// </summary>
    public MouseButton button { get; } = button;
}
