using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;
using Inno.Scene.Components;
using Inno.Scene.Layers;

namespace Inno.Scene;

internal static class SceneGraphSerialization
{
    internal const string C_SCENE_ID_KEY = "sceneId";
    internal const string C_NAME_KEY = "name";
    internal const string C_TAG_KEY = "tag";
    internal const string C_LAYER_KEY = "layer";
    internal const string C_OBJECTS_KEY = "objects";
    internal const string C_SYSTEMS_KEY = "systems";
    internal const string C_SYSTEM_ID_KEY = "systemId";
    internal const string C_OBJECT_ID_KEY = "objectId";
    internal const string C_ACTIVE_SELF_KEY = "activeSelf";
    internal const string C_PARENT_ID_KEY = "parentId";
    internal const string C_SIBLING_INDEX_KEY = "siblingIndex";
    internal const string C_COMPONENTS_KEY = "components";
    internal const string C_COMPONENT_ID_KEY = "componentId";
    internal const string C_STABLE_TYPE_ID_KEY = "stableTypeId";
    internal const string C_TYPE_NAME_KEY = "typeName";
    internal const string C_STATE_KEY = "state";
    internal const string C_STATE_DEPENDENCIES_KEY = "stateDependencies";
    internal const string C_STATE_DEPENDENCY_ASSETS_KEY = "stateDependencyAssets";
    internal const string C_MISSING_REFERENCE_SOURCE_IDS_KEY = "missingReferenceSourceIds";
    internal const string C_MISSING_REFERENCE_TARGET_IDS_KEY = "missingReferenceTargetIds";
    internal const string C_HAS_PREFAB_CONNECTION_KEY = "hasPrefabConnection";
    internal const string C_PREFAB_SOURCE_ASSET_ID_KEY = "prefabSourceAssetId";
    internal const string C_PREFAB_SOURCE_OBJECT_ID_KEY = "prefabSourceObjectId";
    internal const string C_PREFAB_INSTANCE_ROOT_ID_KEY = "prefabInstanceRootId";
    internal const string C_PREFAB_IS_VARIANT_KEY = "prefabIsVariant";
    internal const string C_PREFAB_IS_MISSING_KEY = "prefabIsMissing";
    internal const string C_PREFAB_OVERRIDE_COUNT_KEY = "prefabOverrideCount";
    internal const string C_PREFAB_ORPHANED_OVERRIDE_COUNT_KEY = "prefabOrphanedOverrideCount";
    internal const string C_PREFAB_SOURCE_ASSET_KEY = "prefabSourceAsset";
    internal const string C_PREFAB_COMPONENT_MAPPINGS_KEY = "prefabComponentMappings";
    internal const string C_PREFAB_MAPPING_SOURCE_ID_KEY = "sourceId";
    internal const string C_PREFAB_MAPPING_INSTANCE_ID_KEY = "instanceId";
    internal const string C_PREFAB_PROPERTY_OVERRIDES_KEY = "prefabPropertyOverrides";
    internal const string C_PREFAB_PROPERTY_NAME_KEY = "propertyName";
    internal const string C_PREFAB_PROPERTY_VALUE_KEY = "propertyValue";
    internal const string C_PREFAB_OVERRIDE_ORPHANED_KEY = "isOrphaned";
    internal const string C_PREFAB_STRUCTURE_OVERRIDES_KEY = "prefabStructureOverrides";
    internal const string C_PREFAB_STRUCTURE_KIND_KEY = "structureKind";
    internal const string C_PREFAB_REMOVED_OBJECTS_KEY = "prefabRemovedObjects";
    internal const string C_PREFAB_REMOVED_COMPONENTS_KEY = "prefabRemovedComponents";
    internal const string C_PREFAB_ADDED_OBJECTS_KEY = "prefabAddedObjects";
    internal const string C_PREFAB_ADDED_COMPONENTS_KEY = "prefabAddedComponents";

