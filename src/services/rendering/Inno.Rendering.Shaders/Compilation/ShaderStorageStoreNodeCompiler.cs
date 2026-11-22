using System.Collections.Generic;

namespace Inno.Rendering.Shaders;

internal sealed class ShaderStorageStoreNodeCompiler : IShaderNodeCompiler
{
    /// <summary>
    /// Gets the definition id text used by the current instance.
    /// </summary>
public string definitionID => "inno.shader.storage-store";

    /// <summary>
    /// Retrieves the current ports from authoritative state.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
public IReadOnlyList<ShaderNodePort> GetPorts(ShaderNodeDescriptionContext context) => StorageNodePorts.Describe(context, true, false);

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
        context.builder.StoreStorage(context.Input("resource"), context.Input("coordinate"), context.Input("value"));
        return new Dictionary<string, ShaderIrValue> { ["then"] = context.builder.Constant(true) };
    }
}
