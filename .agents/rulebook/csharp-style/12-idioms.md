---
paths: ["**/*.cs"]
tags: [csharp-style]
---
# 12. Types and expression idioms

## E-01 Local variables state their type; `var` only with `new T(...)`

- A local variable declares its type: `T x = expression;`.
- Use `var` when the right-hand side is `new T(...)` or `new T[...]`, where the type is visible on the same line.
- An anonymous type requires `var`.
- A local variable does not use `T x = new(...)`; that form is for fields and properties (M-04).
- A collection expression needs a target type, so the declaration states it: `List<string> names = [];`.

```csharp
AssetEntry entry = m_entries[index];
int count = entry.dependencies.Count;
string name = entry.displayName;
var builder = new StringBuilder(capacity);
var buffer = new byte[C_BUFFER_SIZE];
List<string> names = [];
var projection = new { entry.id, entry.displayName };
```

Avoid:

```csharp
var entry = m_entries[index];
var count = entry.dependencies.Count;
StringBuilder builder = new(capacity);
StringBuilder other = new StringBuilder(capacity);
```

## E-02 `foreach`, `for`, `using`, `out` and deconstruction state their types

```csharp
foreach (AssetEntry entry in m_entries)
{
    Visit(entry);
}

foreach ((string key, AssetEntry value) in m_byName)
{
    Visit(value);
}

for (int i = 0; i < count; i++)
{
    Visit(m_entries[i]);
}

using Stream stream = lease.OpenRead();

if (m_byName.TryGetValue(name, out AssetEntry? found))
{
    Visit(found);
}
```

Avoid:

```csharp
foreach (var entry in m_entries)
{
    Visit(entry);
}

using var stream = lease.OpenRead();

if (m_byName.TryGetValue(name, out var found))
{
    Visit(found);
}
```

## E-03 Built-in types use keywords

Write `int`, `string`, `float`, `object`, `nint` and `nuint`, not `Int32`, `String`, `IntPtr` or `UIntPtr`. Access static members through the keyword as well.

```csharp
nint handle = nint.Zero;
bool empty = string.IsNullOrWhiteSpace(name);
float largest = float.MaxValue;
```

Avoid:

```csharp
IntPtr handle = IntPtr.Zero;
bool empty = String.IsNullOrWhiteSpace(name);
Single largest = Single.MaxValue;
```

## E-04 Null checks use patterns

Use `is null` and `is not null`. Use `is { } x` or `is T x` to test and capture at once. Do not write `== null` or `!= null`.

```csharp
if (m_source is null) return;

if (m_owner is not null)
{
    m_owner.Release();
}

if (TryFind(id) is { } entry)
{
    Visit(entry);
}
```

Avoid:

```csharp
if (m_source == null) return;

if (!(m_owner is null))
{
    m_owner.Release();
}
```

## E-05 Argument validation

Validation comes first in a method, in parameter order, and uses the `ThrowIf*` helpers. Do not write `?? throw new ArgumentNullException(...)`.

```csharp
public Viewport(
    RenderTarget target,
    string name,
    int width
) {
    ArgumentNullException.ThrowIfNull(target);
    ArgumentException.ThrowIfNullOrWhiteSpace(name);
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);

    m_target = target;
    m_width = width;
    this.name = name;
}
```

Avoid:

```csharp
public Viewport(
    RenderTarget target,
    string name
) {
    m_target = target ?? throw new ArgumentNullException(nameof(target));
    if (name == null)
    {
        throw new ArgumentNullException("name");
    }
}
```

## E-06 Object creation

- A local variable: `var x = new T(...)` (E-01).
- A field or property initializer: `= new(...)` (M-04).
- A member that directly returns a new object of its declared return type may use `new(...)`.
- An argument, or a creation inside a larger expression, writes the type: `new T(...)`.

```csharp
public static Color FromBytes(
    byte r,
    byte g,
    byte b
) => new(r / 255f, g / 255f, b / 255f);
```

```csharp
var item = new RenderItem(mesh, material);
queue.Enqueue(new RenderItem(mesh, material));
```

Avoid:

```csharp
RenderItem item = new(mesh, material);
queue.Enqueue(new(mesh, material));
```

## E-07 Common values

| Meaning | Write | Not |
| --- | --- | --- |
| Empty string | `string.Empty` | `""` |
| Default value | `default` | `default(T)`, unless the type cannot be inferred |
| Empty collection | `[]` | `new List<T>()`, `Array.Empty<T>()` |
| Negation | `!ready` | `ready == false` |
| Non-nullable `bool` test | `if (ready)` | `if (ready == true)` |
| Nullable `bool?` test | `if (flag == true)` | `if (flag ?? false)` |
| Conversion | `(T)value` | `value as T` without a following null check |
| Type test with capture | `value is T typed` | `as` followed by a null check |
| Delegate invocation | `handler?.Invoke(args)` | a null check followed by a call |
| Rethrow | `throw;` | `throw exception;` |
| Infinite loop | `while (true)` | `for (;;)` |
| Current time | `DateTime.UtcNow`, `DateTimeOffset.UtcNow` | `DateTime.Now` |

