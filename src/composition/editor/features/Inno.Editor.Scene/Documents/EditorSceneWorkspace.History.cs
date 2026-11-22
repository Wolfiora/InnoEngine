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
    internal SceneDocumentSnapshot CaptureDocumentSnapshot(GameScene scene)
    {
        ArgumentNullException.ThrowIfNull(scene);
        using IDisposable scope = EnterPresentationScope();
        EnsurePresentedScene(scene);
        SceneDocument? document = null;
        SceneDocumentSnapshot? playBaseline = null;
        if (m_playModeSession is PlayModeLease playModeSession)
        {
            if (playModeSession.TryGetSnapshot(scene.identity.persistentID, out SceneDocumentSnapshot snapshot))
            {
                playBaseline = snapshot;
            }
        }
        else
        {
            document = GetOrCreateDocument(scene);
        }
        SerializationContext serializationContext = AssetSerializationContext.Create(m_assets);
        return new SceneDocumentSnapshot(
            scene.identity.persistentID,
            m_serialization.Serialize(scene, serializationContext),
            document?.sourcePath ?? playBaseline?.sourcePath ?? string.Empty,
            document?.sourceAssetID ?? playBaseline?.sourceAssetID ?? Guid.Empty,
            document?.savedHash.ToArray() ?? playBaseline?.savedHash.ToArray() ?? [],
            document?.isDirty ?? false,
            world.GetSceneIndex(scene),
            document?.nextRefreshTimestamp ?? 0
        );
    }

    internal GameScene RestoreDocumentSnapshot(SceneDocumentSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        using IDisposable scope = EnterPresentationScope();
        SerializationContext serializationContext = AssetSerializationContext.Create(m_assets);
        GameScene scene = m_serialization.Deserialize<GameScene>(snapshot.payload, serializationContext);
        world.LoadSceneAdditive(scene, makeActive: false);
        world.SetSceneIndex(scene, snapshot.sceneIndex);
        if (canPersist)
        {
            m_documents[scene.identity.persistentID] = new SceneDocument(
                scene,
                snapshot.sourcePath,
                snapshot.sourceAssetID,
                snapshot.savedHash.ToArray()
            )
            {
                isDirty = snapshot.isDirty,
                nextRefreshTimestamp = snapshot.nextRefreshTimestamp
            };
        }
        return scene;
    }

    internal bool CloseDocumentForHistory(GameScene scene)
    {
        ArgumentNullException.ThrowIfNull(scene);
        using IDisposable scope = EnterPresentationScope();
        Guid sceneID = scene.identity.persistentID;
        bool closed = world.UnloadScene(scene);
        if (closed && canPersist)
        {
            m_documents.Remove(sceneID);
            m_diagnostics.ResolveSynchronization(sceneID);
            m_diagnostics.ResolveDirtyCheck(sceneID);
        }
        return closed;
    }

    internal void RestoreActiveScene(Guid? activeSceneID)
    {
        using IDisposable scope = EnterPresentationScope();
        if (activeSceneID is Guid sceneID && FindEngineObject(sceneID) is GameScene { isLoaded: true } active)
        {
            world.SetActiveScene(active);
        }
        else if (world.loadedScenes.Count > 0 && world.activeScene is null)
        {
            world.SetActiveScene(world.loadedScenes[0]);
        }
    }

    internal void RestoreSelection(
        Guid? selectedID,
        bool selectActiveSceneWhenMissing = true
    ) {
        if (m_selection is null)
        {
            return;
        }
        object? target = selectedID is Guid id ? FindEngineObject(id) : null;
        if (target is null && selectActiveSceneWhenMissing)
        {
            target = world.activeScene;
        }
        m_selection.SetSelection(target);
    }

    private EngineObject? FindEngineObject(Guid id) => world.Find<EngineObject>(id);

    private Guid? GetSelectedSceneObjectID()
        => m_selection?.selectedTarget is EngineObject { isDestroyed: false } selected
           ? selected.identity.persistentID
           : null;

    internal sealed record SceneDocumentSnapshot(
        Guid sceneID,
        byte[] payload,
        string sourcePath,
        Guid sourceAssetID,
        byte[] savedHash,
        bool isDirty,
        int sceneIndex,
        long nextRefreshTimestamp = 0
    );
}
