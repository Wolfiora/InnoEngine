using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Extensibility.Modules;
using Inno.Core.Identity;
using Inno.Core.Diagnostics;
using Inno.Core.Logging;
using Inno.Extensibility.Types;
using Inno.Scripting.Api;
using Inno.Core.Serialization;
using Inno.Core.Execution;
using Inno.Extensibility.Reload;

namespace Inno.Assets.Pipeline;

sealed partial class AssetPipeline
{
    /// <summary>
    /// Loads a canonical asset by isolated source path.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required asset type.
    /// </typeparam>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <returns>
    /// The canonical asset instance.
    /// </returns>
    public TAsset Load<TAsset>(AssetPath path) where TAsset : AssetObject
    {
        AssetObject? asset = GetLoader().Load(path, typeof(TAsset));
        return asset as TAsset ?? throw new InvalidOperationException(
            $"Asset '{path}' cannot be loaded as '{typeof(TAsset).FullName}'.");
    }

    /// <summary>
    /// Loads a canonical asset using a runtime asset type selected by an authoring workflow.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="assetType">
    /// The required concrete or base asset type.
    /// </param>
    /// <returns>
    /// The canonical asset instance assignable to <paramref name="assetType"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="assetType"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="assetType"/> does not derive from <see cref="AssetObject"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no compatible canonical asset can be loaded.
    /// </exception>
    [ScriptingApiIgnore]
    public AssetObject Load(
        AssetPath path,
        Type assetType
    ) {
        ArgumentNullException.ThrowIfNull(assetType);
        if (!typeof(AssetObject).IsAssignableFrom(assetType))
        {
            throw new ArgumentException(
                $"Asset type '{assetType.FullName}' must derive from '{typeof(AssetObject).FullName}'.",
                nameof(assetType));
        }
        return GetLoader().Load(path, assetType) ?? throw new InvalidOperationException(
            $"Asset '{path}' cannot be loaded as '{assetType.FullName}'.");
    }

    /// <summary>
    /// Loads a canonical asset by persistent identity.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required asset type.
    /// </typeparam>
    /// <param name="persistentId">
    /// The persistent asset identity.
    /// </param>
    /// <returns>
    /// The canonical asset instance.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no compatible asset can be loaded.
    /// </exception>
    public TAsset Load<TAsset>(Guid persistentId) where TAsset : AssetObject
    {
        AssetObject? asset = GetLoader().Load(persistentId, typeof(TAsset));
        return asset as TAsset ?? throw new InvalidOperationException(
            $"Asset '{persistentId}' cannot be loaded as '{typeof(TAsset).FullName}'.");
    }

    /// <summary>
    /// Tries to load a canonical asset by isolated source path.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required asset type.
    /// </typeparam>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="asset">
    /// The canonical asset when successful.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a compatible asset was loaded.
    /// </returns>
    public bool TryLoad<TAsset>(
        AssetPath path,
        out TAsset? asset
    ) where TAsset : AssetObject
    {
        bool success = GetLoader().TryLoad(path, typeof(TAsset), out AssetObject? value);
        asset = value as TAsset;
        return success && asset is not null;
    }

    /// <summary>
    /// Tries to load a canonical asset by persistent identity.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required asset type.
    /// </typeparam>
    /// <param name="persistentId">
    /// The persistent asset identity.
    /// </param>
    /// <param name="asset">
    /// The canonical asset when successful.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a compatible asset was loaded.
    /// </returns>
    public bool TryLoad<TAsset>(
        Guid persistentId,
        out TAsset? asset
    ) where TAsset : AssetObject
    {
        bool success = GetLoader().TryLoad(persistentId, typeof(TAsset), out AssetObject? value);
        asset = value as TAsset;
        return success && asset is not null;
    }

    /// <summary>
    /// Asynchronously loads a canonical asset by isolated source path.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required asset type.
    /// </typeparam>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation for the current caller's wait.
    /// </param>
    /// <returns>
    /// The canonical asset instance.
    /// </returns>
    public async ValueTask<TAsset> LoadAsync<TAsset>(
        AssetPath path,
        CancellationToken cancellationToken = default
    )
        where TAsset : AssetObject
    {
        AssetLoader loader = GetLoader();
        AssetObject? asset = await loader
            .LoadAsync(path, typeof(TAsset), cancellationToken)
            .ConfigureAwait(false);
        return asset as TAsset ?? throw new InvalidOperationException(
            $"Asset '{path}' cannot be loaded as '{typeof(TAsset).FullName}'.");
    }