    internal static void WriteObjects(
        SerializationWriter writer,
        IReadOnlyCollection<SceneObjectStructureSnapshot> entries,
        IReadOnlyDictionary<EngineObject, Guid> sourceIDs,
        Predicate<GameObject> containsObject,
        bool preserveRootSiblingOrder
    ) {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(entries);
        ArgumentNullException.ThrowIfNull(sourceIDs);
        ArgumentNullException.ThrowIfNull(containsObject);
        Action<SerializationWriter, GameComponent> writeComponent = (
            componentWriter,
            component
        ) => WriteComponent(componentWriter, component, sourceIDs);
        WriteMissingReferenceAliases(writer, sourceIDs);
        writer.WriteObjectArray(
            C_OBJECTS_KEY,
            entries,
            (
                objectWriter,
                entry
            ) =>
            {
                GameObject gameObject = entry.gameObject;
                Transform? parent = gameObject.transform.parent;
                bool parentIsIncluded = parent is not null && containsObject(parent.gameObject);
                objectWriter.Write(C_OBJECT_ID_KEY, GetSourceID(sourceIDs, gameObject));
                objectWriter.Write(C_NAME_KEY, gameObject.name);
                objectWriter.Write(C_TAG_KEY, gameObject.tag);
                objectWriter.Write(C_LAYER_KEY, gameObject.layer.index);
                objectWriter.Write(C_ACTIVE_SELF_KEY, gameObject.activeSelf);
                objectWriter.Write(
                    C_PARENT_ID_KEY,
                    parentIsIncluded ? GetSourceID(sourceIDs, parent!.gameObject) : Guid.Empty
                );
                objectWriter.Write(
                    C_SIBLING_INDEX_KEY,
                    parentIsIncluded || preserveRootSiblingOrder ? gameObject.transform.siblingIndex : 0
                );
                PrefabInstanceInfo? prefab = gameObject.prefabInstance;
                bool hasPrefabConnection = prefab is not null && containsObject(prefab.instanceRoot);
                objectWriter.Write(C_HAS_PREFAB_CONNECTION_KEY, hasPrefabConnection);
                if (hasPrefabConnection)
                {
                    AssetObject sourceAsset = prefab!.sourceAsset;
                    objectWriter.Write(C_PREFAB_SOURCE_ASSET_KEY, sourceAsset);
                    objectWriter.Write(C_PREFAB_SOURCE_ASSET_ID_KEY, prefab!.sourceAssetID);
                    objectWriter.Write(C_PREFAB_SOURCE_OBJECT_ID_KEY, prefab.sourceObjectID);
                    objectWriter.Write(C_PREFAB_INSTANCE_ROOT_ID_KEY, GetSourceID(sourceIDs, prefab.instanceRoot));
                    objectWriter.Write(C_PREFAB_IS_VARIANT_KEY, prefab.isVariant);
                    objectWriter.Write(C_PREFAB_IS_MISSING_KEY, prefab.isMissing);
                    PrefabOverrideSet? capturedOverrides = null;
                    if (prefab.isRoot)
                    {
                        PrefabConnectionRecord connection = gameObject.prefabConnection
                            ?? throw new InvalidOperationException(
                                $"Prefab instance root '{gameObject.identity.persistentID}' has no connection mapping."
                            );
                        capturedOverrides = PrefabOverrideProcessor.Capture(
                            connection,
                            gameObject.scene,
                            objectWriter.context,
                            SceneReferenceScope.current
                        );
                        WritePrefabConnection(objectWriter, connection, capturedOverrides, sourceIDs, gameObject.scene);
                    }
                    objectWriter.Write(C_PREFAB_OVERRIDE_COUNT_KEY, capturedOverrides?.count ?? prefab.overrideCount);
                    objectWriter.Write(
                        C_PREFAB_ORPHANED_OVERRIDE_COUNT_KEY,
                        capturedOverrides?.orphanedCount ?? prefab.orphanedOverrideCount
                    );
                }
                objectWriter.WriteObjectArray(C_COMPONENTS_KEY, entry.components, writeComponent);
            },
            captureKey: (
                entry,
                key
            ) =>
            {
                GameObject gameObject = entry.gameObject;
                if (gameObject.prefabInstance is not null || !key.TryAddOwner(gameObject, gameObject.captureRevision))
                {
                    return false;
                }
                Transform? parent = gameObject.transform.parent;
                bool parentIsIncluded = parent is not null && containsObject(parent.gameObject);
                key.AddValue(GetSourceID(sourceIDs, gameObject));
                key.AddValue(parentIsIncluded ? GetSourceID(sourceIDs, parent!.gameObject) : Guid.Empty);
                key.AddValue((ulong)(parentIsIncluded || preserveRootSiblingOrder ? gameObject.transform.siblingIndex : 0));
                for (int componentIndex = 0; componentIndex < entry.components.Count; componentIndex++)
                {
                    GameComponent component = entry.components[componentIndex];
                    if (component is MissingGameComponent || !key.TryAddProperties(component))
                    {
                        return false;
                    }
                    key.AddValue(GetSourceID(sourceIDs, component));
                }
                return true;
            }
        );
    }

