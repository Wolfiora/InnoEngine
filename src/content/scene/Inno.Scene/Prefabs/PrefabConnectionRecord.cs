using System;
using System.Collections.Generic;
using Inno.Assets;

namespace Inno.Scene;

internal sealed class PrefabConnectionRecord
{
    private readonly Dictionary<Guid, Guid> m_objectIdentities = [];
    private readonly Dictionary<Guid, Guid> m_componentIdentities = [];

    internal PrefabConnectionRecord(
        AssetObject sourceAsset,
        Guid sourceRootID,
        bool isVariant,
        PrefabOverrideSet? overrides = null
    ) {
        ArgumentNullException.ThrowIfNull(sourceAsset);
        this.sourceAsset = sourceAsset;
        this.sourceRootID = sourceRootID;
        this.isVariant = isVariant;
        this.overrides = overrides ?? new PrefabOverrideSet();
    }

    internal AssetObject sourceAsset { get; }
    internal Guid sourceRootID { get; }
    internal bool isVariant { get; }
    internal PrefabOverrideSet overrides { get; set; }
    internal IReadOnlyDictionary<Guid, Guid> objectIdentities => m_objectIdentities;
    internal IReadOnlyDictionary<Guid, Guid> componentIdentities => m_componentIdentities;

    internal void MapObject(
        Guid sourceID,
        GameObject gameObject
    ) {
        ArgumentNullException.ThrowIfNull(gameObject);
        m_objectIdentities[sourceID] = gameObject.identity.persistentID;
    }

    internal void MapComponent(
        Guid sourceID,
        GameComponent component
    ) {
        ArgumentNullException.ThrowIfNull(component);
        m_componentIdentities[sourceID] = component.identity.persistentID;
    }

    internal void RemoveObject(Guid sourceID) => m_objectIdentities.Remove(sourceID);

    internal void RemoveComponent(Guid sourceID) => m_componentIdentities.Remove(sourceID);
}
