using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inno.Rendering.Runtime;

/// <summary>
/// States the exact model order and exclusive world-content assignment for one output.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("313e9d03-9423-53a6-8dd8-f4faad999439")]
public sealed class RenderOutputRoute
{
    /// <summary>
    /// Creates a route from explicitly assigned model layers in draw order.
    /// </summary>
    /// <param name="layers">
    /// Exact model identities and exclusive source assignments in draw order.
    /// </param>
    public RenderOutputRoute(IEnumerable<RenderOutputLayer> layers)
    {
        ArgumentNullException.ThrowIfNull(layers);
        RenderOutputLayer[] values = layers.ToArray();
        if (values.Length == 0 || values.Any(static layer => layer is null)
            || values.Select(static layer => layer.modelId).Distinct(StringComparer.Ordinal).Count() != values.Length)
            throw new ArgumentException("A route requires distinct model layers.", nameof(layers));
        string[] sourceIds = values.SelectMany(static layer => layer.sourceIds).ToArray();
        if (sourceIds.Distinct(StringComparer.Ordinal).Count() != sourceIds.Length)
            throw new ArgumentException("Each world-content source may be assigned to only one model layer.", nameof(layers));
        this.layers = new ReadOnlyCollection<RenderOutputLayer>(values);
    }

    /// <summary>
    /// Gets exact model and source assignments in draw order.
    /// </summary>
    public IReadOnlyList<RenderOutputLayer> layers { get; }
}

