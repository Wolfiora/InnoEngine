# Inno.Adapter.Text.FreeTypeHarfBuzz

[分类索引](README.md) · [Native Text](Inno.Native.Text.md) · [Wiki 首页](../../README.md)

默认 Text backend 用 HarfBuzz 做 Unicode shaping，用 FreeType 提取字形 bitmap。具体 ABI 限定在此 adapter 与 `Inno.Native.Text`，Session/脚本不接触原生指针。后端对字体 bytes 创建/缓存 face，并按运行期所有权释放。

公开 `FreeTypeHarfBuzzTextBackend()` 初始化一个原生 context，并实现 `ITextBackend` 的 `LoadFont`、`ReleaseFont`、`Shape`、`Rasterize`、`Dispose`。输入字节复制/固定在调用边界，结果转换为中立 `TextLayout`/`GlyphBitmap`。构造、数据校验或 native 调用失败时抛出明确异常；`Dispose` 幂等，之后的方法拒绝访问。

## 使用与边界

Host 由 [默认 adapter catalog](../../runtime/Inno.Adapter.Default.md) 创建此 backend，再交给 `TextRuntime` 持有。`LoadFont(ReadOnlySpan<byte>, faceIndex)` 允许 OpenType collection 中的显式 face；`Shape` 处理 Unicode、语言/script/方向，`Rasterize` 输出 8-bit coverage，而不是 GPU texture。`ReleaseFont` 后或跨 backend 使用旧 handle 会失败。

```csharp
using System;
using Inno.Adapter.Text.FreeTypeHarfBuzz;
using Inno.Text;

static TextLayout ShapeText(ReadOnlySpan<byte> fontBytes)
{
    using var backend = new FreeTypeHarfBuzzTextBackend();
    TextFontHandle face = backend.LoadFont(fontBytes, 0);
    try
    {
        return backend.Shape(face, "界面", new TextStyle(24f), default);
    }
    finally
    {
        backend.ReleaseFont(face);
    }
}
```

此代码属于 Host/adapter 测试层；游戏脚本应使用 `InnoEngine.Text.Text`。详情参见 [Text Runtime](../../text/Inno.Text.Runtime.md)。

## Composition provider

`FreeTypeHarfBuzzTextBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateBackend()` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。

## 源码归属

当前唯一源码 owner：`backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/Inno.Adapter.Text.FreeTypeHarfBuzz.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend`

<!-- api: T:Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend dc42031ec2e296afdced09f095f6e26b5ee4a711382b1d4789a39b808b49c390 -->
<!-- api: M:Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.#ctor 558746113da085815022aa837debbef48d21cf8ff2fee8ce6eb99cb1f755fcf3 -->
<!-- api: M:Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.LoadFont(System.ReadOnlySpan{System.Byte},System.Int32) 3ac184337b127622689ff8efd60ac4bff8a2b55ce25f752aaddcd5b08e12e62a -->
<!-- api: M:Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.ReleaseFont(Inno.Text.TextFontHandle) 3b2bceeb773d48c5f7fffe43d680474ae0797abd889df65472f7795e95486616 -->
<!-- api: M:Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.Shape(Inno.Text.TextFontHandle,System.String,Inno.Text.TextStyle,Inno.Text.TextShapingOptions) 2479ed462bc8e26446d48f7b26068aa49e6c0679051ed1ac09bfb9ff9662cdcc -->
<!-- api: M:Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.Rasterize(Inno.Text.TextFontHandle,System.UInt32,System.Single) 485d8d94b924b331b22447bec3e1cc6be7fb0fcda1ed7974ddc8012d964dd1c4 -->
<!-- api: M:Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.Dispose 067cdbba486db223c2be0ad14347eeb3738d8ef0e122ee6eb659ae9f9ec1c116 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ad0e224e242a3d2f"></a>[`public Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.FreeTypeHarfBuzzTextBackend()`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L20) | Creates and validates a native text context. | — | — |
| <a id="api-65d00c25bf9536d9"></a>[`public void Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.Dispose()`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L215) | Releases the native text context and every remaining face. | — | — |
| <a id="api-42747f84b42a4d4d"></a>[`public Inno.Text.TextFontHandle Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.LoadFont(System.ReadOnlySpan<byte> data, int faceIndex)`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L40) | Loads one face from encoded OpenType data.<br>返回：A generation-local face handle.<br>`data`：The complete encoded font source.<br>`faceIndex`：The zero-based collection face index. | — | — |
| <a id="api-1795679268cbe250"></a>[`public Inno.Text.GlyphBitmap Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.Rasterize(Inno.Text.TextFontHandle font, uint glyphId, float fontSize)`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L182) | Rasterizes one shaped glyph into 8-bit coverage.<br>返回：The immutable glyph bitmap.<br>`font`：The loaded face.<br>`glyphId`：The font-specific glyph identifier.<br>`fontSize`：The positive logical pixel size. | — | — |
| <a id="api-5eee554dba608f94"></a>[`public void Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.ReleaseFont(Inno.Text.TextFontHandle font)`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L59) | Releases one loaded face.<br>`font`：The face handle to release. | — | — |
| <a id="api-9d5e216ef0865019"></a>[`public Inno.Text.TextLayout Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.Shape(Inno.Text.TextFontHandle font, string text, Inno.Text.TextStyle style, Inno.Text.TextShapingOptions options)`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L85) | Shapes one Unicode string into positioned glyphs.<br>返回：The immutable layout.<br>`font`：The loaded face.<br>`text`：The Unicode source text.<br>`style`：Font size and spacing.<br>`options`：Language, script, and direction hints. | — | — |
| <a id="api-a7499741085fd46b"></a>[`public sealed class Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend : Inno.Text.ITextBackend`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L12) | Implements Unicode shaping and glyph rasterization through pinned FreeType and HarfBuzz sources. | — | — |

### `Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider`

<!-- api: T:Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider 7a1475aebf461f77cf98dcca98a2a27bf4ebcc0e1aa5f2b248a969214255ed18 -->
<!-- api: M:Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider.#ctor e5942de34737bfebc84d504b03968b151de1a6398333db658f7e5b9726a63f45 -->
<!-- api: M:Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider.CreateBackend 38a87f6e7844c307e251168118163de348971750190bd45953c1d9f80f93ed7b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9f892dfd0d00cc0d"></a>[`public Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider.FreeTypeHarfBuzzTextBackendProvider()`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. | — | — |
| <a id="api-4014dd5908b07829"></a>[`public override Inno.Text.ITextBackend Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider.CreateBackend()`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackendProvider.cs#L17) | Creates a caller-owned text service using this implementation.<br>返回：A new service owned by the caller; providers must not return null. | — | — |
| <a id="api-0eb247a373bd5fd5"></a>[`public sealed class Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider : Inno.Adapter.Text.TextBackendProvider`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackendProvider.cs#L9) | Supplies the FreeTypeHarfBuzz implementation through the neutral text creation boundary. | — | — |

## 项目依赖

- [Inno.Adapter.Text](../../text/Inno.Adapter.Text.md)：公开引用边界由实际签名核对。
- [Inno.Text](../../text/Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.Native.Text](Inno.Native.Text.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
