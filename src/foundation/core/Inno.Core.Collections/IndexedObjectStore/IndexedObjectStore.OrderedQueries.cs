using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Inno.Core.Collections;

sealed partial class IndexedObjectStore<T>
where T : class
{
    internal IEnumerable<T> ExecuteOrderedQueryFast<TKey>(
        IndexedObjectKey<TKey> orderKey,
        List<IIndexedObjectQueryCondition<T>> conditions
    ) where TKey : notnull
        => EnumerateOrderedQuery(orderKey, conditions);

    internal IReadOnlyList<T> ExecuteOrderedQuerySnapshot<TKey>(
        IndexedObjectKey<TKey> orderKey,
        List<IIndexedObjectQueryCondition<T>> conditions
    ) where TKey : notnull
    {
        m_lock.EnterReadLock();
        try
        {
            var index = GetIndex(orderKey);
            if ((index.flags & IndexedObjectKeyFlags.Ordered) == 0)
            {
                throw new InvalidOperationException($"Key '{orderKey.name}' is not ordered.");
            }

            if (TryBuildOrderedCandidates(index, conditions, out List<OrderedCandidate<TKey>>? candidates))
                return SelectOrderedItems(candidates);

            return ScanOrderedSnapshot(index, conditions);
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    private IEnumerable<T> EnumerateOrderedQuery<TKey>(
        IndexedObjectKey<TKey> orderKey,
        List<IIndexedObjectQueryCondition<T>> conditions
    ) where TKey : notnull
    {
        var version = Volatile.Read(ref m_version);
        var index = GetIndex(orderKey);
        if ((index.flags & IndexedObjectKeyFlags.Ordered) == 0)
            throw new InvalidOperationException($"Key '{orderKey.name}' is not ordered.");

        if (TryBuildOrderedCandidates(index, conditions, out List<OrderedCandidate<TKey>>? candidates))
        {
            for (int i = 0; i < candidates.Count; i++)
            {
                EnsureVersion(version);
                yield return candidates[i].item;
            }
            yield break;
        }

        foreach (var key in index.EnumerateOrderedKeys())
        {
            EnsureVersion(version);
            if (index.TryGetSingle(key, out var single) && single != null)
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
                    yield return single;

                continue;
            }

            var set = index.FindUnsafe(key);
            if (set == null || set.Count == 0)
                continue;

            foreach (var item in set)
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
    }

    internal T? ExecuteOrderedFirst<TKey>(
        IndexedObjectKey<TKey> orderKey,
        List<IIndexedObjectQueryCondition<T>> conditions
    ) where TKey : notnull
    {
        m_lock.EnterReadLock();
        try
        {
            var index = GetIndex(orderKey);
            if ((index.flags & IndexedObjectKeyFlags.Ordered) == 0)
                throw new InvalidOperationException($"Key '{orderKey.name}' is not ordered.");

            if (TryGetOrderedCandidateSeed(conditions, out int seedIndex, out int seedCount))
            {
                if (seedCount == 0)
                    return null;
                if (seedCount == 1 && conditions[seedIndex].TryGetSingle(this, out T single))
                {
                    return ValidateConditions(conditions, single) ? single : null;
                }

                HashSet<T>? seed = conditions[seedIndex].GetSet(this);
                if (seed is not null)
                {
                    T? first = null;
                    TKey firstKey = default!;
                    int firstDenseIndex = int.MaxValue;
                    foreach (T item in seed)
                    {
                        if (!ValidateConditions(conditions, item) ||
                            !index.TryGetKey(item, out TKey itemKey) ||
                            !m_activeIndex.TryGetValue(item, out int denseIndex))
                        {
                            continue;
                        }

                        int comparison = first is null ? -1 : index.CompareKeys(itemKey, firstKey);
                        if (comparison < 0 || comparison == 0 && denseIndex < firstDenseIndex)
                        {
                            first = item;
                            firstKey = itemKey;
                            firstDenseIndex = denseIndex;
                        }
                    }
                    return first;
                }
            }

            return ScanOrderedFirst(index, conditions);
        }
        finally
        {
            m_lock.ExitReadLock();
        }
    }

    private bool TryBuildOrderedCandidates<TKey>(
        IndexedObjectIndex<T, TKey> orderIndex,
        List<IIndexedObjectQueryCondition<T>> conditions,
        out List<OrderedCandidate<TKey>> candidates
    ) where TKey : notnull
    {
        candidates = [];
        if (!TryGetOrderedCandidateSeed(conditions, out int seedIndex, out int seedCount))
            return false;
        if (seedCount == 0)
            return true;

        if (seedCount == 1 && conditions[seedIndex].TryGetSingle(this, out T single))
        {
            AddOrderedCandidate(orderIndex, conditions, candidates, single);
            return true;
        }

        HashSet<T>? seed = conditions[seedIndex].GetSet(this);
        if (seed is null)
            return false;
        foreach (T item in seed)
            AddOrderedCandidate(orderIndex, conditions, candidates, item);
        SortOrderedCandidates(orderIndex, candidates);
        return true;
    }

    private bool TryGetOrderedCandidateSeed(
        List<IIndexedObjectQueryCondition<T>> conditions,
        out int seedIndex,
        out int seedCount
    ) {
        seedIndex = -1;
        seedCount = int.MaxValue;
        for (int i = 0; i < conditions.Count; i++)
        {
            int candidateCount = conditions[i].GetCandidateCount(this);
            if (candidateCount >= seedCount)
                continue;
            seedCount = candidateCount;
            seedIndex = i;
        }
        return seedIndex >= 0 && seedCount != int.MaxValue;
    }

    private void AddOrderedCandidate<TKey>(
        IndexedObjectIndex<T, TKey> orderIndex,
        List<IIndexedObjectQueryCondition<T>> conditions,
        List<OrderedCandidate<TKey>> candidates,
        T item
    ) where TKey : notnull
    {
        if (ValidateConditions(conditions, item) &&
            orderIndex.TryGetKey(item, out TKey itemKey) &&
            m_activeIndex.TryGetValue(item, out int denseIndex))
        {
            candidates.Add(new OrderedCandidate<TKey>(item, itemKey, denseIndex));
        }
    }

    private static IReadOnlyList<T> SelectOrderedItems<TKey>(List<OrderedCandidate<TKey>> candidates) where TKey : notnull
    {
        var result = new T[candidates.Count];
        for (int i = 0; i < candidates.Count; i++)
            result[i] = candidates[i].item;
        return result;
    }

    private IReadOnlyList<T> ScanOrderedSnapshot<TKey>(
        IndexedObjectIndex<T, TKey> index,
        List<IIndexedObjectQueryCondition<T>> conditions
    ) where TKey : notnull
    {
        var result = new List<T>();
        foreach (TKey key in index.EnumerateOrderedKeys())
        {
            if (index.TryGetSingle(key, out T? single) && single is not null)
            {
                if (ValidateConditions(conditions, single))
                    result.Add(single);
                continue;
            }

            HashSet<T>? set = index.FindUnsafe(key);
            if (set is null)
                continue;
            foreach (T item in set)
            {
                if (ValidateConditions(conditions, item))
                    result.Add(item);
            }
        }
        return result;
    }

    private T? ScanOrderedFirst<TKey>(
        IndexedObjectIndex<T, TKey> index,
        List<IIndexedObjectQueryCondition<T>> conditions
    ) where TKey : notnull
    {
        foreach (TKey key in index.EnumerateOrderedKeys())
        {
            if (index.TryGetSingle(key, out T? single) && single is not null)
            {
                if (ValidateConditions(conditions, single))
                    return single;
                continue;
            }

            HashSet<T>? set = index.FindUnsafe(key);
            if (set is null)
                continue;
            foreach (T item in set)
            {
                if (ValidateConditions(conditions, item))
                    return item;
            }
        }
        return null;
    }

    private static void SortOrderedCandidates<TKey>(
        IndexedObjectIndex<T, TKey> index,
        List<OrderedCandidate<TKey>> candidates
    ) where TKey : notnull
        => candidates.Sort((
            left,
            right
        ) =>
        {
            int comparison = index.CompareKeys(left.key, right.key);
            return comparison != 0
                ? comparison
                : left.denseIndex.CompareTo(right.denseIndex);
        });

    private readonly record struct OrderedCandidate<TKey>(
        T item,
        TKey key,
        int denseIndex
    )
        where TKey : notnull;

}
