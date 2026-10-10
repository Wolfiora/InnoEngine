# Inno.Text

[Text 索引](README.md) · [UI](../ui/Inno.UI.md)

后端中立契约位于 `src/services/text/Inno.Text`。`FontAsset` 保存可恢复资产身份与导入 metadata；`TextStyle`、`TextShapingOptions` 描述字号、字重、方向、语言等输入。`TextLayout` 返回字形与位置，`GlyphBitmap` 返回像素和度量，不包含 GPU、Scene 或 RmlUi 对象。

`ITextService.Shape` 与 `Rasterize` 是实际能力；脚本通过 `Text` 门面在活动 `TextExecutionContext` 内调用。没有 Session scope 会明确失败。唯一 `Properties/ScriptingApi.cs` 声明逻辑 `InnoEngine.Text` 暴露面；不导出 `ITextBackend`、native handle 或 adapter。

该层只依赖 Assets 与 Foundation；绘制、字形图集、Canvas 组件仍属于消费者。

## 公开类型与使用

| 类型 | 语义 |
| --- | --- |
| `FontAsset`、`FontMetadata`、`FontMetadataCodec` | 字体资产、face 数/编码长度与严格 runtime metadata 编解码；Missing 或无 payload 不会静默变成默认字体。 |
| `TextStyle`、`TextShapingOptions`、`TextDirection`、`TextFontStyle` | 字号、face、字重、字间距、语言/script/方向输入；构造时校验范围及有限浮点。 |
| `TextGlyph`、`TextMetrics`、`TextLayout`、`GlyphBitmap` | 不可变 shaping/光栅化结果，字形 ID 可用于后续 `Rasterize`。 |
| `ITextService`、`Text`、`TextExecutionContext` | 显式服务、脚本门面与严格 LIFO 的 Session 作用域。 |
| `ITextBackend`、`TextFontHandle` | Host/adapter 的低层 face 所有权协议，不属于逻辑脚本 API。 |

```csharp
using InnoEngine.Text;

static TextLayout LayoutTitle(FontAsset font)
    => Text.Shape(font, "你好 Inno", new TextStyle(24f));
```

