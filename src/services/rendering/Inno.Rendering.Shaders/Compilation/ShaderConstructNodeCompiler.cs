using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Constructs a vector or column-major matrix from individually connected scalar components.
/// </summary>
public sealed class ShaderConstructNodeCompiler : IShaderNodeCompiler
{
    /// <summary>
    /// Gets the definition id text used by the current instance.
    /// </summary>
    public string definitionID => "inno.shader.construct";

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
        (ShaderSourceType type, ShaderSourceType scalar, int count) = Shape(context);
        return Enumerable.Range(0, count)
            .Select(index => new ShaderNodePort("component." + index, scalar, GraphPortDirection.Input))
            .Append(new ShaderNodePort("value", type, GraphPortDirection.Output))
            .ToArray();
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
    {
        (ShaderSourceType type, _, int count) = Shape(context.description);
        return new Dictionary<string, ShaderIrValue>
        {
            ["value"] = context.builder.Construct(
                type,
                Enumerable.Range(0, count).Select(index => context.Input("component." + index)).ToArray()
            )
        };
    }

    private static (ShaderSourceType type, ShaderSourceType scalar, int count) Shape(ShaderNodeDescriptionContext context)
    {
        string id = context.Read("type", "float4");
        foreach (string scalar in new[] { "float", "int", "uint", "bool" })
        {
            if (id.Length == scalar.Length + 1 && id.StartsWith(scalar, StringComparison.Ordinal) && id[^1] is >= '2' and <= '4')
            {
                return (ShaderSourceType.Atomic(id), ShaderSourceType.Atomic(scalar), id[^1] - '0');
            }
        }
        if (id.Length == 8
            && id.StartsWith("float", StringComparison.Ordinal)
            && id[5] is >= '2' and <= '4'
            && id[6] == 'x'
            && id[7] is >= '2' and <= '4'
        ) {
            return (ShaderSourceType.Atomic(id), ShaderSourceType.Atomic("float"), (id[5] - '0') * (id[7] - '0'));
        }
        throw new InvalidOperationException("Construction requires a numeric vector or floating-point matrix.");
    }
}
