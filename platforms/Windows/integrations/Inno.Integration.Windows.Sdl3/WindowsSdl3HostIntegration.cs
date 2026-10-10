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
    public nint CreateWindow(PlatformWindowOptions options)
    {
        return Sdl3WindowOperations.CreateWindow(options, requireOpenGl: false);
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
        return Sdl3WindowOperations.ReadFocus(windowHandle);
    }
}
