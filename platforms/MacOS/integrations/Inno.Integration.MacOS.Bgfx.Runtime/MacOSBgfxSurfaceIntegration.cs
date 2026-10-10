using System;
using Inno.Adapter.Platform;
using Inno.Adapter.Rendering.Bgfx;

namespace Inno.Integration.MacOS.Bgfx.Runtime;

/// <summary>
/// Connects the MacOS surface ABI to BGFX without exposing native BGFX types.
/// </summary>
public sealed class MacOSBgfxSurfaceIntegration : IBgfxSurfaceIntegration
{
    /// <summary>
    /// Gets whether this system integration permits additional window surfaces.
    /// </summary>
    public bool supportsAdditionalSurfaces => true;

    /// <summary>
    /// Validates this system's borrowed surface ABI before native renderer ownership is acquired.
    /// </summary>
    /// <param name="handles">
    /// The platform window owner's immutable borrowed handles.
    /// </param>
    /// <param name="role">
    /// The requested primary or additional presentation role.
    /// </param>
    /// <returns>
    /// The validated handle binding and reset capability, without transferring window ownership.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The surface ABI or requested presentation role is not supported by this integration.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The requested role or native window handle is invalid.
    /// </exception>
    public BgfxSurfaceDescriptor Resolve(
        PlatformNativeHandles handles,
        BgfxSurfaceRole role
    ) {
        if (role != BgfxSurfaceRole.Primary && role != BgfxSurfaceRole.Additional)
            throw new ArgumentOutOfRangeException(nameof(role));
        if (handles.handleKind != PlatformNativeHandleId.cocoa)
            throw new NotSupportedException("The MacOS BGFX integration requires its explicit cocoa surface ABI.");
        return new BgfxSurfaceDescriptor(handles.windowHandle, handles.displayHandle,
            supportsSrgbReset: true);
    }
}
