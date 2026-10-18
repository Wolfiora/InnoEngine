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
/// Defines one backend-neutral raster or compute shader pass.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("0f50b92b-4d4b-55f8-a954-953630083303")]
public struct ShaderPassDefinition
{
    /// <summary>
    /// Creates a shader pass definition.
    /// </summary>
    /// <param name="name">
    /// Stable pass name within the shader.
    /// </param>
    /// <param name="programKind">
    /// Programmable stage combination.
    /// </param>
    /// <param name="requiredFeatures">
    /// Required device capability mask.
    /// </param>
    /// <param name="renderState">
    /// Backend-neutral fixed-function state.
    /// </param>
    /// <param name="metadata">
    /// Optional provider-defined metadata.
    /// </param>
    public ShaderPassDefinition(
        string name,
        ShaderProgramKind programKind,
        GraphicsCapability requiredFeatures = GraphicsCapability.None,
        ShaderRenderState? renderState = null,
        IEnumerable<ShaderMetadataEntry>? metadata = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        this.name = name;
        this.programKind = programKind;
        this.requiredFeatures = requiredFeatures;
        this.renderState = renderState ?? ShaderRenderState.opaque;
        this.metadata = metadata?.ToArray() ?? [];
    }

    /// <summary>
    /// Gets or sets the stable pass name.
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// Gets or sets the programmable stage combination.
    /// </summary>
    public ShaderProgramKind programKind { get; set; }

    /// <summary>
    /// Gets or sets required device capabilities.
    /// </summary>
    public GraphicsCapability requiredFeatures { get; set; }

    /// <summary>
    /// Gets or sets backend-neutral fixed-function state.
    /// </summary>
    public ShaderRenderState renderState { get; set; }

    /// <summary>
    /// Gets or sets provider-defined metadata.
    /// </summary>
    public ShaderMetadataEntry[] metadata { get; set; }

    /// <summary>
    /// Copies the pass declaration with independently owned metadata storage.
    /// </summary>
    /// <returns>
    /// A detached declaration whose metadata can be changed without affecting this declaration.
    /// Missing metadata is represented by an empty collection.
    /// </returns>
    public ShaderPassDefinition Copy() => ShaderDefinitionSnapshot.Copy(this);
}

