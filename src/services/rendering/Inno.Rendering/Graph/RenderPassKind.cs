namespace Inno.Rendering;

/// <summary>
/// Distinguishes pass command domains for validation and backend execution.
/// </summary>
public enum RenderPassKind
{
    /// <summary>
    /// Raster draw commands with optional attachments.
    /// </summary>
    Raster,
    /// <summary>
    /// Compute dispatch commands.
    /// </summary>
    Compute,
    /// <summary>
    /// Resource copy commands.
    /// </summary>
    Copy
}
