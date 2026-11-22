using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Scene;
using Inno.Scene.Components;

namespace Inno.Scene;

internal static class PrefabOverrideProcessor
{
    internal static PrefabOverrideSet Capture(
        PrefabConnectionRecord connection,
        GameScene scene,
        SerializationContext context,
        SceneReferenceScope storageReferences
    ) {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(storageReferences);
        if (connection.sourceAsset.isMissing || connection.sourceAsset.runtimePayload.IsEmpty)
        {
            return connection.overrides;
        }

        (GameScene sourceScene, _, PrefabConnectionRecord sourceConnection) = InstantiateSource(connection.sourceAsset, context);
        try
        {
            SceneGraphReferenceMap currentReferences = CreateComparisonReferences(scene, connection);
            SceneGraphReferenceMap sourceReferences = CreateComparisonReferences(sourceScene, sourceConnection);
            var overrides = new PrefabOverrideSet();

            CaptureObjectOverrides(connection, sourceConnection, scene, sourceScene, overrides);
            CaptureAdditions(connection, scene, overrides);
            CaptureComponentOverrides(
                connection,
                sourceConnection,
                scene,
                sourceScene,
                context,
                currentReferences,
                sourceReferences,
                storageReferences,
                overrides
            );
            PreserveOrphanedOverrides(connection.overrides, sourceConnection, overrides);
            connection.overrides = overrides;
            return overrides;
        }
        finally
        {
            if (!sourceScene.isDestroyed)
            {
                sourceScene.Unload();
            }
        }
    }

    internal static void Reconcile(
        PrefabConnectionRecord connection,
        GameObject instanceRoot,
        SerializationContext context
    ) {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(instanceRoot);
        ArgumentNullException.ThrowIfNull(context);
        if (connection.sourceAsset.isMissing || connection.sourceAsset.runtimePayload.IsEmpty)
        {
            RefreshInstanceInfo(connection, instanceRoot);
            return;
        }

        GameScene scene = instanceRoot.scene;
        (GameScene sourceScene, _, PrefabConnectionRecord sourceConnection) = InstantiateSource(connection.sourceAsset, context);
        try
        {
            MarkOrphanedOverrides(connection, sourceConnection, sourceScene, context);
            RemoveDeletedSourceObjects(connection, sourceConnection, scene, instanceRoot);
            CreateNewSourceObjects(connection, sourceConnection, scene, sourceScene);
            RemoveDeletedSourceComponents(connection, sourceConnection, scene);
            CreateNewSourceComponents(connection, sourceConnection, scene, sourceScene);

            SceneGraphReferenceMap sourceReferences = CreateComparisonReferences(sourceScene, sourceConnection);
            SceneGraphReferenceMap targetReferences = CreateTargetReferences(scene, connection);
            RestoreSourceProperties(
                connection,
                sourceConnection,
                scene,
                sourceScene,
                context,
                sourceReferences,
                targetReferences
            );
            RestoreSourceObjectState(connection, sourceConnection, scene, sourceScene, instanceRoot);
            RefreshInstanceInfo(connection, instanceRoot);
        }
        finally
        {
            if (!sourceScene.isDestroyed)
            {
                sourceScene.Unload();
            }
        }
    }

    private static void CaptureObjectOverrides(
        PrefabConnectionRecord connection,
        PrefabConnectionRecord sourceConnection,
        GameScene scene,
        GameScene sourceScene,
        PrefabOverrideSet overrides
    ) {
        foreach ((Guid sourceID, Guid sourceRuntimeID) in sourceConnection.objectIdentities)
        {
            GameObject? sourceObject = sourceScene.FindObject(sourceRuntimeID);
            GameObject? currentObject = connection.objectIdentities.TryGetValue(sourceID, out Guid currentRuntimeID)
                                        ? scene.FindObject(currentRuntimeID)
                                        : null;
            if (sourceObject is null)
            {
                continue;
            }
            if (currentObject is null)
            {
                overrides.MarkObjectRemoved(sourceID);
                continue;
            }

            PrefabObjectOverrideKind kind = PrefabObjectOverrideKind.None;
            if (!string.Equals(currentObject.name, sourceObject.name, StringComparison.Ordinal))
            {
                kind |= PrefabObjectOverrideKind.Name;
            }
            if (!string.Equals(currentObject.tag, sourceObject.tag, StringComparison.Ordinal))
            {
                kind |= PrefabObjectOverrideKind.Tag;
            }
            if (currentObject.activeSelf != sourceObject.activeSelf)
            {
                kind |= PrefabObjectOverrideKind.ActiveSelf;
            }
            if (GetMappedParentID(currentObject, connection) != GetMappedParentID(sourceObject, sourceConnection))
            {
                kind |= PrefabObjectOverrideKind.Parent;
            }
            if (currentObject.transform.siblingIndex != sourceObject.transform.siblingIndex)
            {
                kind |= PrefabObjectOverrideKind.SiblingIndex;
            }
            overrides.SetStructure(sourceID, kind);
        }
    }

