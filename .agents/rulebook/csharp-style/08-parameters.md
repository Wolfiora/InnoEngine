---
paths: ["**/*.cs"]
tags: [csharp-style, formatting]
---
# 8. Parameter and argument lists

## P-01 Parameter lists: one line for zero or one parameter, fully expanded for two or more

A declaration with two or more parameters breaks after the opening parenthesis, puts each parameter on its own line indented four spaces, and puts the closing parenthesis on its own line aligned with the start of the declaration. A declaration with zero or one parameter stays on one line.

This applies to methods, constructors, local functions, delegates, operators, primary constructors and lambdas.

```csharp
public void Clear() => m_commands.Clear();

public void Remove(string commandID) => m_commands.Remove(commandID);

public void Register(
    string commandID,
    Action command
) {
    m_commands.Add(commandID, command);
    m_dirty = true;
}

public delegate void AssetCallback(
    AssetID id,
    AssetHandle handle
);
```

Local functions and lambdas follow the same rule:

```csharp
void Visit(
    Node node,
    int depth
) {
    m_visited.Add(node);
    m_depths[node] = depth;
}

bool duplicate = updated.Where((
    _,
    candidateIndex
) => candidateIndex != index).Any();
```

Avoid:

```csharp
public void Register(string commandID, Action command)
{
    m_commands.Add(commandID, command);
    m_dirty = true;
}

public void Register(string commandID,
                     Action command)
{
    m_commands.Add(commandID, command);
    m_dirty = true;
}

public void Register(
    string commandID,
    Action command)
{
    m_commands.Add(commandID, command);
    m_dirty = true;
}

bool duplicate = updated.Where((_, candidateIndex) => candidateIndex != index).Any();
```

## P-02 What follows the closing parenthesis of a multi-line parameter list

| Followed by | Form |
| --- | --- |
| Block body | `) {` |
| Empty body | `) {}` |
| Expression body | `) => expression;` — when the expression must break, it breaks inside its own delimiters: `) => new(` … `);` |
| No body (interface, abstract, delegate, record) | `);` |
| Constructor initializer | `) : base(...)` or `) : this(...)`, opening brace on its own line |
| `where` clause | `) where T : class`, opening brace on its own line |
| Base list of a primary constructor | `) : Base`, opening brace on its own line |
| Lambda body | `) =>`, then the expression, or the opening brace on its own line |

```csharp
public interface IAssetStore
{
    Task<AssetHandle> LoadAsync(
        AssetID id,
        CancellationToken cancellationToken
    );
}

public sealed class NullAssetStore : IAssetStore
{
    public NullAssetStore(
        string name,
        int capacity
    ) {}

    public Task<AssetHandle> LoadAsync(
        AssetID id,
        CancellationToken cancellationToken
    ) => Task.FromResult(AssetHandle.MISSING);
}
```

Avoid:

```csharp
public sealed class NullAssetStore : IAssetStore
{
    public Task<AssetHandle> LoadAsync(
        AssetID id,
        CancellationToken cancellationToken
    )
        => Task.FromResult(AssetHandle.MISSING);
}
```

## P-03 Parameter details

- A default value is written `= value`, with one space on each side of `=`.
- `this`, `ref`, `out`, `in` and `params` are separated from the type by one space.
- `CancellationToken cancellationToken` is always the last parameter.

```csharp
public Task<AssetHandle> LoadAsync(
    AssetID id,
    bool required = true,
    CancellationToken cancellationToken = default
) {
    cancellationToken.ThrowIfCancellationRequested();
    return LoadCoreAsync(id, required, cancellationToken);
}
```

Avoid:

```csharp
public Task<AssetHandle> LoadAsync(
    CancellationToken cancellationToken,
    AssetID id,
    bool required=true
) {
    cancellationToken.ThrowIfCancellationRequested();
    return LoadCoreAsync(id, required, cancellationToken);
}
```

