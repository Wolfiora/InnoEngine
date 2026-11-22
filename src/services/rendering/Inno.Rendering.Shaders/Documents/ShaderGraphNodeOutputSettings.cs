using Inno.Core.Serialization;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Stores values returned by a graph-authored function node.
/// </summary>
public sealed class ShaderGraphNodeOutputSettings : ISerializable
{
    /// <summary>
    /// Gets or sets values collected inside the node graph and exposed as outputs to callers.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphNodePortDefinition[] ports { get; set; } = [];
}
