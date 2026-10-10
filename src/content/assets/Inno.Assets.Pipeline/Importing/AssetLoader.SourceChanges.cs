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
using Inno.Core.Diagnostics;
using Inno.Core.Identity;
using Inno.References;
using Inno.Core.Execution;
using Inno.Core.IO;
using Inno.Core.Logging;
using Inno.Extensibility.Types;
using Inno.Core.Serialization;
using Inno.Core.Collections;

using IOFile = System.IO.File;

namespace Inno.Assets.Pipeline;

sealed partial class AssetLoader
{
    /// <summary>
    /// Reconciles source files, metadata, artifacts and the in-memory catalog.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation for reconciliation and importer work; unpublished candidates can be discarded after it drains.
    /// </param>
    /// <exception cref="OperationCanceledException">
    /// Reconciliation or an importer observes cancellation.
    /// </exception>
    public void Rescan(CancellationToken cancellationToken = default)
        => Execute(() =>
        {
            CancellationToken previous = m_importCancellation;
            m_importCancellation = cancellationToken;
            try
            {
                RescanLocked();
                cancellationToken.ThrowIfCancellationRequested();
            }
            finally
            {
                m_importCancellation = previous;
            }
        });

    /// <summary>
    /// Applies normalized source file changes to the persistent catalog.
    /// </summary>
    /// <param name="changes">
    /// The normalized source changes.
    /// </param>
    public void ApplySourceChanges(IReadOnlyList<AssetChangedEvent> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);
        Execute(() => ApplySourceChangesLocked(changes));
    }

    private void RescanLocked()
    {
        m_importCancellation.ThrowIfCancellationRequested();
        LoadCatalogLocked();
        bool registriesChanged =
            m_importerRegistryVersion != m_importers.snapshotVersion ||
            m_buildProcessorRegistryVersion != m_buildProcessors.snapshotVersion;
        bool releasedRetiredAssets = ReleaseRetiredCanonicalAssetsLocked();
        if (m_runtimeArtifactsOnly)
        {
            ValidateRuntimeArtifactsLocked();
            if (releasedRetiredAssets || registriesChanged)
                RefreshLoadedAssetReferencesLocked();
            m_importerRegistryVersion = m_importers.snapshotVersion;
            m_buildProcessorRegistryVersion = m_buildProcessors.snapshotVersion;
            return;
        }
        EnsureDirectoryMetadataLocked();
        AssetPath[] sourceFiles = m_mounts.Values
            .SelectMany(mount => Directory.GetFiles(mount.rootPath, "*", SearchOption.AllDirectories)
                .Select(path => new AssetPath(
                    mount.id,
                    Path.GetRelativePath(mount.rootPath, path).Replace('\\', '/'))))
            .Where(path => !IsSourceIgnored(path, isDirectory: false))
            .ToArray();
        foreach (AssetPath sourceFile in sourceFiles)
        {
            m_importCancellation.ThrowIfCancellationRequested();
            string relative = sourceFile.ToString();
            string absoluteSource = GetSourcePath(relative);
            AssetImporter? importer = m_importers.FindByPath(relative);
            if (importer is null)
            {
                TrackUnsupportedSourceLocked(relative);
                continue;
            }
            TryAssociateUntrackedRenameLocked(relative, absoluteSource);
            AssetRecord? record = FindRecordLocked(relative);
            if (record is null ||
                record.asset?.isMissing == true ||
                IsStale(record, out _) ||
                (record.meta.importStatus == (int)AssetImportStatus.Imported && !m_artifacts.TryGet(
                    new AssetArtifactKey(record.meta.artifactKey),
                    "asset-state",
                    out _)))
            {
                ImportLocked(relative);
            }
        }

        foreach (AssetRecord record in m_recordsByPath.Values.ToArray())
        {
            if (!IsMounted(record.relativePath))
            {
                RetireUnmountedRecordLocked(record);
                continue;
            }

            if (IOFile.Exists(GetSourcePath(record.relativePath)) ||
                Directory.Exists(GetSourcePath(record.relativePath)))
                continue;
            HandleDeletedLocked(record.relativePath);
        }
        if (releasedRetiredAssets || registriesChanged)
            RefreshLoadedAssetReferencesLocked();
        CommitCatalogLocked();
        EnsureReadOnlyImportsSucceededLocked();
        m_importerRegistryVersion = m_importers.snapshotVersion;
        m_buildProcessorRegistryVersion = m_buildProcessors.snapshotVersion;
    }

    private bool IsMounted(string canonicalPath) => m_mounts.ContainsKey(AssetPath.Parse(canonicalPath).source);

    private void ApplySourceChangesLocked(IReadOnlyList<AssetChangedEvent> changes)
    {
        if (changes.Any(change =>
                IsSampleChangePath(change.relativePath) ||
                IsSampleChangePath(change.oldRelativePath)))
        {
            RescanLocked();
            return;
        }
        IReadOnlyDictionary<string, int> ambiguousRenames = AssociateUntrackedRenamesLocked(changes);
        foreach (AssetChangedEvent change in changes)
        {
            if (change.relativePath.EndsWith(C_META_POSTFIX, StringComparison.OrdinalIgnoreCase))
            {
                string ownerPath = NormalizeRelativePath(change.relativePath[..^C_META_POSTFIX.Length]);
                AssetRecord? owner = FindRecordLocked(ownerPath);
                if (owner is not null && !owner.meta.isDirectory && IsStale(owner, out _))
                    ImportLocked(ownerPath);
                continue;
            }
            if (IsInternalGeneratedPath(change.relativePath))
                continue;
            if (change.changeType.HasFlag(WatcherChangeTypes.Renamed))
            {
                HandleRenameLocked(change.oldRelativePath, change.relativePath);
                continue;
            }
            if (change.changeType.HasFlag(WatcherChangeTypes.Deleted))
            {
                HandleDeletedLocked(change.relativePath);
                continue;
            }
            ImportLocked(NormalizeRelativePath(change.relativePath));
        }
        foreach ((string path, int matchCount) in ambiguousRenames)
            RecordAmbiguousRenameDiagnosticLocked(path, matchCount);
    }

    private void HandleRenameLocked(
        string oldPath,
        string newPath
    ) {
        string oldNormalized = NormalizeRelativePath(oldPath);
        string newNormalized = NormalizeRelativePath(newPath);
        if (Directory.Exists(GetSourcePath(newNormalized)))
        {
            HandleDirectoryRenameLocked(oldNormalized, newNormalized);
            return;
        }
        AssetRecord? record = FindRecordLocked(oldNormalized);
        if (record is null)
        {
            ImportLocked(newNormalized);
            return;
        }
        string oldMeta = GetMetaPath(oldNormalized);
        string newMeta = GetMetaPath(newNormalized);
        if (TryReadSourceMeta(newMeta, out AssetSourceMeta targetMeta) &&
            targetMeta.persistentId != record.persistentId)
        {
            record.meta.importStatus = (int)AssetImportStatus.Conflict;
            record.meta.diagnostics =
            [
                $"Rename target '{newNormalized}' already owns persistent id " +
                $"'{targetMeta.persistentId}'."
            ];
            CommitCatalogLocked();
            return;
        }

        if (IOFile.Exists(oldMeta) && !IOFile.Exists(newMeta))
            MoveGeneratedFile(oldMeta, newMeta);
        m_recordsByPath.Remove(oldNormalized);
        m_importGraph.RemoveNode(oldNormalized);
        record.relativePath = newNormalized;
        record.meta.relativePath = newNormalized;
        AssetImporter? importer = m_importers.FindByPath(newNormalized);
        if (importer is null)
        {
            record.meta.importStatus = (int)AssetImportStatus.Unsupported;
            record.meta.diagnostics =
                [$"No importer supports '{Path.GetExtension(newNormalized)}'."];
            record.meta.importerId = string.Empty;
            record.meta.artifactKey = string.Empty;
            if (record.asset is not null)
            {
                AssetObject replaced = record.asset;
                m_runtimeOwner.Initialize(
                    replaced,
                    AssetPath.Parse(newNormalized),
                    record.meta.sourceHash,
                    ReadOnlyMemory<byte>.Empty,
                    true,
                    replaced.contentVersion + 1);
                m_runtimeOwner.Release(replaced);
                m_identities.Unregister(replaced);
                record.asset = null;
                PublishReloaded(replaced);
            }
        }
        else
        {
            record.meta.importStatus = (int)AssetImportStatus.Imported;
            record.meta.diagnostics = [];
            if (record.asset is not null)
                m_runtimeOwner.UpdateAssetPath(record.asset, AssetPath.Parse(newNormalized));
        }
        m_recordsByPath[newNormalized] = record;
        // A renamed source may deliberately carry a different importer and its matching settings.
        // The source sidecar is authoritative; cached importer state must not relabel those bytes.
        if (ReadMetadata(newMeta) is null)
            WriteSourceMeta(record.meta);
        UpdateGraphsLocked(record);
        CommitCatalogLocked();

        bool catalogChanged = false;
        bool stale = importer is not null && IsStale(record, out catalogChanged);
        if (importer is not null &&
            (!string.Equals(importer.importerId, record.meta.importerId, StringComparison.Ordinal) ||
             stale))
        {
            ImportLocked(newNormalized);
        }
        else if (catalogChanged)
        {
            CommitCatalogLocked();
        }
    }

    private void HandleDeletedLocked(string relativePath)
    {
        string normalized = NormalizeRelativePath(relativePath);
        string prefix = normalized + "/";
        AssetRecord[] records = m_recordsByPath.Values
            .Where(record =>
                string.Equals(record.relativePath, normalized, StringComparison.OrdinalIgnoreCase) ||
                record.relativePath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(static record => record.relativePath.Length)
            .ToArray();
        if (records.Length == 0)
            return;

        for (int i = 0; i < records.Length; i++)
        {
            AssetRecord record = records[i];
            string recordPath = record.relativePath;
            DeleteIfExists(GetMetaPath(recordPath));
            m_recordsByPath.Remove(recordPath);
            m_importGraph.RemoveNode(recordPath);

            if (record.persistentId == Guid.Empty)
                continue;

            record.meta.isTombstone = true;
            record.meta.importStatus = (int)AssetImportStatus.Missing;
            record.meta.diagnostics = [$"Source '{recordPath}' was removed."];
            record.meta.artifactKey = string.Empty;
            // A returning source must be recoverable without the retired managed generation.
            record.payload = [];
            m_runtimeGraph.ReplaceDependencies(record.persistentId, []);
            if (record.asset is not null)
            {
                m_dependencyRetention.Remove(record.asset);
                m_runtimeOwner.Initialize(
                    record.asset,
                    AssetPath.Parse(recordPath),
                    record.meta.sourceHash,
                    ReadOnlyMemory<byte>.Empty,
                    true,
                    record.asset.contentVersion + 1);
                PublishReloaded(record.asset);
            }
        }
        CommitCatalogLocked();
    }

    private int TryAssociateUntrackedRenameLocked(
        string relativePath,
        string absoluteSourcePath
    ) {
        if (m_recordsByPath.ContainsKey(relativePath) || IOFile.Exists(GetMetaPath(relativePath)))
            return 0;
        string fingerprint = ComputeSha256Hex(IOFile.ReadAllBytes(absoluteSourcePath));
        AssetRecord[] matches = m_recordsByPath.Values
            .Where(record =>
                !record.meta.isDirectory &&
                !IOFile.Exists(GetSourcePath(record.relativePath)) &&
                string.Equals(record.meta.sourceHash, fingerprint, StringComparison.Ordinal))
            .ToArray();
        if (matches.Length == 1)
            HandleRenameLocked(matches[0].relativePath, relativePath);
        return matches.Length;
    }

    private IReadOnlyDictionary<string, int> AssociateUntrackedRenamesLocked(IReadOnlyList<AssetChangedEvent> changes)
    {
        var ambiguous = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < changes.Count; i++)
        {
            AssetChangedEvent change = changes[i];
            if (change.changeType.HasFlag(WatcherChangeTypes.Deleted) ||
                change.changeType.HasFlag(WatcherChangeTypes.Renamed) ||
                IsInternalGeneratedPath(change.relativePath))
            {
                continue;
            }

            string relativePath = NormalizeRelativePath(change.relativePath);
            string absolutePath = GetSourcePath(relativePath);
            if (IOFile.Exists(absolutePath))
            {
                int matches = TryAssociateUntrackedRenameLocked(relativePath, absolutePath);
                if (matches > 1)
                    ambiguous[relativePath] = matches;
            }
        }
        return ambiguous;
    }

    private void HandleDirectoryRenameLocked(
        string oldPath,
        string newPath
    ) {
        string oldPrefix = oldPath + "/";
        AssetRecord[] records = m_recordsByPath.Values
            .Where(record =>
                string.Equals(record.relativePath, oldPath, StringComparison.OrdinalIgnoreCase) ||
                record.relativePath.StartsWith(oldPrefix, StringComparison.OrdinalIgnoreCase))
            .OrderBy(static record => record.relativePath.Length)
            .ToArray();
        if (records.Length == 0)
        {
            EnsureDirectoryMetadataLocked();
            CommitCatalogLocked();
            return;
        }

        string oldFolderMeta = GetMetaPath(oldPath);
        string newFolderMeta = GetMetaPath(newPath);
        if (IOFile.Exists(oldFolderMeta) && !IOFile.Exists(newFolderMeta))
            MoveGeneratedFile(oldFolderMeta, newFolderMeta);

        for (int i = 0; i < records.Length; i++)
        {
            AssetRecord record = records[i];
            string suffix = record.relativePath.Length == oldPath.Length
                ? string.Empty
                : record.relativePath[oldPath.Length..];
            string destination = newPath + suffix;
            m_recordsByPath.Remove(record.relativePath);
            m_importGraph.RemoveNode(record.relativePath);
            record.relativePath = destination;
            record.meta.relativePath = destination;
            record.meta.importStatus = (int)AssetImportStatus.Imported;
            record.meta.diagnostics = [];
            if (record.asset is not null)
                m_runtimeOwner.UpdateAssetPath(record.asset, AssetPath.Parse(destination));
            m_recordsByPath[destination] = record;
            UpdateGraphsLocked(record);
        }
        CommitCatalogLocked();
    }

    private bool IsSourceIgnored(
        string relativePath,
        bool isDirectory
    ) => IsSourceIgnored(AssetPath.Parse(relativePath), isDirectory);

    private bool IsSourceIgnored(
        AssetPath assetPath,
        bool isDirectory
    ) {
        string localPath = assetPath.localPath;
        string[] segments = localPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < segments.Length - (isDirectory ? 0 : 1); i++)
        {
            if (m_sourcePolicy.IsIgnored(segments[i], isDirectory: true))
                return true;
        }
        return m_sourcePolicy.IsIgnored(localPath, isDirectory);
    }

    private static bool IsSampleChangePath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return false;
        AssetPath path = AssetPath.Parse(relativePath);
        return AssetSample.IsRoot(path) || AssetSample.Contains(path, isDirectory: false);
    }

}
