using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Assigns exactly one rendering model and its world-content sources to an output layer.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("7bdfe53d-72a0-5e9a-a922-fbc52362618f")]
public sealed class RenderOutputLayer
{
    /// <summary>
    /// Creates a model layer with explicit, distinct content-source IDs.
    /// </summary>
    /// <param name="modelId">
    /// Stable render-model or Editor contributor ID.
    /// </param>
    /// <param name="sourceIds">
    /// World-content sources exclusively owned by this layer.
    /// </param>
    public RenderOutputLayer(
        string modelId,
        IEnumerable<string> sourceIds
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(modelId);
        ArgumentNullException.ThrowIfNull(sourceIds);
        string[] ids = sourceIds.ToArray();
        if (ids.Any(string.IsNullOrWhiteSpace)
            || ids.Distinct(StringComparer.Ordinal).Count() != ids.Length)
            throw new ArgumentException("A layer requires distinct non-empty source IDs.", nameof(sourceIds));
        this.modelId = modelId;
        this.sourceIds = new ReadOnlyCollection<string>(ids);
    }

    /// <summary>
    /// Gets the exact rendering model ID.
    /// </summary>
    public string modelId { get; }
    /// <summary>
    /// Gets the assigned world-content source IDs.
    /// </summary>
    public IReadOnlyList<string> sourceIds { get; }
}

