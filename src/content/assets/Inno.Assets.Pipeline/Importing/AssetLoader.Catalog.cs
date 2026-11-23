using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Collections;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.IO;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.References;

using IOFile = System.IO.File;

namespace Inno.Assets.Pipeline;

public sealed partial class AssetLoader
{
    /// <summary>
    /// Creates an isolated source-mount and catalog candidate without changing the active catalog.
    /// </summary>
    /// <param name="mounts">
    /// The complete source-mount snapshot to validate.
    /// </param>
    /// <param name="sourcePolicy">
    /// The source policy for the candidate, or <see langword="null"/> to reuse this loader's policy.
    /// </param>
    /// <returns>
    /// A candidate that owns its catalog staging storage and exposes the isolated loader to its owner.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the mount snapshot is empty, duplicated, or has no writable project source.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown when the active catalog cannot be copied into candidate storage.
    /// </exception>
    public AssetCatalogCandidate PrepareCatalogCandidate(
        IReadOnlyList<AssetSourceMount> mounts,
        AssetSourcePolicy? sourcePolicy = null
    ) {
        ArgumentNullException.ThrowIfNull(mounts);
        return Execute(() =>
        {
            string candidateLibraryRoot = Path.Combine(
                libraryRoot,
                "AssetDatabase",
                "Candidates",
                Guid.NewGuid().ToString("N")
            );
            AssetLoader? candidateLoader = null;
            try
            {
                m_catalog.CopyLatestTo(candidateLibraryRoot);
                candidateLoader = new AssetLoader(
                    m_types,
                    m_serialization,
                    m_identities,
                    m_diagnosticHub,
                    m_logs,
                    mounts,
                    libraryRoot,
                    candidateLibraryRoot,
                    sourcePolicy ?? m_sourcePolicy
                );
                candidateLoader.m_identitiesActive = false;
                candidateLoader.m_deferUnavailableExtensions = true;
                candidateLoader.m_preserveInitialExtensionDiscovery = m_deferUnavailableExtensions;
                candidateLoader.m_diagnostics.SetActive(false);
                candidateLoader.m_sourceMetadataStage = new AssetSourceMetadataStage();
                return new AssetCatalogCandidate(libraryRoot, candidateLibraryRoot, candidateLoader);
            }
            catch
            {
                candidateLoader?.Dispose();
                if (Directory.Exists(candidateLibraryRoot))
                {
                    Directory.Delete(candidateLibraryRoot, recursive: true);
                }
                throw;
            }
        });
    }

    /// <summary>
    /// Tries to get a catalog snapshot by isolated source path.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="info">
    /// The immutable catalog snapshot when found.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the source is cataloged.
    /// </returns>
    public bool TryGetInfo(
        AssetPath path,
        out AssetInfo? info
    ) {
        if (!TryNormalizeCatalogPath(path, out string normalized))
        {
            info = null;
            return false;
        }
        AssetInfo? result = Execute(() => CreateInfo(FindRecordLocked(normalized)));
        info = result;
        return result is not null;
    }

