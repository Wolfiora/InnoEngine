---
paths: ["**/*.cs"]
tags: [csharp-style, formatting]
---
# 9. Line breaks

## W-01 Binary operators lead the continuation line

When an expression breaks at `&&`, `||`, `??`, `+`, `-`, `|` or another binary operator, the operator starts the next line. Continuation lines align with the first operand (F-05).

```csharp
bool changed = previous.width != current.width
               || previous.height != current.height
               || previous.format != current.format;

string message = "The asset database could not be opened because "
                 + "another editor instance holds the workspace lock.";

string name = entry.displayName
              ?? entry.path.GetFileName()
              ?? string.Empty;
```

Avoid:

```csharp
bool changed = previous.width != current.width ||
    previous.height != current.height;
```

## W-02 Conditional expressions

A conditional expression stays on one line when it fits. Otherwise `?` and `:` each start a line, aligned with the condition (F-05). Do not nest conditional expressions; use a `switch` expression or an `if` statement.

```csharp
int order = byName != 0 ? byName : left.order.CompareTo(right.order);

ShaderCompilationResult result = pending.IsCompletedSuccessfully
                                 ? pending.Result
                                 : new ShaderCompilationResult(null, diagnostics);
```

Avoid:

```csharp
ShaderCompilationResult result = pending.IsCompletedSuccessfully ?
    pending.Result :
    new ShaderCompilationResult(null, diagnostics);

string label = count == 0 ? "none" : count == 1 ? "one" : "many";
```

## W-03 Call chains

A call chain stays on one line when it fits. Otherwise the first link stays on the statement line and every following `.` (or `?.`) starts a line, indented four spaces from the start of the statement.

```csharp
List<string> names = m_entries
    .Where(static entry => !entry.isMissing)
    .Select(static entry => entry.displayName)
    .OrderBy(static name => name, StringComparer.Ordinal)
    .ToList();
```

Avoid:

```csharp
List<string> names = m_entries.Where(static entry => !entry.isMissing).
    Select(static entry => entry.displayName).
    ToList();
```

## W-04 Assignments and arrows

- Never break a line directly before or after `=`. Keep the start of the right-hand side on the line of the `=` and break inside it: after an opening parenthesis (A-02), before an operator (W-01) or before a `.` (W-03). The only exception is the opening bracket of a multi-line collection expression, which takes its own line (I-03).
- A lambda `=>` always ends its line when the body moves down; the body then follows B-04. An expression body that is not a call is wrapped in parentheses.
- The `=>` of a `switch` expression arm stays on the line of the arm.
- The `=>` of an expression-bodied member follows M-03.

```csharp
string intermediate = scoped.GetNativeBuildRoot(
    componentDescriptor,
    configuration,
    targetArchitecture
);

Func<AssetEntry, bool> isStale = entry => (
    entry.lastWriteTimeUtc < threshold && !entry.isPinned
);
```

Avoid:

```csharp
string intermediate =
    scoped.GetNativeBuildRoot(componentDescriptor, configuration, targetArchitecture);

Func<AssetEntry, bool> isStale = entry =>
    entry.lastWriteTimeUtc < threshold && !entry.isPinned;
```

## W-05 Multi-line conditions and statement headers

- `is` stays on the line of its left operand. `or` and `and` start their continuation lines.
- When the parenthesized header of `if`, `else if`, `while`, `for`, `foreach`, `switch`, `lock`, `using` or `fixed` spans several lines, its closing parenthesis is on its own line, aligned with the keyword, and the opening brace follows it: `) {`. This mirrors the multi-line parameter list (B-02).

```csharp
if (token.kind is TokenKind.Comma
    or TokenKind.Semicolon
    or TokenKind.CloseBrace
) {
    return;
}

if (capabilities != EditorViewportNavigationCapabilities.None
    && !SupportsMode(capabilities, defaultMode)
) {
    throw new ArgumentException("The default mode is not supported.", nameof(defaultMode));
}

foreach (KeyValuePair<string, string> dependency in HostDependencyManifest
    .GetInnoRuntimeAssemblies(roots)
    .OrderBy(static dependency => dependency.Key, StringComparer.OrdinalIgnoreCase)
) {
    Enqueue(dependency.Key, dependency.Value);
}
```

Avoid:

```csharp
if (token.kind is TokenKind.Comma
    or TokenKind.Semicolon
    or TokenKind.CloseBrace)
{
    return;
}

if (capabilities != EditorViewportNavigationCapabilities.None
    && !SupportsMode(capabilities, defaultMode))
    throw new ArgumentException("The default mode is not supported.", nameof(defaultMode));
```