    private static void CaptureComponentOverrides(
        PrefabConnectionRecord connection,
        PrefabConnectionRecord sourceConnection,
        GameScene scene,
        GameScene sourceScene,
        SerializationContext context,
        SceneGraphReferenceMap currentReferences,
        SceneGraphReferenceMap sourceReferences,
        SceneReferenceScope storageReferences,
        PrefabOverrideSet overrides
    ) {
        foreach ((Guid sourceID, Guid sourceRuntimeID) in sourceConnection.componentIdentities)
        {
            GameComponent? sourceComponent = sourceScene.FindComponent(sourceRuntimeID);
            GameComponent? currentComponent = connection.componentIdentities.TryGetValue(sourceID, out Guid currentRuntimeID)
                                              ? scene.FindComponent(currentRuntimeID)
                                              : null;
            if (sourceComponent is null)
            {
                continue;
            }
            if (currentComponent is null || currentComponent.GetType() != sourceComponent.GetType())
            {
                overrides.MarkComponentRemoved(sourceID);
                continue;
            }

            IReadOnlyList<SerializationMemberMetadata> members = SerializedPropertyValueCodec.GetMembers(
                sourceComponent.GetType(),
                context
            );
            for (int memberIndex = 0; memberIndex < members.Count; memberIndex++)
            {
                SerializationMemberMetadata member = members[memberIndex];
                byte[] sourceValue;
                byte[] currentValue;
                try
                {
                    sourceValue = SerializedPropertyValueCodec.Encode(
                        member,
                        sourceComponent,
                        context,
                        sourceReferences
                    );
                    currentValue = SerializedPropertyValueCodec.Encode(
                        member,
                        currentComponent,
                        context,
                        currentReferences
                    );
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException(
                        $"Failed to compare prefab override for GameComponent "
                        + $"'{currentComponent.GetType().FullName}.{member.name}' on GameObject "
                        + $"'{currentComponent.gameObject.name}'.",
                        exception
                    );
                }
                if (sourceValue.AsSpan().SequenceEqual(currentValue))
                {
                    continue;
                }
                byte[] persistedValue;
                try
                {
                    persistedValue = SerializedPropertyValueCodec.Encode(
                        member,
                        currentComponent,
                        context,
                        storageReferences
                    );
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException(
                        $"Failed to persist prefab override for GameComponent "
                        + $"'{currentComponent.GetType().FullName}.{member.name}' on GameObject "
                        + $"'{currentComponent.gameObject.name}'.",
                        exception
                    );
                }
                overrides.SetProperty(new PrefabPropertyOverride(sourceID, member.name, persistedValue));
            }
        }
    }

    private static void CaptureAdditions(
        PrefabConnectionRecord connection,
        GameScene scene,
        PrefabOverrideSet overrides
    ) {
        if (!connection.objectIdentities.TryGetValue(connection.sourceRootID, out Guid rootRuntimeID))
        {
            return;
        }
        GameObject? root = scene.FindObject(rootRuntimeID);
        if (root is null)
        {
            return;
        }
        HashSet<Guid> mappedObjectIDs = connection.objectIdentities.Values.ToHashSet();
        HashSet<Guid> mappedComponentIDs = connection.componentIdentities.Values.ToHashSet();
        var stack = new Stack<GameObject>();
        stack.Push(root);
        while (stack.Count != 0)
        {
            GameObject gameObject = stack.Pop();
            if (!mappedObjectIDs.Contains(gameObject.identity.persistentID))
            {
                overrides.MarkObjectAdded(gameObject.identity.persistentID);
            }
            foreach (GameComponent component in gameObject.GetComponents())
            {
                if (!mappedComponentIDs.Contains(component.identity.persistentID))
                {
                    overrides.MarkComponentAdded(component.identity.persistentID);
                }
            }
            foreach (Transform child in gameObject.transform.children)
            {
                stack.Push(child.gameObject);
            }
        }
    }

