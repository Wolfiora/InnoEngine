using Inno.Core.Serialization;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Stores graph-level identity and catalog metadata for a reusable Shader node.
/// </summary>
public sealed class ShaderGraphNodeSettings : ISerializable
{
    /// <summary>
    /// Gets or sets the node title displayed to authors.
    /// </summary>
    [SerializableProperty]
    public string displayName { get; set; } = "Graph Node";

    /// <summary>
    /// Gets or sets the slash-separated creation catalog beneath Graph Nodes.
    /// </summary>
    [SerializableProperty]
    public string createPath { get; set; } = "General";

    /// <summary>
    /// Gets or sets the deterministic order within the creation catalog.
    /// </summary>
    [SerializableProperty]
    public int createOrder { get; set; }

    /// <summary>
    /// Gets or sets how a reference to this graph participates in compilation.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphNodeKind kind { get; set; }

    /// <summary>
    /// Gets or sets the observable computation behavior of an inline function.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphNodeEffect effect { get; set; }

    /// <summary>
    /// Gets or sets the domain role consumed by a Target; empty for ordinary inline functions.
    /// </summary>
    [SerializableProperty]
    public string role { get; set; } = string.Empty;
}
