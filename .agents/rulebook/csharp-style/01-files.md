---
paths: ["**/*.cs"]
tags: [csharp-style, formatting]
---
# 1. Files and physical layout

## F-01 Encoding, line endings and indentation

- UTF-8 without a byte order mark.
- LF line endings.
- Four spaces per indentation level. Never use tab characters.
- No trailing whitespace.
- The file ends with exactly one newline.
- At most one consecutive blank line.

In the next two blocks `→` is a tab, `·` is a space and `¶` is a newline.

```text
····private int m_count;¶
}¶
```

Avoid (tab indentation, trailing whitespace, extra blank line at the end of the file):

```text
→private int m_count;··¶
}¶
¶
```

## F-02 One top-level type per file, named after the type

- The file name is the PascalCase name of its primary type: `EventDispatcher.cs` contains `EventDispatcher`.
- Generic arity is not part of the file name: `Pool<T>` lives in `Pool.cs`.
- Additional files of a `partial` type are named `Type.Part.cs`.
- A small, tightly coupled contract (an enum, delegate or parameter record used only by the primary type) may share the file. The primary type comes first.

```text
Inno.Core.Events/
    EventDispatcher.cs              class EventDispatcher
    EventDispatcher.Observation.cs  partial class EventDispatcher
    IEventSink.cs                   interface IEventSink
```

## F-03 File structure

A file contains, in this order: the using directives, one blank line, the file-scoped namespace, one blank line, the types. There is no file header comment, no license banner and no leading blank line.

```csharp
using System;
using System.Collections.Generic;

namespace Inno.Core.Events;

/// <summary>
/// Dispatches events to registered sinks.
/// </summary>
public sealed class EventDispatcher
{
    private readonly List<IEventSink> m_sinks = [];
}
```

Avoid (no blank line after the namespace):

```csharp
namespace Inno.Core.Events;
public sealed class EventDispatcher
{
    private readonly List<IEventSink> m_sinks = [];
}
```

## F-04 Line width

- Aim for 120 columns or fewer. 140 columns is the hard limit.
- Only an unsplittable string literal (a path, a URL, one diagnostic sentence) may exceed the hard limit.
- Do not split a name, a modifier list or a short expression body merely to shorten a line.

```csharp
// 107 columns: stays on one line.
public bool isMissing => m_source is null || !m_source.exists || m_source.generation != m_owner.generation;

// 145 columns on one line: break it by meaning.
throw new InvalidOperationException(
    $"The importer '{importer.name}' produced no result for '{path}' while generation {generation} was active."
);
```

Avoid (a short declaration split for no reason):

```csharp
public bool isMissing
    => m_source is null;
```

## F-05 Indentation and alignment of continuation lines

Prefer alignment over a fixed indent.

- **Operator continuation.** When an expression is broken at an operator (`&&`, `||`, `??`, `+`, `?`, `:`, `or`, `and`, …), each continuation line begins with the operator and aligns with the first operand of that expression.
- **Fallback.** When alignment would push a line past the hard limit, indent the continuation lines four spaces from the start of the statement instead.
- **Delimited lists.** The contents of a broken parameter list, argument list, initializer or collection are indented four spaces from the line that opens them. The closing delimiter, when it is on its own line, aligns with the first token of the opening line.
- **Call chains, wrapped `=>` and clauses.** A wrapped `.Member(...)` link, a wrapped expression-body `=>`, and a wrapped `where` or constructor-initializer clause are indented four spaces from the start of the statement or declaration.

```csharp
bool changed = previous.width != current.width
               || previous.height != current.height
               || previous.format != current.format;

return left.order != right.order
       || left.layer != right.layer;

string name = entry.displayName
              ?? entry.path.GetFileName()
              ?? string.Empty;

List<string> names = m_entries
    .Where(static entry => !entry.isMissing)
    .Select(static entry => entry.displayName)
    .ToList();
```

Avoid (operator continuation indented by a fixed amount instead of aligned):

```csharp
bool changed = previous.width != current.width
    || previous.height != current.height
    || previous.format != current.format;
```
