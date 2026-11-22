---
paths: ["**/*.cs"]
tags: [csharp-style]
---
# 4. Type declarations

## T-01 Write every access modifier

Every type and every member states its accessibility, including each file of a `partial` type. Interface members do not write `public`.

```csharp
internal sealed partial class AssetLoader
{
    private int m_generation;
}

public interface IEventSink
{
    void Publish(EventRecord record);
}
```

Avoid:

```csharp
sealed partial class AssetLoader
{
    int m_generation;
}
```

## T-02 Modifier order

Accessibility, then `static`, `extern`, `new`, then `virtual` / `abstract` / `sealed` / `override`, then `readonly`, `unsafe`, `required`, `volatile`, `async`, `partial`.

```csharp
public static readonly Color RED = FromBytes(255, 0, 0);

private static async Task RunAsync()
{
    await Task.Yield();
    Publish();
}

public sealed override string ToString() => name;

public override readonly int GetHashCode() => m_value;

internal static unsafe partial class NativeMethods;
```

Avoid:

```csharp
public readonly override int GetHashCode() => m_value;

static private int s_count;
```

## T-03 Classes are sealed by default

Every class is `static`, `abstract` or `sealed`. Leave a class open only when it is a designed extension point, and document the derivation contract in its XML documentation. Nested private classes are sealed too.

```csharp
public sealed class EventDispatcher;

internal static class PathUtility
{
    internal static string Normalize(string path) => path.Replace('\\', '/');
}

public abstract class EditorDrop;
```

Avoid:

```csharp
public class EventDispatcher;
```

## T-04 Records

- A reference-type data contract is a `sealed record` with positional parameters, ending in `;`.
- A small value type is a `readonly record struct`.
- Positional parameters are properties, so they are camelCase.

```csharp
public sealed record AssetChangedEvent(
    string path,
    WatcherChangeTypes change
);

public readonly record struct AssetID(Guid value);
```

Avoid:

```csharp
public record AssetChangedEvent(string Path, WatcherChangeTypes Change);

public record struct AssetID(Guid value);
```

## T-05 Structs

A struct is `readonly` by default. Use a mutable struct only for interop layouts and math types that are assigned field by field; its fields are public and camelCase.

```csharp
public readonly struct FrameRange
{
    public int start { get; }
    public int count { get; }
}

[StructLayout(LayoutKind.Sequential)]
internal struct GlyphVertex
{
    public Vector2 position;
    public Vector2 uv;
}
```

Avoid (a mutable struct that is not an interop or math type):

```csharp
public struct FrameRange
{
    public int start { get; set; }
    public int count { get; set; }
}
```

## T-06 Base lists and generic constraints on types

- The base list stays on the line of the type name.
- Each `where` clause of a type is on its own line, indented four spaces. The opening brace stays on its own line.

```csharp
public abstract class EditorDrop<TSource, TTarget> : EditorDrop
    where TSource : IdentityObject
    where TTarget : class
{
    public abstract bool Accepts(TSource source);
}
```

Avoid:

```csharp
public abstract class EditorDrop<TSource, TTarget>
    : EditorDrop where TSource : IdentityObject where TTarget : class
{
    public abstract bool Accepts(TSource source);
}
```

## T-07 Primary constructors

- A record uses a primary constructor. A record whose constructor validates or normalizes its arguments uses an ordinary constructor, and a record populated through `init` or `required` properties declares none.
- A class or struct may use a primary constructor to receive its dependencies. When it does, it uses the parameters directly and does not copy them into `m_` fields.
- Use an ordinary constructor with `m_` fields when arguments need validation or the fields must be `readonly`.
- Primary constructor parameters follow P-01. A base list follows the closing parenthesis as `) : Base`, with the opening brace on its own line. Without a base list the type body starts with `) {`.

```csharp
internal sealed class TogglePlayModeAction(EditorPlayMode playMode) : EditorAction
{
    protected override bool CanExecute() => playMode.state != EditorPlayModeState.Compiling;
}

internal sealed class SceneReloadChange(
    RuntimeSession runtimeSession,
    ISceneReloadStateTransfer migration
) : IGenerationChange
{
    public void Apply() => migration.Transfer(runtimeSession);
}

internal sealed class FrameScope(
    RenderGraph graph,
    int frameIndex
) {
    public bool isFirst => frameIndex == 0;

    public void Begin() => graph.BeginFrame(frameIndex);
}
```

Avoid (base list on a separate line, parameters copied into fields):

```csharp
internal sealed class SceneReloadChange(
    RuntimeSession runtimeSession,
    ISceneReloadStateTransfer migration
)
    : IGenerationChange
{
    private readonly RuntimeSession m_runtimeSession = runtimeSession;
    private readonly ISceneReloadStateTransfer m_migration = migration;
}
```

## T-08 Nested types

A nested type is placed at the end of its containing type. Nest a type only when the containing type is its only user.

```csharp
public sealed class AssetCache
{
    private readonly Dictionary<AssetID, Entry> m_entries = [];

    public int count => m_entries.Count;

    private sealed class Entry
    {
        public AssetHandle handle { get; init; }
        public int generation { get; set; }
    }
}
```

Avoid (the nested type comes first):

```csharp
public sealed class AssetCache
{
    private sealed class Entry
    {
        public AssetHandle handle { get; init; }
        public int generation { get; set; }
    }

    private readonly Dictionary<AssetID, Entry> m_entries = [];
}
```
