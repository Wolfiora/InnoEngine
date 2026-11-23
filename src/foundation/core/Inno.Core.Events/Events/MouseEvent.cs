namespace Inno.Core.Events;

/// <summary>
/// Base class for mouse events.
/// </summary>
public abstract class MouseEvent : Event
{
    /// <summary>
    /// Creates an independent mouse event belonging to one platform window.
    /// </summary>
    /// <param name="windowID">
    /// The platform window that produced the event.
    /// </param>
    protected MouseEvent(uint windowID)
    {
        this.windowID = windowID;
    }

    /// <summary>
    /// Creates a routed mouse event sharing its source's window and global consumption.
    /// </summary>
    /// <param name="source">
    /// The mouse event whose input lifetime remains authoritative.
    /// </param>
    /// <exception cref="System.ArgumentNullException">
    /// The source event is null.
    /// </exception>
    protected MouseEvent(MouseEvent source)
        : base(source)
        {
        this.windowID = source.windowID;
    }

    /// <summary>
    /// Gets the source window id for this event.
    /// </summary>
    public uint windowID { get; }
}
