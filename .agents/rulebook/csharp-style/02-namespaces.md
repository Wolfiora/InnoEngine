---
paths: ["**/*.cs"]
tags: [csharp-style]
---
# 2. Namespaces and using directives

## N-01 File-scoped namespaces

Use `namespace X;`. A file declares exactly one namespace.

```csharp
namespace Inno.Core.Events;

public sealed class EventDispatcher
{
    private int m_generation;
}
```

Avoid:

```csharp
namespace Inno.Core.Events
{
    public sealed class EventDispatcher
    {
        private int m_generation;
    }
}
```

## N-02 The namespace is the project name

The namespace starts with `Inno`, uses PascalCase segments and equals the assembly name of the owning project. Directories inside a project organize files; they do not add namespace segments.

```csharp
// File: src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs
namespace Inno.Assets.Pipeline;
```

Avoid:

```csharp
// File: src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs
namespace Inno.Assets.Pipeline.Importing;
```

The one registered exception is `Inno.Editor.ImGui/Widgets`, which uses `Inno.Editor.ImGui.ImGuiWidget`.

## N-03 Placement and kinds of using directives

- All using directives are at the top of the file, outside the namespace.
- `global using`, implicit usings and MSBuild `Using` items are prohibited. Handwritten code never uses `global::`.
- `using static` is reserved for pure function containers such as math helpers.

```csharp
using System;
using Inno.Core.Events;

namespace Inno.Rendering;
```

Avoid:

```csharp
global using System;

namespace Inno.Rendering;

using Inno.Core.Events;

public sealed class Renderer
{
    private readonly global::Inno.Core.Events.EventDispatcher m_dispatcher = new();
}
```

## N-04 Order of using directives

Using directives form up to three groups, in this order, separated by exactly one blank line:

1. Namespace usings.
2. `using static` directives.
3. Alias usings.

Inside the namespace group the order is:

1. `System` and `System.*`.
2. Everything else that is not `Inno` (third-party and framework packages).
3. `Inno` and `Inno.*`.

Each tier is sorted alphabetically (ordinal). There are no blank lines inside a group. Static usings follow the same three tiers; aliases are sorted by alias name.

```csharp
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using Xunit;
using Inno.Core.Events;
using Inno.Core.Identity;

using static System.MathF;

using AssetEvent = Inno.Assets.Pipeline.AssetChangedEvent;
using NativeWindow = Inno.Adapter.Platform.Sdl3.Sdl3Window;
```

Avoid (`Inno` before third-party namespaces, a blank line inside the namespace group, an alias among the namespace usings, no blank line between kinds):

```csharp
using Inno.Core.Events;
using AssetEvent = Inno.Assets.Pipeline.AssetChangedEvent;
using Xunit;

using System;
using System.Collections.Generic;
using static System.MathF;
```

## N-05 No fully qualified names at the point of use

Import a type with a using directive and refer to it by its short name. When two names collide, declare one alias at the top of the file and use only that alias throughout the file.

```csharp
using System.IO;

using AssetEvent = Inno.Assets.Pipeline.AssetChangedEvent;

namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetLoaderTests
{
    private static AssetEvent Deleted(string path) => new(path, WatcherChangeTypes.Deleted);
}
```

Avoid:

```csharp
namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetLoaderTests
{
    private static Inno.Assets.Pipeline.AssetChangedEvent Deleted(string path)
        => new(path, System.IO.WatcherChangeTypes.Deleted);
}
```

## N-06 Alias names

An alias is PascalCase, names the role the type plays in this file and differs from the short name of its target. One target has one alias per file.

```csharp
using NativeWindow = Inno.Adapter.Platform.Sdl3.Sdl3Window;
```

Avoid (the alias repeats the target name, and a second alias names the same target):

```csharp
using Sdl3Window = Inno.Adapter.Platform.Sdl3.Sdl3Window;
using Window = Inno.Adapter.Platform.Sdl3.Sdl3Window;
```
