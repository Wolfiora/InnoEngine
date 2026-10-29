using System;
using System.Collections.Generic;
using Inno.Core.Identity;

namespace Inno.Scene;

/// <summary>
/// Resolves property references through the current owner domain without materializing a second live-object index.
/// </summary>
internal sealed class ScenePropertyReferenceScope : SceneReferenceScope
{
    private readonly IdentityAllocator m_identities;
    private readonly RuntimeIdentity m_sceneIdentity;
    private readonly Dictionary<Guid, Guid>? m_aliases;

    internal ScenePropertyReferenceScope(
        GameScene scene,
        IReadOnlyDictionary<Guid, Guid>? aliases = null
    ) : base(scene) {
        m_identities = IdentityAllocator.current;
        if (scene.identity.runtimeIdentity is not { } owner || owner.domainId != m_identities.domainId ||
            scene.isDestroyed || !ReferenceEquals(m_identities.Get<GameScene>(owner), scene))
            throw new InvalidOperationException("Scene property serialization requires its current owning identity domain.");
        m_sceneIdentity = owner;
        if (aliases is not { Count: > 0 })
            return;
        m_aliases = new Dictionary<Guid, Guid>(aliases.Count);
        foreach ((Guid alias, Guid targetId) in aliases)
        {
            EngineObject? target = m_identities.Get<EngineObject>(targetId);
            if (target is null || !BelongsToScene(target))
                throw new InvalidOperationException($"Scene reference alias '{alias}' targets missing object '{targetId}'.");
            EngineObject? previous = m_identities.Get<EngineObject>(alias);
            if (previous is not null && BelongsToScene(previous) && !ReferenceEquals(previous, target))
                throw new InvalidOperationException($"Duplicate scene graph local identity '{alias}'.");
            m_aliases.Add(alias, targetId);
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// The current allocator must still own the Scene and the exact registered object in that Scene.
    /// </remarks>
    protected override bool TryGetSourceId(
        EngineObject engineObject,
        out Guid sourceId
    ) {
        sourceId = default;
        if (!HasCurrentOwner() || engineObject.identity.runtimeIdentity is not { } identity ||
            identity.domainId != m_identities.domainId || !ReferenceEquals(m_identities.Get<EngineObject>(identity), engineObject) ||
            !BelongsToScene(engineObject))
            return false;
        sourceId = engineObject.identity.persistentId;
        return true;
    }

    /// <inheritdoc />
    /// <remarks>
    /// Neutral aliases resolve to current Core identities; destroyed or foreign Scene objects are rejected.
    /// </remarks>
    protected override bool TryResolve(
        Guid sourceId,
        out EngineObject? engineObject
    ) {
        engineObject = null;
        if (!HasCurrentOwner())
            return false;
        Guid target = m_aliases is not null && m_aliases.TryGetValue(sourceId, out Guid alias) ? alias : sourceId;
        EngineObject? candidate = m_identities.Get<EngineObject>(target);
        if (candidate is null || !BelongsToScene(candidate))
            return false;
        engineObject = candidate;
        return true;
    }

    private bool HasCurrentOwner()
        => IdentityAllocator.hasCurrent && ReferenceEquals(IdentityAllocator.current, m_identities) &&
           !scene.isDestroyed && ReferenceEquals(m_identities.Get<GameScene>(m_sceneIdentity), scene);

    private bool BelongsToScene(EngineObject value)
        => !value.isDestroyed && value switch
        {
            GameObject gameObject => gameObject.isRuntimeValid && ReferenceEquals(gameObject.scene, scene),
            GameComponent component => component.ownerOrNull is { isRuntimeValid: true } owner && ReferenceEquals(owner.scene, scene),
            GameSystem system => ReferenceEquals(system.ownerScene, scene),
            _ => false
        };
}
