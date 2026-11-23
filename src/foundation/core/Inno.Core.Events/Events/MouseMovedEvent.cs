namespace Inno.Core.Events;

/// <summary>
/// Raised when the cursor moves.
/// </summary>
public sealed class MouseMovedEvent : MouseEvent
{
    /// <summary>
    /// Creates a pointer movement in platform-window coordinates.
    /// </summary>
    /// <param name="windowID">
    /// The platform window that produced the movement.
    /// </param>
    /// <param name="x">
    /// The horizontal pointer coordinate in logical pixels.
    /// </param>
    /// <param name="y">
    /// The vertical pointer coordinate in logical pixels.
    /// </param>
    public MouseMovedEvent(
        uint windowID,
        float x,
        float y
    ) : base(windowID)
    {
        this.x = x;
        this.y = y;
    }

    private MouseMovedEvent(
        MouseMovedEvent source,
        float x,
        float y
    ) : base(source)
    {
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
}