    private static void PreserveOrphanedOverrides(
        PrefabOverrideSet previous,
        PrefabConnectionRecord sourceConnection,
        PrefabOverrideSet current
    ) {
        foreach (PrefabPropertyOverride property in previous.properties)
        {
            if (!sourceConnection.componentIdentities.ContainsKey(property.sourceComponentID))
            {
                current.SetProperty(
                    property with
                    {
                        value = (byte[])property.value.Clone(),
                        isOrphaned = true
                    }
                );
            }
        }
        foreach (PrefabStructureOverride structure in previous.structures)
        {
            if (!sourceConnection.objectIdentities.ContainsKey(structure.sourceObjectID))
            {
                current.SetStructure(structure.sourceObjectID, structure.kind, isOrphaned: true);
            }
        }
    }

    private static void MarkOrphanedOverrides(
        PrefabConnectionRecord connection,
        PrefabConnectionRecord sourceConnection,
        GameScene sourceScene,
        SerializationContext context
    ) {
        foreach (PrefabPropertyOverride property in connection.overrides.properties.ToArray())
        {
            bool hasProperty = false;
            if (sourceConnection.componentIdentities.TryGetValue(property.sourceComponentID, out Guid sourceRuntimeID)
                && sourceScene.FindComponent(sourceRuntimeID) is GameComponent sourceComponent
            ) {
                hasProperty = SerializedPropertyValueCodec.GetMembers(sourceComponent.GetType(), context)
                    .Any(member => string.Equals(member.name, property.propertyName, StringComparison.Ordinal));
            }
            if (!hasProperty && !property.isOrphaned)
            {
                connection.overrides.SetProperty(property with { isOrphaned = true });
            }
        }
        foreach (PrefabStructureOverride structure in connection.overrides.structures.ToArray())
        {
            if (!sourceConnection.objectIdentities.ContainsKey(structure.sourceObjectID)
                && !structure.isOrphaned
            ) {
                connection.overrides.SetStructure(structure.sourceObjectID, structure.kind, isOrphaned: true);
            }
        }
    }

    private static void RemoveDeletedSourceObjects(
        PrefabConnectionRecord connection,
        PrefabConnectionRecord sourceConnection,
        GameScene scene,
        GameObject instanceRoot
    ) {
        Guid[] removedSourceIDs = connection.objectIdentities.Keys
            .Where(sourceID => (
                connection.overrides.IsObjectRemoved(sourceID)
                || !sourceConnection.objectIdentities.ContainsKey(sourceID)
            ))
            .ToArray();
        for (int i = 0; i < removedSourceIDs.Length; i++)
        {
            Guid sourceID = removedSourceIDs[i];
            if (!connection.objectIdentities.TryGetValue(sourceID, out Guid runtimeID))
            {
                continue;
            }
            GameObject? target = scene.FindObject(runtimeID);
            if (target is null || ReferenceEquals(target, instanceRoot))
            {
                continue;
            }

            GameObject[] addedChildren = target.transform.children
                .Select(static child => child.gameObject)
                .Where(child => !connection.objectIdentities.Values.Contains(child.identity.persistentID))
                .ToArray();
            for (int childIndex = 0; childIndex < addedChildren.Length; childIndex++)
            {
                scene.SetParent(addedChildren[childIndex].transform, instanceRoot.transform, worldPositionStays: false);
            }
            scene.DestroyObject(target);
            connection.RemoveObject(sourceID);
        }
    }