## A-01 Argument lists stay on one line when they fit

The arguments of a call, an object creation or a constructor initializer stay on one line whenever the whole statement fits within the line width.

```csharp
m_commands.Add(commandID, command);
var buffer = new VertexBuffer(device, capacity, VertexLayout.DEFAULT);
```

Avoid (a short call broken for no reason):

```csharp
m_commands.Add(
    commandID,
    command
);
```

## A-02 Broken argument lists: one argument per line, closing parenthesis on its own line

When an argument list does not fit on one line:

- The opening parenthesis ends its line.
- Each argument is on its own line, indented four spaces from the line that contains the opening parenthesis.
- The closing parenthesis is on its own line, aligned with the start of the line that contains the opening parenthesis. Whatever follows the call — `;`, `.Next(...)`, `,` or an outer closing delimiter — follows that parenthesis.
- Nested calls apply the same rule one level deeper.

This holds for every call, including `new`, `throw new`, `await`, `return`, attribute arguments and constructor initializers.

```csharp
return await m_textureCompiler.CompileKtxAsync(
    encoded,
    texture.slot.colorSpace,
    cancellationToken
).ConfigureAwait(false);

throw new ArgumentException(
    $"The viewport '{viewport.name}' does not support mode '{defaultMode}'.",
    nameof(defaultMode)
);

var result = new ShaderCompilationResult(
    null,
    [new ShaderDiagnostic(
        "SHADER_COMPILE_EXCEPTION",
        DiagnosticSeverity.Error,
        message
    )]
);

string intermediate = scoped.GetNativeBuildRoot(
    componentDescriptor,
    configuration,
    targetArchitecture
);
```

Avoid (closing parenthesis left on the last argument, first argument left on the opening line, several arguments packed on a line, arguments aligned with the parenthesis):

```csharp
return await m_textureCompiler.CompileKtxAsync(
    encoded,
    texture.slot.colorSpace,
    cancellationToken).ConfigureAwait(false);

return await m_textureCompiler.CompileKtxAsync(encoded,
    texture.slot.colorSpace, cancellationToken).ConfigureAwait(false);

throw new ArgumentException($"The viewport '{viewport.name}' does not support mode '{defaultMode}'.",
                            nameof(defaultMode));
```

## A-03 Hugging a single multi-line argument

When exactly one argument spans several lines, and that argument is a lambda, an object initializer, an anonymous object or a collection expression, the arguments may all start on the line of the opening parenthesis. The multi-line argument is laid out by its own rule, and the closing parenthesis follows its closing delimiter.

When two or more arguments span several lines, expand the list (A-02).

```csharp
m_dispatcher.Subscribe(entry =>
{
    m_pending.Add(entry);
    m_dirty = true;
});

Task task = Task.Run(() =>
{
    Compile(source);
    Publish(source);
}, cancellationToken);

loader.ApplySourceChanges([
    new AssetEvent("Text/new.txt", WatcherChangeTypes.Deleted),
    new AssetEvent("Text/old.txt", WatcherChangeTypes.Deleted)
]);

scheduler.Schedule(
    () =>
    {
        Compile(source);
        Publish(source);
    },
    exception =>
    {
        m_log.Error(exception.Message);
        m_failed = true;
    }
);
```

Avoid (two multi-line arguments hugged together):

```csharp
scheduler.Schedule(() =>
{
    Compile(source);
    Publish(source);
}, exception =>
{
    m_log.Error(exception.Message);
    m_failed = true;
});
```

## A-04 Named arguments

A named argument is written `name: value`, with no space before the colon and one space after it. Name adjacent `bool` or numeric literal arguments whose meaning is not obvious from the call.

```csharp
var state = new EditorActionState(true, true, displayName: "Enter Play Mode");
window.Resize(width: 1280, height: 720);
```

Avoid:

```csharp
window.Resize(width : 1280, height:720);
texture.Update(true, false, 0, 4);
```
