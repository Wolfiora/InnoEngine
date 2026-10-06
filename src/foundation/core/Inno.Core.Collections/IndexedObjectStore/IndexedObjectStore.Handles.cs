using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Inno.Core.Collections;

sealed partial class IndexedObjectStore<T>
where T : class
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool IsValidItem(T item)
    {
        if (item == null)
            return false;

        m_lock.EnterReadLock();
        try
        {
            return m_activeIndex.ContainsKey(item);
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Tries to get the runtime handle for an item currently stored in the store.
    /// </summary>
    /// <param name="item">
    /// Item to resolve.
    /// </param>
    /// <param name="handle">
    /// Resolved runtime handle when successful.
    /// </param>
    /// <returns>
    /// True when the item is currently stored in this store.
    /// </returns>
    internal bool TryGetHandle(
        T item,
        out IndexedObjectRuntimeHandle handle
    ) {
        if (item == null)
        {
            handle = default;
            return false;
        }

        m_lock.EnterReadLock();
        try
        {
            return m_handleByItem.TryGetValue(item, out handle);
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Returns true when the provided runtime handle still points to a live item in this store.
    /// </summary>
    /// <param name="handle">
    /// The opaque handle validated by this operation.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    internal bool IsHandleValid(IndexedObjectRuntimeHandle handle)
    {
        m_lock.EnterReadLock();
        try
        {
            return IsHandleValidNoLock(handle);
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Tries to resolve an item by runtime handle.
    /// </summary>
    /// <param name="handle">
    /// The opaque handle validated by this operation.
    /// </param>
    /// <param name="item">
    /// The stored item associated with the validated handle.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    internal bool TryGetByHandle(
        IndexedObjectRuntimeHandle handle,
        out T? item
    ) {
        m_lock.EnterReadLock();
        try
        {
            if (!IsHandleValidNoLock(handle))
            {
                item = null;
                return false;
            }

            int denseIndex = m_sparseToDense[handle.slot];
            item = m_activeList[denseIndex];
            return true;
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IndexedObjectRuntimeHandle AllocateHandle()
    {
        int slot;
        if (m_freeSlots.Count > 0)
        {
            slot = m_freeSlots.Pop();
        }
        else
        {
            slot = m_generations.Count;
            m_generations.Add(1);
            m_sparseToDense.Add(-1);
        }

        return new IndexedObjectRuntimeHandle(slot, m_generations[slot]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ReleaseSlot(int slot)
    {
        m_sparseToDense[slot] = -1;
        m_generations[slot]++;
        m_freeSlots.Push(slot);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsHandleValidNoLock(IndexedObjectRuntimeHandle handle)
    {
        int slot = handle.slot;
        if ((uint)slot >= (uint)m_generations.Count)
            return false;

        if (m_generations[slot] != handle.generation)
            return false;

        int denseIndex = m_sparseToDense[slot];
        return denseIndex >= 0 && denseIndex < m_activeList.Count;
    }

}
