using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Stores all compiled stages and state for one shader pass.
/// </summary>
[StableTypeID("6a66aac0-d44d-5d90-a801-d92605a92c91")]
public sealed class CompiledShaderPass
{
    private readonly ShaderPassDefinition m_definition;

    /// <summary>
    /// Creates a compiled pass artifact.
    /// </summary>
    /// <param name="definition">
    /// Stable pass definition.
    /// </param>
    /// <param name="stages">
    /// Compiled target stages.
    /// </param>
    /// <param name="shaderInterface">
    /// Pass-local manifest binding contract.
    /// </param>
    public CompiledShaderPass(
        ShaderPassDefinition definition,
        IReadOnlyList<ShaderStageArtifact> stages,
        ShaderInterface shaderInterface
    ) {
        ArgumentNullException.ThrowIfNull(stages);
        ArgumentNullException.ThrowIfNull(shaderInterface);
        definition.metadata = definition.metadata?.ToArray() ?? [];
        m_definition = definition;
        this.stages = Array.AsReadOnly(stages.ToArray());
        this.shaderInterface = shaderInterface;
    }

    /// <summary>
    /// Gets the stable pass definition.
    /// </summary>
    public ShaderPassDefinition definition
    {
        get
        {
            ShaderPassDefinition value = m_definition;
            value.metadata = value.metadata.ToArray();
            return value;
        }
    }

    /// <summary>
    /// Gets compiled target stages.
    /// </summary>
    public IReadOnlyList<ShaderStageArtifact> stages { get; }

    /// <summary>
    /// Gets the pass-local manifest binding contract.
    /// </summary>
    public ShaderInterface shaderInterface { get; }
}
