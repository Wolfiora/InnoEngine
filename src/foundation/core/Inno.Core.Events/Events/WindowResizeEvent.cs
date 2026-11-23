namespace Inno.Core.Events;

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
