---
paths: ["**/*.cs"]
tags: [csharp-style, formatting]
---
# 10. Initializers and collections

## I-01 Object initializers

- A short initializer stays on one line with one space inside each brace: `new Foo { a = 1 }`.
- Otherwise the opening brace is on its own line, aligned with the start of the statement; each member is on its own line, indented four spaces; the closing brace is on its own line.
- Omit the empty `()` when an initializer is present.
- `with` expressions and anonymous objects follow the same rule.

```csharp
var options = new JsonSerializerOptions { WriteIndented = true };

var request = new GameBuildRequest
{
    profile = profile,
    outputDirectory = outputDirectory,
    configuration = BuildConfiguration.Release
};

RenderSettings updated = settings with
{
    exposure = 1.5f,
    toneMapping = ToneMapping.Aces
};
```

Avoid:

```csharp
var request = new GameBuildRequest() {
    profile = profile, outputDirectory = outputDirectory,
    configuration = BuildConfiguration.Release,
};
```

## I-02 Collections use collection expressions

- Arrays, `List<T>`, spans and immutable collections are written as collection expressions: `[a, b]`, `[]`, `[.. source]`.
- A spread operator is followed by one space: `[.. source, extra]`.
- Use `new T(...)` when a constructor argument (a capacity, a comparer) is needed, and `new T[n]` for a fixed-size buffer.
- Do not write `new[] { ... }`, `new T[] { ... }`, `new List<T> { ... }` or `Array.Empty<T>()`.

```csharp
List<RenderItem> items = [];
string[] extensions = [".png", ".jpg", ".ktx"];
List<string> all = [.. builtIn, .. custom, "fallback"];
var byName = new Dictionary<string, AssetEntry>(StringComparer.Ordinal);
var buffer = new byte[C_BUFFER_SIZE];
```

Avoid:

```csharp
List<RenderItem> items = new List<RenderItem>();
string[] extensions = new[] { ".png", ".jpg", ".ktx" };
List<string> all = [..builtIn, ..custom, "fallback"];
string[] none = Array.Empty<string>();
```

## I-03 Multi-line collection expressions

- With two or more elements that do not fit on one line: one element per line, indented four spaces, and the closing bracket on its own line.
- As the value of a declaration or assignment, the opening bracket is on its own line, aligned with the start of the statement, like the brace of an object initializer.
- As an argument, the opening bracket follows the opening parenthesis or the preceding argument (A-03).
- A single element that itself spans several lines hugs the brackets: `[new X(` … `)]`.

```csharp
string[] searchPaths =
[
    Path.Combine(root, "Assets"),
    Path.Combine(root, "Packages"),
    Path.Combine(root, "Library", "Imported")
];

loader.ApplySourceChanges([
    new AssetEvent("Text/new.txt", WatcherChangeTypes.Deleted),
    new AssetEvent("Text/old.txt", WatcherChangeTypes.Deleted)
]);

return new ShaderCompilationResult(
    null,
    [new ShaderDiagnostic(
        "SHADER_COMPILE_EXCEPTION",
        DiagnosticSeverity.Error,
        message
    )]
);
```

Avoid:

```csharp
string[] searchPaths = [Path.Combine(root, "Assets"),
    Path.Combine(root, "Packages"), Path.Combine(root, "Library", "Imported")];

return new ShaderCompilationResult(
    null,
    [new ShaderDiagnostic(
        "SHADER_COMPILE_EXCEPTION",
        DiagnosticSeverity.Error,
        message)]);
```

## I-04 `switch` expressions

`value switch` is on one line; the opening brace is on its own line; each arm is on its own line with its `=>`; the discard arm is last.

```csharp
string label = state switch
{
    EditorPlayModeState.Editing => "Enter Play Mode",
    EditorPlayModeState.Compiling => "Compiling",
    EditorPlayModeState.Playing => "Exit Play Mode",
    _ => throw new ArgumentOutOfRangeException(nameof(state))
};
```

Avoid:

```csharp
string label = state switch { EditorPlayModeState.Editing => "Enter", _ => "Exit", };
```

## I-05 No trailing commas

The last item of an object initializer, a collection, an enum, a `switch` expression or an anonymous object is not followed by a comma.

```csharp
public enum ToneMapping
{
    None,
    Reinhard,
    Aces
}
```

Avoid:

```csharp
public enum ToneMapping
{
    None,
    Reinhard,
    Aces,
}
```
