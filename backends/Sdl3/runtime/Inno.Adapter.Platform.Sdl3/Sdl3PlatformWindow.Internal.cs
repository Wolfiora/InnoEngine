using Inno.Adapter.Platform;
using System;
using Inno.Native.Sdl3;
using Inno.Platform;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Represents a native platform window.
/// </summary>
public sealed partial class Sdl3PlatformWindow
{
    private readonly Action<Sdl3PlatformWindow> m_onDisposed;
    private readonly int m_ownerThreadId;
    private SDLWindow m_window;
    private readonly uint m_windowId;
    private readonly string m_title;
    private readonly bool m_ownsNativeWindow;
    private int m_width;
    private int m_height;
    private int m_pixelWidth;
    private int m_pixelHeight;
    private bool m_isClosed;
    private bool m_isFocused;
    private bool m_hidden;
    private bool m_minimized;
    private readonly PlatformNativeHandles m_nativeHandles;
    private readonly nint m_sdlWindowHandle;
    private bool m_disposed;
    internal SDLWindow sdlWindow => m_window;
    internal bool ownsNativeWindow => m_ownsNativeWindow;
    internal bool isVisible => !m_hidden && !m_minimized;

    internal Sdl3PlatformWindow(
        SDLWindow window,
        string title,
        bool ownsNativeWindow,
        ISdl3HostIntegration hostIntegration,
        Action<Sdl3PlatformWindow> onDisposed,
        int ownerThreadId
    ) {
        m_onDisposed = onDisposed;
        m_ownerThreadId = ownerThreadId;
        m_window = window;
        m_sdlWindowHandle = (nint)window.Handle;
        m_title = title;
        m_ownsNativeWindow = ownsNativeWindow;
        m_windowId = SDL.GetWindowID(m_window);

        var currentWidth = 0;
        var currentHeight = 0;
        if (!SDL.GetWindowSize(m_window, ref currentWidth, ref currentHeight))
            throw new InvalidOperationException(SDL.GetError() ?? "SDL_GetWindowSize failed.");
        m_width = currentWidth;
        m_height = currentHeight;
        RefreshPixelSize();
        var flags = (SDLWindowFlags)SDL.GetWindowFlags(m_window);
        m_isFocused = hostIntegration.GetInitialFocus(m_sdlWindowHandle);
        m_hidden = (flags & SDLWindowFlags.Hidden) != 0;
        m_minimized = (flags & SDLWindowFlags.Minimized) != 0;
        m_nativeHandles = hostIntegration.ResolveNativeSurface(m_sdlWindowHandle);
    }

    internal void UpdateLogicalSize(
        int width,
        int height
    ) {
        m_width = width;
        m_height = height;
        RefreshPixelSize();
    }

    internal void UpdatePixelSize(
        int width,
        int height
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(width);
        ArgumentOutOfRangeException.ThrowIfNegative(height);
        m_pixelWidth = width;
        m_pixelHeight = height;
    }

    internal void MarkClosed()
    {
        m_isClosed = true;
    }

    internal void UpdateFocus(bool isFocused)
    {
        m_isFocused = isFocused;
    }

    internal void UpdateVisibility(SDLEventType eventType)
    {
        switch (eventType)
        {
            case SDLEventType.WindowHidden:
                m_hidden = true;
                break;
            case SDLEventType.WindowShown:
                m_hidden = false;
                break;
            case SDLEventType.WindowMinimized:
                m_minimized = true;
                break;
            case SDLEventType.WindowRestored:
                m_minimized = false;
                break;
        }
    }

    private void RefreshPixelSize()
    {
        var pixelWidth = 0;
        var pixelHeight = 0;
        if (!SDL.GetWindowSizeInPixels(m_window, ref pixelWidth, ref pixelHeight))
            throw new InvalidOperationException(SDL.GetError() ?? "SDL_GetWindowSizeInPixels failed.");
        UpdatePixelSize(pixelWidth, pixelHeight);
    }
    
    /// <summary>
    /// Queues a close request for processing at the next platform safety point.
    /// </summary>
    public partial void RequestClose()
    {
        m_isClosed = true;
    }

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public partial void Dispose()
    {
        if (Environment.CurrentManagedThreadId != m_ownerThreadId)
            throw new InvalidOperationException("SDL window disposal must run on its owner thread.");
        if (m_disposed)
        {
            return;
        }

        if (!m_window.IsNull && m_ownsNativeWindow)
        {
            SDL.DestroyWindow(m_window);
        }

        m_window = SDLWindow.Null;
        m_isClosed = true;
        m_isFocused = false;
        m_disposed = true;
        m_onDisposed(this);
    }

}
