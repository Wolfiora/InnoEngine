using System;

namespace Inno.Core.Identity;

/// <summary>
/// Identity payload for an object, carrying persistent id and runtime id.
/// </summary>
public struct Identity
{
    /// <summary>
    /// Gets the stable identifier preserved by serialization and runtime reconstruction.
    /// </summary>
    public Guid persistentID { get; private set; }

    /// <summary>
    /// Gets the identifier assigned by the currently bound runtime registry.
    /// </summary>
    /// <remarks>
    /// The value is <see langword="null"/> while this identity is not bound to a live registry.
    /// </remarks>
    public int? runtimeID
    {
        get
        {
            if (m_registryRef is null || !m_registryRef.TryGetTarget(out IdentityRegistry? registry))
            {
                return null;
            }

            return registry.TryGetRuntimeID(this, out int runtimeID) ? runtimeID : null;
        }
    }

    /// <summary>
    /// Gets the domain-qualified runtime identity while this object is registered.
    /// </summary>
    /// <remarks>
    /// The value is <see langword="null"/> when this identity is detached or its registry is no longer reachable.
    /// </remarks>
    public RuntimeIdentity? runtimeIdentity
    {
        get
        {
            if (m_registryRef is null || !m_registryRef.TryGetTarget(out IdentityRegistry? registry))
            {
                return null;
            }
            return registry.TryGetRuntimeID(this, out int runtimeID)
                   ? new RuntimeIdentity(registry.domainID, runtimeID)
                   : null;
        }
    }

    private int m_runtimeID;
    private WeakReference<IdentityRegistry>? m_registryRef;

    /// <summary>
    /// Creates an unbound identity with the supplied persistent identifier.
    /// </summary>
    /// <param name="persistentID">
    /// The stable identifier to preserve across serialization boundaries.
    /// </param>
    public Identity(Guid persistentID)
    {
        this.persistentID = persistentID;
        m_runtimeID = 0;
        m_registryRef = null;
    }

    internal void Bind(
        IdentityRegistry registry,
        int runtimeID
    ) {
        if (persistentID == Guid.Empty)
        {
            persistentID = Guid.NewGuid();
        }

        m_runtimeID = runtimeID;
        m_registryRef = new WeakReference<IdentityRegistry>(registry);
    }

    internal void Unbind(IdentityRegistry registry)
    {
        if (m_registryRef is not null
            && m_registryRef.TryGetTarget(out IdentityRegistry? current)
            && ReferenceEquals(current, registry)
        ) {
            m_runtimeID = 0;
            m_registryRef = null;
        }
    }

    internal int rawRuntimeID => m_runtimeID;

    /// <summary>
    /// Resolves this transient snapshot through its original weak identity registry.
    /// </summary>
    /// <typeparam name="TObject">
    /// The expected identity object type.
    /// </typeparam>
    /// <returns>
    /// The current object, or null if the registry retired, the slot changed generation, or the type differs.
    /// </returns>
    public TObject? Resolve<TObject>()
        where TObject : IdentityObject
            => m_registryRef is not null && m_registryRef.TryGetTarget(out IdentityRegistry? registry)
               ? registry.Get<TObject>(m_runtimeID)
               : null;
}