```csharp
string label = string.Empty;
Color color = default;
flushed?.Invoke();
if (value is AssetEntry typed)
{
    Visit(typed);
}
```

Avoid:

```csharp
string label = "";
Color color = default(Color);
if (flushed != null)
{
    flushed.Invoke();
}

AssetEntry typed = value as AssetEntry;
if (typed != null)
{
    Visit(typed);
}
```

## E-08 `this.`

- Inside a constructor, every assignment to an instance member that is not an `m_` field goes through `this.`: `this.name = name;`, `this.count = 0;`. This holds whether or not a parameter has the same name.
- Outside constructors, do not write `this.` unless it is needed to disambiguate.
- Apart from constructor parameters, a parameter or local variable does not take the name of a property of the same type.

```csharp
public GameBuildPackageContext(
    BuildProfile profile,
    string outputDirectory
) {
    m_createdUtc = DateTime.UtcNow;
    this.profile = profile;
    this.outputDirectory = outputDirectory;
    this.isIncremental = true;
}

public BuildProfile profile { get; }
public string outputDirectory { get; }
public bool isIncremental { get; private set; }

public string Describe() => $"{profile.name} -> {outputDirectory}";
```

Avoid (`isIncremental` assigned without `this.` in the constructor, `this.` outside the constructor):

```csharp
public GameBuildPackageContext(
    BuildProfile profile,
    string outputDirectory
) {
    this.profile = profile;
    this.outputDirectory = outputDirectory;
    isIncremental = true;
}

public string Describe() => $"{this.profile.name} -> {this.outputDirectory}";
```

## E-09 Strings

- Build strings from values with interpolation, `$"..."`. Do not use `string.Format` or `string.Concat`.
- Split a long message with `+` at the start of the continuation line (W-01). Use a raw string literal, `"""`, for multi-line text.
- Comparison, lookup, sorting and dictionaries state `StringComparison.Ordinal` / `StringComparer.Ordinal` or the `OrdinalIgnoreCase` variants.
- Change case with `ToLowerInvariant()` / `ToUpperInvariant()` and format with `CultureInfo.InvariantCulture`.
- Refer to identifiers with `nameof(...)`.
- Test external input with `string.IsNullOrWhiteSpace`, and use `string.IsNullOrEmpty` only when whitespace is meaningful.

```csharp
string message = $"The asset '{path}' could not be imported: {reason}";
bool same = string.Equals(left, right, StringComparison.Ordinal);
var index = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
throw new ArgumentException("The path must be relative.", nameof(path));
```

Avoid:

```csharp
string message = string.Format("The asset '{0}' could not be imported: {1}", path, reason);
bool same = left.ToLower() == right.ToLower();
throw new ArgumentException("The path must be relative.", "path");
```

## E-10 Numeric literals

- `float`: write an integral value as `1f` and a fractional value as `1.5f`. Do not write `1.0f` or `1.50f`.
- Suffixes: `f`, `d`, `m` and `u` are lower case; `L` and `UL` are upper case.
- Hexadecimal: the `0x` prefix is lower case, the digits are upper case, and long values are grouped by four digits with `_`.
- Large decimal values are grouped by three digits with `_`.

```csharp
float half = 0.5f;
float one = 1f;
uint mask = 0xFF00_00FFu;
long ticks = 10_000_000L;
```

Avoid:

```csharp
float one = 1.0f;
float ratio = 1.50f;
uint mask = 0xff0000ffU;
long ticks = 10000000l;
```

## E-11 Lambdas

- One parameter has no parentheses: `x => ...`. No parameters: `() => ...`. Two or more parameters are expanded (P-01).
- Parameters have meaningful names. An unused parameter is `_`.
- A lambda that captures neither a local variable nor `this` is `static`.
- A single-expression body uses the expression form; several statements use a block (B-04).
- When an expression body does not fit on the line of the lambda, it is wrapped in parentheses, unless it is a call or object creation, which breaks inside its own parentheses (B-04).
- Do not use anonymous methods (`delegate { }`).

```csharp
entries.RemoveAll(static entry => entry.isMissing);
entries.RemoveAll(entry => entry.generation < m_generation);
button.clicked += () => m_log.Info("Clicked.");

Func<AssetEntry, bool> isStale = entry => (
    entry.lastWriteTimeUtc < threshold && !entry.isPinned
);
```

Avoid:

```csharp
entries.RemoveAll((entry) => entry.isMissing);
entries.RemoveAll(x => x.isMissing);

Func<AssetEntry, bool> isStale = entry =>
    entry.lastWriteTimeUtc < threshold && !entry.isPinned;
```

## E-12 `using` declarations

Use a `using` declaration when the resource lives until the end of the enclosing block. Use a `using` statement with a block only when the resource must be released earlier.

```csharp
using Stream stream = lease.OpenRead();
using var reader = new StreamReader(stream);
return reader.ReadToEnd();
```

Avoid (a block that ends where the method ends):

```csharp
using (Stream stream = lease.OpenRead())
{
    using (var reader = new StreamReader(stream))
    {
        return reader.ReadToEnd();
    }
}
```
