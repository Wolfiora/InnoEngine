using System;
using System.Collections.Generic;

namespace Inno.Assets;

/// <summary>
/// Provides an immutable public snapshot of one cataloged asset.
/// </summary>
public sealed class AssetInfo
{
    /// <summary>
    /// Creates an asset information snapshot.
    /// </summary>
    /// <param name="persistentID">
    /// Persistent identity assigned to the source asset.
    /// </param>
    /// <param name="assetPath">
    /// Current isolated source path.
    /// </param>
    /// <param name="sourceKind">
    /// Whether the catalog entry represents a file or directory.
    /// </param>
    /// <param name="status">
    /// Current import status.
    /// </param>
    /// <param name="importerID">
    /// Stable importer identifier, or an empty value when none applies.
    /// </param>
    /// <param name="stableAssetTypeID">
    /// Stable imported asset type identity.
    /// </param>
    /// <param name="artifactKey">
    /// Current committed artifact key.
    /// </param>
    /// <param name="lastSuccessfulArtifactKey">
    /// Most recent successfully imported artifact key.
    /// </param>
    /// <param name="diagnostics">
    /// Latest import diagnostics, or <see langword="null"/> for none.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="assetPath"/> is invalid.
    /// </exception>
    public AssetInfo(
        Guid persistentID,
        AssetPath assetPath,
        AssetSourceKind sourceKind,
        AssetImportStatus status,
        string importerID,
        Guid stableAssetTypeID,
        AssetArtifactKey artifactKey,
        AssetArtifactKey lastSuccessfulArtifactKey,
        IReadOnlyList<string>? diagnostics = null
    ) {
        this.persistentID = persistentID;
        if (!assetPath.isValid)
        {
            throw new ArgumentException("An asset information snapshot requires a valid path.", nameof(assetPath));
        }
        this.assetPath = assetPath;
        this.sourceKind = sourceKind;
        this.status = status;
        this.importerID = importerID ?? string.Empty;
        this.stableAssetTypeID = stableAssetTypeID;
        this.artifactKey = artifactKey;
        this.lastSuccessfulArtifactKey = lastSuccessfulArtifactKey;
        this.diagnostics = diagnostics ?? Array.Empty<string>();
    }

    /// <summary>
    /// Gets the persistent asset identity.
    /// </summary>
    public Guid persistentID { get; }

    /// <summary>
    /// Gets the current isolated source path.
    /// </summary>
    public AssetPath assetPath { get; }

    /// <summary>
    /// Gets whether the catalog entry represents a file or directory source.
    /// </summary>
    public AssetSourceKind sourceKind { get; }

    /// <summary>
    /// Gets the current import status.
    /// </summary>
    public AssetImportStatus status { get; }

    /// <summary>
    /// Gets the selected importer identifier.
    /// </summary>
    public string importerID { get; }

    /// <summary>
    /// Gets the stable imported asset type identity.
    /// </summary>
    public Guid stableAssetTypeID { get; }

    /// <summary>
    /// Gets the current committed artifact key.
    /// </summary>
    public AssetArtifactKey artifactKey { get; }

    /// <summary>
    /// Gets the most recent successfully imported artifact key.
    /// </summary>
    public AssetArtifactKey lastSuccessfulArtifactKey { get; }

    /// <summary>
    /// Gets diagnostics produced by the latest reconciliation or import.
    /// </summary>
    public IReadOnlyList<string> diagnostics { get; }
}
