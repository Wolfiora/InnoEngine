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
