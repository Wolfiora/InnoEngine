using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Inno.Core.Collections;

public sealed partial class IndexedObjectStore<T>
    where T : class
{
    /// <summary>
    /// Finds items by key and returns a lazy fail-fast enumerable.
    /// </summary>
    /// <typeparam name="TKey">
    /// Key type.
    /// </typeparam>
    /// <param name="key">
    /// The key handle to query.
    /// </param>
    /// <param name="value">
    /// The key value to look up.
    /// </param>
    /// <returns>
    /// Lazy fail-fast enumerable of matching items.
    /// </returns>
    /// <remarks>
    /// Enumeration throws if the store is modified during iteration.
    /// </remarks>
    public IEnumerable<T> FindFast<TKey>(
        IndexedObjectKey<TKey> key,
        TKey value
    ) where TKey : notnull
    {
        IndexedObjectIndex<T, TKey> index = GetIndex(key);
        return EnumerateFind(index, value);
    }

    /// <summary>
    /// Finds items by key and returns a stable snapshot.
    /// </summary>
    /// <typeparam name="TKey">
    /// Key type.
    /// </typeparam>
    /// <param name="key">
    /// The key handle to query.
    /// </param>
    /// <param name="value">
    /// The key value to look up.
    /// </param>
    /// <returns>
    /// A stable snapshot list of matching items.
    /// </returns>
    /// <remarks>
    /// The returned list is detached from subsequent store mutations.
    /// </remarks>
    public IReadOnlyList<T> Find<TKey>(
        IndexedObjectKey<TKey> key,
        TKey value
    ) where TKey : notnull
    {
        m_lock.EnterReadLock();
        try
        {
            IndexedObjectIndex<T, TKey> index = GetIndex(key);
            return BuildFindSnapshot(index, value);
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Returns the first item by key or null if none exists.
    /// </summary>
    /// <typeparam name="TKey">
    /// Key type.
    /// </typeparam>
    /// <param name="key">
    /// The key handle to query.
    /// </param>
    /// <param name="value">
    /// The key value to look up.
    /// </param>
    /// <returns>
    /// The first matching item or null.
    /// </returns>
    public T? First<TKey>(
        IndexedObjectKey<TKey> key,
        TKey value
    ) where TKey : notnull
    {
        m_lock.EnterReadLock();
        try
        {
            if (GetIndex(key).TryGetSingle(value, out T? item))
            {
                return item;
            }
            return null;
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Returns all stored items as a stable snapshot.
    /// </summary>
    /// <returns>
    /// A snapshot list of all stored items.
    /// </returns>
    /// <remarks>
    /// The returned list is detached from subsequent store mutations.
    /// </remarks>
    public IReadOnlyList<T> All()
    {
        m_lock.EnterReadLock();
        try
        {
            var result = new List<T>(m_activeList.Count);
            result.AddRange(m_activeList);
            return result;
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Returns all stored items as a lazy fail-fast enumerable.
    /// </summary>
    /// <returns>
    /// Lazy fail-fast enumerable of all stored items.
    /// </returns>
    /// <remarks>
    /// Enumeration throws if the store is modified during iteration.
    /// </remarks>
    public IEnumerable<T> AllFast()
    {
        int version = Volatile.Read(ref m_version);
        foreach (T item in m_activeList)
        {
            EnsureVersion(version);
            yield return item;
        }
    }

    /// <summary>
    /// Starts a query over stored items.
    /// </summary>
    /// <returns>
    /// A query builder.
    /// </returns>
    public IndexedObjectQuery<T> Query() => new(this);

    internal IEnumerable<T> ExecuteQueryFast(List<IIndexedObjectQueryCondition<T>> conditions) => EnumerateQuery(conditions);

    internal IReadOnlyList<T> ExecuteQuerySnapshot(List<IIndexedObjectQueryCondition<T>> conditions)
    {
        m_lock.EnterReadLock();
        try
        {
            List<T> result = [];
            if (conditions.Count == 0)
            {
                result.AddRange(m_activeList);
                return result;
            }

            int seedIndex = 0;
            int seedCount = conditions[0].GetCandidateCount(this);
            for (int i = 1; i < conditions.Count; i++)
            {
                int candidateCount = conditions[i].GetCandidateCount(this);
                if (candidateCount < seedCount)
                {
                    seedCount = candidateCount;
                    seedIndex = i;
                }
            }

            if (seedCount == 0)
            {
                return result;
            }

            if (seedCount == 1 && conditions[seedIndex].TryGetSingle(this, out T single))
            {
                bool ok = true;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Validate(this, single))
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok)
                {
                    result.Add(single);
                }

                return result;
            }

            HashSet<T>? seed = conditions[seedIndex].GetSet(this);
            if (seed is null)
            {
                foreach (T item in m_activeList)
                {
                    bool ok = true;
                    for (int i = 0; i < conditions.Count; i++)
                    {
                        if (!conditions[i].Validate(this, item))
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                    {
                        result.Add(item);
                    }
                }

                return result;
            }

            foreach (T item in seed)
            {
                bool ok = true;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Validate(this, item))
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok)
                {
                    result.Add(item);
                }
            }

            return result;
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    internal T? ExecuteFirst(List<IIndexedObjectQueryCondition<T>> conditions)
    {
        m_lock.EnterReadLock();
        try
        {
            if (conditions.Count == 0)
            {
                return m_activeList.Count > 0 ? m_activeList[0] : null;
            }

            int seedIndex = 0;
            int seedCount = conditions[0].GetCandidateCount(this);

            for (int i = 1; i < conditions.Count; i++)
            {
                int candidateCount = conditions[i].GetCandidateCount(this);
                if (candidateCount < seedCount)
                {
                    seedCount = candidateCount;
                    seedIndex = i;
                }
            }

            if (seedCount == 0)
            {
                return null;
            }

            if (seedCount == 1 && conditions[seedIndex].TryGetSingle(this, out T single))
            {
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Validate(this, single))
                    {
                        return null;
                    }
                }

                return single;
            }

            HashSet<T>? seed = conditions[seedIndex].GetSet(this);
            if (seed is null)
            {
                foreach (T item in m_activeList)
                {
                    bool ok = true;
                    for (int i = 0; i < conditions.Count; i++)
                    {
                        if (!conditions[i].Validate(this, item))
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                    {
                        return item;
                    }
                }

                return null;
            }

            if (seed.Count == 0)
            {
                return null;
            }

            foreach (T item in seed)
            {
                bool ok = true;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Validate(this, item))
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok)
                {
                    return item;
                }
            }

            return null;
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    private IEnumerable<T> EnumerateFind<TKey>(
        IndexedObjectIndex<T, TKey> index,
        TKey value
    ) where TKey : notnull
    {
        int version = Volatile.Read(ref m_version);
        if ((index.flags & IndexedObjectKeyFlags.Unique) != 0 && index.TryGetSingle(value, out T? single) && single is not null)
        {
            EnsureVersion(version);
            yield return single;
            yield break;
        }

        HashSet<T>? set = index.FindUnsafe(value);
        if (set is null || set.Count == 0)
        {
            yield break;
        }

        foreach (T item in set)
        {
            EnsureVersion(version);
            yield return item;
        }
    }

    private List<T> BuildFindSnapshot<TKey>(
        IndexedObjectIndex<T, TKey> index,
        TKey value
    ) where TKey : notnull
    {
        List<T> result = [];
        if ((index.flags & IndexedObjectKeyFlags.Unique) != 0 && index.TryGetSingle(value, out T? single) && single is not null)
        {
            result.Add(single);
            return result;
        }

        HashSet<T>? set = index.FindUnsafe(value);
        if (set is null || set.Count == 0)
        {
            return result;
        }

        foreach (T item in set)
        {
            result.Add(item);
        }

        return result;
    }

    private IEnumerable<T> EnumerateQuery(List<IIndexedObjectQueryCondition<T>> conditions)
    {
        int version = Volatile.Read(ref m_version);
        if (conditions.Count == 0)
        {
            foreach (T item in m_activeList)
            {
                EnsureVersion(version);
                yield return item;
            }
            yield break;
        }

        int seedIndex = 0;
        int seedCount = conditions[0].GetCandidateCount(this);

        for (int i = 1; i < conditions.Count; i++)
        {
            int candidateCount = conditions[i].GetCandidateCount(this);
            if (candidateCount < seedCount)
            {
                seedCount = candidateCount;
                seedIndex = i;
            }
        }

        if (seedCount == 0)
        {
            yield break;
        }

        if (seedCount == 1 && conditions[seedIndex].TryGetSingle(this, out T single))
        {
            bool ok = true;
            for (int i = 0; i < conditions.Count; i++)
            {
                if (!conditions[i].Validate(this, single))
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
            {
                yield return single;
            }

            yield break;
        }

        HashSet<T>? seed = conditions[seedIndex].GetSet(this);
        if (seed is null)
        {
            foreach (T item in m_activeList)
            {
                bool ok = true;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Validate(this, item))
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok)
                {
                    EnsureVersion(version);
                    yield return item;
                }
            }

            yield break;
        }

        if (seed.Count == 0)
        {
            yield break;
        }

        foreach (T item in seed)
        {
            bool ok = true;
            for (int i = 0; i < conditions.Count; i++)
            {
                if (!conditions[i].Validate(this, item))
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
            {
                EnsureVersion(version);
                yield return item;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ValidateConditions(
        List<IIndexedObjectQueryCondition<T>> conditions,
        T item
    ) {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (!conditions[i].Validate(this, item))
            {
                return false;
            }
        }
        return true;
    }
}
