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
    private Dictionary<EngineObject, Guid>? m_sourceIds;
    private IReadOnlyDictionary<EngineObject, Guid>? m_frozenSourceIds;

    internal SceneStructureSnapshot(IReadOnlyList<SceneObjectStructureSnapshot> objects) => this.objects = objects;

    internal IReadOnlyList<SceneObjectStructureSnapshot> objects { get; }

    internal IReadOnlyDictionary<EngineObject, Guid> CaptureObjectSourceIds()
    {
        if (m_sourceIds is not null && HasCurrentIdentities(m_sourceIds))
            return m_frozenSourceIds!;

        int count = objects.Count;
        foreach (SceneObjectStructureSnapshot entry in objects)
            count = checked(count + entry.components.Count);
        var sourceIds = new Dictionary<EngineObject, Guid>(count, ReferenceEqualityComparer.Instance);
        foreach (SceneObjectStructureSnapshot entry in objects)
        {
            sourceIds.Add(entry.gameObject, entry.gameObject.identity.persistentId);
            for (int index = 0; index < entry.components.Count; index++)
            {
                GameComponent component = entry.components[index];
                sourceIds.Add(component, component.identity.persistentId);
            }
        }
        // Replace the projection so an already borrowed graph write keeps its original identities.
        var frozen = new ReadOnlyDictionary<EngineObject, Guid>(sourceIds);
        m_sourceIds = sourceIds;
        m_frozenSourceIds = frozen;
        return frozen;
    }

    private static bool HasCurrentIdentities(Dictionary<EngineObject, Guid> sourceIds)
    {
        // Core registry rebinding can replace an identity without changing the structural snapshot.
        foreach ((EngineObject value, Guid persistentId) in sourceIds)
            if (value.identity.persistentId != persistentId)
                return false;
        return true;
    }
}
