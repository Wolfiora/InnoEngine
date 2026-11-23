using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Supplies one node's connected values and the region builder during lowering only.
/// </summary>
public sealed class ShaderNodeLoweringContext
{
    internal ShaderNodeLoweringContext(
        ShaderNodeDescriptionContext description,
        ShaderIrBuilder builder,
        IReadOnlyDictionary<string, ShaderIrValue> inputs
    ) {
        this.description = description;
        this.builder = builder;
        this.inputs = new ReadOnlyDictionary<string, ShaderIrValue>(new Dictionary<string, ShaderIrValue>(inputs, StringComparer.Ordinal));
    }

    /// <summary>
    /// Gets the invocation-scoped node description.
    /// </summary>
    public ShaderNodeDescriptionContext description { get; }

    /// <summary>
    /// Gets the builder shared by nodes in this typed region.
    /// </summary>
    public ShaderIrBuilder builder { get; }

    /// <summary>
    /// Gets connected values keyed by semantic input port identity, never connection order.
    /// </summary>
    public IReadOnlyDictionary<string, ShaderIrValue> inputs { get; }

    /// <summary>
    /// Requires one connected input without implicit defaults or positional rebinding.
    /// </summary>
    /// <param name="id">
    /// Exact semantic input port identity.
    /// </param>
    /// <returns>
    /// The connected typed value.
    /// </returns>
    public ShaderIrValue Input(string id)
        => inputs.TryGetValue(id, out ShaderIrValue? value)
           ? value
           : throw new InvalidOperationException($"Input '{id}' is not connected.");
}
