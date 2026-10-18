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

    IDisposable IEditorScenePlayMode.BeginPlayMode(RuntimeSession runtimeSession)
    {
        ArgumentNullException.ThrowIfNull(runtimeSession);
        if (runtimeSession.options.kind != RuntimeSessionKind.Play)
        {
            throw new ArgumentException(
                "An editor Play Mode scene session requires a Play runtime session.",
                nameof(runtimeSession));
        }
        SceneWorld runtimeWorld = runtimeSession.scenes;
        if (m_isPreparingPlayMode || m_playModeSession is not null)
            throw new InvalidOperationException("An editor Play Mode scene session is already active.");
        if (runtimeWorld.loadedScenes.Count != 0)
            throw new InvalidOperationException("A Play Mode scene snapshot requires an empty runtime world.");

        m_isPreparingPlayMode = true;
        try
        {
            Refresh();
            SceneDocumentSnapshot[] snapshots = m_runtimeSession.scenes.loadedScenes
                .Select(CaptureDocumentSnapshot)
                .OrderBy(static snapshot => snapshot.sceneIndex)
                .ToArray();
            Guid? activeSceneId = m_runtimeSession.scenes.activeScene?.identity.persistentId;
            Guid? selectedSceneObjectId = GetSelectedSceneObjectId();
            var session = new PlayModeLease(
                this,
                runtimeSession,
                snapshots,
                activeSceneId,
                selectedSceneObjectId);
            MaterializeRuntimeSceneSet(session);
            m_playModeSession = session;
            if (selectedSceneObjectId is not null)
                RestoreSelection(selectedSceneObjectId);
            return session;
        }
        finally
        {
            m_isPreparingPlayMode = false;
        }
    }

    private void ReleasePlayModeLease(PlayModeLease session)
    {
        if (!ReferenceEquals(m_playModeSession, session))
            return;
        object? runtimeSelection = m_selection?.selectedTarget;
        Guid? runtimeSelectionId = runtimeSelection is EngineObject { isDestroyed: false } selected
            ? selected.identity.persistentId
            : null;
        m_playModeSession = null;
        Guid? editSelectionId = runtimeSelectionId is Guid currentId &&
                                m_runtimeSession.scenes.Find<EngineObject>(currentId) is not null
            ? currentId
            : session.editSelectionId;
        if (runtimeSelection is EngineObject || runtimeSelection is null && session.editSelectionId is not null)
            RestoreSelection(editSelectionId, selectActiveSceneWhenMissing: false);
    }

    private void MaterializeRuntimeSceneSet(PlayModeLease session)
    {
        RuntimeSession runtimeSession = session.runtimeSession;
        SceneWorld runtimeWorld = runtimeSession.scenes;
        using IDisposable runtimeScope = runtimeSession.EnterExecutionScope();
        try
        {
            IReadOnlyList<SceneDocumentSnapshot> snapshots = session.snapshots;
            SerializationContext serializationContext = AssetSerializationContext.Create(m_assets);
            for (int i = 0; i < snapshots.Count; i++)
            {
                SceneDocumentSnapshot snapshot = snapshots[i];
                GameScene runtimeScene = m_serialization.Deserialize<GameScene>(
                    snapshot.payload,
                    serializationContext);
                runtimeWorld.LoadSceneAdditive(runtimeScene, makeActive: false);
                runtimeWorld.SetSceneIndex(runtimeScene, snapshot.sceneIndex);
            }
            if (session.activeSceneId is Guid activeSceneId)
            {
                GameScene? activeScene = runtimeWorld.loadedScenes.FirstOrDefault(
                    scene => scene.identity.persistentId == activeSceneId);
                if (activeScene is not null)
                    runtimeWorld.SetActiveScene(activeScene);
            }
        }
        catch (Exception exception)
        {
            try
            {
                runtimeWorld.UnloadAllScenes();
            }
            catch (Exception cleanupFailure)
            {
                throw new AggregateException(
                    "The Play Mode scene set could not be constructed and its partial state could not be released.",
                    exception,
                    cleanupFailure);
            }
            throw new InvalidOperationException(
                "The Play Mode start snapshot could not be materialized in its isolated runtime world.",
                exception);
        }
    }

    private sealed class PlayModeLease(
        EditorSceneWorkspace workspace,
        RuntimeSession playSession,
        SceneDocumentSnapshot[] sceneSnapshots,
        Guid? activeScene,
        Guid? selectedSceneObject
    ) : IDisposable
    {
        private readonly Dictionary<Guid, SceneDocumentSnapshot> m_snapshotBySceneId =
            sceneSnapshots.ToDictionary(static snapshot => snapshot.sceneId);

        internal IReadOnlyList<SceneDocumentSnapshot> snapshots { get; } = sceneSnapshots;

        internal Guid? activeSceneId { get; } = activeScene;

        internal Guid? editSelectionId { get; } = selectedSceneObject;

        internal RuntimeSession runtimeSession { get; } = playSession;

        internal SceneWorld runtimeWorld => runtimeSession.scenes;

        private bool m_disposed;

        /// <summary>
        /// Releases the resources owned by this instance.
        /// </summary>
        public void Dispose()
        {
            if (m_disposed)
                return;
            m_disposed = true;
            workspace.ReleasePlayModeLease(this);
        }

        internal void Capture(EditorState state)
        {
            state.Set(
                "openScenes",
                sceneSnapshots
                    .Select(static snapshot => snapshot.sourcePath)
                    .Where(static path => !string.IsNullOrEmpty(path))
                    .ToArray());
            string activePath = activeSceneId is Guid activeId &&
                                m_snapshotBySceneId.TryGetValue(activeId, out SceneDocumentSnapshot? active)
                ? active.sourcePath
                : string.Empty;
            state.Set("activeScene", activePath);
        }

        internal bool TryGetSnapshot(
            Guid sceneId,
            out SceneDocumentSnapshot snapshot
        )
            => m_snapshotBySceneId.TryGetValue(sceneId, out snapshot!);
    }

}
