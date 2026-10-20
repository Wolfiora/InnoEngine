using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Native.Sdl3;
using Inno.Platform;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Platform runtime entry point responsible for window creation and platform event polling.
/// </summary>
public sealed unsafe partial class Sdl3PlatformApplication
{
    private readonly ISdl3HostIntegration m_hostIntegration;
    private readonly int m_ownerThreadId = Environment.CurrentManagedThreadId;
    private readonly List<Event> m_pendingEvents = [];
    private readonly Dictionary<uint, Sdl3PlatformWindow> m_windows = [];
    private readonly List<ISdl3ApplicationExtension> m_extensions = [];
    private SDLEventFilter? m_liveResizeEventWatch;
    private SDLMainThreadCallback? m_liveResizeMainThreadCallback;
    private unsafe delegate* unmanaged[Cdecl]<void*, SDLEvent*, byte> m_liveResizeEventWatchPointer;
    private unsafe delegate* unmanaged[Cdecl]<void*, void> m_liveResizeMainThreadCallbackPointer;
    private GCHandle m_liveResizeEventWatchHandle;
    private int m_liveResizeRedrawQueued;
    private uint m_liveResizeWindowId;
    private ExceptionDispatchInfo? m_redrawFailure;
    private bool m_dispatchingRedraw;
    private int m_pendingEventReadIndex;
    private bool m_disposed;

    private unsafe void Initialize()
    {
        // Deliver the activating mouse press to the application as well as focusing the window.
        // This is required for first-click widgets, double-clicks, and drags across editor viewports.
        _ = SDL.SetHint(SDL.Constants.SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH, "1");
        _ = SDL.SetHint(SDL.Constants.SDL_HINT_MOUSE_AUTO_CAPTURE, "0");

        m_hostIntegration.ConfigureInitialization();

        if (!SDL.Init(SDLInitFlags.Video | SDLInitFlags.Events))
        {
            throw new InvalidOperationException(Sdl3NativeText.ReadError() ?? "SDL_Init failed.");
        }

        // Browser frames are already driven by requestAnimationFrame; SDL's native
        // resize callback is only needed by desktop window move/resize loops.
        if (!m_hostIntegration.capabilities.liveResize)
            return;

        m_liveResizeEventWatch = LiveResizeEventWatch;
        m_liveResizeMainThreadCallback = LiveResizeMainThreadCallback;
        m_liveResizeEventWatchPointer = (delegate* unmanaged[Cdecl]<void*, SDLEvent*, byte>)
            Marshal.GetFunctionPointerForDelegate(m_liveResizeEventWatch);
        m_liveResizeMainThreadCallbackPointer = (delegate* unmanaged[Cdecl]<void*, void>)
            Marshal.GetFunctionPointerForDelegate(m_liveResizeMainThreadCallback);
        m_liveResizeEventWatchHandle = GCHandle.Alloc(this, GCHandleType.Normal);
        var userData = (nint)GCHandle.ToIntPtr(m_liveResizeEventWatchHandle);
        if (SDL.Raw.AddEventWatch(m_liveResizeEventWatchPointer, (void*)userData) == 0)
        {
            Exception failure = new InvalidOperationException(Sdl3NativeText.ReadError() ?? "SDL_AddEventWatch failed.");
            m_liveResizeEventWatchHandle.Free();
            m_liveResizeEventWatch = null;
            m_liveResizeMainThreadCallback = null;
            m_liveResizeEventWatchPointer = null;
            m_liveResizeMainThreadCallbackPointer = null;
            SDL.QuitSubSystem(SDLInitFlags.Video | SDLInitFlags.Events);
            throw failure;
        }
    }

    /// <summary>
    /// Attempts to dequeue the next backend-neutral platform event.
    /// </summary>
    /// <param name="evnt">
    /// The evnt consumed by poll event; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public partial bool PollEvent(out Event? evnt)
    {
        ValidateOwnerThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        m_redrawFailure?.Throw();

        if (TryDequeuePendingEvent(out evnt))
        {
            return true;
        }

        PollAndCoalescePendingEvents();
        if (TryDequeuePendingEvent(out evnt))
        {
            return true;
        }

        evnt = null;
        return false;
    }

    /// <summary>
    /// Retrieves the requested windows value from current authoritative state.
    /// </summary>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public partial IReadOnlyList<Sdl3PlatformWindow> GetWindows()
    {
        ValidateOwnerThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);