    private static void CreateNewSourceObjects(
        PrefabConnectionRecord connection,
        PrefabConnectionRecord sourceConnection,
        GameScene scene,
        GameScene sourceScene
    ) {
        foreach ((Guid sourceID, Guid sourceRuntimeID) in sourceConnection.objectIdentities)
        {
            if (connection.objectIdentities.ContainsKey(sourceID) || connection.overrides.IsObjectRemoved(sourceID))
            {
                continue;
            }
            GameObject sourceObject = sourceScene.FindObject(sourceRuntimeID)
                ?? throw new InvalidOperationException($"Prefab source object '{sourceID}' disappeared during reconciliation.");
            GameObject target = scene.CreateObject(
                sourceObject.name,
                persistentID: null,
                transformPersistentID: null,
                invokeReset: false
            );
            target.SetTagDirect(sourceObject.tag);
            target.SetActiveSelfDirect(sourceObject.activeSelf);
            connection.MapObject(sourceID, target);

            Guid sourceTransformID = sourceConnection.componentIdentities.Single(
                pair => pair.Value == sourceObject.transform.identity.persistentID
            ).Key;
            connection.MapComponent(sourceTransformID, target.transform);
        }
    }

    private static void RemoveDeletedSourceComponents(
        PrefabConnectionRecord connection,
        PrefabConnectionRecord sourceConnection,
        GameScene scene
    ) {
        Guid[] removedSourceIDs = connection.componentIdentities.Keys
            .Where(sourceID => (
                connection.overrides.IsComponentRemoved(sourceID)
                || !sourceConnection.componentIdentities.ContainsKey(sourceID)
            ))
            .ToArray();
        for (int i = 0; i < removedSourceIDs.Length; i++)
        {
            Guid sourceID = removedSourceIDs[i];
            if (!connection.componentIdentities.TryGetValue(sourceID, out Guid runtimeID))
            {
                continue;
            }
            GameComponent? target = scene.FindComponent(runtimeID);
            if (target is not null && target is not Transform)
            {
                scene.RemoveComponent(target.gameObject, target);
            }
            connection.RemoveComponent(sourceID);
        }
    }

    private static void CreateNewSourceComponents(
        PrefabConnectionRecord connection,
        PrefabConnectionRecord sourceConnection,
        GameScene scene,
        GameScene sourceScene
    ) {
        foreach ((Guid sourceID, Guid sourceRuntimeID) in sourceConnection.componentIdentities)
        {
            if (connection.componentIdentities.ContainsKey(sourceID)
                || connection.overrides.IsComponentRemoved(sourceID)
            ) {
                continue;
            }
            GameComponent sourceComponent = sourceScene.FindComponent(sourceRuntimeID)
                ?? throw new InvalidOperationException($"Prefab source component '{sourceID}' disappeared during reconciliation.");
            Guid sourceOwnerID = sourceConnection.objectIdentities.Single(
                pair => pair.Value == sourceComponent.gameObject.identity.persistentID
            ).Key;
            if (!connection.objectIdentities.TryGetValue(sourceOwnerID, out Guid targetOwnerID))
            {
                continue;
            }
            GameObject targetOwner = scene.FindObject(targetOwnerID)
                ?? throw new InvalidOperationException($"Prefab target owner for source object '{sourceOwnerID}' is unavailable.");
            GameComponent target = sourceComponent is Transform
                                   ? targetOwner.transform
                                   : scene.AddComponent(targetOwner, sourceComponent.GetType(), persistentID: null, invokeReset: false);
            connection.MapComponent(sourceID, target);
        }
    }

    private static void RestoreSourceProperties(
        PrefabConnectionRecord connection,
        PrefabConnectionRecord sourceConnection,
        GameScene scene,
        GameScene sourceScene,
        SerializationContext context,
        SceneGraphReferenceMap sourceReferences,
        SceneGraphReferenceMap targetReferences
    ) {
        foreach ((Guid sourceID, Guid sourceRuntimeID) in sourceConnection.componentIdentities)
        {
            if (!connection.componentIdentities.TryGetValue(sourceID, out Guid targetRuntimeID))
            {
                continue;
            }
            GameComponent? sourceComponent = sourceScene.FindComponent(sourceRuntimeID);
            GameComponent? targetComponent = scene.FindComponent(targetRuntimeID);
            if (sourceComponent is null || targetComponent is null
                || sourceComponent.GetType() != targetComponent.GetType()
            ) {
                continue;
            }

            IReadOnlyList<SerializationMemberMetadata> members = SerializedPropertyValueCodec.GetMembers(
                sourceComponent.GetType(),
                context
            );
            for (int memberIndex = 0; memberIndex < members.Count; memberIndex++)
            {
                SerializationMemberMetadata member = members[memberIndex];
                if (connection.overrides.IsPropertyOverridden(sourceID, member.name))
                {
                    continue;
                }
                byte[] value = SerializedPropertyValueCodec.Encode(member, sourceComponent, context, sourceReferences);
                SerializedPropertyValueCodec.Decode(member, targetComponent, value, context, targetReferences);
            }
        }
    }

