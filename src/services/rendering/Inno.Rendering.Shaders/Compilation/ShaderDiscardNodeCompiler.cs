using System.Collections.Generic;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

internal sealed class ShaderDiscardNodeCompiler : IShaderNodeCompiler
{
    /// <summary>
    /// Gets the definition id text used by the current instance.
    /// </summary>
public string definitionID => "inno.shader.discard";

    /// <summary>
    /// Retrieves the current ports from authoritative state.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
public IReadOnlyList<ShaderNodePort> GetPorts(ShaderNodeDescriptionContext context)
    => [
        new ShaderNodePort("condition", ShaderSourceType.Atomic("bool"), GraphPortDirection.Input),
        new ShaderNodePort("after", ShaderSourceType.Atomic("bool"), GraphPortDirection.Input, false),
        new ShaderNodePort("then", ShaderSourceType.Atomic("bool"), GraphPortDirection.Output)
    ];

    /// <summary>
    /// Lowers this graph node to typed shader IR after validating its inputs.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
public IReadOnlyDictionary<string, ShaderIrValue> Lower(ShaderNodeLoweringContext context)
    {
        context.builder.Discard(context.Input("condition"));
        return new Dictionary<string, ShaderIrValue> { ["then"] = context.builder.Constant(true) };
    }
}
