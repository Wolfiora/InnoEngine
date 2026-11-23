using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Freezes the graph-lowered stages of one material-selectable GPU pass.
/// </summary>
public sealed class ShaderGraphPass
{
    /// <summary>
    /// Captures a pass's ordered typed stages.
    /// </summary>
    /// <param name="name">
    /// Exact pass identity in the program contract.
    /// </param>
    /// <param name="stages">
    /// Complete raster or compute stage set.
    /// </param>
    public ShaderGraphPass(
        string name,
        IEnumerable<ShaderIrStage> stages
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(stages);
        this.name = name;
        this.stages = Array.AsReadOnly(stages.ToArray());
    }

    /// <summary>
    /// Gets the stable pass identity.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets immutable graph-lowered stages.
    /// </summary>
    public IReadOnlyList<ShaderIrStage> stages { get; }
}
