using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets;

/// <summary>
/// Provides script-facing asset queries through the lookup bound to the current runtime session.
/// </summary>
/// <remarks>
/// This façade owns no asset state. Engine and Editor infrastructure should depend on an explicit
/// <see cref="IAssetLookup"/> instance.
/// </remarks>
public static class Assets
{
    /// <summary>
    /// Creates a path relative to the Asset source that owns the calling script.
    /// </summary>
    /// <param name="localPath">
    /// The normalized path relative to the caller's Project or installed Plugin Assets root.
    /// </param>
    /// <param name="sourceFile">
    /// The compiler-supplied canonical Asset path of the calling source file; callers normally omit it.
    /// </param>
    /// <returns>
    /// A mount-qualified path that follows the script from Project development into an installed Plugin.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The caller does not provide a canonical source location produced by the scripting compiler.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The local path or source location is null.
    /// </exception>
    public static AssetPath LocalPath(
        string localPath,
        [CallerFilePath] string sourceFile = ""
    ) {
        ArgumentNullException.ThrowIfNull(localPath);
        ArgumentNullException.ThrowIfNull(sourceFile);
        if (!sourceFile.Contains("::", StringComparison.Ordinal))
            throw new InvalidOperationException("Source-local Asset paths require a compiler-supplied canonical source location.");
        AssetSourceId source;
        try
        {
            source = AssetPath.Parse(sourceFile).source;
        }
        catch (ArgumentException exception)
        {
            throw new InvalidOperationException("The caller's canonical Asset source location is invalid.", exception);
        }
        return new AssetPath(source, localPath);
    }

    /// <summary>
    /// Loads the canonical asset at a logical catalog path in the current session.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required asset contract.
    /// </typeparam>
    /// <param name="path">
    /// The mount-qualified logical catalog path.
    /// </param>
    /// <returns>
    /// The canonical compatible asset owned by the current lookup.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no asset lookup is active, the asset does not exist, or its type is incompatible.
    /// </exception>
    public static TAsset Load<TAsset>(AssetPath path)
        where TAsset : AssetObject
        => AssetExecutionContext.current.Load<TAsset>(path);

    /// <summary>
    /// Loads the canonical asset with a persistent identity in the current session.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required asset contract.
    /// </typeparam>
    /// <param name="persistentId">
    /// The non-empty persistent asset identity.
    /// </param>
    /// <returns>
    /// The canonical compatible asset owned by the current lookup.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no asset lookup is active, the asset does not exist, or its type is incompatible.
    /// </exception>
    public static TAsset Load<TAsset>(Guid persistentId)
        where TAsset : AssetObject
        => AssetExecutionContext.current.Load<TAsset>(persistentId);

    /// <summary>
    /// Tries to load the canonical asset at a logical catalog path in the current session.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required asset contract.
    /// </typeparam>
    /// <param name="path">
    /// The mount-qualified logical catalog path.
    /// </param>
    /// <param name="asset">
    /// Receives the canonical compatible asset when available.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the current lookup contains a compatible asset; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no asset lookup is active for the caller.
    /// </exception>
    public static bool TryLoad<TAsset>(
        AssetPath path,
        out TAsset? asset
    )
        where TAsset : AssetObject
        => AssetExecutionContext.current.TryLoad(path, out asset);

    /// <summary>
    /// Tries to load the canonical asset with a persistent identity in the current session.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required asset contract.
    /// </typeparam>
    /// <param name="persistentId">
    /// The non-empty persistent asset identity.
    /// </param>
    /// <param name="asset">
    /// Receives the canonical compatible asset when available.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the current lookup contains a compatible asset; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no asset lookup is active for the caller.
    /// </exception>
    public static bool TryLoad<TAsset>(
        Guid persistentId,
        out TAsset? asset
    )
        where TAsset : AssetObject
        => AssetExecutionContext.current.TryLoad(persistentId, out asset);

    /// <summary>
    /// Acquires a canonical asset and explicitly retains it until the returned lease is disposed.
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
    /// A lease that owns residency for the canonical asset.
    /// </returns>
    public static ValueTask<AssetLease<TAsset>> AcquireAsync<TAsset>(
        AssetPath path,
        CancellationToken cancellationToken = default
    )
        where TAsset : AssetObject
        => GetResidency().AcquireAsync<TAsset>(path, cancellationToken);

    /// <summary>
    /// Acquires a canonical asset by persistent identity and explicitly retains it.
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
    /// A lease that owns residency for the canonical asset.
    /// </returns>
    public static ValueTask<AssetLease<TAsset>> AcquireAsync<TAsset>(
        Guid persistentId,
        CancellationToken cancellationToken = default
    )
        where TAsset : AssetObject
        => GetResidency().AcquireAsync<TAsset>(persistentId, cancellationToken);

    /// <summary>
    /// Acquires one verified immutable artifact output for an explicit lifetime.
    /// </summary>
    /// <param name="persistentId">
    /// Persistent identity of the artifact owner.
    /// </param>
    /// <param name="outputName">
    /// Stable artifact output name.
    /// </param>
    /// <returns>
    /// A lease over verified artifact metadata and bytes.
    /// </returns>
    public static ArtifactLease AcquireArtifact(
        Guid persistentId,
        string outputName
    )
        => GetResidency().AcquireArtifact(persistentId, outputName);

    private static IAssetResidency GetResidency()
        => AssetExecutionContext.current as IAssetResidency
            ?? throw new InvalidOperationException(
                "The current asset lookup does not provide explicit residency ownership.");
}
