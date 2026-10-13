# Inno.Native.Text

[分类索引](README.md) · [Wiki 首页](../../README.md) · [Text Toolchain](Inno.Build.Toolchains.Text.md)

手写 `Native/include/TextRuntime.hpp` 是窄 C++ 语义 facade，`Native/src/TextRuntime.cpp` 通过 PImpl 隐藏 FreeType 与 HarfBuzz。`Bindings/text.bridge.json` 由 BGCS Cpp2C 生成宿主 `Native/Generated` C 桥，`Bindings/bindgen.json` 再从生成的 C header 生成 `Generated/Bindings.cs`。本项目没有第二套手写 C export。ABI 只供所属 Text adapter、toolchain 和 native tests 使用，脚本及其他业务不引用。升级 facade、映射或第三方输入后，通过统一构建入口重新生成并编译同一目标闭包。

## ABI、初始化与失败

构建 profile 选择动态或静态符号解析。BGCS 生成的 `TextNative` 暴露 context 创建/销毁、face 加载/释放、metrics、UTF-8 shaping、glyph rasterization 及结果消息；`InnoText*` 数据结构只为 ABI 使用，不是引擎脚本协议。动态 profile 的类型初始化通过 `NativeDllLoader` 读取应用已部署的 `native/text/<platform>`，缺库即启动失败，不查询仓库构建缓存或提供伪排版结果。

`InnoTextRuntime` 与 face ID 由 [Text adapter](Inno.Adapter.Text.FreeTypeHarfBuzz.md) 独占。Runtime 复制字体输入并拥有全部 face，在销毁底层字体库之前释放 face；调用方不能跨 session 使用句柄。Shape 和 Rasterize 接收 caller-owned 缓冲区及容量，容量不足返回明确结果；UTF-8 结果说明为 borrowed process-lifetime 文本。绑定映射与 ownership 位于本项目 `Bindings/common.json`。

使用[绑定验收工具](../../build/Inno.Build.Cli.md)核对生成文件、实际库和完整部署输入。测试入口与当前环境结果分开记录；实际已执行范围见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)，单一宿主的通过不能推断其他平台已经通过。

## 操作与内存所有权

下面是后端实现中的最小字体测量工作流。前提是当前进程已部署与 managed binding 相同目标的 Text Native 库，初始化路径由所属 adapter 负责。示例在一个调用内独占 Runtime；正式 adapter 可以在自身生命周期内复用 Runtime 与已加载字体，避免反复解析字体。

```csharp
using System;
using Inno.Native.Text;

public static class FontMeasurement
{
    public static InnoTextMetrics Measure(
        ReadOnlySpan<byte> fontBytes,
        float fontSize
    ) {
        InnoTextRuntime runtime = TextNative.Create();
        if (runtime.IsNull)
            throw new InvalidOperationException("The native font session could not be created.");
        try
        {
            ulong font = 0;
            InnoTextResult loaded = TextNative.LoadFont(runtime, fontBytes, 0, ref font);
            if (loaded != InnoTextResult.Success)
                throw new InvalidOperationException(TextNative.ResultMessage(loaded));
            InnoTextMetrics metrics = default;
            InnoTextResult measured = TextNative.GetMetrics(runtime, font, fontSize, ref metrics);
            if (measured != InnoTextResult.Success)
                throw new InvalidOperationException(TextNative.ResultMessage(measured));
            return metrics;
        }
        finally
        {
            TextNative.Destroy(runtime);
        }
    }
}
```

下面的操作属于 Native adapter SPI，只有实际持有字体 Runtime 的实现可以调用。`InnoTextRuntime` 是句柄值；复制、比较或包装该值不会创建第二个 Runtime，也不会延长原生对象寿命。调用方在所有字体操作结束后调用一次 `Destroy`，不得销毁后继续使用副本。Runtime 内没有并发访问保护；所属 adapter 串行协调同一个 Runtime 的操作。

| 操作 | 行为与结果 | 调用约束 / 失败 |
| --- | --- | --- |
| `Create` / `Destroy` | 创建字体库会话；销毁时先释放其字体 face，再释放字体库 | 创建失败不能当作有效句柄继续调用；唯一 owner 负责最终销毁 |
| `LoadFont` | 复制输入字体字节，成功时写入会话内 font handle | 输入在调用期间有效；长度必须可表达、face index 非负。无效字体返回 `InvalidFont`，分配失败返回 `OutOfMemory` |
| `ReleaseFont` | 从该 Runtime 释放一个已加载 face | 零值返回 `InvalidArgument`，不存在的 handle 返回 `InvalidHandle`；不能跨 Runtime 传递 handle |
| `GetMetrics` | 按指定尺寸写入 ascender、descender、line height 和 underline 数据 | 输出内存由调用方提供；字号必须有限且大于零。失效 handle 或字号返回明确结果 |
| `ShapeUtf8` | 输出 glyph、UTF-8 cluster、advance 与 offset | `glyphs = null`、容量为零可查询数量；非空缓冲区不足返回 `BufferTooSmall`。文本长度是 UTF-8 字节长度；language/script 是调用期间有效的可选零结尾文本 |
| `RasterizeGlyph` | 写入 glyph 位图尺寸与布局，以及紧密排列的 8-bit coverage | `pixels = null`、容量为零先查询大小；调用方随后分配缓冲区。渲染失败返回 `BackendError`，容量不足返回 `BufferTooSmall` |
| `ResultMessage` | 把结果码转换成说明文本 | 原生文本是进程寿命的借用数据；生成的字符串封送不转移原生释放权 |

`InnoTextGlyph`、`InnoTextMetrics` 和 `InnoTextBitmap` 是 ABI 值结构。生成的 `*Ptr` 包装、索引器及 `ref` 属性借用对应原生内存，没有范围检查或自动释放；内存无效时不能读取这些属性。`Glyph.Cluster` 对应输入 UTF-8 字节位置，不能作为 C# UTF-16 字符索引。

桥接异常由生成层的 `GetLastError` / `ClearLastError` 协议诊断；它与上述 `InnoTextResult` 的领域失败分别处理。`InitApi` / `FreeApi` 属于生成绑定加载协议，由 Native owner 协调，不能用 `FreeApi` 代替逐 Runtime 清理。完整生成成员的签名与调用约束见下方清单；同一机制的各个 overload 分别列出。

## 静态目标

同一项目通过 `text.bridge.browser-wasm.json` 和 `bindgen.browser-wasm.json` 选择 wasm32 ABI 与静态符号解析，复用共同 facade 和映射。目标编译使用 `INNO_STATIC_NATIVE`，不初始化动态 loader。Host profile 通过类型初始化装载对应配置的库。目标 C 桥和 managed 源属于同一 `artifacts/bindings/text/browser-wasm/<generationFingerprint>`，分别位于 `Native` 和 `Generated/Bindings.cs`，不会覆盖宿主生成物。CMake 中间文件属于 Text 或 Browser toolchain 的目标与指纹目录。

## 源码归属

当前唯一源码 owner：`backends/Text/native/Inno.Native.Text/Inno.Native.Text.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

本清单覆盖当前宿主选择的全部 124 个公开声明，包含生成类型、overload、字段和指针包装。以下说明同时核对语义 facade 与当前生成调用体；生成文件保持只读。不同 ABI 的结构布局由对应目标生成与编译验证，不能照搬本机大小。

### `Inno.Native.Text.InnoTextBitmap`

<!-- api: T:Inno.Native.Text.InnoTextBitmap 26ffe362cbebe1cb3cda070df5ab30e6cccf3a9c12fd0f53381daa5abe969041 -->
<!-- api: F:Inno.Native.Text.InnoTextBitmap.Width d8fc73994dd9295d20c2676a868b2d04e4f6093342032597e3b5092ebe7a2a3d -->
<!-- api: F:Inno.Native.Text.InnoTextBitmap.Height e39a3f815464eca1a219a9f27f9827bb81e0942ed5d95bcc83693997df30e6c3 -->
<!-- api: F:Inno.Native.Text.InnoTextBitmap.BearingX 177039b41fb31220266c86c914c9f183fcfe4283034f912ccb6a42f917562c49 -->
<!-- api: F:Inno.Native.Text.InnoTextBitmap.BearingY 142a30b914c860633ba11f409902f0fe75d038bf253207ce8a384ee24b229a28 -->
<!-- api: F:Inno.Native.Text.InnoTextBitmap.AdvanceX 5c8628bd82bc608dfd8fc2230379930ba54c2081fc3b2d27ca7e662361ae3c0c -->
<!-- api: F:Inno.Native.Text.InnoTextBitmap.ByteLength 77a8bc196ada1fa8221d02fe7c36046c3a93aee4db01fab94caa9ad5e8989b2a -->
<!-- api: M:Inno.Native.Text.InnoTextBitmap.#ctor(System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.UInt32) 99bab39a02645917b5eaa9b6670d2eacf151000f204c339bb67f173e8e4d9de6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1f3dc9c17cd189e9"></a>[`public struct Inno.Native.Text.InnoTextBitmap`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L117) | 栅格化布局与所需 coverage 大小。 | 纯值描述，不拥有像素内存。 | 像素缓冲区由调用方另行提供。 |
| <a id="api-053a5c1627ae2839"></a>[`public int Inno.Native.Text.InnoTextBitmap.Width`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L119) | 位图宽度，单位为像素。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-749198c492b497c8"></a>[`public int Inno.Native.Text.InnoTextBitmap.Height`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L120) | 位图高度，单位为像素。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-3fc6302b896981d2"></a>[`public int Inno.Native.Text.InnoTextBitmap.BearingX`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L121) | 位图左边界相对于 glyph 原点的水平位置。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-866f92c594f33702"></a>[`public int Inno.Native.Text.InnoTextBitmap.BearingY`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L122) | 位图顶部相对于基线的垂直位置。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-4054b2d806b2fd11"></a>[`public float Inno.Native.Text.InnoTextBitmap.AdvanceX`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L123) | 水平方向排版推进量。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-45acf5ee7bff52f0"></a>[`public uint Inno.Native.Text.InnoTextBitmap.ByteLength`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L124) | 完整紧密排列 8-bit coverage 缓冲区所需的字节数。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-8673fa26d4b4b8ed"></a>[`public Inno.Native.Text.InnoTextBitmap.InnoTextBitmap(int width = 0, int height = 0, int bearing_x = 0, int bearing_y = 0, float advance_x = 0, uint byte_length = 0)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L125) | 按参数初始化对应结构字段；省略参数使用声明的默认值。 | 值复制不转移任何原生 owner。 | 默认零值不表示某次字体操作成功。 |

