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
    /// Moves a loaded scene to a hierarchy index and records the two integer positions.
    /// </summary>
    /// <param name="scene">
    /// The loaded scene to reorder.
    /// </param>
    /// <param name="sceneIndex">
    /// The requested hierarchy index.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    public void SetSceneIndex(
        GameScene scene,
        int sceneIndex,
        string historyName = "Reorder Scene"
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        int beforeIndex = m_workspace.world.GetSceneIndex(scene);
        int afterIndex;
        try
        {
            m_workspace.world.SetSceneIndex(scene, sceneIndex);
            afterIndex = m_workspace.world.GetSceneIndex(scene);
        }
        catch (Exception exception)
        {
            RollbackAndRethrow(exception, () => m_workspace.world.SetSceneIndex(scene, beforeIndex));
            throw;
        }
        if (beforeIndex == afterIndex)
            return;
        var data = new SceneOrderHistoryData(scene.identity.persistentId, beforeIndex, afterIndex);
        RecordWithRollback(
            () => m_interactions.history.RecordApplied(
                historyName,
                new EditorHistoryChange(
                    SceneHistoryKinds.Order,
                    EditorHistoryPayload.FromBytes(data.Encode()))),
            () => m_workspace.world.SetSceneIndex(scene, beforeIndex));
    }

    /// <summary>
    /// Applies a hierarchy mutation and records only the affected parent and sibling-index tuples.
    /// </summary>
    /// <param name="gameObject">
    /// The primary GameObject being moved.
    /// </param>
    /// <param name="mutation">
    /// The hierarchy mutation to execute through the world-owned operation context.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    /// <param name="relatedObjects">
    /// Additional objects whose placements the mutation may change, such as promoted children.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when at least one placement changed.
    /// </returns>
    public bool ChangeHierarchy(
        GameObject gameObject,
        Action<SceneHierarchyEdit> mutation,
        string historyName = "Move GameObject",
        IReadOnlyCollection<GameObject>? relatedObjects = null
    ) {
        ArgumentNullException.ThrowIfNull(gameObject);
        ArgumentNullException.ThrowIfNull(mutation);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(gameObject.scene);
        GameObject[] affected = (relatedObjects ?? Array.Empty<GameObject>())
            .Prepend(gameObject)
            .DistinctBy(static candidate => candidate.identity.persistentId)
            .ToArray();
        foreach (GameObject affectedObject in affected)
            m_workspace.EnsureEditable(affectedObject.scene);
        SceneObjectPlacement[] before = CapturePlacements(affected);
        SceneObjectPlacement[] after;
        try
        {
            mutation(new SceneHierarchyEdit(m_workspace.world));
            after = CapturePlacements(affected);
        }
        catch (Exception exception)
        {
            RollbackAndRethrow(exception, () => RestorePlacements(before));
            throw;
        }
        if (before.SequenceEqual(after))
            return false;
        var data = new SceneHierarchyHistoryData(
            before,
            after,
            gameObject.identity.persistentId);
        RecordWithRollback(
            () => Record(historyName, SceneHistoryKinds.Hierarchy, data.Encode()),
            () => RestorePlacements(before));
        return true;
    }

    private static SceneObjectPlacement[] CapturePlacements(IReadOnlyList<GameObject> gameObjects)
    {
        var placements = new SceneObjectPlacement[gameObjects.Count];
        for (int i = 0; i < gameObjects.Count; i++)
        {
            GameObject gameObject = gameObjects[i];
            placements[i] = new SceneObjectPlacement(
                gameObject.scene.identity.persistentId,
                gameObject.identity.persistentId,
                gameObject.transform.parent?.gameObject.identity.persistentId,
                gameObject.transform.siblingIndex);
        }
        return placements;
    }

    private void RestorePlacements(IReadOnlyList<SceneObjectPlacement> placements)
    {
        for (int i = 0; i < placements.Count; i++)
        {
            SceneObjectPlacement placement = placements[i];
            GameObject gameObject = m_workspace.Find<GameObject>(placement.objectId)
                ?? throw new InvalidOperationException($"GameObject '{placement.objectId}' is unavailable.");
            GameScene destination = m_workspace.Find<GameScene>(placement.sceneId)
                ?? throw new InvalidOperationException($"Scene '{placement.sceneId}' is unavailable.");
            if (!ReferenceEquals(gameObject.scene, destination))
                m_workspace.world.MoveGameObjectToScene(gameObject, destination);
        }
        for (int i = 0; i < placements.Count; i++)
        {
            SceneObjectPlacement placement = placements[i];
            GameObject gameObject = m_workspace.Find<GameObject>(placement.objectId)!;
            Transform? parent = placement.parentId is Guid parentId
                ? m_workspace.Find<GameObject>(parentId)?.transform
                : null;
            gameObject.transform.SetParent(parent);
        }
        foreach (SceneObjectPlacement placement in placements.OrderBy(static value => value.siblingIndex))
        {
            m_workspace.Find<GameObject>(placement.objectId)!
                .transform.SetSiblingIndex(placement.siblingIndex);
        }
    }

}
