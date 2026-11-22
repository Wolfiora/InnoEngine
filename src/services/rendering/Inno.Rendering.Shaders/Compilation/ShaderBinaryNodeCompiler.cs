using System;
using System.Collections.Generic;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Lowers an explicit arithmetic/comparison operation; operation IDs are node configuration, not backend code.
/// </summary>
public sealed class ShaderBinaryNodeCompiler : IShaderNodeCompiler
{
    /// <summary>
    /// Gets the definition id text used by the current instance.
    /// </summary>
    public string definitionID => "inno.shader.binary";

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
        ShaderIrOperation operation = Operation(context);
        return [
            new ShaderNodePort("left", type, GraphPortDirection.Input),
            new ShaderNodePort("right", type, GraphPortDirection.Input),
            new ShaderNodePort(
                "value",
                operation is ShaderIrOperation.Equal or ShaderIrOperation.LessThan ? ShaderSourceType.Atomic("bool") : type,
                GraphPortDirection.Output
            )
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
        ["value"] = context.builder.Binary(Operation(context.description), context.Input("left"), context.Input("right"))
    };

    private static ShaderIrOperation Operation(ShaderNodeDescriptionContext context)
        => context.Read("operation", "add") switch
    {
        "add" => ShaderIrOperation.Add,
        "subtract" => ShaderIrOperation.Subtract,
        "multiply" => ShaderIrOperation.Multiply,
        "divide" => ShaderIrOperation.Divide,
        "minimum" => ShaderIrOperation.Minimum,
        "maximum" => ShaderIrOperation.Maximum,
        "equal" => ShaderIrOperation.Equal,
        "less-than" => ShaderIrOperation.LessThan,
        _ => throw new InvalidOperationException("Unknown binary shader operation.")
    };
}
