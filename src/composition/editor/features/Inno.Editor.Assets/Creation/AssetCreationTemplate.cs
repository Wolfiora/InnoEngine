using System;
using Inno.Assets;

namespace Inno.Editor.Assets;

/// <summary>
/// Produces the detached initial value for one authorable native asset source.
/// </summary>
/// <remarks>
/// Derivation is the discovery contract. <see cref="AssetCreationMenuAttribute"/> supplies only
/// source-format and presentation data that the asset inheritance hierarchy cannot express.
/// </remarks>
public abstract class AssetCreationTemplate
{
    /// <summary>
    /// Gets the concrete asset type produced by this template.
    /// </summary>
    public abstract Type assetType { get; }

    /// <summary>
    /// Creates a detached default value that has not been imported or published.
    /// </summary>
    /// <returns>
    /// A new asset value owned by the caller.
    /// </returns>
    public abstract AssetObject Create();

    /// <summary>
    /// Encodes the detached initial value into the source representation owned by this template.
    /// </summary>
    /// <param name="context">
    /// Creation services scoped to the current Asset source pipeline.
    /// </param>
    /// <param name="asset">
    /// The exact value returned by <see cref="Create"/>.
    /// </param>
    /// <returns>
    /// Complete source bytes ready for an atomic File Browser create operation.
    /// </returns>
    /// <remarks>
    /// The default implementation uses native structured Asset serialization. Templates paired
    /// with a textual or custom binary importer can override this method without changing the
    /// File Browser or registering a second creation protocol.
    /// </remarks>
    public virtual byte[] Encode(
        AssetCreationContext context,
        AssetObject asset
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(asset);
        return context.EncodeNative(asset);
    }
}

/// <summary>
/// Creates a default-constructed native source for an authorable asset type.
/// </summary>
/// <typeparam name="TAsset">
/// Concrete serializable asset type.
/// </typeparam>
public abstract class AssetCreationTemplate<TAsset> : AssetCreationTemplate
    where TAsset : AssetObject, new()
{
    /// <summary>
    /// Gets the asset type created by this Editor template.
    /// </summary>
    public sealed override Type assetType => typeof(TAsset);

    /// <summary>
    /// Creates a value using this implementation's validated inputs.
    /// </summary>
    /// <returns>
    /// The validated asset object that represents the completed operation.
    /// </returns>
    public override AssetObject Create() => CreateAsset();

    /// <summary>
    /// Creates the initial detached value, allowing specialized deterministic defaults.
    /// </summary>
    /// <returns>
    /// A new asset value owned by the caller.
    /// </returns>
    protected virtual TAsset CreateAsset() => new();
}
