namespace Inno.Core.Events;

/// <summary>
/// Raised when the window requests close.
/// </summary>
/// <param name="windowID">
/// The window id used to initialize this instance.
/// </param>
public sealed class WindowCloseEvent(uint windowID) : WindowEvent(windowID);
