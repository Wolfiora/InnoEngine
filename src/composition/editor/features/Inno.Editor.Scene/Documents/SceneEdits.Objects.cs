using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Linq;

using Inno.Extensibility.Types;
using Inno.Core.Serialization;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Scene;
using Inno.Scene.Components;
using Inno.Scene.Layers;

namespace Inno.Editor.Scene;

sealed partial class SceneEdits
{
    /// <summary>
    /// Creates a GameObject, optionally parents it, and records only the new subtree state.
    /// </summary>
    /// <param name="scene">
    /// The loaded scene that will own the object.
    /// </param>
    /// <param name="parent">
    /// The optional parent transform.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    /// <returns>
    /// The newly created GameObject.
    /// </returns>
    public GameObject CreateGameObject(
        GameScene scene,
        Transform? parent = null,
        string historyName = "Create GameObject"
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(scene);
        if (parent is not null && !ReferenceEquals(parent.gameObject.scene, scene))
            throw new ArgumentException("The parent belongs to another scene.", nameof(parent));
        Guid? selectedBefore = GetSelectionId();
        GameObject gameObject = scene.CreateObject();
        RecordWithRollback(
            () =>
            {
                if (parent is not null)
                    gameObject.transform.SetParent(parent);
                byte[] subtree = SceneSubtreeSerialization.Capture(
                    gameObject,
                    m_workspace.serialization,
                    m_workspace.assets);
                RecordSubtree(
                    historyName,
                    gameObject,
                    existsBefore: false,
                    existsAfter: true,
                    subtree,
                    [],
                    selectedBefore,
                    gameObject.identity.persistentId);
            },
            () =>
            {
                if (gameObject.isRuntimeValid && !scene.DestroyObject(gameObject))
                    throw new InvalidOperationException("The unrecorded GameObject could not be removed.");
            });
        return gameObject;
    }

    /// <summary>
    /// Instantiates a prefab into a loaded scene and records the created subtree as one reversible edit.
    /// </summary>
    /// <param name="prefab">
    /// The imported prefab asset to instantiate.
    /// </param>
    /// <param name="scene">
    /// The loaded scene that will own the instance.
    /// </param>
    /// <param name="parent">
    /// The optional parent transform for the instantiated root.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    /// <returns>
    /// The instantiated prefab root.
    /// </returns>
    public GameObject InstantiatePrefab(
        PrefabAsset prefab,
        GameScene scene,
        Transform? parent = null,
        string historyName = "Instantiate Prefab"
    ) {
        ArgumentNullException.ThrowIfNull(prefab);
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(scene);
        if (!scene.isLoaded)
            throw new InvalidOperationException("A prefab can only be instantiated into a loaded scene.");
        if (parent is not null && !ReferenceEquals(parent.gameObject.scene, scene))
            throw new ArgumentException("The parent belongs to another scene.", nameof(parent));

        Guid? selectedBefore = GetSelectionId();
        GameObject instance = prefab.Instantiate(
            scene,
            m_workspace.serialization,
            m_workspace.assets,
            parent);
        RecordWithRollback(
            () =>
            {
                byte[] subtree = SceneSubtreeSerialization.Capture(
                    instance,
                    m_workspace.serialization,
                    m_workspace.assets);
                RecordSubtree(
                    historyName,
                    instance,
                    existsBefore: false,
                    existsAfter: true,
                    subtree,
                    [],
                    selectedBefore,
                    instance.identity.persistentId);
            },
            () =>
            {
                if (instance.isRuntimeValid && !scene.DestroyObject(instance))
                    throw new InvalidOperationException("The unrecorded prefab instance could not be removed.");
            });
        return instance;
    }

    /// <summary>
    /// Deletes a GameObject subtree and records only that subtree plus incoming serialized references.
    /// </summary>
    /// <param name="gameObject">
    /// The live subtree root to delete.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the subtree was deleted and recorded.
    /// </returns>
    public bool DeleteGameObject(
        GameObject gameObject,
        string historyName = "Delete GameObject"
    ) {
        ArgumentNullException.ThrowIfNull(gameObject);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(gameObject.scene);
        if (!gameObject.isRuntimeValid)
            return false;
        GameScene scene = gameObject.scene;
        byte[] subtree = SceneSubtreeSerialization.Capture(
            gameObject,
            m_workspace.serialization,
            m_workspace.assets);
        SceneIncomingReferenceState[] incoming = SceneReferenceIndex.CaptureIncoming(
            gameObject,
            m_workspace);
        Guid? parentId = gameObject.transform.parent?.gameObject.identity.persistentId;
        int siblingIndex = gameObject.transform.siblingIndex;
        Guid rootId = gameObject.identity.persistentId;
        Guid? selectedBefore = GetSelectionId();
        try
        {
            if (!scene.DestroyObject(gameObject))
                return false;
            var data = new SceneSubtreeHistoryData(
                scene.identity.persistentId,
                rootId,
                parentId,
                siblingIndex,
                existsBefore: true,
                existsAfter: false,
                subtree,
                incoming,
                selectedBefore,
                selectedAfter: null);
            Record(historyName, SceneHistoryKinds.Subtree, data.Encode());
        }
        catch (Exception exception)
        {
            if (m_workspace.Find<GameObject>(rootId) is not null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
            RollbackAndRethrow(exception, () =>
            {
                Transform? parent = parentId is Guid id
                    ? m_workspace.Find<GameObject>(id)?.transform
                    : null;
                _ = SceneSubtreeSerialization.Restore(
                    scene,
                    subtree,
                    m_workspace.serialization,
                    m_workspace.assets,
                    parent,
                    siblingIndex);
                RequireReferenceRestore(SceneReferenceIndex.RestoreIncoming(incoming, m_workspace));
            });
        }
        return true;
    }

}
