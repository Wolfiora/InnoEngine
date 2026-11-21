---
paths: ["**/*.cs"]
tags: [csharp-style, formatting]
---
# 7. Braces and blocks

## B-01 Braces are on their own lines

The opening brace of a type, a method body, an accessor, a control statement, a lambda block, an object or collection initializer and a `switch` expression is on its own line, aligned with the start of the construct. The closing brace is on its own line, aligned with the opening brace.

The only places where an opening brace shares a line with other code are:

- `) {` after a multi-line parameter list or a multi-line statement header (B-02).
- A single-line construct: an auto-property, a short initializer, a property pattern, a compact guard (B-05).
- An empty body `{}` (B-07).

```csharp
public void Flush()
{
    foreach (RenderItem item in m_items)
    {
        Draw(item);
        m_drawn++;
    }

    try
    {
        Present();
    }
    catch (DeviceLostException exception)
    {
        m_log.Error(exception.Message);
        throw;
    }
    finally
    {
        m_items.Clear();
    }
}
```

Avoid:

```csharp
public void Flush() {
    foreach (RenderItem item in m_items) {
        Draw(item);
    }

    try { Present(); }
    catch (DeviceLostException) { throw; }
    finally { m_items.Clear(); }
}
```

## B-02 `) {` after a multi-line parameter list or statement header

When a parameter list is expanded over several lines (P-01) and the body follows it directly, the opening brace shares the line of the closing parenthesis: `) {`. The closing brace aligns with the start of the declaration. This applies to methods, constructors, local functions, operators and primary constructors without a base list. The same form closes a control statement whose header spans several lines (W-05).

```csharp
internal void Register(
    string commandID,
    Action command
) {
    ArgumentException.ThrowIfNullOrWhiteSpace(commandID);
    ArgumentNullException.ThrowIfNull(command);
    m_commands.Add(commandID, command);
}
```

Avoid (brace on its own line after a multi-line parameter list, several parameters on one line):

```csharp
internal void Register(
    string commandID,
    Action command
)
{
    m_commands.Add(commandID, command);
    m_dirty = true;
}

internal void Unregister(string commandID, Action command)
{
    m_commands.Remove(commandID);
    m_dirty = true;
}
```

## B-03 Clauses between `)` and the body

A constructor initializer (`: base(...)`, `: this(...)`) or a `where` clause sits between the parameter list and the body.

- After a multi-line parameter list, the clause follows the closing parenthesis on the same line: `) : this(...)` or `) where T : class`. The opening brace is then on its own line.
- After a single-line signature, the clause is on the next line, indented four spaces. The opening brace is on its own line.
- Additional `where` clauses each take their own line, aligned with the first `where`.

```csharp
public InspectorMessageAttribute(
    string text,
    InspectorMessageType messageType
) : this(text, messageType, string.Empty)
{
    this.isExpanded = true;
}

public InspectorMessageAttribute(string text)
    : this(text, InspectorMessageType.Info)
{
    this.isExpanded = false;
}

public T Resolve<T>(
    string id,
    bool required
) where T : class
{
    object resolved = ResolveCore(typeof(T), id, required);
    return (T)resolved;
}

public TTarget Convert<TSource, TTarget>(
    TSource source,
    bool required
) where TSource : IdentityObject
  where TTarget : class
{
    object converted = ConvertCore(source, typeof(TTarget), required);
    return (TTarget)converted;
}

public T Find<T>(string id)
    where T : class
{
    object found = FindCore(typeof(T), id);
    return (T)found;
}
```

Avoid (clause on its own line after a multi-line parameter list, clause on the signature line of a single-line signature, `) {` after a clause):

```csharp
public InspectorMessageAttribute(
    string text,
    InspectorMessageType messageType
)
    : this(text, messageType, string.Empty)
{
    this.isExpanded = true;
}

public InspectorMessageAttribute(string text) : this(text, InspectorMessageType.Info)
{
    this.isExpanded = false;
}

public T Resolve<T>(
    string id,
    bool required
) where T : class {
    object resolved = ResolveCore(typeof(T), id, required);
    return (T)resolved;
}
```

## B-04 Lambda bodies

- **Expression body that fits:** `x => expression` on one line.
- **Block body:** `=>` ends the line; the braces are on their own lines, aligned with the start of the statement that contains the lambda. The closing brace is followed directly by `)`, `,` or `;`. This holds when the lambda has a multi-line parameter list too: `) =>`, then the brace on the next line.
- **Expression body that is a call or object creation and does not fit:** the call starts on the `=>` line and breaks inside its parentheses (A-02).
- **Any other expression body that does not fit:** wrap it in parentheses. `(` follows `=>`, the expression is indented four spaces, and `)` is on its own line.

