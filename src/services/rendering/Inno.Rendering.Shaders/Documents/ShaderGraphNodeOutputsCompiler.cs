using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Describes the multi-port returned values while editing a graph-authored function node asset.
/// </summary>
public sealed class ShaderGraphNodeOutputsCompiler : IShaderNodeCompiler
{
    /// <summary>
    /// Gets the definition id text used by the current instance.
    /// </summary>
    public string definitionID => ShaderGraphNodes.C_OUTPUT_DEFINITION_ID;

    /// <summary>
    /// Gets a ports required by the implemented contract.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public IReadOnlyList<ShaderNodePort> GetPorts(ShaderNodeDescriptionContext context)
        => context.Read(ShaderGraphDocument.C_SETTINGS_KEY, new ShaderGraphNodeOutputSettings()).ports
            .Select(static value => value.ToPort(GraphPortDirection.Input))
            .ToArray();

    /// <summary>
    /// Lowers this graph node to typed shader IR after validating its inputs.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public IReadOnlyDictionary<string, ShaderIrValue> Lower(ShaderNodeLoweringContext context)
        => throw new InvalidOperationException("Node Outputs exist only inside a graph-authored node definition.");
}
