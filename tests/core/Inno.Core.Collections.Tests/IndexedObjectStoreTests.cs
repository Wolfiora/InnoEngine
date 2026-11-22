using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Inno.Core.Collections;

namespace Inno.Core.Collections.Tests;

public sealed class IndexedObjectStoreTests
{
    [Fact]
    public void AddRemoveCountClear()
    {
        var store = new IndexedObjectStore<Item>();
        var a = new Item("A", 1);
        var b = new Item("B", 2);

        store.Add(a);
        store.Add(b);
        Assert.Equal(2, store.count);

        Assert.True(store.Remove(a));
        Assert.Equal(1, store.count);
        Assert.False(store.Remove(a));

        store.Clear();
        Assert.Equal(0, store.count);
    }

    [Fact]
    public void DefineKeyUniqueAndLookup()
    {
        var store = new IndexedObjectStore<Item>();
        IndexedObjectKey<int> key = store.DefineKey<int>("id", IndexedObjectKeyFlags.Unique);

        var a = new Item("A", 1);
        var b = new Item("B", 2);

        store.Add(a).Set(key, 10);
        store.Add(b).Set(key, 20);

        Item? found = store.First(key, 20);
        Assert.Same(b, found);

        List<Item> all = store.Find(key, 10).ToList();
        Assert.Single(all);
        Assert.Same(a, all[0]);
    }

    [Fact]
    public void DefineKeyUniqueThrowsOnDuplicateValueFromDifferentItems()
    {
        var store = new IndexedObjectStore<Item>();
        IndexedObjectKey<int> key = store.DefineKey<int>("id", IndexedObjectKeyFlags.Unique);

        var a = new Item("A", 1);
        var b = new Item("B", 2);

        store.Add(a).Set(key, 10);

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => store.Add(b).Set(key, 10));

