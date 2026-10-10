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
    /// Imports one source asset from an isolated source mount.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an importer handled the source.
    /// </returns>
    public bool Import(AssetPath path)
    {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        AssetLoader loader = GetLoader();
        bool imported = loader.Import(path);
        if (imported)
        {
            GetFileSystem().Refresh();
            _ = loader.TryGetPersistentId(path, out Guid persistentId);
            PublishMutation(new AssetChange(AssetChangeKind.Modified, persistentId, path));
        }
        return imported;
    }

    /// <summary>
    /// Reads a detached settings copy for the currently registered importer.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <returns>
    /// The settings copy and the fingerprint required for saving it.
    /// </returns>
    public AssetImportSettingsSnapshot GetImportSettings(AssetPath path)
    {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        return GetLoader().GetImportSettings(path);
    }

    /// <summary>
    /// Saves import settings with conflict detection and immediately attempts to reimport the source.
    /// </summary>
    /// <param name="path">
    /// The writable isolated source path.
    /// </param>
    /// <param name="settings">
    /// The edited settings, or null to reset to importer defaults.
    /// </param>
    /// <param name="expectedFingerprint">
    /// The fingerprint obtained when reading the settings.
    /// </param>
    /// <returns>
    /// True when settings were saved and reimport succeeded; false when the saved settings failed import.
    /// </returns>
    /// <exception cref="IOException">
    /// The settings changed externally or could not be written.
    /// </exception>
    public bool SaveImportSettings(
        AssetPath path,
        ISerializable? settings,
        string expectedFingerprint
    ) {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        _ = NormalizeMutationPath(path, nameof(path));
        AssetLoader loader = GetLoader();
        bool imported = loader.SaveImportSettings(path, settings, expectedFingerprint);
        GetFileSystem().Refresh();
        _ = loader.TryGetPersistentId(path, out Guid persistentId);
        PublishMutation(new AssetChange(AssetChangeKind.Modified, persistentId, path));
        return imported;
    }

    /// <summary>
    /// Saves an asset to its current source path.
    /// </summary>
    /// <param name="asset">
    /// The asset to save.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an importer exported the asset.
    /// </returns>
    public bool Save(AssetObject asset)
    {
        ArgumentNullException.ThrowIfNull(asset);
        return Save(asset.assetPath, asset);
    }

    /// <summary>
    /// Saves an asset to a writable isolated source path, preserving the destination source identity when it exists.
    /// </summary>
    /// <param name="path">
    /// Writable isolated source path.
    /// </param>
    /// <param name="asset">
    /// Asset to save. A detached value replaces the destination content without replacing its persistent identity.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an importer exported the asset.
    /// </returns>
    public bool Save(
        AssetPath path,
        AssetObject asset
    ) {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        _ = NormalizeMutationPath(path, nameof(path));
        ArgumentNullException.ThrowIfNull(asset);
        AssetLoader loader = GetLoader();
        bool existed = loader.TryGetPersistentId(path, out _);
        bool saved = loader.Save(path, asset);
        if (saved)
        {
            GetFileSystem().Refresh();
            _ = loader.TryGetPersistentId(path, out Guid persistentId);
            PublishMutation(new AssetChange(
                existed ? AssetChangeKind.Modified : AssetChangeKind.Added,
                persistentId,
                path));
        }
        return saved;
    }

    /// <summary>
    /// Moves a source asset while preserving its persistent identity and generated metadata.
    /// </summary>
    /// <param name="source">
    /// Existing isolated source path.
    /// </param>
    /// <param name="target">
    /// New isolated source path.
    /// </param>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the source does not exist.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown when the target source or metadata already exists.
    /// </exception>
    public void Move(
        AssetPath source,
        AssetPath target
    ) {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        string sourcePath = NormalizeMutationPath(source, nameof(source));
        string targetPath = NormalizeMutationPath(target, nameof(target));
        if (string.Equals(sourcePath, targetPath, StringComparison.Ordinal))
            return;

        AssetFileSystem fileSystem = GetFileSystem();
        AssetLoader loader = GetLoader();
        if (fileSystem.isWatching)
        {
            IReadOnlyList<AssetChangedEvent> pending = fileSystem.WaitForIdle(out bool requiresFullRescan);
            if (pending.Count > 0 || requiresFullRescan)
                ApplySourceChanges(pending, requiresFullRescan);
        }

        string absoluteSource = Path.Combine(assetRoot, sourcePath);
        string absoluteTarget = Path.Combine(assetRoot, targetPath);
        bool isDirectory = Directory.Exists(absoluteSource);
        if (!isDirectory && !System.IO.File.Exists(absoluteSource))
            throw new FileNotFoundException($"Asset source '{sourcePath}' does not exist.", absoluteSource);
        if (Directory.Exists(absoluteTarget) || System.IO.File.Exists(absoluteTarget))
            throw new IOException($"Asset move target '{targetPath}' already exists.");
        if (System.IO.File.Exists(absoluteTarget + ".imeta"))
            throw new IOException($"Asset move target metadata '{targetPath}.imeta' already exists.");

        bool restartWatcher = fileSystem.isWatching;
        fileSystem.Stop();
        Directory.CreateDirectory(Path.GetDirectoryName(absoluteTarget)!);
        var change = new AssetChangedEvent(targetPath, WatcherChangeTypes.Renamed, sourcePath);
        Dictionary<string, Guid> previousIds = CapturePreviousIds(loader, [change]);
        try
        {
            MovePhysicalSource(absoluteSource, absoluteTarget, isDirectory);
            loader.ApplySourceChanges([change]);
            fileSystem.Refresh();
            AssetChange[] committed = CreateCommittedChanges(loader, [change], previousIds, requiresFullRescan: false);
            InvokeObservers(Changed, new AssetChangeSet(Interlocked.Increment(ref m_revision), committed));
        }
        catch
        {
            TryRollbackPhysicalMove(absoluteSource, absoluteTarget, isDirectory);
            TryRollbackMetadataMove(absoluteSource + ".imeta", absoluteTarget + ".imeta");
            loader.Rescan();
            fileSystem.Refresh();
            throw;
        }
        finally
        {
            if (restartWatcher)
                fileSystem.Start();
        }
    }

    /// <summary>
    /// Deletes a source asset and its metadata while retaining a Library tombstone for existing references.
    /// </summary>
    /// <param name="path">
    /// Existing isolated file or directory path.
    /// </param>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the source does not exist.
    /// </exception>
    public void Delete(AssetPath path)
    {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        string sourcePath = NormalizeMutationPath(path, nameof(path));
        AssetFileSystem fileSystem = GetFileSystem();
        AssetLoader loader = GetLoader();
        DrainPendingChanges(fileSystem);

        string absoluteSource = Path.Combine(assetRoot, sourcePath);
        bool isDirectory = Directory.Exists(absoluteSource);
        if (!isDirectory && !System.IO.File.Exists(absoluteSource))
            throw new FileNotFoundException($"Asset source '{sourcePath}' does not exist.", absoluteSource);

        AssetChangedEvent[] changes = CreateDeletionEvents(fileSystem, sourcePath);
        Dictionary<string, Guid> previousIds = CapturePreviousIds(loader, changes);
        string transactionRoot = Path.Combine(
            libraryRoot,
            "AssetDatabase",
            "Transactions",
            Guid.NewGuid().ToString("N"));
        string stagedSource = Path.Combine(transactionRoot, "source");
        string sourceMeta = absoluteSource + ".imeta";
        string stagedMeta = Path.Combine(transactionRoot, "source.imeta");
        bool restartWatcher = fileSystem.isWatching;
        fileSystem.Stop();
        Directory.CreateDirectory(transactionRoot);
        AssetChange[] committed;
        try
        {
            MovePhysicalSource(absoluteSource, stagedSource, isDirectory);
            if (System.IO.File.Exists(sourceMeta))
                System.IO.File.Move(sourceMeta, stagedMeta);
            loader.ApplySourceChanges(changes);
            fileSystem.Refresh();
            committed = CreateCommittedChanges(loader, changes, previousIds, requiresFullRescan: false);
        }
        catch
        {
            RestoreStagedDeletion(absoluteSource, stagedSource, isDirectory);
            RestoreStagedMetadata(sourceMeta, stagedMeta);
            loader.Rescan();
            fileSystem.Refresh();
            DeleteTransactionDirectorySafely(transactionRoot);
            throw;
        }
        finally
        {
            if (restartWatcher)
                fileSystem.Start();
        }
        DeleteTransactionDirectorySafely(transactionRoot);
        InvokeObservers(Changed, new AssetChangeSet(Interlocked.Increment(ref m_revision), committed));
        CollectArtifactsIfDue(loader, force: true);
    }

    /// <summary>
    /// Creates a tracked source directory and its persistent metadata.
    /// </summary>
    /// <param name="path">
    /// New isolated directory path.
    /// </param>
    /// <exception cref="DirectoryNotFoundException">
    /// Thrown when the parent directory does not exist.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown when the target already exists.
    /// </exception>
    public void CreateDirectory(AssetPath path)
    {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        string sourcePath = NormalizeMutationPath(path, nameof(path));
        AssetFileSystem fileSystem = GetFileSystem();
        AssetLoader loader = GetLoader();
        DrainPendingChanges(fileSystem);

        string absolutePath = Path.Combine(assetRoot, sourcePath);
        string parentPath = Path.GetDirectoryName(absolutePath)!;
        if (!Directory.Exists(parentPath))
        {
            throw new DirectoryNotFoundException(
                $"Asset directory parent '{Path.GetDirectoryName(sourcePath)}' does not exist.");
        }
        if (Directory.Exists(absolutePath) || System.IO.File.Exists(absolutePath))
            throw new IOException($"Asset directory target '{sourcePath}' already exists.");
        if (System.IO.File.Exists(absolutePath + ".imeta"))
            throw new IOException($"Asset directory target metadata '{sourcePath}.imeta' already exists.");

        bool restartWatcher = fileSystem.isWatching;
        fileSystem.Stop();
        var change = new AssetChangedEvent(sourcePath, WatcherChangeTypes.Created);
        try
        {
            Directory.CreateDirectory(absolutePath);
            loader.Rescan();
            fileSystem.Refresh();
            AssetChange[] committed = CreateCommittedChanges(
                loader,
                [change],
                new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase),
                requiresFullRescan: false);
            InvokeObservers(Changed, new AssetChangeSet(Interlocked.Increment(ref m_revision), committed));
        }
        catch
        {
            if (Directory.Exists(absolutePath))
                Directory.Delete(absolutePath, recursive: true);
            if (System.IO.File.Exists(absolutePath + ".imeta"))
                System.IO.File.Delete(absolutePath + ".imeta");
            loader.Rescan();
            fileSystem.Refresh();
            throw;
        }
        finally
        {
            if (restartWatcher)
                fileSystem.Start();
        }
    }

    private string NormalizeMutationPath(
        AssetPath path,
        string parameterName
    ) {
        if (!path.isValid || string.IsNullOrWhiteSpace(path.localPath))
            throw new ArgumentException("Asset source path is required.", parameterName);
        if (path.source != AssetSourceId.project)
            throw new InvalidOperationException($"Asset source '{path.source}' is read-only.");
        return path.localPath;
    }

    private void MovePhysicalSource(
        string sourcePath,
        string targetPath,
        bool isDirectory
    ) {
        if (isDirectory)
            Directory.Move(sourcePath, targetPath);
        else
            System.IO.File.Move(sourcePath, targetPath);
    }

    private void TryRollbackPhysicalMove(
        string sourcePath,
        string targetPath,
        bool isDirectory
    ) {
        try
        {
            bool targetExists = isDirectory ? Directory.Exists(targetPath) : System.IO.File.Exists(targetPath);
            bool sourceExists = isDirectory ? Directory.Exists(sourcePath) : System.IO.File.Exists(sourcePath);
            if (targetExists && !sourceExists)
                MovePhysicalSource(targetPath, sourcePath, isDirectory);
        }
        catch
        {
            // The following catalog rescan reports any remaining physical conflict.
        }
    }

    private void TryRollbackMetadataMove(
        string sourcePath,
        string targetPath
    ) {
        try
        {
            if (System.IO.File.Exists(targetPath) && !System.IO.File.Exists(sourcePath))
                System.IO.File.Move(targetPath, sourcePath);
        }
        catch
        {
            // The following catalog rescan reports any remaining metadata conflict.
        }
    }

    private void RestoreStagedDeletion(
        string sourcePath,
        string stagedSource,
        bool isDirectory
    ) {
        if (isDirectory ? Directory.Exists(stagedSource) : System.IO.File.Exists(stagedSource))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(sourcePath)!);
            MovePhysicalSource(stagedSource, sourcePath, isDirectory);
        }
    }

    private void RestoreStagedMetadata(
        string metaPath,
        string stagedMetaPath
    ) {
        if (!System.IO.File.Exists(stagedMetaPath))
            return;
        Directory.CreateDirectory(Path.GetDirectoryName(metaPath)!);
        System.IO.File.Move(stagedMetaPath, metaPath);
    }

    private static void DeleteTransactionDirectorySafely(string transactionRoot)
    {
        try
        {
            if (Directory.Exists(transactionRoot))
                Directory.Delete(transactionRoot, recursive: true);
        }
        catch (IOException)
        {
            // Rebuildable transaction debris is retried by later Library maintenance.
        }
        catch (UnauthorizedAccessException)
        {
            // Read-only transaction debris must not roll back an already committed deletion.
        }
    }

}
