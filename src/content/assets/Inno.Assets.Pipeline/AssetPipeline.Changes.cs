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
    /// Reconciles source files, generated files and the persistent catalog.
    /// </summary>
    public void Rescan()
    {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        PruneRetiredObservers();
        GetLoader().Rescan();
        GetFileSystem().Refresh();
    }

    /// <summary>
    /// Applies queued source and build changes on the initialization thread.
    /// </summary>
    public void Update()
    {
        EnsureInitializationThread();
        if (m_sampleImport is not null)
            return;
        using IDisposable operationScope = AcquireOperation();
        PruneRetiredObservers();
        AssetFileSystem fileSystem = GetFileSystem();
        IReadOnlyList<AssetChangedEvent> changes = fileSystem.PollChanges(out bool requiresFullRescan);
        if (m_reconcileSampleImport)
        {
            requiresFullRescan = true;
            m_reconcileSampleImport = false;
        }
        bool registriesChanged = GetLoader().RefreshRegistries();
        if (changes.Count == 0 && !requiresFullRescan && !registriesChanged)
        {
            CollectArtifactsIfDue(GetLoader(), force: false);
            return;
        }
        ApplySourceChanges(changes, requiresFullRescan || registriesChanged);
        CollectArtifactsIfDue(GetLoader(), force: false);
    }

    /// <summary>
    /// Gets indexed source entries.
    /// </summary>
    /// <param name="includeDirectories">
    /// Whether directories should be included.
    /// </param>
    /// <returns>
    /// The stable source entry snapshot.
    /// </returns>
    public IReadOnlyList<AssetFileEntry> GetFileSystemEntries(bool includeDirectories = true)
        => GetFileSystem().GetEntries(includeDirectories);

    /// <summary>
    /// Gets immediate indexed children of a source directory.
    /// </summary>
    /// <param name="parent">
    /// The isolated parent path.
    /// </param>
    /// <returns>
    /// The immediate child entry snapshot.
    /// </returns>
    public IReadOnlyList<AssetFileEntry> GetFileSystemChildren(AssetPath parent) => GetFileSystem().GetChildren(parent);

    /// <summary>
    /// Tries to resolve an indexed source entry.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="entry">
    /// The resolved source entry.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the entry exists and is not generated metadata.
    /// </returns>
    public bool TryGetFileSystemEntry(
        AssetPath path,
        out AssetFileEntry entry
    ) {
        return GetFileSystem().TryGetEntry(path, out entry);
    }

    private void ApplySourceChanges(
        IReadOnlyList<AssetChangedEvent> changes,
        bool requiresFullRescan
    ) {
        AssetLoader? loader = m_loader;
        if (loader is null)
            return;
        Dictionary<string, Guid> previousIds = CapturePreviousIds(loader, changes);
        try
        {
            if (requiresFullRescan)
            {
                loader.Rescan();
                GetFileSystem().Refresh();
            }
            else
                loader.ApplySourceChanges(changes);
            m_diagnostics.ResolveSourceDatabase();
        }
        catch (Exception exception)
        {
            try
            {
                loader.Rescan();
                GetFileSystem().Refresh();
                m_diagnostics.ResolveSourceDatabase();
                m_log.Write(
                    LogLevel.Warn,
                    "Asset source refresh failed and was recovered by a full rescan: {0}",
                    [exception]);
            }
            catch (Exception recoveryException)
            {
                m_log.Write(
                    LogLevel.Error,
                    "Asset source refresh and recovery rescan both failed. Refresh: {0} Recovery: {1}",
                    [exception, recoveryException]);
                m_diagnostics.PublishSourceDatabaseFailure(
                    exception,
                    recoveryException);
            }
        }
        AssetChange[] committed = CreateCommittedChanges(loader, changes, previousIds, requiresFullRescan);
        InvokeObservers(Changed, new AssetChangeSet(Interlocked.Increment(ref m_revision), committed));
    }

    private void DrainPendingChanges(AssetFileSystem fileSystem)
    {
        if (!fileSystem.isWatching)
            return;
        IReadOnlyList<AssetChangedEvent> pending = fileSystem.WaitForIdle(out bool requiresFullRescan);
        if (pending.Count > 0 || requiresFullRescan)
            ApplySourceChanges(pending, requiresFullRescan);
    }

    private AssetChangedEvent[] CreateDeletionEvents(
        AssetFileSystem fileSystem,
        string sourcePath
    ) {
        string prefix = sourcePath + "/";
        return fileSystem.GetEntries()
            .Where(entry =>
                string.Equals(entry.assetPath.ToString(), sourcePath, StringComparison.OrdinalIgnoreCase) ||
                entry.assetPath.ToString().StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .OrderBy(static entry => entry.assetPath.ToString().Length)
            .Select(static entry => new AssetChangedEvent(entry.assetPath.ToString(), WatcherChangeTypes.Deleted))
            .DefaultIfEmpty(new AssetChangedEvent(sourcePath, WatcherChangeTypes.Deleted))
            .ToArray();
    }

    private void OnAssetReloaded(AssetObject asset) => InvokeObservers(AssetReloaded, asset);

    private void InvokeObservers<T>(
        Action<T>? handlers,
        T value
    ) {
        if (handlers is null)
            return;
        foreach (Delegate handler in handlers.GetInvocationList())
        {
            try
            {
                ((Action<T>)handler)(value);
            }
            catch
            {
                // Observer failures cannot roll back committed manager state.
            }
        }
    }

    private void InvokeObservers(Action? handlers)
    {
        if (handlers is null)
            return;
        foreach (Delegate handler in handlers.GetInvocationList())
        {
            try
            {
                ((Action)handler)();
            }
            catch
            {
                // Observer failures cannot roll back committed manager state.
            }
        }
    }

    private AssetFileSystem GetFileSystem()
    {
        EnsureAccess();
        return isInitialized && m_fileSystem is not null
            ? m_fileSystem
            : throw new InvalidOperationException("AssetPipeline is not initialized.");
    }

    private Dictionary<string, Guid> CapturePreviousIds(
        AssetLoader loader,
        IReadOnlyList<AssetChangedEvent> changes
    ) {
        var result = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < changes.Count; i++)
        {
            AssetChangedEvent change = changes[i];
            string path = string.IsNullOrWhiteSpace(change.oldRelativePath)
                ? change.relativePath
                : change.oldRelativePath;
            if (loader.TryGetPersistentId(AssetPath.Parse(path), out Guid id))
                result[path] = id;
        }
        return result;
    }

    private AssetChange[] CreateCommittedChanges(
        AssetLoader loader,
        IReadOnlyList<AssetChangedEvent> changes,
        IReadOnlyDictionary<string, Guid> previousIds,
        bool requiresFullRescan
    ) {
        if (requiresFullRescan && changes.Count == 0)
            return [new AssetChange(AssetChangeKind.StatusChanged, Guid.Empty, AssetPath.Project(string.Empty))];

        var result = new List<AssetChange>(changes.Count);
        for (int i = 0; i < changes.Count; i++)
        {
            AssetChangedEvent change = changes[i];
            bool moved = change.changeType.HasFlag(WatcherChangeTypes.Renamed);
            bool deleted = change.changeType.HasFlag(WatcherChangeTypes.Deleted);
            Guid id = Guid.Empty;
            if (!loader.TryGetPersistentId(AssetPath.Parse(change.relativePath), out id))
            {
                string previousPath = moved ? change.oldRelativePath : change.relativePath;
                _ = previousIds.TryGetValue(previousPath, out id);
            }
            AssetChangeKind kind = moved
                ? AssetChangeKind.Moved
                : deleted
                    ? System.IO.File.Exists(Path.Combine(assetRoot, change.relativePath + ".imeta"))
                        ? AssetChangeKind.Missing
                        : AssetChangeKind.Removed
                    : change.changeType.HasFlag(WatcherChangeTypes.Created)
                        ? AssetChangeKind.Added
                        : AssetChangeKind.Modified;
            result.Add(new AssetChange(
                kind,
                id,
                AssetPath.Parse(change.relativePath),
                string.IsNullOrWhiteSpace(change.oldRelativePath)
                    ? null
                    : AssetPath.Parse(change.oldRelativePath)));
        }
        return result.ToArray();
    }

    private void PublishMutation(AssetChange change)
        => InvokeObservers(
            Changed,
            new AssetChangeSet(Interlocked.Increment(ref m_revision), [change]));

}
