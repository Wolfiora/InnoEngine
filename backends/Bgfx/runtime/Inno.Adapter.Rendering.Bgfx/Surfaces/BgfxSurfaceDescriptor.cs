using System;

namespace Inno.Adapter.Rendering.Bgfx;

/// <summary>
/// Freezes borrowed window and display values without exposing BGFX native structures.
/// </summary>
/// <remarks>
/// The original window owner must keep these values valid until rendering retirement completes.
/// The default value is invalid and is rejected before native initialization or framebuffer creation.
/// </remarks>
public readonly record struct BgfxSurfaceDescriptor
{
    /// <summary>
    /// Creates a validated borrowing description for a selected host integration.
    /// </summary>
    /// <param name="windowHandle">
    /// The nonzero opaque native window value or stable canvas selector.
    /// </param>
    /// <param name="displayHandle">
    /// The optional display value required by the selected ABI.
    /// </param>
    /// <param name="supportsSrgbReset">
    /// Whether this primary surface permits BGFX's sRGB reset option.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The window value is zero.
    /// </exception>
    public BgfxSurfaceDescriptor(
        nint windowHandle,
        nint displayHandle,
        bool supportsSrgbReset
    ) {
        if (windowHandle == 0)
            throw new ArgumentException("A live borrowed window value is required.", nameof(windowHandle));
        this.windowHandle = windowHandle;
        this.displayHandle = displayHandle;
        this.supportsSrgbReset = supportsSrgbReset;
    }

    /// <summary>
    /// Gets the borrowed window value; the default descriptor has no valid window.
    /// </summary>
    public nint windowHandle { get; }

    /// <summary>
    /// Gets the borrowed display value, or zero when this ABI does not require one.
    /// </summary>
    public nint displayHandle { get; }

    /// <summary>
    /// Gets whether this primary surface permits sRGB reset.
    /// </summary>
    public bool supportsSrgbReset { get; }
}
