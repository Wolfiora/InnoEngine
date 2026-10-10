using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Inno.Core.Collections;

/// <summary>
/// Internal store validation surface for IndexedObjectKey.
/// </summary>
internal interface IIndexedObjectStore
{
    /// <summary>
    /// Determines whether the key belongs to a live slot in the current storage generation.
    /// </summary>
    /// <param name="id">
    /// The stable identity used to locate the requested value.
    /// </param>
    /// <param name="keyType">
    /// The key type consumed by is valid key; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    bool IsValidKey(
        int id,
        Type keyType
    );
}

/// <summary>
/// Thread-safe object store with optional query keys over stored items.
/// </summary>
/// <typeparam name="T">
/// The stored reference type.
/// </typeparam>
public sealed partial class IndexedObjectStore<T> : IIndexedObjectStore where T : class
{
    private readonly WeakReference<IIndexedObjectStore> m_storeRef;
    private readonly List<T> m_activeList = new();
    private readonly Dictionary<T, int> m_activeIndex = new(ReferenceEqualityComparer<T>.INSTANCE);
    private readonly Dictionary<T, IndexedObjectRuntimeHandle> m_handleByItem = new(ReferenceEqualityComparer<T>.INSTANCE);
    private readonly List<int> m_sparseToDense = new();
    private readonly List<uint> m_generations = new();
    private readonly List<int> m_denseToSlot = new();
    private Stack<int> m_freeSlots = new();
    private readonly Dictionary<int, IIndexedObjectIndex<T>> m_indexes = new();
    private readonly Dictionary<string, int> m_indexByName = new(StringComparer.Ordinal);
    private readonly ReaderWriterLockSlim m_lock = new(LockRecursionPolicy.NoRecursion);
    private int m_nextIndexId = 1;
    private int m_version;

    /// <summary>
    /// Creates an empty object store.
    /// </summary>
    public IndexedObjectStore()
    {
        m_storeRef = new WeakReference<IIndexedObjectStore>(this);
    }

    /// <summary>
    /// Number of stored items.
    /// </summary>
    public int count
    {
        get
        {
            m_lock.EnterReadLock();
            try
            {
                return m_activeList.Count;
            }
            finally
            {
                m_lock.ExitReadLock();
            }
        }
    }

    /// <summary>
    /// Clears all stored items and removes all defined keys.
    /// </summary>
    /// <remarks>
    /// After calling this, all key handles are invalid and must be redefined via <see cref="DefineKey{TKey}"/>.
    /// </remarks>
    public void Clear()
    {
        m_lock.EnterWriteLock();
        try
        {
            m_activeList.Clear();
            m_activeIndex.Clear();
            m_handleByItem.Clear();
            m_sparseToDense.Clear();
            m_generations.Clear();
            m_denseToSlot.Clear();
            m_freeSlots = new Stack<int>();
            m_indexes.Clear();
            m_indexByName.Clear();
            m_nextIndexId = 1;
            BumpVersion();
        }
        finally
        {
            m_lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Clears all stored items but keeps defined keys.
    /// </summary>
    /// <remarks>
    /// Keys remain valid but all indexed values are removed.
    /// </remarks>
    public void RemoveAll()
    {
        m_lock.EnterWriteLock();
        try
        {
            for (int i = 0; i < m_denseToSlot.Count; i++)
            {
                int slot = m_denseToSlot[i];
                m_sparseToDense[slot] = -1;
                m_generations[slot]++;
                m_freeSlots.Push(slot);
            }

            m_activeList.Clear();
            m_activeIndex.Clear();
            m_handleByItem.Clear();
            m_denseToSlot.Clear();
            foreach (var index in m_indexes.Values)
                index.Clear();
            BumpVersion();
        }
        finally
        {
            m_lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Adds an item to the store without indexing.
    /// </summary>
    /// <param name="item">
    /// The item to add.
    /// </param>
    /// <returns>
    /// The validated indexed object entryt that represents the completed operation.
    /// </returns>
    public IndexedObjectEntry<T> Add(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        m_lock.EnterWriteLock();
        try
        {
            if (!m_activeIndex.ContainsKey(item))
            {
                IndexedObjectRuntimeHandle handle = AllocateHandle();
                int denseIndex = m_activeList.Count;
                m_activeIndex[item] = denseIndex;
                m_handleByItem[item] = handle;
                m_activeList.Add(item);
                m_denseToSlot.Add(handle.slot);
                m_sparseToDense[handle.slot] = denseIndex;
                BumpVersion();
            }
        }
        finally
        {
            m_lock.ExitWriteLock();
        }

        return new IndexedObjectEntry<T>(this, item);
    }

    /// <summary>
    /// Removes an item from the store and all keys.
    /// </summary>
    /// <param name="item">
    /// The item to remove.
    /// </param>
    /// <returns>
    /// True if removed.
    /// </returns>
    public bool Remove(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        m_lock.EnterWriteLock();
        try
        {
            if (!m_activeIndex.TryGetValue(item, out var index))
                return false;

            foreach (var objectIndex in m_indexes.Values)
                objectIndex.Remove(item);

            var lastIndex = m_activeList.Count - 1;
            var lastItem = m_activeList[lastIndex];
            var removedSlot = m_denseToSlot[index];
            var lastSlot = m_denseToSlot[lastIndex];
            m_activeList.RemoveAt(lastIndex);
            m_denseToSlot.RemoveAt(lastIndex);
            m_activeIndex.Remove(item);
            m_handleByItem.Remove(item);

            if (index != lastIndex)
            {
                m_activeList[index] = lastItem;
                m_activeIndex[lastItem] = index;
                m_denseToSlot[index] = lastSlot;
                m_sparseToDense[lastSlot] = index;
            }

            ReleaseSlot(removedSlot);
            BumpVersion();
            return true;
        }
        finally
        {
            m_lock.ExitWriteLock();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void BumpVersion() => Interlocked.Increment(ref m_version);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureVersion(int expected)
    {
        if (Volatile.Read(ref m_version) != expected)
            throw new InvalidOperationException("Collection was modified during enumeration.");
    }

    internal sealed class ReferenceEqualityComparer<TItem> : IEqualityComparer<TItem> where TItem : class
    {
        /// <summary>
        /// The instance value used as part of this type's public representation.
        /// </summary>
        public static readonly ReferenceEqualityComparer<TItem> INSTANCE = new();

        /// <summary>
        /// Determines whether this value and the supplied value represent the same logical state.
        /// </summary>
        /// <param name="x">
        /// The horizontal or first component.
        /// </param>
        /// <param name="y">
        /// The vertical or second component.
        /// </param>
        /// <returns>
        /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(
            TItem? x,
            TItem? y
        ) => ReferenceEquals(x, y);

        /// <summary>
        /// Computes a hash code consistent with the implemented equality contract.
        /// </summary>
        /// <param name="obj">
        /// The object compared with this value.
        /// </param>
        /// <returns>
        /// The scalar result calculated from the supplied inputs.
        /// </returns>
        public int GetHashCode(TItem obj) => RuntimeHelpers.GetHashCode(obj);
    }
}
