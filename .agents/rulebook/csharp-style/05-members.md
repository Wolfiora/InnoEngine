---
paths: ["**/*.cs"]
tags: [csharp-style]
---
# 5. Member declarations

## M-01 Property forms

- An auto-property is written on one line: `{ get; }`, `{ get; set; }`, `{ get; init; }`, `{ get; private set; }`.
- A read-only computed property uses an expression body.
- A property whose accessors contain logic is expanded: the braces are on their own lines, an accessor with a single expression uses `=>`, an accessor with statements uses a block.

```csharp
public string name { get; }
public int count { get; private set; }
public bool isEmpty => m_items.Count == 0;

public float scale
{
    get => m_scale;
    set
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
        m_scale = value;
    }
}
```

Avoid:

```csharp
public bool isEmpty { get { return m_items.Count == 0; } }
public float scale { get => m_scale; set => m_scale = value; }
```

## M-02 Expression-bodied members

- A method, property, indexer or operator whose body is one expression (one `return`, one call or one assignment) uses an expression body.
- A body with control flow, several statements or `try` uses a block.
- A constructor always uses a block body.

```csharp
public override string ToString() => $"{name} ({count})";

public static bool operator ==(
    Color left,
    Color right
) => left.Equals(right);

public RenderQueue(int capacity)
{
    m_items = new List<RenderItem>(capacity);
}
```

Avoid:

```csharp
public override string ToString()
{
    return $"{name} ({count})";
}

public RenderQueue(int capacity) => m_items = new List<RenderItem>(capacity);
```

## M-03 Placement of `=>` in expression-bodied members

1. When the whole member fits on one line, `=> expression` stays on the declaration line.
2. After a multi-line parameter list, the body starts on the line of the closing parenthesis: `) => expression`. When the expression is a call, an object creation or a collection that does not fit, break inside it (A-02).
3. When the signature is on one line and the body does not fit after it, move `=>` to the next line, indented four spaces.
4. After a wrapped `=>`, a brace-delimited expression (a `switch` expression, an object initializer or a `with` expression) puts its braces on their own lines at the column of the declaration, with its contents indented four spaces from the declaration.
5. After a wrapped `=>`, a call or object creation that must break follows A-02 relative to the `=>` line, and operator continuation lines align with the first operand (F-05).
6. `=>` never ends a line with the expression on the next line, and never stands alone on a line.

```csharp
public bool isMissing => m_source is null;

public static Color FromBytes(
    byte r,
    byte g,
    byte b
) => new(r / 255f, g / 255f, b / 255f);

public static Color Lerp(
    Color from,
    Color to,
    float amount
) => new(
    from.r + (to.r - from.r) * amount,
    from.g + (to.g - from.g) * amount,
    from.b + (to.b - from.b) * amount
);

protected override EditorActionState Query(EditorActionContext context)
    => playMode.state switch
{
    EditorPlayModeState.Editing => new EditorActionState(true, true),
    EditorPlayModeState.Playing => new EditorActionState(true, false),
    _ => EditorActionState.DISABLED
};

internal bool IsActiveFor(object? target)
    => m_isActive
       && m_activeTarget is not null
       && m_activeTarget.Matches(m_activeInteractions, target);

protected override EditorActionState Describe(EditorActionContext context)
    => new EditorActionState(
        isVisible: context.selection.count > 0,
        isEnabled: playMode.state == EditorPlayModeState.Editing,
        displayName: "Duplicate Selection"
    );
```

Avoid (`=>` left at the end of the line, braces indented under the wrapped `=>`, closing parenthesis left on the last argument):

```csharp
protected override EditorActionState Query(EditorActionContext context) =>
    playMode.state switch
    {
        EditorPlayModeState.Editing => new EditorActionState(true, true),
        _ => EditorActionState.DISABLED
    };

public static Color Lerp(
    Color from,
    Color to,
    float amount
)
    => new(
        from.r + (to.r - from.r) * amount,
        from.g + (to.g - from.g) * amount,
        from.b + (to.b - from.b) * amount);
```

## M-04 Field and property initializers

- A field or property initializer uses a target-typed expression so the type is written once: `= new(...)` or `= []`.
- One declaration declares one field or one local variable.

```csharp
private readonly Dictionary<string, Action> m_commands = new(StringComparer.Ordinal);
private readonly List<RenderItem> m_items = [];

public IReadOnlyList<string> tags { get; init; } = [];
```

Avoid:

```csharp
private readonly Dictionary<string, Action> m_commands = new Dictionary<string, Action>(StringComparer.Ordinal);
private readonly List<RenderItem> m_items = new List<RenderItem>();
```

## M-05 Attributes

- Each attribute is on its own line above the declaration. One pair of brackets holds one attribute.
- Omit the `Attribute` suffix. Omit empty parentheses.
- An attribute on a parameter stays on the line of the parameter.
- Attribute arguments break like any other argument list (A-02).

```csharp
[Fact]
[Trait("Category", "Lifecycle")]
public void DisposeIsIdempotent()
{
    m_queue.Dispose();
    m_queue.Dispose();
}

[SerializableProperty]
public float speed { get; set; }

public bool TryRead(
    string key,
    [NotNullWhen(true)] out string? value
) => m_values.TryGetValue(key, out value);
```

Avoid:

```csharp
[Fact, Trait("Category", "Lifecycle")]
public void DisposeIsIdempotent()
{
    m_queue.Dispose();
    m_queue.Dispose();
}

[SerializableProperty()] public float speed { get; set; }
```