        return GetWindowsCore();
    }

    private IReadOnlyList<Sdl3PlatformWindow> GetWindowsCore()
        => new List<Sdl3PlatformWindow>(m_windows.Values).AsReadOnly();

    /// <summary>
    /// Registers a platform extension for the application lifetime.
    /// </summary>
    /// <param name="extension">
    /// The extension consumed by register extension; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// The validated idisposable that represents the completed operation.
    /// </returns>
    public partial IDisposable RegisterExtension(ISdl3ApplicationExtension extension)
    {
        ValidateOwnerThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        ArgumentNullException.ThrowIfNull(extension);
        if (m_extensions.Contains(extension))
            throw new InvalidOperationException("The platform extension is already registered.");
        m_extensions.Add(extension);
        return new ExtensionRegistration(this, extension);
    }

    private bool TryDequeuePendingEvent(out Event? evnt)
    {
        if (m_pendingEventReadIndex >= m_pendingEvents.Count)
        {
            evnt = null;
            return false;
        }

        evnt = m_pendingEvents[m_pendingEventReadIndex++];
        if (m_pendingEventReadIndex >= m_pendingEvents.Count)
        {
            m_pendingEvents.Clear();
            m_pendingEventReadIndex = 0;
        }

        return true;
    }

    private void PollAndCoalescePendingEvents()
    {
        m_pendingEvents.Clear();
        m_pendingEventReadIndex = 0;

        Dictionary<PendingEventCoalesceKey, int> latestEventIndicesByKey = new();
        SDLEvent sdlEvent = default;
        while (SDL.Raw.PollEvent(&sdlEvent) != 0)
        {
            m_redrawFailure?.Throw();
            DispatchNativeEvent(ref sdlEvent);

            if (!TryTranslateEvent(ref sdlEvent, out var translatedEvent) || translatedEvent is null)
            {
                continue;
            }

            if (TryCreatePendingEventCoalesceKey(translatedEvent, out var key)
                && latestEventIndicesByKey.TryGetValue(key, out var index))
            {
                m_pendingEvents[index] = translatedEvent;
                continue;
            }

            var nextIndex = m_pendingEvents.Count;
            m_pendingEvents.Add(translatedEvent);
            if (TryCreatePendingEventCoalesceKey(translatedEvent, out key))
            {
                latestEventIndicesByKey[key] = nextIndex;
            }
        }
    }

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public partial void Dispose()
    {
        ValidateOwnerThread();
        if (m_disposed)
        {
            return;
        }

        if (m_liveResizeEventWatch is not null && m_liveResizeEventWatchHandle.IsAllocated)
        {
            var userData = (nint)GCHandle.ToIntPtr(m_liveResizeEventWatchHandle);
            unsafe
            {
                SDL.Raw.RemoveEventWatch(m_liveResizeEventWatchPointer, (void*)userData);
            }
            m_liveResizeEventWatchHandle.Free();
            m_liveResizeEventWatch = null;
            m_liveResizeMainThreadCallback = null;
            unsafe
            {
                m_liveResizeEventWatchPointer = null;
                m_liveResizeMainThreadCallbackPointer = null;
            }
        }

        foreach (ISdl3ApplicationExtension extension in m_extensions.ToArray())
            extension.OnApplicationDisposing(this);
        m_extensions.Clear();

        foreach (var window in new List<Sdl3PlatformWindow>(m_windows.Values))
        {
            window.Dispose();
        }

        m_windows.Clear();
        SDL.QuitSubSystem(SDLInitFlags.Video | SDLInitFlags.Events);
        m_disposed = true;
    }

    private static unsafe byte LiveResizeEventWatch(
        void* userData,
        SDLEvent* evnt
    ) {
        if (userData == null || evnt == null)
        {
            return 1;
        }

        var eventType = (SDLEventType)evnt->Type;
        if (eventType != SDLEventType.WindowExposed || evnt->Window.Data1 == 0)
        {
            return 1;
        }

        var handle = GCHandle.FromIntPtr((IntPtr)userData);
        if (handle.Target is not Sdl3PlatformApplication application || application.m_disposed)
        {
            return 1;
        }

        Volatile.Write(ref application.m_liveResizeWindowId, evnt->Window.WindowID);
        if (Interlocked.Exchange(ref application.m_liveResizeRedrawQueued, 1) == 0)
        {
            var userDataPtr = (nint)GCHandle.ToIntPtr(application.m_liveResizeEventWatchHandle);
            if (SDL.Raw.RunOnMainThread(application.m_liveResizeMainThreadCallbackPointer, (void*)userDataPtr, 0) == 0)
            {
                Interlocked.Exchange(ref application.m_liveResizeRedrawQueued, 0);
            }
        }

        return 1;
    }

    private static unsafe void LiveResizeMainThreadCallback(void* userData)
    {
        if (userData == null)
        {
            return;
        }

        var handle = GCHandle.FromIntPtr((IntPtr)userData);
        if (handle.Target is not Sdl3PlatformApplication application || application.m_disposed)
        {
            return;
        }

        var windowId = Volatile.Read(ref application.m_liveResizeWindowId);
        Interlocked.Exchange(ref application.m_liveResizeRedrawQueued, 0);
        if (windowId != 0)
        {
            try
            {
                application.DispatchLiveResizeRedraw(windowId);
            }
            catch (Exception exception)
            {
                // Never unwind managed exceptions through SDL's native event callback.
                application.m_redrawFailure ??= ExceptionDispatchInfo.Capture(exception);
            }
        }
    }

    private unsafe void DispatchNativeEvent(ref SDLEvent sdlEvent)
    {
        ReadOnlySpan<byte> nativeEventData = new(
            Unsafe.AsPointer(ref sdlEvent),
            sizeof(SDLEvent));
        foreach (ISdl3ApplicationExtension extension in m_extensions.ToArray())
            extension.ProcessNativeEvent(this, nativeEventData);
    }

    private void DispatchLiveResizeRedraw(uint windowId)
    {
        if (m_dispatchingRedraw || m_redrawFailure is not null)
            return;
        m_dispatchingRedraw = true;
        try
        {
            if (m_windows.TryGetValue(windowId, out Sdl3PlatformWindow? window))
            {
                int width = 0, height = 0;
                if (SDL.Raw.GetWindowSize(window.sdlWindow, &width, &height) == 0)
                    throw new InvalidOperationException(Sdl3NativeText.ReadError() ?? "SDL_GetWindowSize failed during live resize.");
                window.UpdateLogicalSize(width, height);
            }
            foreach (ISdl3ApplicationExtension extension in m_extensions.ToArray())
                extension.PrepareLiveResizeWindow(this, windowId);
            redrawRequested?.Invoke(windowId);
        }
        finally
        {
            m_dispatchingRedraw = false;
        }
    }

    private void UnregisterExtension(ISdl3ApplicationExtension extension) => m_extensions.Remove(extension);

    private sealed class ExtensionRegistration(
        Sdl3PlatformApplication application,
        ISdl3ApplicationExtension extension
    ) : IDisposable
    {
        private Sdl3PlatformApplication? m_application = application;
        private ISdl3ApplicationExtension? m_extension = extension;

        /// <summary>
        /// Releases the resources owned by this instance.
        /// </summary>
        public void Dispose()
        {
            Sdl3PlatformApplication? owner = Interlocked.Exchange(ref m_application, null);
            ISdl3ApplicationExtension? registered = Interlocked.Exchange(ref m_extension, null);
            if (owner is not null && registered is not null)
                owner.UnregisterExtension(registered);
        }
    }

    private unsafe bool TryTranslateEvent(
        ref SDLEvent sdlEvent,
        out Event? evnt
    ) {
        var eventType = (SDLEventType)sdlEvent.Type;
        switch (eventType)
        {
            case SDLEventType.Quit:
                evnt = new ApplicationQuitEvent();
                return true;

            case SDLEventType.WillEnterBackground:
                evnt = new ApplicationSuspensionChangedEvent(isSuspended: true);
                return true;

            case SDLEventType.DidEnterForeground:
                evnt = new ApplicationSuspensionChangedEvent(isSuspended: false);
                return true;

            case SDLEventType.WindowHidden:
            case SDLEventType.WindowMinimized:
            case SDLEventType.WindowShown:
            case SDLEventType.WindowRestored:
                if (m_windows.TryGetValue(sdlEvent.Window.WindowID, out Sdl3PlatformWindow? visibleWindow))
                {
                    visibleWindow.UpdateVisibility(eventType);
                    evnt = new WindowVisibilityChangedEvent(visibleWindow.windowId, visibleWindow.isVisible);
                    return true;
                }
                evnt = null;
                return false;

            case SDLEventType.WindowResized:
                if (m_windows.TryGetValue(sdlEvent.Window.WindowID, out var resizedWindow))
                {
                    resizedWindow.UpdateLogicalSize(sdlEvent.Window.Data1, sdlEvent.Window.Data2);
                }

                evnt = new WindowResizeEvent(sdlEvent.Window.WindowID, sdlEvent.Window.Data1, sdlEvent.Window.Data2);
                return true;

            case SDLEventType.WindowPixelSizeChanged:
                if (m_windows.TryGetValue(sdlEvent.Window.WindowID, out var pixelResizedWindow))
                {
                    pixelResizedWindow.UpdatePixelSize(sdlEvent.Window.Data1, sdlEvent.Window.Data2);
                    evnt = new WindowResizeEvent(
                        sdlEvent.Window.WindowID,
                        pixelResizedWindow.width,
                        pixelResizedWindow.height);
                    return true;
                }

                evnt = null;
                return false;

            case SDLEventType.WindowCloseRequested:
                if (m_windows.TryGetValue(sdlEvent.Window.WindowID, out var closingWindow))
                {
                    closingWindow.MarkClosed();
                }

                evnt = new WindowCloseEvent(sdlEvent.Window.WindowID);
                return true;

            case SDLEventType.WindowFocusGained:
            case SDLEventType.WindowFocusLost:
            {
                bool isFocused = eventType == SDLEventType.WindowFocusGained;
                if (m_windows.TryGetValue(sdlEvent.Window.WindowID, out Sdl3PlatformWindow? focusedWindow))
                    focusedWindow.UpdateFocus(isFocused);
                evnt = new WindowFocusChangedEvent(sdlEvent.Window.WindowID, isFocused);
                return true;
            }

            case SDLEventType.KeyDown:
            {
                var key = TranslateKey(sdlEvent.Key.Key);
                var modifiers = TranslateModifiers((SDLKeymod)sdlEvent.Key.Mod);
                evnt = new KeyPressedEvent(sdlEvent.Key.WindowID, key, modifiers, sdlEvent.Key.Repeat != 0);
                return true;
            }

            case SDLEventType.KeyUp:
            {
                var key = TranslateKey(sdlEvent.Key.Key);
                var modifiers = TranslateModifiers((SDLKeymod)sdlEvent.Key.Mod);
                evnt = new KeyReleasedEvent(sdlEvent.Key.WindowID, key, modifiers);
                return true;
            }

            case SDLEventType.TextInput:
            {
                string? text = sdlEvent.Text.Text == null
                    ? null
                    : Marshal.PtrToStringUTF8((IntPtr)sdlEvent.Text.Text);
                if (!string.IsNullOrEmpty(text))
                {
                    evnt = new TextInputEvent(sdlEvent.Text.WindowID, text);
                    return true;
                }
                break;
            }

            case SDLEventType.MouseMotion:
                evnt = new MouseMovedEvent(sdlEvent.Motion.WindowID, sdlEvent.Motion.X, sdlEvent.Motion.Y);
                return true;

            case SDLEventType.MouseWheel:
            {
                var wheelX = sdlEvent.Wheel.X;
                var wheelY = sdlEvent.Wheel.Y;
                if (sdlEvent.Wheel.Direction == SDLMouseWheelDirection.MousewheelFlipped)
                {
                    wheelX = -wheelX;
                    wheelY = -wheelY;
                }

                evnt = new MouseScrolledEvent(sdlEvent.Wheel.WindowID, wheelX, wheelY);
                return true;
            }

            case SDLEventType.MouseButtonDown:
                if (TryTranslateMouseButton(sdlEvent.Button.Button, out var pressedButton))
                {
                    evnt = new MouseButtonPressedEvent(sdlEvent.Button.WindowID, pressedButton);
                    return true;
                }

                break;

            case SDLEventType.MouseButtonUp:
                if (TryTranslateMouseButton(sdlEvent.Button.Button, out var releasedButton))
                {
                    evnt = new MouseButtonReleasedEvent(sdlEvent.Button.WindowID, releasedButton);
                    return true;
                }

                break;
        }

        evnt = null;
        return false;
    }

    private static KeyModifier TranslateModifiers(SDLKeymod modifiers)
    {
        var sdlModifiers = (uint)modifiers;
        var result = KeyModifier.None;

        if ((sdlModifiers & (SDL.Constants.SDL_KMOD_LSHIFT | SDL.Constants.SDL_KMOD_RSHIFT)) != 0)
        {
            result |= KeyModifier.Shift;
        }

        if ((sdlModifiers & (SDL.Constants.SDL_KMOD_LCTRL | SDL.Constants.SDL_KMOD_RCTRL)) != 0)
        {
            result |= KeyModifier.Control;
        }

        if ((sdlModifiers & (SDL.Constants.SDL_KMOD_LALT | SDL.Constants.SDL_KMOD_RALT)) != 0)
        {
            result |= KeyModifier.Alt;
        }

        if ((sdlModifiers & (SDL.Constants.SDL_KMOD_LGUI | SDL.Constants.SDL_KMOD_RGUI)) != 0)
        {
            result |= KeyModifier.Super;
        }

        return result;
    }

    private static bool TryTranslateMouseButton(
        byte sdlButton,
        out MouseButton button
    ) {
        switch (sdlButton)
        {
            case SDL.Constants.SDL_BUTTON_LEFT:
                button = MouseButton.Left;
                return true;
            case SDL.Constants.SDL_BUTTON_RIGHT:
                button = MouseButton.Right;
                return true;
            case SDL.Constants.SDL_BUTTON_MIDDLE:
                button = MouseButton.Middle;
                return true;
            case SDL.Constants.SDL_BUTTON_X1:
                button = MouseButton.XButton1;
                return true;
            case SDL.Constants.SDL_BUTTON_X2:
                button = MouseButton.XButton2;
                return true;
            default:
                button = MouseButton.Left;
                return false;
        }
    }

    private static KeyCode TranslateKey(uint sdlKey)
    {
        return sdlKey switch
        {
            SDL.Constants.SDLK_A => KeyCode.A,
            SDL.Constants.SDLK_B => KeyCode.B,
            SDL.Constants.SDLK_C => KeyCode.C,
            SDL.Constants.SDLK_D => KeyCode.D,
            SDL.Constants.SDLK_E => KeyCode.E,
            SDL.Constants.SDLK_F => KeyCode.F,
            SDL.Constants.SDLK_G => KeyCode.G,
            SDL.Constants.SDLK_H => KeyCode.H,
            SDL.Constants.SDLK_I => KeyCode.I,
            SDL.Constants.SDLK_J => KeyCode.J,
            SDL.Constants.SDLK_K => KeyCode.K,
            SDL.Constants.SDLK_L => KeyCode.L,
            SDL.Constants.SDLK_M => KeyCode.M,
            SDL.Constants.SDLK_N => KeyCode.N,
            SDL.Constants.SDLK_O => KeyCode.O,
            SDL.Constants.SDLK_P => KeyCode.P,
            SDL.Constants.SDLK_Q => KeyCode.Q,
            SDL.Constants.SDLK_R => KeyCode.R,
            SDL.Constants.SDLK_S => KeyCode.S,
            SDL.Constants.SDLK_T => KeyCode.T,
            SDL.Constants.SDLK_U => KeyCode.U,
            SDL.Constants.SDLK_V => KeyCode.V,
            SDL.Constants.SDLK_W => KeyCode.W,
            SDL.Constants.SDLK_X => KeyCode.X,
            SDL.Constants.SDLK_Y => KeyCode.Y,
            SDL.Constants.SDLK_Z => KeyCode.Z,

            SDL.Constants.SDLK_0 => KeyCode.D0,
            SDL.Constants.SDLK_1 => KeyCode.D1,
            SDL.Constants.SDLK_2 => KeyCode.D2,
            SDL.Constants.SDLK_3 => KeyCode.D3,
            SDL.Constants.SDLK_4 => KeyCode.D4,
            SDL.Constants.SDLK_5 => KeyCode.D5,
            SDL.Constants.SDLK_6 => KeyCode.D6,
            SDL.Constants.SDLK_7 => KeyCode.D7,
            SDL.Constants.SDLK_8 => KeyCode.D8,
            SDL.Constants.SDLK_9 => KeyCode.D9,

            SDL.Constants.SDLK_ESCAPE => KeyCode.Escape,
            SDL.Constants.SDLK_SPACE => KeyCode.Space,
            SDL.Constants.SDLK_RETURN => KeyCode.Enter,
            SDL.Constants.SDLK_TAB => KeyCode.Tab,
            SDL.Constants.SDLK_BACKSPACE => KeyCode.Backspace,

            SDL.Constants.SDLK_LEFT => KeyCode.LeftArrow,
            SDL.Constants.SDLK_UP => KeyCode.UpArrow,
            SDL.Constants.SDLK_RIGHT => KeyCode.RightArrow,
            SDL.Constants.SDLK_DOWN => KeyCode.DownArrow,

            SDL.Constants.SDLK_LGUI => KeyCode.LeftSuper,
            SDL.Constants.SDLK_RGUI => KeyCode.RightSuper,
            SDL.Constants.SDLK_LSHIFT => KeyCode.LeftShift,
            SDL.Constants.SDLK_RSHIFT => KeyCode.RightShift,
            SDL.Constants.SDLK_LCTRL => KeyCode.LeftCtrl,
            SDL.Constants.SDLK_RCTRL => KeyCode.RightCtrl,
            SDL.Constants.SDLK_LALT => KeyCode.LeftAlt,
            SDL.Constants.SDLK_RALT => KeyCode.RightAlt,

            SDL.Constants.SDLK_CAPSLOCK => KeyCode.CapsLock,
            SDL.Constants.SDLK_INSERT => KeyCode.Insert,
            SDL.Constants.SDLK_DELETE => KeyCode.Delete,
            SDL.Constants.SDLK_HOME => KeyCode.Home,
            SDL.Constants.SDLK_END => KeyCode.End,
            SDL.Constants.SDLK_PAGEUP => KeyCode.PageUp,
            SDL.Constants.SDLK_PAGEDOWN => KeyCode.PageDown,

            SDL.Constants.SDLK_KP_0 => KeyCode.NumPad0,
            SDL.Constants.SDLK_KP_1 => KeyCode.NumPad1,
            SDL.Constants.SDLK_KP_2 => KeyCode.NumPad2,
            SDL.Constants.SDLK_KP_3 => KeyCode.NumPad3,
            SDL.Constants.SDLK_KP_4 => KeyCode.NumPad4,
            SDL.Constants.SDLK_KP_5 => KeyCode.NumPad5,
            SDL.Constants.SDLK_KP_6 => KeyCode.NumPad6,
            SDL.Constants.SDLK_KP_7 => KeyCode.NumPad7,
            SDL.Constants.SDLK_KP_8 => KeyCode.NumPad8,
            SDL.Constants.SDLK_KP_9 => KeyCode.NumPad9,

            SDL.Constants.SDLK_NUMLOCKCLEAR => KeyCode.NumLock,
            SDL.Constants.SDLK_SCROLLLOCK => KeyCode.ScrollLock,

            SDL.Constants.SDLK_F1 => KeyCode.F1,
            SDL.Constants.SDLK_F2 => KeyCode.F2,
            SDL.Constants.SDLK_F3 => KeyCode.F3,
            SDL.Constants.SDLK_F4 => KeyCode.F4,
            SDL.Constants.SDLK_F5 => KeyCode.F5,
            SDL.Constants.SDLK_F6 => KeyCode.F6,
            SDL.Constants.SDLK_F7 => KeyCode.F7,
            SDL.Constants.SDLK_F8 => KeyCode.F8,
            SDL.Constants.SDLK_F9 => KeyCode.F9,
            SDL.Constants.SDLK_F10 => KeyCode.F10,
            SDL.Constants.SDLK_F11 => KeyCode.F11,
            SDL.Constants.SDLK_F12 => KeyCode.F12,

            SDL.Constants.SDLK_PLUS => KeyCode.Plus,
            SDL.Constants.SDLK_EQUALS => KeyCode.Plus,
            SDL.Constants.SDLK_COMMA => KeyCode.Comma,
            SDL.Constants.SDLK_MINUS => KeyCode.Minus,
            SDL.Constants.SDLK_PERIOD => KeyCode.Period,
            SDL.Constants.SDLK_SLASH => KeyCode.Slash,
            SDL.Constants.SDLK_GRAVE => KeyCode.Tilde,
            SDL.Constants.SDLK_BACKSLASH => KeyCode.Backslash,
            SDL.Constants.SDLK_SEMICOLON => KeyCode.Semicolon,
            SDL.Constants.SDLK_APOSTROPHE => KeyCode.Quote,
            SDL.Constants.SDLK_LEFTBRACKET => KeyCode.LeftBracket,
            SDL.Constants.SDLK_RIGHTBRACKET => KeyCode.RightBracket,
            _ => KeyCode.Unknown
        };
    }

    private static bool TryCreatePendingEventCoalesceKey(
        Event evnt,
        out PendingEventCoalesceKey key
    ) {
        if (evnt is WindowEvent windowEvent)
        {
            key = new PendingEventCoalesceKey(windowEvent.windowId, evnt.GetType());
            return true;
        }

        if (evnt is ApplicationEvent)
        {
            key = new PendingEventCoalesceKey(0, evnt.GetType());
            return true;
        }

        key = default;
        return false;
    }

    private readonly record struct PendingEventCoalesceKey(
        uint windowId,
        Type eventType
    );
}
