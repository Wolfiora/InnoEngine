using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.References;
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
                nameof(runtimeSession)
            );
        }
        SceneWorld runtimeWorld = runtimeSession.scenes;
        if (m_isPreparingPlayMode || m_playModeSession is not null)
        {
            throw new InvalidOperationException("An editor Play Mode scene session is already active.");
        }
        if (runtimeWorld.loadedScenes.Count != 0)
        {
            throw new InvalidOperationException("A Play Mode scene snapshot requires an empty runtime world.");
        }

        m_isPreparingPlayMode = true;
        try
        {
            Refresh();
            SceneDocumentSnapshot[] snapshots = m_runtimeSession.scenes.loadedScenes
                .Select(CaptureDocumentSnapshot)
                .OrderBy(static snapshot => snapshot.sceneIndex)
                .ToArray();
            Guid? activeSceneID = m_runtimeSession.scenes.activeScene?.identity.persistentID;
            Guid? selectedSceneObjectID = GetSelectedSceneObjectID();
            var session = new PlayModeLease(
                this,
                runtimeSession,
                snapshots,
                activeSceneID,
                selectedSceneObjectID,
                m_selection?.RegisterIdentityDomain(runtimeSession.identities)
            );
            try
            {
                MaterializeRuntimeSceneSet(session);
                m_playModeSession = session;
                if (selectedSceneObjectID is not null)
                {
                    RestoreSelection(selectedSceneObjectID);
                }
                return session;
            }
            catch (Exception startupFailure)
            {
                try
                {
                    session.Dispose();
                }
                catch (Exception cleanupFailure)
                {
                    throw new AggregateException(
                        "Play scene preparation and identity routing retirement failed.",
                        startupFailure,
                        cleanupFailure
                    );
                }
                throw;
            }
        }
        finally
        {
            m_isPreparingPlayMode = false;
        }
    }

    private void ReleasePlayModeLease(PlayModeLease session)
    {
        if (!ReferenceEquals(m_playModeSession, session))
        {
            return;
        }
        object? runtimeSelection = m_selection?.selectedTarget;
        Guid? runtimeSelectionID = runtimeSelection is EngineObject { isDestroyed: false } selected
                                   ? selected.identity.persistentID
                                   : null;
        m_playModeSession = null;
        Guid? editSelectionID = runtimeSelectionID is Guid currentID
                                && m_runtimeSession.scenes.Find<EngineObject>(currentID) is not null
                                ? currentID
                                : session.editSelectionID;
        try
        {
            if (runtimeSelection is EngineObject || runtimeSelection is null && session.editSelectionID is not null)
            {
                RestoreSelection(editSelectionID, selectActiveSceneWhenMissing: false);
            }
        }
        catch
        {
            m_playModeSession = session;
            throw;
        }
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
                GameScene runtimeScene = m_serialization.Deserialize<GameScene>(snapshot.payload, serializationContext);
                runtimeWorld.LoadSceneAdditive(runtimeScene, makeActive: false);
                runtimeWorld.SetSceneIndex(runtimeScene, snapshot.sceneIndex);
            }
            if (session.activeSceneID is Guid activeSceneID)
            {
                GameScene? activeScene = runtimeWorld.loadedScenes.FirstOrDefault(
                    scene => scene.identity.persistentID == activeSceneID
                );
                if (activeScene is not null)
                {
                    runtimeWorld.SetActiveScene(activeScene);
                }
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
                    cleanupFailure
                );
            }
            throw new InvalidOperationException(
                "The Play Mode start snapshot could not be materialized in its isolated runtime world.",
                exception
            );
        }
    }

    private sealed class PlayModeLease(
        EditorSceneWorkspace workspace,
        RuntimeSession playSession,
        SceneDocumentSnapshot[] sceneSnapshots,
        Guid? activeScene,
        Guid? selectedSceneObject,
        IDisposable? identityDomain
    ) : IDisposable
    {
        private readonly Dictionary<Guid, SceneDocumentSnapshot> m_snapshotBySceneID =
            sceneSnapshots.ToDictionary(static snapshot => snapshot.sceneID);

        private bool m_disposed;

        /// <summary>
        /// Releases the resources owned by this instance.
        /// </summary>
        public void Dispose()
        {
            if (m_disposed)
            {
                return;
            }
            workspace.ReleasePlayModeLease(this);
            identityDomain?.Dispose();
            m_disposed = true;
        }

        internal IReadOnlyList<SceneDocumentSnapshot> snapshots { get; } = sceneSnapshots;

        internal Guid? activeSceneID { get; } = activeScene;

        internal Guid? editSelectionID { get; } = selectedSceneObject;

        internal RuntimeSession runtimeSession { get; } = playSession;

        internal SceneWorld runtimeWorld => runtimeSession.scenes;

        internal void Capture(EditorState state)
        {
            state.Set(
                "openScenes",
                sceneSnapshots
                    .Select(static snapshot => snapshot.sourcePath)
                    .Where(static path => !string.IsNullOrEmpty(path))
                    .ToArray()
            );
            string activePath = activeSceneID is Guid activeID
                                && m_snapshotBySceneID.TryGetValue(activeID, out SceneDocumentSnapshot? active)
                                ? active.sourcePath
                                : string.Empty;
            state.Set("activeScene", activePath);
        }

        internal bool TryGetSnapshot(
            Guid sceneID,
            out SceneDocumentSnapshot snapshot
        ) => m_snapshotBySceneID.TryGetValue(sceneID, out snapshot!);
    }
}
