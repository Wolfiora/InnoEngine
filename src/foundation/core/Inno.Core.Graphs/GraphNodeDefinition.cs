using System;
using System.Collections.Generic;

namespace Inno.Core.Graphs;

/// <summary>
/// Describes node presentation and resolves ports without entering graph persistence.
/// </summary>
public abstract class GraphNodeDefinition
{
    /// <summary>
    /// Creates a graph node definition.
    /// </summary>
    /// <param name="id">
    /// Globally stable definition identifier.
    /// </param>
    /// <param name="displayName">
    /// Artist-facing display name.
    /// </param>
    /// <param name="category">
    /// Search-menu category path.
    /// </param>
    protected GraphNodeDefinition(
        string id,
        string displayName,
        string category
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        this.id = id;
        this.displayName = displayName;
        this.category = category;
    }

    /// <summary>
    /// Gets the globally stable definition identifier.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the artist-facing display name.
    /// </summary>
    public string displayName { get; }

    /// <summary>
    /// Gets the search-menu category path.
    /// </summary>
    public string category { get; }

    /// <summary>
    /// Resolves ports for one node record, including any data-driven dynamic ports.
    /// </summary>
    /// <param name="node">
    /// Neutral node record to inspect.
    /// </param>
    /// <returns>
    /// The complete port definitions for the current node state.
    /// </returns>
    public abstract IReadOnlyList<GraphPortDefinition> GetPorts(GraphNodeRecord node);
}
