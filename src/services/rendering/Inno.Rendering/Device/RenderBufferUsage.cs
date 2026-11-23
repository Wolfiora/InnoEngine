using System;

namespace Inno.Rendering;

/// <summary>
/// Declares intended buffer operations for capability and hazard validation.
/// </summary>
[Flags]
public enum RenderBufferUsage
{
    /// <summary>
    /// Vertex input data.
    /// </summary>
    Vertex = 1 << 0,
    /// <summary>
    /// Index input data.
    /// </summary>
    Index = 1 << 1,
    /// <summary>
    /// Read-only shader data.
    /// </summary>
    Uniform = 1 << 2,
    /// <summary>
    /// Shader-readable and writable storage data.
    /// </summary>
    Storage = 1 << 3,
    /// <summary>
    /// Indirect command data.
    /// </summary>
    Indirect = 1 << 4,
    /// <summary>
    /// Copy operation source.
    /// </summary>
    CopySource = 1 << 5,
    /// <summary>
    /// Copy operation destination.
    /// </summary>
    CopyDestination = 1 << 6,
    /// <summary>
    /// Contents may be replaced at frame safety points.
    /// </summary>
    Dynamic = 1 << 7
}
