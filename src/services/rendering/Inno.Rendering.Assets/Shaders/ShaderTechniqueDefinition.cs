using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Declares one contract-compatible pass mapping selectable by materials.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("461d82ac-0279-5044-b17b-776085f2ba35")]
public struct ShaderTechniqueDefinition
{
    /// <summary>
    /// Creates a shader technique definition.
    /// </summary>
    /// <param name="id">
    /// Stable technique identifier.
    /// </param>
    /// <param name="contract">
    /// Open rendering-provider contract.
    /// </param>
    /// <param name="passes">
    /// Role-to-pass mappings.
    /// </param>
    /// <param name="requiredFeatures">
    /// Capabilities required by the complete technique.
    /// </param>
    public ShaderTechniqueDefinition(
        ShaderTechniqueId id,
        ShaderContractId contract,
        IEnumerable<ShaderTechniquePass> passes,
        GraphicsCapability requiredFeatures = GraphicsCapability.None
    ) {
        if (!id.isValid)
            throw new ArgumentException("A technique ID must be valid.", nameof(id));
        if (!contract.isValid)
            throw new ArgumentException("A shader contract ID must be valid.", nameof(contract));
        ArgumentNullException.ThrowIfNull(passes);
        this.id = id;
        this.contract = contract;
        this.passes = passes.ToArray();
        this.requiredFeatures = requiredFeatures;
    }

    /// <summary>
    /// Gets or sets the stable technique identifier.
    /// </summary>
    public ShaderTechniqueId id { get; set; }

    /// <summary>
    /// Gets or sets the open rendering-provider contract.
    /// </summary>
    public ShaderContractId contract { get; set; }

    /// <summary>
    /// Gets or sets role-to-pass mappings.
    /// </summary>
    public ShaderTechniquePass[] passes { get; set; }

    /// <summary>
    /// Gets or sets capabilities required by the complete technique.
    /// </summary>
    public GraphicsCapability requiredFeatures { get; set; }
}

