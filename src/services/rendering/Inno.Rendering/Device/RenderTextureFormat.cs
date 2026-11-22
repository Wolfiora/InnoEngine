namespace Inno.Rendering;

/// <summary>
/// Declares backend-neutral texture storage formats.
/// </summary>
public enum RenderTextureFormat
{
    /// <summary>
    /// Eight-bit single-channel normalized format.
    /// </summary>
    R8,
    /// <summary>
    /// Eight-bit two-channel normalized format.
    /// </summary>
    RG8,
    /// <summary>
    /// Eight-bit four-channel linear normalized format.
    /// </summary>
    RGBA8,
    /// <summary>
    /// Eight-bit four-channel sRGB format.
    /// </summary>
    RGBA8Srgb,
    /// <summary>
    /// Ten-bit RGB and two-bit alpha normalized format.
    /// </summary>
    RGB10A2,
    /// <summary>
    /// Eleven-bit RGB floating-point format.
    /// </summary>
    RG11B10Float,
    /// <summary>
    /// Half-precision four-channel floating-point format.
    /// </summary>
    RGBA16Float,
    /// <summary>
    /// Single-channel 32-bit floating-point format.
    /// </summary>
    R32Float,
    /// <summary>
    /// Twenty-four-bit depth and eight-bit stencil format.
    /// </summary>
    Depth24Stencil8,
    /// <summary>
    /// Thirty-two-bit floating-point depth format.
    /// </summary>
    Depth32Float
}
