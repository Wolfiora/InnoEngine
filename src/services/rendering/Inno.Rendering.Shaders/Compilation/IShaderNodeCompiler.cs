using System.Collections.Generic;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Defines one shader node's typed ports and lowering; drawing belongs to a separate editor extension.
/// </summary>
public interface IShaderNodeCompiler
{
    /// <summary>
    /// Gets the exact graph node definition identity implemented by this compiler.
    /// </summary>
    string definitionID { get; }

    /// <summary>
    /// Describes ports for the current neutral properties and resolved source/target inputs.
    /// </summary>
    /// <param name="context">
    /// Invocation-scoped node properties and frozen source/target data.
    /// </param>
    /// <returns>
    /// Complete typed ports; duplicate identities are invalid.
    /// </returns>
    IReadOnlyList<ShaderNodePort> GetPorts(ShaderNodeDescriptionContext context);

    /// <summary>
    /// Lowers the node into the supplied typed builder without generating source strings.
    /// </summary>
    /// <param name="context">
    /// Connected typed inputs and the shared region builder.
    /// </param>
    /// <returns>
    /// Exactly the declared output ports by stable identity.
    /// </returns>
    IReadOnlyDictionary<string, ShaderIrValue> Lower(ShaderNodeLoweringContext context);
}
