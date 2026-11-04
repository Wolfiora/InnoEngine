using System.Collections.Generic;
using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Native.Sdl3;
using Inno.Platform;
using Xunit;

namespace Inno.Adapter.Platform.Sdl3.Tests;

public sealed unsafe class GamepadTests
{
    [Fact]
    public void VirtualDeviceUsesOwnedNativeConnectionAndFocusedWindowRouting()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(GamepadTests) + "." + nameof(VirtualDeviceUsesOwnedNativeConnectionAndFocusedWindowRouting)))
            return;
        using var app = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = app.CreateWindow(new PlatformWindowOptions
        { title = "Controller input contract", width = 40, height = 40, visible = false });
        var descriptor = new SDLVirtualJoystickDesc
        {
            Version = (uint)sizeof(SDLVirtualJoystickDesc),
            Type = (ushort)SDLJoystickType.SDL_JOYSTICK_TYPE_GAMEPAD,
            Naxes = 6,
            Nbuttons = 15,
            AxisMask = 0x3f,
            ButtonMask = 0x7fff
        };
        uint id = SDL.Raw.AttachVirtualJoystick(&descriptor);
        Assert.NotEqual(0u, id);
        bool detached = false;
        try
        {
            List<Event> connected = Drain(app);
            Assert.Contains(connected, evnt => evnt is GamepadEvent
            { change: GamepadConnectionChange { connected: true } change } && change.deviceId == id);

            PushFocus(window.windowId, true);
            var button = new SDLEvent
            {
                Gbutton = new SDLGamepadButtonEvent
                {
                    Type = SDLEventType.GamepadButtonDown,
                    Which = id,
                    Button = (byte)SDLGamepadButton.SDL_GAMEPAD_BUTTON_SOUTH,
                    Down = 1
                }
            };
            Assert.NotEqual(0, SDL.Raw.PushEvent(&button));
            var axis = new SDLEvent
            {
                Gaxis = new SDLGamepadAxisEvent
                {
                    Type = SDLEventType.GamepadAxisMotion,
                    Which = id,
                    Axis = (byte)SDLGamepadAxis.SDL_GAMEPAD_AXIS_LEFTX,
                    Value = short.MinValue
                }
            };
            Assert.NotEqual(0, SDL.Raw.PushEvent(&axis));
            List<Event> controls = Drain(app);
            Assert.Contains(controls, evnt => evnt is GamepadEvent
            { change: GamepadButtonChange { button: GamepadButton.South, pressed: true } } gamepad
                && gamepad.windowId == window.windowId);
            Assert.Contains(controls, evnt => evnt is GamepadEvent
            { change: GamepadAxisChange { axis: GamepadAxis.LeftX, value: -1f } });

            PushFocus(window.windowId, false);
            Assert.NotEqual(0, SDL.Raw.PushEvent(&button));
            Assert.DoesNotContain(Drain(app), evnt => evnt is GamepadEvent { change: GamepadButtonChange });
            Assert.True(SDL.DetachVirtualJoystick(id));
            detached = true;
            Assert.Contains(Drain(app), evnt => evnt is GamepadEvent
            { change: GamepadConnectionChange { connected: false } change } && change.deviceId == id);
        }
        finally
        {
            if (!detached)
                Assert.True(SDL.DetachVirtualJoystick(id));
        }
    }

    private static List<Event> Drain(Sdl3PlatformApplication app)
    {
        List<Event> events = [];
        while (app.PollEvent(out Event? evnt))
            if (evnt is not null)
                events.Add(evnt);
        return events;
    }

    private static void PushFocus(
        uint windowId,
        bool focused
    ) {
        var evnt = new SDLEvent
        {
            Window = new SDLWindowEvent
            {
                Type = focused ? SDLEventType.WindowFocusGained : SDLEventType.WindowFocusLost,
                WindowID = windowId
            }
        };
        Assert.NotEqual(0, SDL.Raw.PushEvent(&evnt));
    }
}
