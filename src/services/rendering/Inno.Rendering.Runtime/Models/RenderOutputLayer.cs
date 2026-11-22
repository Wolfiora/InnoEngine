using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inno.Extensibility.Types;
using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Assigns exactly one rendering model and its world-content sources to an output layer.
/// </summary>
[StableTypeID("7bdfe53d-72a0-5e9a-a922-fbc52362618f")]
public sealed class RenderOutputLayer
{
    /// <summary>
    /// Creates a model layer with explicit, distinct content-source IDs.
    /// </summary>
    /// <param name="modelID">
    /// Stable render-model or Editor contributor ID.
    /// </param>
    /// <param name="sourceIDs">
    /// World-content sources exclusively owned by this layer.
    /// </param>
    public RenderOutputLayer(
        string modelID,
        IEnumerable<string> sourceIDs
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(modelID);
        ArgumentNullException.ThrowIfNull(sourceIDs);
        string[] ids = sourceIDs.ToArray();
        if (ids.Any(string.IsNullOrWhiteSpace)
            || ids.Distinct(StringComparer.Ordinal).Count() != ids.Length
        ) {
            throw new ArgumentException("A layer requires distinct non-empty source IDs.", nameof(sourceIDs));
        }
        this.modelID = modelID;
        this.sourceIDs = new ReadOnlyCollection<string>(ids);
    }

    /// <summary>
    /// Gets the exact rendering model ID.
    /// </summary>
    public string modelID { get; }

    /// <summary>
    /// Gets the assigned world-content source IDs.
    /// </summary>
    public IReadOnlyList<string> sourceIDs { get; }
}
