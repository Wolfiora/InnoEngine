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

    internal SceneDocumentSnapshot CaptureDocumentSnapshot(GameScene scene)
    {
        ArgumentNullException.ThrowIfNull(scene);
        using IDisposable scope = EnterPresentationScope();
        EnsurePresentedScene(scene);
        SceneDocument? document = null;
        SceneDocumentSnapshot? playBaseline = null;
        if (m_playModeSession is PlayModeLease playModeSession)
        {
            if (playModeSession.TryGetSnapshot(
                    scene.identity.persistentId,
                    out SceneDocumentSnapshot snapshot))
            {
                playBaseline = snapshot;
            }
        }
        else
            document = GetOrCreateDocument(scene);
        SerializationContext serializationContext = AssetSerializationContext.Create(m_assets);
        return new SceneDocumentSnapshot(
            scene.identity.persistentId,
            m_serialization.Serialize(scene, serializationContext),
            document?.sourcePath ?? playBaseline?.sourcePath ?? string.Empty,
            document?.sourceAssetId ?? playBaseline?.sourceAssetId ?? Guid.Empty,
            document?.savedHash.ToArray() ?? playBaseline?.savedHash.ToArray() ?? [],
            document?.isDirty ?? false,
            world.GetSceneIndex(scene),
            document?.nextRefreshTimestamp ?? 0);
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
            m_documents[scene.identity.persistentId] = new SceneDocument(
                scene,
                snapshot.sourcePath,
                snapshot.sourceAssetId,
                snapshot.savedHash.ToArray())
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
        Guid sceneId = scene.identity.persistentId;
        bool closed = world.UnloadScene(scene);
        if (closed && canPersist)
        {
            m_documents.Remove(sceneId);
            m_diagnostics.ResolveSynchronization(sceneId);
            m_diagnostics.ResolveDirtyCheck(sceneId);
        }
        return closed;
    }

    internal void RestoreActiveScene(Guid? activeSceneId)
    {
        using IDisposable scope = EnterPresentationScope();
        if (activeSceneId is Guid sceneId && FindEngineObject(sceneId) is GameScene { isLoaded: true } active)
            world.SetActiveScene(active);
        else if (world.loadedScenes.Count > 0 && world.activeScene is null)
            world.SetActiveScene(world.loadedScenes[0]);
    }

    internal void RestoreSelection(
        Guid? selectedId,
        bool selectActiveSceneWhenMissing = true
    ) {
        if (m_selection is null)
            return;
        object? target = selectedId is Guid id ? FindEngineObject(id) : null;
        if (target is null && selectActiveSceneWhenMissing)
            target = world.activeScene;
        m_selection.SetSelection(target);
    }

    private EngineObject? FindEngineObject(Guid id) => world.Find<EngineObject>(id);

    private Guid? GetSelectedSceneObjectId()
        => m_selection?.selectedTarget is EngineObject { isDestroyed: false } selected
            ? selected.identity.persistentId
            : null;

    internal sealed record SceneDocumentSnapshot(
        Guid sceneId,
        byte[] payload,
        string sourcePath,
        Guid sourceAssetId,
        byte[] savedHash,
        bool isDirty,
        int sceneIndex,
        long nextRefreshTimestamp = 0
    );

}
