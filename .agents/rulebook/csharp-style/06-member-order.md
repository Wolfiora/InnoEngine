---
paths: ["**/*.cs"]
tags: [csharp-style, members]
---
# 6. Member order

## O-01 Group order

Members of a type appear in these groups, in this order, with one blank line between groups:

1. Constants (`const`).
2. Static fields: `static readonly` first, then static mutable.
3. Instance fields: `readonly` first, then mutable.
4. Constructors.
5. Public members.
6. Protected and internal members.
7. Private members.
8. Nested types.

Inside groups 5, 6 and 7 the order is: properties, events, instance methods, static methods. An explicit interface implementation follows the public members of the interface it implements.

```csharp
public sealed class RenderQueue : IDisposable
{
    private const int C_INITIAL_CAPACITY = 64;

    private static readonly RenderItemComparer COMPARER = new();

    private readonly List<RenderItem> m_items;
    private bool m_disposed;

    public RenderQueue()
    {
        m_items = new List<RenderItem>(C_INITIAL_CAPACITY);
    }

    public int count => m_items.Count;

    public event Action? flushed;

    public void Enqueue(RenderItem item)
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        m_items.Add(item);
    }

    public void Dispose()
    {
        m_items.Clear();
        m_disposed = true;
    }

    internal void Sort() => m_items.Sort(COMPARER);

    private void NotifyFlushed() => flushed?.Invoke();

    private sealed class RenderItemComparer : IComparer<RenderItem>
    {
        public int Compare(
            RenderItem left,
            RenderItem right
        ) => left.order.CompareTo(right.order);
    }
}
```

Avoid (a private method before the public members, fields after the constructor):

```csharp
public sealed class RenderQueue
{
    public RenderQueue()
    {
        m_items = new List<RenderItem>(C_INITIAL_CAPACITY);
    }

    private void NotifyFlushed() => flushed?.Invoke();

    private readonly List<RenderItem> m_items;

    public int count => m_items.Count;

    private const int C_INITIAL_CAPACITY = 64;
}
```

## O-02 Keep related members together

Members of the same accessibility and kind are contiguous. Overloads are adjacent and ordered from fewest to most parameters.

```csharp
public void Draw(Mesh mesh) => Draw(mesh, m_defaultMaterial);

public void Draw(
    Mesh mesh,
    Material material
) {
    Submit(new RenderItem(mesh, material));
    m_drawn++;
}

public void Clear() => m_items.Clear();

private void Submit(RenderItem item) => m_items.Add(item);
```

Avoid (overloads separated, a private method between public methods):

```csharp
public void Draw(Mesh mesh) => Draw(mesh, m_defaultMaterial);

private void Submit(RenderItem item) => m_items.Add(item);

public void Clear() => m_items.Clear();

public void Draw(
    Mesh mesh,
    Material material
) {
    Submit(new RenderItem(mesh, material));
    m_drawn++;
}
```
