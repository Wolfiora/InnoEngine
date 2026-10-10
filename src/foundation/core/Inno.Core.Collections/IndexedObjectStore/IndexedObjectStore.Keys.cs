using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Inno.Core.Collections;

sealed partial class IndexedObjectStore<T>
where T : class
{
    /// <summary>
    /// Defines a query key over stored items.
    /// </summary>
    /// <typeparam name="TKey">
    /// Key type.
    /// </typeparam>
    /// <param name="name">
    /// Unique key name.
    /// </param>
    /// <param name="flags">
    /// Key behavior flags.
    /// </param>
    /// <param name="orderComparer">
    /// Optional order comparer. Required when <see cref="IndexedObjectKeyFlags.Ordered"/> is set.
    /// </param>
    /// <returns>
    /// The created key handle.
    /// </returns>
    public IndexedObjectKey<TKey> DefineKey<TKey>(
        string name,
        IndexedObjectKeyFlags flags = IndexedObjectKeyFlags.Unordered,
        IComparer<TKey>? orderComparer = null
    ) where TKey : notnull
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Key name is required.", nameof(name));

        m_lock.EnterWriteLock();
        try
        {
            if (m_indexByName.TryGetValue(name, out _))
                throw new InvalidOperationException($"Key '{name}' already exists.");

            if ((flags & IndexedObjectKeyFlags.Ordered) != 0 && orderComparer == null)
                throw new ArgumentNullException(nameof(orderComparer), $"{nameof(orderComparer)} cannot be null when {nameof(flags)} is set to {nameof(IndexedObjectKeyFlags.Ordered)}.)");

            var index = new IndexedObjectIndex<T, TKey>(name, flags, orderComparer);
            var id = m_nextIndexId++;
            m_indexes[id] = index;
            m_indexByName[name] = id;
            BumpVersion();

            return new IndexedObjectKey<TKey>(m_storeRef, id, name);
        }
        finally
        {
            m_lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Removes a key previously defined on the store.
    /// </summary>
    /// <typeparam name="TKey">
    /// Key type.
    /// </typeparam>
    /// <param name="key">
    /// The key handle to remove.
    /// </param>
    /// <returns>
    /// True if removed.
    /// </returns>
    public bool RemoveKey<TKey>(IndexedObjectKey<TKey> key)
    {
        m_lock.EnterWriteLock();
        try
        {
            if (!m_indexes.Remove(key.id, out var removed))
                return false;

            m_indexByName.Remove(removed.name);
            BumpVersion();
            return true;
        }
        finally
        {
            m_lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Gets a previously defined key by name.
    /// </summary>
    /// <typeparam name="TKey">
    /// Key type.
    /// </typeparam>
    /// <param name="name">
    /// Key name.
    /// </param>
    /// <param name="key">
    /// The resolved key handle.
    /// </param>
    /// <returns>
    /// True if found and type matches.
    /// </returns>
    public bool TryGetKey<TKey>(
        string name,
        out IndexedObjectKey<TKey> key
    ) where TKey : notnull
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Key name is required.", nameof(name));

        m_lock.EnterReadLock();
        try
        {
            if (!m_indexByName.TryGetValue(name, out var id))
            {
                key = default;
                return false;
            }

            if (!m_indexes.TryGetValue(id, out var index) || index.keyType != typeof(TKey))
            {
                key = default;
                return false;
            }

            key = new IndexedObjectKey<TKey>(m_storeRef, id, name);
            return true;
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    bool IIndexedObjectStore.IsValidKey(
        int id,
        Type keyType
    ) {
        m_lock.EnterReadLock();
        try
        {
            return m_indexes.TryGetValue(id, out var index) && index.keyType == keyType;
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Gets all defined key names as a lazy enumerable.
    /// </summary>
    /// <remarks>
    /// Enumeration is fail-fast and throws if the store is modified during iteration.
    /// </remarks>
    /// <returns>
    /// Lazy enumerable of key names.
    /// </returns>
    public IEnumerable<string> GetAllKeys() => EnumerateKeys();

    private IEnumerable<string> EnumerateKeys()
    {
        var version = Volatile.Read(ref m_version);
        foreach (var name in m_indexByName.Keys)
        {
            EnsureVersion(version);
            yield return name;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool IsOrderedKey<TKey>(IndexedObjectKey<TKey> key) where TKey : notnull
        => (GetIndex(key).flags & IndexedObjectKeyFlags.Ordered) != 0;

    private IndexedObjectIndex<T, TKey> GetIndex<TKey>(IndexedObjectKey<TKey> handle) where TKey : notnull
    {
        if (handle.storeRef == null || !handle.storeRef.TryGetTarget(out var owner) || !ReferenceEquals(owner, this))
            throw new InvalidOperationException($"Key '{handle.name}' does not belong to this store.");

        if (!m_indexes.TryGetValue(handle.id, out var index))
            throw new InvalidOperationException($"Key '{handle.name}' not found.");

        if (index.keyType != typeof(TKey))
            throw new InvalidOperationException($"Key '{handle.name}' type mismatch.");

        return (IndexedObjectIndex<T, TKey>)index;
    }

    internal void SetKey<TKey>(
        T item,
        IndexedObjectKey<TKey> key,
        TKey value
    ) where TKey : notnull
    {
        m_lock.EnterWriteLock();
        try
        {
            if (!m_activeIndex.ContainsKey(item))
                throw new InvalidOperationException("Item is not in the store.");

            GetIndex(key).AddOrUpdate(item, value);
            BumpVersion();
        }
        finally
        {
            m_lock.ExitWriteLock();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal int GetCountUnsafe<TKey>(
        IndexedObjectKey<TKey> key,
        TKey value
    ) where TKey : notnull => GetIndex(key).GetCount(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool TryGetSingleUnsafe<TKey>(
        IndexedObjectKey<TKey> key,
        TKey value,
        out T? item
    ) where TKey : notnull => GetIndex(key).TryGetSingle(value, out item);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal HashSet<T>? GetSetUnsafe<TKey>(
        IndexedObjectKey<TKey> key,
        TKey value
    ) where TKey : notnull => GetIndex(key).FindUnsafe(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool ContainsInSet<TKey>(
        IndexedObjectKey<TKey> key,
        TKey value,
        T item
    ) where TKey : notnull => GetIndex(key).Contains(value, item);

}
