using System;
using System.Collections.Generic;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Describes graph-authored node references before they are expanded or consumed by a Target.
/// </summary>
public sealed class ShaderGraphCallNodeCompiler : IShaderNodeCompiler
{
    /// <summary>
    /// Gets the definition id text used by the current instance.
    /// </summary>
    public string definitionID => ShaderGraphNodes.C_CALL_DEFINITION_ID;

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
        => context.Read(ShaderGraphNodes.C_INTERFACE_KEY, new ShaderGraphNodeInterface()).GetPorts();

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
        => throw new InvalidOperationException("A graph-authored node must be expanded or consumed by its Shader Target before lowering.");
}
