using Inno.Core.Input;

namespace Inno.Core.Events;

/// <summary>
/// Raised when a mouse button is released.
/// </summary>
/// <param name="windowID">
/// The window id used to initialize this instance.
/// </param>
/// <param name="button">
/// The button used to initialize this instance.
/// </param>
public sealed class MouseButtonReleasedEvent(
    uint windowID,
    MouseButton button
) : MouseButtonEvent(windowID, button);
