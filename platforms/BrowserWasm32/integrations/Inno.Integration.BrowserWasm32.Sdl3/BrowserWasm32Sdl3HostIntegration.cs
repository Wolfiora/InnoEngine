using System;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Native.Sdl3;
using Inno.Platform;

namespace Inno.Integration.BrowserWasm32.Sdl3;

/// <summary>
/// Provides the Browser SDK, native surface and focus policies for the shared SDL backend.
/// </summary>
public sealed class BrowserWasm32Sdl3HostIntegration : ISdl3HostIntegration
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
        uint properties = SDL.Raw.GetWindowProperties((SDLWindow*)windowHandle);
        ReadOnlySpan<byte> propertyName = "SDL.window.emscripten.canvas_id\0"u8;
        fixed (byte* name = propertyName)
        {
            nint selector = (nint)SDL.Raw.GetStringProperty(properties, (sbyte*)name, null);
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
