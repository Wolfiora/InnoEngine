using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Scene;

internal sealed class PrefabOverrideSet
{
    private readonly Dictionary<(Guid sourceComponentID, string propertyName), PrefabPropertyOverride>
        m_properties = [];

    private readonly Dictionary<Guid, PrefabStructureOverride> m_structures = [];
    private readonly HashSet<Guid> m_removedObjects = [];
    private readonly HashSet<Guid> m_removedComponents = [];
    private readonly HashSet<Guid> m_addedObjects = [];
    private readonly HashSet<Guid> m_addedComponents = [];

    internal IReadOnlyCollection<PrefabPropertyOverride> properties => m_properties.Values;
    internal IReadOnlyCollection<PrefabStructureOverride> structures => m_structures.Values;
    internal IReadOnlyCollection<Guid> removedObjects => m_removedObjects;
    internal IReadOnlyCollection<Guid> removedComponents => m_removedComponents;
    internal IReadOnlyCollection<Guid> addedObjects => m_addedObjects;
    internal IReadOnlyCollection<Guid> addedComponents => m_addedComponents;

    internal int count
        => m_properties.Count + m_structures.Count
           + m_removedObjects.Count + m_removedComponents.Count
           + m_addedObjects.Count + m_addedComponents.Count;

    internal int orphanedCount
        => m_properties.Values.Count(static item => item.isOrphaned)
           + m_structures.Values.Count(static item => item.isOrphaned);

    internal bool IsPropertyOverridden(
        Guid sourceComponentID,
        string propertyName
    ) => m_properties.ContainsKey((sourceComponentID, propertyName));

    internal PrefabObjectOverrideKind GetStructureOverride(Guid sourceObjectID)
        => m_structures.TryGetValue(sourceObjectID, out PrefabStructureOverride? value)
           ? value.kind
           : PrefabObjectOverrideKind.None;

    internal void SetProperty(PrefabPropertyOverride value)
    {
        ArgumentNullException.ThrowIfNull(value);
        m_properties[(value.sourceComponentID, value.propertyName)] = value;
    }

    internal void SetStructure(
        Guid sourceObjectID,
        PrefabObjectOverrideKind kind,
        bool isOrphaned = false
    ) {
        if (kind == PrefabObjectOverrideKind.None)
        {
            m_structures.Remove(sourceObjectID);
            return;
        }
        m_structures[sourceObjectID] = new PrefabStructureOverride(sourceObjectID, kind, isOrphaned);
    }

    internal void MarkObjectRemoved(Guid sourceObjectID) => m_removedObjects.Add(sourceObjectID);

    internal void MarkComponentRemoved(Guid sourceComponentID) => m_removedComponents.Add(sourceComponentID);

    internal void MarkObjectAdded(Guid instanceObjectID) => m_addedObjects.Add(instanceObjectID);

    internal void MarkComponentAdded(Guid instanceComponentID) => m_addedComponents.Add(instanceComponentID);

    internal bool IsObjectRemoved(Guid sourceObjectID) => m_removedObjects.Contains(sourceObjectID);

    internal bool IsComponentRemoved(Guid sourceComponentID) => m_removedComponents.Contains(sourceComponentID);
}
