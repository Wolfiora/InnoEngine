using System;
using System.Collections.Generic;
using System.Numerics;
using Inno.Core.Events;
using Inno.Core.Input;

namespace Inno.Editor.ImGui;

/// <summary>
/// Routes platform input to the Play session only while the visible Game View owns focus.
/// </summary>
public sealed class EditorGameInputCapture
{
    private readonly Func<uint, uint?> m_resolveWindow;
    private readonly HashSet<MouseButton> m_pressedButtons = [];
    private Vector2 m_origin;
    private Vector2 m_size;
    private uint m_targetWindowID;
    private uint m_deliveredWindowID;
    private bool m_reported;
    private bool m_focused;
    private bool m_hovered;
    private bool m_pointerInside;

    /// <summary>
    /// Creates a capture policy using a presentation viewport to platform window resolver.
    /// </summary>
    /// <param name="resolveWindow">
    /// Returns a live window ID for a viewport, or null when absent.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the resolver is null.
    /// </exception>
    public EditorGameInputCapture(Func<uint, uint?> resolveWindow)
    {
        m_resolveWindow = resolveWindow ?? throw new ArgumentNullException(nameof(resolveWindow));
    }

    /// <summary>
    /// Starts a presentation pass in which the visible Game View must report its current bounds.
    /// </summary>
    public void BeginFrame() => m_reported = false;

    /// <summary>
    /// Reports the Game View image in coordinates local to its platform window.
    /// </summary>
    /// <param name="viewportId">
    /// The presentation viewport containing the Game View.
    /// </param>
    /// <param name="origin">
    /// The image's upper-left corner relative to that window.
    /// </param>
    /// <param name="size">
    /// The visible image size in logical pixels.
    /// </param>
    /// <param name="focused">
    /// Whether the Game View is selected and its platform window has focus.
    /// </param>
    /// <param name="hovered">
    /// Whether the image is inside the frontmost hovered ImGui window.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the Game View and its platform window both own focus.
    /// </returns>
    public bool Report(
        uint viewportId,
        Vector2 origin,
        Vector2 size,
        bool focused,
        bool hovered
    ) {
        m_reported = true;
        m_targetWindowID = m_resolveWindow(viewportId) ?? 0;
        m_origin = origin;
        m_size = size;
        m_focused = focused && m_targetWindowID != 0;
        m_hovered = hovered && m_focused;
        m_pointerInside = m_hovered;
        return m_focused;
    }

    /// <summary>
    /// Ends a presentation pass and identifies input that must be released after focus loss.
    /// </summary>
    /// <returns>
    /// The former platform window ID to reset, or zero if no reset is needed.
    /// </returns>
    public uint CompleteFrame()
    {
        if (!m_reported)
        {
            m_targetWindowID = 0;
            m_focused = false;
            m_hovered = false;
            m_pointerInside = false;
        }

        if (m_deliveredWindowID == 0
            || m_focused && m_deliveredWindowID == m_targetWindowID
        ) {
            return 0;
        }

        uint formerWindowID = m_deliveredWindowID;
        m_deliveredWindowID = 0;
        m_pressedButtons.Clear();
        return formerWindowID;
    }

    /// <summary>
    /// Filters an engine event through the current Game View focus and pointer bounds.
    /// </summary>
    /// <param name="evnt">
    /// The original backend-neutral platform event.
    /// </param>
    /// <returns>
    /// A routed event or a synthetic focus reset, or null when the Game View must not receive input.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the event is null.
    /// </exception>
    /// <remarks>
    /// A globally consumed key, button, focus or close release is not replayed to the game.
    /// It returns a new focus-reset event so previously held input is released before simulation.
    /// Consumed pointer movement still updates hit-test bounds without reaching the game backend.
    /// </remarks>
    public Event? Route(Event evnt)
    {
        ArgumentNullException.ThrowIfNull(evnt);
        if (evnt is MouseMovedEvent observed && observed.windowId == m_targetWindowID)
        {
            UpdatePointerBounds(observed);
        }
        if (evnt.isGlobalHandled)
        {
            if (m_deliveredWindowID != 0 && IsReleaseFromWindow(evnt, m_deliveredWindowID)
                || IsWindowExit(evnt, m_targetWindowID)
            ) {
                uint formerWindowID = m_deliveredWindowID;
                ReleaseCapture();
                return formerWindowID != 0 ? new WindowFocusChangedEvent(formerWindowID, false) : null;
            }
            return null;
        }
        if (evnt is WindowFocusChangedEvent { isFocused: false } lostFocus
            && (lostFocus.windowId == m_deliveredWindowID || lostFocus.windowId == m_targetWindowID)
        ) {
            bool needsReset = m_deliveredWindowID != 0;
            ReleaseCapture();
            return needsReset ? lostFocus : null;
        }
        if (evnt is WindowCloseEvent closed
            && (closed.windowId == m_deliveredWindowID || closed.windowId == m_targetWindowID)
        ) {
            bool needsReset = m_deliveredWindowID != 0;
            ReleaseCapture();
            return needsReset ? new WindowFocusChangedEvent(closed.windowId, false) : null;
        }
        if (!m_focused || m_targetWindowID == 0)
        {
            return null;
        }

        Event? routed = evnt switch
        {
            KeyEvent key when key.windowId == m_targetWindowID => key,
            TextInputEvent text when text.windowId == m_targetWindowID => text,
            MouseMovedEvent moved when moved.windowId == m_targetWindowID => RouteMove(moved),
            MouseButtonPressedEvent pressed when pressed.windowId == m_targetWindowID => RoutePress(pressed),
            MouseButtonReleasedEvent released when released.windowId == m_targetWindowID => RouteRelease(released),
            MouseScrolledEvent scrolled when scrolled.windowId == m_targetWindowID && m_pointerInside => scrolled,
            _ => null
        };
        if (routed is not null)
        {
            m_deliveredWindowID = m_targetWindowID;
        }
        return routed;
    }

    private void ReleaseCapture()
    {
        m_deliveredWindowID = 0;
        m_pressedButtons.Clear();
        m_focused = false;
        m_hovered = false;
        m_pointerInside = false;
    }

    private Event? RouteMove(MouseMovedEvent moved)
    {
        var local = new Vector2(moved.x - m_origin.X, moved.y - m_origin.Y);
        return m_pointerInside || m_pressedButtons.Count > 0
               ? moved.WithPosition(local.X, local.Y)
               : null;
    }

    private void UpdatePointerBounds(MouseMovedEvent moved)
    {
        var local = new Vector2(moved.x - m_origin.X, moved.y - m_origin.Y);
        m_pointerInside = m_hovered && local.X >= 0f && local.Y >= 0f
                          && local.X < m_size.X && local.Y < m_size.Y;
    }

    private Event? RoutePress(MouseButtonPressedEvent pressed)
    {
        if (!m_pointerInside)
        {
            return null;
        }
        m_pressedButtons.Add(pressed.button);
        return pressed;
    }

    private Event? RouteRelease(MouseButtonReleasedEvent released) => m_pressedButtons.Remove(released.button) ? released : null;

    private static bool IsReleaseFromWindow(
        Event evnt,
        uint windowID
    ) => evnt switch
    {
        KeyReleasedEvent key => key.windowId == windowID,
        MouseButtonReleasedEvent button => button.windowId == windowID,
        _ => IsWindowExit(evnt, windowID)
    };

    private static bool IsWindowExit(
        Event evnt,
        uint windowID
    ) => evnt switch
    {
        WindowFocusChangedEvent { isFocused: false } focus => focus.windowId == windowID,
        WindowCloseEvent close => close.windowId == windowID,
        _ => false
    };
}
