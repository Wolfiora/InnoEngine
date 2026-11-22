namespace Inno.Rendering;

/// <summary>
/// Identifies the dimensional shape of a backend-neutral texture resource.
/// </summary>
public enum RenderTextureDimension
{
    /// <summary>
    /// Two-dimensional texture with optional array layers.
    /// </summary>
    Texture2D,
    /// <summary>
    /// Three-dimensional volume texture.
    /// </summary>
    Texture3D,
    /// <summary>
    /// Cubemap texture with optional cubemap array layers.
    /// </summary>
    Cube
}
