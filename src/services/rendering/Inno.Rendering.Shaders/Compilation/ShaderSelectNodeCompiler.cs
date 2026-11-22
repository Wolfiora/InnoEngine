using System.Collections.Generic;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Selects between equal typed values; all producer effects remain evaluated before selection.
/// </summary>
public sealed class ShaderSelectNodeCompiler : IShaderNodeCompiler
{
    /// <summary>
    /// Gets the definition id text used by the current instance.
    /// </summary>
    public string definitionID => "inno.shader.select";

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
    {
        ShaderSourceType type = ShaderSourceType.Atomic(context.Read("type", "float"));
        return [
            new ShaderNodePort("condition", ShaderSourceType.Atomic("bool"), GraphPortDirection.Input),
            new ShaderNodePort("true", type, GraphPortDirection.Input),
            new ShaderNodePort("false", type, GraphPortDirection.Input),
            new ShaderNodePort("value", type, GraphPortDirection.Output)
        ];
    }

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
        => new Dictionary<string, ShaderIrValue>
    {
        ["value"] = context.builder.Select(context.Input("condition"), context.Input("true"), context.Input("false"))
    };
}
