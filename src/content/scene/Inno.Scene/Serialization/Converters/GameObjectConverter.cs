using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;
using Inno.Scene;
using Inno.Scene.Components;

namespace Inno.Scene;

internal sealed class GameObjectConverter : SerializationConverter<GameObject>
{
    private const string C_SOURCE_ROOT_ID_KEY = "sourceRootId";

    /// <summary>
    /// Writes the supplied value through the owning subsystem's validated output boundary.
    /// </summary>
    /// <param name="writer">
    /// The writer that receives the deterministic structured representation.
    /// </param>
    /// <param name="value">
    /// The concrete value read or transformed by this operation.
    /// </param>
    public override void Write(
        SerializationWriter writer,
        GameObject value
    ) {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);
        if (SceneReferenceScope.TryGetCurrent(out SceneReferenceScope? activeReferences))
        {
            WriteReference(writer, activeReferences!, value);
            return;
        }

        if (!value.isRuntimeValid)
        {
            throw new InvalidOperationException("A destroyed or detached GameObject cannot be captured as a prefab root.");
        }

        var subtree = new List<GameObject>();
        CollectSubtree(value, subtree);
        SceneStructureSnapshot snapshot = value.scene.CaptureStructure();
        var included = new HashSet<GameObject>(subtree, ReferenceEqualityComparer.Instance);
        SceneObjectStructureSnapshot[] entries = snapshot.objects
            .Where(entry => included.Contains(entry.gameObject))
            .ToArray();
        EngineObject[] engineObjects = entries
            .SelectMany(static entry => entry.components.Cast<EngineObject>().Prepend(entry.gameObject))
            .ToArray();
        var sourceIds = new Dictionary<EngineObject, Guid>(ReferenceEqualityComparer.Instance);
        PrefabConnectionRecord? existingConnection = value.prefabConnection;
        var mappedIds = new Dictionary<Guid, Guid>();
        if (existingConnection is not null)
        {
            foreach ((Guid sourceID, Guid runtimeID) in existingConnection.objectIdentities)
            {
                mappedIds[runtimeID] = sourceID;
            }
            foreach ((Guid sourceID, Guid runtimeID) in existingConnection.componentIdentities)
            {
                mappedIds[runtimeID] = sourceID;
            }
        }
        for (int i = 0; i < engineObjects.Length; i++)
        {
            sourceIds.Add(
                engineObjects[i],
                mappedIds.TryGetValue(engineObjects[i].identity.persistentId, out Guid mappedID)
                ? mappedID
                : Guid.NewGuid()
            );
        }

