---
paths: ["**/*.cs"]
tags: [csharp-style, naming]
---
# 3. Naming

## Summary

| Element | Rule | Example |
| --- | --- | --- |
| Namespace segment | PascalCase | `Inno.Core.Events` |
| Class, struct, record, enum, delegate | PascalCase | `RenderGraph`, `AssetHandle` |
| Interface | `I` + PascalCase | `IEventSink` |
| Type parameter | `T` or `T` + PascalCase | `T`, `TSource` |
| Method, local function | PascalCase | `TryResolve`, `CreateBuffer` |
| Property, indexer | camelCase | `transform`, `isMissing` |
| Event | camelCase | `redrawRequested` |
| Constant (`const`, including local constants) | `C_` + UPPER_SNAKE | `C_MAX_COUNT` |
| `static readonly` field (any accessibility) | UPPER_SNAKE | `DEFAULT_OPTIONS`, `RED` |
| Private static mutable field | `s_` + camelCase | `s_nextID` |
| Private or protected instance field | `m_` + camelCase | `m_generation` |
| Internal or public instance field | camelCase | `latestSucceeded` |
| Enum member | PascalCase | `Borrowed` |
| Parameter, local variable, tuple element | camelCase | `outputDirectory` |
| Two-letter acronym | Upper case in PascalCase positions | `AssetID`, `IOStatus`, `UIScale` |
| Test class and test method | `XxxTests`, PascalCase sentence | `DispatchAfterDisposeThrows` |

## NM-01 Type names

A type name is a PascalCase noun or noun phrase. The suffix follows the role: `Attribute`, `Exception`, `EventArgs`, `Tests`, and `Extensions` for a class that holds extension methods. An abstract class has no `Base` or `Abstract` affix.

```csharp
public sealed class InspectorMessageAttribute : Attribute;

public static class PathExtensions
{
    public static string Normalize(this string path) => path.Replace('\\', '/');
}

public abstract class EditorDrop;
```

Avoid:

```csharp
public sealed class InspectorMessage : Attribute;

public static class PathHelpers
{
    public static string Normalize(this string path) => path.Replace('\\', '/');
}

public abstract class EditorDropBase;
```

## NM-02 Interfaces and type parameters

An interface name starts with `I`. No other type starts with `I` followed by an upper-case letter. Use `T` when there is a single, obvious type parameter; otherwise use `T` plus a description.

```csharp
public interface IAssetImporter<TAsset>
    where TAsset : class
{
    TAsset Import(string path);
}

public sealed class Pool<T>;
```

Avoid:

```csharp
public interface AssetImporter<Asset>
    where Asset : class
{
    Asset Import(string path);
}
```

## NM-03 Method names

A method name is PascalCase and starts with a verb. These prefixes have fixed meanings:

- `Try*` returns `bool`, does not throw on failure and returns its result through an `out` parameter.
- `Is*`, `Has*` and `Can*` return `bool` and have no side effects.
- `Create*` builds a new object that the caller owns.
- `Get*` reads existing state without side effects.
- `*Async` returns `Task` or `ValueTask` (NM-04).

```csharp
public bool TryResolve(
    AssetID id,
    out AssetHandle handle
) => m_handles.TryGetValue(id, out handle);
```

Avoid (`Try*` that does not return `bool`):

```csharp
public AssetHandle? TryResolve(AssetID id) => m_handles.GetValueOrDefault(id);
```

## NM-04 Asynchronous methods end with `Async`

A method that returns `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` ends with `Async`. Overrides and interface implementations follow their contract. xUnit test methods do not take the suffix.

```csharp
public async Task<ShaderCompilationResult> CompileAsync(
    ShaderSource source,
    CancellationToken cancellationToken
) {
    ShaderProgram program = await m_frontend.ParseAsync(source, cancellationToken).ConfigureAwait(false);
    return await m_backend.EmitAsync(program, cancellationToken).ConfigureAwait(false);
}
```

Avoid:

```csharp
public async Task<ShaderCompilationResult> Compile(
    ShaderSource source,
    CancellationToken cancellationToken
) {
    ShaderProgram program = await m_frontend.ParseAsync(source, cancellationToken).ConfigureAwait(false);
    return await m_backend.EmitAsync(program, cancellationToken).ConfigureAwait(false);
}
```

## NM-05 Properties and indexers are camelCase

Every property is camelCase, whatever its accessibility. A `bool` property starts with `is`, `has` or `can`, or is a past participle that states a condition, such as `disposed`.

```csharp
public Transform transform { get; }
public bool isMissing => m_source is null;
public int count { get; private set; }
```

Avoid:

```csharp
public Transform Transform { get; }
public bool Missing => m_source is null;
public int Count { get; private set; }
```

## NM-06 Instance fields

- A private or protected instance field is `m_` + camelCase, whether or not it is `readonly`.
- An internal or public instance field is camelCase without a prefix. Such fields appear only in frame-local workspaces, interop structs and math types.
- One declaration declares one field.

```csharp
public sealed class RenderQueue
{
    private readonly List<RenderItem> m_items = [];
    private int m_generation;
}

internal struct GlyphVertex
{
    public Vector2 position;
    public Vector2 uv;
}
```

Avoid:

```csharp
public sealed class RenderQueue
{
    private readonly List<RenderItem> _items = [];
    private int generation, frameIndex;
}
```

## NM-07 Constants

