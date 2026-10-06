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
/// Declares one static shader keyword that may produce compiled variants.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("551868bd-dbca-5da5-9dd8-d7e6e2ca7983")]
public struct ShaderKeywordDefinition
{
    /// <summary>
    /// Creates a shader keyword definition.
    /// </summary>
    /// <param name="id">
    /// Stable keyword identifier.
    /// </param>
    /// <param name="options">
    /// Allowed stable option identifiers.
    /// </param>
    public ShaderKeywordDefinition(
        string id,
        IEnumerable<string> options
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(options);
        this.id = id;
        this.options = options.ToArray();
    }

    /// <summary>
    /// Gets or sets the stable keyword identifier.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets allowed stable option identifiers.
    /// </summary>
    public string[] options { get; set; }
}