在活动 Session 帧中调用；`TextLayout.glyphs` 的 ID 与 FontAsset/face 对应，跨后端代际不可直接复用。Text 本身不返回 GPU 纹理或 Scene 组件。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Text.FontAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.FontAsset`](../../src/services/text/Inno.Text/FontAsset.cs#L99) | Represents one imported OpenType font file or collection. |
| [`Inno.Text.FontMetadata? Inno.Text.FontAsset.metadata`](../../src/services/text/Inno.Text/FontAsset.cs#L107) | Gets imported metadata, or null before runtime content is loaded. |
| [`override void Inno.Text.FontAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory<byte> previousPayload, System.ReadOnlyMemory<byte> currentPayload)`](../../src/services/text/Inno.Text/FontAsset.cs#L118) | Refreshes imported metadata after an artifact commit. |

### `Inno.Text.FontMetadata`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.FontMetadata`](../../src/services/text/Inno.Text/FontAsset.cs#L12) | Describes immutable metadata for an imported OpenType font source. |
| [`Inno.Text.FontMetadata.FontMetadata(int faceCount, long encodedByteLength)`](../../src/services/text/Inno.Text/FontAsset.cs#L23) | Creates validated imported font metadata. |
| [`int Inno.Text.FontMetadata.faceCount`](../../src/services/text/Inno.Text/FontAsset.cs#L36) | Gets the number of independently addressable faces in the source. |
| [`long Inno.Text.FontMetadata.encodedByteLength`](../../src/services/text/Inno.Text/FontAsset.cs#L41) | Gets the encoded source length in bytes. |

### `Inno.Text.FontMetadataCodec`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.FontMetadataCodec`](../../src/services/text/Inno.Text/FontAsset.cs#L47) | Encodes the compact runtime payload shared by the font importer and text runtime. |
| [`static Inno.Text.FontMetadata Inno.Text.FontMetadataCodec.Decode(System.ReadOnlySpan<byte> payload)`](../../src/services/text/Inno.Text/FontAsset.cs#L79) | Decodes and validates a compact font runtime payload. |
| [`static byte[] Inno.Text.FontMetadataCodec.Encode(Inno.Text.FontMetadata metadata)`](../../src/services/text/Inno.Text/FontAsset.cs#L61) | Encodes validated metadata into a deterministic runtime payload. |

### `Inno.Text.GlyphBitmap`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.GlyphBitmap`](../../src/services/text/Inno.Text/TextTypes.cs#L301) | Contains one rasterized glyph as tightly packed 8-bit coverage. |
| [`Inno.Text.GlyphBitmap.GlyphBitmap(int width, int height, int bearingX, int bearingY, float advanceX, System.ReadOnlySpan<byte> pixels)`](../../src/services/text/Inno.Text/TextTypes.cs#L326) | Creates a validated immutable glyph bitmap. |
| [`System.ReadOnlyMemory<byte> Inno.Text.GlyphBitmap.pixels`](../../src/services/text/Inno.Text/TextTypes.cs#L371) | Gets tightly packed row-major 8-bit coverage. |
| [`float Inno.Text.GlyphBitmap.advanceX`](../../src/services/text/Inno.Text/TextTypes.cs#L367) | Gets the horizontal glyph advance. |
| [`int Inno.Text.GlyphBitmap.bearingX`](../../src/services/text/Inno.Text/TextTypes.cs#L359) | Gets the horizontal bearing in pixels. |
| [`int Inno.Text.GlyphBitmap.bearingY`](../../src/services/text/Inno.Text/TextTypes.cs#L363) | Gets the vertical bearing in pixels. |
| [`int Inno.Text.GlyphBitmap.height`](../../src/services/text/Inno.Text/TextTypes.cs#L355) | Gets the bitmap height in pixels. |
| [`int Inno.Text.GlyphBitmap.width`](../../src/services/text/Inno.Text/TextTypes.cs#L351) | Gets the bitmap width in pixels. |

### `Inno.Text.ITextBackend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.GlyphBitmap Inno.Text.ITextBackend.Rasterize(Inno.Text.TextFontHandle font, uint glyphId, float fontSize)`](../../src/services/text/Inno.Text/ITextBackend.cs#L75) | Rasterizes one shaped glyph into 8-bit coverage. |
| [`Inno.Text.ITextBackend`](../../src/services/text/Inno.Text/ITextBackend.cs#L8) | Defines the replaceable native shaping and glyph-rasterization boundary. |
| [`Inno.Text.TextFontHandle Inno.Text.ITextBackend.LoadFont(System.ReadOnlySpan<byte> data, int faceIndex)`](../../src/services/text/Inno.Text/ITextBackend.cs#L22) | Loads one face from encoded OpenType data. |
| [`Inno.Text.TextLayout Inno.Text.ITextBackend.Shape(Inno.Text.TextFontHandle font, string text, Inno.Text.TextStyle style, Inno.Text.TextShapingOptions options)`](../../src/services/text/Inno.Text/ITextBackend.cs#L53) | Shapes one Unicode string into positioned glyphs. |
| [`void Inno.Text.ITextBackend.ReleaseFont(Inno.Text.TextFontHandle font)`](../../src/services/text/Inno.Text/ITextBackend.cs#L33) | Releases one loaded face. |

### `Inno.Text.ITextService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.GlyphBitmap Inno.Text.ITextService.Rasterize(Inno.Text.FontAsset font, int faceIndex, uint glyphId, float fontSize)`](../../src/services/text/Inno.Text/ITextService.cs#L51) | Rasterizes one glyph from an imported font. |
| [`Inno.Text.ITextService`](../../src/services/text/Inno.Text/ITextService.cs#L6) | Provides backend-neutral Unicode shaping and glyph rasterization for imported fonts. |
| [`Inno.Text.TextLayout Inno.Text.ITextService.Shape(Inno.Text.FontAsset font, string text, Inno.Text.TextStyle style, Inno.Text.TextShapingOptions options)`](../../src/services/text/Inno.Text/ITextService.cs#L26) | Shapes one Unicode string with an imported font. |

### `Inno.Text.Text`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.Text`](../../src/services/text/Inno.Text/Text.cs#L38) | Provides script-friendly access to Unicode shaping and glyph rasterization. |
| [`static Inno.Text.GlyphBitmap Inno.Text.Text.Rasterize(Inno.Text.FontAsset font, int faceIndex, uint glyphId, float fontSize)`](../../src/services/text/Inno.Text/Text.cs#L106) | Rasterizes one shaped glyph into immutable 8-bit coverage. |
| [`static Inno.Text.TextLayout Inno.Text.Text.Shape(Inno.Text.FontAsset font, string text, Inno.Text.TextStyle style)`](../../src/services/text/Inno.Text/Text.cs#L55) | Shapes text with default automatic language and direction detection. |
| [`static Inno.Text.TextLayout Inno.Text.Text.Shape(Inno.Text.FontAsset font, string text, Inno.Text.TextStyle style, Inno.Text.TextShapingOptions options)`](../../src/services/text/Inno.Text/Text.cs#L80) | Shapes text with explicit language, script, and direction hints. |

### `Inno.Text.TextDirection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextDirection`](../../src/services/text/Inno.Text/TextTypes.cs#L9) | Selects the logical direction used by the shaping engine. |
| [`Inno.Text.TextDirection.Automatic`](../../src/services/text/Inno.Text/TextTypes.cs#L14) | Lets the shaping engine infer direction from the text. |
| [`Inno.Text.TextDirection.BottomToTop`](../../src/services/text/Inno.Text/TextTypes.cs#L30) | Shapes text from bottom to top. |
| [`Inno.Text.TextDirection.LeftToRight`](../../src/services/text/Inno.Text/TextTypes.cs#L18) | Shapes text from left to right. |
| [`Inno.Text.TextDirection.RightToLeft`](../../src/services/text/Inno.Text/TextTypes.cs#L22) | Shapes text from right to left. |
| [`Inno.Text.TextDirection.TopToBottom`](../../src/services/text/Inno.Text/TextTypes.cs#L26) | Shapes text from top to bottom. |

### `Inno.Text.TextExecutionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextExecutionContext`](../../src/services/text/Inno.Text/Text.cs#L10) | Binds one text service to the current asynchronous execution context. |
| [`static Inno.Text.ITextService Inno.Text.TextExecutionContext.current`](../../src/services/text/Inno.Text/Text.cs#L17) | Gets the text service bound to the current execution context. |
| [`static System.IDisposable Inno.Text.TextExecutionContext.EnterScope(Inno.Text.ITextService text)`](../../src/services/text/Inno.Text/Text.cs#L28) | Binds a text service until the returned strict last-in-first-out scope is disposed. |

### `Inno.Text.TextFontHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextFontHandle`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | Identifies one font face loaded into a text backend generation. |
| [`bool Inno.Text.TextFontHandle.isValid`](../../src/services/text/Inno.Text/TextTypes.cs#L63) | Gets whether this handle identifies a loaded face. |

### `Inno.Text.TextFontStyle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextFontStyle`](../../src/services/text/Inno.Text/TextTypes.cs#L36) | Selects the typographic slant requested from a font face. |
| [`Inno.Text.TextFontStyle.Italic`](../../src/services/text/Inno.Text/TextTypes.cs#L45) | Uses an italic face. |
| [`Inno.Text.TextFontStyle.Normal`](../../src/services/text/Inno.Text/TextTypes.cs#L41) | Uses an upright face. |
| [`Inno.Text.TextFontStyle.Oblique`](../../src/services/text/Inno.Text/TextTypes.cs#L49) | Uses an oblique face. |

### `Inno.Text.TextGlyph`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextGlyph`](../../src/services/text/Inno.Text/TextTypes.cs#L206) | Describes one positioned glyph emitted by Unicode shaping. |

### `Inno.Text.TextLayout`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextLayout`](../../src/services/text/Inno.Text/TextTypes.cs#L244) | Contains immutable positioned glyphs and aggregate bounds for one shaped string. |
| [`Inno.Text.TextLayout.TextLayout(System.Collections.Generic.IEnumerable<Inno.Text.TextGlyph> glyphs, Inno.Text.TextMetrics metrics, float width, float height)`](../../src/services/text/Inno.Text/TextTypes.cs#L263) | Creates an immutable text layout. |
| [`Inno.Text.TextMetrics Inno.Text.TextLayout.metrics`](../../src/services/text/Inno.Text/TextTypes.cs#L287) | Gets the font metrics used to build this layout. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Text.TextGlyph> Inno.Text.TextLayout.glyphs`](../../src/services/text/Inno.Text/TextTypes.cs#L283) | Gets the immutable positioned glyph sequence. |
| [`float Inno.Text.TextLayout.height`](../../src/services/text/Inno.Text/TextTypes.cs#L295) | Gets the aggregate line height. |
| [`float Inno.Text.TextLayout.width`](../../src/services/text/Inno.Text/TextTypes.cs#L291) | Gets the aggregate horizontal advance. |

### `Inno.Text.TextMetrics`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextMetrics`](../../src/services/text/Inno.Text/TextTypes.cs#L233) | Describes scalable metrics for one font face and logical size. |

### `Inno.Text.TextShapingOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextDirection Inno.Text.TextShapingOptions.direction`](../../src/services/text/Inno.Text/TextTypes.cs#L174) | Gets the requested logical direction. |
| [`Inno.Text.TextShapingOptions`](../../src/services/text/Inno.Text/TextTypes.cs#L141) | Defines language and script hints for one shaping operation. |
| [`Inno.Text.TextShapingOptions.TextShapingOptions(Inno.Text.TextDirection direction = Inno.Text.TextDirection.Automatic, string? language = null, string? script = null)`](../../src/services/text/Inno.Text/TextTypes.cs#L155) | Creates shaping hints. |
| [`static Inno.Text.TextShapingOptions Inno.Text.TextShapingOptions.automatic`](../../src/services/text/Inno.Text/TextTypes.cs#L170) | Gets automatic shaping hints. |
| [`string? Inno.Text.TextShapingOptions.language`](../../src/services/text/Inno.Text/TextTypes.cs#L178) | Gets the optional BCP 47 language code. |
| [`string? Inno.Text.TextShapingOptions.script`](../../src/services/text/Inno.Text/TextTypes.cs#L182) | Gets the optional ISO 15924 script code. |

### `Inno.Text.TextStyle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextFontStyle Inno.Text.TextStyle.style`](../../src/services/text/Inno.Text/TextTypes.cs#L131) | Gets the requested face slant. |
| [`Inno.Text.TextStyle`](../../src/services/text/Inno.Text/TextTypes.cs#L69) | Defines immutable font selection and sizing for one shaping operation. |
| [`Inno.Text.TextStyle.TextStyle(float fontSize, int faceIndex = 0, int weight = 400, Inno.Text.TextFontStyle style = Inno.Text.TextFontStyle.Normal, float letterSpacing = 0)`](../../src/services/text/Inno.Text/TextTypes.cs#L89) | Creates validated text styling. |
| [`float Inno.Text.TextStyle.fontSize`](../../src/services/text/Inno.Text/TextTypes.cs#L119) | Gets the logical font size. |
| [`float Inno.Text.TextStyle.letterSpacing`](../../src/services/text/Inno.Text/TextTypes.cs#L135) | Gets additional spacing appended to each glyph advance. |
| [`int Inno.Text.TextStyle.faceIndex`](../../src/services/text/Inno.Text/TextTypes.cs#L123) | Gets the collection face index. |
| [`int Inno.Text.TextStyle.weight`](../../src/services/text/Inno.Text/TextTypes.cs#L127) | Gets the CSS-compatible font weight. |
| [`static Inno.Text.TextStyle Inno.Text.TextStyle.defaultValue`](../../src/services/text/Inno.Text/TextTypes.cs#L115) | Gets a 16-pixel regular default style. |

## 项目依赖

- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
