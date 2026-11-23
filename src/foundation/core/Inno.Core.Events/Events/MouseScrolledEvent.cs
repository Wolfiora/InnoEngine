namespace Inno.Core.Events;

/// <summary>
/// Raised when mouse wheel scrolls.
/// </summary>
/// <param name="windowID">
/// The window id used to initialize this instance.
/// </param>
/// <param name="offsetX">
/// The offset x used to initialize this instance.
/// </param>
/// <param name="offsetY">
/// The offset y used to initialize this instance.
/// </param>
public sealed class MouseScrolledEvent(
    uint windowID,
    float offsetX,
    float offsetY
) : MouseEvent(windowID)
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