        Assert.Contains("Duplicate key", ex.Message);
    }

    [Fact]
    public void DefineKeyUniqueAllowsResettingSameValueOnSameItem()
    {
        var store = new IndexedObjectStore<Item>();
        IndexedObjectKey<int> key = store.DefineKey<int>("id", IndexedObjectKeyFlags.Unique);

        var a = new Item("A", 1);

        store.Add(a).Set(key, 10);
        store.Add(a).Set(key, 20);

        Item? found = store.First(key, 20);
        Assert.Same(a, found);
    }

    [Fact]
    public void DefineKeyMultiAndQuery()
    {
        var store = new IndexedObjectStore<Item>();
        IndexedObjectKey<int> key = store.DefineKey<int>("category", IndexedObjectKeyFlags.Unordered);

        var a = new Item("A", 1);
        var b = new Item("B", 1);
        var c = new Item("C", 2);

        store.Add(a).Set(key, 1);
        store.Add(b).Set(key, 1);
        store.Add(c).Set(key, 2);

        List<Item> results = store.Find(key, 1).ToList();
        Assert.Equal(2, results.Count);
        Assert.Contains(a, results);
        Assert.Contains(b, results);

        IndexedObjectQuery<Item> query = store.Query().Find(key, 2);
        Item? single = query.First();
        Assert.Same(c, single);
    }

    [Fact]
    public void OrderedQueryUsesSmallestIndexedCandidateSetAndReturnsAnIsolatedSnapshot()
    {
        var store = new IndexedObjectStore<Item>();
        IndexedObjectKey<int> categoryKey = store.DefineKey<int>("category");
        IndexedObjectKey<int> orderKey = store.DefineKey<int>(
            "order",
            IndexedObjectKeyFlags.Ordered | IndexedObjectKeyFlags.Unique,
            Comparer<int>.Default
        );
        var unrelated = new Item("Unrelated", 2);
        var last = new Item("Last", 1);
        var excluded = new Item("Excluded", 1);
        var first = new Item("First", 1);
        store.Add(unrelated).Set(categoryKey, 2).Set(orderKey, 0);
        store.Add(last).Set(categoryKey, 1).Set(orderKey, 30);
        store.Add(excluded).Set(categoryKey, 1).Set(orderKey, 20);
        store.Add(first).Set(categoryKey, 1).Set(orderKey, 10);

        int predicateCalls = 0;
        IReadOnlyList<Item> snapshot = store.Query()
            .Where(item =>
            {
                predicateCalls++;
                return item.name != "Excluded";
            })
            .Find(categoryKey, 1)
            .OrderBy(orderKey)
            .Get();

        Assert.Equal(new[] { first, last }, snapshot);
        Assert.Equal(3, predicateCalls);
        Assert.True(store.Remove(first));
        Assert.Equal(new[] { first, last }, snapshot);

        predicateCalls = 0;
        Item? earliest = store.Query()
            .Where(item =>
            {
                predicateCalls++;
                return true;
            })
            .Find(categoryKey, 1)
            .OrderBy(orderKey)
            .First();
        Assert.Same(excluded, earliest);
        Assert.Equal(2, predicateCalls);

        predicateCalls = 0;
        Item[] fast = store.Query()
            .Where(item =>
            {
                predicateCalls++;
                return true;
            })
            .Find(categoryKey, 1)
            .OrderBy(orderKey)
            .GetFast()
            .ToArray();
        Assert.Equal([excluded, last], fast);
        Assert.Equal(2, predicateCalls);
    }

    [Fact]
    public void OrderByUsesSortedKey()
    {
        var store = new IndexedObjectStore<Item>();
        IndexedObjectKey<int> key = store.DefineKey<int>("ordered", IndexedObjectKeyFlags.Ordered, Comparer<int>.Default);

        var a = new Item("A", 1);
        var b = new Item("B", 1);
        var c = new Item("C", 1);

        store.Add(a).Set(key, 3);
        store.Add(b).Set(key, 1);
        store.Add(c).Set(key, 2);

        List<Item> ordered = store.Query().OrderBy(key).Get().ToList();
        Assert.Equal(3, ordered.Count);
        Assert.Same(b, ordered[0]);
        Assert.Same(c, ordered[1]);
        Assert.Same(a, ordered[2]);
    }

    [Fact]
    public void RemoveKeyInvalidatesKey()
    {
        var store = new IndexedObjectStore<Item>();
        IndexedObjectKey<int> key = store.DefineKey<int>("temp");

        var item = new Item("A", 1);
        store.Add(item).Set(key, 1);

        Assert.True(key.isValid);
        Assert.True(store.RemoveKey(key));
        Assert.False(key.isValid);
    }

    [Fact]
    public void TryGetKeyResolvesByName()
    {
        var store = new IndexedObjectStore<Item>();
        IndexedObjectKey<int> key = store.DefineKey<int>("resolve");

        Assert.True(store.TryGetKey("resolve", out IndexedObjectKey<int> resolved));
        Assert.True(resolved.isValid);
        Assert.Equal(key.name, resolved.name);
    }

    [Fact]
    public void PredicateQueryWorks()
    {
        var store = new IndexedObjectStore<Item>();
        var a = new Item("A", 1);
        var b = new Item("B", 2);

        store.Add(a);
        store.Add(b);

        List<Item> results = store.Query()
            .Where(item => item.category == 2)
            .Get()
            .ToList();

        Assert.Single(results);
        Assert.Same(b, results[0]);
    }

    [Fact]
    public void GetFastIsFailFastWhenStoreMutates()
    {
        var store = new IndexedObjectStore<Item>();
        store.Add(new Item("A", 1));
        store.Add(new Item("B", 2));

        using IEnumerator<Item> enumerator = store.Query().GetFast().GetEnumerator();
        Assert.True(enumerator.MoveNext());

        store.Add(new Item("C", 3));

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    [Fact]
    public void GetReturnsStableSnapshotWhenStoreMutates()
    {
        var store = new IndexedObjectStore<Item>();
        var a = new Item("A", 1);
        var b = new Item("B", 2);
        store.Add(a);
        store.Add(b);

        IReadOnlyList<Item> snapshot = store.Query().Get();
        store.Add(new Item("C", 3));

        Assert.Equal(2, snapshot.Count);
        Assert.Contains(a, snapshot);
        Assert.Contains(b, snapshot);
    }

    [Fact]
    public void RemoveAllKeepsKeys()
    {
        var store = new IndexedObjectStore<Item>();
        IndexedObjectKey<int> key = store.DefineKey<int>("category");

        var a = new Item("A", 1);
        store.Add(a).Set(key, 1);

        store.RemoveAll();
        Assert.Equal(0, store.count);
        Assert.True(key.isValid);
    }

    [Fact]
    public void AllReturnsStableSnapshotWhenStoreMutates()
    {
        var store = new IndexedObjectStore<Item>();
        var a = new Item("A", 1);
        var b = new Item("B", 2);
        store.Add(a);
        store.Add(b);

        IReadOnlyList<Item> snapshot = store.All();
        store.Add(new Item("C", 3));

        Assert.Equal(2, snapshot.Count);
        Assert.Contains(a, snapshot);
        Assert.Contains(b, snapshot);
    }

    [Fact]
    public void AllFastReturnsAllItems()
    {
        var store = new IndexedObjectStore<Item>();
        var a = new Item("A", 1);
        var b = new Item("B", 2);
        var c = new Item("C", 3);
        store.Add(a);
        store.Add(b);
        store.Add(c);

        IReadOnlyList<Item> all = store.AllFast().ToList();

        Assert.Equal(3, all.Count);
        Assert.Contains(a, all);
        Assert.Contains(b, all);
        Assert.Contains(c, all);
    }

    [Fact]
    public void AllFastIsFailFastWhenStoreMutates()
    {
        var store = new IndexedObjectStore<Item>();
        store.Add(new Item("A", 1));
        store.Add(new Item("B", 2));

        using IEnumerator<Item> enumerator = store.AllFast().GetEnumerator();
        Assert.True(enumerator.MoveNext());

        store.Add(new Item("C", 3));

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    private sealed class Item
    {
        public Item(
            string name,
            int category
        ) {
            this.name = name;
            this.category = category;
        }

        public string name { get; }
        public int category { get; }
    }
}