    /// <summary>
    /// Tries to get a catalog snapshot by persistent identity.
    /// </summary>
    /// <param name="persistentID">
    /// The stable persistent identity used for lookup.
    /// </param>
    /// <param name="info">
    /// The resolved immutable metadata returned to the caller.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetInfo(
        Guid persistentID,
        out AssetInfo? info
    ) {
        AssetInfo? result = Execute(() => (
            m_recordsByID.TryGetValue(persistentID, out AssetRecord? record)
            ? CreateInfo(record)
            : null
        ));
        info = result;
        return result is not null;
    }

    /// <summary>
    /// Tries to resolve a persistent identity without loading the asset.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="persistentID">
    /// The resolved identity.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when catalog metadata exists.
    /// </returns>
    public bool TryGetPersistentID(
        AssetPath path,
        out Guid persistentID
    ) {
        if (!TryNormalizeCatalogPath(path, out string normalized))
        {
            persistentID = Guid.Empty;
            return false;
        }
        Guid result = Execute(() => FindRecordLocked(normalized)?.persistentID ?? Guid.Empty);
        persistentID = result;
        return result != Guid.Empty;
    }

    /// <summary>
    /// Tries to resolve the concrete asset type without loading it.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="assetType">
    /// The resolved type.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the type can be resolved.
    /// </returns>
    public bool TryGetAssetType(
        AssetPath path,
        out Type? assetType
    ) {
        if (!TryNormalizeCatalogPath(path, out string normalized))
        {
            assetType = null;
            return false;
        }
        Type? result = Execute(() => ResolveRecordType(FindRecordLocked(normalized)));
        assetType = result;
        return result is not null;
    }

    /// <summary>
    /// Gets isolated source paths of all canonical loaded assets.
    /// </summary>
    /// <returns>
    /// A stable isolated path snapshot.
    /// </returns>
    public IReadOnlyList<AssetPath> GetLoadedPaths()
        => Execute(() => m_recordsByPath.Values
            .Where(static record => record.asset is not null)
            .Select(static record => AssetPath.Parse(record.relativePath))
            .OrderBy(static path => path.source.value, StringComparer.Ordinal)
            .ThenBy(static path => path.localPath, StringComparer.Ordinal)
            .ToArray());

    internal void PromoteCatalogTo(string destinationLibraryRoot)
        => Execute(() =>
        {
            if (m_sourceMetadataStage is not null)
            {
                m_sourceMetadataStage.Commit(() => m_catalog.PromoteTo(destinationLibraryRoot));
                m_sourceMetadataStage = null;
            }
            else
            {
                m_catalog.PromoteTo(destinationLibraryRoot);
            }
        });

    private void LoadCatalogLocked()
    {
        AssetMeta[] catalogEntries;
        try
        {
            catalogEntries = m_catalog.Load();
        }
        catch (Exception exception)
        {
            if (m_diagnostics.PublishCatalogFailure(exception))
            {
                m_log.Write(LogLevel.Error, "Asset catalog load failed: {0}", [exception]);
            }
            throw;
        }
        for (int i = 0; i < catalogEntries.Length; i++)
        {
            MergeCatalogMetaLocked(catalogEntries[i]);
        }

        foreach (AssetSourceMount mount in m_mounts.Values)
        {
            foreach (string metaPath in Directory.GetFiles(
                mount.rootPath,
                "*" + C_META_POSTFIX,
                SearchOption.AllDirectories
            )) {
                string localMeta = Path.GetRelativePath(mount.rootPath, metaPath).Replace('\\', '/');
                string relative = new AssetPath(mount.id, localMeta[..^C_META_POSTFIX.Length]).ToString();
                string sourcePath = GetSourcePath(relative);
                if (Directory.Exists(sourcePath))
                {
                    continue;
                }
                try
                {
                    AssetSourceMeta sourceMeta = m_serialization.Deserialize<AssetSourceMeta>(
                        ReadMetadata(metaPath) ?? throw new IOException("Source metadata is no longer available.")
                    );
                    if (sourceMeta.persistentId != Guid.Empty)
                    {
                        AssetRecord? existing = FindRecordByIDWithoutLoading(sourceMeta.persistentId);
                        if (existing is not null
                            && !string.Equals(existing.relativePath, relative, StringComparison.OrdinalIgnoreCase)
                            && IOFile.Exists(GetSourcePath(existing.relativePath))
                        ) {
                            if (mount.isReadOnly)
                            {
                                throw new InvalidDataException(
                                    $"Read-only source '{relative}' duplicates persistent ID '{sourceMeta.persistentId}'."
                                );
                            }
                            sourceMeta.persistentId = Guid.NewGuid();
                            WriteAtomic(metaPath, m_serialization.Serialize(sourceMeta));
                        }
                        // Index every source identity before importing source bodies. Runtime
                        // dependencies are identity-first, so import order must not decide whether
                        // a relocated asset can resolve another asset in the same mount snapshot.
                        _ = FindRecordLocked(relative);
                        continue;
                    }
                }
                catch when (!mount.isReadOnly)
                {
                    // Writable corrupt sidecars remain visible as catalog diagnostics.
                }
            }
        }
    }

    private AssetRecord? FindRecordLocked(string relativePath)
    {
        m_recordsByPath.TryGetValue(relativePath, out AssetRecord? record);
        string metaPath = GetMetaPath(relativePath);
        if (!TryReadSourceMeta(metaPath, out AssetSourceMeta sourceMeta))
        {
            return record;
        }
        if (sourceMeta.persistentId == Guid.Empty)
        {
            return record;
        }
        if (record is not null && record.persistentID == sourceMeta.persistentId)
        {
            return record;
        }
        if (record is not null && record.persistentID != Guid.Empty)
        {
            // The source sidecar owns path identity. A catalog entry at the same path can be
            // historical after an interrupted save or catalog promotion and must not replace it.
            RetireRecordLocked(
                record,
                $"Source '{relativePath}' now declares persistent id '{sourceMeta.persistentId}'."
            );
            record = null;
        }
        if (m_recordsByID.TryGetValue(sourceMeta.persistentId, out AssetRecord? tombstone)
            && tombstone.meta.isTombstone
        ) {
            tombstone.relativePath = relativePath;
            tombstone.persistentID = sourceMeta.persistentId;
            tombstone.meta = new AssetMeta
            {
                relativePath = relativePath,
                persistentId = sourceMeta.persistentId,
                importerId = sourceMeta.importerId,
                importStatus = (int)AssetImportStatus.Pending
            };
            AddOrReplaceRecordLocked(tombstone);
            return tombstone;
        }
        if (m_recordsByID.TryGetValue(sourceMeta.persistentId, out AssetRecord? sameID)
            && !string.Equals(sameID.relativePath, relativePath, StringComparison.OrdinalIgnoreCase)
        ) {
            if (IOFile.Exists(GetSourcePath(sameID.relativePath))
                || Directory.Exists(GetSourcePath(sameID.relativePath))
            ) {
                sourceMeta.persistentId = Guid.NewGuid();
                WriteAtomic(metaPath, m_serialization.Serialize(sourceMeta));
            }
            else
            {
                HandleRenameLocked(sameID.relativePath, relativePath);
                return sameID;
            }
        }
        record = m_recordsByID.GetValueOrDefault(sourceMeta.persistentId) ?? record ?? new AssetRecord();
        record.relativePath = relativePath;
        record.persistentID = sourceMeta.persistentId;
        if (record.meta.isTombstone || record.meta.persistentId == Guid.Empty)
        {
            record.meta = new AssetMeta
            {
                relativePath = relativePath,
                persistentId = sourceMeta.persistentId,
                importerId = sourceMeta.importerId,
                importStatus = (int)AssetImportStatus.Pending
            };
        }
        AddOrReplaceRecordLocked(record);
        return record;
    }

    private void AddOrReplaceRecordLocked(AssetRecord record)
    {
        if (record.persistentID == Guid.Empty)
        {
            m_recordsByPath[record.relativePath] = record;
            return;
        }
        if (m_recordsByID.TryGetValue(record.persistentID, out AssetRecord? sameID)
            && !ReferenceEquals(sameID, record)
            && !string.Equals(sameID.relativePath, record.relativePath, StringComparison.OrdinalIgnoreCase)
        ) {
            throw new InvalidOperationException(
                $"Asset persistent id '{record.persistentID}' is used by both "
                + $"'{sameID.relativePath}' and '{record.relativePath}'."
            );
        }
        m_recordsByPath[record.relativePath] = record;
        m_recordsByID[record.persistentID] = record;
    }

    private void RebindTombstoneLocked(
        AssetRecord record,
        string relativePath
    ) {
        m_recordsByPath.Remove(record.relativePath);
        m_importGraph.RemoveNode(record.relativePath);
        record.relativePath = relativePath;
        record.meta.relativePath = relativePath;
        m_recordsByPath[relativePath] = record;
    }

    private void RemoveRecordLocked(
        AssetRecord record,
        bool removeGeneratedFiles = true
    ) {
        m_recordsByPath.Remove(record.relativePath);
        if (record.persistentID != Guid.Empty)
        {
            m_recordsByID.Remove(record.persistentID);
            m_runtimeGraph.RemoveNode(record.persistentID);
        }
        m_importGraph.RemoveNode(record.relativePath);
        if (removeGeneratedFiles)
        {
            DeleteIfExists(GetMetaPath(record.relativePath));
        }
    }

    private Type? ResolveRecordType(AssetRecord? record)
    {
        if (record is null)
        {
            return null;
        }
        if (record.asset is not null)
        {
            return record.asset.GetType();
        }
        var typeRef = new TypeRef(record.stableTypeID);
        return m_types.TryResolve(typeRef, out Type? type) && typeof(AssetObject).IsAssignableFrom(type)
               ? type
               : m_importers.FindByID(record.meta.importerId)?.targetAssetType;
    }

    private Type ResolveDependencyExpectedType(AssetDependency dependency)
    {
        TypeRef typeRef = dependency.type;
        if (m_types.TryResolve(typeRef, out Type? type) && typeof(AssetObject).IsAssignableFrom(type))
        {
            return type;
        }
        return typeof(MissingAsset);
    }

    private bool TryNormalizeCatalogPath(
        AssetPath path,
        out string normalized
    ) {
        if (!path.isValid)
        {
            throw new ArgumentException("A valid isolated asset path is required.", nameof(path));
        }
        _ = GetMount(path);
        if (string.IsNullOrWhiteSpace(path.localPath))
        {
            normalized = string.Empty;
            return false;
        }
        normalized = path.ToString();
        return true;
    }

    private AssetInfo? CreateInfo(AssetRecord? record)
    {
        if (record is null)
        {
            return null;
        }
        return new AssetInfo(
            record.persistentID,
            AssetPath.Parse(record.relativePath),
            record.meta.isDirectory ? AssetSourceKind.Directory : AssetSourceKind.File,
            Enum.IsDefined(typeof(AssetImportStatus), record.meta.importStatus)
            ? (AssetImportStatus)record.meta.importStatus
            : AssetImportStatus.Failed,
            record.meta.importerId,
            record.stableTypeID,
            new AssetArtifactKey(record.meta.artifactKey),
            new AssetArtifactKey(record.meta.lastSuccessfulArtifactKey),
            record.meta.diagnostics
        );
    }

    private void CommitCatalogLocked()
    {
        AssetMeta[] entries = m_recordsByID.Values
            .Concat(m_recordsByPath.Values.Where(static record => record.persistentID == Guid.Empty))
            .Distinct()
            .Select(static record => record.meta)
            .OrderBy(static meta => meta.relativePath, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        try
        {
            m_catalog.Commit(entries);
            m_diagnostics.ResolveCatalog();
        }
        catch (Exception exception)
        {
            if (m_diagnostics.PublishCatalogFailure(exception))
            {
                m_log.Write(LogLevel.Error, "Asset catalog commit failed: {0}", [exception]);
            }
            throw;
        }
        m_diagnostics.SynchronizeImports(entries);
    }

    private void MergeCatalogMetaLocked(AssetMeta meta)
    {
        if (string.IsNullOrWhiteSpace(meta.relativePath))
        {
            return;
        }
        if (meta.isTombstone)
        {
            if (meta.persistentId == Guid.Empty)
            {
                return;
            }
            AssetRecord tombstone = FindRecordByIDWithoutLoading(meta.persistentId) ?? new AssetRecord();
            if (!string.IsNullOrWhiteSpace(tombstone.relativePath)
                && m_recordsByPath.TryGetValue(tombstone.relativePath, out AssetRecord? pathRecord)
                && ReferenceEquals(pathRecord, tombstone)
            ) {
                m_recordsByPath.Remove(tombstone.relativePath);
            }
            tombstone.relativePath = meta.relativePath;
            tombstone.persistentID = meta.persistentId;
            tombstone.stableTypeID = meta.stableAssetTypeId;
            tombstone.meta = meta;
            tombstone.payload = [];
            m_recordsByID[meta.persistentId] = tombstone;
            return;
        }
        AssetRecord record = meta.persistentId == Guid.Empty
                             ? m_recordsByPath.GetValueOrDefault(meta.relativePath) ?? new AssetRecord()
                             : FindRecordByIDWithoutLoading(meta.persistentId) ?? new AssetRecord();
        if (!string.IsNullOrWhiteSpace(record.relativePath)
            && !string.Equals(record.relativePath, meta.relativePath, StringComparison.OrdinalIgnoreCase)
        ) {
            if (m_recordsByPath.TryGetValue(record.relativePath, out AssetRecord? pathRecord)
                && ReferenceEquals(pathRecord, record)
            ) {
                m_recordsByPath.Remove(record.relativePath);
            }
        }
        record.relativePath = meta.relativePath;
        record.persistentID = meta.persistentId;
        record.stableTypeID = meta.stableAssetTypeId;
        record.meta = meta;
        record.payload = m_artifacts.Read(new AssetArtifactKey(meta.artifactKey), "runtime");
        record.importerGeneration = m_importers.GetGeneration(meta.importerId);
        AddOrReplaceRecordLocked(record);
        if (record.persistentID != Guid.Empty)
        {
            UpdateGraphsLocked(record);
        }
    }

    private AssetRecord? FindRecordByIDWithoutLoading(Guid persistentID)
        => m_recordsByID.TryGetValue(persistentID, out AssetRecord? record) ? record : null;

    private sealed class AssetRecord
    {
        internal string relativePath = string.Empty;
        internal Guid persistentID;
        internal Guid stableTypeID;
        internal AssetMeta meta = new();
        internal byte[] payload = [];
        internal AssetObject? asset;
        internal bool? lastSweepReachability;
        internal long importerGeneration;
        internal long failedTypeGeneration;
        internal AssetSourceFileStamp settingsStamp;
    }
}
