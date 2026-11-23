using System;

namespace Inno.Rendering;

/// <summary>
/// Declares intended texture operations for capability and hazard validation.
/// </summary>
[Flags]
public enum RenderTextureUsage
{
    /// <summary>
    /// Shader-readable texture.
    /// </summary>
    Sampled = 1 << 0,
    /// <summary>
    /// Raster color attachment.
    /// </summary>
    ColorAttachment = 1 << 1,
    /// <summary>
    /// Raster depth or stencil attachment.
    /// </summary>
    DepthStencilAttachment = 1 << 2,
    /// <summary>
    /// Shader-readable and writable unordered texture.
    /// </summary>
    Storage = 1 << 3,
    /// <summary>
    /// Copy operation source.
    /// </summary>
    CopySource = 1 << 4,
    /// <summary>
    /// Copy operation destination.
    /// </summary>
    CopyDestination = 1 << 5,
    /// <summary>
    /// Asynchronous transfer from GPU texture memory to CPU-visible bytes.
    /// </summary>
    Readback = 1 << 6
}
