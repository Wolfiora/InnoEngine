using Inno.Core.Graphs;
using Inno.Core.Serialization;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Declares one stable, typed port on a graph-authored node interface.
/// </summary>
public sealed class ShaderGraphNodePortDefinition : ISerializable
{
    /// <summary>
    /// Gets or sets the stable node-local port identity.
    /// </summary>
    [SerializableProperty]
    public string id { get; set; } = "value";

    /// <summary>
    /// Gets or sets the complete backend-neutral value type.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphType type { get; set; } = new() { id = "float" };

    /// <summary>
    /// Gets or sets whether callers must connect the input instead of using an explicit/default zero.
    /// </summary>
    [SerializableProperty]
    public bool required { get; set; } = true;

    internal ShaderNodePort ToPort(GraphPortDirection direction)
        => new(id, type.CreateType(), direction, direction == GraphPortDirection.Input && required);
}
