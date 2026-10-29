using System;
using System.Collections.Generic;

using Inno.Scene;

namespace Inno.Scene;

/// <summary>
/// Freezes local identities for graph construction without indexing live property access.
/// </summary>
internal sealed class SceneGraphReferenceMap : SceneReferenceScope
{
    private readonly IReadOnlyDictionary<EngineObject, Guid>? m_sourceIdByObject;
    private readonly Dictionary<Guid, EngineObject> m_objectBySourceId = new();

    internal SceneGraphReferenceMap(
        GameScene scene,
        IReadOnlyDictionary<EngineObject, Guid>? sourceIds = null,
        GameObject? captureRoot = null
    ) : base(scene, captureRoot) {
        // Capture callers own the complete map and keep it fixed for this synchronous scope.
        m_sourceIdByObject = sourceIds;
    }

    internal void Register(
        Guid sourceId,
        EngineObject engineObject
    ) {
        if (!m_objectBySourceId.TryAdd(sourceId, engineObject) &&
            !ReferenceEquals(m_objectBySourceId[sourceId], engineObject))
        {
            throw new InvalidOperationException($"Duplicate scene graph local identity '{sourceId}'.");
        }
    }

    internal Dictionary<Guid, Guid> CaptureRemappedAliases()
    {
        var aliases = new Dictionary<Guid, Guid>();
        foreach ((Guid sourceId, EngineObject engineObject) in m_objectBySourceId)
        {
            Guid persistentId = engineObject.identity.persistentId;
            if (sourceId != persistentId)
                aliases.Add(sourceId, persistentId);
        }
        return aliases;
    }

    internal EngineObject GetRegistered(Guid sourceId)
        => m_objectBySourceId.TryGetValue(sourceId, out EngineObject? engineObject)
            ? engineObject
            : throw new InvalidOperationException(
                $"Scene graph reference alias target '{sourceId}' could not be resolved.");

    protected override bool TryGetSourceId(
        EngineObject engineObject,
        out Guid sourceId
    ) {
        sourceId = default;
        return m_sourceIdByObject is not null && m_sourceIdByObject.TryGetValue(engineObject, out sourceId);
    }

    protected override bool TryResolve(
        Guid sourceId,
        out EngineObject? engineObject
    ) => m_objectBySourceId.TryGetValue(sourceId, out engineObject);
}
