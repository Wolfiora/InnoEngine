using System;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Native.Sdl3;
using Inno.Platform;

namespace Inno.Integration.MacOSArm64.Sdl3;

/// <summary>
/// Provides the MacOS SDK, native surface and focus policies for the shared SDL backend.
/// </summary>
public sealed class MacOSArm64Sdl3HostIntegration : ISdl3HostIntegration
{
    /// <inheritdoc />
    public Sdl3HostCapabilities capabilities { get; } = new(multipleWindows: true, liveResize: true);

    /// <inheritdoc />
    public void ConfigureInitialization()
    {
        _ = SDL.SetHint("SDL_MAC_PRESS_AND_HOLD", "0");
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
        uint properties = SDL.Raw.GetWindowProperties((SDLWindow*)windowHandle);
        ReadOnlySpan<byte> propertyName = "SDL.window.cocoa.window\0"u8;
        nint surface;
        fixed (byte* name = propertyName)
            surface = (nint)SDL.Raw.GetPointerProperty(properties, (sbyte*)name, null);
        if (surface == 0)
            throw new InvalidOperationException("SDL did not provide the required MacOS surface.");
        return new PlatformNativeHandles(surface, handleKind: PlatformNativeHandleId.cocoa);
    }

    /// <inheritdoc />
    public bool GetInitialFocus(nint windowHandle)
    {
        return Sdl3WindowOperations.ReadFocus(windowHandle);
    }
}
