# Inno.Text

[Text 索引](README.md) · [UI](../ui/Inno.UI.md)

后端中立契约位于 `src/services/text/Inno.Text`。`FontAsset` 保存可恢复资产身份与导入 metadata；`TextStyle`、`TextShapingOptions` 描述字号、字重、方向、语言等输入。`TextLayout` 返回字形与位置，`GlyphBitmap` 返回像素和度量，不包含 GPU、Scene 或 RmlUi 对象。

`ITextService.Shape` 与 `Rasterize` 是实际能力；脚本通过 `Text` 门面在活动 `TextExecutionContext` 内调用。没有 Session scope 会明确失败。唯一 `Properties/ScriptingApi.cs` 声明逻辑 `InnoEngine.Text` 暴露面；不导出 `ITextBackend`、native handle 或 adapter。

该层只依赖 Assets 与 Foundation；绘制、字形图集和场景呈现由调用方负责。

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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Text.FontAsset`

<!-- api: T:Inno.Text.FontAsset 40e24b27d108856cc32c3cf7b98be5eb18e1b754da47f5aee7c0a9451ccfd8c0 -->
<!-- api: P:Inno.Text.FontAsset.metadata aa389f59100c5217613e370b11f8f3d414d2e37d9a77eaca8d7e2ff537cc27f0 -->
<!-- api: M:Inno.Text.FontAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory{System.Byte},System.ReadOnlyMemory{System.Byte}) 843ba1d87294dba638268ce940d175d09766ae1b0795e3563cd30670341ddcdd -->
<!-- api: M:Inno.Text.FontAsset.#ctor c4f85a56b707bd92ee960e9ffc584c6e6f47329c30813e1091302f9655f4fc9f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cfb0bbaf183f8bdf"></a>[`public Inno.Text.FontAsset.FontAsset()`](../../src/services/text/Inno.Text/FontAsset.cs#L100) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-45533f79d08af1ba"></a>[`protected override void Inno.Text.FontAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory<byte> previousPayload, System.ReadOnlyMemory<byte> currentPayload)`](../../src/services/text/Inno.Text/FontAsset.cs#L118) | Refreshes imported metadata after an artifact commit.<br>`previousPayload`：The previous compact runtime payload.<br>`currentPayload`：The current compact runtime payload. | — | — |
| <a id="api-33356320dca52449"></a>[`public Inno.Text.FontMetadata? Inno.Text.FontAsset.metadata { get; }`](../../src/services/text/Inno.Text/FontAsset.cs#L107) | Gets imported metadata, or null before runtime content is loaded. | — | — |
| <a id="api-229f0918bf695370"></a>[`public sealed class Inno.Text.FontAsset : Inno.Assets.AssetObject`](../../src/services/text/Inno.Text/FontAsset.cs#L100) | Represents one imported OpenType font file or collection. | — | — |

### `Inno.Text.FontMetadata`

<!-- api: T:Inno.Text.FontMetadata 95746f7a69da6803f1ffdfadb836d52dfe2b649e1381d8392323588f32a7c353 -->
<!-- api: M:Inno.Text.FontMetadata.#ctor(System.Int32,System.Int64) 165f193e401809e34ee32f8377bdc0b63b68ff1af5bdbbc5763884dada7f9a5d -->
<!-- api: P:Inno.Text.FontMetadata.faceCount 9c0c9ffc8209d74ddab5dccd5d4770e7cfa0dd5f90890ce2437b659d63ccb36c -->
<!-- api: P:Inno.Text.FontMetadata.encodedByteLength 79a0161463ec67f588de949660cd180bc6b8d777eecfcf6316525e6cd45b8c1c -->
<!-- api: M:Inno.Text.FontMetadata.ToString 5b32fa9f707a503be5144f42e50e3564ce8791830229a9818de7cd6ca4cba1c7 -->
<!-- api: M:Inno.Text.FontMetadata.op_Inequality(Inno.Text.FontMetadata,Inno.Text.FontMetadata) 199e6da3ea748e4c648a5262f9b133a7708913a6c0fee5eb70d1098983d0b378 -->
<!-- api: M:Inno.Text.FontMetadata.op_Equality(Inno.Text.FontMetadata,Inno.Text.FontMetadata) 27fce64acf76edb79ea7d7b67889327cf63bea940c0bc28637e040efb1a3fdda -->
<!-- api: M:Inno.Text.FontMetadata.GetHashCode e20bf3c04150821f9862559b5409261c45e61eb61a2bc5c483d76540048cbc5c -->
<!-- api: M:Inno.Text.FontMetadata.Equals(System.Object) 8810c2830875ce20501ee613e960ab3cac9d316879ad63f1afaa438d95715b61 -->
<!-- api: M:Inno.Text.FontMetadata.Equals(Inno.Text.FontMetadata) 3751e7453ac95e2d07b8fff6f8576ad3d17f3db7fe4b69e6e651ef2431cadd39 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-627b529393bf7a0b"></a>[`public Inno.Text.FontMetadata.FontMetadata(int faceCount, long encodedByteLength)`](../../src/services/text/Inno.Text/FontAsset.cs#L23) | Creates validated imported font metadata.<br>`faceCount`：The number of faces in the source collection.<br>`encodedByteLength`：The encoded source length in bytes. | — | — |
| <a id="api-3d379731cb5aa970"></a>[`public bool Inno.Text.FontMetadata.Equals(Inno.Text.FontMetadata other)`](../../src/services/text/Inno.Text/FontAsset.cs#L12) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-6dd5f224f3dccb30"></a>[`public override bool Inno.Text.FontMetadata.Equals(object obj)`](../../src/services/text/Inno.Text/FontAsset.cs#L12) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-1728a794b6a7d39f"></a>[`public override int Inno.Text.FontMetadata.GetHashCode()`](../../src/services/text/Inno.Text/FontAsset.cs#L12) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-6662fd49b82fcb85"></a>[`public static bool Inno.Text.FontMetadata.operator ==(Inno.Text.FontMetadata left, Inno.Text.FontMetadata right)`](../../src/services/text/Inno.Text/FontAsset.cs#L12) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-873e5111b6eafa03"></a>[`public static bool Inno.Text.FontMetadata.operator !=(Inno.Text.FontMetadata left, Inno.Text.FontMetadata right)`](../../src/services/text/Inno.Text/FontAsset.cs#L12) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2e5c0118b29cad56"></a>[`public override string Inno.Text.FontMetadata.ToString()`](../../src/services/text/Inno.Text/FontAsset.cs#L12) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-6edc6432247032c2"></a>[`public long Inno.Text.FontMetadata.encodedByteLength { get; }`](../../src/services/text/Inno.Text/FontAsset.cs#L41) | Gets the encoded source length in bytes. | — | — |
| <a id="api-3e291afcdac7d63e"></a>[`public int Inno.Text.FontMetadata.faceCount { get; }`](../../src/services/text/Inno.Text/FontAsset.cs#L36) | Gets the number of independently addressable faces in the source. | — | — |
| <a id="api-285ef95f0b4873ed"></a>[`public readonly record struct Inno.Text.FontMetadata : System.IEquatable<Inno.Text.FontMetadata>`](../../src/services/text/Inno.Text/FontAsset.cs#L12) | Describes immutable metadata for an imported OpenType font source. | — | — |

### `Inno.Text.FontMetadataCodec`

<!-- api: T:Inno.Text.FontMetadataCodec 169e49b3c2bd29541e71e0a94116236f5c1793a699dfd59162b1f998b8efb2a2 -->
<!-- api: M:Inno.Text.FontMetadataCodec.Encode(Inno.Text.FontMetadata) bbd3f9eb86acf93f28384100e6cce9491b46d61e5ac3624a159b23ae3621c95a -->
<!-- api: M:Inno.Text.FontMetadataCodec.Decode(System.ReadOnlySpan{System.Byte}) e8cb013a6823a448c1a3821cb675d683fa1128c8839145ee5aa6305ddfd580ad -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3b5dd0b2ad55c89f"></a>[`public static Inno.Text.FontMetadata Inno.Text.FontMetadataCodec.Decode(System.ReadOnlySpan<byte> payload)`](../../src/services/text/Inno.Text/FontAsset.cs#L79) | Decodes and validates a compact font runtime payload.<br>返回：The decoded metadata.<br>`payload`：The complete runtime payload. | — | — |
| <a id="api-bbaf2750d1d8e4d3"></a>[`public static byte[] Inno.Text.FontMetadataCodec.Encode(Inno.Text.FontMetadata metadata)`](../../src/services/text/Inno.Text/FontAsset.cs#L61) | Encodes validated metadata into a deterministic runtime payload.<br>返回：The compact runtime payload.<br>`metadata`：The metadata to encode. | — | — |
| <a id="api-e8d5c01b19e82afa"></a>[`public static class Inno.Text.FontMetadataCodec`](../../src/services/text/Inno.Text/FontAsset.cs#L47) | Encodes the compact runtime payload shared by the font importer and text runtime. | — | — |

### `Inno.Text.GlyphBitmap`

<!-- api: T:Inno.Text.GlyphBitmap 1ecd7d65e487daabad24819caa653f7eba31fcef87a63f31e41163ea702659f3 -->
<!-- api: M:Inno.Text.GlyphBitmap.#ctor(System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.ReadOnlySpan{System.Byte}) 20e7909a954fe9a68e96a901d00478e59551626f341e8ed148865e92c1f8d690 -->
<!-- api: P:Inno.Text.GlyphBitmap.width 09688847206b474b0d2a0bec378f39f732d468ffd5f5257529815160b489f6e1 -->
<!-- api: P:Inno.Text.GlyphBitmap.height 0bba508f37067779bfdb76a4f856e016888b739861ce98bacc5dca09ab57b5e7 -->
<!-- api: P:Inno.Text.GlyphBitmap.bearingX a5d18ff63e768affc58f95d640fecb0b4dc6d3e1ecb0e6d132d756246d26a00b -->
<!-- api: P:Inno.Text.GlyphBitmap.bearingY 5aaa7f74d7e84d1e20e7b99cf9fcc3f9e218c75ce0407bc5de9958e29b20bfdc -->
<!-- api: P:Inno.Text.GlyphBitmap.advanceX 886a8c06268a62dae83a6074d7bf692c3586c7c813f0d8b2734611a0a87a57a9 -->
<!-- api: P:Inno.Text.GlyphBitmap.pixels bba9b0a396e9adeedd4e78b02e8ef64e00aa8299d5a27f3dba0a566481e86218 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-001df24d326d77b5"></a>[`public Inno.Text.GlyphBitmap.GlyphBitmap(int width, int height, int bearingX, int bearingY, float advanceX, System.ReadOnlySpan<byte> pixels)`](../../src/services/text/Inno.Text/TextTypes.cs#L326) | Creates a validated immutable glyph bitmap.<br>`width`：The bitmap width in pixels.<br>`height`：The bitmap height in pixels.<br>`bearingX`：The horizontal bearing in pixels.<br>`bearingY`：The vertical bearing in pixels.<br>`advanceX`：The horizontal glyph advance.<br>`pixels`：Tightly packed row-major 8-bit coverage. | — | — |
| <a id="api-c2244a15e4c17310"></a>[`public float Inno.Text.GlyphBitmap.advanceX { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L367) | Gets the horizontal glyph advance. | — | — |
| <a id="api-2de2f4a4ce9d11bf"></a>[`public int Inno.Text.GlyphBitmap.bearingX { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L359) | Gets the horizontal bearing in pixels. | — | — |
| <a id="api-ef47e96d02eb89e8"></a>[`public int Inno.Text.GlyphBitmap.bearingY { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L363) | Gets the vertical bearing in pixels. | — | — |
| <a id="api-e04143ca16c775f1"></a>[`public int Inno.Text.GlyphBitmap.height { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L355) | Gets the bitmap height in pixels. | — | — |
| <a id="api-60e3c796c2c40812"></a>[`public System.ReadOnlyMemory<byte> Inno.Text.GlyphBitmap.pixels { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L371) | Gets tightly packed row-major 8-bit coverage. | — | — |
| <a id="api-3eaa04c9c387c827"></a>[`public int Inno.Text.GlyphBitmap.width { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L351) | Gets the bitmap width in pixels. | — | — |
| <a id="api-832f392c8ae640eb"></a>[`public sealed class Inno.Text.GlyphBitmap`](../../src/services/text/Inno.Text/TextTypes.cs#L301) | Contains one rasterized glyph as tightly packed 8-bit coverage. | — | — |

### `Inno.Text.ITextBackend`

<!-- api: T:Inno.Text.ITextBackend a49307087921953d6961419cda5d94a33342b5008091c4460713064f748377f4 -->
<!-- api: M:Inno.Text.ITextBackend.LoadFont(System.ReadOnlySpan{System.Byte},System.Int32) 9a0402955992c2714e0d8918e4da7dfd731b3faf752fdff6b30c4f7a5ba968b6 -->
<!-- api: M:Inno.Text.ITextBackend.ReleaseFont(Inno.Text.TextFontHandle) 6454bd66c3125d48a5ed571407e6c63413855fd75ee8afcea116746c7375ecfe -->
<!-- api: M:Inno.Text.ITextBackend.Shape(Inno.Text.TextFontHandle,System.String,Inno.Text.TextStyle,Inno.Text.TextShapingOptions) 7e884732be1ee6940ed12d8272bb593a11f18c9123f7cf66181e3dd23b262f6a -->
<!-- api: M:Inno.Text.ITextBackend.Rasterize(Inno.Text.TextFontHandle,System.UInt32,System.Single) 7943dccd6eddd5a33c34a3708bd57ba985a816ce927deb2da9e589acebf187ed -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-893c74225c7b9d22"></a>[`Inno.Text.TextFontHandle Inno.Text.ITextBackend.LoadFont(System.ReadOnlySpan<byte> data, int faceIndex)`](../../src/services/text/Inno.Text/ITextBackend.cs#L22) | Loads one face from encoded OpenType data.<br>返回：A generation-local face handle.<br>`data`：The complete encoded font source.<br>`faceIndex`：The zero-based collection face index. | — | — |
| <a id="api-bb2a4623f7de1dee"></a>[`Inno.Text.GlyphBitmap Inno.Text.ITextBackend.Rasterize(Inno.Text.TextFontHandle font, uint glyphId, float fontSize)`](../../src/services/text/Inno.Text/ITextBackend.cs#L75) | Rasterizes one shaped glyph into 8-bit coverage.<br>返回：The immutable glyph bitmap.<br>`font`：The loaded face.<br>`glyphId`：The font-specific glyph identifier.<br>`fontSize`：The positive logical pixel size. | — | — |
| <a id="api-dcc3f8642605cc35"></a>[`void Inno.Text.ITextBackend.ReleaseFont(Inno.Text.TextFontHandle font)`](../../src/services/text/Inno.Text/ITextBackend.cs#L33) | Releases one loaded face.<br>`font`：The face handle to release. | — | — |
| <a id="api-dd7bfe939e8aebeb"></a>[`Inno.Text.TextLayout Inno.Text.ITextBackend.Shape(Inno.Text.TextFontHandle font, string text, Inno.Text.TextStyle style, Inno.Text.TextShapingOptions options)`](../../src/services/text/Inno.Text/ITextBackend.cs#L53) | Shapes one Unicode string into positioned glyphs.<br>返回：The immutable layout.<br>`font`：The loaded face.<br>`text`：The Unicode source text.<br>`style`：Font size and spacing.<br>`options`：Language, script, and direction hints. | — | — |
| <a id="api-27e41acddd05a81d"></a>[`public interface Inno.Text.ITextBackend : System.IDisposable`](../../src/services/text/Inno.Text/ITextBackend.cs#L8) | Defines the replaceable native shaping and glyph-rasterization boundary. | — | — |

### `Inno.Text.ITextService`

<!-- api: T:Inno.Text.ITextService c28fc9b7561dc627f0f6a901f42b17d6cd5dd12f22e611d18c35ebd9e731a6c7 -->
<!-- api: M:Inno.Text.ITextService.Shape(Inno.Text.FontAsset,System.String,Inno.Text.TextStyle,Inno.Text.TextShapingOptions) 6e94cda0b61108f4ffaeb920c8e5ccb7dd030d021551aa2084705f7a940634d8 -->
<!-- api: M:Inno.Text.ITextService.Rasterize(Inno.Text.FontAsset,System.Int32,System.UInt32,System.Single) c8dbbcdc4393b60ac0ba7e3d1a199e698d326065cb34c4a285709ccf458a10bd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2b79652a67d5e680"></a>[`Inno.Text.GlyphBitmap Inno.Text.ITextService.Rasterize(Inno.Text.FontAsset font, int faceIndex, uint glyphId, float fontSize)`](../../src/services/text/Inno.Text/ITextService.cs#L51) | Rasterizes one glyph from an imported font.<br>返回：The immutable glyph bitmap.<br>`font`：The imported font source.<br>`faceIndex`：The zero-based collection face index.<br>`glyphId`：The font-specific glyph identifier.<br>`fontSize`：The positive logical pixel size. | — | — |
| <a id="api-24e92feeabc0d75a"></a>[`Inno.Text.TextLayout Inno.Text.ITextService.Shape(Inno.Text.FontAsset font, string text, Inno.Text.TextStyle style, Inno.Text.TextShapingOptions options)`](../../src/services/text/Inno.Text/ITextService.cs#L26) | Shapes one Unicode string with an imported font.<br>返回：The immutable shaped layout.<br>`font`：The imported font source.<br>`text`：The Unicode source text.<br>`style`：Font selection and sizing.<br>`options`：Language, script, and direction hints. | — | — |
| <a id="api-2ceeda142615f331"></a>[`public interface Inno.Text.ITextService`](../../src/services/text/Inno.Text/ITextService.cs#L6) | Provides backend-neutral Unicode shaping and glyph rasterization for imported fonts. | — | — |

### `Inno.Text.Text`

<!-- api: T:Inno.Text.Text 9a370006c3980028f31cc22f7e0923ab7a77815d40ffb3b881fd76a621006bf0 -->
<!-- api: M:Inno.Text.Text.Shape(Inno.Text.FontAsset,System.String,Inno.Text.TextStyle) 4242ddfc29286ec0a56c60ea0cfdee6e2c5dda103f3073464c4050d5093af6f1 -->
<!-- api: M:Inno.Text.Text.Shape(Inno.Text.FontAsset,System.String,Inno.Text.TextStyle,Inno.Text.TextShapingOptions) d51a9eff234a3f23f4cc2bb8dee17a21b7f9ed2128ce352ff6611d9e5bf07806 -->
<!-- api: M:Inno.Text.Text.Rasterize(Inno.Text.FontAsset,System.Int32,System.UInt32,System.Single) e86152a9f6785aaefb6015f4e35052797158fbb83ac95c38045e8fd2df21f9fb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d542f50acd19dc07"></a>[`public static Inno.Text.GlyphBitmap Inno.Text.Text.Rasterize(Inno.Text.FontAsset font, int faceIndex, uint glyphId, float fontSize)`](../../src/services/text/Inno.Text/Text.cs#L106) | Rasterizes one shaped glyph into immutable 8-bit coverage.<br>返回：The immutable glyph bitmap.<br>`font`：The imported font source.<br>`faceIndex`：The zero-based collection face index.<br>`glyphId`：The font-specific glyph identifier.<br>`fontSize`：The positive logical pixel size. | — | — |
| <a id="api-b6170c42e862016b"></a>[`public static Inno.Text.TextLayout Inno.Text.Text.Shape(Inno.Text.FontAsset font, string text, Inno.Text.TextStyle style, Inno.Text.TextShapingOptions options)`](../../src/services/text/Inno.Text/Text.cs#L80) | Shapes text with explicit language, script, and direction hints.<br>返回：The immutable shaped layout.<br>`font`：The imported font source.<br>`text`：The Unicode source text.<br>`style`：Font selection and sizing.<br>`options`：Language, script, and direction hints. | — | — |
| <a id="api-88913e3c605d8a6c"></a>[`public static Inno.Text.TextLayout Inno.Text.Text.Shape(Inno.Text.FontAsset font, string text, Inno.Text.TextStyle style)`](../../src/services/text/Inno.Text/Text.cs#L55) | Shapes text with default automatic language and direction detection.<br>返回：The immutable shaped layout.<br>`font`：The imported font source.<br>`text`：The Unicode source text.<br>`style`：Font selection and sizing. | — | — |
| <a id="api-3661add59d3ad7ed"></a>[`public static class Inno.Text.Text`](../../src/services/text/Inno.Text/Text.cs#L38) | Provides script-friendly access to Unicode shaping and glyph rasterization. | — | — |

### `Inno.Text.TextDirection`

<!-- api: T:Inno.Text.TextDirection 5fc83b138bad42cfea1cb9f0552e5a4bbff831f89b349b6a6dae01685de00e02 -->
<!-- api: F:Inno.Text.TextDirection.Automatic ef5c147d1c4b5758f3617040b8c259794f18207cac4644565f4259c7f404d7b6 -->
<!-- api: F:Inno.Text.TextDirection.LeftToRight 87abfa93f3d12a0c3bcb390813c5edce6bd133f74400f2dcf9e10cdcb6fbb48a -->
<!-- api: F:Inno.Text.TextDirection.RightToLeft e1018346e76c969ecdc972c45ded96525bad91ae1c1e2c812ae890c69916e76b -->
<!-- api: F:Inno.Text.TextDirection.TopToBottom 090c399c1319a1c96f29453f8d44906a460810fbad6250d8e5cc27a80432699f -->
<!-- api: F:Inno.Text.TextDirection.BottomToTop 27e436c936174fbcc572297a52a29f005bd785d7f5920cf39fdadf4990d2a892 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7baa9c7d446e25c0"></a>[`Inno.Text.TextDirection.Automatic = 0`](../../src/services/text/Inno.Text/TextTypes.cs#L14) | Lets the shaping engine infer direction from the text. | — | — |
| <a id="api-0d681a2e30e5b6c5"></a>[`Inno.Text.TextDirection.BottomToTop = 4`](../../src/services/text/Inno.Text/TextTypes.cs#L30) | Shapes text from bottom to top. | — | — |
| <a id="api-05d39e700e557638"></a>[`Inno.Text.TextDirection.LeftToRight = 1`](../../src/services/text/Inno.Text/TextTypes.cs#L18) | Shapes text from left to right. | — | — |
| <a id="api-df8da577ea5d85e7"></a>[`Inno.Text.TextDirection.RightToLeft = 2`](../../src/services/text/Inno.Text/TextTypes.cs#L22) | Shapes text from right to left. | — | — |
| <a id="api-a1db108eb53a9f52"></a>[`Inno.Text.TextDirection.TopToBottom = 3`](../../src/services/text/Inno.Text/TextTypes.cs#L26) | Shapes text from top to bottom. | — | — |
| <a id="api-ff30fe8d6e73607e"></a>[`public enum Inno.Text.TextDirection : int`](../../src/services/text/Inno.Text/TextTypes.cs#L9) | Selects the logical direction used by the shaping engine. | — | — |

### `Inno.Text.TextExecutionContext`

<!-- api: T:Inno.Text.TextExecutionContext 25ea516846c65aae799639fe8216ac07bf14fe191aaff4709abfd96595d77076 -->
<!-- api: P:Inno.Text.TextExecutionContext.current afe2f48de77aadeb2ae2d615afc7b71f407be39f777c29fd69a542f7a5f98771 -->
<!-- api: M:Inno.Text.TextExecutionContext.EnterScope(Inno.Text.ITextService) 280875546c978c4e0db7e0057ab8f973ab76195ecacd1863d7da797092589ede -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-98c23eee2bdc307d"></a>[`public static System.IDisposable Inno.Text.TextExecutionContext.EnterScope(Inno.Text.ITextService text)`](../../src/services/text/Inno.Text/Text.cs#L28) | Binds a text service until the returned strict last-in-first-out scope is disposed.<br>返回：The caller-owned binding scope.<br>`text`：The host-owned text service. | — | — |
| <a id="api-7d2a9d289fe383c7"></a>[`public static Inno.Text.ITextService Inno.Text.TextExecutionContext.current { get; }`](../../src/services/text/Inno.Text/Text.cs#L17) | Gets the text service bound to the current execution context. | — | — |
| <a id="api-1c3a7dc6335c074d"></a>[`public static class Inno.Text.TextExecutionContext`](../../src/services/text/Inno.Text/Text.cs#L10) | Binds one text service to the current asynchronous execution context. | — | — |

### `Inno.Text.TextFontHandle`

<!-- api: T:Inno.Text.TextFontHandle 8fc4a398faa6d9d9bf3e7623b051a14d776b2d78bbe089df3982cbc883222935 -->
<!-- api: M:Inno.Text.TextFontHandle.#ctor(System.UInt64) 23c1cc10e7046d87cdd10ee8acb64a18dee2ce63e522b0aaeed966385beca03d -->
<!-- api: P:Inno.Text.TextFontHandle.value 4792c4bcaf5874a78d0a83fabebf05bfeeb8abb4240ebab187663f3829020913 -->
<!-- api: P:Inno.Text.TextFontHandle.isValid 93712d9609be077a8922ca043205f1f768179a94a23a1b4a23d92a39ce5bab2e -->
<!-- api: M:Inno.Text.TextFontHandle.ToString 4ebfe0f32961864a7df8c9259bed78aa08a8a4f694ccb6d6b1b028e86578d7cb -->
<!-- api: M:Inno.Text.TextFontHandle.op_Inequality(Inno.Text.TextFontHandle,Inno.Text.TextFontHandle) 4be5f6070b3e28f2e76966eb9068a7bda8cf76cf835ef31f9f19f092917e2644 -->
<!-- api: M:Inno.Text.TextFontHandle.op_Equality(Inno.Text.TextFontHandle,Inno.Text.TextFontHandle) 9fe8d0ec34e445ded8e855e8cc26f32001ab5f209ed61026d3f2b4d8bc3a2c7e -->
<!-- api: M:Inno.Text.TextFontHandle.GetHashCode 9dc2b828f5d33ebe037fa5ae9daf8b7a7cb615b9a09515719fdc43ce8519eeaa -->
<!-- api: M:Inno.Text.TextFontHandle.Equals(System.Object) 1d29b8d8f0636f956af0cf8ea7b3570994e8fd3c7aa46c3de935013dd48f88cb -->
<!-- api: M:Inno.Text.TextFontHandle.Equals(Inno.Text.TextFontHandle) 8b2d1f00ace07bc32c643e0fcfe2e5f7d86e4df485f383cdbf5da3d9abdc8f4f -->
<!-- api: M:Inno.Text.TextFontHandle.Deconstruct(System.UInt64@) 4ea8ffd496863ff99b85e3037003709f967224a9049e4672d08b203c39b2a5d6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1249aa692332d36c"></a>[`public Inno.Text.TextFontHandle.TextFontHandle(ulong value)`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | Identifies one font face loaded into a text backend generation.<br>`value`：The concrete value read or transformed by this operation. | — | — |
| <a id="api-7943ecc751eb9976"></a>[`public void Inno.Text.TextFontHandle.Deconstruct(out ulong value)`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-02a9bb50ebeadca0"></a>[`public bool Inno.Text.TextFontHandle.Equals(Inno.Text.TextFontHandle other)`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-00136e1b8fdeb63d"></a>[`public override bool Inno.Text.TextFontHandle.Equals(object obj)`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-b818fb68fde2d9e2"></a>[`public override int Inno.Text.TextFontHandle.GetHashCode()`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-224ce6e2b1ba72b3"></a>[`public static bool Inno.Text.TextFontHandle.operator ==(Inno.Text.TextFontHandle left, Inno.Text.TextFontHandle right)`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e4d338c2a2d32c3c"></a>[`public static bool Inno.Text.TextFontHandle.operator !=(Inno.Text.TextFontHandle left, Inno.Text.TextFontHandle right)`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-db0d0bff7ef6ee96"></a>[`public override string Inno.Text.TextFontHandle.ToString()`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-f37f07fb9c5fa8c0"></a>[`public bool Inno.Text.TextFontHandle.isValid { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L63) | Gets whether this handle identifies a loaded face. | — | — |
| <a id="api-53367a38f91587d3"></a>[`public ulong Inno.Text.TextFontHandle.value { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | The concrete value read or transformed by this operation. | — | — |
| <a id="api-213ea3cce8ff82c4"></a>[`public readonly record struct Inno.Text.TextFontHandle : System.IEquatable<Inno.Text.TextFontHandle>`](../../src/services/text/Inno.Text/TextTypes.cs#L58) | Identifies one font face loaded into a text backend generation.<br>`value`：The concrete value read or transformed by this operation. | — | — |

### `Inno.Text.TextFontStyle`

<!-- api: T:Inno.Text.TextFontStyle 75ddf3981e847bcb52da20684df943fb038008afe614cb66df2921976d5009ed -->
<!-- api: F:Inno.Text.TextFontStyle.Normal 6a54be1a0efd3ce5da385424f0ecc9e86b02f3694780d13636d181b7fc8531c9 -->
<!-- api: F:Inno.Text.TextFontStyle.Italic e6ced042b477e6259ea34d004f33c0ef00f28b1357cafabb4d6c9aae876eccf7 -->
<!-- api: F:Inno.Text.TextFontStyle.Oblique 951b73d7f2e22f6dcd7b24f8032330a86c74e87fa9e09bb0593904736380e41e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-372e203241f3b43b"></a>[`Inno.Text.TextFontStyle.Italic = 1`](../../src/services/text/Inno.Text/TextTypes.cs#L45) | Uses an italic face. | — | — |
| <a id="api-10067220be314d7e"></a>[`Inno.Text.TextFontStyle.Normal = 0`](../../src/services/text/Inno.Text/TextTypes.cs#L41) | Uses an upright face. | — | — |
| <a id="api-f49cd479479615f0"></a>[`Inno.Text.TextFontStyle.Oblique = 2`](../../src/services/text/Inno.Text/TextTypes.cs#L49) | Uses an oblique face. | — | — |
| <a id="api-2f44ed380f86cab5"></a>[`public enum Inno.Text.TextFontStyle : int`](../../src/services/text/Inno.Text/TextTypes.cs#L36) | Selects the typographic slant requested from a font face. | — | — |

### `Inno.Text.TextGlyph`

<!-- api: T:Inno.Text.TextGlyph 177c638e3902d1f30bac0610d3c885ee8526fe0f5fd275253321eb3245d7b7cd -->
<!-- api: M:Inno.Text.TextGlyph.#ctor(System.UInt32,System.UInt32,System.Single,System.Single,System.Single,System.Single) a6bd696c2cd9ef82248f015a0529a9139e62c16bce0d277b04f6cbd3daf5da74 -->
<!-- api: P:Inno.Text.TextGlyph.glyphId 263f1c07319a35fe49e8829775c8ea7737e41cffd0315578f54d4cf9156e7ec7 -->
<!-- api: P:Inno.Text.TextGlyph.cluster cf1689b71037e68c4e638e3a07c482503134e4e6906bf6f3989d7ea6d36bca18 -->
<!-- api: P:Inno.Text.TextGlyph.advanceX 71131feeda6cea4b5eb139097809ea056e60bb2d83ce18b5c90036ad61890c56 -->
<!-- api: P:Inno.Text.TextGlyph.advanceY 95aee00faec803caffca32be8c8b89a6ad4f09c66ea4fab41831249a27c4653b -->
<!-- api: P:Inno.Text.TextGlyph.offsetX ca9c588d37d7ae79f54c835c24cd4e931abb2bd1e9232fac834388da24ef3d51 -->
<!-- api: P:Inno.Text.TextGlyph.offsetY 0ad727f2843aec6798cf3db3d4b793c2996dd67afa297453032a3df0b83182d4 -->
<!-- api: M:Inno.Text.TextGlyph.ToString b6bfac7075b03b967500861fa1a9e9660dd5cb17a7bb4eac608d0840e66b3b4d -->
<!-- api: M:Inno.Text.TextGlyph.op_Inequality(Inno.Text.TextGlyph,Inno.Text.TextGlyph) 89ddc539844aa7edf399325e41ae79666463b700eb6d8f12f1f87fa0efd88870 -->
<!-- api: M:Inno.Text.TextGlyph.op_Equality(Inno.Text.TextGlyph,Inno.Text.TextGlyph) 32c1d771b0f591e9a9b9cf2408c0133e51f3863d699a58de757690f0f8fea5dc -->
<!-- api: M:Inno.Text.TextGlyph.GetHashCode df769991c70af48cd6c2b29d723214f648c7bcc14051b931796f4fd0d179724f -->
<!-- api: M:Inno.Text.TextGlyph.Equals(System.Object) 11e228bf9abf7a553a4b80cdfb53cf5a1c732281b967cba667bc72a7366bcd8b -->
<!-- api: M:Inno.Text.TextGlyph.Equals(Inno.Text.TextGlyph) 5ab022f82abee10e39367fd1a5003baf85981cbcdb0bae25a0522cfeaa830898 -->
<!-- api: M:Inno.Text.TextGlyph.Deconstruct(System.UInt32@,System.UInt32@,System.Single@,System.Single@,System.Single@,System.Single@) a248d1d7ad3bb71d8b7f126adc3a7baf18cdfd344b993b247a692fa942e77658 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0c5f7a4b3e1d602d"></a>[`public Inno.Text.TextGlyph.TextGlyph(uint glyphId, uint cluster, float advanceX, float advanceY, float offsetX, float offsetY)`](../../src/services/text/Inno.Text/TextTypes.cs#L206) | Describes one positioned glyph emitted by Unicode shaping.<br>`glyphId`：The uint value used to initialize this instance.<br>`cluster`：The uint value used to initialize this instance.<br>`advanceX`：The float value used to initialize this instance.<br>`advanceY`：The float value used to initialize this instance.<br>`offsetX`：The horizontal scroll delta reported by the platform.<br>`offsetY`：The vertical scroll delta reported by the platform. | — | — |
| <a id="api-7bd14e6157fc4995"></a>[`public void Inno.Text.TextGlyph.Deconstruct(out uint glyphId, out uint cluster, out float advanceX, out float advanceY, out float offsetX, out float offsetY)`](../../src/services/text/Inno.Text/TextTypes.cs#L206) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-a5c763a2f0869ae8"></a>[`public bool Inno.Text.TextGlyph.Equals(Inno.Text.TextGlyph other)`](../../src/services/text/Inno.Text/TextTypes.cs#L206) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-07e17e3959505b20"></a>[`public override bool Inno.Text.TextGlyph.Equals(object obj)`](../../src/services/text/Inno.Text/TextTypes.cs#L206) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-93aafa7c253c2edf"></a>[`public override int Inno.Text.TextGlyph.GetHashCode()`](../../src/services/text/Inno.Text/TextTypes.cs#L206) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-c67c3bef44aa2346"></a>[`public static bool Inno.Text.TextGlyph.operator ==(Inno.Text.TextGlyph left, Inno.Text.TextGlyph right)`](../../src/services/text/Inno.Text/TextTypes.cs#L206) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-8dd75b62defce5f6"></a>[`public static bool Inno.Text.TextGlyph.operator !=(Inno.Text.TextGlyph left, Inno.Text.TextGlyph right)`](../../src/services/text/Inno.Text/TextTypes.cs#L206) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-1a36592eca97a152"></a>[`public override string Inno.Text.TextGlyph.ToString()`](../../src/services/text/Inno.Text/TextTypes.cs#L206) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-141ddcd55458ae5f"></a>[`public float Inno.Text.TextGlyph.advanceX { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L209) | The float value used to initialize this instance. | — | — |
| <a id="api-fcdec7c8b72c7e28"></a>[`public float Inno.Text.TextGlyph.advanceY { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L210) | The float value used to initialize this instance. | — | — |
| <a id="api-18ed4c568091d392"></a>[`public uint Inno.Text.TextGlyph.cluster { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L208) | The uint value used to initialize this instance. | — | — |
| <a id="api-5901141e763352cb"></a>[`public uint Inno.Text.TextGlyph.glyphId { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L207) | The uint value used to initialize this instance. | — | — |
| <a id="api-4e24eb7eef682f13"></a>[`public float Inno.Text.TextGlyph.offsetX { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L211) | The horizontal scroll delta reported by the platform. | — | — |
| <a id="api-d539a50d4464fd45"></a>[`public float Inno.Text.TextGlyph.offsetY { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L212) | The vertical scroll delta reported by the platform. | — | — |
| <a id="api-027e94403f7bec65"></a>[`public readonly record struct Inno.Text.TextGlyph : System.IEquatable<Inno.Text.TextGlyph>`](../../src/services/text/Inno.Text/TextTypes.cs#L206) | Describes one positioned glyph emitted by Unicode shaping.<br>`glyphId`：The uint value used to initialize this instance.<br>`cluster`：The uint value used to initialize this instance.<br>`advanceX`：The float value used to initialize this instance.<br>`advanceY`：The float value used to initialize this instance.<br>`offsetX`：The horizontal scroll delta reported by the platform.<br>`offsetY`：The vertical scroll delta reported by the platform. | — | — |

### `Inno.Text.TextLayout`

<!-- api: T:Inno.Text.TextLayout ef1e435aa1831244635796b87d01a3c086ef0aac9a7adfae1ec4d4662ce89839 -->
<!-- api: M:Inno.Text.TextLayout.#ctor(System.Collections.Generic.IEnumerable{Inno.Text.TextGlyph},Inno.Text.TextMetrics,System.Single,System.Single) e65bf26c61476b1f4240fcceb3b39678d9056e654c1889dcb49a78332e1c8d21 -->
<!-- api: P:Inno.Text.TextLayout.glyphs 11d0d80d02ffa3a216f8589eb1e7d26e5d9d424a1b1aa12a140736c6132e57fb -->
<!-- api: P:Inno.Text.TextLayout.metrics b768716f07f197a8b4f5cb75513c2336feda03e64feeedc904e9e84ef68b0419 -->
<!-- api: P:Inno.Text.TextLayout.width 6cbdfb8ee7b259a686b4ad478c0e8aeb1ac7ce8e6b1a7fbbb42a412160c36c85 -->
<!-- api: P:Inno.Text.TextLayout.height 16816e88300808b7b8aa157a2e0aab55d5b0723205c3ea669071c4b67bd9e7a1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c88dd6d7bb258665"></a>[`public Inno.Text.TextLayout.TextLayout(System.Collections.Generic.IEnumerable<Inno.Text.TextGlyph> glyphs, Inno.Text.TextMetrics metrics, float width, float height)`](../../src/services/text/Inno.Text/TextTypes.cs#L263) | Creates an immutable text layout.<br>`glyphs`：The positioned glyph sequence.<br>`metrics`：The face metrics used by the layout.<br>`width`：The logical horizontal advance.<br>`height`：The logical line height. | — | — |
| <a id="api-5b9b4a1258ee30bc"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Text.TextGlyph> Inno.Text.TextLayout.glyphs { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L283) | Gets the immutable positioned glyph sequence. | — | — |
| <a id="api-dccf92ca1db8c92d"></a>[`public float Inno.Text.TextLayout.height { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L295) | Gets the aggregate line height. | — | — |
| <a id="api-2da28083da51e413"></a>[`public Inno.Text.TextMetrics Inno.Text.TextLayout.metrics { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L287) | Gets the font metrics used to build this layout. | — | — |
| <a id="api-97db62e7d187f123"></a>[`public float Inno.Text.TextLayout.width { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L291) | Gets the aggregate horizontal advance. | — | — |
| <a id="api-ef32529435b6d56c"></a>[`public sealed class Inno.Text.TextLayout`](../../src/services/text/Inno.Text/TextTypes.cs#L244) | Contains immutable positioned glyphs and aggregate bounds for one shaped string. | — | — |

### `Inno.Text.TextMetrics`

<!-- api: T:Inno.Text.TextMetrics 9d4b5169deea03f295d7a6c1a721ab9d62d18209d3d62b19e1f10c63830111b6 -->
<!-- api: M:Inno.Text.TextMetrics.#ctor(System.Single,System.Single,System.Single,System.Single,System.Single) e693f078054f365585c397f04f5e39d096bef6f1b71384d7fa79c227986ea9e9 -->
<!-- api: P:Inno.Text.TextMetrics.ascender ee43f44fdc4f3b1de11672ffcbc2ee6ebab0ac8793ae79e96c33f64ed0fe8b3b -->
<!-- api: P:Inno.Text.TextMetrics.descender 7f1fad939fc1f2154115a57908546dd3cdb146d14183166d6693f816f3528184 -->
<!-- api: P:Inno.Text.TextMetrics.lineHeight 9d4fa077ad0c20ad41dcac1b4d1ca0bd018fabba2b47f5adefe8d0afbb3dbef1 -->
<!-- api: P:Inno.Text.TextMetrics.underlinePosition 7456f62a4adcf5329b3e67ed8bdb29ef883a22ace0dc0d6ae664549a77c7b3cf -->
<!-- api: P:Inno.Text.TextMetrics.underlineThickness 8fe74c8cc8afb45884777aec054463d36255395d8a6b51732a0f40405c36f148 -->
<!-- api: M:Inno.Text.TextMetrics.ToString e3a21df35401fc97faa506927d3bc6c050f6c553d2b274310c7d1964395832a5 -->
<!-- api: M:Inno.Text.TextMetrics.op_Inequality(Inno.Text.TextMetrics,Inno.Text.TextMetrics) 5845c2ea06040f62bc9460ed631c123a3436044465c31dc6c4ef94f8456795fa -->
<!-- api: M:Inno.Text.TextMetrics.op_Equality(Inno.Text.TextMetrics,Inno.Text.TextMetrics) 9576ce40b932acd325a05235f4b9450032640e128b932b526741fa39c0c084d5 -->
<!-- api: M:Inno.Text.TextMetrics.GetHashCode fcb895dd1ef321750f89f0a0488893f2035db91edf015d0f17ab955f526278ad -->
<!-- api: M:Inno.Text.TextMetrics.Equals(System.Object) 98c5a754ad9e9dd5f41f57e09b628b226c5eaa06e10ca43bc2c29d231ee1bfae -->
<!-- api: M:Inno.Text.TextMetrics.Equals(Inno.Text.TextMetrics) e2921fc8aaffb2a72796eeadecbd01ba01e3f115410ec4af70f5dd0dfd87222b -->
<!-- api: M:Inno.Text.TextMetrics.Deconstruct(System.Single@,System.Single@,System.Single@,System.Single@,System.Single@) a45e85749cefe99a7d816b15d73b896d41a77c3edb9d7e7993e4d35068f84fdf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2e2ac5914e16fbb7"></a>[`public Inno.Text.TextMetrics.TextMetrics(float ascender, float descender, float lineHeight, float underlinePosition, float underlineThickness)`](../../src/services/text/Inno.Text/TextTypes.cs#L233) | Describes scalable metrics for one font face and logical size.<br>`ascender`：The float value used to initialize this instance.<br>`descender`：The float value used to initialize this instance.<br>`lineHeight`：The float value used to initialize this instance.<br>`underlinePosition`：The float value used to initialize this instance.<br>`underlineThickness`：The float value used to initialize this instance. | — | — |
| <a id="api-84534d5600123982"></a>[`public void Inno.Text.TextMetrics.Deconstruct(out float ascender, out float descender, out float lineHeight, out float underlinePosition, out float underlineThickness)`](../../src/services/text/Inno.Text/TextTypes.cs#L233) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-0da8988bdb34058b"></a>[`public bool Inno.Text.TextMetrics.Equals(Inno.Text.TextMetrics other)`](../../src/services/text/Inno.Text/TextTypes.cs#L233) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-933980d100dafa95"></a>[`public override bool Inno.Text.TextMetrics.Equals(object obj)`](../../src/services/text/Inno.Text/TextTypes.cs#L233) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-b93679d5a7eb9f96"></a>[`public override int Inno.Text.TextMetrics.GetHashCode()`](../../src/services/text/Inno.Text/TextTypes.cs#L233) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-9c1a6009b214b642"></a>[`public static bool Inno.Text.TextMetrics.operator ==(Inno.Text.TextMetrics left, Inno.Text.TextMetrics right)`](../../src/services/text/Inno.Text/TextTypes.cs#L233) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-8a304d01854f0166"></a>[`public static bool Inno.Text.TextMetrics.operator !=(Inno.Text.TextMetrics left, Inno.Text.TextMetrics right)`](../../src/services/text/Inno.Text/TextTypes.cs#L233) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-1a380294f14306b3"></a>[`public override string Inno.Text.TextMetrics.ToString()`](../../src/services/text/Inno.Text/TextTypes.cs#L233) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-b8e3af8feffe1aad"></a>[`public float Inno.Text.TextMetrics.ascender { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L234) | The float value used to initialize this instance. | — | — |
| <a id="api-9b64224151668733"></a>[`public float Inno.Text.TextMetrics.descender { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L235) | The float value used to initialize this instance. | — | — |
| <a id="api-8d4ba81b5d5d5463"></a>[`public float Inno.Text.TextMetrics.lineHeight { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L236) | The float value used to initialize this instance. | — | — |
| <a id="api-55eed78c5c2e46c1"></a>[`public float Inno.Text.TextMetrics.underlinePosition { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L237) | The float value used to initialize this instance. | — | — |
| <a id="api-567b471bee29380a"></a>[`public float Inno.Text.TextMetrics.underlineThickness { get; init; }`](../../src/services/text/Inno.Text/TextTypes.cs#L238) | The float value used to initialize this instance. | — | — |
| <a id="api-ac7c1fd5668efd67"></a>[`public readonly record struct Inno.Text.TextMetrics : System.IEquatable<Inno.Text.TextMetrics>`](../../src/services/text/Inno.Text/TextTypes.cs#L233) | Describes scalable metrics for one font face and logical size.<br>`ascender`：The float value used to initialize this instance.<br>`descender`：The float value used to initialize this instance.<br>`lineHeight`：The float value used to initialize this instance.<br>`underlinePosition`：The float value used to initialize this instance.<br>`underlineThickness`：The float value used to initialize this instance. | — | — |

### `Inno.Text.TextShapingOptions`

<!-- api: T:Inno.Text.TextShapingOptions 5eec2d4999b11a71e2fc754763404ad05626a8462c909caad9de487c303ede2a -->
<!-- api: M:Inno.Text.TextShapingOptions.#ctor(Inno.Text.TextDirection,System.String,System.String) ae7f0e148cab62e8fb92b3b19bbfb7dcb44d33941c36ffa848ebdd11f56233cd -->
<!-- api: P:Inno.Text.TextShapingOptions.automatic b7e3aa0d3c1a418f75d2c598e18f5d1ed0204556798a45363fdf0e36a9959656 -->
<!-- api: P:Inno.Text.TextShapingOptions.direction 1eba837cd6d73b6fa4fa41674c9c63aeffc6a0bfb919143fb2d24d12ceac21ac -->
<!-- api: P:Inno.Text.TextShapingOptions.language 08d1f9c3916210ee62f66dd14d554eaefbae37391e2040637838cc5fc2af799e -->
<!-- api: P:Inno.Text.TextShapingOptions.script a65520be25f628c3b64ddeeef489d4879ce679837e3b0654aebd9a1490ec62b0 -->
<!-- api: M:Inno.Text.TextShapingOptions.ToString c828426ad828305b12509fe21cfa3796f81ed28980f80be722ae51774b6a706c -->
<!-- api: M:Inno.Text.TextShapingOptions.op_Inequality(Inno.Text.TextShapingOptions,Inno.Text.TextShapingOptions) 5907d4a6c5c27a0707849f227825e892b1d347fe49104958eae34dbc51a3d9ca -->
<!-- api: M:Inno.Text.TextShapingOptions.op_Equality(Inno.Text.TextShapingOptions,Inno.Text.TextShapingOptions) c964fa1125bcb22b86178ed00d42f392574754ebb85970374579ffa0587eae6b -->
<!-- api: M:Inno.Text.TextShapingOptions.GetHashCode db394755fe26efa67e919498c53abe0832893f945afe1213f39929b6b7633c65 -->
<!-- api: M:Inno.Text.TextShapingOptions.Equals(System.Object) 5361fcebce03e4932362b3a75848a3964ed7aaf9c824be562da228a10dddf99f -->
<!-- api: M:Inno.Text.TextShapingOptions.Equals(Inno.Text.TextShapingOptions) 70526e453525184a4c4840bb78be26251f31bde9725a031f17ca65be38f8ca3d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-563597e8abba0e0f"></a>[`public Inno.Text.TextShapingOptions.TextShapingOptions(Inno.Text.TextDirection direction = Inno.Text.TextDirection.Automatic, string? language = null, string? script = null)`](../../src/services/text/Inno.Text/TextTypes.cs#L155) | Creates shaping hints.<br>`direction`：The logical text direction.<br>`language`：An optional BCP 47 language code.<br>`script`：An optional ISO 15924 script code. | — | — |
| <a id="api-97c21da61185c825"></a>[`public bool Inno.Text.TextShapingOptions.Equals(Inno.Text.TextShapingOptions other)`](../../src/services/text/Inno.Text/TextTypes.cs#L141) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-067f59f900ffeb3c"></a>[`public override bool Inno.Text.TextShapingOptions.Equals(object obj)`](../../src/services/text/Inno.Text/TextTypes.cs#L141) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-58b5f672e2e89a5b"></a>[`public override int Inno.Text.TextShapingOptions.GetHashCode()`](../../src/services/text/Inno.Text/TextTypes.cs#L141) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-3a3d539b858f249b"></a>[`public static bool Inno.Text.TextShapingOptions.operator ==(Inno.Text.TextShapingOptions left, Inno.Text.TextShapingOptions right)`](../../src/services/text/Inno.Text/TextTypes.cs#L141) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e4ceff5b9cb1c05c"></a>[`public static bool Inno.Text.TextShapingOptions.operator !=(Inno.Text.TextShapingOptions left, Inno.Text.TextShapingOptions right)`](../../src/services/text/Inno.Text/TextTypes.cs#L141) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-b979fc8a1e7bc5e0"></a>[`public override string Inno.Text.TextShapingOptions.ToString()`](../../src/services/text/Inno.Text/TextTypes.cs#L141) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-082ab576860b6622"></a>[`public static Inno.Text.TextShapingOptions Inno.Text.TextShapingOptions.automatic { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L170) | Gets automatic shaping hints. | — | — |
| <a id="api-9c6271ffd9e4af68"></a>[`public Inno.Text.TextDirection Inno.Text.TextShapingOptions.direction { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L174) | Gets the requested logical direction. | — | — |
| <a id="api-b3484362ee2a63f4"></a>[`public string? Inno.Text.TextShapingOptions.language { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L178) | Gets the optional BCP 47 language code. | — | — |
| <a id="api-4b4a27d9d79905b5"></a>[`public string? Inno.Text.TextShapingOptions.script { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L182) | Gets the optional ISO 15924 script code. | — | — |
| <a id="api-54ef59fb6723c7dd"></a>[`public readonly record struct Inno.Text.TextShapingOptions : System.IEquatable<Inno.Text.TextShapingOptions>`](../../src/services/text/Inno.Text/TextTypes.cs#L141) | Defines language and script hints for one shaping operation. | — | — |

### `Inno.Text.TextStyle`

<!-- api: T:Inno.Text.TextStyle 59dc665fcb01addc4ea6d15f9005eb5904a0c4d2d9449d90aa94d8bfc82e1128 -->
<!-- api: M:Inno.Text.TextStyle.#ctor(System.Single,System.Int32,System.Int32,Inno.Text.TextFontStyle,System.Single) 7ec5c4ae2914487d1d15b3a2441c2deda562a85de02bc9ea7633c03bd841a5d1 -->
<!-- api: P:Inno.Text.TextStyle.defaultValue a9b0bb1f0f77c52c8be2a644d6c8aff25d10fbea057347a24d6955228c74f6f5 -->
<!-- api: P:Inno.Text.TextStyle.fontSize 0e5a850f0995bac4b8f731a9a098f9317387fdbe02c4ec90cebadffc6e35e839 -->
<!-- api: P:Inno.Text.TextStyle.faceIndex 5daa506b5d528f50b384fa69b15fbb2f0049c5b34882aecc22289331f900de7b -->
<!-- api: P:Inno.Text.TextStyle.weight 90ac851c86edbdb57991a8589ab07cc97cbdf64f646dd6410d44a6eb7259107e -->
<!-- api: P:Inno.Text.TextStyle.style 657d34f5b4baea2f72c21a9d80c93a3debd74de2143c95488f1f868fe667f7ab -->
<!-- api: P:Inno.Text.TextStyle.letterSpacing 46a9185f49ff7333e5d3d05b9bfd07aa7088ff180d05044e359fdc9cebe0d97b -->
<!-- api: M:Inno.Text.TextStyle.ToString 8ea666be511421df26e6a3a488eff5a088a0e35eb872c0d4349aaea82e4af440 -->
<!-- api: M:Inno.Text.TextStyle.op_Inequality(Inno.Text.TextStyle,Inno.Text.TextStyle) b522b59abc7604511c02b217b448e49f93c7325e1e2954e1b02742ddf0baee87 -->
<!-- api: M:Inno.Text.TextStyle.op_Equality(Inno.Text.TextStyle,Inno.Text.TextStyle) 892b55bc254059678a7695f5d85e039a70c3fe2c5314c2f1e2cbda0fc17f6050 -->
<!-- api: M:Inno.Text.TextStyle.GetHashCode 7b793166346628e36a84510103cfe3989437e56d789915ac54bc3380ff91dd4a -->
<!-- api: M:Inno.Text.TextStyle.Equals(System.Object) 894700aabf3150b458b65221325146a27b91403be199fdcecfcf033863805915 -->
<!-- api: M:Inno.Text.TextStyle.Equals(Inno.Text.TextStyle) 80ba6534189ef6633960b47e2cc0404dadf1b9ef0176382b89045dbc430027a0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b74ac912f5552ad3"></a>[`public Inno.Text.TextStyle.TextStyle(float fontSize, int faceIndex = 0, int weight = 400, Inno.Text.TextFontStyle style = Inno.Text.TextFontStyle.Normal, float letterSpacing = 0)`](../../src/services/text/Inno.Text/TextTypes.cs#L89) | Creates validated text styling.<br>`fontSize`：The positive font size in logical pixels.<br>`faceIndex`：The zero-based face index within the font collection.<br>`weight`：The requested CSS-compatible weight from 1 through 1000.<br>`style`：The requested face slant.<br>`letterSpacing`：Additional logical pixels inserted after each glyph. | — | — |
| <a id="api-d596f6f4b47209f1"></a>[`public bool Inno.Text.TextStyle.Equals(Inno.Text.TextStyle other)`](../../src/services/text/Inno.Text/TextTypes.cs#L69) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-e35d82ee18357862"></a>[`public override bool Inno.Text.TextStyle.Equals(object obj)`](../../src/services/text/Inno.Text/TextTypes.cs#L69) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-736f18cba32326f1"></a>[`public override int Inno.Text.TextStyle.GetHashCode()`](../../src/services/text/Inno.Text/TextTypes.cs#L69) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-5efc7c044a2e0c0d"></a>[`public static bool Inno.Text.TextStyle.operator ==(Inno.Text.TextStyle left, Inno.Text.TextStyle right)`](../../src/services/text/Inno.Text/TextTypes.cs#L69) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-3177f10e6b02ed07"></a>[`public static bool Inno.Text.TextStyle.operator !=(Inno.Text.TextStyle left, Inno.Text.TextStyle right)`](../../src/services/text/Inno.Text/TextTypes.cs#L69) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-b1df1eace3d973b5"></a>[`public override string Inno.Text.TextStyle.ToString()`](../../src/services/text/Inno.Text/TextTypes.cs#L69) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-ed7e0152e7276ad6"></a>[`public static Inno.Text.TextStyle Inno.Text.TextStyle.defaultValue { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L115) | Gets a 16-pixel regular default style. | — | — |
| <a id="api-395a1ebfe5aa4205"></a>[`public int Inno.Text.TextStyle.faceIndex { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L123) | Gets the collection face index. | — | — |
| <a id="api-e30b5da5ba9a5a13"></a>[`public float Inno.Text.TextStyle.fontSize { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L119) | Gets the logical font size. | — | — |
| <a id="api-4bd17a5f70742eb3"></a>[`public float Inno.Text.TextStyle.letterSpacing { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L135) | Gets additional spacing appended to each glyph advance. | — | — |
| <a id="api-7b58043b1c14327c"></a>[`public Inno.Text.TextFontStyle Inno.Text.TextStyle.style { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L131) | Gets the requested face slant. | — | — |
| <a id="api-ff725b231f55caa9"></a>[`public int Inno.Text.TextStyle.weight { get; }`](../../src/services/text/Inno.Text/TextTypes.cs#L127) | Gets the CSS-compatible font weight. | — | — |
| <a id="api-db35d5629057cf0a"></a>[`public readonly record struct Inno.Text.TextStyle : System.IEquatable<Inno.Text.TextStyle>`](../../src/services/text/Inno.Text/TextTypes.cs#L69) | Defines immutable font selection and sizing for one shaping operation. | — | — |

## 项目依赖

- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
