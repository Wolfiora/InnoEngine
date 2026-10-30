using System;
using System.Collections;
using System.Collections.Generic;

namespace Inno.Scene;

/// <summary>
/// Combines frozen structural identities with systems borrowed only by the current graph write.
/// </summary>
internal sealed class SceneGraphSourceIds : IReadOnlyDictionary<EngineObject, Guid>
{
    private readonly IReadOnlyDictionary<EngineObject, Guid> m_objects;
    private readonly Dictionary<EngineObject, Guid>? m_systems;

    internal SceneGraphSourceIds(
        IReadOnlyDictionary<EngineObject, Guid> objects,
        IReadOnlyList<GameSystem> systems
    ) {
        m_objects = objects;
        // Systems have a separate lifecycle owner; the structural cache must not retain them.
        if (systems.Count == 0)
            return;
        m_systems = new Dictionary<EngineObject, Guid>(systems.Count, ReferenceEqualityComparer.Instance);
        for (int index = 0; index < systems.Count; index++)
        {
            GameSystem system = systems[index];
            m_systems.Add(system, system.identity.persistentId);
        }
    }

    /// <inheritdoc />
    public int Count => checked(m_objects.Count + (m_systems?.Count ?? 0));

    /// <inheritdoc />
    public IEnumerable<EngineObject> Keys => EnumerateKeys();

    /// <inheritdoc />
    public IEnumerable<Guid> Values => EnumerateValues();

    /// <inheritdoc />
    public Guid this[EngineObject key] => TryGetValue(key, out Guid value) ? value : throw new KeyNotFoundException();

    /// <inheritdoc />
    public bool ContainsKey(EngineObject key) => TryGetValue(key, out _);

    /// <inheritdoc />
    public bool TryGetValue(
        EngineObject key,
        out Guid value
    ) {
        if (m_objects.TryGetValue(key, out value))
            return true;
        if (m_systems is not null)
            return m_systems.TryGetValue(key, out value);
        value = default;
        return false;
    }

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<EngineObject, Guid>> GetEnumerator()
    {
        foreach (KeyValuePair<EngineObject, Guid> entry in m_objects)
            yield return entry;
        if (m_systems is not null)
            foreach (KeyValuePair<EngineObject, Guid> entry in m_systems)
                yield return entry;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private IEnumerable<EngineObject> EnumerateKeys()
    {
        foreach (KeyValuePair<EngineObject, Guid> entry in this)
            yield return entry.Key;
    }

    private IEnumerable<Guid> EnumerateValues()
    {
        foreach (KeyValuePair<EngineObject, Guid> entry in this)
            yield return entry.Value;
    }
}
