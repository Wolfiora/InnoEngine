using System;
using Inno.Native.Sdl3;
using Inno.Platform;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Implements shared SDL window operations without selecting a system host or registering windows.
/// </summary>
public static class Sdl3WindowOperations
{
    /// <summary>
    /// Creates an SDL window and transfers its native destruction responsibility to the caller.
    /// </summary>
    /// <param name="options">
    /// The title, logical extent, visibility, resizing and pixel density requested by the product.
    /// </param>
    /// <param name="requireOpenGl">
    /// Whether the selected host requires an OpenGL-capable window.
    /// </param>
    /// <returns>
    /// A nonzero opaque SDL handle. Temporary creation properties are always released.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The title is null or the logical extent is not positive.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// SDL could not allocate properties, apply an option or create the window.
    /// </exception>
    public static unsafe nint CreateWindow(
        PlatformWindowOptions options,
        bool requireOpenGl
    ) {
        ArgumentNullException.ThrowIfNull(options.title);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(options.width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(options.height);
        uint properties = SDL.CreateProperties();
        if (properties == 0)
            throw new InvalidOperationException(SDL.GetError() ?? "SDL_CreateProperties failed.");
        try
        {
            if (!SDL.SetStringProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_TITLE_STRING, options.title)
                || !SDL.SetNumberProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_WIDTH_NUMBER, options.width)
                || !SDL.SetNumberProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_HEIGHT_NUMBER, options.height)
                || !SDL.SetBooleanProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_OPENGL_BOOLEAN, requireOpenGl)
                || !SDL.SetBooleanProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_HIDDEN_BOOLEAN, !options.visible)
                || !SDL.SetBooleanProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_RESIZABLE_BOOLEAN, options.resizable)
                || !SDL.SetBooleanProperty(properties, SDL.SDL_PROP_WINDOW_CREATE_HIGH_PIXEL_DENSITY_BOOLEAN, options.highPixelDensity))
            {
                throw new InvalidOperationException(SDL.GetError() ?? "SDL window properties could not be set.");
            }
            SDLWindow window = SDL.CreateWindowWithProperties(properties);
            if (window.IsNull)
                throw new InvalidOperationException(SDL.GetError() ?? "SDL window creation failed.");
            return (nint)window.Handle;
        }
        finally
        {
            SDL.DestroyProperties(properties);
        }
    }

    /// <summary>
    /// Reads native focus without registering the window or changing its input state.
    /// </summary>
    /// <param name="windowHandle">
    /// A live SDL window whose owner keeps it alive for this call.
    /// </param>
    /// <returns>
    /// Whether SDL currently reports keyboard focus for this window.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The handle is zero.
    /// </exception>
    public static bool ReadFocus(nint windowHandle)
    {
        if (windowHandle == 0)
            throw new ArgumentException("A live SDL window is required.", nameof(windowHandle));
        return (SDL.GetWindowFlags(new SDLWindow(windowHandle)) & SDLWindowFlags.InputFocus) != 0;
    }
}
