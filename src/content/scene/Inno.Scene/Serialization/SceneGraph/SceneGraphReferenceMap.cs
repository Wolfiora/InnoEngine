using System;
using System.Collections.Generic;
using Inno.Scene;

namespace Inno.Scene;

/// <summary>
/// Freezes local identities for graph construction without indexing live property access.
/// </summary>
internal sealed class SceneGraphReferenceMap : SceneReferenceScope
{
    private readonly IReadOnlyDictionary<EngineObject, Guid>? m_sourceIDByObject;
    private readonly Dictionary<Guid, EngineObject> m_objectBySourceID = [];

    internal SceneGraphReferenceMap(
        GameScene scene,
        IReadOnlyDictionary<EngineObject, Guid>? sourceIds = null,
        GameObject? captureRoot = null
    ) : base(scene, captureRoot)
    {
        // Capture callers own the complete map and keep it fixed for this synchronous scope.
        m_sourceIDByObject = sourceIds;
    }

    /// <inheritdoc/>
    protected override bool TryGetSourceID(
        EngineObject engineObject,
        out Guid sourceId
    ) {
        sourceId = default;
        return m_sourceIDByObject is not null && m_sourceIDByObject.TryGetValue(engineObject, out sourceId);
    }

    /// <inheritdoc/>
    protected override bool TryResolve(
        Guid sourceId,
        out EngineObject? engineObject
    ) => m_objectBySourceID.TryGetValue(sourceId, out engineObject);

    internal void Register(
        Guid sourceID,
        EngineObject engineObject
    ) {
        if (!m_objectBySourceID.TryAdd(sourceID, engineObject)
            && !ReferenceEquals(m_objectBySourceID[sourceID], engineObject)
        ) {
            throw new InvalidOperationException($"Duplicate scene graph local identity '{sourceID}'.");
        }
    }

    internal Dictionary<Guid, Guid> CaptureRemappedAliases()
    {
        var aliases = new Dictionary<Guid, Guid>();
        foreach ((Guid sourceID, EngineObject engineObject) in m_objectBySourceID)
        {
            Guid persistentID = engineObject.identity.persistentId;
            if (sourceID != persistentID)
            {
                aliases.Add(sourceID, persistentID);
            }
        }
        return aliases;
    }

    internal EngineObject GetRegistered(Guid sourceID)
        => m_objectBySourceID.TryGetValue(sourceID, out EngineObject? engineObject)
           ? engineObject
           : throw new InvalidOperationException(
               $"Scene graph reference alias target '{sourceID}' could not be resolved."
           );
}
