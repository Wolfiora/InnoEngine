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
/// Maps one provider-defined role to one concrete shader pass.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("5ca78007-1d3e-53a0-a5e3-dce6b61088b4")]
public struct ShaderTechniquePass
{
    /// <summary>
    /// Creates a technique pass mapping.
    /// </summary>
    /// <param name="role">
    /// Open role defined by the technique contract.
    /// </param>
    /// <param name="passName">
    /// Concrete pass name within the shader.
    /// </param>
    public ShaderTechniquePass(
        ShaderPassRoleId role,
        string passName
    ) {
        if (!role.isValid)
            throw new ArgumentException("A pass role must be valid.", nameof(role));
        ArgumentException.ThrowIfNullOrWhiteSpace(passName);
        this.role = role;
        this.passName = passName;
    }

    /// <summary>
    /// Gets or sets the provider-defined role.
    /// </summary>
    public ShaderPassRoleId role { get; set; }

    /// <summary>
    /// Gets or sets the concrete pass name.
    /// </summary>
    public string passName { get; set; }
}