    private static void RestoreSourceObjectState(
        PrefabConnectionRecord connection,
        PrefabConnectionRecord sourceConnection,
        GameScene scene,
        GameScene sourceScene,
        GameObject instanceRoot
    ) {
        foreach ((Guid sourceID, Guid sourceRuntimeID) in sourceConnection.objectIdentities)
        {
            if (!connection.objectIdentities.TryGetValue(sourceID, out Guid targetRuntimeID))
            {
                continue;
            }
            GameObject? sourceObject = sourceScene.FindObject(sourceRuntimeID);
            GameObject? targetObject = scene.FindObject(targetRuntimeID);
            if (sourceObject is null || targetObject is null)
            {
                continue;
            }

            PrefabObjectOverrideKind kind = connection.overrides.GetStructureOverride(sourceID);
            if ((kind & PrefabObjectOverrideKind.Name) == 0)
            {
                targetObject.SetNameDirect(sourceObject.name);
            }
            if ((kind & PrefabObjectOverrideKind.Tag) == 0)
            {
                targetObject.SetTagDirect(sourceObject.tag);
            }
            if ((kind & PrefabObjectOverrideKind.ActiveSelf) == 0)
            {
                targetObject.SetActiveSelfDirect(sourceObject.activeSelf);
            }
            if (!ReferenceEquals(targetObject, instanceRoot)
                && (kind & PrefabObjectOverrideKind.Parent) == 0
            ) {
                Guid sourceParentID = GetMappedParentID(sourceObject, sourceConnection);
                Transform? targetParent = sourceParentID == Guid.Empty
                                          ? instanceRoot.transform
                                          : connection.objectIdentities.TryGetValue(sourceParentID, out Guid parentRuntimeID)
                                            ? scene.FindObject(parentRuntimeID)?.transform
                                            : null;
                scene.SetParent(targetObject.transform, targetParent, worldPositionStays: false);
            }
        }

        foreach ((Guid sourceID, Guid sourceRuntimeID) in sourceConnection.objectIdentities
            .OrderBy(pair => sourceScene.FindObject(pair.Value)?.transform.siblingIndex ?? int.MaxValue)
        ) {
            if (!connection.objectIdentities.TryGetValue(sourceID, out Guid targetRuntimeID))
            {
                continue;
            }
            GameObject? targetObject = scene.FindObject(targetRuntimeID);
            if (targetObject is null || ReferenceEquals(targetObject, instanceRoot))
            {
                continue;
            }
            if ((connection.overrides.GetStructureOverride(sourceID) & PrefabObjectOverrideKind.SiblingIndex) == 0)
            {
                GameObject? sourceObject = sourceScene.FindObject(sourceRuntimeID);
                if (sourceObject is not null)
                {
                    scene.SetSiblingIndex(targetObject.transform, sourceObject.transform.siblingIndex);
                }
            }
            scene.RecomputeActiveSubtree(targetObject);
        }
        scene.RecomputeActiveSubtree(instanceRoot);
    }

    private static void RefreshInstanceInfo(
        PrefabConnectionRecord connection,
        GameObject instanceRoot
    ) {
        GameScene scene = instanceRoot.scene;
        foreach ((Guid sourceID, Guid runtimeID) in connection.objectIdentities)
        {
            GameObject? gameObject = scene.FindObject(runtimeID);
            if (gameObject is null)
            {
                continue;
            }
            gameObject.SetPrefabInstanceDirect(
                new PrefabInstanceInfo(
                    connection.sourceAsset.identity.persistentID,
                    sourceID,
                    instanceRoot,
                    ReferenceEquals(gameObject, instanceRoot),
                    connection.isVariant,
                    connection.sourceAsset.isMissing,
                    connection.overrides.count,
                    connection.overrides.orphanedCount,
                    connection.sourceAsset
                )
            );
        }
        instanceRoot.SetPrefabConnectionDirect(connection);
    }

