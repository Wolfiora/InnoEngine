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
/// Contains the source-of-truth definition shared by shader import and material tooling.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("5bce7c89-203f-5921-8efc-1bca0f364943")]
public sealed class ShaderDefinition : ISerializable
{
    /// <summary>
    /// Creates an empty shader definition for native deserialization.
    /// </summary>
    public ShaderDefinition()
    {
    }

    /// <summary>
    /// Creates a shader definition.
    /// </summary>
    /// <param name="name">
    /// Artist-facing shader name.
    /// </param>
    /// <param name="properties">
    /// Stable property declarations.
    /// </param>
    /// <param name="keywords">
    /// Static keyword declarations.
    /// </param>
    /// <param name="passes">
    /// Backend-neutral pass declarations.
    /// </param>
    /// <param name="techniques">
    /// Open contract and role mappings.
    /// </param>
    public ShaderDefinition(
        string name,
        IEnumerable<ShaderPropertyDefinition> properties,
        IEnumerable<ShaderKeywordDefinition> keywords,
        IEnumerable<ShaderPassDefinition> passes,
        IEnumerable<ShaderTechniqueDefinition>? techniques = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentNullException.ThrowIfNull(keywords);
        ArgumentNullException.ThrowIfNull(passes);
        this.name = name;
        this.properties = properties.ToArray();
        this.keywords = keywords.ToArray();
        this.passes = passes.ToArray();
        this.techniques = techniques?.ToArray() ?? [];
    }

    /// <summary>
    /// Gets or sets the artist-facing shader name.
    /// </summary>
    [SerializableProperty]
    public string name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets stable property declarations.
    /// </summary>
    [SerializableProperty]
    public ShaderPropertyDefinition[] properties { get; set; } = [];

    /// <summary>
    /// Gets or sets static keyword declarations.
    /// </summary>
    [SerializableProperty]
    public ShaderKeywordDefinition[] keywords { get; set; } = [];

    /// <summary>
    /// Gets or sets backend-neutral pass declarations.
    /// </summary>
    [SerializableProperty]
    public ShaderPassDefinition[] passes { get; set; } = [];

    /// <summary>
    /// Gets or sets open contract and role mappings.
    /// </summary>
    [SerializableProperty]
    public ShaderTechniqueDefinition[] techniques { get; set; } = [];
}

