using System;
using System.Collections.Generic;
using Inno.Assets;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Provides a stable input snapshot to an aggregate asset build.
/// </summary>
/// <typeparam name="TDefinition">
/// The build definition asset type.
/// </typeparam>
public sealed class AssetBuildContext<TDefinition>
    where TDefinition : AssetObject
{
    /// <summary>
    /// Creates a build context.
    /// </summary>
    /// <param name="definition">
    /// The build definition.
    /// </param>
    /// <param name="inputs">
    /// The immutable input catalog snapshots.
    /// </param>
    public AssetBuildContext(
        TDefinition definition,
        IReadOnlyList<AssetInfo> inputs
    ) {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(inputs);
        this.definition = definition;
        this.inputs = inputs;
    }

    /// <summary>
    /// Gets the build definition.
    /// </summary>
    public TDefinition definition { get; }

    /// <summary>
    /// Gets the input catalog snapshots.
    /// </summary>
    public IReadOnlyList<AssetInfo> inputs { get; }
}
