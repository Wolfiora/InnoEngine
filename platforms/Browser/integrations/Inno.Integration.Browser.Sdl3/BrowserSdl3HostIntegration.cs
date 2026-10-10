using System;
using System.Text;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Native.Sdl3;
using Inno.Platform;

namespace Inno.Integration.Browser.Sdl3;

/// <summary>
/// Provides the Browser SDK, native surface and focus policies for the shared SDL backend.
/// </summary>
public sealed class BrowserSdl3HostIntegration : ISdl3HostIntegration
{
    /// <inheritdoc />
    public Sdl3HostCapabilities capabilities { get; } = new(multipleWindows: false, liveResize: false);

    /// <inheritdoc />
    public void ConfigureInitialization()
    {
    }

    /// <inheritdoc />
    public nint CreateWindow(PlatformWindowOptions options)
    {
        return Sdl3WindowOperations.CreateWindow(options, requireOpenGl: true);
    }

    /// <inheritdoc />
    public unsafe PlatformNativeHandles ResolveNativeSurface(nint windowHandle)
    {
        if (windowHandle == 0)
            throw new ArgumentException("A live SDL window is required.", nameof(windowHandle));
        uint properties = SDL.GetWindowProperties(new SDLWindow(windowHandle));
        byte[] propertyName = Encoding.UTF8.GetBytes(SDL.SDL_PROP_WINDOW_EMSCRIPTEN_CANVAS_ID_STRING + '\0');
        fixed (byte* name = propertyName)
        {
            nint selector = (nint)SDL.GetStringProperty(properties, name, null);
            if (selector == 0)
                throw new InvalidOperationException("SDL did not provide a browser canvas selector.");
            return new PlatformNativeHandles(selector, handleKind: PlatformNativeHandleId.browserCanvas);
        }
    }

    /// <inheritdoc />
    public bool GetInitialFocus(nint windowHandle)
    {
        if (windowHandle == 0)
            throw new ArgumentException("A live SDL window is required.", nameof(windowHandle));
        return false;
    }
}
