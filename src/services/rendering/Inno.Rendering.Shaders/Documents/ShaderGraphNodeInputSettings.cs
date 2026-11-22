using Inno.Core.Serialization;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Stores values supplied by callers to a graph-authored node.
/// </summary>
public sealed class ShaderGraphNodeInputSettings : ISerializable
{
    /// <summary>
    /// Gets or sets values supplied by callers and exposed as outputs inside the node graph.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphNodePortDefinition[] ports { get; set; } = [];
}