    /// <summary>
    /// Asynchronously loads a canonical asset by persistent identity.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required asset type.
    /// </typeparam>
    /// <param name="persistentId">
    /// The persistent asset identity.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation for the current caller's wait.
    /// </param>
    /// <returns>
    /// The canonical asset instance.
    /// </returns>
    public async ValueTask<TAsset> LoadAsync<TAsset>(
        Guid persistentId,
        CancellationToken cancellationToken = default
    )
        where TAsset : AssetObject
    {
        AssetLoader loader = GetLoader();
        AssetObject? asset = await loader
            .LoadAsync(persistentId, typeof(TAsset), cancellationToken)
            .ConfigureAwait(false);
        return asset as TAsset ?? throw new InvalidOperationException(
            $"Asset '{persistentId}' cannot be loaded as '{typeof(TAsset).FullName}'.");
    }

    /// <summary>
    /// Asynchronously acquires a canonical authoring asset by path for an explicit managed lifetime.
    /// </summary>
    /// <typeparam name="TAsset">
    /// Required asset contract.
    /// </typeparam>
    /// <param name="path">
    /// Mount-qualified logical asset path.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation for this caller's asynchronous wait.
    /// </param>
    /// <returns>
    /// A lease whose strong reference participates in the existing reachability sweep.
    /// </returns>
    public async ValueTask<AssetLease<TAsset>> AcquireAsync<TAsset>(
        AssetPath path,
        CancellationToken cancellationToken = default
    )
        where TAsset : AssetObject
    {
        TAsset asset = await LoadAsync<TAsset>(path, cancellationToken).ConfigureAwait(false);
        return CreateAssetLease(asset, static () => { });
    }

    /// <summary>
    /// Asynchronously acquires a canonical authoring asset by persistent identity.
    /// </summary>
    /// <typeparam name="TAsset">
    /// Required asset contract.
    /// </typeparam>
    /// <param name="persistentId">
    /// Persistent asset identity.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation for this caller's asynchronous wait.
    /// </param>
    /// <returns>
    /// A lease whose strong reference participates in the existing reachability sweep.
    /// </returns>
    public async ValueTask<AssetLease<TAsset>> AcquireAsync<TAsset>(
        Guid persistentId,
        CancellationToken cancellationToken = default
    )
        where TAsset : AssetObject
    {
        TAsset asset = await LoadAsync<TAsset>(persistentId, cancellationToken).ConfigureAwait(false);
        return CreateAssetLease(asset, static () => { });
    }

    AssetObject IAssetReferenceResolver.Resolve(
        Guid persistentId,
        Guid stableTypeId,
        string lastKnownPath,
        Type expectedType,
        string propertyPath
    ) {
        try
        {
            return GetLoader().ResolveReference(
                persistentId,
                stableTypeId,
                lastKnownPath,
                expectedType);
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                $"Asset reference '{persistentId:D}' at '{propertyPath}' cannot be resolved as " +
                $"'{expectedType.FullName}'.",
                exception);
        }
    }

    /// <summary>
    /// Gets isolated source paths for all canonical loaded assets.
    /// </summary>
    /// <returns>
    /// A stable isolated path snapshot.
    /// </returns>
    public IReadOnlyList<AssetPath> GetLoadedPaths() => GetLoader().GetLoadedPaths();

    /// <summary>
    /// Gets direct or transitive runtime dependencies of an asset.
    /// </summary>
    /// <param name="asset">
    /// The asset to query.
    /// </param>
    /// <param name="recursive">
    /// Whether transitive dependencies should be included.
    /// </param>
    /// <returns>
    /// The dependency descriptors.
    /// </returns>
    public IReadOnlyList<AssetDependency> GetDependencies(
        AssetObject asset,
        bool recursive = false
    )
        => GetLoader().GetDependencies(asset, recursive);

    /// <summary>
    /// Gets source import dependencies that invalidate an asset artifact.
    /// </summary>
    /// <param name="asset">
    /// The asset to query.
    /// </param>
    /// <param name="recursive">
    /// Whether transitive source dependencies should be included.
    /// </param>
    /// <returns>
    /// Canonical isolated source paths in stable order.
    /// </returns>
    public IReadOnlyList<AssetPath> GetImportDependencies(
        AssetObject asset,
        bool recursive = false
    )
        => GetLoader().GetImportDependencies(asset, recursive);

    /// <summary>
    /// Gets an engine-known reference diagnostic snapshot.
    /// </summary>
    /// <param name="asset">
    /// The asset to inspect.
    /// </param>
    /// <returns>
    /// The reference diagnostic snapshot.
    /// </returns>
    public AssetReferenceInfo GetReferenceInfo(AssetObject asset) => GetLoader().GetReferenceInfo(asset);

    private AssetLoader GetLoader()
    {
        EnsureAccess();
        _ = m_types.current;
        AssetLoader loader = isInitialized && m_loader is not null
            ? m_loader
            : throw new InvalidOperationException("AssetPipeline is not initialized.");
        return loader;
    }

}