        var references = new SceneGraphReferenceMap(value.scene, sourceIds, value);
        writer.Write(C_SOURCE_ROOT_ID_KEY, sourceIds[value]);
        using (references.Enter())
        {
            SceneGraphSerialization.WriteObjects(
                writer,
                entries,
                sourceIds,
                included.Contains,
                preserveRootSiblingOrder: false
            );
        }
    }

    /// <summary>
    /// Reads and validates the requested value without transferring storage ownership.
    /// </summary>
    /// <param name="reader">
    /// The reader positioned at the structured value to decode.
    /// </param>
    /// <returns>
    /// The validated game object that represents the completed operation.
    /// </returns>
    public override GameObject Read(SerializationReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);
        if (SceneReferenceScope.TryGetCurrent(out SceneReferenceScope? activeReferences))
        {
            return (GameObject)ReadReference(reader, activeReferences!);
        }

        GameScene scene = reader.context.GetRequired<GameScene>();
        Transform? parent = null;
        if (reader.context.TryGet(out Transform? configuredParent))
        {
            parent = configuredParent;
        }
        if (parent is not null && !ReferenceEquals(parent.gameObject.scene, scene))
        {
            throw new InvalidOperationException("The prefab parent must belong to the target scene.");
        }

        ValidatePrefab(reader);
        var existingObjects = new HashSet<GameObject>(scene.GetOwnedObjects(), ReferenceEqualityComparer.Instance);
        try
        {
            var references = new SceneGraphReferenceMap(scene);
            RestoredSceneGraph restored = SceneGraphSerialization.RestoreObjects(
                scene,
                reader.ReadObjectArray(SceneGraphSerialization.C_OBJECTS_KEY),
                preservePersistentIds: false,
                references,
                SceneGraphSerialization.ReadMissingReferenceAliases(reader),
                reader.context
            );
            Guid sourceRootID = reader.Read<Guid>(C_SOURCE_ROOT_ID_KEY);
            if (!restored.objects.TryGetValue(sourceRootID, out GameObject? root))
            {
                throw new InvalidDataException($"Prefab root '{sourceRootID}' is missing at '{reader.path}'.");
            }
            if (reader.context.TryGet(out AssetObject? sourceAsset) && sourceAsset is not null)
            {
                PrefabConnectionRecord? embeddedConnection = root.prefabConnection;
                bool sourceRepresentsVariant = embeddedConnection is not null
                                               && root.prefabInstance?.isRoot == true;
                if (sourceRepresentsVariant)
                {
                    PrefabOverrideProcessor.Reconcile(embeddedConnection!, root, reader.context);
                }
                var connection = new PrefabConnectionRecord(sourceAsset, sourceRootID, sourceRepresentsVariant);
                foreach ((Guid sourceObjectID, GameObject gameObject) in restored.objects)
                {
                    connection.MapObject(sourceObjectID, gameObject);
                }
                foreach ((Guid sourceComponentID, GameComponent component) in restored.components)
                {
                    connection.MapComponent(sourceComponentID, component);
                }
                if (sourceRepresentsVariant)
                {
                    foreach ((Guid sourceObjectID, Guid runtimeID) in embeddedConnection!.objectIdentities)
                    {
                        GameObject? mappedObject = scene.FindObject(runtimeID);
                        if (mappedObject is not null)
                        {
                            connection.MapObject(sourceObjectID, mappedObject);
                        }
                    }
                    foreach ((Guid sourceComponentID, Guid runtimeID) in embeddedConnection.componentIdentities)
                    {
                        GameComponent? mappedComponent = scene.FindComponent(runtimeID);
                        if (mappedComponent is not null)
                        {
                            connection.MapComponent(sourceComponentID, mappedComponent);
                        }
                    }
                }
                root.SetPrefabConnectionDirect(connection);

                foreach ((Guid sourceObjectID, Guid runtimeID) in connection.objectIdentities)
                {
                    GameObject? gameObject = scene.FindObject(runtimeID);
                    if (gameObject is null)
                    {
                        continue;
                    }
                    PrefabInstanceInfo? existingConnection = gameObject.prefabInstance;
                    if (existingConnection is not null
                        && !ReferenceEquals(existingConnection.instanceRoot, root)
                    ) {
                        continue;
                    }
                    gameObject.SetPrefabInstanceDirect(
                        new PrefabInstanceInfo(
                            sourceAsset.identity.persistentId,
                            sourceObjectID,
                            root,
                            ReferenceEquals(gameObject, root),
                            sourceRepresentsVariant,
                            sourceAsset.isMissing,
                            existingConnection?.overrideCount ?? 0,
                            existingConnection?.orphanedOverrideCount ?? 0,
                            sourceAsset
                        )
                    );
                }
            }
            SceneGraphSerialization.ReconcilePrefabConnections(scene, reader.context, root);
            if (parent is not null)
            {
                scene.SetParent(root.transform, parent, worldPositionStays: false);
            }
            return root;
        }
        catch
        {
            GameObject[] createdObjects = scene.GetOwnedObjects()
                .Where(gameObject => !existingObjects.Contains(gameObject))
                .ToArray();
            for (int i = 0; i < createdObjects.Length; i++)
            {
                if (createdObjects[i].isRuntimeValid)
                {
                    scene.DestroyObject(createdObjects[i]);
                }
            }
            throw;
        }
    }

    private static void WriteReference(
        SerializationWriter writer,
        SceneReferenceScope references,
        GameObject gameObject
    ) {
        EngineReferenceToken token = references.Capture(gameObject, writer.path);
        writer.Write("kind", (int)token.kind);
        writer.Write("sourceId", token.sourceID);
    }

    private static EngineObject ReadReference(
        SerializationReader reader,
        SceneReferenceScope references
    ) {
        var token = new EngineReferenceToken(
            (EngineReferenceKind)reader.Read<int>("kind"),
            reader.Read<Guid>("sourceId")
        );
        if (token.kind != EngineReferenceKind.GameObject)
        {
            throw new InvalidDataException($"Reference token at '{reader.path}' is not a GameObject reference.");
        }
        return references.Resolve(token, reader.valueType, reader.path);
    }

    private static void ValidatePrefab(SerializationReader reader)
    {
        SceneGraphSerialization.ValidateMissingReferenceAliases(reader);
        Guid rootID = reader.Read<Guid>(C_SOURCE_ROOT_ID_KEY);
        if (rootID == Guid.Empty)
        {
            throw new InvalidDataException($"Prefab source root identity at '{reader.path}' cannot be empty.");
        }
        IReadOnlyList<SerializationReader> objectReaders =
            reader.ReadObjectArray(SceneGraphSerialization.C_OBJECTS_KEY);
        SceneGraphSerialization.ValidateObjects(objectReaders);
        if (!objectReaders.Any(objectReader => (
            objectReader.Read<Guid>(SceneGraphSerialization.C_OBJECT_ID_KEY) == rootID
        ))
        ) {
            throw new InvalidDataException($"Prefab root '{rootID}' is missing at '{reader.path}'.");
        }
    }

    private static void CollectSubtree(
        GameObject gameObject,
        ICollection<GameObject> result
    ) {
        result.Add(gameObject);
        IReadOnlyList<Transform> children = gameObject.transform.children;
        for (int i = 0; i < children.Count; i++)
        {
            CollectSubtree(children[i].gameObject, result);
        }
    }
}
