using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Inno.Core.Identity;

/// <summary>
/// Concrete identity registry that manages persistent id and runtime id mappings.
/// </summary>
internal sealed class IdentityRegistry
{
    private readonly ReaderWriterLockSlim m_lock = new(LockRecursionPolicy.NoRecursion);

    private readonly List<RegistryEntry> m_active = [];

    private readonly ConditionalWeakTable<object, RegistrySlot> m_slotByObject = new();

    private readonly List<int> m_denseToSlot = [];
    private readonly List<int> m_sparseToDense = [];
    private readonly List<int> m_generations = [];
    private readonly Stack<int> m_freeSlots = new();

    private readonly Dictionary<Guid, int> m_slotByPersistent = [];

    internal IdentityRegistry(IdentityDomainID domainID)
    {
        this.domainID = domainID;
    }

    /// <summary>
    /// Gets the number of values in the current immutable view.
    /// </summary>
    public int count
    {
        get
        {
            m_lock.EnterReadLock();
            try
            {
                return m_active.Count;
            }
            finally
            {
                m_lock.ExitReadLock();
            }
        }
    }

    /// <summary>
    /// Registers an object and binds a runtime id, with optional persistent id override.
    /// </summary>
    /// <param name="obj">
    /// Identity object to register.
    /// </param>
    /// <param name="persistentID">
    /// Preferred persistent id. When null, uses object's current identity persistent id.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when registered; <see langword="false"/> when already registered.
    /// </returns>
    public bool Register(
        IdentityObject obj,
        Guid? persistentID = null
    ) {
        ArgumentNullException.ThrowIfNull(obj);

        m_lock.EnterWriteLock();
        try
        {
            Identity identity = obj.identity;
            Guid resolvedPersistentID = persistentID.GetValueOrDefault(identity.persistentID);
            if (resolvedPersistentID == Guid.Empty)
            {
                resolvedPersistentID = Guid.NewGuid();
            }

            if (TryGetSlotByObjectNoLock(obj, out int objectSlot))
            {
                if (TryGetLiveObjectBySlotNoLock(objectSlot, out _, out _))
                {
                    return false;
                }

                RemoveSlotByObjectNoLock(obj, objectSlot);
            }

            if (TryGetSlotByPersistentNoLock(resolvedPersistentID, out int existingSlot))
            {
                if (TryGetLiveObjectBySlotNoLock(existingSlot, out IdentityObject? existing, out _)
                    && !ReferenceEquals(existing, obj)
                ) {
                    throw new InvalidOperationException(
                        $"Persistent id '{resolvedPersistentID}' is already registered."
                    );
                }

                RemoveSlotBySlotNoLock(existingSlot);
            }

            int slot = AllocateSlot();
            int generation = m_generations[slot];
            int runtimeID = RuntimeIDCodec.Pack(slot, generation);

            int denseIndex = m_active.Count;
            m_active.Add(new RegistryEntry(obj, resolvedPersistentID));
            m_slotByObject.Remove(obj);
            m_slotByObject.Add(obj, new RegistrySlot(slot));
            m_denseToSlot.Add(slot);
            m_sparseToDense[slot] = denseIndex;

            m_slotByPersistent[resolvedPersistentID] = slot;

            identity = new Identity(resolvedPersistentID);
            identity.Bind(this, runtimeID);
            obj.SetIdentity(identity);
            return true;
        }
        finally
        {
            m_lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Removes the supplied value from the active generation.
    /// </summary>
    /// <param name="obj">
    /// The object compared with this value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Unregister(IdentityObject obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        m_lock.EnterWriteLock();
        try
        {
            if (!TryGetSlotByObjectNoLock(obj, out int objectSlot))
            {
                return false;
            }

            if (!TryGetLiveEntryBySlotNoLock(objectSlot, out RegistryEntry? entry, out int denseIndex))
            {
                m_slotByObject.Remove(obj);
                return false;
            }

            Identity identity = obj.identity;
            if (!TryGetLiveObject(entry!, out IdentityObject? activeObject))
            {
                m_slotByObject.Remove(obj);
                RemoveSlotBySlotNoLock(objectSlot);
                return false;
            }

            if (!ReferenceEquals(activeObject, obj))
            {
                return false;
            }

            RemoveSlotBySlotNoLock(objectSlot);
            identity.Unbind(this);
            obj.SetIdentity(identity);
            return true;
        }
        finally
        {
            m_lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Retrieves the requested value value from current authoritative state.
    /// </summary>
    /// <typeparam name="TIdentity">
    /// The caller-selected tidentity type whose declared constraints are enforced by this operation.
    /// </typeparam>
    /// <param name="runtimeID">
    /// The runtime id consumed by get; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// The validated tidentity? that represents the completed operation.
    /// </returns>
    public TIdentity? Get<TIdentity>(int runtimeID)
        where TIdentity : IdentityObject
    {
        int slot = -1;
        bool shouldCleanup = false;
        TIdentity? result = null;
        m_lock.EnterReadLock();
        try
        {
            if (!TryResolveDenseIndexNoLock(runtimeID, out int denseIndex))
            {
                return null;
            }

            if (!TryGetEntryByDenseNoLock(denseIndex, out RegistryEntry? entry))
            {
                return null;
            }

            if (!entry!.TryGetObject(out IdentityObject? obj))
            {
                slot = m_denseToSlot[denseIndex];
                shouldCleanup = true;
            }
            else if (obj is TIdentity typed)
            {
                result = typed;
            }
        }
        finally
        {
            m_lock.ExitReadLock();
        }

        if (shouldCleanup && slot >= 0)
        {
            m_lock.EnterWriteLock();
            try
            {
                CleanupStaleSlotBySlotNoLock(slot);
            }
            finally
            {
                m_lock.ExitWriteLock();
            }
        }

        return result;
    }

    /// <summary>
    /// Retrieves the requested value value from current authoritative state.
    /// </summary>
    /// <typeparam name="TIdentity">
    /// The caller-selected tidentity type whose declared constraints are enforced by this operation.
    /// </typeparam>
    /// <param name="persistentID">
    /// The stable persistent identity used for lookup.
    /// </param>
    /// <returns>
    /// The validated tidentity? that represents the completed operation.
    /// </returns>
    public TIdentity? Get<TIdentity>(Guid persistentID)
        where TIdentity : IdentityObject
    {
        if (persistentID == Guid.Empty)
        {
            return null;
        }

        int slot = -1;
        bool shouldCleanup = false;
        TIdentity? result = null;
        m_lock.EnterReadLock();
        try
        {
            if (!m_slotByPersistent.TryGetValue(persistentID, out slot))
            {
                return null;
            }

            if (!TryGetDenseIndexBySlotNoLock(slot, out int denseIndex))
            {
                return null;
            }

            if (!TryGetEntryByDenseNoLock(denseIndex, out RegistryEntry? entry))
            {
                return null;
            }

            if (!entry!.TryGetObject(out IdentityObject? obj))
            {
                slot = m_denseToSlot[denseIndex];
                shouldCleanup = true;
            }
            else if (obj is TIdentity typed)
            {
                result = typed;
            }
        }
        finally
        {
            m_lock.ExitReadLock();
        }

        if (shouldCleanup && slot >= 0)
        {
            m_lock.EnterWriteLock();
            try
            {
                CleanupStaleSlotBySlotNoLock(slot);
            }
            finally
            {
                m_lock.ExitWriteLock();
            }
        }
        return result;
    }

    internal IdentityDomainID domainID { get; }

    internal bool TryGetRuntimeID(
        in Identity identity,
        out int runtimeID
    ) {
        runtimeID = 0;
        if (identity.persistentID == Guid.Empty || identity.rawRuntimeID == 0)
        {
            return false;
        }

        m_lock.EnterReadLock();
        try
        {
            if (!TryGetDenseIndexByRuntimeIDNoLock(identity.rawRuntimeID, out int denseIndex))
            {
                return false;
            }

            if (!TryGetEntryByDenseNoLock(denseIndex, out RegistryEntry? entry))
            {
                return false;
            }

            if (entry!.persistentID != identity.persistentID)
            {
                return false;
            }

            runtimeID = identity.rawRuntimeID;
            return true;
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    private int AllocateSlot()
    {
        if (m_freeSlots.Count > 0)
        {
            return m_freeSlots.Pop();
        }

        int slot = m_generations.Count;
        m_generations.Add(1);
        m_sparseToDense.Add(-1);
        return slot;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryResolveDenseIndexNoLock(
        int runtimeID,
        out int denseIndex
    ) {
        int slot = RuntimeIDCodec.UnpackSlot(runtimeID);
        int generation = RuntimeIDCodec.UnpackGeneration(runtimeID);
        if ((uint)slot >= (uint)m_generations.Count)
        {
            denseIndex = -1;
            return false;
        }

        if (m_generations[slot] != generation)
        {
            denseIndex = -1;
            return false;
        }

        denseIndex = m_sparseToDense[slot];
        return denseIndex >= 0 && denseIndex < m_active.Count;
    }

    private bool TryGetDenseIndexBySlotNoLock(
        int slot,
        out int denseIndex
    ) {
        if ((uint)slot >= (uint)m_sparseToDense.Count)
        {
            denseIndex = -1;
            return false;
        }

        denseIndex = m_sparseToDense[slot];
        return denseIndex >= 0 && denseIndex < m_active.Count;
    }

    private bool TryGetDenseIndexByRuntimeIDNoLock(
        int runtimeID,
        out int denseIndex
    ) => TryResolveDenseIndexNoLock(runtimeID, out denseIndex) && TryGetEntryByDenseNoLock(denseIndex, out _);

    private bool TryGetEntryByDenseNoLock(
        int denseIndex,
        out RegistryEntry? entry
    ) {
        if (denseIndex < 0 || denseIndex >= m_active.Count)
        {
            entry = null;
            return false;
        }

        entry = m_active[denseIndex];
        return entry.TryGetObject(out _);
    }

    private bool TryGetLiveObjectByDenseNoLock(
        int denseIndex,
        out IdentityObject? obj
    ) {
        if (!TryGetEntryByDenseNoLock(denseIndex, out RegistryEntry? entry))
        {
            obj = null;
            return false;
        }

        return entry!.TryGetObject(out obj);
    }

    private bool TryGetLiveObjectBySlotNoLock(
        int slot,
        out IdentityObject? obj,
        out RegistryEntry? entry
    ) {
        if (!TryGetDenseIndexBySlotNoLock(slot, out int denseIndex))
        {
            obj = null;
            entry = null;
            return false;
        }

        if (!TryGetEntryByDenseNoLock(denseIndex, out entry))
        {
            obj = null;
            return false;
        }

        return entry!.TryGetObject(out obj);
    }

    private bool TryGetLiveEntryBySlotNoLock(
        int slot,
        out RegistryEntry? entry,
        out int denseIndex
    ) {
        if (!TryGetDenseIndexBySlotNoLock(slot, out denseIndex))
        {
            entry = null;
            return false;
        }

        return TryGetEntryByDenseNoLock(denseIndex, out entry);
    }

    private bool TryGetSlotByObjectNoLock(
        IdentityObject obj,
        out int slot
    ) {
        if (m_slotByObject.TryGetValue(obj, out RegistrySlot? slotInfo))
        {
            slot = slotInfo.slot;
            return true;
        }

        slot = 0;
        return false;
    }

    private bool TryGetSlotByPersistentNoLock(
        Guid persistentID,
        out int slot
    ) {
        if (!m_slotByPersistent.TryGetValue(persistentID, out slot))
        {
            return false;
        }

        if (TryGetLiveObjectBySlotNoLock(slot, out _, out _))
        {
            return true;
        }

        RemoveSlotBySlotNoLock(slot);
        return false;
    }

    private bool TryGetLiveObject(
        RegistryEntry entry,
        out IdentityObject? obj
    ) => entry.TryGetObject(out obj);

    private void CleanupStaleSlotBySlotNoLock(int slot)
    {
        if (!TryGetDenseIndexBySlotNoLock(slot, out int denseIndex))
        {
            m_slotByPersistent.Remove(GetPersistentIDBySlotNoLock(slot));
            m_sparseToDense[slot] = -1;
            return;
        }

        if (TryGetEntryByDenseNoLock(denseIndex, out RegistryEntry? entry)
            && entry!.TryGetObject(out _)
        ) {
            return;
        }

        RemoveSlotBySlotNoLock(slot, denseIndex);
    }

    private Guid GetPersistentIDBySlotNoLock(int slot)
    {
        if (slot < 0 || slot >= m_sparseToDense.Count)
        {
            return Guid.Empty;
        }

        int denseIndex = m_sparseToDense[slot];
        if (denseIndex < 0 || denseIndex >= m_active.Count)
        {
            return Guid.Empty;
        }

        return m_active[denseIndex].persistentID;
    }

    private void RemoveSlotByObjectNoLock(
        IdentityObject obj,
        int objectSlot
    ) {
        m_slotByObject.Remove(obj);
        if (TryGetDenseIndexBySlotNoLock(objectSlot, out int denseIndex))
        {
            RemoveSlotBySlotNoLock(objectSlot, denseIndex);
        }
    }

    private void RemoveSlotBySlotNoLock(int slot)
    {
        if (!TryGetDenseIndexBySlotNoLock(slot, out int denseIndex))
        {
            return;
        }

        RemoveSlotBySlotNoLock(slot, denseIndex);
    }

    private void RemoveSlotBySlotNoLock(
        int slot,
        int denseIndex
    ) {
        RegistryEntry removed = m_active[denseIndex];
        RegistryEntry lastEntry = m_active[^1];
        int lastIndex = m_active.Count - 1;
        int lastSlot = m_denseToSlot[lastIndex];

        if (removed.TryGetObject(out IdentityObject? removedObj))
        {
            m_slotByObject.Remove(removedObj!);
        }

        if (m_slotByPersistent.TryGetValue(removed.persistentID, out int mappedSlot)
            && mappedSlot == slot
        ) {
            m_slotByPersistent.Remove(removed.persistentID);
        }

        m_active.RemoveAt(lastIndex);
        m_denseToSlot.RemoveAt(lastIndex);

        m_sparseToDense[slot] = -1;
        int nextGeneration = m_generations[slot] + 1;
        if (nextGeneration > RuntimeIDCodec.C_GENERATION_MASK)
        {
            nextGeneration = 1;
        }

        m_generations[slot] = nextGeneration;
        m_freeSlots.Push(slot);

        if (denseIndex == lastIndex)
        {
            return;
        }

        m_active[denseIndex] = lastEntry;
        m_denseToSlot[denseIndex] = lastSlot;
        m_sparseToDense[lastSlot] = denseIndex;
        if (m_active[denseIndex].TryGetObject(out IdentityObject? movedObj))
        {
            if (m_slotByObject.TryGetValue(movedObj!, out RegistrySlot? movedSlot))
            {
                movedSlot.slot = lastSlot;
            }
        }
    }

    private sealed class RegistryEntry
    {
        /// <summary>
        /// The m object ref value used as part of this type's public representation.
        /// </summary>
        public WeakReference<IdentityObject> objectRef;

        /// <summary>
        /// The persistent id value used as part of this type's public representation.
        /// </summary>
        public Guid persistentID;

        /// <summary>
        /// Creates a validated registry entry instance.
        /// </summary>
        /// <param name="obj">
        /// The object compared with this value.
        /// </param>
        /// <param name="persistentID">
        /// The stable persistent identity used for lookup.
        /// </param>
        public RegistryEntry(
            IdentityObject obj,
            Guid persistentID
        ) {
            this.objectRef = new WeakReference<IdentityObject>(obj);
            this.persistentID = persistentID;
        }

        /// <summary>
        /// Attempts to get object without changing state when the operation cannot complete.
        /// </summary>
        /// <param name="obj">
        /// The object compared with this value.
        /// </param>
        /// <returns>
        /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryGetObject(out IdentityObject? obj) => objectRef.TryGetTarget(out obj);
    }

    private sealed class RegistrySlot
    {
        /// <summary>
        /// The slot value used as part of this type's public representation.
        /// </summary>
        public int slot;

        /// <summary>
        /// Creates a validated registry slot instance.
        /// </summary>
        /// <param name="slot">
        /// The dense storage slot encoded in this runtime handle.
        /// </param>
        public RegistrySlot(int slot)
        {
            this.slot = slot;
        }
    }
}
