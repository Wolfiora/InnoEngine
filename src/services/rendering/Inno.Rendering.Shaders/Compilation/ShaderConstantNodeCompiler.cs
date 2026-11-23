using System;
using System.Collections.Generic;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Lowers an exact scalar constant; its type/value are native graph properties.
/// </summary>
public sealed class ShaderConstantNodeCompiler : IShaderNodeCompiler
{
    /// <summary>
    /// Gets the definition id text used by the current instance.
    /// </summary>
    public string definitionID => "inno.shader.constant";

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
        => [new ShaderNodePort("value", ShaderSourceType.Atomic(context.Read("type", "float")), GraphPortDirection.Output)];

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
    {
        ShaderNodeDescriptionContext node = context.description;
        ShaderIrValue value = node.Read("type", "float") switch
        {
            "float" => context.builder.Constant(node.Read("value", 0f)),
            "int" => context.builder.Constant(node.Read("value", 0)),
            "uint" => context.builder.Constant(node.Read("value", 0u)),
            "bool" => context.builder.Constant(node.Read("value", false)),
            _ => throw new InvalidOperationException("A scalar constant must declare float, int, uint or bool.")
        };
        return new Dictionary<string, ShaderIrValue> { ["value"] = value };
    }
}
