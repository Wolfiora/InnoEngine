using System;
using System.Collections.Generic;

using Inno.Core.Input;
using Inno.UI;

namespace Inno.Adapter.UI.RmlUi;

/// <summary>
/// Adapts routed controller input to the document's native focus and control policies.
/// </summary>
public sealed unsafe partial class RmlUiBackend
{
    private const double C_NAVIGATION_DELAY = 0.4;
    private const double C_NAVIGATION_INTERVAL = 0.08;
    private const float C_STICK_ACTIVATION = 0.55f;
    private const float C_STICK_RELEASE = 0.35f;
    private readonly Dictionary<UiContextHandle, GamepadNavigation> m_gamepadNavigation = [];

    private void ProcessGamepad(
        UiContextHandle context,
        GamepadInputChange change
    ) {
        if (!m_gamepadNavigation.TryGetValue(context, out GamepadNavigation? navigation))
            throw new InvalidOperationException("The UI context is not owned by this active backend.");
        if (change is GamepadConnectionChange connection)
        {
            if (!connection.connected && navigation.deviceId == connection.deviceId)
                navigation.Reset();
            return;
        }
        if (change is GamepadButtonChange button)
        {
            KeyCode key = button.button switch
            {
                GamepadButton.DpadLeft => KeyCode.LeftArrow,
                GamepadButton.DpadRight => KeyCode.RightArrow,
                GamepadButton.DpadUp => KeyCode.UpArrow,
                GamepadButton.DpadDown => KeyCode.DownArrow,
                GamepadButton.South => KeyCode.Enter,
                GamepadButton.East => KeyCode.Escape,
                _ => KeyCode.Unknown
            };
            if (key == KeyCode.Unknown)
                return;
            if (!button.pressed)
            {
                if (navigation.deviceId == button.deviceId && navigation.button == button.button)
                    navigation.Reset();
                return;
            }
            if (navigation.deviceId == button.deviceId && navigation.button == button.button)
                return;
            navigation.Reset();
            SendNavigation(context, key);
            if (button.button is GamepadButton.South or GamepadButton.East)
                return;
            navigation.button = button.button;
            navigation.Start(button.deviceId, key, GetTime(context).seconds);
            return;
        }
        if (change is not GamepadAxisChange axis || axis.axis is not (GamepadAxis.LeftX or GamepadAxis.LeftY))
            return;
        bool sameAxis = navigation.deviceId == axis.deviceId && navigation.axis == axis.axis;
        if (MathF.Abs(axis.value) <= C_STICK_RELEASE)
        {
            if (sameAxis)
                navigation.Reset();
            return;
        }
        if (MathF.Abs(axis.value) < C_STICK_ACTIVATION)
            return;
        KeyCode direction = axis.axis == GamepadAxis.LeftX
            ? (axis.value < 0 ? KeyCode.LeftArrow : KeyCode.RightArrow)
            : (axis.value < 0 ? KeyCode.UpArrow : KeyCode.DownArrow);
        if (sameAxis && navigation.key == direction)
            return;
        navigation.Reset();
        SendNavigation(context, direction);
        navigation.axis = axis.axis;
        navigation.Start(axis.deviceId, direction, GetTime(context).seconds);
    }

    private void RepeatGamepadNavigation(
        UiContextHandle context,
        UiInputSnapshot input
    ) {
        GamepadNavigation navigation = m_gamepadNavigation[context];
        if (navigation.deviceId == 0)
            return;
        if (!input.gamepads.TryGetValue(navigation.deviceId, out GamepadState? state)
            || !navigation.IsHeld(state))
        {
            navigation.Reset();
            return;
        }
        double seconds = GetTime(context).seconds;
        if (seconds < navigation.nextRepeat)
            return;
        SendNavigation(context, navigation.key);
        // Long frames never replay an unbounded backlog of navigation actions.
        navigation.nextRepeat = seconds + C_NAVIGATION_INTERVAL;
    }

    private void ResetGamepadNavigation(UiContextHandle context)
        => m_gamepadNavigation[context].Reset();

    private void SendNavigation(
        UiContextHandle context,
        KeyCode key
    ) {
        // This is a terminal backend adaptation, never a synthetic Core input event.
        ulong handle = Require(context);
        ThrowIfFailed(RmlUiNativeRuntime.ProcessKey(m_runtime, handle, (int)key, 1, 0), "navigate UI control");
        ThrowIfFailed(RmlUiNativeRuntime.ProcessKey(m_runtime, handle, (int)key, 0, 0), "release UI navigation");
    }

    private sealed class GamepadNavigation
    {
        internal ulong deviceId;
        internal KeyCode key;
        internal GamepadButton? button;
        internal GamepadAxis? axis;
        internal double nextRepeat;

        internal void Start(
            ulong device,
            KeyCode direction,
            double seconds
        ) {
            deviceId = device;
            key = direction;
            nextRepeat = seconds + C_NAVIGATION_DELAY;
        }

        internal bool IsHeld(GamepadState state)
        {
            if (button is GamepadButton heldButton)
                return state.buttonsDown.Contains(heldButton);
            if (axis is not GamepadAxis heldAxis)
                return false;
            float value = state.GetAxis(heldAxis);
            return key is KeyCode.LeftArrow or KeyCode.UpArrow
                ? value < -C_STICK_RELEASE
                : value > C_STICK_RELEASE;
        }

        internal void Reset()
        {
            deviceId = 0;
            key = KeyCode.Unknown;
            button = null;
            axis = null;
            nextRepeat = 0;
        }
    }
}
