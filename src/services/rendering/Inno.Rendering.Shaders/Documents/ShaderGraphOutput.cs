namespace Inno.Rendering.Shaders;

/// <summary>
/// Stores one stable output port's destination, independent of adapter source syntax.
/// </summary>
public struct ShaderGraphOutput
{
    /// <summary>
    /// Gets or sets the stable input port identity on the stage output node.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the GPU destination category.
    /// </summary>
    public ShaderIrOutputKind kind { get; set; }

    /// <summary>
    /// Gets or sets the varying semantic; empty for fixed position, color or depth outputs.
    /// </summary>
    public string semantic { get; set; }

    /// <summary>
    /// Gets or sets the varying or attachment index.
    /// </summary>
    public int location { get; set; }
}
