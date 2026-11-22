using System;
using System.Collections.Concurrent;
using System.Threading;
using Inno.Extensibility.Types;

namespace Inno.Core.Serialization;

internal sealed class ConverterRegistryLease : IDisposable
{
    private ConcurrentDictionary<SerializationTypeMetadata, (ISerializationMetadataSource source, bool eligible)>? m_revisionPlans;
    private ConverterRegistry.ConverterRegistrySnapshot? m_snapshot;

    internal ConverterRegistryLease(ConverterRegistry.ConverterRegistrySnapshot snapshot)
    {
        m_snapshot = snapshot;
    }

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public void Dispose()
    {
        ConverterRegistry.ConverterRegistrySnapshot? snapshot = Interlocked.Exchange(ref m_snapshot, null);
        m_revisionPlans?.Clear();
        snapshot?.Release();
    }

    internal TypeCacheSnapshot types => (m_snapshot ?? throw new ObjectDisposedException(nameof(ConverterRegistryLease))).types;
    internal Guid captureIdentity => (m_snapshot ?? throw new ObjectDisposedException(nameof(ConverterRegistryLease))).captureIdentity;

    internal bool CanReuseRevision(
        SerializationTypeMetadata metadata,
        ISerializationMetadataSource source
    ) {
        ConcurrentDictionary<SerializationTypeMetadata, (ISerializationMetadataSource source, bool eligible)>? plans = Volatile.Read(
            ref m_revisionPlans
        );
        if (plans is null)
        {
            var created = new ConcurrentDictionary<SerializationTypeMetadata, (ISerializationMetadataSource source, bool eligible)>();
            plans = Interlocked.CompareExchange(ref m_revisionPlans, created, null) ?? created;
        }
        if (plans.TryGetValue(metadata, out (ISerializationMetadataSource source, bool eligible) known)
            && ReferenceEquals(known.source, source)
        ) {
            return known.eligible;
        }
        bool eligible = !metadata.requiresConverter;
        foreach (SerializationMemberMetadata member in metadata.members)
        {
            if (!eligible)
            {
                break;
            }
            if ((member.visibility & PropertyVisibility.Serialize) == 0)
            {
                continue;
            }
            if (!IsKnownDefault(member.type))
            {
                eligible = false;
                break;
            }
            if (ValuePipeline.IsPrimitive(member.type) || member.type.IsEnum)
            {
                continue;
            }
            SerializationTypeMetadata nested = source.GetMetadata(member.type);
            if (!nested.hasPureFieldAccess)
            {
                eligible = false;
                break;
            }
            foreach (SerializationMemberMetadata field in nested.members)
            {
                if ((field.visibility & PropertyVisibility.Serialize) != 0 && !IsKnownDefault(field.type))
                {
                    eligible = false;
                    break;
                }
            }
        }
        // An unresolved fact can become known through an ordinary write in this same operation.
        // Cache only positive proofs; a key query still never invokes the resolver itself.
        if (eligible)
        {
            plans[metadata] = (source, true);
        }
        return eligible;
    }

    internal ConverterInvoker? Resolve(Type valueType)
    {
        ArgumentNullException.ThrowIfNull(valueType);
        ConverterRegistry.ConverterRegistrySnapshot snapshot = m_snapshot
                                                               ?? throw new ObjectDisposedException(nameof(ConverterRegistryLease));
        Type normalizedType = Nullable.GetUnderlyingType(valueType) ?? valueType;
        if (snapshot.cache.TryGetValue(normalizedType, out ConverterInvoker? cached))
        {
            return cached;
        }
        lock (snapshot.sync)
        {
            if (snapshot.cache.TryGetValue(normalizedType, out cached))
            {
                return cached;
            }
            ConverterInvoker? resolved = ConverterRegistry.ResolveSnapshot(snapshot, normalizedType);
            if (!snapshot.cache.TryAdd(normalizedType, resolved))
            {
                throw new InvalidOperationException("The serialization converter resolution was published twice.");
            }
            return resolved;
        }
    }

    private bool IsKnownDefault(Type type)
    {
        ConverterRegistry.ConverterRegistrySnapshot snapshot = m_snapshot
                                                               ?? throw new ObjectDisposedException(nameof(ConverterRegistryLease));
        Type normalized = Nullable.GetUnderlyingType(type) ?? type;
        // Reuse only facts established by the authoritative resolver. A key query must not
        // construct converters, change their execution order, or introduce another resolver.
        return snapshot.cache.TryGetValue(normalized, out ConverterInvoker? converter) && converter is null;
    }
}