Every `const` is `C_` + UPPER_SNAKE: private, internal and public constants, and constants local to a method.

```csharp
public sealed class SceneFile
{
    public const string C_DEFAULT_EXTENSION = ".iscene";
    private const int C_MAX_COUNT = 64;

    public void Flush()
    {
        const int C_BATCH_SIZE = 256;
        Write(C_BATCH_SIZE);
    }
}
```

Avoid:

```csharp
public sealed class SceneFile
{
    public const string DefaultExtension = ".iscene";
    private const int MAX_COUNT = 64;

    public void Flush()
    {
        const int batchSize = 256;
        Write(batchSize);
    }
}
```

## NM-08 `static readonly` fields

Every `static readonly` field is UPPER_SNAKE without any prefix, whatever its accessibility. The `C_` prefix is reserved for `const`.

```csharp
public readonly struct Color
{
    public static readonly Color RED = FromBytes(255, 0, 0);
    private static readonly JsonSerializerOptions SERIALIZER_OPTIONS = new(JsonSerializerDefaults.Web);
}
```

Avoid:

```csharp
public readonly struct Color
{
    public static readonly Color Red = FromBytes(255, 0, 0);
    private static readonly JsonSerializerOptions S_SERIALIZER_OPTIONS = new();
    private static readonly JsonSerializerOptions s_serializerOptions = new();
    private static readonly JsonSerializerOptions C_SERIALIZER_OPTIONS = new();
}
```

## NM-09 Static mutable fields

A private static mutable field is `s_` + camelCase. Product code does not declare non-private static mutable fields; shared process state belongs to an explicit host.

```csharp
public sealed class HandleAllocator
{
    private static int s_nextID;
}
```

Avoid:

```csharp
public sealed class HandleAllocator
{
    private static int m_nextID;
    public static int nextID;
}
```

## NM-10 Events

An event is camelCase, like a property. Name it with a past participle or as `xxxChanged` / `xxxRequested`. Its delegate type is a nullable `Action` or `Action<...>`.

```csharp
public event Action? redrawRequested;
public event Action<AssetID>? assetReloaded;
```

Avoid:

```csharp
public event Action? RedrawRequested;
public event EventHandler? OnRedraw;
```

## NM-11 Enums

- A single-value enum has a singular name. A `[Flags]` enum has a plural name or ends with `Flags`.
- Members are PascalCase and never start with an underscore.
- `[Flags]` values are written as `1 << n`, with `None = 0`. Combined values are expressed with member names.
- One member per line, no trailing comma, no blank lines between members.

```csharp
public enum EditorPlayModeState
{
    Editing,
    Compiling,
    Playing
}

[Flags]
public enum EditorViewportNavigationCapabilities
{
    None = 0,
    Orbit = 1 << 0,
    Pan = 1 << 1,
    Zoom = 1 << 2,
    All = Orbit | Pan | Zoom
}
```

Avoid:

```csharp
[Flags]
public enum EditorViewportNavigationCapability
{
    None = 0,
    Orbit = 1,
    Pan = 0x2,
    Zoom = 4,
}
```

## NM-12 Parameters, local variables and tuple elements

Use camelCase and whole words. Do not use Hungarian prefixes. Do not give a local variable or a parameter the name of a property of the same type, except for constructor parameters (E-08).

| Situation | Name |
| --- | --- |
| `CancellationToken` parameter | `cancellationToken` |
| Default `catch` variable | `exception` |
| Unused lambda parameter | `_` |
| Loop index | `i`, `j`, `index` |
| Coordinate or component | `x`, `y`, `z`, `w` |

Apart from this table, do not use one- or two-character names. `id` and `io` are whole words and are allowed.

```csharp
public async Task SaveAsync(
    Stream destination,
    CancellationToken cancellationToken
) {
    (int width, int height) size = Measure();
    foreach (AssetEntry entry in m_entries)
    {
        await entry.WriteAsync(destination, size, cancellationToken).ConfigureAwait(false);
    }
}
```

Avoid:

```csharp
public async Task SaveAsync(
    Stream dst,
    CancellationToken token
) {
    (int Width, int Height) sz = Measure();
    foreach (AssetEntry e in m_entries)
    {
        await e.WriteAsync(dst, sz, token).ConfigureAwait(false);
    }
}
```

## NM-13 Acronyms

- A two-letter acronym is upper case wherever a PascalCase word would start: `ID`, `IO`, `UI`. This includes `ID`: write `AssetID`, `persistentID`, `GetID`, never `AssetId`.
- A plural keeps the acronym upper case and adds a lower-case `s`: `sourceIDs`, `GetAssetIDs`.
- An acronym of three or more letters is written as an ordinary word: `Json`, `Http`, `Ktx`, `Sdl`.
- When an acronym starts a camelCase identifier, the whole acronym is lower case: `id`, `ioStatus`, `uiScale`.
- In UPPER_SNAKE names every word is upper case: `C_MAX_ID`.

```csharp
public sealed class JsonAssetWriter
{
    public AssetID persistentID { get; }
    public IOStatus ioStatus { get; }
    public float uiScale { get; set; }

    public AssetID GetID(string id) => m_ids[id];
}
```

Avoid:

```csharp
public sealed class JSONAssetWriter
{
    public AssetId persistentId { get; }
    public IoStatus IOStatus { get; }
    public float UIScale { get; set; }

    public AssetId GetId(string ID) => m_ids[ID];
}
```
