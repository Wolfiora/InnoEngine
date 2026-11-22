namespace Inno.Rendering;

/// <summary>
/// Controls how existing attachment contents enter a raster pass.
/// </summary>
public enum RenderLoadAction
{
    /// <summary>
    /// Preserves prior attachment contents.
    /// </summary>
    Load,
    /// <summary>
    /// Clears the attachment before rendering.
    /// </summary>
    Clear,
    /// <summary>
    /// Does not require prior attachment contents.
    /// </summary>
    Discard
}
