using System;
using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Native.Sdl3;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Owns native controller connections and translates their ordered facts into neutral input events.
/// </summary>
public sealed unsafe partial class Sdl3PlatformApplication
{
    private void InitializeGamepads()
    {
        int count = 0;
        uint* devices = SDL.Raw.GetGamepads(&count);
        if (devices == null)
            throw new InvalidOperationException(Sdl3NativeText.ReadError() ?? "SDL_GetGamepads failed.");
        try
        {
            for (int index = 0; index < count; index++)
            {
                var added = new SDLEvent
                {
                    Gdevice = new SDLGamepadDeviceEvent { Type = SDLEventType.GamepadAdded, Which = devices[index] }
                };
                if (TryTranslateGamepad(ref added, out Event? connected) && connected is not null)
                    m_pendingEvents.Add(connected);
            }
        }
        finally
        {
            SDL.Raw.Free(devices);
        }
    }

    private void ReleaseGamepads()
    {
        foreach (nint handle in m_gamepads.Values)
            SDL.Raw.CloseGamepad((SDLGamepad*)handle);
        m_gamepads.Clear();
    }

    private bool TryTranslateGamepad(
        ref SDLEvent native,
        out Event? evnt
    ) {
        evnt = null;
        var type = (SDLEventType)native.Type;
        if (type == SDLEventType.GamepadAdded)
        {
            uint id = native.Gdevice.Which;
            if (m_gamepads.ContainsKey(id) || !SDL.IsGamepad(id))
                return false;
            SDLGamepad* handle = SDL.Raw.OpenGamepad(id);
            if (handle == null)
                throw new InvalidOperationException(Sdl3NativeText.ReadError() ?? "SDL_OpenGamepad failed.");
            m_gamepads.Add(id, (nint)handle);
            evnt = new GamepadEvent(0, new GamepadConnectionChange(id, true));
            return true;
        }
        if (type == SDLEventType.GamepadRemoved)
        {
            uint id = native.Gdevice.Which;
            if (!m_gamepads.Remove(id, out nint handle))
                return false;
            SDL.Raw.CloseGamepad((SDLGamepad*)handle);
            evnt = new GamepadEvent(0, new GamepadConnectionChange(id, false));
            return true;
        }

        uint windowId = 0;
        foreach (Sdl3PlatformWindow window in m_windows.Values)
            if (window.isFocused && !window.isClosed)
            {
                windowId = window.windowId;
                break;
            }
        if (windowId == 0)
            return false;
        if (type is SDLEventType.GamepadButtonDown or SDLEventType.GamepadButtonUp)
        {
            if (!m_gamepads.ContainsKey(native.Gbutton.Which)
                || !TryTranslateGamepadButton((SDLGamepadButton)native.Gbutton.Button, out GamepadButton button))
                return false;
            evnt = new GamepadEvent(windowId, new GamepadButtonChange(native.Gbutton.Which, button, native.Gbutton.Down != 0));
            return true;
        }
        if (!m_gamepads.ContainsKey(native.Gaxis.Which)
            || !TryTranslateGamepadAxis((SDLGamepadAxis)native.Gaxis.Axis, out GamepadAxis axis))
            return false;
        short sample = native.Gaxis.Value;
        float normalized = axis is GamepadAxis.LeftTrigger or GamepadAxis.RightTrigger
            ? Math.Clamp(sample / (float)short.MaxValue, 0f, 1f)
            : sample / (sample < 0 ? -(float)short.MinValue : short.MaxValue);
        evnt = new GamepadEvent(windowId, new GamepadAxisChange(native.Gaxis.Which, axis, normalized));
        return true;
    }

    private static bool TryTranslateGamepadButton(
        SDLGamepadButton native,
        out GamepadButton button
    ) {
        GamepadButton? mapped = native switch
        {
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_SOUTH => GamepadButton.South,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_EAST => GamepadButton.East,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_WEST => GamepadButton.West,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_NORTH => GamepadButton.North,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_BACK => GamepadButton.Back,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_GUIDE => GamepadButton.Guide,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_START => GamepadButton.Start,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_LEFT_STICK => GamepadButton.LeftStick,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_RIGHT_STICK => GamepadButton.RightStick,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_LEFT_SHOULDER => GamepadButton.LeftShoulder,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_RIGHT_SHOULDER => GamepadButton.RightShoulder,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_DPAD_UP => GamepadButton.DpadUp,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_DPAD_DOWN => GamepadButton.DpadDown,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_DPAD_LEFT => GamepadButton.DpadLeft,
            SDLGamepadButton.SDL_GAMEPAD_BUTTON_DPAD_RIGHT => GamepadButton.DpadRight,
            _ => null
        };
        button = mapped.GetValueOrDefault();
        return mapped.HasValue;
    }

    private static bool TryTranslateGamepadAxis(
        SDLGamepadAxis native,
        out GamepadAxis axis
    ) {
        GamepadAxis? mapped = native switch
        {
            SDLGamepadAxis.SDL_GAMEPAD_AXIS_LEFTX => GamepadAxis.LeftX,
            SDLGamepadAxis.SDL_GAMEPAD_AXIS_LEFTY => GamepadAxis.LeftY,
            SDLGamepadAxis.SDL_GAMEPAD_AXIS_RIGHTX => GamepadAxis.RightX,
            SDLGamepadAxis.SDL_GAMEPAD_AXIS_RIGHTY => GamepadAxis.RightY,
            SDLGamepadAxis.SDL_GAMEPAD_AXIS_LEFT_TRIGGER => GamepadAxis.LeftTrigger,
            SDLGamepadAxis.SDL_GAMEPAD_AXIS_RIGHT_TRIGGER => GamepadAxis.RightTrigger,
            _ => null
        };
        axis = mapped.GetValueOrDefault();
        return mapped.HasValue;
    }
}
