---
paths: ["**/*.cs"]
tags: [csharp-style, xml]
---
# 14. Comments and XML documentation

## D-01 Which members need XML documentation

Every externally visible public member, and every protected member that an external type can override, has complete English XML documentation. Important internal APIs are documented as well. Private members and test code do not need documentation.

```csharp
/// <summary>
/// Stores render items until the next flush.
/// </summary>
public sealed class RenderQueue
{
    private readonly List<RenderItem> m_items = [];

    /// <summary>
    /// Gets the number of queued items.
    /// </summary>
    public int count => m_items.Count;

    private void Trim() => m_items.TrimExcess();
}
```

Avoid (a public type and a public property without documentation):

```csharp
public sealed class RenderQueue
{
    public int count => m_items.Count;
}
```

## D-02 Tags are expanded, one element per block

- The start tag, the content and the end tag are on separate lines. `///` is followed by one space. A documentation block has no empty `///` lines.
- Tag order: `summary`, `typeparam`, `param` (in signature order), `returns`, `exception`, `remarks`.
- Every content block starts with a capital letter and ends with a period.
- Wrap long content at complete semantic boundaries.

```csharp
/// <summary>
/// Resolves a loaded asset by its persistent identity.
/// </summary>
/// <typeparam name="TAsset">
/// The asset type expected by the caller.
/// </typeparam>
/// <param name="id">
/// The persistent identity of the asset.
/// </param>
/// <param name="asset">
/// Receives the asset when this method returns <see langword="true"/>;
/// otherwise <see langword="null"/>.
/// </param>
/// <returns>
/// <see langword="true"/> when the asset is loaded and has the requested type;
/// otherwise <see langword="false"/>.
/// </returns>
/// <exception cref="ObjectDisposedException">
/// The loader has been disposed.
/// </exception>
public bool TryResolve<TAsset>(
    AssetID id,
    [NotNullWhen(true)] out TAsset? asset
) where TAsset : class
{
    ObjectDisposedException.ThrowIf(m_disposed, this);
    return TryResolveCore(id, out asset);
}
```

Avoid:

```csharp
/// <summary>Resolves an asset</summary>
/// <param name="asset">the asset</param>
/// <param name="id">the id</param>
/// <returns>true if found</returns>
public bool TryResolve<TAsset>(
    AssetID id,
    [NotNullWhen(true)] out TAsset? asset
) where TAsset : class
{
    ObjectDisposedException.ThrowIf(m_disposed, this);
    return TryResolveCore(id, out asset);
}
```

## D-03 Wording of summaries

| Member | Starts with | Example |
| --- | --- | --- |
| Read-only property | `Gets` | `Gets the current width in pixels.` |
| Writable property | `Gets or sets` | `Gets or sets the clear color.` |
| `bool` property | `Gets a value indicating whether` | `Gets a value indicating whether the asset is missing.` |
| Method | A third-person verb | `Creates`, `Releases`, `Determines whether`, `Attempts to` |
| Constructor | `Initializes a new instance of` or `Creates` | `Creates a dispatcher bound to the given hub.` |
| Type | A third-person verb or a noun phrase | `Identifies`, `Describes`, `Provides`, `Represents` |
| Event | `Occurs when` | `Occurs when the platform requests a redraw.` |

A summary describes real behavior. It does not restate the member name.

```csharp
/// <summary>
/// Gets the current width of the viewport in pixels.
/// </summary>
public int width => m_width;
```

Avoid:

```csharp
/// <summary>
/// The width.
/// </summary>
public int width => m_width;
```

## D-04 References inside documentation

- A type or member: `<see cref="AssetLoader"/>`. A parameter: `<paramref name="id"/>`. A type parameter: `<typeparamref name="TAsset"/>`.
- A keyword: `<see langword="null"/>`, `<see langword="true"/>`, `<see langword="false"/>`.
- A self-closing tag has no space before `/>`.
- An override or interface implementation uses `/// <inheritdoc/>` and documents any additional constraint locally with `remarks` or `exception`.

```csharp
/// <inheritdoc/>
public override string ToString() => name;
```

Avoid:

```csharp
/// <summary>
/// Returns null if id is not found. See AssetLoader.
/// </summary>
/// <inheritdoc />
public override string ToString() => name;
```

## D-05 `//` comments

- A comment explains a reason, a constraint, ownership or a lifetime. It does not restate the code.
- A comment is on its own line, above the code it explains, at the indentation of that code. Do not write trailing comments.
- `//` is followed by one space. The comment is a complete English sentence that starts with a capital letter and ends with a period.

```csharp
// The native window must outlive the swap chain, so it is released last.
m_swapChain.Dispose();
m_window.Dispose();
```

Avoid:

```csharp
m_swapChain.Dispose(); // dispose swap chain
//release window
m_window.Dispose();
```

## D-06 Prohibited comments and directives

- Comments in any language other than English.
- `TODO`, `FIXME` or `HACK` markers left in place indefinitely.
- Commented-out code.
- Separator lines, banner comments and `/* */` block comments.
- `#region` and `#endregion`.
- `#nullable` inside a file, and `#pragma warning disable` without a comment that states the reason.

`#if`, `#elif`, `#else` and `#endif` start at column 0. Conditional symbols are UPPER_SNAKE with the `INNO_` prefix, except `DEBUG`.

```csharp
#if INNO_HOST_WINDOWS_X64
    private const string C_LIBRARY_NAME = "bgfx.dll";
#elif INNO_HOST_MACOS_ARM64
    private const string C_LIBRARY_NAME = "libbgfx.dylib";
#endif
```

Avoid:

```csharp
#region Constants
    // ---------- library ----------
    // TODO: revisit
    // private const string C_OLD_NAME = "bgfx_old.dll";
    private const string C_LIBRARY_NAME = "bgfx.dll"; /* windows */
#endregion
```
