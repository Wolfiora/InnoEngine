using Inno.Adapter.Platform;
using Inno.Platform;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Connects an explicitly selected system host to SDL without selecting platforms inside the backend.
/// </summary>
/// <remarks>
/// The application borrows immutable integration configuration. Every operation runs on its owner thread.
/// SDL handles are adapter-only identities and must never enter domain or scripting APIs.
/// </remarks>
public interface ISdl3HostIntegration
{
    /// <summary>
    /// Gets the host's supported window and scheduling behavior.
    /// </summary>
    Sdl3HostCapabilities capabilities { get; }

    /// <summary>
    /// Applies host-specific SDK configuration before the application's SDL initialization.
    /// </summary>
    void ConfigureInitialization();

    /// <summary>
    /// Creates one native window whose ownership transfers to the application on success.
    /// </summary>
    /// <param name="options">
    /// The explicit window options supplied by the product.
    /// </param>
    /// <returns>
    /// A nonzero opaque SDL window handle. Failure must throw without leaking a window.
    /// </returns>
    nint CreateWindow(PlatformWindowOptions options);

    /// <summary>
    /// Resolves borrowed graphics surface information for a live SDL window.
    /// </summary>
    /// <param name="windowHandle">
    /// The opaque handle created or explicitly adopted by the application.
    /// </param>
    /// <returns>
    /// Borrowed surface handles valid until the native window is destroyed; unavailable surfaces fail.
    /// </returns>
    PlatformNativeHandles ResolveNativeSurface(nint windowHandle);

    /// <summary>
    /// Reads the initial focus policy before platform events begin updating the wrapper.
    /// </summary>
    /// <param name="windowHandle">
    /// The live SDL window being registered.
    /// </param>
    /// <returns>
    /// Whether game and editor input may initially treat the window as focused.
    /// </returns>
    bool GetInitialFocus(nint windowHandle);
}
