using System;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Native.Sdl3;
using Inno.Platform;

namespace Inno.Integration.Windows.Sdl3;

/// <summary>
/// Provides the Windows SDK, native surface and focus policies for the shared SDL backend.
/// </summary>
public sealed class WindowsSdl3HostIntegration : ISdl3HostIntegration
{
    /// <inheritdoc />
    public Sdl3HostCapabilities capabilities { get; } = new(multipleWindows: true, liveResize: true);

    /// <inheritdoc />
    public void ConfigureInitialization()
    {
    }

    /// <inheritdoc />
    public unsafe nint CreateWindow(PlatformWindowOptions options)
    {
        SDLWindowFlags flags = options.highPixelDensity ? SDLWindowFlags.HighPixelDensity : 0;
        if (!options.visible)
            flags |= SDLWindowFlags.Hidden;
        if (options.resizable)
            flags |= SDLWindowFlags.Resizable;
        SDLWindow window = SDL.CreateWindow(options.title, options.width, options.height, flags);
        if (window.IsNull)
            throw new InvalidOperationException(SDL.GetError() ?? "SDL window creation failed.");
        return (nint)window.Handle;
    }

    /// <inheritdoc />
    public unsafe PlatformNativeHandles ResolveNativeSurface(nint windowHandle)
    {
        if (windowHandle == 0)
            throw new ArgumentException("A live SDL window is required.", nameof(windowHandle));
        SDLWindow window = new(windowHandle);
        uint properties = SDL.GetWindowProperties(window);
        nint surface = (nint)SDL.GetPointerProperty(properties, SDL.SDL_PROP_WINDOW_WIN32_HWND_POINTER, null);
        if (surface == 0)
            throw new InvalidOperationException("SDL did not provide the required Windows surface.");
        return new PlatformNativeHandles(surface, handleKind: PlatformNativeHandleId.win32);
    }

    /// <inheritdoc />
    public bool GetInitialFocus(nint windowHandle)
    {
        if (windowHandle == 0)
            throw new ArgumentException("A live SDL window is required.", nameof(windowHandle));
        return (SDL.GetWindowFlags(new SDLWindow(windowHandle)) & SDLWindowFlags.InputFocus) != 0;
    }
}
