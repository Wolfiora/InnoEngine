using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Describes one capability-compatible material pass selection.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("e4a37a17-bfc1-5b29-9c57-f7b387271f9f")]
public sealed class MaterialPassResolution
{
    private readonly ShaderTechniqueDefinition m_technique;
    private readonly ShaderPassDefinition m_pass;

    /// <summary>
    /// Creates a material pass resolution.
    /// </summary>
    /// <param name="technique">
    /// Selected open-contract technique.
    /// </param>
    /// <param name="pass">
    /// Concrete pass mapped to the requested role.
    /// </param>
    public MaterialPassResolution(
        ShaderTechniqueDefinition technique,
        ShaderPassDefinition pass
    ) {
        m_technique = ShaderDefinitionSnapshot.Copy(technique);
        m_pass = ShaderDefinitionSnapshot.Copy(pass);
    }

    /// <summary>
    /// Gets the selected technique with detached role mapping storage.
    /// </summary>
    public ShaderTechniqueDefinition technique => ShaderDefinitionSnapshot.Copy(m_technique);

    /// <summary>
    /// Gets the concrete shader pass with detached metadata storage.
    /// </summary>
    public ShaderPassDefinition pass => ShaderDefinitionSnapshot.Copy(m_pass);
}

