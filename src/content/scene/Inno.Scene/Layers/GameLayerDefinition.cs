using System;
using Inno.Core.Settings;

namespace Inno.Scene.Layers;

/// <summary>
/// Describes one named layer slot in a <see cref="GameLayerCatalog"/> snapshot.
/// </summary>
public sealed class GameLayerDefinition
{
    /// <summary>
    /// Creates an immutable layer definition.
    /// </summary>
    /// <param name="localID">
    /// The stable project-independent identity.
    /// </param>
    /// <param name="layer">
    /// The layer slot represented by the definition.
    /// </param>
    /// <param name="name">
    /// The non-empty display and lookup name.
    /// </param>
    public GameLayerDefinition(
        ProjectLocalID localID,
        GameLayer layer,
        string name
    ) {
        if (string.IsNullOrEmpty(localID.value))
        {
            throw new ArgumentException("A layer definition requires a valid local identity.", nameof(localID));
        }
        this.localID = localID;
        this.layer = layer;
        this.name = GameLayerCatalog.NormalizeName(name);
    }

    /// <summary>
    /// Gets the stable project-independent identity.
    /// </summary>
    public ProjectLocalID localID { get; }

    /// <summary>
    /// Gets the layer slot represented by this definition.
    /// </summary>
    public GameLayer layer { get; }

    /// <summary>
    /// Gets the unique ordinal layer name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Resolves the complete identity under a project namespace.
    /// </summary>
    /// <param name="projectID">
    /// The current project namespace.
    /// </param>
    /// <returns>
    /// The canonical project-scoped layer identity.
    /// </returns>
    public GameLayerID GetID(ProjectID projectID) => new(projectID, localID);
}
