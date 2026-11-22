using System;
using System.Collections.Generic;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Reads one target-assigned stage/resource input without choosing a native variable name.
/// </summary>
public sealed class ShaderStageInputNodeCompiler : IShaderNodeCompiler
{
    /// <summary>
    /// Gets the definition id text used by the current instance.
    /// </summary>
    public string definitionID => "inno.shader.stage-input";

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
        => [new ShaderNodePort("value", RequireBinding(context).type, GraphPortDirection.Output)];

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
        ShaderIrStageInput input = RequireBinding(context.description);
        return new Dictionary<string, ShaderIrValue> { ["value"] = context.builder.Input(input.id, input.type) };
    }

    private static ShaderIrStageInput RequireBinding(ShaderNodeDescriptionContext context)
        => context.stageInput ?? throw new InvalidOperationException("The target has not resolved this stage input binding.");
}
