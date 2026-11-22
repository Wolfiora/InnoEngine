---
paths: ["**/*.cs"]
tags: [csharp-style]
---
# 13. Control flow, exceptions and asynchronous code

## X-01 Guard clauses return early

Handle invalid and boundary cases first and leave with `return`, `throw` or `continue`. The main path is not nested inside `else`. Guards may use the compact form of B-05.

```csharp
public void Draw(RenderItem item)
{
    if (m_disposed) return;
    if (!item.isVisible) return;

    Submit(item);
    m_drawn++;
}
```

Avoid:

```csharp
public void Draw(RenderItem item)
{
    if (!m_disposed)
    {
        if (item.isVisible)
        {
            Submit(item);
            m_drawn++;
        }
    }
}
```

## X-02 `switch`

- Use a `switch` expression to map a value to a value (I-04).
- Use a `switch` statement when the branches execute statements. `case` is indented four spaces from `switch`, and the statements four spaces from `case`. Each label is on its own line. `default` is last. Add braces to a section only when it declares local variables that would otherwise collide.

```csharp
switch (command.kind)
{
    case CommandKind.Draw:
    case CommandKind.DrawIndexed:
        Submit(command);
        break;
    case CommandKind.Clear:
        Clear(command.color);
        break;
    default:
        throw new InvalidOperationException($"Unknown command kind '{command.kind}'.");
}
```

Avoid:

```csharp
switch (command.kind)
{
case CommandKind.Draw: case CommandKind.DrawIndexed:
    Submit(command); break;
default: throw new InvalidOperationException($"Unknown command kind '{command.kind}'.");
case CommandKind.Clear:
    Clear(command.color);
    break;
}
```

## X-03 Exception types and messages

| Situation | Exception |
| --- | --- |
| An argument is `null` | `ArgumentNullException`, through `ThrowIfNull` |
| An argument value is invalid | `ArgumentException`, `ArgumentOutOfRangeException` |
| The object state does not allow the operation | `InvalidOperationException` |
| The object has been disposed | `ObjectDisposedException`, through `ThrowIf` |
| Input data is malformed | `InvalidDataException` |
| The platform or backend does not support the operation | `NotSupportedException` |

A message is a complete English sentence that includes the offending value. Parameter names use `nameof`. A `throw` whose arguments do not fit on one line breaks like any other call (A-02).

```csharp
throw new InvalidDataException(
    $"The scene file '{path}' declares version {version}, but only version {C_VERSION} is supported."
);
```

Avoid:

```csharp
throw new InvalidDataException(
    $"The scene file '{path}' declares version {version}, but only version {C_VERSION} is supported.");

throw new Exception("bad version");
```

## X-04 `catch`

- Catch the specific type that can be handled. When catching `Exception`, filter with `when` or rethrow with `throw;` after handling.
- The variable is named `exception` by default. A name that states the role, such as `cleanupFailure`, is allowed when the exception is stored or aggregated. Omit the variable when it is not used.
- A bare `catch` without a type is only for "clean up, then rethrow".
- A `catch` block that deliberately ignores an exception contains a comment that says why.

```csharp
try
{
    Directory.Delete(scratch, recursive: true);
}
catch (IOException)
{
    // The scratch directory is retried by the next retirement pass.
}

try
{
    Publish(candidate);
}
catch (Exception exception) when (exception is not OperationCanceledException)
{
    m_log.Error($"Publication failed: {exception.Message}");
    throw;
}
```

Avoid:

```csharp
try
{
    Directory.Delete(scratch, recursive: true);
}
catch {}

try
{
    Publish(candidate);
}
catch (Exception ex)
{
    throw ex;
}
```

## X-05 Asynchronous code

- Return `Task` or `ValueTask`. Never write `async void`.
- The method name ends with `Async` (NM-04). `CancellationToken cancellationToken` is the last parameter and is passed down to every asynchronous call.
- In product code (`src`, `build`, `backends`, `platforms`, `tools`, `entry`) every `await` ends with `.ConfigureAwait(false)`. Test code does not use `ConfigureAwait`.
- Do not block on asynchronous results with `.Wait()`, `.Result` or `.GetAwaiter().GetResult()`. An entry point that must wait synchronously carries a comment that explains why.
- On a broken `await` call, `.ConfigureAwait(false)` follows the closing parenthesis on its line.

```csharp
public async Task<AssetHandle> LoadAsync(
    AssetID id,
    CancellationToken cancellationToken
) {
    await using Stream stream = await m_storage.OpenAsync(id, cancellationToken).ConfigureAwait(false);
    return await m_reader.ReadAsync(
        stream,
        id,
        cancellationToken
    ).ConfigureAwait(false);
}
```

Avoid:

```csharp
public async void Load(AssetID id)
{
    Stream stream = m_storage.OpenAsync(id, CancellationToken.None).Result;
    await m_reader.ReadAsync(stream, id, CancellationToken.None);
}
```
