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
    public unsafe nint CreateWindow(PlatformWindowOptions options)
    {
        uint properties = SDL.CreateProperties();
        if (properties == 0)
            throw new InvalidOperationException(SDL.GetError() ?? "SDL_CreateProperties failed.");
        try
        {
            if (!SDL.SetStringProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_TITLE_STRING, options.title)
                || !SDL.SetNumberProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_WIDTH_NUMBER, options.width)
                || !SDL.SetNumberProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_HEIGHT_NUMBER, options.height)
                || !SDL.SetBooleanProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_OPENGL_BOOLEAN, true)
                || !SDL.SetBooleanProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_HIDDEN_BOOLEAN, !options.visible)
                || !SDL.SetBooleanProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_RESIZABLE_BOOLEAN, options.resizable)
                || !SDL.SetBooleanProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_HIGH_PIXEL_DENSITY_BOOLEAN, options.highPixelDensity))
            {
                throw new InvalidOperationException(SDL.GetError() ?? "SDL window properties could not be set.");
            }
            SDLWindow window = SDL.CreateWindowWithProperties(properties);
            if (window.IsNull)
                throw new InvalidOperationException(SDL.GetError() ?? "SDL canvas window creation failed.");
            return (nint)window.Handle;
        }
        finally
        {
            SDL.DestroyProperties(properties);
        }
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
