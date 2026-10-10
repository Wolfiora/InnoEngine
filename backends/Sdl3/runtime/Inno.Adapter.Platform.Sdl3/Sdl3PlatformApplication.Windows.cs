using System;
using Inno.Native.Sdl3;
using Inno.Platform;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Owns the application window directory and its native lifetime transfers.
/// </summary>
public sealed partial class Sdl3PlatformApplication
{
    /// <summary>
    /// Creates and registers a window through the borrowed host integration.
    /// </summary>
    /// <param name="options">
    /// The logical size, visibility and title requested by the product.
    /// </param>
    /// <returns>
    /// A wrapper whose native window is owned by this application until disposal.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Creation or surface inspection fails, or execution is outside the owner thread.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The host does not support an additional window.
    /// </exception>
    public partial Sdl3PlatformWindow CreateWindow(PlatformWindowOptions options)
    {
        ValidateWindowRegistration();
        nint handle = m_hostIntegration.CreateWindow(options);
        if (handle == 0)
            throw new InvalidOperationException("The SDL host integration returned no window.");
        SDLWindow nativeWindow = new(handle);
        try
        {
            return RegisterWindow(nativeWindow, options.title, ownsNativeWindow: true);
        }
        catch
        {
            SDL.DestroyWindow(nativeWindow);
            throw;
        }
    }

    /// <summary>
    /// Registers an externally owned window using this application's host and surface integration.
    /// </summary>
    /// <param name="windowHandle">
    /// The live opaque SDL window handle; its original owner retains destruction responsibility.
    /// </param>
    /// <returns>
    /// A borrowed wrapper visible only in this application's window directory.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The handle is null or the window is already registered in this application.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The host does not support additional windows.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Registration runs outside the owner thread or window inspection fails.
    /// </exception>
    public Sdl3PlatformWindow AdoptWindow(nint windowHandle)
    {
        ValidateWindowRegistration();
        if (windowHandle == 0)
            throw new ArgumentException("A live SDL window is required.", nameof(windowHandle));
        SDLWindow window = new(windowHandle);
        return RegisterWindow(window, SDL.GetWindowTitle(window) ?? string.Empty, ownsNativeWindow: false);
    }

    /// <summary>
    /// Invalidates and unregisters a borrowed window before its original owner destroys it.
    /// </summary>
    /// <param name="window">
    /// The external wrapper returned by this application's adoption operation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The window is owned by SDL application creation or is not registered by this application.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The operation runs outside the owner thread.
    /// </exception>
    public void ReleaseWindow(Sdl3PlatformWindow window)
    {
        ValidateOwnerThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        ArgumentNullException.ThrowIfNull(window);
        if (window.ownsNativeWindow || !m_windows.TryGetValue(window.windowId, out Sdl3PlatformWindow? registered)
            || !ReferenceEquals(registered, window))
            throw new ArgumentException("Only this application's adopted window can be released.", nameof(window));
        window.Dispose();
    }

    private Sdl3PlatformWindow RegisterWindow(
        SDLWindow nativeWindow,
        string title,
        bool ownsNativeWindow
    ) {
        uint id = SDL.GetWindowID(nativeWindow);
        if (id == 0 || m_windows.ContainsKey(id))
            throw new ArgumentException("The SDL window is invalid or already registered.", nameof(nativeWindow));
        var window = new Sdl3PlatformWindow(nativeWindow, title, ownsNativeWindow, m_hostIntegration, RemoveWindow, m_ownerThreadId);
        m_windows.Add(id, window);
        return window;
    }

    private void RemoveWindow(Sdl3PlatformWindow window) => m_windows.Remove(window.windowId);

    private void ValidateWindowRegistration()
    {
        ValidateOwnerThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (!m_hostIntegration.capabilities.multipleWindows && m_windows.Count != 0)
            throw new NotSupportedException("The selected SDL host supports only one window.");
    }

    private void ValidateOwnerThread()
    {
        if (Environment.CurrentManagedThreadId != m_ownerThreadId)
            throw new InvalidOperationException("SDL application operations must run on their owner thread.");
    }
}
