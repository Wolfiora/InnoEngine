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

    /// <summary>
    /// Captures an immutable snapshot of the current observable state.
    /// </summary>
    /// <param name="state">
    /// The lifecycle or domain state applied by this operation.
    /// </param>
    protected override void Capture(EditorState state)
    {
        if (m_playModeSession is PlayModeLease playModeSession)
        {
            playModeSession.Capture(state);
            return;
        }
        if (m_pendingScenePaths is not null)
        {
            state.Set("openScenes", m_pendingScenePaths);
            state.Set("activeScene", m_pendingActivePath);
            return;
        }
        string[] scenePaths = m_runtimeSession.scenes.loadedScenes
            .Select(scene => TryGetSourcePath(scene, out string path) ? path : string.Empty)
            .Where(static path => !string.IsNullOrEmpty(path))
            .ToArray();
        state.Set("openScenes", scenePaths);
        if (m_runtimeSession.scenes.activeScene is GameScene active && TryGetSourcePath(active, out string activePath))
            state.Set("activeScene", activePath);
    }

    /// <summary>
    /// Restores the supplied snapshot while preserving current invariants.
    /// </summary>
    /// <param name="state">
    /// The lifecycle or domain state applied by this operation.
    /// </param>
    protected override void Restore(EditorState state)
    {
        string[] paths = state.Get("openScenes", Array.Empty<string>());
        m_pendingActivePath = state.Get("activeScene", string.Empty);
        if (paths.Length == 0)
        {
            m_pendingScenePaths = null;
            m_diagnostics.ResolveRestore();
            return;
        }
        m_pendingScenePaths = paths
            .Select(NormalizePath)
            .Where(static path => !string.IsNullOrEmpty(path))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        m_nextRestoreAttemptTimestamp = 0;
        m_waitingTypeCatalogVersion = -1;
    }

    private void TryRestorePendingScenes()
    {
        if (m_pendingScenePaths is null)
        {
            m_diagnostics.ResolveRestore();
            return;
        }
        long typeCatalogVersion = m_types.current.version;
        if (m_waitingTypeCatalogVersion == typeCatalogVersion)
            return;
        long now = Stopwatch.GetTimestamp();
        if (now < m_nextRestoreAttemptTimestamp)
            return;
        m_nextRestoreAttemptTimestamp = now + Stopwatch.Frequency / 4;

        var candidates = new List<(GameScene Scene, string Path, Guid AssetId, byte[] Hash)>();
        bool waitingForSourceIndex = false;
        try
        {
            for (int i = 0; i < m_pendingScenePaths.Length; i++)
            {
                string path = m_pendingScenePaths[i];
                if (!m_assets.TryGetFileSystemEntry(AssetPath.Parse(path), out _))
                {
                    string absolutePath = Path.Combine(
                        m_assets.assetRoot,
                        path.Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(absolutePath))
                    {
                        waitingForSourceIndex = true;
                        break;
                    }
                    m_log.Write(LogLevel.Warn, "Editor scene workspace skipped missing scene '{0}'.", [path]);
                    continue;
                }
                SceneAsset asset = m_assets.Load<SceneAsset>(AssetPath.Parse(path));
                GameScene scene = asset.Instantiate(m_serialization, m_assets);
                scene.name = GetAssetName(path);
                candidates.Add((
                    scene,
                    path,
                    asset.identity.persistentId,
                    ComputeSceneHash(scene)));
            }
        }
        catch (SceneTypeResolutionException exception)
        {
            DisposeRestoreCandidates(candidates);
            m_waitingTypeCatalogVersion = typeCatalogVersion;
            string message =
                $"{exception.elementKind} stable type id '{exception.stableTypeId}' " +
                "is not present in the active type catalog.";
            if (m_diagnostics.PublishRestoreFailure("SCENE-TYPE", message))
            {
                m_log.Write(
                    LogLevel.Error,
                    "Editor scene workspace cannot restore saved scenes because {0}",
                    [message]);
            }
            return;
        }
        catch (Exception exception)
        {
            DisposeRestoreCandidates(candidates);
            m_waitingTypeCatalogVersion = typeCatalogVersion;
            if (m_diagnostics.PublishRestoreFailure("SCENE-RESTORE", exception.Message))
                m_log.Write(
                    LogLevel.Error,
                    "Editor scene workspace could not restore saved scenes: {0}",
                    [exception]);
            return;
        }
        if (waitingForSourceIndex)
        {
            DisposeRestoreCandidates(candidates);
            return;
        }

        m_runtimeSession.scenes.UnloadAllScenes();
        m_documents.Clear();
        for (int i = 0; i < candidates.Count; i++)
        {
            (GameScene scene, string path, Guid assetId, byte[] hash) = candidates[i];
            m_runtimeSession.scenes.LoadSceneAdditive(scene, makeActive: false);
            m_documents.Add(
                scene.identity.persistentId,
                new SceneDocument(scene, path, assetId, hash));
        }
        SceneDocument? activeDocument = m_documents.Values.FirstOrDefault(document =>
            string.Equals(document.sourcePath, m_pendingActivePath, StringComparison.OrdinalIgnoreCase));
        if (activeDocument is not null && activeDocument.scene.isLoaded)
            m_runtimeSession.scenes.SetActiveScene(activeDocument.scene);
        m_selection?.SetSelection(null);
        m_pendingScenePaths = null;
        m_waitingTypeCatalogVersion = -1;
        m_diagnostics.ResolveRestore();
    }

    private void DisposeRestoreCandidates(IReadOnlyList<(GameScene Scene, string Path, Guid AssetId, byte[] Hash)> candidates)
    {
        for (int i = candidates.Count - 1; i >= 0; i--)
        {
            if (candidates[i].Scene.isDestroyed)
                continue;
            m_runtimeSession.scenes.LoadSceneAdditive(candidates[i].Scene, makeActive: false);
            _ = m_runtimeSession.scenes.UnloadScene(candidates[i].Scene);
        }
    }

}
