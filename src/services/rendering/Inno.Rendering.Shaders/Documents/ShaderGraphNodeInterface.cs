using System.Collections.Generic;
using System.Linq;
using Inno.Core.Graphs;
using Inno.Core.Serialization;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Freezes the complete public interface resolved from one graph-authored node asset.
/// </summary>
public sealed class ShaderGraphNodeInterface : ISerializable
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
    /// Gets or sets whether the graph is inline computation or a Target-owned output boundary.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphNodeKind kind { get; set; }

    /// <summary>
    /// Gets or sets whether an inline function is pure or intentionally emits ordered effects.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphNodeEffect effect { get; set; }

    /// <summary>
    /// Gets or sets the Target-owned role for a domain output.
    /// </summary>
    [SerializableProperty]
    public string role { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets externally supplied inputs.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphNodePortDefinition[] inputs { get; set; } = [];

    /// <summary>
    /// Gets or sets externally visible results.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphNodePortDefinition[] outputs { get; set; } = [];

    /// <summary>
    /// Gets detached typed ports in deterministic input-then-output order.
    /// </summary>
    /// <returns>
    /// A provider-free port snapshot suitable for Editor presentation and validation.
    /// </returns>
    public IReadOnlyList<ShaderNodePort> GetPorts()
        => inputs.Select(static value => value.ToPort(GraphPortDirection.Input))
            .Concat(outputs.Select(static value => value.ToPort(GraphPortDirection.Output)))
            .ToArray();
}
