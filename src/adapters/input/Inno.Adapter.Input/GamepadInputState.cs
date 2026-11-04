using System;
using System.Collections.Generic;
using System.Collections.Frozen;
using Inno.Core.Events;
using Inno.Core.Input;

namespace Inno.Adapter.Input;

internal sealed class GamepadInputState
{
    private readonly Dictionary<ulong, Controls> m_devices = [];
    private FrozenDictionary<ulong, GamepadState>? m_snapshot;
    private bool m_focused = true;
    private bool m_visible = true;
    private bool m_suspended;
    private bool m_closed;

    internal IReadOnlyDictionary<ulong, GamepadState> Capture()
        => m_snapshot ??= m_devices.ToFrozenDictionary(pair => pair.Key, pair => pair.Value.Capture(pair.Key));

    internal void Initialize(GamepadInputState source)
    {
        m_focused = source.m_focused;
        m_visible = source.m_visible;
        m_suspended = source.m_suspended;
        m_closed = source.m_closed;
        foreach ((ulong id, GamepadState state) in source.Capture())
            m_devices.Add(id, new Controls(state));
        m_snapshot = null;
    }

    internal void Process(
        Event evnt,
        uint windowId,
        List<InputChange>? changes
    ) {
        if (evnt is GamepadEvent { change: GamepadConnectionChange connection })
        {
            if (evnt is GamepadEvent { windowId: not 0 } routed && windowId != 0 && routed.windowId != windowId)
                return;
            bool changed = connection.connected
                ? m_devices.TryAdd(connection.deviceId, new Controls())
                : m_devices.Remove(connection.deviceId);
            if (changed)
            {
                m_snapshot = null;
                changes?.Add(connection);
            }
            return;
        }
        if (evnt is WindowEvent window && windowId != 0 && window.windowId != windowId)
            return;
        switch (evnt)
        {
            case WindowFocusChangedEvent focus:
                m_focused = focus.isFocused;
                if (windowId == 0 && focus.isFocused)
                    m_visible = true;
                break;
            case WindowVisibilityChangedEvent visibility: m_visible = visibility.isVisible; break;
            case ApplicationSuspensionChangedEvent suspension: m_suspended = suspension.isSuspended; break;
            case WindowCloseEvent:
                if (windowId == 0)
                    m_focused = false;
                else
                    m_closed = true;
                break;
        }
        if (!m_focused || !m_visible || m_suspended || m_closed)
        {
            foreach ((ulong id, Controls controls) in m_devices)
            {
                foreach (GamepadButton button in controls.buttons)
                    changes?.Add(new GamepadButtonChange(id, button, false));
                foreach ((GamepadAxis axis, float value) in controls.axes)
                    if (value != 0f)
                        changes?.Add(new GamepadAxisChange(id, axis, 0f));
                if (controls.buttons.Count != 0 || controls.axes.Count != 0)
                {
                    controls.Clear();
                    m_snapshot = null;
                }
            }
            return;
        }
        if (evnt is not GamepadEvent gamepad || evnt.isGlobalHandled
            || (windowId != 0 && gamepad.windowId != windowId)
            || !m_devices.TryGetValue(gamepad.change.deviceId, out Controls? target))
            return;
        bool updated = gamepad.change switch
        {
            GamepadButtonChange button => button.pressed ? target.buttons.Add(button.button) : target.buttons.Remove(button.button),
            GamepadAxisChange axis => target.SetAxis(axis.axis, axis.value),
            _ => false
        };
        if (!updated)
            return;
        target.Invalidate();
        m_snapshot = null;
        changes?.Add(gamepad.change);
    }

    internal void Clear()
    {
        m_devices.Clear();
        m_snapshot = null;
    }

    private sealed class Controls
    {
        internal readonly HashSet<GamepadButton> buttons = [];
        internal readonly Dictionary<GamepadAxis, float> axes = [];
        private GamepadState? m_snapshot;

        internal Controls() { }

        internal Controls(GamepadState state)
        {
            buttons.UnionWith(state.buttonsDown);
            foreach ((GamepadAxis axis, float value) in state.axes)
                axes.Add(axis, value);
            m_snapshot = state;
        }

        internal GamepadState Capture(ulong id) => m_snapshot ??= new GamepadState(id, buttons, axes);

        internal bool SetAxis(
            GamepadAxis axis,
            float value
        ) {
            if (axes.GetValueOrDefault(axis) == value)
                return false;
            if (value == 0f)
                axes.Remove(axis);
            else
                axes[axis] = value;
            return true;
        }

        internal void Invalidate() => m_snapshot = null;

        internal void Clear()
        {
            buttons.Clear();
            axes.Clear();
            Invalidate();
        }
    }
}
