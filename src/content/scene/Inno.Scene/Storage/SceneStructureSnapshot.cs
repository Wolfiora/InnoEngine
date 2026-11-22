using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inno.Scene;

/// <summary>
/// Captures the stable attachment order for one scene object.
/// </summary>
/// <param name="gameObject">
/// The game object used to initialize this instance.
/// </param>
/// <param name="components">
/// The components used to initialize this instance.
/// </param>
internal sealed record SceneObjectStructureSnapshot(
    GameObject gameObject,
    IReadOnlyList<GameComponent> components
);

/// <summary>
/// Owns one attachment snapshot and its frozen object/component serialization identities.
/// </summary>
internal sealed class SceneStructureSnapshot
{
    private Dictionary<EngineObject, Guid>? m_sourceIDs;
    private IReadOnlyDictionary<EngineObject, Guid>? m_frozenSourceIDs;

    internal SceneStructureSnapshot(IReadOnlyList<SceneObjectStructureSnapshot> objects)
    {
        this.objects = objects;
    }

    internal IReadOnlyList<SceneObjectStructureSnapshot> objects { get; }

    internal IReadOnlyDictionary<EngineObject, Guid> CaptureObjectSourceIDs()
    {
        if (m_sourceIDs is not null && HasCurrentIdentities(m_sourceIDs))
        {
            return m_frozenSourceIDs!;
        }

        int count = objects.Count;
        foreach (SceneObjectStructureSnapshot entry in objects)
        {
            count = checked(count + entry.components.Count);
        }
        var sourceIDs = new Dictionary<EngineObject, Guid>(count, ReferenceEqualityComparer.Instance);
        foreach (SceneObjectStructureSnapshot entry in objects)
        {
            sourceIDs.Add(entry.gameObject, entry.gameObject.identity.persistentID);
            for (int index = 0; index < entry.components.Count; index++)
            {
                GameComponent component = entry.components[index];
                sourceIDs.Add(component, component.identity.persistentID);
            }
        }
        // Replace the projection so an already borrowed graph write keeps its original identities.
        var frozen = new ReadOnlyDictionary<EngineObject, Guid>(sourceIDs);
        m_sourceIDs = sourceIDs;
        m_frozenSourceIDs = frozen;
        return frozen;
    }

    private static bool HasCurrentIdentities(Dictionary<EngineObject, Guid> sourceIDs)
    {
        // Core registry rebinding can replace an identity without changing the structural snapshot.
        foreach ((EngineObject value, Guid persistentID) in sourceIDs)
        {
            if (value.identity.persistentID != persistentID)
            {
                return false;
            }
        }
        return true;
    }
}
