using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;

namespace Inno.Rendering.Assets;

/// <summary>
/// Resolves materials through open provider-owned shader contracts and roles.
/// </summary>
[StableTypeID("dcf92cc9-04c6-52cd-8fd3-b2dc48f73b5a")]
public static class MaterialPassResolver
{
    /// <summary>
    /// Resolves one capability-compatible pass.
    /// </summary>
    /// <param name="material">
    /// Material whose shader and technique should be queried.
    /// </param>
    /// <param name="contractID">
    /// Rendering-provider contract required by the caller.
    /// </param>
    /// <param name="passRoleID">
    /// Provider-defined pass role required by the caller.
    /// </param>
    /// <param name="capabilities">
    /// Current device capability snapshot.
    /// </param>
    /// <returns>
    /// The selected technique and pass, or <see langword="null"/> when no compatible mapping exists.
    /// </returns>
    public static MaterialPassResolution? Resolve(
        MaterialAsset material,
        ShaderContractID contractID,
        ShaderPassRoleID passRoleID,
        GraphicsCapabilities capabilities
    ) {
        ArgumentNullException.ThrowIfNull(material);
        ShaderDefinition? definition = material.shader?.definition;
        return definition is null ? null : Resolve(definition, material.techniqueId, contractID, passRoleID, capabilities);
    }

    /// <summary>
    /// Resolves a role against an exact published shader contract rather than a possibly newer authoring asset.
    /// </summary>
    /// <param name="definition">
    /// The immutable publication's detached material contract.
    /// </param>
    /// <param name="techniqueID">
    /// Explicit material technique, or an invalid ID to select the first compatible technique.
    /// </param>
    /// <param name="contractID">
    /// Open rendering contract required by the caller.
    /// </param>
    /// <param name="passRoleID">
    /// Open pass role required by the caller.
    /// </param>
    /// <param name="capabilities">
    /// Current target capability snapshot.
    /// </param>
    /// <returns>
    /// The matching technique and pass, or null when this publication has no compatible mapping.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// A required contract or role identity is invalid.
    /// </exception>
    public static MaterialPassResolution? Resolve(
        ShaderDefinition definition,
        ShaderTechniqueID techniqueID,
        ShaderContractID contractID,
        ShaderPassRoleID passRoleID,
        GraphicsCapabilities capabilities
    ) {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(capabilities);
        if (!contractID.isValid)
        {
            throw new ArgumentException("A shader contract ID must be valid.", nameof(contractID));
        }
        if (!passRoleID.isValid)
        {
            throw new ArgumentException("A shader pass role ID must be valid.", nameof(passRoleID));
        }

        foreach (ShaderTechniqueDefinition technique in definition.techniques)
        {
            if (technique.contract != contractID || !capabilities.Supports(technique.requiredFeatures)
                || techniqueID.isValid && technique.id != techniqueID
            ) {
                    continue;
            }
            foreach (ShaderTechniquePass mapping in technique.passes)
            {
                if (mapping.role != passRoleID)
                {
                    continue;
                }
                foreach (ShaderPassDefinition pass in definition.passes)
                {
                    if (string.Equals(pass.name, mapping.passName, StringComparison.Ordinal)
                        && capabilities.Supports(pass.requiredFeatures)
                    ) {
                        return new MaterialPassResolution(technique, pass);
                    }
                }
            }
        }

        return null;
    }
}
