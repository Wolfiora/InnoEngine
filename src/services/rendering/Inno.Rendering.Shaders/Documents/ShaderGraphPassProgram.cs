namespace Inno.Rendering.Shaders;

/// <summary>
/// References shared stage output identities from a material-selectable pass.
/// </summary>
public struct ShaderGraphPassProgram
{
    /// <summary>
    /// Gets or sets the exact pass identity in the runtime shader definition.
    /// </summary>
    public string pass { get; set; }

    /// <summary>
    /// Gets or sets stable stage output node identities; computation remains owned by those nodes.
    /// </summary>
    public string[] stages { get; set; }
}
