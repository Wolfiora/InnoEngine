namespace Inno.Rendering;

/// <summary>
/// Controls whether attachment contents remain valid after a raster pass.
/// </summary>
public enum RenderStoreAction
{
    /// <summary>
    /// Preserves rendered contents.
    /// </summary>
    Store,
    /// <summary>
    /// Allows the backend to discard rendered contents.
    /// </summary>
    Discard
}
