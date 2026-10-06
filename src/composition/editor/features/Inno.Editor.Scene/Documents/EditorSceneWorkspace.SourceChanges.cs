using Inno.Extensibility.Reload;
using System;
using Inno.References;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Extensibility.Modules;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Extensibility.Types;
using Inno.Core.Serialization;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scene.Components;

namespace Inno.Editor.Scene;

internal sealed partial class EditorSceneWorkspace

{

    private void SynchronizeSource(
        GameScene scene,
        SceneDocument document
    ) {
        if (document.sourceAssetId == Guid.Empty)
            return;
        if (!m_assets.TryLoad(document.sourceAssetId, out SceneAsset? asset) ||
            asset is null ||
            asset.isMissing)
        {
            document.sourcePath = string.Empty;
            document.sourceAssetId = Guid.Empty;
            document.isDirty = true;
            return;
        }

        string sourcePath = NormalizePath(asset.assetPath.ToString());
        string sourceName = GetAssetName(sourcePath);
        bool pathChanged = !string.Equals(document.sourcePath, sourcePath, StringComparison.Ordinal);
        if (!pathChanged)
            return;

        bool wasDirty = HasSerializedChanges(scene, document);
        document.sourcePath = sourcePath;
        scene.name = sourceName;
        if (!wasDirty)
            document.savedHash = ComputeSceneHash(scene);
        document.isDirty = wasDirty;
        document.nextRefreshTimestamp = Stopwatch.GetTimestamp() +
                                        (long)(Stopwatch.Frequency * C_DIRTY_REFRESH_SECONDS);
    }

    private void ApplyRename(
        string oldRelativePath,
        string newRelativePath
    ) {
        string oldPath = NormalizePath(oldRelativePath);
        string newPath = NormalizePath(newRelativePath);
        if (m_assets.TryGetFileSystemEntry(AssetPath.Parse(newPath), out Inno.Assets.Pipeline.AssetFileEntry entry) &&
            entry.isDirectory)
        {
            ApplyDirectoryRename(oldPath, newPath);
            return;
        }
        string extension = Path.GetExtension(newPath);
        if (string.Equals(extension, C_SCENE_EXTENSION, StringComparison.OrdinalIgnoreCase))
        {
            ApplySceneRename(oldPath, newPath);
            return;
        }
        if (string.Equals(extension, C_PREFAB_EXTENSION, StringComparison.OrdinalIgnoreCase))
            ApplyPrefabRename(oldPath, newPath);
    }

    private void ApplySceneRename(
        string oldPath,
        string newPath
    ) {
        foreach (SceneDocument document in m_documents.Values)
        {
            if (!string.Equals(document.sourcePath, oldPath, StringComparison.OrdinalIgnoreCase) ||
                document.scene.isDestroyed)
            {
                continue;
            }

            RelocateSceneDocument(document, newPath);
        }
    }

    private void ApplyDirectoryRename(
        string oldPath,
        string newPath
    ) {
        string oldPrefix = oldPath + "/";
        foreach (SceneDocument document in m_documents.Values)
        {
            if (document.scene.isDestroyed ||
                !document.sourcePath.StartsWith(oldPrefix, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string suffix = document.sourcePath[oldPrefix.Length..];
            RelocateSceneDocument(document, Combine(newPath, suffix));
        }
    }

    private void RelocateSceneDocument(
        SceneDocument document,
        string newPath
    ) {
        bool wasDirty = HasSerializedChanges(document.scene, document);
        document.sourcePath = newPath;
        document.scene.name = GetAssetName(newPath);
        if (m_assets.TryGetPersistentId(AssetPath.Parse(newPath), out Guid sourceAssetId))
            document.sourceAssetId = sourceAssetId;
        if (!wasDirty)
            document.savedHash = ComputeSceneHash(document.scene);
        document.isDirty = wasDirty;
        document.nextRefreshTimestamp = Stopwatch.GetTimestamp() +
                                        (long)(Stopwatch.Frequency * C_DIRTY_REFRESH_SECONDS);
    }

    private string RenameSceneSourceIfNeeded(
        GameScene scene,
        SceneDocument document
    ) {
        string currentPath = NormalizePath(document.sourcePath);
        string directory = NormalizePath(Path.GetDirectoryName(currentPath));
        string targetPath = Combine(directory, SanitizeFileName(scene.name) + C_SCENE_EXTENSION);
        if (string.Equals(currentPath, targetPath, StringComparison.Ordinal))
            return currentPath;
        if (m_assets.TryGetFileSystemEntry(AssetPath.Parse(targetPath), out _))
        {
            throw new IOException(
                $"Scene asset '{targetPath}' already exists. Choose a different scene name before saving.");
        }

        m_assets.Move(AssetPath.Parse(currentPath), AssetPath.Parse(targetPath));
        document.sourcePath = targetPath;
        return targetPath;
    }

    private void ApplyPrefabRename(
        string oldPath,
        string newPath
    ) {
        if (!m_assets.TryGetPersistentId(AssetPath.Parse(newPath), out Guid sourceAssetId))
            return;
        string oldName = GetAssetName(oldPath);
        string newName = GetAssetName(newPath);
        IReadOnlyList<GameScene> scenes = m_runtimeSession.scenes.loadedScenes;
        for (int sceneIndex = 0; sceneIndex < scenes.Count; sceneIndex++)
        {
            IReadOnlyList<GameObject> objects = scenes[sceneIndex].GetObjects();
            for (int objectIndex = 0; objectIndex < objects.Count; objectIndex++)
            {
                GameObject gameObject = objects[objectIndex];
                PrefabInstanceInfo? prefab = gameObject.prefabInstance;
                if (prefab?.isRoot == true &&
                    prefab.sourceAssetId == sourceAssetId &&
                    string.Equals(gameObject.name, oldName, StringComparison.Ordinal))
                {
                    gameObject.name = newName;
                }
            }
        }
    }

    private void OnAssetDatabaseChanged(AssetChangeSet changeSet)
    {
        m_waitingTypeCatalogVersion = -1;
        for (int i = 0; i < changeSet.changes.Count; i++)
            m_sourceChanges.Enqueue(changeSet.changes[i]);
    }

    private void ApplyPendingSourceChanges()
    {
        while (m_sourceChanges.TryDequeue(out AssetChange change))
        {
            try
            {
                if (change.kind == AssetChangeKind.Moved)
                    ApplyRename(
                        change.previousAssetPath?.ToString() ?? string.Empty,
                        change.assetPath.ToString());
            }
            catch (Exception exception)
            {
                m_log.Write(LogLevel.Error, "Editor asset rename synchronization failed: {0}", [exception]);
            }
        }
    }

    private void SynchronizeReplacedScenes()
    {
        IReadOnlyList<GameScene> loaded = m_runtimeSession.scenes.loadedScenes;
        for (int i = 0; i < loaded.Count; i++)
        {
            GameScene scene = loaded[i];
            if (m_documents.TryGetValue(scene.identity.persistentId, out SceneDocument? document) &&
                !ReferenceEquals(document.scene, scene))
            {
                document.scene = scene;
            }
        }
    }

}
