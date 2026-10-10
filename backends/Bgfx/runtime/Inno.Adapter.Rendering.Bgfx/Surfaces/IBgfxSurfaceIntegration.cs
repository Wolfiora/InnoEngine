using Inno.Adapter.Platform;

namespace Inno.Adapter.Rendering.Bgfx;

/// <summary>
/// Identifies how a borrowed window is used by a BGFX device, independently of its system ABI.
/// </summary>
public enum BgfxSurfaceRole
{
    /// <summary>
    /// The primary window supplied before device initialization.
    /// </summary>
    Primary,
    /// <summary>
    /// An additional window whose framebuffer is owned by an active device generation.
    /// </summary>
    Additional
}

/// <summary>
/// Resolves an explicitly selected host's borrowed surface handles for the shared BGFX backend.
/// </summary>
/// <remarks>
/// The device borrows this immutable integration. Resolution runs once per surface on its owner thread,
/// before acquiring native ownership. Neither the integration nor the device owns the native window.
/// </remarks>
public interface IBgfxSurfaceIntegration
{
    /// <summary>
    /// Gets whether this host permits additional surfaces, subject to the device's SwapChain capability.
    /// </summary>
    bool supportsAdditionalSurfaces { get; }

    /// <summary>
    /// Validates a host ABI and freezes the handle values needed by the requested surface role.
    /// </summary>
    /// <param name="handles">
    /// The borrowed window and optional display, kept alive by their original owner until retirement.
    /// </param>
    /// <param name="role">
    /// The primary or additional use requested by the backend.
    /// </param>
    /// <returns>
    /// A valid immutable borrowing description; unsupported ABIs or roles must fail explicitly.
    /// </returns>
    /// <exception cref="System.ArgumentException">
    /// A handle or role is invalid.
    /// </exception>
    /// <exception cref="System.NotSupportedException">
    /// The host cannot connect the supplied ABI or requested role.
    /// </exception>
    BgfxSurfaceDescriptor Resolve(
        PlatformNativeHandles handles,
        BgfxSurfaceRole role
    );
}