    internal static RestoredSceneGraph RestoreObjects(
        GameScene scene,
        IReadOnlyList<SerializationReader> objectReaders,
        bool preservePersistentIDs,
        SceneGraphReferenceMap references,
        IReadOnlyList<KeyValuePair<Guid, Guid>> missingReferenceAliases,
        SerializationContext context,
        bool restoreProperties = true
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(objectReaders);
        ArgumentNullException.ThrowIfNull(references);
        ValidateObjects(objectReaders);

        var gameObjectBySourceID = new Dictionary<Guid, GameObject>();
        var componentBySourceID = new Dictionary<Guid, GameComponent>();
        List<(GameComponent component, byte[] state)> componentRestores = [];
        List<EngineObject> missingPlaceholders = [];

        for (int objectIndex = 0; objectIndex < objectReaders.Count; objectIndex++)
        {
            SerializationReader objectReader = objectReaders[objectIndex];
            Guid sourceObjectID = objectReader.Read<Guid>(C_OBJECT_ID_KEY);
            IReadOnlyList<SerializationReader> componentReaders = objectReader.ReadObjectArray(C_COMPONENTS_KEY);
            SerializationReader transformReader = componentReaders.Single(IsTransform);
            Guid sourceTransformID = transformReader.Read<Guid>(C_COMPONENT_ID_KEY);
            GameObject gameObject = scene.CreateObject(
                objectReader.Read<string>(C_NAME_KEY),
                preservePersistentIDs ? sourceObjectID : null,
                preservePersistentIDs ? sourceTransformID : null,
                invokeReset: false
            );
            gameObject.SetTagDirect(objectReader.Read<string>(C_TAG_KEY));
            gameObject.SetLayerDirect(new GameLayer(objectReader.Read<int>(C_LAYER_KEY)));
            gameObject.SetActiveSelfDirect(objectReader.Read<bool>(C_ACTIVE_SELF_KEY));
            gameObjectBySourceID.Add(sourceObjectID, gameObject);
            componentBySourceID.Add(sourceTransformID, gameObject.transform);
            componentRestores.Add((gameObject.transform, transformReader.Read<byte[]>(C_STATE_KEY)));
        }

        for (int objectIndex = 0; objectIndex < objectReaders.Count; objectIndex++)
        {
            SerializationReader objectReader = objectReaders[objectIndex];
            Guid sourceObjectID = objectReader.Read<Guid>(C_OBJECT_ID_KEY);
            GameObject gameObject = gameObjectBySourceID[sourceObjectID];
            IReadOnlyList<SerializationReader> componentReaders = objectReader.ReadObjectArray(C_COMPONENTS_KEY);
            int transformIndex = -1;
            for (int componentIndex = 0; componentIndex < componentReaders.Count; componentIndex++)
            {
                SerializationReader componentReader = componentReaders[componentIndex];
                Guid stableTypeID = componentReader.Read<Guid>(C_STABLE_TYPE_ID_KEY);
                if (TryResolveComponentType(stableTypeID, componentReader.context, out Type? componentType)
                    && componentType == typeof(Transform)
                ) {
                    transformIndex = componentIndex;
                    continue;
                }

                Guid sourceComponentID = componentReader.Read<Guid>(C_COMPONENT_ID_KEY);
                byte[] state = componentReader.Read<byte[]>(C_STATE_KEY);
                AssetDependency[] dependencies = ReadStateDependencies(componentReader);
                GameComponent component;
                if (componentType is null)
                {
                    component = scene.AddMissingComponent(
                        gameObject,
                        new TypeRef(stableTypeID),
                        componentReader.Read<string>(C_TYPE_NAME_KEY),
                        state,
                        preservePersistentIDs ? sourceComponentID : null,
                        dependencies
                    );
                    missingPlaceholders.Add(component);
                }
                else
                {
                    component = scene.AddComponent(
                        gameObject,
                        componentType,
                        preservePersistentIDs ? sourceComponentID : null,
                        invokeReset: false
                    );
                    componentRestores.Add((component, state));
                }
                componentBySourceID.Add(sourceComponentID, component);
            }
            if (transformIndex < 0)
            {
                throw new InvalidDataException(
                    $"Scene graph object '{sourceObjectID}' does not contain its validated Transform component."
                );
            }
            gameObject.SetComponentIndex(gameObject.transform, transformIndex);
        }

        foreach ((Guid sourceID, GameObject gameObject) in gameObjectBySourceID)
        {
            references.Register(sourceID, gameObject);
        }
        foreach ((Guid sourceID, GameComponent component) in componentBySourceID)
        {
            references.Register(sourceID, component);
        }

        if (restoreProperties)
        {
            RestoreMissingReferenceAliases(missingReferenceAliases, references, missingPlaceholders);
            using (references.Enter())
            {
                SerializationRegistry serialization = context.GetRequired<SerializationRegistry>();
                for (int i = 0; i < componentRestores.Count; i++)
                {
                    serialization.RestorePropertiesData(
                        componentRestores[i].component,
                        componentRestores[i].state,
                        context: context
                    );
                }
            }
        }

        for (int objectIndex = 0; objectIndex < objectReaders.Count; objectIndex++)
        {
            SerializationReader objectReader = objectReaders[objectIndex];
            Guid sourceObjectID = objectReader.Read<Guid>(C_OBJECT_ID_KEY);
            Guid parentID = objectReader.Read<Guid>(C_PARENT_ID_KEY);
            Transform? parent = parentID == Guid.Empty
                                ? null
                                : gameObjectBySourceID.TryGetValue(parentID, out GameObject? parentObject)
                                  ? parentObject.transform
                                  : throw new InvalidDataException(
                                      $"Parent object '{parentID}' for object '{sourceObjectID}' is outside the restored boundary."
                                  );
            scene.SetParent(gameObjectBySourceID[sourceObjectID].transform, parent, worldPositionStays: false);
        }

        foreach (IGrouping<Guid, SerializationReader> siblingGroup in objectReaders.GroupBy(
            objectReader => objectReader.Read<Guid>(C_PARENT_ID_KEY)
        )) {
            foreach (SerializationReader objectReader in siblingGroup.OrderBy(
                reader => reader.Read<int>(C_SIBLING_INDEX_KEY)
            )) {
                scene.SetSiblingIndex(
                    gameObjectBySourceID[objectReader.Read<Guid>(C_OBJECT_ID_KEY)].transform,
                    objectReader.Read<int>(C_SIBLING_INDEX_KEY)
                );
            }
        }

        var connectionByRootID = new Dictionary<Guid, PrefabConnectionRecord>();
        for (int objectIndex = 0; objectIndex < objectReaders.Count; objectIndex++)
        {
            SerializationReader objectReader = objectReaders[objectIndex];
            if (!objectReader.Read<bool>(C_HAS_PREFAB_CONNECTION_KEY))
            {
                continue;
            }
            Guid serializedObjectID = objectReader.Read<Guid>(C_OBJECT_ID_KEY);
            Guid instanceRootID = objectReader.Read<Guid>(C_PREFAB_INSTANCE_ROOT_ID_KEY);
            if (serializedObjectID != instanceRootID)
            {
                continue;
            }
            if (!gameObjectBySourceID.TryGetValue(instanceRootID, out GameObject? instanceRoot))
            {
                throw new InvalidDataException(
                    $"Prefab instance root '{instanceRootID}' at '{objectReader.path}' is outside the restored graph."
                );
            }
            AssetObject sourceAsset = objectReader.Read<AssetObject>(C_PREFAB_SOURCE_ASSET_KEY);
            Guid sourceAssetID = objectReader.Read<Guid>(C_PREFAB_SOURCE_ASSET_ID_KEY);
            if (sourceAsset.identity.persistentID != sourceAssetID)
            {
                throw new InvalidDataException(
                    $"Prefab source token and source identity disagree at '{objectReader.path}'."
                );
            }

            PrefabOverrideSet overrides = ReadPrefabOverrides(objectReader);
            var connection = new PrefabConnectionRecord(
                sourceAsset,
                objectReader.Read<Guid>(C_PREFAB_SOURCE_OBJECT_ID_KEY),
                objectReader.Read<bool>(C_PREFAB_IS_VARIANT_KEY),
                overrides
            );
            foreach (SerializationReader mappingReader in
                     objectReader.ReadObjectArray(C_PREFAB_COMPONENT_MAPPINGS_KEY)
            ) {
                Guid componentSourceID = mappingReader.Read<Guid>(C_PREFAB_MAPPING_SOURCE_ID_KEY);
                Guid componentInstanceID = mappingReader.Read<Guid>(C_PREFAB_MAPPING_INSTANCE_ID_KEY);
                if (!componentBySourceID.TryGetValue(componentInstanceID, out GameComponent? component))
                {
                    throw new InvalidDataException(
                        $"Prefab component mapping '{componentInstanceID}' at '{mappingReader.path}' does not resolve."
                    );
                }
                connection.MapComponent(componentSourceID, component);
            }
            connectionByRootID.Add(instanceRootID, connection);
            instanceRoot.SetPrefabConnectionDirect(connection);
        }

        for (int objectIndex = 0; objectIndex < objectReaders.Count; objectIndex++)
        {
            SerializationReader objectReader = objectReaders[objectIndex];
            if (!objectReader.Read<bool>(C_HAS_PREFAB_CONNECTION_KEY))
            {
                continue;
            }
            Guid serializedObjectID = objectReader.Read<Guid>(C_OBJECT_ID_KEY);
            Guid instanceRootID = objectReader.Read<Guid>(C_PREFAB_INSTANCE_ROOT_ID_KEY);
            if (!gameObjectBySourceID.TryGetValue(instanceRootID, out GameObject? instanceRoot)
                || !connectionByRootID.TryGetValue(instanceRootID, out PrefabConnectionRecord? connection)
            ) {
                throw new InvalidDataException(
                    $"Prefab instance root '{instanceRootID}' at '{objectReader.path}' has no connection record."
                );
            }
            GameObject gameObject = gameObjectBySourceID[serializedObjectID];
            Guid prefabSourceObjectID = objectReader.Read<Guid>(C_PREFAB_SOURCE_OBJECT_ID_KEY);
            connection.MapObject(prefabSourceObjectID, gameObject);
            gameObject.SetPrefabInstanceDirect(
                new PrefabInstanceInfo(
                    connection.sourceAsset.identity.persistentID,
                    prefabSourceObjectID,
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

        foreach (GameObject root in gameObjectBySourceID.Values.Where(
            static candidate => candidate.transform.parent is null
        )) {
            scene.RecomputeActiveSubtree(root);
        }
        return new RestoredSceneGraph(
            gameObjectBySourceID,
            componentBySourceID,
            componentRestores,
            missingPlaceholders,
            missingReferenceAliases
        );
    }

    internal static void ValidateScene(SerializationReader reader)
    {
        EnsurePersistentID(reader.Read<Guid>(C_SCENE_ID_KEY), $"{reader.path}.{C_SCENE_ID_KEY}");
        _ = reader.Read<string>(C_NAME_KEY);
        ValidateMissingReferenceAliases(reader);
        ValidateObjects(reader.ReadObjectArray(C_OBJECTS_KEY));
        ValidateSystems(reader.ReadObjectArray(C_SYSTEMS_KEY));
    }

    internal static void WriteSystems(
        SerializationWriter writer,
        IReadOnlyCollection<GameSystem> systems,
        IReadOnlyDictionary<EngineObject, Guid> sourceIDs
    ) => writer.WriteObjectArray(C_SYSTEMS_KEY, systems, (
        systemWriter,
        system
    ) =>
    {
            systemWriter.Write(C_SYSTEM_ID_KEY, GetSourceID(sourceIDs, system));
            if (system is MissingGameSystem missing)
            {
                systemWriter.Write(C_STABLE_TYPE_ID_KEY, missing.missingType.stableID);
                systemWriter.Write(C_TYPE_NAME_KEY, missing.missingTypeName);
                systemWriter.Write(C_STATE_KEY, missing.CaptureSerializedState());
                WriteStateDependencies(systemWriter, missing.dependencies);
                return;
            }
            systemWriter.Write(C_STABLE_TYPE_ID_KEY, systemWriter.typeSnapshot.GetTypeRef(system.GetType()).stableID);
            systemWriter.Write(C_TYPE_NAME_KEY, system.GetType().FullName ?? system.GetType().Name);
            AssetDependency[] dependencies = WriteState(systemWriter, system);
            WriteStateDependencies(systemWriter, dependencies);
        });

    internal static IReadOnlyList<(GameSystem system, byte[] state)> CreateSystems(
        GameScene scene,
        IReadOnlyList<SerializationReader> systemReaders,
        bool preservePersistentIDs,
        SceneGraphReferenceMap references,
        ICollection<EngineObject>? missingPlaceholders = null
    ) {
        ValidateSystems(systemReaders);
        var result = new List<(GameSystem, byte[])>(systemReaders.Count);
        foreach (SerializationReader systemReader in systemReaders)
        {
            Guid sourceID = systemReader.Read<Guid>(C_SYSTEM_ID_KEY);
            Guid stableTypeID = systemReader.Read<Guid>(C_STABLE_TYPE_ID_KEY);
            byte[] state = systemReader.Read<byte[]>(C_STATE_KEY);
            AssetDependency[] dependencies = ReadStateDependencies(systemReader);
            GameSystem system;
            if (!TryResolveSystemType(stableTypeID, systemReader.context, out Type? systemType) || systemType is null)
            {
                    system = scene.AddMissingSystem(
                        new TypeRef(stableTypeID),
                        systemReader.Read<string>(C_TYPE_NAME_KEY),
                        state,
                        preservePersistentIDs ? sourceID : null,
                        dependencies
                    );
                missingPlaceholders?.Add(system);
            }
            else
            {
                system = scene.AddSystem(systemType, preservePersistentIDs ? sourceID : null, invokeReset: false);
                result.Add((system, state));
            }
            references.Register(sourceID, system);
        }
        return result;
    }

    internal static void RestoreMissingReferenceAliases(
        RestoredSceneGraph graph,
        SceneGraphReferenceMap references
    ) => RestoreMissingReferenceAliases(graph.missingReferenceAliases, references, graph.missingPlaceholders);

    internal static void ReconcilePrefabConnections(
        GameScene scene,
        SerializationContext context,
        GameObject? restoredRoot = null,
        bool includeRestoredRoot = false
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(context);
        IEnumerable<GameObject> candidates = restoredRoot is null
                                             ? scene.GetOwnedObjects()
                                             : EnumerateRestoredObjects(restoredRoot, includeRestoredRoot);
        GameObject[] roots = candidates
            .Where(gameObject => gameObject.prefabInstance?.isRoot == true && gameObject.prefabConnection is not null)
            .ToArray();
        for (int i = 0; i < roots.Length; i++)
        {
            if (!roots[i].isRuntimeValid || roots[i].prefabConnection is null)
            {
                continue;
            }
            PrefabOverrideProcessor.Reconcile(roots[i].prefabConnection!, roots[i], context);
        }
    }

    internal static void ValidateObjects(IReadOnlyList<SerializationReader> objectReaders)
    {
        var objectIDs = new HashSet<Guid>();
        var componentIDs = new HashSet<Guid>();
        for (int objectIndex = 0; objectIndex < objectReaders.Count; objectIndex++)
        {
            SerializationReader objectReader = objectReaders[objectIndex];
            Guid objectID = objectReader.Read<Guid>(C_OBJECT_ID_KEY);
            EnsurePersistentID(objectID, $"{objectReader.path}.{C_OBJECT_ID_KEY}");
            if (!objectIDs.Add(objectID))
            {
                throw new InvalidDataException($"Duplicate GameObject local identity '{objectID}' at '{objectReader.path}'.");
            }
            _ = objectReader.Read<string>(C_NAME_KEY);
            if (objectReader.Contains(C_TAG_KEY))
            {
                _ = GameObject.NormalizeTag(objectReader.Read<string>(C_TAG_KEY));
            }
            _ = objectReader.Read<bool>(C_ACTIVE_SELF_KEY);
            _ = objectReader.Read<Guid>(C_PARENT_ID_KEY);
            _ = objectReader.Read<int>(C_SIBLING_INDEX_KEY);
            bool hasPrefabConnection = objectReader.Read<bool>(C_HAS_PREFAB_CONNECTION_KEY);
            if (hasPrefabConnection)
            {
                if (!objectReader.Contains(C_PREFAB_SOURCE_ASSET_KEY))
                {
                    throw new InvalidDataException($"Prefab source token is missing at '{objectReader.path}'.");
                }
                EnsurePersistentID(
                    objectReader.Read<Guid>(C_PREFAB_SOURCE_ASSET_ID_KEY),
                    $"{objectReader.path}.{C_PREFAB_SOURCE_ASSET_ID_KEY}"
                );
                EnsurePersistentID(
                    objectReader.Read<Guid>(C_PREFAB_SOURCE_OBJECT_ID_KEY),
                    $"{objectReader.path}.{C_PREFAB_SOURCE_OBJECT_ID_KEY}"
                );
                EnsurePersistentID(
                    objectReader.Read<Guid>(C_PREFAB_INSTANCE_ROOT_ID_KEY),
                    $"{objectReader.path}.{C_PREFAB_INSTANCE_ROOT_ID_KEY}"
                );
                _ = objectReader.Read<bool>(C_PREFAB_IS_VARIANT_KEY);
                _ = objectReader.Read<bool>(C_PREFAB_IS_MISSING_KEY);
                _ = objectReader.Read<int>(C_PREFAB_OVERRIDE_COUNT_KEY);
                _ = objectReader.Read<int>(C_PREFAB_ORPHANED_OVERRIDE_COUNT_KEY);
                if (objectID == objectReader.Read<Guid>(C_PREFAB_INSTANCE_ROOT_ID_KEY))
                {
                    _ = objectReader.ReadObjectArray(C_PREFAB_COMPONENT_MAPPINGS_KEY);
                    _ = objectReader.ReadObjectArray(C_PREFAB_PROPERTY_OVERRIDES_KEY);
                    _ = objectReader.ReadObjectArray(C_PREFAB_STRUCTURE_OVERRIDES_KEY);
                    _ = objectReader.Read<Guid[]>(C_PREFAB_REMOVED_OBJECTS_KEY);
                    _ = objectReader.Read<Guid[]>(C_PREFAB_REMOVED_COMPONENTS_KEY);
                    _ = objectReader.Read<Guid[]>(C_PREFAB_ADDED_OBJECTS_KEY);
                    _ = objectReader.Read<Guid[]>(C_PREFAB_ADDED_COMPONENTS_KEY);
                }
            }

            IReadOnlyList<SerializationReader> componentReaders = objectReader.ReadObjectArray(C_COMPONENTS_KEY);
            int transformCount = 0;
            for (int componentIndex = 0; componentIndex < componentReaders.Count; componentIndex++)
            {
                SerializationReader componentReader = componentReaders[componentIndex];
                Guid componentID = componentReader.Read<Guid>(C_COMPONENT_ID_KEY);
                EnsurePersistentID(componentID, $"{componentReader.path}.{C_COMPONENT_ID_KEY}");
                if (!componentIDs.Add(componentID))
                {
                    throw new InvalidDataException($"Duplicate GameComponent local identity '{componentID}' at '{componentReader.path}'.");
                }
                Guid stableTypeID = componentReader.Read<Guid>(C_STABLE_TYPE_ID_KEY);
                EnsurePersistentID(stableTypeID, $"{componentReader.path}.{C_STABLE_TYPE_ID_KEY}");
                if (TryResolveComponentType(stableTypeID, componentReader.context, out Type? componentType)
                    && componentType == typeof(Transform)
                ) {
                    transformCount++;
                }
                EnsureTypeName(componentReader.Read<string>(C_TYPE_NAME_KEY), componentReader.path);
                _ = componentReader.Read<byte[]>(C_STATE_KEY);
                _ = ReadStateDependencies(componentReader);
            }

            if (transformCount != 1)
            {
                throw new InvalidDataException(
                    $"Scene graph object '{objectID}' must contain exactly one Transform, but found '{transformCount}'."
                );
            }
        }

        for (int objectIndex = 0; objectIndex < objectReaders.Count; objectIndex++)
        {
            Guid parentID = objectReaders[objectIndex].Read<Guid>(C_PARENT_ID_KEY);
            if (parentID != Guid.Empty && !objectIDs.Contains(parentID))
            {
                throw new InvalidDataException(
                    $"Parent object '{parentID}' at '{objectReaders[objectIndex].path}' is outside the serialized boundary."
                );
            }
        }
    }

    internal static IReadOnlyList<KeyValuePair<Guid, Guid>> ReadMissingReferenceAliases(SerializationReader rootReader)
    {
        Guid[] aliases = rootReader.Read<Guid[]>(C_MISSING_REFERENCE_SOURCE_IDS_KEY);
        Guid[] targets = rootReader.Read<Guid[]>(C_MISSING_REFERENCE_TARGET_IDS_KEY);
        var result = new KeyValuePair<Guid, Guid>[aliases.Length];
        for (int i = 0; i < aliases.Length; i++)
        {
            result[i] = new KeyValuePair<Guid, Guid>(aliases[i], targets[i]);
        }
        return result;
    }

    internal static void ValidateMissingReferenceAliases(SerializationReader reader)
    {
        Guid[] aliases = reader.Read<Guid[]>(C_MISSING_REFERENCE_SOURCE_IDS_KEY);
        Guid[] targets = reader.Read<Guid[]>(C_MISSING_REFERENCE_TARGET_IDS_KEY);
        if (aliases.Length != targets.Length)
        {
            throw new InvalidDataException($"Missing scene reference alias arrays disagree at '{reader.path}'.");
        }
        if (aliases.Any(static value => value == Guid.Empty) || targets.Any(static value => value == Guid.Empty))
        {
            throw new InvalidDataException($"Missing scene reference aliases contain an empty identity at '{reader.path}'.");
        }
        if (aliases.Distinct().Count() != aliases.Length)
        {
            throw new InvalidDataException($"Missing scene reference aliases contain duplicates at '{reader.path}'.");
        }
    }

    private static void ValidateSystems(IReadOnlyList<SerializationReader> systemReaders)
    {
        var identities = new HashSet<Guid>();
        foreach (SerializationReader systemReader in systemReaders)
        {
            Guid systemID = systemReader.Read<Guid>(C_SYSTEM_ID_KEY);
            EnsurePersistentID(systemID, $"{systemReader.path}.{C_SYSTEM_ID_KEY}");
            if (!identities.Add(systemID))
            {
                throw new InvalidDataException($"Duplicate GameSystem identity '{systemID}' at '{systemReader.path}'.");
            }
            Guid stableTypeID = systemReader.Read<Guid>(C_STABLE_TYPE_ID_KEY);
            EnsurePersistentID(stableTypeID, $"{systemReader.path}.{C_STABLE_TYPE_ID_KEY}");
            _ = TryResolveSystemType(stableTypeID, systemReader.context, out _);
            EnsureTypeName(systemReader.Read<string>(C_TYPE_NAME_KEY), systemReader.path);
            _ = systemReader.Read<byte[]>(C_STATE_KEY);
            _ = ReadStateDependencies(systemReader);
        }
    }

    private static IEnumerable<GameObject> EnumerateRestoredObjects(
        GameObject root,
        bool includeRoot
    ) {
        var pending = new Stack<Transform>();
        if (includeRoot)
        {
            pending.Push(root.transform);
        }
        else
        {
            foreach (Transform child in root.transform.children)
            {
                pending.Push(child);
            }
        }
        while (pending.Count != 0)
        {
            Transform current = pending.Pop();
            yield return current.gameObject;
            foreach (Transform child in current.children)
            {
                pending.Push(child);
            }
        }
    }

    private static Guid GetSourceID(
        IReadOnlyDictionary<EngineObject, Guid> sourceIDs,
        EngineObject engineObject
    ) => sourceIDs.TryGetValue(engineObject, out Guid sourceID)
         ? sourceID
         : throw new InvalidOperationException(
             $"Engine object '{engineObject.identity.persistentID}' has no source-local identity."
         );

    private static void WriteMissingReferenceAliases(
        SerializationWriter writer,
        IReadOnlyDictionary<EngineObject, Guid> sourceIDs
    ) {
        EngineObject[] missing = sourceIDs.Keys
            .Where(static engineObject => engineObject is MissingGameComponent or MissingGameSystem)
            .ToArray();
        if (missing.Length == 0)
        {
            writer.Write(C_MISSING_REFERENCE_SOURCE_IDS_KEY, Array.Empty<Guid>());
            writer.Write(C_MISSING_REFERENCE_TARGET_IDS_KEY, Array.Empty<Guid>());
            return;
        }

        Dictionary<Guid, Guid> targetByPersistentID = sourceIDs.Keys.ToDictionary(
            static engineObject => engineObject.identity.persistentID,
            engineObject => sourceIDs[engineObject]
        );
        var aliases = new Dictionary<Guid, Guid>();
        foreach ((Guid persistentID, Guid sourceID) in targetByPersistentID)
        {
            if (persistentID != sourceID)
            {
                aliases[persistentID] = sourceID;
            }
        }
        for (int i = 0; i < missing.Length; i++)
        {
            IReadOnlyDictionary<Guid, Guid> retained = missing[i] switch
            {
                MissingGameComponent component => component.referenceAliases,
                MissingGameSystem system => system.referenceAliases,
                _ => throw new InvalidOperationException("Unknown missing scene element.")
            };
            foreach ((Guid alias, Guid runtimePersistentID) in retained)
            {
                if (targetByPersistentID.TryGetValue(runtimePersistentID, out Guid targetSourceID)
                    && alias != targetSourceID
                ) {
                    aliases[alias] = targetSourceID;
                }
            }
        }
        KeyValuePair<Guid, Guid>[] ordered = aliases
            .OrderBy(static pair => pair.Key)
            .ToArray();
        writer.Write(C_MISSING_REFERENCE_SOURCE_IDS_KEY, ordered.Select(static pair => pair.Key).ToArray());
        writer.Write(C_MISSING_REFERENCE_TARGET_IDS_KEY, ordered.Select(static pair => pair.Value).ToArray());
    }

    private static void RestoreMissingReferenceAliases(
        IReadOnlyList<KeyValuePair<Guid, Guid>> aliases,
        SceneGraphReferenceMap references,
        IReadOnlyList<EngineObject> missingPlaceholders
    ) {
        Dictionary<Guid, Guid> retained = references.CaptureRemappedAliases();
        foreach ((Guid alias, Guid targetSourceID) in aliases)
        {
            EngineObject target = references.GetRegistered(targetSourceID);
            references.Register(alias, target);
            retained[alias] = target.identity.persistentID;
        }
        for (int i = 0; i < missingPlaceholders.Count; i++)
        {
            if (missingPlaceholders[i] is MissingGameComponent component)
            {
                component.SetReferenceAliases(retained);
            }
            else if (missingPlaceholders[i] is MissingGameSystem system)
            {
                system.SetReferenceAliases(retained);
            }
        }
    }

    private static void WritePrefabConnection(
        SerializationWriter writer,
        PrefabConnectionRecord connection,
        PrefabOverrideSet overrides,
        IReadOnlyDictionary<EngineObject, Guid> serializedIDs,
        GameScene scene
    ) {
        KeyValuePair<Guid, Guid>[] liveComponentMappings = connection.componentIdentities
            .Where(pair => scene.FindComponent(pair.Value) is not null)
            .ToArray();
        writer.WriteObjectArray(
            C_PREFAB_COMPONENT_MAPPINGS_KEY,
            liveComponentMappings,
            (
                mappingWriter,
                pair
            ) =>
            {
                GameComponent component = scene.FindComponent(pair.Value)!;
                mappingWriter.Write(C_PREFAB_MAPPING_SOURCE_ID_KEY, pair.Key);
                mappingWriter.Write(C_PREFAB_MAPPING_INSTANCE_ID_KEY, GetSourceID(serializedIDs, component));
            }
        );
        writer.WriteObjectArray(
            C_PREFAB_PROPERTY_OVERRIDES_KEY,
            overrides.properties,
            static (
                overrideWriter,
                property
            ) =>
            {
                overrideWriter.Write(C_PREFAB_MAPPING_SOURCE_ID_KEY, property.sourceComponentID);
                overrideWriter.Write(C_PREFAB_PROPERTY_NAME_KEY, property.propertyName);
                overrideWriter.Write(C_PREFAB_PROPERTY_VALUE_KEY, property.value);
                overrideWriter.Write(C_PREFAB_OVERRIDE_ORPHANED_KEY, property.isOrphaned);
            }
        );
        writer.WriteObjectArray(
            C_PREFAB_STRUCTURE_OVERRIDES_KEY,
            overrides.structures,
            static (
                overrideWriter,
                structure
            ) =>
            {
                overrideWriter.Write(C_PREFAB_MAPPING_SOURCE_ID_KEY, structure.sourceObjectID);
                overrideWriter.Write(C_PREFAB_STRUCTURE_KIND_KEY, (int)structure.kind);
                overrideWriter.Write(C_PREFAB_OVERRIDE_ORPHANED_KEY, structure.isOrphaned);
            }
        );
        writer.Write(C_PREFAB_REMOVED_OBJECTS_KEY, overrides.removedObjects.ToArray());
        writer.Write(C_PREFAB_REMOVED_COMPONENTS_KEY, overrides.removedComponents.ToArray());
        writer.Write(C_PREFAB_ADDED_OBJECTS_KEY, overrides.addedObjects.ToArray());
        writer.Write(C_PREFAB_ADDED_COMPONENTS_KEY, overrides.addedComponents.ToArray());
    }

    private static PrefabOverrideSet ReadPrefabOverrides(SerializationReader reader)
    {
        var result = new PrefabOverrideSet();
        foreach (SerializationReader propertyReader in
                 reader.ReadObjectArray(C_PREFAB_PROPERTY_OVERRIDES_KEY)
        ) {
            result.SetProperty(
                new PrefabPropertyOverride(
                    propertyReader.Read<Guid>(C_PREFAB_MAPPING_SOURCE_ID_KEY),
                    propertyReader.Read<string>(C_PREFAB_PROPERTY_NAME_KEY),
                    propertyReader.Read<byte[]>(C_PREFAB_PROPERTY_VALUE_KEY),
                    propertyReader.Read<bool>(C_PREFAB_OVERRIDE_ORPHANED_KEY)
                )
            );
        }
        foreach (SerializationReader structureReader in
                 reader.ReadObjectArray(C_PREFAB_STRUCTURE_OVERRIDES_KEY)
        ) {
            result.SetStructure(
                structureReader.Read<Guid>(C_PREFAB_MAPPING_SOURCE_ID_KEY),
                (PrefabObjectOverrideKind)structureReader.Read<int>(C_PREFAB_STRUCTURE_KIND_KEY),
                structureReader.Read<bool>(C_PREFAB_OVERRIDE_ORPHANED_KEY)
            );
        }
        foreach (Guid sourceID in reader.Read<Guid[]>(C_PREFAB_REMOVED_OBJECTS_KEY))
        {
            result.MarkObjectRemoved(sourceID);
        }
        foreach (Guid sourceID in reader.Read<Guid[]>(C_PREFAB_REMOVED_COMPONENTS_KEY))
        {
            result.MarkComponentRemoved(sourceID);
        }
        foreach (Guid instanceID in reader.Read<Guid[]>(C_PREFAB_ADDED_OBJECTS_KEY))
        {
            result.MarkObjectAdded(instanceID);
        }
        foreach (Guid instanceID in reader.Read<Guid[]>(C_PREFAB_ADDED_COMPONENTS_KEY))
        {
            result.MarkComponentAdded(instanceID);
        }
        return result;
    }

    private static bool IsTransform(SerializationReader reader)
        => TryResolveComponentType(reader.Read<Guid>(C_STABLE_TYPE_ID_KEY), reader.context, out Type? type)
           && type == typeof(Transform);

    private static bool TryResolveComponentType(
        Guid stableTypeID,
        SerializationContext context,
        out Type? componentType
    ) {
        if (!TryResolve(new TypeRef(stableTypeID), context, out componentType))
        {
            return false;
        }
        Type resolved = componentType!;
        if (!typeof(GameComponent).IsAssignableFrom(resolved) || resolved.IsAbstract)
        {
            throw new InvalidDataException(
                $"Stable type id '{stableTypeID}' resolves to invalid component type '{resolved.FullName}'."
            );
        }
        return true;
    }

    private static bool TryResolveSystemType(
        Guid stableTypeID,
        SerializationContext context,
        out Type? systemType
    ) {
        if (!TryResolve(new TypeRef(stableTypeID), context, out systemType))
        {
            return false;
        }
        Type resolved = systemType!;
        if (!typeof(GameSystem).IsAssignableFrom(resolved) || resolved.IsAbstract)
        {
            throw new InvalidDataException($"Stable type id '{stableTypeID}' resolves to invalid system '{resolved.FullName}'.");
        }
        return true;
    }

    private static bool TryResolve(
        TypeRef typeRef,
        SerializationContext context,
        out Type? type
    ) => context.GetRequired<TypeCatalog>().TryResolve(typeRef, out type);

    private static void WriteComponent(
        SerializationWriter writer,
        GameComponent component,
        IReadOnlyDictionary<EngineObject, Guid> sourceIDs
    ) {
        writer.Write(C_COMPONENT_ID_KEY, GetSourceID(sourceIDs, component));
        if (component is MissingGameComponent missing)
        {
            writer.Write(C_STABLE_TYPE_ID_KEY, missing.missingType.stableID);
            writer.Write(C_TYPE_NAME_KEY, missing.missingTypeName);
            writer.Write(C_STATE_KEY, missing.CaptureSerializedState());
            WriteStateDependencies(writer, missing.dependencies);
            return;
        }
        writer.Write(C_STABLE_TYPE_ID_KEY, writer.typeSnapshot.GetTypeRef(component.GetType()).stableID);
        writer.Write(C_TYPE_NAME_KEY, component.GetType().FullName ?? component.GetType().Name);
        GameObject gameObject = component.gameObject;
        try
        {
            AssetDependency[] dependencies = WriteState(writer, component);
            WriteStateDependencies(writer, dependencies);
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                $"Failed to serialize GameComponent '{component.GetType().FullName}' on "
                + $"GameObject '{gameObject.name}' at '{writer.path}'. {exception.Message}",
                exception
            );
        }
    }

    private static AssetDependency[] WriteState(
        SerializationWriter writer,
        ISerializable value
    ) {
        SerializationContext outerContext = writer.context;
        bool includeLastKnownPaths = !outerContext.TryGet(out AssetDependencyCollection? outerDependencies)
                                     || outerDependencies is null
                                     || outerDependencies.includeLastKnownPaths;
        var dependencies = new AssetDependencyCollection(includeLastKnownPaths);
        writer.WritePropertiesData(C_STATE_KEY, value, outerContext.With(dependencies));
        return dependencies.dependencies.ToArray();
    }

    private static void WriteStateDependencies(
        SerializationWriter writer,
        IReadOnlyList<AssetDependency> dependencies
    ) {
        writer.Write(C_STATE_DEPENDENCIES_KEY, dependencies.ToArray());
        IAssetReferenceResolver resolver = writer.context.GetRequired<IAssetReferenceResolver>();
        if (dependencies.Count == 0)
        {
            writer.Write(C_STATE_DEPENDENCY_ASSETS_KEY, Array.Empty<AssetObject>());
            return;
        }
        var assets = new AssetObject[dependencies.Count];
        for (int index = 0; index < dependencies.Count; index++)
        {
            AssetDependency dependency = dependencies[index];
            assets[index] = resolver.Resolve(
                dependency.persistentID,
                dependency.type.stableID,
                dependency.lastKnownPath,
                typeof(AssetObject),
                "scene.state.dependencies"
            );
        }
        writer.Write(C_STATE_DEPENDENCY_ASSETS_KEY, assets);
    }

    private static AssetDependency[] ReadStateDependencies(SerializationReader reader)
    {
        AssetDependency[] dependencies = reader.Read<AssetDependency[]>(C_STATE_DEPENDENCIES_KEY);
        AssetObject[] assets = reader.Read<AssetObject[]>(C_STATE_DEPENDENCY_ASSETS_KEY);
        if (dependencies.Length != assets.Length)
        {
            throw new InvalidDataException(
                $"Scene state dependency descriptors and assets disagree at '{reader.path}'."
            );
        }
        for (int i = 0; i < dependencies.Length; i++)
        {
            if (dependencies[i].persistentID != assets[i].identity.persistentID)
            {
                throw new InvalidDataException(
                    $"Scene state dependency '{dependencies[i].persistentID}' does not match its asset token at '{reader.path}'."
                );
            }
        }
        return dependencies;
    }

    private static void EnsurePersistentID(
        Guid persistentID,
        string path
    ) {
        if (persistentID == Guid.Empty)
        {
            throw new InvalidDataException($"Persistent or local identity at '{path}' cannot be empty.");
        }
    }

    private static void EnsureTypeName(
        string typeName,
        string path
    ) {
        if (string.IsNullOrWhiteSpace(typeName))
        {
            throw new InvalidDataException($"Scene element type name at '{path}' cannot be empty.");
        }
    }
}
