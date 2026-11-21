---
paths: ["**/*.cs"]
tags: [csharp-style, formatting]
---
# 11. Spacing and blank lines

## S-01 Spaces

| Position | Rule | Example |
| --- | --- | --- |
| Control keyword and `(` | One space | `if (x)`, `foreach (T item in items)` |
| Method name, `typeof`, `nameof`, `default` and `(` | No space | `Draw(item)`, `typeof(int)` |
| `new` and `(` or `[` | No space | `new()`, `new[]` |
| Inside parentheses and brackets | No space | `Add(a, b)`, `items[0]` |
| Before an index bracket | No space | `items[0]` |
| Comma | None before, one after | `Add(a, b)` |
| Semicolon in `for` | None before, one after | `for (int i = 0; i < n; i++)` |
| Binary operators, assignment, `=>`, `??`, `?:` | One on each side | `a + b`, `x => x.id` |
| Unary operators | Attached to the operand | `!ready`, `-value`, `i++` |
| After a cast | No space | `(float)count` |
| Member access `.`, `?.`, `->` | No space | `entry?.name` |
| Inside generic angle brackets | No space | `Dictionary<string, int>` |
| Nullable `?` and pointer `*` | Attached to the type | `string?`, `byte* data` |
| Range `..` | No space | `span[1..^1]` |
| Spread `..` | One space after | `[.. items]` |
| Inside single-line braces with content | One on each side | `{ get; }`, `new T { a = 1 }`, `is { }` |
| Empty body | No space inside | `{}` |
| Before `{` on the same line | One space | `) {`, `() {}` |
| Colon of a base list, constraint or initializer | One on each side | `class A : B`, `where T : class` |
| Colon of a named argument or attribute target | None before, one after | `width: 1280`, `[assembly: X]` |

```csharp
if (ready)
{
    float ratio = (float)count / total;
    int first = items[0];
    Draw(item, ratio, first);
}
```

Avoid:

```csharp
if(ready)
{
    float ratio = (float) count/total;
    int first = items [0];
    Draw( item,ratio , first );
}
```

## S-02 Column alignment

- When more than three consecutive `const` declarations, or more than three consecutive `static readonly` declarations, have the same type, aligning their `=` signs in a column is recommended. It is not required.
- A numeric matrix or lookup table may align its elements in columns.
- Everywhere else, use a single space between tokens. Do not column-align local variables, assignments, parameters or comments.

```csharp
private const int C_WIDTH        = 1280;
private const int C_HEIGHT       = 720;
private const int C_REFRESH_RATE = 60;
private const int C_MIN_SIZE     = 16;

public static readonly Color RED   = FromBytes(255, 0, 0);
public static readonly Color GREEN = FromBytes(0, 255, 0);
public static readonly Color BLUE  = FromBytes(0, 0, 255);
public static readonly Color WHITE = FromBytes(255, 255, 255);

private static readonly float[] IDENTITY =
[
    1f, 0f, 0f, 0f,
    0f, 1f, 0f, 0f,
    0f, 0f, 1f, 0f,
    0f, 0f, 0f, 1f
];
```

Avoid (alignment of local variables and trailing comments):

```csharp
int    width        = 1280;
string title        = "Scene";     // window title
float  refreshRate  = 60f;         // hertz
```

## S-03 Blank lines between members

- Methods, constructors, members with documentation comments and multi-line properties are separated by one blank line.
- Fields, constants and single-line properties of the same group that have no documentation comments are written consecutively.
- Different groups (O-01) are separated by one blank line.
- Enum members are not separated by blank lines, even when they have documentation comments.
- Types are separated by one blank line.

```csharp
public sealed class Viewport
{
    private const int C_MIN_SIZE = 16;
    private const int C_MAX_SIZE = 16384;

    private readonly RenderTarget m_target;
    private int m_width;
    private int m_height;

    /// <summary>
    /// Gets the current width in pixels.
    /// </summary>
    public int width => m_width;

    /// <summary>
    /// Gets the current height in pixels.
    /// </summary>
    public int height => m_height;

    internal bool isDirty { get; set; }
    internal int generation { get; set; }
}
```

Avoid:

```csharp
public sealed class Viewport
{
    private const int C_MIN_SIZE = 16;

    private const int C_MAX_SIZE = 16384;
    private readonly RenderTarget m_target;
    /// <summary>
    /// Gets the current width in pixels.
    /// </summary>
    public int width => m_width;
    internal bool isDirty { get; set; }

}
```

## S-04 Blank lines inside method bodies

- Separate processing phases (validation, preparation, execution, completion) with one blank line. Do not add blank lines mechanically.
- The argument validation at the start of a method (a run of `ThrowIf*` calls and guards) is contiguous and followed by one blank line.
- A local function is preceded by one blank line and placed at the end of the method body.
- No blank line follows an opening brace or precedes a closing brace. Never leave two consecutive blank lines.

```csharp
public AssetHandle Import(
    string path,
    ImportOptions options
) {
    ArgumentException.ThrowIfNullOrWhiteSpace(path);
    ArgumentNullException.ThrowIfNull(options);
    ObjectDisposedException.ThrowIf(m_disposed, this);

    AssetSource source = OpenSource(path);
    ImportPlan plan = BuildPlan(source, options);

    AssetHandle handle = Execute(plan);
    m_handles.Add(handle.id, handle);
    return handle;

    ImportPlan BuildPlan(
        AssetSource target,
        ImportOptions settings
    ) => new(target, settings);
}
```

Avoid:

```csharp
public AssetHandle Import(
    string path,
    ImportOptions options
) {

    ArgumentException.ThrowIfNullOrWhiteSpace(path);

    ArgumentNullException.ThrowIfNull(options);
    AssetSource source = OpenSource(path);


    return Execute(BuildPlan(source, options));

}
```
