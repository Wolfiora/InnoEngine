# Inno.Adapter.Text.FreeTypeHarfBuzz

[分类索引](README.md) · [Native Text](Inno.Native.Text.md) · [Wiki 首页](../../README.md)

默认 Text backend 用 HarfBuzz 做 Unicode shaping，用 FreeType 提取字形 bitmap。具体 ABI 限定在此 adapter 与 `Inno.Native.Text`，Session/脚本不接触原生指针。后端对字体 bytes 创建/缓存 face，并按运行期所有权释放。

公开 `FreeTypeHarfBuzzTextBackend()` 初始化一个原生 context，并实现 `ITextBackend` 的 `LoadFont`、`ReleaseFont`、`Shape`、`Rasterize`、`Dispose`。输入字节复制/固定在调用边界，结果转换为中立 `TextLayout`/`GlyphBitmap`。构造、数据校验或 native 调用失败时抛出明确异常；`Dispose` 幂等，之后的方法拒绝访问。

## 使用与边界

Host 由 [默认 adapter catalog](../../runtime/Inno.Adapter.Default.md) 创建此 backend，再交给 `TextRuntime` 持有。`LoadFont(ReadOnlySpan<byte>, faceIndex)` 允许 OpenType collection 中的显式 face；`Shape` 处理 Unicode、语言/script/方向，`Rasterize` 输出 8-bit coverage，而不是 GPU texture。`ReleaseFont` 后或跨 backend 使用旧 handle 会失败。

```csharp
using Inno.Adapter.Text.FreeTypeHarfBuzz;
using Inno.Text;

using var backend = new FreeTypeHarfBuzzTextBackend();
TextFontHandle face = backend.LoadFont(fontBytes, 0);
TextLayout layout = backend.Shape(face, "界面", new TextStyle(24f), default);
backend.ReleaseFont(face);
```

此代码属于 Host/adapter 测试层；游戏脚本应使用 `InnoEngine.Text.Text`。详情参见 [Text Runtime](../../text/Inno.Text.Runtime.md)。

## Composition provider

`FreeTypeHarfBuzzTextBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateBackend()` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。

## 源码归属

当前唯一源码 owner：`backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/Inno.Adapter.Text.FreeTypeHarfBuzz.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L12) | Implements Unicode shaping and glyph rasterization through pinned FreeType and HarfBuzz sources. |
| [`Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.FreeTypeHarfBuzzTextBackend()`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L20) | Creates and validates a native text context. |
| [`Inno.Text.GlyphBitmap Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.Rasterize(Inno.Text.TextFontHandle font, uint glyphId, float fontSize)`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L182) | Rasterizes one shaped glyph into 8-bit coverage. |
| [`Inno.Text.TextFontHandle Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.LoadFont(System.ReadOnlySpan<byte> data, int faceIndex)`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L40) | Loads one face from encoded OpenType data. |
| [`Inno.Text.TextLayout Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.Shape(Inno.Text.TextFontHandle font, string text, Inno.Text.TextStyle style, Inno.Text.TextShapingOptions options)`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L85) | Shapes one Unicode string into positioned glyphs. |
| [`void Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.Dispose()`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L215) | Releases the native text context and every remaining face. |
| [`void Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackend.ReleaseFont(Inno.Text.TextFontHandle font)`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackend.cs#L59) | Releases one loaded face. |

### `Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackendProvider.cs#L9) | Supplies the FreeTypeHarfBuzz implementation through the neutral text creation boundary. |
| [`Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider.FreeTypeHarfBuzzTextBackendProvider()`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. |
| [`override Inno.Text.ITextBackend Inno.Adapter.Text.FreeTypeHarfBuzz.FreeTypeHarfBuzzTextBackendProvider.CreateBackend()`](../../../backends/Text/runtime/Inno.Adapter.Text.FreeTypeHarfBuzz/FreeTypeHarfBuzzTextBackendProvider.cs#L17) | See the implemented contract. |

## 项目依赖

- [Inno.Adapter.Text](../../text/Inno.Adapter.Text.md)：公开引用边界由实际签名核对。
- [Inno.Text](../../text/Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.Native.Text](Inno.Native.Text.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
