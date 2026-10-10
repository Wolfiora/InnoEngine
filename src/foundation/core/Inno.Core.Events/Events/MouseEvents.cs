using Inno.Core.Input;

namespace Inno.Core.Events;

/// <summary>
/// Base class for mouse events.
/// </summary>
public abstract class MouseEvent : Event
{
    /// <summary>
    /// Creates an independent mouse event belonging to one platform window.
    /// </summary>
    /// <param name="windowId">
    /// The platform window that produced the event.
    /// </param>
    protected MouseEvent(uint windowId) => this.windowId = windowId;

    /// <summary>
    /// Creates a routed mouse event sharing its source's window and global consumption.
    /// </summary>
    /// <param name="source">
    /// The mouse event whose input lifetime remains authoritative.
    /// </param>
    /// <exception cref="System.ArgumentNullException">
    /// The source event is null.
    /// </exception>
    protected MouseEvent(MouseEvent source) : base(source) => windowId = source.windowId;

    /// <summary>
    /// Gets the source window id for this event.
    /// </summary>
    public uint windowId { get; }
}


/// <summary>
/// Raised when the cursor moves.
/// </summary>
public class MouseMovedEvent : MouseEvent
{
    /// <summary>
    /// Creates a pointer movement in platform-window coordinates.
    /// </summary>
    /// <param name="windowId">
    /// The platform window that produced the movement.
    /// </param>
    /// <param name="x">
    /// The horizontal pointer coordinate in logical pixels.
    /// </param>
    /// <param name="y">
    /// The vertical pointer coordinate in logical pixels.
    /// </param>
    public MouseMovedEvent(
        uint windowId,
        float x,
        float y
    ) : base(windowId) {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Gets cursor X coordinate.
    /// </summary>
    public float x { get; }

    /// <summary>
    /// Gets cursor Y coordinate.
    /// </summary>
    public float y { get; }

    /// <summary>
    /// Repositions this movement without starting a new global consumption lifetime.
    /// </summary>
    /// <param name="x">
    /// The routed horizontal coordinate in logical pixels.
    /// </param>
    /// <param name="y">
    /// The routed vertical coordinate in logical pixels.
    /// </param>
    /// <returns>
    /// A new coordinate representation sharing global consumption with this movement.
    /// </returns>
    public MouseMovedEvent WithPosition(
        float x,
        float y
    ) => new(this, x, y);

    private MouseMovedEvent(
        MouseMovedEvent source,
        float x,
        float y
    ) : base(source) {
        this.x = x;
        this.y = y;
    }
}

/// <summary>
/// Raised when mouse wheel scrolls.
/// </summary>
/// <param name="windowId">
/// The window id used to initialize this instance.
/// </param>
/// <param name="offsetX">
/// The offset x used to initialize this instance.
/// </param>
/// <param name="offsetY">
/// The offset y used to initialize this instance.
/// </param>
public class MouseScrolledEvent(
    uint windowId,
    float offsetX,
    float offsetY
) : MouseEvent(windowId)
{
    /// <summary>
    /// Gets horizontal scroll offset.
    /// </summary>
    public float offsetX { get; } = offsetX;

    /// <summary>
    /// Gets vertical scroll offset.
    /// </summary>
    public float offsetY { get; } = offsetY;
}

/// <summary>
/// Base class for mouse button events.
/// </summary>
/// <param name="windowId">
/// The window id used to initialize this instance.
/// </param>
/// <param name="button">
/// The button used to initialize this instance.
/// </param>
public abstract class MouseButtonEvent(
    uint windowId,
    MouseButton button
) : MouseEvent(windowId)
{
    /// <summary>
    /// Gets the mouse button for this event.
    /// </summary>
    public MouseButton button { get; } = button;
}

/// <summary>
/// Raised when a mouse button is pressed.
/// </summary>
/// <param name="windowId">
/// The window id used to initialize this instance.
/// </param>
/// <param name="button">
/// The button used to initialize this instance.
/// </param>
public class MouseButtonPressedEvent(
    uint windowId,
    MouseButton button
) : MouseButtonEvent(windowId, button)
{
}

/// <summary>
/// Raised when a mouse button is released.
/// </summary>
/// <param name="windowId">
/// The window id used to initialize this instance.
/// </param>
/// <param name="button">
/// The button used to initialize this instance.
/// </param>
public class MouseButtonReleasedEvent(
    uint windowId,
    MouseButton button
) : MouseButtonEvent(windowId, button)
{
}
