using System;
using System.Collections.Generic;

using Inno.Assets;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Provides read-only authoring source access for active or isolated candidate compilation.
/// </summary>
/// <remarks>
/// Capture inputs on the owner thread and retain the owning generation transaction until all
/// asynchronous consumers drain. This view neither activates nor commits its underlying catalog.
/// </remarks>
public interface IAssetSourceSnapshot
{
    /// <summary>
    /// Gets the source mounts represented by this view.
    /// </summary>
    IReadOnlyList<AssetSourceMount> sourceMounts { get; }

    /// <summary>
    /// Captures the indexed entries represented by this view.
    /// </summary>
    /// <param name="includeDirectories">
    /// Whether directory entries are included.
    /// </param>
    /// <returns>
    /// A stable entry collection for owner-thread input discovery.
    /// </returns>
    IReadOnlyList<AssetFileEntry> GetFileSystemEntries(bool includeDirectories = true);

    /// <summary>
    /// Loads an authoring asset without publishing a candidate catalog.
    /// </summary>
    /// <typeparam name="TAsset">
    /// The required authoring asset type.
    /// </typeparam>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <returns>
    /// The resolved authoring asset.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// No compatible asset can be loaded from this view.
    /// </exception>
    TAsset Load<TAsset>(AssetPath path) where TAsset : AssetObject;

    /// <summary>
    /// Resolves immutable catalog information by source path.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="info">
    /// The catalog information, or null when the source is absent.
    /// </param>
    /// <returns>
    /// True when catalog information exists.
    /// </returns>
    bool TryGetInfo(
        AssetPath path,
        out AssetInfo? info
    );

    /// <summary>
    /// Resolves a named immutable artifact from one persistent asset identity.
    /// </summary>
    /// <param name="persistentId">
    /// The persistent asset identity.
    /// </param>
    /// <param name="outputName">
    /// The named importer output.
    /// </param>
    /// <param name="artifact">
    /// The artifact descriptor, or null when the output is absent.
    /// </param>
    /// <returns>
    /// True when the requested artifact is available.
    /// </returns>
    bool TryGetArtifact(
        Guid persistentId,
        string outputName,
        out AssetArtifactInfo? artifact
    );
}
