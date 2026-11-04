using System;
using System.Collections.Generic;
using System.Threading;

using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Input;

namespace Inno.Adapter.Input;

/// <summary>
/// Accumulates backend-neutral platform events into complete runtime input snapshots.
/// </summary>
public sealed class EventInputBackend : IInputBackend
{
    private readonly HashSet<KeyCode> m_keysDown = [];
    private readonly HashSet<MouseButton> m_mouseButtonsDown = [];
    private readonly List<InputChange> m_changes = [];
    private readonly Lock m_sync = new();
    private readonly uint m_windowId;
    private Action<EventInputBackend>? m_disposeCallback;
    private IDisposable? m_subscription;
    private Vector2 m_mousePosition;
    private KeyModifier m_modifiers;
    private bool m_disposed;
    private bool m_suspended;

    /// <summary>
    /// Creates an event-backed input adapter for one platform window.
    /// </summary>
    /// <param name="windowId">
    /// The translated event window identifier, or zero to accept input from every window.
    /// </param>
    public EventInputBackend(uint windowId)
        : this(windowId, null)
    {
    }

    internal EventInputBackend(
        uint windowId,
        Action<EventInputBackend>? disposeCallback
    ) {
        m_windowId = windowId;
        m_disposeCallback = disposeCallback;
    }

    /// <summary>
    /// Applies one backend-neutral event translated by the owning platform application.
    /// </summary>
    /// <param name="evnt">
    /// The event observed before runtime dispatch.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="evnt"/> is null.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this backend is disposed.
    /// </exception>
    public void ProcessEvent(Event evnt)
    {
        ArgumentNullException.ThrowIfNull(evnt);
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (evnt.isGlobalHandled || !Accepts(evnt))
            return;
        lock (m_sync)
        {
            ObjectDisposedException.ThrowIf(m_disposed, this);
            if (evnt is ApplicationSuspensionChangedEvent suspension)
                m_suspended = suspension.isSuspended;
            if (m_suspended && evnt is KeyEvent or MouseEvent or TextInputEvent)
                return;
            switch (evnt)
            {
                case KeyPressedEvent key:
                    m_modifiers = key.modifiers;
                    bool newlyPressed = m_keysDown.Add(key.key);
                    if (key.repeat || newlyPressed)
                        m_changes.Add(new KeyInputChange(key.key, true, key.repeat, key.modifiers));
                    break;
                case KeyReleasedEvent key:
                    m_modifiers = key.modifiers;
                    if (m_keysDown.Remove(key.key))
                        m_changes.Add(new KeyInputChange(key.key, false, false, key.modifiers));
                    break;
                case TextInputEvent text:
                    m_changes.Add(new TextInputChange(text.text, m_modifiers));
                    break;
                case MouseButtonPressedEvent button:
                    m_mousePosition = new Vector2(button.x, button.y);
                    if (m_mouseButtonsDown.Add(button.button))
                        m_changes.Add(new PointerButtonInputChange(m_mousePosition, button.button, true, m_modifiers));
                    break;
                case MouseButtonReleasedEvent button:
                    m_mousePosition = new Vector2(button.x, button.y);
                    if (m_mouseButtonsDown.Remove(button.button))
                        m_changes.Add(new PointerButtonInputChange(m_mousePosition, button.button, false, m_modifiers));
                    break;
                case MouseMovedEvent movement:
                    var nextPosition = new Vector2(movement.x, movement.y);
                    m_changes.Add(new PointerMoveInputChange(nextPosition, m_mousePosition, m_modifiers));
                    m_mousePosition = nextPosition;
                    break;
                case MouseScrolledEvent scroll:
                    m_mousePosition = new Vector2(scroll.x, scroll.y);
                    m_changes.Add(new PointerScrollInputChange(m_mousePosition, new Vector2(scroll.offsetX, scroll.offsetY), m_modifiers));
                    break;
                case WindowCloseEvent:
                case WindowFocusChangedEvent { isFocused: false }:
                case WindowVisibilityChangedEvent { isVisible: false }:
                case ApplicationSuspensionChangedEvent { isSuspended: true }:
                    m_changes.Clear();
                    foreach (KeyCode key in m_keysDown)
                        m_changes.Add(new KeyInputChange(key, false, false, KeyModifier.None));
                    foreach (MouseButton button in m_mouseButtonsDown)
                        m_changes.Add(new PointerButtonInputChange(m_mousePosition, button, false, KeyModifier.None));
                    m_keysDown.Clear();
                    m_mouseButtonsDown.Clear();
                    m_modifiers = KeyModifier.None;
                    break;
            }
        }
    }

    /// <summary>
    /// Captures an immutable snapshot of the current observable state.
    /// </summary>
    /// <param name="frameIndex">
    /// The monotonic frame identity associated with this operation.
    /// </param>
    /// <returns>
    /// The validated input snapshot that represents the completed operation.
    /// </returns>
    public InputSnapshot Capture(long frameIndex)
    {
        lock (m_sync)
        {
            ObjectDisposedException.ThrowIf(m_disposed, this);
            var snapshot = new InputSnapshot(
                frameIndex,
                m_changes.Count == 0 ? InputSequence.empty : new InputSequence(m_changes),
                m_keysDown,
                m_mouseButtonsDown,
                m_mousePosition,
                m_modifiers);
            m_changes.Clear();
            return snapshot;
        }
    }

    /// <summary>
    /// Releases the resources owned by this implementation.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
            return;
        lock (m_sync)
        {
            m_disposed = true;
            m_subscription?.Dispose();
            m_subscription = null;
            Action<EventInputBackend>? callback = m_disposeCallback;
            m_disposeCallback = null;
            callback?.Invoke(this);
            m_keysDown.Clear();
            m_mouseButtonsDown.Clear();
            m_changes.Clear();
        }
    }

    internal void ConnectSource(IDisposable subscription) => m_subscription = subscription;

    internal void DisconnectSource()
    {
        m_subscription?.Dispose();
        m_subscription = null;
        m_disposeCallback = null;
    }

    private bool Accepts(Event evnt)
        => m_windowId == 0 || evnt switch
        {
            KeyEvent key => key.windowId == m_windowId,
            TextInputEvent text => text.windowId == m_windowId,
            MouseEvent mouse => mouse.windowId == m_windowId,
            WindowEvent window => window.windowId == m_windowId,
            ApplicationSuspensionChangedEvent => true,
            _ => false
        };
}