    private static SceneGraphReferenceMap CreateComparisonReferences(
        GameScene scene,
        PrefabConnectionRecord connection
    ) {
        EngineObject[] allObjects = scene.GetOwnedObjects()
            .SelectMany(static gameObject => gameObject.GetComponents().Cast<EngineObject>().Prepend(gameObject))
            .ToArray();
        var ids = new Dictionary<EngineObject, Guid>(ReferenceEqualityComparer.Instance);
        for (int objectIndex = 0; objectIndex < allObjects.Length; objectIndex++)
        {
            ids.Add(allObjects[objectIndex], allObjects[objectIndex].identity.persistentID);
        }
        foreach ((Guid sourceID, Guid runtimeID) in connection.objectIdentities)
        {
            GameObject? gameObject = scene.FindObject(runtimeID);
            if (gameObject is not null)
            {
                ids[gameObject] = sourceID;
            }
        }
        foreach ((Guid sourceID, Guid runtimeID) in connection.componentIdentities)
        {
            GameComponent? component = scene.FindComponent(runtimeID);
            if (component is not null)
            {
                ids[component] = sourceID;
            }
        }
        return new SceneGraphReferenceMap(scene, ids);
    }

    private static SceneGraphReferenceMap CreateTargetReferences(
        GameScene scene,
        PrefabConnectionRecord connection
    ) {
        SceneGraphReferenceMap references = CreateComparisonReferences(scene, connection);
        foreach ((Guid sourceID, Guid runtimeID) in connection.objectIdentities)
        {
            GameObject? gameObject = scene.FindObject(runtimeID);
            if (gameObject is not null)
            {
                references.Register(sourceID, gameObject);
            }
        }
        foreach ((Guid sourceID, Guid runtimeID) in connection.componentIdentities)
        {
            GameComponent? component = scene.FindComponent(runtimeID);
            if (component is not null)
            {
                references.Register(sourceID, component);
            }
        }
        return references;
    }

    private static Guid GetMappedParentID(
        GameObject gameObject,
        PrefabConnectionRecord connection
    ) {
        Transform? parent = gameObject.transform.parent;
        if (parent is null)
        {
            return Guid.Empty;
        }
        Guid parentRuntimeID = parent.gameObject.identity.persistentID;
        foreach ((Guid sourceID, Guid runtimeID) in connection.objectIdentities)
        {
            if (runtimeID == parentRuntimeID)
            {
                return sourceID;
            }
        }
        return parentRuntimeID;
    }

    private static (GameScene scene, GameObject root, PrefabConnectionRecord connection) InstantiateSource(
        AssetObject sourceAsset,
        SerializationContext context
    ) {
        var sourceScene = new GameScene(SceneWorld.current.typeCatalog, "Prefab Source Comparison", persistentID: null);
        SerializationContext sourceContext = context
            .With(sourceScene)
            .With(sourceAsset);
        try
        {
            using (SceneReferenceScope.Suspend())
            {
                GameObject sourceRoot = context.GetRequired<SerializationRegistry>().Deserialize<GameObject>(
                    sourceAsset.runtimePayload.Span,
                    sourceContext
                );
                if (!string.IsNullOrWhiteSpace(sourceAsset.assetPath.ToString()))
                {
                    sourceRoot.name = Path.GetFileNameWithoutExtension(sourceAsset.assetPath.ToString());
                }
                PrefabConnectionRecord connection = sourceRoot.prefabConnection
                    ?? throw new InvalidOperationException(
                        $"Prefab source asset '{sourceAsset.identity.persistentID}' did not create a source mapping."
                    );
                return (sourceScene, sourceRoot, connection);
            }
        }
        catch
        {
            if (!sourceScene.isDestroyed)
            {
                sourceScene.Unload();
            }
            throw;
        }
    }
}