### `Inno.Native.Text.InnoTextBitmapPtr`

<!-- api: T:Inno.Native.Text.InnoTextBitmapPtr 328050a04dd6d53ece5573719ce7710a30feecdc5b6943586a1b89bdb89d2b7a -->
<!-- api: M:Inno.Native.Text.InnoTextBitmapPtr.#ctor(Inno.Native.Text.InnoTextBitmap*) 485d8a71ea0c99b21b028ba19835e950ca0c6ff35e0a2ace0d31328243ff7b34 -->
<!-- api: F:Inno.Native.Text.InnoTextBitmapPtr.Handle 9065698c4a8f5e0dc5bab8c9cb3731dadaeddc2c12a2268133eddb9477bb42f0 -->
<!-- api: P:Inno.Native.Text.InnoTextBitmapPtr.IsNull 156273b14138c243cb4293c67b26fec6bf8eae7f095dc4087d745d6dd2b16382 -->
<!-- api: P:Inno.Native.Text.InnoTextBitmapPtr.Null 667e820540f369abffe7c37320e08de06f23e71573f462169e81941486527415 -->
<!-- api: P:Inno.Native.Text.InnoTextBitmapPtr.Item(System.Int32) 01fdf64852c8df5ae560175d39d69f55ecbf79a6c6c8e40819c8a293b361222e -->
<!-- api: M:Inno.Native.Text.InnoTextBitmapPtr.op_Implicit(Inno.Native.Text.InnoTextBitmap*)~Inno.Native.Text.InnoTextBitmapPtr e74136caef926976737c3e9061ddf9e3c331798884b8769785b09693604c3a06 -->
<!-- api: M:Inno.Native.Text.InnoTextBitmapPtr.op_Implicit(Inno.Native.Text.InnoTextBitmapPtr)~Inno.Native.Text.InnoTextBitmap* 226fe42ef86112f6092bf2664d3284af72eaf8d3ab8f06979ba5dd2b702e9331 -->
<!-- api: M:Inno.Native.Text.InnoTextBitmapPtr.op_Equality(Inno.Native.Text.InnoTextBitmapPtr,Inno.Native.Text.InnoTextBitmapPtr) eec7e2bad4cdc66526adc82def59b958e3d1aa17fdcc8465bb7ccf4788d57f6b -->
<!-- api: M:Inno.Native.Text.InnoTextBitmapPtr.op_Inequality(Inno.Native.Text.InnoTextBitmapPtr,Inno.Native.Text.InnoTextBitmapPtr) 663c29dea16d96413052cae35f0b9d7ef1bcc6b36cfc6c908d2acbb971d5e7de -->
<!-- api: M:Inno.Native.Text.InnoTextBitmapPtr.Equals(Inno.Native.Text.InnoTextBitmapPtr) d24b3db54d70cb23671ddc85c1ba1e44c1a88cd6c2f062603ab585a1f272167c -->
<!-- api: M:Inno.Native.Text.InnoTextBitmapPtr.Equals(System.Object) bf89593fca5f2347cf65ba8de112b082d526c911669978d2ff8262e6a5368efe -->
<!-- api: M:Inno.Native.Text.InnoTextBitmapPtr.GetHashCode 237486a65657d94c2955a62daf079d1b1ebfb88726c753085d2224cda99e5d22 -->
<!-- api: P:Inno.Native.Text.InnoTextBitmapPtr.Width 7960374abdf2821bc3a4cb43ee6c35bc05cb388e13925cfae2f2886e92ef70ec -->
<!-- api: P:Inno.Native.Text.InnoTextBitmapPtr.Height 5cc7583b8799867e1afaf8d995c7f807d5cb1b8042cd4c617130ea8aa9478743 -->
<!-- api: P:Inno.Native.Text.InnoTextBitmapPtr.BearingX 93a25c191fead9a6bde7cbe473699034a009ae69777688d87d2cfcca060aea61 -->
<!-- api: P:Inno.Native.Text.InnoTextBitmapPtr.BearingY 31182b785b8b1d3c751a9b6da2ac06a0be2266ecae222c7962efc9c145cf7e57 -->
<!-- api: P:Inno.Native.Text.InnoTextBitmapPtr.AdvanceX 71088df7a135a8f4de0b310318b62a0f1c68606411961a76ef3ae8d32d43b831 -->
<!-- api: P:Inno.Native.Text.InnoTextBitmapPtr.ByteLength 94315971853718f30b93bc5df6c6f8a9a5b1f20f2252bf186a3881f6d617b15f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2ffaedea0c4c6741"></a>[`public struct Inno.Native.Text.InnoTextBitmapPtr : System.IEquatable<Inno.Native.Text.InnoTextBitmapPtr>`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L137) | 借用对应 ABI 结构指针的值包装。 | 不分配、固定或释放内存；原 owner 必须保持地址有效。 | 索引和 ref 属性没有容量、空指针或生命周期检查。 |
| <a id="api-937f8c0d24cba443"></a>[`public Inno.Native.Text.InnoTextBitmapPtr.InnoTextBitmapPtr(Inno.Native.Text.InnoTextBitmap* handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L139) | 包装传入的原生地址值，不创建或验证其目标。 | 借用原内存，不改变其 owner。 | 调用方自行保证类型、地址和生命周期。 |
| <a id="api-f69f01da8308e133"></a>[`public Inno.Native.Text.InnoTextBitmap* Inno.Native.Text.InnoTextBitmapPtr.Handle`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L144) | 读取或替换包装的原生地址。 | 不延长原对象或内存寿命。 | 更换地址不释放原地址，也不检查新地址有效性。 |
| <a id="api-22fb0e874cb2da3b"></a>[`public bool Inno.Native.Text.InnoTextBitmapPtr.IsNull { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L145) | 只比较地址是否为零。 | 无资源分配或销毁。 | 不检查非零地址是否悬空。 |
| <a id="api-3568d3851bc3500c"></a>[`public static Inno.Native.Text.InnoTextBitmapPtr Inno.Native.Text.InnoTextBitmapPtr.Null { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L146) | 返回零地址包装。 | 无资源分配或销毁。 | 不检查非零地址是否悬空。 |
| <a id="api-b4b720e3a1a495a8"></a>[`public Inno.Native.Text.InnoTextBitmap Inno.Native.Text.InnoTextBitmapPtr.this[int index] { get; set; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L148) | 读写 Handle[index] 对应的完整 ABI 值。 | 借用连续数组；调用方保证 index 位于真实分配范围内。 | 无边界检查；无效指针访问不能作为可恢复参数错误处理。 |
| <a id="api-1be31ecaa32b7737"></a>[`public static Inno.Native.Text.InnoTextBitmapPtr.implicit operator Inno.Native.Text.InnoTextBitmapPtr(Inno.Native.Text.InnoTextBitmap* handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L150) | 在原生地址与其 typed wrapper 之间转换。 | 仅复制地址，不转移释放权。 | 不验证地址、数组长度或目标类型。 |
| <a id="api-184af288d90514ed"></a>[`public static Inno.Native.Text.InnoTextBitmapPtr.implicit operator Inno.Native.Text.InnoTextBitmap*(Inno.Native.Text.InnoTextBitmapPtr handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L151) | 在原生地址与其 typed wrapper 之间转换。 | 仅复制地址，不转移释放权。 | 不验证地址、数组长度或目标类型。 |
| <a id="api-5e2bd3e596ae7614"></a>[`public static bool Inno.Native.Text.InnoTextBitmapPtr.operator ==(Inno.Native.Text.InnoTextBitmapPtr left, Inno.Native.Text.InnoTextBitmapPtr right)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L152) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-af809a07a651cce9"></a>[`public static bool Inno.Native.Text.InnoTextBitmapPtr.operator !=(Inno.Native.Text.InnoTextBitmapPtr left, Inno.Native.Text.InnoTextBitmapPtr right)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L153) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-9783d6d0c0ccb04f"></a>[`public bool Inno.Native.Text.InnoTextBitmapPtr.Equals(Inno.Native.Text.InnoTextBitmapPtr other)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L154) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-71e560505b7b85cb"></a>[`public override bool Inno.Native.Text.InnoTextBitmapPtr.Equals(object? obj)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L155) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-04691cb7f5612983"></a>[`public override int Inno.Native.Text.InnoTextBitmapPtr.GetHashCode()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L156) | 计算包装地址值的哈希。 | 不访问指针目标。 | 不可用于持久身份或跨会话定位。 |
| <a id="api-2e669a3b1444912f"></a>[`public ref int Inno.Native.Text.InnoTextBitmapPtr.Width { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L158) | 位图宽度，单位为像素。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-ea94cf9c1b5c124f"></a>[`public ref int Inno.Native.Text.InnoTextBitmapPtr.Height { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L159) | 位图高度，单位为像素。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-5525f14261448e12"></a>[`public ref int Inno.Native.Text.InnoTextBitmapPtr.BearingX { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L160) | 位图左边界相对于 glyph 原点的水平位置。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-7e4dd53722d6b63c"></a>[`public ref int Inno.Native.Text.InnoTextBitmapPtr.BearingY { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L161) | 位图顶部相对于基线的垂直位置。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-4f528a86d35fa3ba"></a>[`public ref float Inno.Native.Text.InnoTextBitmapPtr.AdvanceX { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L162) | 水平方向排版推进量。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-d1f1099bbd06027b"></a>[`public ref uint Inno.Native.Text.InnoTextBitmapPtr.ByteLength { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L163) | 完整紧密排列 8-bit coverage 缓冲区所需的字节数。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |

### `Inno.Native.Text.InnoTextDirection`

<!-- api: T:Inno.Native.Text.InnoTextDirection 9995f6aed62bf6860f12eaae06eeec8a1546d3a064fed7274b9f7d863b87ce7f -->
<!-- api: F:Inno.Native.Text.InnoTextDirection.Automatic 802d7257127c972843e72e6609b1deb66306a63f66d5c4a44d6d60a5d09bfd8f -->
<!-- api: F:Inno.Native.Text.InnoTextDirection.LeftToRight 0ae202a1b2f73a87ba601ddf067a7d6ee43284723e50bec35575d5418db8427f -->
<!-- api: F:Inno.Native.Text.InnoTextDirection.RightToLeft 6cc59570a063bf16599e8dfcda1b2ad051ce89d1f520a6e3907d3f1478e538bf -->
<!-- api: F:Inno.Native.Text.InnoTextDirection.TopToBottom 0e3f1fde10d946c2576925ca062bdd73f720510f1656135db75a21206aca0a7c -->
<!-- api: F:Inno.Native.Text.InnoTextDirection.BottomToTop 41ca30b5657e59c083fb028fdc5b04ac40ba7044959377f8366b39283f2aff5d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-82a1a87e16bd6e57"></a>[`public enum Inno.Native.Text.InnoTextDirection : int`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L36) | 调用方指定的 shaping 方向。 | 纯 ABI 值，不拥有资源。 | Automatic 允许根据输入推断方向。 |
| <a id="api-eda5d50469ea8214"></a>[`Inno.Native.Text.InnoTextDirection.Automatic = 0`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L38) | 从文本和其余 shaping 属性推断方向。 | 纯值，不拥有资源。 | 不包含字体或语言对象。 |
| <a id="api-92a5d74cfce6e093"></a>[`Inno.Native.Text.InnoTextDirection.LeftToRight = 1`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L39) | 按从左至右方向排版。 | 纯值，不拥有资源。 | 不包含字体或语言对象。 |
| <a id="api-b3c7be90e117c072"></a>[`Inno.Native.Text.InnoTextDirection.RightToLeft = 2`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L40) | 按从右至左方向排版。 | 纯值，不拥有资源。 | 不包含字体或语言对象。 |
| <a id="api-3bc66ef2ecc659a8"></a>[`Inno.Native.Text.InnoTextDirection.TopToBottom = 3`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L41) | 按从上至下方向排版。 | 纯值，不拥有资源。 | 不包含字体或语言对象。 |
| <a id="api-ea580f2f0c851b62"></a>[`Inno.Native.Text.InnoTextDirection.BottomToTop = 4`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L42) | 按从下至上方向排版。 | 纯值，不拥有资源。 | 不包含字体或语言对象。 |

### `Inno.Native.Text.InnoTextGlyph`

<!-- api: T:Inno.Native.Text.InnoTextGlyph c64b2416adf38e6109f672af07f995df77e37bda3a12a08a1a506496de492026 -->
<!-- api: F:Inno.Native.Text.InnoTextGlyph.GlyphId 031bfb99564009ff2129bbdc76813b61679fac1ce6c378c76a26bb04b9a2f906 -->
<!-- api: F:Inno.Native.Text.InnoTextGlyph.Cluster c849709806584e3a0c9c6f20d4668661ccda1127f04db3ca8ca018740deba964 -->
<!-- api: F:Inno.Native.Text.InnoTextGlyph.AdvanceX 7b16e7f69f510125320602df801fd2f81f145105e76d952c8a92467c0625246a -->
<!-- api: F:Inno.Native.Text.InnoTextGlyph.AdvanceY a8844453f45c63de2c2e336ad8b359d68ac4a374f1583b7eb6e678e1cfbc7c0b -->
<!-- api: F:Inno.Native.Text.InnoTextGlyph.OffsetX e7c9507fac44be99e6616d191b6094de754753990b49418bd4ed7884330a1a0b -->
<!-- api: F:Inno.Native.Text.InnoTextGlyph.OffsetY 4b7d372895254acc7d7c4d98a4829385c4e99428f3aa3abf7318f9735c772e16 -->
<!-- api: M:Inno.Native.Text.InnoTextGlyph.#ctor(System.UInt32,System.UInt32,System.Single,System.Single,System.Single,System.Single) f8bb729e105455e962eded99e5d6bf5365240f94e2bb8a9652a876a53f98bf2c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7b80e41e111acf59"></a>[`public struct Inno.Native.Text.InnoTextGlyph`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L73) | shaping 输出的单个 glyph 与位置数据。 | 纯值结构，可复制。 | Cluster 是 UTF-8 字节位置。 |
| <a id="api-cd5b7d25a717db87"></a>[`public uint Inno.Native.Text.InnoTextGlyph.GlyphId`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L75) | 当前字体内的 glyph 编号，用于后续栅格化。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-52d531f36904cbce"></a>[`public uint Inno.Native.Text.InnoTextGlyph.Cluster`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L76) | 原 UTF-8 输入的字节位置；不是 UTF-16 字符下标。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-3cb675a979b725f7"></a>[`public float Inno.Native.Text.InnoTextGlyph.AdvanceX`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L77) | 水平方向排版推进量。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-5a1169baaf042af4"></a>[`public float Inno.Native.Text.InnoTextGlyph.AdvanceY`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L78) | 垂直方向排版推进量。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-aceff7af45cc7184"></a>[`public float Inno.Native.Text.InnoTextGlyph.OffsetX`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L79) | glyph 相对当前位置的水平偏移。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-a1b0832db38ec5d3"></a>[`public float Inno.Native.Text.InnoTextGlyph.OffsetY`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L80) | glyph 相对当前位置的垂直偏移。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-b7b7003a633c9e4f"></a>[`public Inno.Native.Text.InnoTextGlyph.InnoTextGlyph(uint glyph_id = 0, uint cluster = 0, float advance_x = 0, float advance_y = 0, float offset_x = 0, float offset_y = 0)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L81) | 按参数初始化对应结构字段；省略参数使用声明的默认值。 | 值复制不转移任何原生 owner。 | 默认零值不表示某次字体操作成功。 |

### `Inno.Native.Text.InnoTextGlyphPtr`

<!-- api: T:Inno.Native.Text.InnoTextGlyphPtr 48ff3716160623c498e15aeca43541e4f25905c08454d6b6adbffa2df983a32a -->
<!-- api: M:Inno.Native.Text.InnoTextGlyphPtr.#ctor(Inno.Native.Text.InnoTextGlyph*) e2e05d440cf7f7c5c96db509af0a256005ea292973302c999b4bd8b4353b563d -->
<!-- api: F:Inno.Native.Text.InnoTextGlyphPtr.Handle 16315864268380fd65003fa900ec4e4c2d7d07e3e70e5878a15ac35feb07245f -->
<!-- api: P:Inno.Native.Text.InnoTextGlyphPtr.IsNull 4fb4bb5dc44253e8377d1eb6b7647bc5a76789a49c90c07f38c7cdf4013e4aa7 -->
<!-- api: P:Inno.Native.Text.InnoTextGlyphPtr.Null 84e26cc941c49e5cd17405550bde26f1fa7e546756c29fe1feb89a06267fa72a -->
<!-- api: P:Inno.Native.Text.InnoTextGlyphPtr.Item(System.Int32) c348783409f26dec48299aed84d3023e8fcb6d4228c44d39ad21f346fa8d6089 -->
<!-- api: M:Inno.Native.Text.InnoTextGlyphPtr.op_Implicit(Inno.Native.Text.InnoTextGlyph*)~Inno.Native.Text.InnoTextGlyphPtr 14496bc946d36b3b5c9fbb5284c330405f1258702010d03aed70c63d723a8e8b -->
<!-- api: M:Inno.Native.Text.InnoTextGlyphPtr.op_Implicit(Inno.Native.Text.InnoTextGlyphPtr)~Inno.Native.Text.InnoTextGlyph* febba1631db621042e18a2ba986df986c97671467adee7602c25706889e70897 -->
<!-- api: M:Inno.Native.Text.InnoTextGlyphPtr.op_Equality(Inno.Native.Text.InnoTextGlyphPtr,Inno.Native.Text.InnoTextGlyphPtr) 09bfc627cf133795debf19585cb5db4a6237d8758ff1c9247bd24a16353db88b -->
<!-- api: M:Inno.Native.Text.InnoTextGlyphPtr.op_Inequality(Inno.Native.Text.InnoTextGlyphPtr,Inno.Native.Text.InnoTextGlyphPtr) f60cedca775bb4073ed6704e324b0e0a89cfcf026a702c5bde58307d7f26bf8c -->
<!-- api: M:Inno.Native.Text.InnoTextGlyphPtr.Equals(Inno.Native.Text.InnoTextGlyphPtr) 3eded192bea2cb812a34d353603b7d33899016abe4ccbec6a0c58fbc3b6b24bc -->
<!-- api: M:Inno.Native.Text.InnoTextGlyphPtr.Equals(System.Object) bf001638d01df1f28ca9fa3941093b14538112bd86b5bdaa8d39c3b4d12b5f32 -->
<!-- api: M:Inno.Native.Text.InnoTextGlyphPtr.GetHashCode fffe703c43cdec8aaebc65ab16a1e7f19bd19a78639690bace80118f007a01ba -->
<!-- api: P:Inno.Native.Text.InnoTextGlyphPtr.GlyphId 673b56dedf5a1ac53f174cb40fc94fb562de4c6d06e14baad2b0fef270f8df59 -->
<!-- api: P:Inno.Native.Text.InnoTextGlyphPtr.Cluster e729fe8161dfc82ab288eb6678d246a89d299076944fd7017b54e3f6ecc25f21 -->
<!-- api: P:Inno.Native.Text.InnoTextGlyphPtr.AdvanceX 966ad210469b33940a2108c6d245f574590a16cf00daf10de1925cda194ddd99 -->
<!-- api: P:Inno.Native.Text.InnoTextGlyphPtr.AdvanceY ce3f5e571965e0b48f8b2631b668f98282e309d581fdcbf49bd848d925dba97e -->
<!-- api: P:Inno.Native.Text.InnoTextGlyphPtr.OffsetX f0f608fae38bc7cbed4b854988c0da49ee8e7d2a87602fa95813807edc1918e9 -->
<!-- api: P:Inno.Native.Text.InnoTextGlyphPtr.OffsetY ae1699be7ae9bc942ceb0263dd4b92195635134d84bfbb524d9a7bac75c882b3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6ce6aa5477a0eb54"></a>[`public struct Inno.Native.Text.InnoTextGlyphPtr : System.IEquatable<Inno.Native.Text.InnoTextGlyphPtr>`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L167) | 借用对应 ABI 结构指针的值包装。 | 不分配、固定或释放内存；原 owner 必须保持地址有效。 | 索引和 ref 属性没有容量、空指针或生命周期检查。 |
| <a id="api-8923ce3904fd3a9b"></a>[`public Inno.Native.Text.InnoTextGlyphPtr.InnoTextGlyphPtr(Inno.Native.Text.InnoTextGlyph* handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L169) | 包装传入的原生地址值，不创建或验证其目标。 | 借用原内存，不改变其 owner。 | 调用方自行保证类型、地址和生命周期。 |
| <a id="api-8f13c503a4f11c3e"></a>[`public Inno.Native.Text.InnoTextGlyph* Inno.Native.Text.InnoTextGlyphPtr.Handle`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L174) | 读取或替换包装的原生地址。 | 不延长原对象或内存寿命。 | 更换地址不释放原地址，也不检查新地址有效性。 |
| <a id="api-f966fd6692bc208e"></a>[`public bool Inno.Native.Text.InnoTextGlyphPtr.IsNull { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L175) | 只比较地址是否为零。 | 无资源分配或销毁。 | 不检查非零地址是否悬空。 |
| <a id="api-7cb7d5a515092063"></a>[`public static Inno.Native.Text.InnoTextGlyphPtr Inno.Native.Text.InnoTextGlyphPtr.Null { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L176) | 返回零地址包装。 | 无资源分配或销毁。 | 不检查非零地址是否悬空。 |
| <a id="api-a0fe9426a128c404"></a>[`public Inno.Native.Text.InnoTextGlyph Inno.Native.Text.InnoTextGlyphPtr.this[int index] { get; set; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L178) | 读写 Handle[index] 对应的完整 ABI 值。 | 借用连续数组；调用方保证 index 位于真实分配范围内。 | 无边界检查；无效指针访问不能作为可恢复参数错误处理。 |
| <a id="api-6fe8e3a6584fe165"></a>[`public static Inno.Native.Text.InnoTextGlyphPtr.implicit operator Inno.Native.Text.InnoTextGlyphPtr(Inno.Native.Text.InnoTextGlyph* handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L180) | 在原生地址与其 typed wrapper 之间转换。 | 仅复制地址，不转移释放权。 | 不验证地址、数组长度或目标类型。 |
| <a id="api-17a13d418eedd1be"></a>[`public static Inno.Native.Text.InnoTextGlyphPtr.implicit operator Inno.Native.Text.InnoTextGlyph*(Inno.Native.Text.InnoTextGlyphPtr handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L181) | 在原生地址与其 typed wrapper 之间转换。 | 仅复制地址，不转移释放权。 | 不验证地址、数组长度或目标类型。 |
| <a id="api-ab39354ec16a5b82"></a>[`public static bool Inno.Native.Text.InnoTextGlyphPtr.operator ==(Inno.Native.Text.InnoTextGlyphPtr left, Inno.Native.Text.InnoTextGlyphPtr right)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L182) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-82d7fab10a7d782e"></a>[`public static bool Inno.Native.Text.InnoTextGlyphPtr.operator !=(Inno.Native.Text.InnoTextGlyphPtr left, Inno.Native.Text.InnoTextGlyphPtr right)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L183) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-3404cc9473068c9c"></a>[`public bool Inno.Native.Text.InnoTextGlyphPtr.Equals(Inno.Native.Text.InnoTextGlyphPtr other)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L184) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-73a3f6774a0b71ec"></a>[`public override bool Inno.Native.Text.InnoTextGlyphPtr.Equals(object? obj)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L185) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-d040aefba57ed561"></a>[`public override int Inno.Native.Text.InnoTextGlyphPtr.GetHashCode()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L186) | 计算包装地址值的哈希。 | 不访问指针目标。 | 不可用于持久身份或跨会话定位。 |
| <a id="api-f5e0b69f595658f7"></a>[`public ref uint Inno.Native.Text.InnoTextGlyphPtr.GlyphId { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L188) | 当前字体内的 glyph 编号，用于后续栅格化。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-a76ac0cf62d73a70"></a>[`public ref uint Inno.Native.Text.InnoTextGlyphPtr.Cluster { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L189) | 原 UTF-8 输入的字节位置；不是 UTF-16 字符下标。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-b6a8f2b4e1a775e3"></a>[`public ref float Inno.Native.Text.InnoTextGlyphPtr.AdvanceX { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L190) | 水平方向排版推进量。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-09b3845d7813c4e3"></a>[`public ref float Inno.Native.Text.InnoTextGlyphPtr.AdvanceY { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L191) | 垂直方向排版推进量。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-fb60fe9072156b28"></a>[`public ref float Inno.Native.Text.InnoTextGlyphPtr.OffsetX { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L192) | glyph 相对当前位置的水平偏移。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-e572f08a4344e476"></a>[`public ref float Inno.Native.Text.InnoTextGlyphPtr.OffsetY { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L193) | glyph 相对当前位置的垂直偏移。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |

### `Inno.Native.Text.InnoTextMetrics`

<!-- api: T:Inno.Native.Text.InnoTextMetrics 97c7e33fc8bcee7434c6525224ef75bccd629c3502e33e991dcadbb8cda8f28c -->
<!-- api: F:Inno.Native.Text.InnoTextMetrics.Ascender e28a46d787e36d9b8504a7025d04ce78c71c6973be311399c726141130ba165d -->
<!-- api: F:Inno.Native.Text.InnoTextMetrics.Descender 19097e7c6122631f2735b0a75b7d6c50c57fba864b39666113b868eb2292ebb9 -->
<!-- api: F:Inno.Native.Text.InnoTextMetrics.LineHeight 4f68164e4206ce0742fa9fe1289bf695cad83d44096351c188f10b27b7207ab2 -->
<!-- api: F:Inno.Native.Text.InnoTextMetrics.UnderlinePosition 42c2a5da9725756f8f1616764b8d0dab065fd3ad71c7b7f6d3ea50ef7d9ed6d0 -->
<!-- api: F:Inno.Native.Text.InnoTextMetrics.UnderlineThickness d0a8136f4d34760c1cc341f3c8ffcbfb50ecf5828aa1fca33e814ae2512342bf -->
<!-- api: M:Inno.Native.Text.InnoTextMetrics.#ctor(System.Single,System.Single,System.Single,System.Single,System.Single) 765a6f273d71c9221bf049dfc2292ab44f803eed69c76ca44b384bff2fbb4b3f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-70f0ebecce407664"></a>[`public struct Inno.Native.Text.InnoTextMetrics`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L96) | 指定字号下的逻辑字体尺寸。 | 纯值结构，可复制。 | 数据仅在 GetMetrics 成功后有效。 |
| <a id="api-09c79b6c96ee9716"></a>[`public float Inno.Native.Text.InnoTextMetrics.Ascender`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L98) | 指定字号下的上行高度。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-b74b281990caf7e3"></a>[`public float Inno.Native.Text.InnoTextMetrics.Descender`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L99) | 指定字号下的下行位置。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-ce129317b2ae7fa6"></a>[`public float Inno.Native.Text.InnoTextMetrics.LineHeight`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L100) | 指定字号下的行高。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-14a2f138059e34a4"></a>[`public float Inno.Native.Text.InnoTextMetrics.UnderlinePosition`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L101) | 相对于基线的下划线位置。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-c1fe38f5088d5e10"></a>[`public float Inno.Native.Text.InnoTextMetrics.UnderlineThickness`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L102) | 按字号缩放并至少为 1 的下划线厚度。 | 值字段属于当前结构；复制不关联 Runtime。 | 输出仅在生成该数据的操作成功后解释。 |
| <a id="api-d59a043142b35e3e"></a>[`public Inno.Native.Text.InnoTextMetrics.InnoTextMetrics(float ascender = 0, float descender = 0, float line_height = 0, float underline_position = 0, float underline_thickness = 0)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L103) | 按参数初始化对应结构字段；省略参数使用声明的默认值。 | 值复制不转移任何原生 owner。 | 默认零值不表示某次字体操作成功。 |

### `Inno.Native.Text.InnoTextMetricsPtr`

<!-- api: T:Inno.Native.Text.InnoTextMetricsPtr 38919c6d26575111997cc0044853261d415fbf178d3d8b8a7fbddfeff93dd414 -->
<!-- api: M:Inno.Native.Text.InnoTextMetricsPtr.#ctor(Inno.Native.Text.InnoTextMetrics*) e1b5be94fcbc84b3b018f5b75e10f8204640d3ffc2a2e682754284dff44fb97a -->
<!-- api: F:Inno.Native.Text.InnoTextMetricsPtr.Handle e77c4aa1ede93548605dab857067b938f51b95af10503e3a100e5aa4d0700587 -->
<!-- api: P:Inno.Native.Text.InnoTextMetricsPtr.IsNull bd143cb59ff21670c3a58733e68c189a2fd7148b3946a53fd1a49e2a30fc5c43 -->
<!-- api: P:Inno.Native.Text.InnoTextMetricsPtr.Null 3a9376b31a0e55d60e925740a299ed20cfce103b3d08eb1d6a0cd856e8945421 -->
<!-- api: P:Inno.Native.Text.InnoTextMetricsPtr.Item(System.Int32) 1206563fd005b43da2cbb562aae47989a3829dfa3e7ee52de0608351f52e50e0 -->
<!-- api: M:Inno.Native.Text.InnoTextMetricsPtr.op_Implicit(Inno.Native.Text.InnoTextMetrics*)~Inno.Native.Text.InnoTextMetricsPtr f9bfcdd2d5d97e39a700639c706059577c875b135f1dbeb88ca4542c166fae25 -->
<!-- api: M:Inno.Native.Text.InnoTextMetricsPtr.op_Implicit(Inno.Native.Text.InnoTextMetricsPtr)~Inno.Native.Text.InnoTextMetrics* f357cb5334e7cdadd97db093b09d9873941cbb8f50483e8464eaa60ba4b165e0 -->
<!-- api: M:Inno.Native.Text.InnoTextMetricsPtr.op_Equality(Inno.Native.Text.InnoTextMetricsPtr,Inno.Native.Text.InnoTextMetricsPtr) 057c0a820068c4f1523ce6f98d53b35e2fd7d8d6a7f275fd2f6e770e84bf624d -->
<!-- api: M:Inno.Native.Text.InnoTextMetricsPtr.op_Inequality(Inno.Native.Text.InnoTextMetricsPtr,Inno.Native.Text.InnoTextMetricsPtr) a2f41ce1531ba9cb85f322bd72804bf15fa22613823be53300166bbfb8cbbbd2 -->
<!-- api: M:Inno.Native.Text.InnoTextMetricsPtr.Equals(Inno.Native.Text.InnoTextMetricsPtr) 25197ff69da7b6d39d95e2c04ff42cf55b5439f34e80881de8e6496bb8dbb7ac -->
<!-- api: M:Inno.Native.Text.InnoTextMetricsPtr.Equals(System.Object) cfcb892207b335556c1e1d914a2d8c3bfc0b7dfc957c6caa9ae8aeb3b074dbe0 -->
<!-- api: M:Inno.Native.Text.InnoTextMetricsPtr.GetHashCode a9e864c0b3b87c186a451e999bc961def3ced9ebb7f460b4cd34659f89bc7a92 -->
<!-- api: P:Inno.Native.Text.InnoTextMetricsPtr.Ascender b8e1ce91b811db607916770dda8108aa5a4221e46f4084da99ae9eab7b7944c1 -->
<!-- api: P:Inno.Native.Text.InnoTextMetricsPtr.Descender 185dc9016481191d278522bf545c90007ac5840572653faa182afd780bc4f931 -->
<!-- api: P:Inno.Native.Text.InnoTextMetricsPtr.LineHeight efd457aee653f0cf305334938ac1b46e4f4318ffdf4fd2f42e9a360527e5e2db -->
<!-- api: P:Inno.Native.Text.InnoTextMetricsPtr.UnderlinePosition c553b7218ff61f43fd0a78543a401a2d373bf807a8626ba4208ae50cb4989540 -->
<!-- api: P:Inno.Native.Text.InnoTextMetricsPtr.UnderlineThickness 56d0e95f0fd09d7057dc16be607187075b7b44535892b6d1f2dfb9ed036d8714 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c7653b1f6b3e8b66"></a>[`public struct Inno.Native.Text.InnoTextMetricsPtr : System.IEquatable<Inno.Native.Text.InnoTextMetricsPtr>`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L197) | 借用对应 ABI 结构指针的值包装。 | 不分配、固定或释放内存；原 owner 必须保持地址有效。 | 索引和 ref 属性没有容量、空指针或生命周期检查。 |
| <a id="api-dc9f7dfa71cfbe77"></a>[`public Inno.Native.Text.InnoTextMetricsPtr.InnoTextMetricsPtr(Inno.Native.Text.InnoTextMetrics* handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L199) | 包装传入的原生地址值，不创建或验证其目标。 | 借用原内存，不改变其 owner。 | 调用方自行保证类型、地址和生命周期。 |
| <a id="api-a60a2f8a252bf70c"></a>[`public Inno.Native.Text.InnoTextMetrics* Inno.Native.Text.InnoTextMetricsPtr.Handle`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L204) | 读取或替换包装的原生地址。 | 不延长原对象或内存寿命。 | 更换地址不释放原地址，也不检查新地址有效性。 |
| <a id="api-417db2d413eeff1a"></a>[`public bool Inno.Native.Text.InnoTextMetricsPtr.IsNull { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L205) | 只比较地址是否为零。 | 无资源分配或销毁。 | 不检查非零地址是否悬空。 |
| <a id="api-2b4f379210af7086"></a>[`public static Inno.Native.Text.InnoTextMetricsPtr Inno.Native.Text.InnoTextMetricsPtr.Null { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L206) | 返回零地址包装。 | 无资源分配或销毁。 | 不检查非零地址是否悬空。 |
| <a id="api-9dd2f803884f1e31"></a>[`public Inno.Native.Text.InnoTextMetrics Inno.Native.Text.InnoTextMetricsPtr.this[int index] { get; set; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L208) | 读写 Handle[index] 对应的完整 ABI 值。 | 借用连续数组；调用方保证 index 位于真实分配范围内。 | 无边界检查；无效指针访问不能作为可恢复参数错误处理。 |
| <a id="api-03f7b7daae395faa"></a>[`public static Inno.Native.Text.InnoTextMetricsPtr.implicit operator Inno.Native.Text.InnoTextMetricsPtr(Inno.Native.Text.InnoTextMetrics* handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L210) | 在原生地址与其 typed wrapper 之间转换。 | 仅复制地址，不转移释放权。 | 不验证地址、数组长度或目标类型。 |
| <a id="api-98cc0753a7c9689f"></a>[`public static Inno.Native.Text.InnoTextMetricsPtr.implicit operator Inno.Native.Text.InnoTextMetrics*(Inno.Native.Text.InnoTextMetricsPtr handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L211) | 在原生地址与其 typed wrapper 之间转换。 | 仅复制地址，不转移释放权。 | 不验证地址、数组长度或目标类型。 |
| <a id="api-51c4da5aefeb7582"></a>[`public static bool Inno.Native.Text.InnoTextMetricsPtr.operator ==(Inno.Native.Text.InnoTextMetricsPtr left, Inno.Native.Text.InnoTextMetricsPtr right)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L212) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-33f65d04aa09cc8a"></a>[`public static bool Inno.Native.Text.InnoTextMetricsPtr.operator !=(Inno.Native.Text.InnoTextMetricsPtr left, Inno.Native.Text.InnoTextMetricsPtr right)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L213) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-a7bb74aad1356772"></a>[`public bool Inno.Native.Text.InnoTextMetricsPtr.Equals(Inno.Native.Text.InnoTextMetricsPtr other)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L214) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-37c947b2cf9d5820"></a>[`public override bool Inno.Native.Text.InnoTextMetricsPtr.Equals(object? obj)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L215) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-5264dd4424d73a05"></a>[`public override int Inno.Native.Text.InnoTextMetricsPtr.GetHashCode()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L216) | 计算包装地址值的哈希。 | 不访问指针目标。 | 不可用于持久身份或跨会话定位。 |
| <a id="api-f407d13128ee12c3"></a>[`public ref float Inno.Native.Text.InnoTextMetricsPtr.Ascender { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L218) | 指定字号下的上行高度。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-55717fdef7c070bb"></a>[`public ref float Inno.Native.Text.InnoTextMetricsPtr.Descender { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L219) | 指定字号下的下行位置。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-2676ad62dccb7e34"></a>[`public ref float Inno.Native.Text.InnoTextMetricsPtr.LineHeight { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L220) | 指定字号下的行高。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-4a72108ba7a41a2e"></a>[`public ref float Inno.Native.Text.InnoTextMetricsPtr.UnderlinePosition { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L221) | 相对于基线的下划线位置。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |
| <a id="api-bedf015639ff3ca5"></a>[`public ref float Inno.Native.Text.InnoTextMetricsPtr.UnderlineThickness { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L222) | 按字号缩放并至少为 1 的下划线厚度。 | 返回原生字段的可写 ref；仅在原内存有效时使用。 | 无指针或寿命检查，不在源内存释放后保存 ref。 |

### `Inno.Native.Text.InnoTextResult`

<!-- api: T:Inno.Native.Text.InnoTextResult e7c4585e34f6ac87ac82f5b60e9c045a1ee0ed7a5a6051686dfd7bd0baa820d7 -->
<!-- api: F:Inno.Native.Text.InnoTextResult.UnknownError 9f5e500d876fe767d49a0ab34af74ef55cc9f71cd1539d4e6e771fc5c1d5e0c2 -->
<!-- api: F:Inno.Native.Text.InnoTextResult.Success 696228bb41a1d398d616c6e31d6f5b0bd1edca643fac786f1f2ac0c3ca023f89 -->
<!-- api: F:Inno.Native.Text.InnoTextResult.InvalidArgument 5c7e572752b82f69876f504bc7a59ce3bee8d84a8667e25b4e4fecb77872571b -->
<!-- api: F:Inno.Native.Text.InnoTextResult.OutOfMemory 81e5a4c5efa213c36101a2a49002eb0ccbf463b7832df6adeef44db581640b1d -->
<!-- api: F:Inno.Native.Text.InnoTextResult.InvalidFont ae382fed578c1e0fb406d435fb71382b8f2e154614e44e547ba4eb83e92061a1 -->
<!-- api: F:Inno.Native.Text.InnoTextResult.InvalidHandle 2e2013c1bef322c21ab022d1193bafbd6d0bd9c59473fc53d51f81d6f350b490 -->
<!-- api: F:Inno.Native.Text.InnoTextResult.BackendError 97a85a1f24424f166b6a78029963cd2b47ca94dc950b8f3535014752f804b810 -->
<!-- api: F:Inno.Native.Text.InnoTextResult.BufferTooSmall a9fe25c8e7aa4d264e80d9e41f220947cc784cb82c19234788c8f55b7aaf600b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d7491864bf4322c4"></a>[`public enum Inno.Native.Text.InnoTextResult : int`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L21) | 字体操作结果码；零值保留给桥接失败。 | 纯 ABI 值，不拥有资源。 | 只有 Success 表示领域操作成功。 |
| <a id="api-ebb2eeeb0b10908d"></a>[`Inno.Native.Text.InnoTextResult.UnknownError = 0`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L23) | 桥接层未能完成调用；读取当前线程的桥接错误。 | 纯值，不拥有资源。 | 值见当前声明；与桥接错误分开判断。 |
| <a id="api-a74aa162c5a48e90"></a>[`Inno.Native.Text.InnoTextResult.Success = 1`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L24) | 操作成功，按本次操作语义读取输出。 | 纯值，不拥有资源。 | 值见当前声明；与桥接错误分开判断。 |
| <a id="api-bd3960792c5041af"></a>[`Inno.Native.Text.InnoTextResult.InvalidArgument = 2`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L25) | 参数、容量或字号无效。 | 纯值，不拥有资源。 | 值见当前声明；与桥接错误分开判断。 |
| <a id="api-76c04f40199133e0"></a>[`Inno.Native.Text.InnoTextResult.OutOfMemory = 3`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L26) | 为当前操作分配内存失败。 | 纯值，不拥有资源。 | 值见当前声明；与桥接错误分开判断。 |
| <a id="api-9980aef858312201"></a>[`Inno.Native.Text.InnoTextResult.InvalidFont = 4`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L27) | 输入不是可加载的字体或指定 face 不可用。 | 纯值，不拥有资源。 | 值见当前声明；与桥接错误分开判断。 |
| <a id="api-c217f25023b26c3d"></a>[`Inno.Native.Text.InnoTextResult.InvalidHandle = 5`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L28) | 当前 Runtime 不包含所请求的字体 handle。 | 纯值，不拥有资源。 | 值见当前声明；与桥接错误分开判断。 |
| <a id="api-33221a390342fc59"></a>[`Inno.Native.Text.InnoTextResult.BackendError = 6`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L29) | 底层字体操作无法完成。 | 纯值，不拥有资源。 | 值见当前声明；与桥接错误分开判断。 |
| <a id="api-66ac323dceb8fbba"></a>[`Inno.Native.Text.InnoTextResult.BufferTooSmall = 7`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L30) | 输出缓冲区不足；查询所需容量后重新调用。 | 纯值，不拥有资源。 | 值见当前声明；与桥接错误分开判断。 |

### `Inno.Native.Text.InnoTextRuntime`

<!-- api: T:Inno.Native.Text.InnoTextRuntime 22c3139a7a61eda06cfdf993fe9ce89ad50fa0115863908c77e9aa4ce18c5a8d -->
<!-- api: M:Inno.Native.Text.InnoTextRuntime.#ctor(System.IntPtr) 640fe995cd25a6b3d7cb711b3858cd053ef7312cc1de0ac6d0275ce6c78150b2 -->
<!-- api: P:Inno.Native.Text.InnoTextRuntime.Handle ce920aed82d883eab941ce642309bf7050661e83fa5801c65cea6d7864c0453a -->
<!-- api: P:Inno.Native.Text.InnoTextRuntime.IsNull 399adfc0a1ccc3c3b7a9aa6610cea8915762d64335eac14ebbf85332bab5199d -->
<!-- api: P:Inno.Native.Text.InnoTextRuntime.Null 18f6b5779d76b355c96ba5300989e99869cbc561b785da7f45e2bf174d3056b6 -->
<!-- api: M:Inno.Native.Text.InnoTextRuntime.op_Implicit(System.IntPtr)~Inno.Native.Text.InnoTextRuntime a4550e9e285122df22bb7b45db3938fe5e38f1abae127c34effcb455cb859161 -->
<!-- api: M:Inno.Native.Text.InnoTextRuntime.op_Equality(Inno.Native.Text.InnoTextRuntime,Inno.Native.Text.InnoTextRuntime) d748a0a3a225027cd6076c3196dd440e05a125c5f1a555e58562bf77dce5897e -->
<!-- api: M:Inno.Native.Text.InnoTextRuntime.op_Inequality(Inno.Native.Text.InnoTextRuntime,Inno.Native.Text.InnoTextRuntime) 509c83b34f5983471db588c64b49e961f06b75dfc3c37297ffb8ab0a276dfa04 -->
<!-- api: M:Inno.Native.Text.InnoTextRuntime.Equals(Inno.Native.Text.InnoTextRuntime) 200ce52f944a092c3a50d16f0dfefd6695874583c7611dbf58affaffa7d57390 -->
<!-- api: M:Inno.Native.Text.InnoTextRuntime.Equals(System.Object) 4fdf8505949f72c10ee7e4bbabc8537a07c451a311e54b6f404ba8d0019dc617 -->
<!-- api: M:Inno.Native.Text.InnoTextRuntime.GetHashCode d711eb88612c840df447653f856f73727a41826271be7c62b243339bbe7884e0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-99805c04d265afba"></a>[`public readonly struct Inno.Native.Text.InnoTextRuntime : System.IEquatable<Inno.Native.Text.InnoTextRuntime>`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L49) | 不透明 Runtime handle 的值包装。 | 复制不转移或增加销毁权。 | IsNull 只检查零值，不检测已销毁的 Runtime。 |
| <a id="api-407423525f70b02c"></a>[`public Inno.Native.Text.InnoTextRuntime.InnoTextRuntime(nint handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L51) | 包装传入的原生地址值，不创建或验证其目标。 | 值复制不转移任何原生 owner。 | 默认零值不表示某次字体操作成功。 |
| <a id="api-80d0672d6e1f4e27"></a>[`public nint Inno.Native.Text.InnoTextRuntime.Handle { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L56) | 读取包装的原生地址。 | 不延长原对象或内存寿命。 | 更换地址不释放原地址，也不检查新地址有效性。 |
| <a id="api-93492a61a97280e4"></a>[`public bool Inno.Native.Text.InnoTextRuntime.IsNull { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L57) | 只比较地址是否为零。 | 无资源分配或销毁。 | 不检查非零地址是否悬空。 |
| <a id="api-776e464af7e50143"></a>[`public static Inno.Native.Text.InnoTextRuntime Inno.Native.Text.InnoTextRuntime.Null { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L58) | 返回零地址包装。 | 无资源分配或销毁。 | 不检查非零地址是否悬空。 |
| <a id="api-34097057ea15865e"></a>[`public static Inno.Native.Text.InnoTextRuntime.implicit operator Inno.Native.Text.InnoTextRuntime(nint handle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L60) | 在原生地址与其 typed wrapper 之间转换。 | 仅复制地址，不转移释放权。 | 不验证地址、数组长度或目标类型。 |
| <a id="api-41568523e5981560"></a>[`public static bool Inno.Native.Text.InnoTextRuntime.operator ==(Inno.Native.Text.InnoTextRuntime left, Inno.Native.Text.InnoTextRuntime right)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L61) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-16cb9b65e59ccc49"></a>[`public static bool Inno.Native.Text.InnoTextRuntime.operator !=(Inno.Native.Text.InnoTextRuntime left, Inno.Native.Text.InnoTextRuntime right)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L62) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-cb01531a84a14547"></a>[`public bool Inno.Native.Text.InnoTextRuntime.Equals(Inno.Native.Text.InnoTextRuntime other)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L63) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-b811e1d81c35421f"></a>[`public override bool Inno.Native.Text.InnoTextRuntime.Equals(object? obj)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L64) | 按原生地址值判断相等或不等。 | 不访问指针目标。 | 地址相等不能证明目标仍存活，也不能替代 generation 验证。 |
| <a id="api-4db2ceedf02eb09a"></a>[`public override int Inno.Native.Text.InnoTextRuntime.GetHashCode()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L65) | 计算包装地址值的哈希。 | 不访问指针目标。 | 不可用于持久身份或跨会话定位。 |

### `Inno.Native.Text.TextNative`

<!-- api: T:Inno.Native.Text.TextNative 45c1c0ffbc1e3fe3c543ef53e11669e064481d19965440215597309b941a7f87 -->
<!-- api: M:Inno.Native.Text.TextNative.GetLastError e910ddcb942f4bc23f62873eac8a33c2ae3fca330b64c177d859757bdd391afe -->
<!-- api: M:Inno.Native.Text.TextNative.ClearLastError 8d832fc45b875e74b58bb249edaf6f03c59c246e302a0b5fa06219367d6cf98d -->
<!-- api: M:Inno.Native.Text.TextNative.Create 169c2445feb4c4aa4b84e8f8caf54c5e955e96acfcc1f7815f01bffd18c55db7 -->
<!-- api: M:Inno.Native.Text.TextNative.Destroy(Inno.Native.Text.InnoTextRuntime) 6b3e67c1cac4191dfcc740165344cfd5af9e06e7d27d67cf1ade16c11684f56a -->
<!-- api: M:Inno.Native.Text.TextNative.LoadFont(Inno.Native.Text.InnoTextRuntime,System.Byte*,System.UInt32,System.Int32,System.UInt64*) 21a536903a062cfae1eaeebb6b7c2a8bb5461e8b7151fb0822a8a64faa4de6ae -->
<!-- api: M:Inno.Native.Text.TextNative.LoadFont(Inno.Native.Text.InnoTextRuntime,System.ReadOnlySpan{System.Byte},System.Int32,System.UInt64@) ca992eea7c068230ccb2bed9892266f4cbd7fff65cf8e1097a8f07b337806904 -->
<!-- api: M:Inno.Native.Text.TextNative.ReleaseFont(Inno.Native.Text.InnoTextRuntime,System.UInt64) d8aab85eef994bcfa434abc8da2880c517da23b123fc0e5d048b5bc509005fcd -->
<!-- api: M:Inno.Native.Text.TextNative.GetMetrics(Inno.Native.Text.InnoTextRuntime,System.UInt64,System.Single,Inno.Native.Text.InnoTextMetricsPtr) 84485b8b83a892135c06ea20b3e9754b1f52a437003cc396515186f71e55de07 -->
<!-- api: M:Inno.Native.Text.TextNative.GetMetrics(Inno.Native.Text.InnoTextRuntime,System.UInt64,System.Single,Inno.Native.Text.InnoTextMetrics@) 0ec52f1f59ccd25025c0ccfefb039a22a6cbb74327a9621615b3698d1f4543c8 -->
<!-- api: M:Inno.Native.Text.TextNative.ShapeUtf8(Inno.Native.Text.InnoTextRuntime,System.UInt64,System.Byte*,System.UInt32,System.Single,Inno.Native.Text.InnoTextDirection,System.Byte*,System.Byte*,Inno.Native.Text.InnoTextGlyphPtr,System.UInt32,System.UInt32*) 542970d34f352508be19f07d2e52f84d0ce15a1cdb3557a88c5872af414c5c18 -->
<!-- api: M:Inno.Native.Text.TextNative.ShapeUtf8(Inno.Native.Text.InnoTextRuntime,System.UInt64,System.Byte*,System.UInt32,System.Single,Inno.Native.Text.InnoTextDirection,System.Byte*,System.Byte*,Inno.Native.Text.InnoTextGlyph@,System.UInt32,System.UInt32*) ad049b68b07180f3fe6dd3d28a225ba8dce18e41c3f9f49520953a980b4cc9a9 -->
<!-- api: M:Inno.Native.Text.TextNative.ShapeUtf8(Inno.Native.Text.InnoTextRuntime,System.UInt64,System.String,System.UInt32,System.Single,Inno.Native.Text.InnoTextDirection,System.String,System.String,Inno.Native.Text.InnoTextGlyphPtr,System.UInt32,System.UInt32@) 42f6fb77632da56ec5aaa77e56d1d106885f71ac021ad45f4196d8e6edc1e3f2 -->
<!-- api: M:Inno.Native.Text.TextNative.RasterizeGlyph(Inno.Native.Text.InnoTextRuntime,System.UInt64,System.UInt32,System.Single,System.Byte*,System.UInt32,Inno.Native.Text.InnoTextBitmapPtr) 113cc6527d67f18d3d1ce8c820972051c77a61766d1c5e1b61167177fd3ef056 -->
<!-- api: M:Inno.Native.Text.TextNative.RasterizeGlyph(Inno.Native.Text.InnoTextRuntime,System.UInt64,System.UInt32,System.Single,System.Byte*,System.UInt32,Inno.Native.Text.InnoTextBitmap@) 6accddc44c29b158f5e2481a34af7e9bb2e1f3c7d78e35d2afda04f9e0e9a4b5 -->
<!-- api: M:Inno.Native.Text.TextNative.RasterizeGlyph(Inno.Native.Text.InnoTextRuntime,System.UInt64,System.UInt32,System.Single,System.Byte@,System.UInt32,Inno.Native.Text.InnoTextBitmapPtr) 2352f3eaf9ab710c7ec5f564c421da2e8a1893ff908bfe59d4a83e4d5a520560 -->
<!-- api: M:Inno.Native.Text.TextNative.ResultMessage(Inno.Native.Text.InnoTextResult) 4132bad62e69dffc67951d09e84ea2d7732a207c9f9b7bc14a14fa0c201b53ec -->
<!-- api: M:Inno.Native.Text.TextNative.InitApi(BGCS.Runtime.INativeContext) 1afac8c4adb02d2f534d177373bbec32f745607c1d8f5c2a91e6d1a1c32b1900 -->
<!-- api: M:Inno.Native.Text.TextNative.FreeApi ee8970f9a2fbdaa6cd95fa8f578cfc4ac13804937b85b05e8a74f4ed28289b5f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3ac6134c38063fce"></a>[`public static class Inno.Native.Text.TextNative`](../../../backends/Text/native/Inno.Native.Text/TextNative.cs#L10) | 字体语义 facade 的 managed 调用入口。 | 由所属 adapter 管理函数表、Runtime 和调用顺序。 | 仅后端 SPI；不向脚本导出。 |
| <a id="api-3afbb15ea94b1823"></a>[`public static byte* Inno.Native.Text.TextNative.GetLastError()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L351) | 读取当前线程最近一次 C 桥错误的 UTF-8 指针。 | 借用错误文本；立即复制需要保留的诊断，不释放原生指针。 | 不要跨后续桥接调用保存此指针；无错误时文本为空。 |
| <a id="api-774be7317c567104"></a>[`public static void Inno.Native.Text.TextNative.ClearLastError()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L358) | 清除当前线程的 C 桥错误。 | 不修改字体 Runtime 或释放字体资源。 | 不把清除诊断当作失败操作已成功。 |
| <a id="api-887be3e0551bc50a"></a>[`public static Inno.Native.Text.InnoTextRuntime Inno.Native.Text.TextNative.Create()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L364) | 创建一个隔离的字体 Runtime；成功返回非零 handle。 | 调用方接管 Runtime，全部操作结束后调用一次 Destroy。 | 桥接创建失败返回空 handle；用 GetLastError 读取原因。 |
| <a id="api-0a440cd2d8f4c14f"></a>[`public static void Inno.Native.Text.TextNative.Destroy(Inno.Native.Text.InnoTextRuntime self)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L371) | 销毁 Runtime 及其仍加载的所有字体。 | 唯一 owner 串行销毁；不接管其他 Runtime。 | 非零失效指针或重复销毁没有安全保护；销毁后所有副本失效。 |
| <a id="api-da8acdfbe1346882"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.LoadFont(Inno.Native.Text.InnoTextRuntime self, byte* data, uint length, int faceIndex, ulong* fontHandle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L377) | 复制字体字节，成功后写入 Runtime 内的 font handle。 | 输入只需在调用期间有效；成功后 Runtime 拥有复制的字体数据。 | 空输入、无效长度或 face index 返回 InvalidArgument；字体、分配和底层失败以结果码报告。 |
| <a id="api-94c170ab1acfc57d"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.LoadFont(Inno.Native.Text.InnoTextRuntime self, System.ReadOnlySpan<byte> data, int faceIndex, ref ulong fontHandle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L384) | 复制字体字节，成功后写入 Runtime 内的 font handle。 | 输入只需在调用期间有效；成功后 Runtime 拥有复制的字体数据。 Span overload 在调用期间固定输入，并从 Span.Length 传递字节长度。 | 空输入、无效长度或 face index 返回 InvalidArgument；字体、分配和底层失败以结果码报告。 |
| <a id="api-c847ec34531c7f01"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.ReleaseFont(Inno.Native.Text.InnoTextRuntime self, ulong fontHandle)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L396) | 释放当前 Runtime 内的一份字体及其底层 face。 | 只传入该 Runtime 成功返回的 font handle。 | 零值返回 InvalidArgument，不存在的 handle 返回 InvalidHandle。 |
| <a id="api-d593ba93a8c8f9d3"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.GetMetrics(Inno.Native.Text.InnoTextRuntime self, ulong fontHandle, float fontSize, Inno.Native.Text.InnoTextMetricsPtr metrics)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L403) | 按字号写入 ascender、descender、行高及下划线数据。 | 输出由调用方提供；同一 Runtime 上串行操作。 | 失效字体返回 InvalidHandle；输出或字号无效返回 InvalidArgument。 |
| <a id="api-61b57f61207adb17"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.GetMetrics(Inno.Native.Text.InnoTextRuntime self, ulong fontHandle, float fontSize, ref Inno.Native.Text.InnoTextMetrics metrics)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L410) | 按字号写入 ascender、descender、行高及下划线数据。 | 输出由调用方提供；同一 Runtime 上串行操作。 | 失效字体返回 InvalidHandle；输出或字号无效返回 InvalidArgument。 |
| <a id="api-b40db747209a5ada"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.ShapeUtf8(Inno.Native.Text.InnoTextRuntime self, ulong fontHandle, byte* text, uint textLength, float fontSize, Inno.Native.Text.InnoTextDirection direction, byte* language, byte* script, Inno.Native.Text.InnoTextGlyphPtr glyphs, uint glyphCapacity, uint* glyphCount)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L420) | 输出 glyph、UTF-8 cluster、advance 和 offset；glyphs 为 null、容量为零时查询数量。 | 容量按 glyph 数计；输出内存属于调用方。language/script 为调用期间有效的可选 UTF-8 文本。 | textLength 必须是实际 UTF-8 字节数；字符串 overload 不自动计算它。非空输出不足返回 BufferTooSmall。 |
| <a id="api-d58e05a7fac9f7fe"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.ShapeUtf8(Inno.Native.Text.InnoTextRuntime self, ulong fontHandle, byte* text, uint textLength, float fontSize, Inno.Native.Text.InnoTextDirection direction, byte* language, byte* script, ref Inno.Native.Text.InnoTextGlyph glyphs, uint glyphCapacity, uint* glyphCount)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L427) | 从所传 ref 指向的位置起写入连续 glyph；本 overload 不能表达 null 输出查询。 | 容量按 glyph 数计；输出内存属于调用方。language/script 为调用期间有效的可选 UTF-8 文本。 ref 只固定首元素，不验证后续 glyphCapacity 个元素的真实容量。 | textLength 必须是实际 UTF-8 字节数；字符串 overload 不自动计算它。非空输出不足返回 BufferTooSmall。 |
| <a id="api-9f372fff64bdd929"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.ShapeUtf8(Inno.Native.Text.InnoTextRuntime self, ulong fontHandle, string text, uint textLength, float fontSize, Inno.Native.Text.InnoTextDirection direction, string language, string script, Inno.Native.Text.InnoTextGlyphPtr glyphs, uint glyphCapacity, ref uint glyphCount)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L437) | 输出 glyph、UTF-8 cluster、advance 和 offset；glyphs 为 null、容量为零时查询数量。 | 容量按 glyph 数计；输出内存属于调用方。language/script 为调用期间有效的可选 UTF-8 文本。 字符串临时转为 UTF-8，调用结束释放临时内存。 | textLength 必须是实际 UTF-8 字节数；字符串 overload 不自动计算它。非空输出不足返回 BufferTooSmall。 |
| <a id="api-3f4804592100c7fe"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.RasterizeGlyph(Inno.Native.Text.InnoTextRuntime self, ulong fontHandle, uint glyphId, float fontSize, byte* pixels, uint pixelCapacity, Inno.Native.Text.InnoTextBitmapPtr bitmap)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L464) | 查询或填充紧密排列的 8-bit coverage，并写入 bitmap 描述。 | pixels 为 null、容量为零先查询；随后提供 ByteLength 字节的连续空间。 | ref byte 只固定首元素，不验证后续容量；无效字体、参数或底层栅格化失败以结果码报告。 |
| <a id="api-a4e5e69f5022139d"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.RasterizeGlyph(Inno.Native.Text.InnoTextRuntime self, ulong fontHandle, uint glyphId, float fontSize, byte* pixels, uint pixelCapacity, ref Inno.Native.Text.InnoTextBitmap bitmap)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L471) | 查询或填充紧密排列的 8-bit coverage，并写入 bitmap 描述。 | pixels 为 null、容量为零先查询；随后提供 ByteLength 字节的连续空间。 | ref byte 只固定首元素，不验证后续容量；无效字体、参数或底层栅格化失败以结果码报告。 |
| <a id="api-01273cc036853622"></a>[`public static Inno.Native.Text.InnoTextResult Inno.Native.Text.TextNative.RasterizeGlyph(Inno.Native.Text.InnoTextRuntime self, ulong fontHandle, uint glyphId, float fontSize, ref byte pixels, uint pixelCapacity, Inno.Native.Text.InnoTextBitmapPtr bitmap)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L481) | 从所传 ref byte 起写入 coverage；本 overload 不能表达 null 像素查询。 | 调用方提供至少 ByteLength 个连续字节；ref 只固定首元素。 | ref byte 只固定首元素，不验证后续容量；无效字体、参数或底层栅格化失败以结果码报告。 |
| <a id="api-d6d5188840feb973"></a>[`public static string? Inno.Native.Text.TextNative.ResultMessage(Inno.Native.Text.InnoTextResult result)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L490) | 复制结果码对应的原生 UTF-8 文本为 managed string。 | 原生说明为进程寿命借用数据；返回的字符串由 GC 管理。 | 底层返回空指针时返回 null；不能据此改变原操作结果。 |
| <a id="api-e465c6781c70a337"></a>[`public static void Inno.Native.Text.TextNative.InitApi(BGCS.Runtime.INativeContext context)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L500) | 完整解析本目标所需符号后切换函数表；解析失败释放候选并保留旧表。 | 仅 Native 初始化 owner 调用；不得与使用该函数表的调用并发。 FunctionTable 构造成功后接管 context；后续符号加载失败也会释放该 context。 | 缺少必需 export 时传播加载错误；本方法不创建字体 Runtime。 |
| <a id="api-a46455ecfecd4fa7"></a>[`public static void Inno.Native.Text.TextNative.FreeApi()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L527) | 释放生成函数表持有的解析上下文。 | 全部 Native 调用停止后由加载 owner 执行。 | 不会逐个销毁字体 Runtime；释放后不能继续调用函数表。 |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。