```csharp
m_dispatcher.Subscribe(entry =>
{
    m_pending.Add(entry);
    m_dirty = true;
});

entries.Sort((
    left,
    right
) =>
{
    int byName = string.CompareOrdinal(left.name, right.name);
    return byName != 0 ? byName : left.order.CompareTo(right.order);
});

IEnumerable<Color> blended = pairs.Select(pair => new Color(
    pair.from.r + (pair.to.r - pair.from.r) * amount,
    pair.from.g + (pair.to.g - pair.from.g) * amount,
    pair.from.b + (pair.to.b - pair.from.b) * amount
));

Func<AssetEntry, bool> isStale = entry => (
    entry.lastWriteTimeUtc < threshold && !entry.isPinned
);
```

Avoid (brace on the `=>` line, block squeezed onto one line, expression body moved to the next line without parentheses):

```csharp
m_dispatcher.Subscribe(entry => {
    m_pending.Add(entry);
    m_dirty = true;
});

m_dispatcher.Subscribe(entry => { m_pending.Add(entry); m_dirty = true; });

Func<AssetEntry, bool> isStale = entry =>
    entry.lastWriteTimeUtc < threshold && !entry.isPinned;
```

## B-05 Control statement bodies always use braces

1. The body of `if`, `else`, `for`, `foreach`, `while`, `do`, `using`, `lock` and `fixed` is always enclosed in braces, each brace on its own line.
2. One compact form is permitted where it reads better: an `if` without `else` whose body is a single short statement may be written entirely on one line, `if (condition) statement;`. Use it for guard clauses and keep a run of adjacent guards in the same form.
3. A body on the next line without braces is never allowed.
4. Stacked `using` or `fixed` headers that share one braced body are the only headers that may follow each other without braces in between.

```csharp
if (m_disposed)
{
    return;
}

while (reader.Read())
{
    Process(reader.current);
}

foreach (RenderItem item in m_items)
{
    if (item is null) continue;
    if (!item.isVisible) continue;

    Draw(item);
}

fixed (byte* source = input)
fixed (byte* target = output)
{
    Copy(source, target, length);
}
```

Avoid:

```csharp
if (m_disposed)
    return;

foreach (RenderItem item in m_items)
    Draw(item);

foreach (AssetEntry entry in m_entries)
    if (entry.isMissing)
        missing.Add(entry);

foreach (RenderItem item in m_items) Draw(item);
```

## B-06 `if` / `else` chains

Every branch of an `if … else if … else` chain uses braces. The compact one-line form (B-05) is not used when the `if` has an `else`.

```csharp
if (state == EditorPlayModeState.Editing)
{
    EnterPlayMode();
}
else if (state == EditorPlayModeState.Playing)
{
    ExitPlayMode();
}
else
{
    m_log.Warning("Play mode is changing.");
}
```

Avoid:

```csharp
if (state == EditorPlayModeState.Editing) EnterPlayMode();
else ExitPlayMode();

if (state == EditorPlayModeState.Editing)
    EnterPlayMode();
else
{
    m_log.Warning("Play mode is already active.");
    ExitPlayMode();
}
```

## B-07 Empty bodies

- An empty type ends with a semicolon and has no braces.
- An empty method, constructor, accessor, local function or lambda body is `{}`, with no space inside, on the line where the header ends.
- An empty `catch` block is not empty: it contains a comment that explains why the exception is ignored (X-04).
- A property pattern that matches any non-null value is written `{ }`.

```csharp
public sealed class EmptyPayload;

public interface IMarker;

protected virtual void OnActivated() {}

public NullSink(
    string name,
    int capacity
) {}

public NamedSink(string name)
    : base(name) {}
```

Inside a method:

```csharp
Action noop = () => {};

if (value is { } resolved)
{
    Use(resolved);
}
```

Avoid:

```csharp
public sealed class EmptyPayload
{
}

public sealed class OtherPayload { }

protected virtual void OnActivated()
{
}

protected virtual void OnDeactivated() { }
```

## B-08 `else`, `catch`, `finally` and `while`

`else`, `catch` and `finally` start a new line after the closing brace. The `while` of a `do … while` loop starts a new line too.

```csharp
do
{
    read = stream.Read(buffer);
    total += read;
}
while (read > 0);
```

Avoid:

```csharp
do
{
    read = stream.Read(buffer);
    total += read;
} while (read > 0);
```

## B-09 One statement per line

A line holds one statement. The statement after a `case` label starts on the next line. Nothing follows an opening brace on its line. The compact guard of B-05 is the only construct in which a statement shares the line of its header.

```csharp
switch (key)
{
    case KeyCode.Left:
        cursor--;
        break;
    case KeyCode.Right:
        cursor++;
        break;
}
```

Avoid:

```csharp
switch (key)
{
    case KeyCode.Left: cursor--; break;
    case KeyCode.Right: cursor++; break;
}

x = 0; y = 0;
```
