using System;
using Inno.Core.Input;

namespace Inno.Core.Events;

/// <summary>
/// Carries a positioned mouse fact and its shared global consumption lifetime.
/// </summary>
public abstract class MouseEvent : Event
{
    /// <summary>
    /// Creates one positioned fact in logical platform-window coordinates.
    /// </summary>
    /// <param name="windowId">
    /// Platform window that produced this event.
    /// </param>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A pointer coordinate is not finite.
    /// </exception>
    protected MouseEvent(
        uint windowId,
        float x,
        float y
    ) {
        ValidatePosition(x, y);
        this.windowId = windowId;
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Creates a coordinate projection sharing the source consumption lifetime.
    /// </summary>
    /// <param name="source">
    /// Source whose global consumption lifetime remains authoritative.
    /// </param>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A projected coordinate is not finite.
    /// </exception>
    protected MouseEvent(
        MouseEvent source,
        float x,
        float y
    ) : base(source) {
        ValidatePosition(x, y);
        windowId = source.windowId;
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Gets the platform window that owns this input fact.
    /// </summary>
    public uint windowId { get; }

    /// <summary>
    /// Gets the horizontal pointer coordinate in this event projection.
    /// </summary>
    public float x { get; }

    /// <summary>
    /// Gets the vertical pointer coordinate in this event projection.
    /// </summary>
    public float y { get; }

    private static void ValidatePosition(
        float x,
        float y
    ) {
        if (!float.IsFinite(x))
            throw new ArgumentOutOfRangeException(nameof(x));
        if (!float.IsFinite(y))
            throw new ArgumentOutOfRangeException(nameof(y));
    }
}

/// <summary>
/// Reports pointer motion at its event-local position.
/// </summary>
public class MouseMovedEvent : MouseEvent
{
    /// <summary>
    /// Creates a positioned moved input fact.
    /// </summary>
    /// <param name="windowId">
    /// Platform window that produced this event.
    /// </param>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A pointer coordinate is not finite.
    /// </exception>
    public MouseMovedEvent(
        uint windowId,
        float x,
        float y
    ) : base(windowId, x, y) {
    }

    /// <summary>
    /// Projects this fact without creating another global consumption lifetime.
    /// </summary>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <returns>
    /// A positioned event sharing consumption with the original fact.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A projected coordinate is not finite.
    /// </exception>
    public MouseMovedEvent WithPosition(
        float x,
        float y
    ) => new(this, x, y);

    private MouseMovedEvent(
        MouseMovedEvent source,
        float x,
        float y
    ) : base(source, x, y) {
    }
}

/// <summary>
/// Reports wheel movement at its event-local pointer position.
/// </summary>
public class MouseScrolledEvent : MouseEvent
{
    /// <summary>
    /// Creates a positioned scrolled input fact.
    /// </summary>
    /// <param name="windowId">
    /// Platform window that produced this event.
    /// </param>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="offsetX">
    /// Horizontal scroll delta, positive to the right.
    /// </param>
    /// <param name="offsetY">
    /// Vertical scroll delta, positive upwards.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A pointer coordinate is not finite.
    /// </exception>
    public MouseScrolledEvent(
        uint windowId,
        float x,
        float y,
        float offsetX,
        float offsetY
    ) : base(windowId, x, y) {
        this.offsetX = offsetX;
        this.offsetY = offsetY;
    }

    /// <summary>
    /// Gets horizontal wheel delta, positive to the right.
    /// </summary>
    public float offsetX { get; }

    /// <summary>
    /// Gets vertical wheel delta, positive upwards.
    /// </summary>
    public float offsetY { get; }

    /// <summary>
    /// Projects this fact without creating another global consumption lifetime.
    /// </summary>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <returns>
    /// A positioned event sharing consumption with the original fact.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A projected coordinate is not finite.
    /// </exception>
    public MouseScrolledEvent WithPosition(
        float x,
        float y
    ) => new(this, x, y);

    private MouseScrolledEvent(
        MouseScrolledEvent source,
        float x,
        float y
    ) : base(source, x, y) {
        offsetX = source.offsetX;
        offsetY = source.offsetY;
    }
}

/// <summary>
/// Reports a button press at its event-local pointer position.
/// </summary>
public class MouseButtonPressedEvent : MouseButtonEvent
{
    /// <summary>
    /// Creates a positioned buttonpressed input fact.
    /// </summary>
    /// <param name="windowId">
    /// Platform window that produced this event.
    /// </param>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="button">
    /// Physical button that changed.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A pointer coordinate is not finite.
    /// </exception>
    public MouseButtonPressedEvent(
        uint windowId,
        float x,
        float y,
        MouseButton button
    ) : base(windowId, x, y, button) {
    }

    /// <summary>
    /// Projects this fact without creating another global consumption lifetime.
    /// </summary>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <returns>
    /// A positioned event sharing consumption with the original fact.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A projected coordinate is not finite.
    /// </exception>
    public MouseButtonPressedEvent WithPosition(
        float x,
        float y
    ) => new(this, x, y);

    private MouseButtonPressedEvent(
        MouseButtonPressedEvent source,
        float x,
        float y
    ) : base(source, x, y) {
    }
}

/// <summary>
/// Reports a button release at its event-local pointer position.
/// </summary>
public class MouseButtonReleasedEvent : MouseButtonEvent
{
    /// <summary>
    /// Creates a positioned buttonreleased input fact.
    /// </summary>
    /// <param name="windowId">
    /// Platform window that produced this event.
    /// </param>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="button">
    /// Physical button that changed.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A pointer coordinate is not finite.
    /// </exception>
    public MouseButtonReleasedEvent(
        uint windowId,
        float x,
        float y,
        MouseButton button
    ) : base(windowId, x, y, button) {
    }

    /// <summary>
    /// Projects this fact without creating another global consumption lifetime.
    /// </summary>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <returns>
    /// A positioned event sharing consumption with the original fact.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A projected coordinate is not finite.
    /// </exception>
    public MouseButtonReleasedEvent WithPosition(
        float x,
        float y
    ) => new(this, x, y);

    private MouseButtonReleasedEvent(
        MouseButtonReleasedEvent source,
        float x,
        float y
    ) : base(source, x, y) {
    }
}

/// <summary>
/// Carries a positioned physical button transition.
/// </summary>
public abstract class MouseButtonEvent : MouseEvent
{
    /// <summary>
    /// Creates one physical button transition at its event-local position.
    /// </summary>
    /// <param name="windowId">
    /// Platform window that produced this event.
    /// </param>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="button">
    /// Physical button that changed.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A pointer coordinate is not finite.
    /// </exception>
    protected MouseButtonEvent(
        uint windowId,
        float x,
        float y,
        MouseButton button
    ) : base(windowId, x, y) {
        this.button = button;
    }

    /// <summary>
    /// Projects an existing button transition without replacing consumption ownership.
    /// </summary>
    /// <param name="source">
    /// Source whose global consumption lifetime remains authoritative.
    /// </param>
    /// <param name="x">
    /// Horizontal pointer coordinate in logical window pixels.
    /// </param>
    /// <param name="y">
    /// Vertical pointer coordinate in logical window pixels.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A projected coordinate is not finite.
    /// </exception>
    protected MouseButtonEvent(
        MouseButtonEvent source,
        float x,
        float y
    ) : base(source, x, y) {
        button = source.button;
    }

    /// <summary>
    /// Gets the physical button that changed.
    /// </summary>
    public MouseButton button { get; }
}
