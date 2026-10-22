# Inno.Native.Text

[分类索引](README.md) · [Wiki 首页](../../README.md) · [Text Toolchain](Inno.Build.Toolchains.Text.md)

手写 `Native/include/TextRuntime.hpp` 是窄 C++ 语义 facade，`Native/src/TextRuntime.cpp` 通过 PImpl 隐藏 FreeType 与 HarfBuzz。`Bindings/bindings.props` 选择正式 BGCS 配置；同一次 SDK 生成将 C++ facade 降为 C ABI，再产生 `Native/Generated` 和 `Generated/Bindings.cs`。本项目没有第二套手写 C export。ABI 只供所属 Text adapter、toolchain 和 native tests 使用，脚本及其他业务不引用。升级 facade、映射或第三方输入后，通过统一构建入口重新生成并编译同一目标闭包。

## ABI、初始化与失败

构建 profile 选择动态或静态符号解析。BGCS 生成的 `TextNative` 暴露 context 创建/销毁、face 加载/释放、metrics、UTF-8 shaping、glyph rasterization 及结果消息；`InnoText*` 数据结构只为 ABI 使用，不是引擎脚本协议。动态 profile 的类型初始化通过 `NativeDllLoader` 读取应用已部署的 `native/text/<platform>`，缺库即启动失败，不查询仓库构建缓存或提供伪排版结果。

`InnoTextFontAdapterRuntime*` 与 face ID 由 [Text adapter](Inno.Adapter.Text.FreeTypeHarfBuzz.md) 独占。Runtime 复制字体输入并拥有全部 face，在销毁底层字体库之前释放 face；调用方不能跨 session 使用句柄。Shape 和 Rasterize 接收 caller-owned 缓冲区及容量，容量不足返回明确结果；UTF-8 结果说明为 borrowed process-lifetime 文本。绑定映射与 ownership 位于本项目 `Bindings/bindgen.json`。

使用[绑定验收工具](../../build/Inno.Build.Cli.md)核对生成文件、实际库和完整部署输入。测试入口与当前环境结果分开记录；实际已执行范围见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)，单一宿主的通过不能推断其他平台已经通过。

## 操作与内存所有权

下面的后端示例借用 owner thread 上已创建的 Runtime 与已加载字体，只读取测量结果，不转移其销毁权。进程须部署与 managed binding 相同目标的原生库。`Metrics` 是经过生成存储断言验证的 DTO；其 `Native` 类型只用于 ABI 地址传递。

```csharp
using System;
using System.Runtime.InteropServices;
using Inno.Native.Text;

internal static unsafe class FontMeasurement
{
    internal static InnoTextFontAdapterMetrics Measure(
        InnoTextFontAdapterRuntime* runtime,
        ulong font,
        float fontSize
    ) {
        InnoTextFontAdapterMetrics metrics = default;
        Result result = default;
        BgcsInnoTextError* error = null;
        int status = TextNative.Raw.GetMetrics(runtime, font, fontSize,
            (InnoTextFontAdapterMetricsNative*)&metrics, &result, &error);
        try
        {
            if (status != 0 || error != null)
                throw new InvalidOperationException(error == null ? $"Bridge status: {status}"
                    : Marshal.PtrToStringUTF8((nint)TextNative.Raw.BgcsInnoTextErrorMessage(error)));
            if (result != Result.Success)
                throw new InvalidOperationException($"Font metrics failed: {result}");
            return metrics;
        }
        finally
        {
            if (error != null)
                TextNative.Raw.BgcsInnoTextErrorRelease(error);
        }
    }
}
```

下面的操作属于 Native adapter SPI，只有实际持有字体 Runtime 的实现可以调用。`InnoTextFontAdapterRuntime*` 是句柄值；复制、比较或包装该值不会创建第二个 Runtime，也不会延长原生对象寿命。调用方在所有字体操作结束后调用一次 `Destroy`，不得销毁后继续使用副本。Runtime 内没有并发访问保护；所属 adapter 串行协调同一个 Runtime 的操作。

| 操作 | 行为与结果 | 调用约束 / 失败 |
| --- | --- | --- |
| `Create` / `Destroy` | 创建字体库会话；销毁时先释放其字体 face，再释放字体库 | 创建失败不能当作有效句柄继续调用；唯一 owner 负责最终销毁 |
| `LoadFont` | 复制输入字体字节，成功时写入会话内 font handle | 输入在调用期间有效；长度必须可表达、face index 非负。无效字体返回 `InvalidFont`，分配失败返回 `OutOfMemory` |
| `ReleaseFont` | 从该 Runtime 释放一个已加载 face | 零值返回 `InvalidArgument`，不存在的 handle 返回 `InvalidHandle`；不能跨 Runtime 传递 handle |
| `GetMetrics` | 按指定尺寸写入 ascender、descender、line height 和 underline 数据 | 输出内存由调用方提供；字号必须有限且大于零。失效 handle 或字号返回明确结果 |
| `ShapeUtf8` | 输出 glyph、UTF-8 cluster、advance 与 offset | `glyphs = null`、容量为零可查询数量；非空缓冲区不足返回 `BufferTooSmall`。文本长度是 UTF-8 字节长度；language/script 是调用期间有效的可选零结尾文本 |
| `RasterizeGlyph` | 写入 glyph 位图尺寸与布局，以及紧密排列的 8-bit coverage | `pixels = null`、容量为零先查询大小；调用方随后分配缓冲区。渲染失败返回 `BackendError`，容量不足返回 `BufferTooSmall` |
| `ResultMessage` | 把结果码转换成说明文本 | 原生文本是进程寿命的借用数据；生成的字符串封送不转移原生释放权 |

`InnoTextFontAdapterGlyph`、`InnoTextFontAdapterMetrics` 和 `InnoTextFontAdapterBitmap` 是具有已验证布局的 ABI 存储结构。Raw 指针借用对应原生内存，没有自动释放或范围检查；地址无效时不能访问。`Glyph.Cluster` 对应输入 UTF-8 字节位置，不能作为 C# UTF-16 字符索引。

桥接调用返回 status，并独立写入 `Result` 与 `BgcsInnoTextError*`。错误对象仅属于这次调用，读取消息后必须由 `BgcsInnoTextErrorRelease` 释放；桥接失败与 `Result` 的领域失败分别检查。生成的 module context 只拥有函数加载会话，不能代替逐 Runtime 清理。完整生成成员的签名与调用约束见下方清单；同一机制的各个 overload 分别列出。

## 静态目标

组件仅维护公共 `Bindings/bindgen.json`。Browser Build integration 提供明确 wasm32 ABI、托管 runtime 和静态库名称；Windows、macOS integration 分别提供自己的 Profile，组件没有平台目标表。静态产品使用 `INNO_STATIC_NATIVE`，不初始化动态 loader。HostSource 输出属于组件的 `Generated/Bindings.cs` 和 `Native/Generated`；TargetArtifacts 的 managed/native 输出共同属于 `artifacts/bindings/text/<target>/<generationFingerprint>`。是否可运行以[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)为准。

## 源码归属

唯一源码 owner：`backends/Text/native/Inno.Native.Text/Inno.Native.Text.csproj`。共同领域与平台产品通过明确契约组合。

## 当前源码公开 API 清单

以下签名来自当前源码与正式生成结果；原生所有权、线程及关闭顺序见本页前文。

<!-- api: F:Inno.Native.Text.Direction.Automatic b526cd0d8b186a047a409a4fe094447e1718437bbab91a269d7b6641c63fd65f -->
<!-- api: F:Inno.Native.Text.Direction.BottomToTop 94e538aaf4a742d69c9fe660148d4ef5530062747057a17cd8110d463ee6875f -->
<!-- api: F:Inno.Native.Text.Direction.LeftToRight f9e169a1f76c398bc1ee887539bddd09494bd26e11e32bd0f763b2928f4d6d42 -->
<!-- api: F:Inno.Native.Text.Direction.RightToLeft 46f3b8b8a04cf7f28205330b476c89e67d67cd3fb080036a67b15889e62a4d57 -->
<!-- api: F:Inno.Native.Text.Direction.TopToBottom 01a37c0eb2b22184897c8f29f7723d0ff39e2fbf0c71f6d16a2e61e83d604483 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterBitmap.AdvanceX 6e85082d3e8fc3b90fb82ef71faae295be77b6b39a6ad66361202a5ce09ab3cd -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterBitmap.BearingX b4893982bf1a288e6cfe5489fdbcf142804f1e734fea339dbc42c39e48ea7902 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterBitmap.BearingY b75e175e07c1fffeaff04ec04578b6b5e222fd67a09727aa3b5d7f6691db6d17 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterBitmap.ByteLength d6c46eb4279a28f9be88739869d13748db8929f044a9b3bbadc8ac3d24b6f491 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterBitmap.Height 5241b5baec79c30a8e5e53c2db06c00fa7c7f877b3ccd6267323df944285572d -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterBitmap.Width 0d7623c0ebb15d93dde4415203c57f692a7cd2198c5a78b1a7babc5ca7696e16 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterGlyph.AdvanceX 9ac58a2a7ea3891267834915ed1d528d1b963658e9b1eb5fd2dfa28ad6f43d63 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterGlyph.AdvanceY ebe1418e8a16b50f5771b51170d1798db82c0d0527b3c72dd838ef9723a93551 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterGlyph.Cluster d2d6814a065ca42596af62328f8683d4f053b3eb7754cd07fa8079092ea49aff -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterGlyph.GlyphId 2f4044931490b4cc74753fa5445ce02095169e67f9abb979514b07cd4debe688 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterGlyph.OffsetX ffbf01a96540ed48c863f1c50d23314c283b240460289ad06cf68124ac254cd4 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterGlyph.OffsetY e98808d19744b59e8e8ee4ddda60f854ea531de118d64227fd09ef4e1b4c6c70 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterMetrics.Ascender 494e5518b84e8a6ff03b379ac6927bafb06f4dd2b7cf4cbaba39c7bbab6343d3 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterMetrics.Descender 5ddf6aa73deaef854207b81a08d95506b74ba05e6e81a2987fb2016797810efa -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterMetrics.LineHeight 204852fd87fbb370698ae65c338356af7e3da16b556b348d516469b9d5682c2d -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterMetrics.UnderlinePosition cd53fab32af48cbb8f7d09c2f404037bda907013b46be4950df26e13c2fcc7e7 -->
<!-- api: F:Inno.Native.Text.InnoTextFontAdapterMetrics.UnderlineThickness da87b3240b11cae7583d230ade884eabfa45101a573e37b9f0234f231503092b -->
<!-- api: F:Inno.Native.Text.Result.BackendError 20140238786a1b3aa5e647277153596e821109331eaf3956ee0ea31002408cc7 -->
<!-- api: F:Inno.Native.Text.Result.BufferTooSmall 1f2717c72eefa5d696c8761504baf9107601c94aa90d4160d5b909ca5a903ce3 -->
<!-- api: F:Inno.Native.Text.Result.InvalidArgument 13318ce758e1bfb96b2b0df2edd94d1c2ac3a229e90b4b364ad6a3175af547e1 -->
<!-- api: F:Inno.Native.Text.Result.InvalidFont f91ba370fd52af27e6a8f124bd3a954252ea375c287a6f94adb22e620b50842d -->
<!-- api: F:Inno.Native.Text.Result.InvalidHandle f28d5a31f1652e16ef51a2c0ef49ba28aa8740d4641937f3868d00dfe5c40d60 -->
<!-- api: F:Inno.Native.Text.Result.OutOfMemory 0734e4bc16239ad35527cbde32b052ef7814ab195cd6561129069003db5300b1 -->
<!-- api: F:Inno.Native.Text.Result.Success 289b42e54b52b6d1ea5744a31a214f1779ec2e1140950c5bb2e6a8418ffe69cc -->
<!-- api: F:Inno.Native.Text.Result.UnknownError c0e2e6cc79a7110812491fc77171c72ac8ea0fef6f801e2b5bf85d386d6b4df9 -->
<!-- api: F:Inno.Native.Text.TextNative.NativeLibraryName 788a2a3e2a23983f6fb3739c907002fb070ca34c62f9fe5ec0181f4a76eaebbb -->
<!-- api: F:Inno.Native.Text.TextNativeContext.NativeLibraryName 855bdeae738fa37a5ff85a7d711082ffdb6c352d5b389ae166d1efe77a609ea6 -->
<!-- api: M:Inno.Native.Text.TextNative.Dispose f6b08b02ce44154e8749ce49d014ec45fcc055f4b92ea42c01c8fff882161aa8 -->
<!-- api: M:Inno.Native.Text.TextNative.DisposeAsync c449b2addf21589ad3eeb029803dd8f68b25fb74a5f6cbfc351e612d331d00ce -->
<!-- api: M:Inno.Native.Text.TextNative.FromProvider(BGCS.Runtime.INativeModuleProvider) f3970ec35003cf796f0422de8baaf2f51586fc9e9b531ef639f249d28bc65a79 -->
<!-- api: M:Inno.Native.Text.TextNative.Load(System.String) f44a6c23cc942db3c15b17c6039789d0e31c87f53bbfa8f17adc06bfa898529c -->
<!-- api: M:Inno.Native.Text.TextNativeContext.Dispose ec1ab280297c5ccb396e236e61da8ecaf33c1b0d602e5216811755c665229f8b -->
<!-- api: M:Inno.Native.Text.TextNativeContext.DisposeAsync d3b646055c0007c8e68cbb98172c74510f4cbdea15a565cefb456d97ad7299be -->
<!-- api: M:Inno.Native.Text.TextNativeContext.FromProvider(BGCS.Runtime.INativeModuleProvider) e7a0cf1b91f8a1d052353b609e73c1a306c97561f531942c7909f88451806c59 -->
<!-- api: M:Inno.Native.Text.TextNativeContext.Load(System.String) 81e925a99d81448871796f4e5128f887d42cee7a97662ebf2c92b32661ed45d5 -->
<!-- api: M:Inno.Native.Text.TextNativeContext.RawApi.BgcsInnoTextErrorMessage(Inno.Native.Text.BgcsInnoTextError*) 1be357b1b9dc091858a6693bcb4fc7dd7562a8fef43410cdf0c039b9f0495d9f -->
<!-- api: M:Inno.Native.Text.TextNativeContext.RawApi.BgcsInnoTextErrorRelease(Inno.Native.Text.BgcsInnoTextError*) f1cded0a00f90be2d953cfc2b06c0eabe32299340892c07bd0ce6bc4b09b07fc -->
<!-- api: M:Inno.Native.Text.TextNativeContext.RawApi.CreateRuntime(Inno.Native.Text.InnoTextFontAdapterRuntime**,Inno.Native.Text.BgcsInnoTextError**) aaa1582b8c0d36e2793cbbaf88474ba7dd0f7713b4b1b085317708d5a3e9b839 -->
<!-- api: M:Inno.Native.Text.TextNativeContext.RawApi.DestroyRuntime(Inno.Native.Text.InnoTextFontAdapterRuntime*,Inno.Native.Text.BgcsInnoTextError**) fbeadbe49eb2577677de7c914d9d5890116f38d907a1f44b07e6195fa1231fd4 -->
<!-- api: M:Inno.Native.Text.TextNativeContext.RawApi.GetMetrics(Inno.Native.Text.InnoTextFontAdapterRuntime*,System.UInt64,System.Single,Inno.Native.Text.InnoTextFontAdapterMetricsNative*,Inno.Native.Text.Result*,Inno.Native.Text.BgcsInnoTextError**) d9ae9aa663c274878b07368683a91adae482d96d8ccffe19bc33bfb2516b848e -->
<!-- api: M:Inno.Native.Text.TextNativeContext.RawApi.LoadFont(Inno.Native.Text.InnoTextFontAdapterRuntime*,System.Byte*,System.UInt32,System.Int32,System.UInt64*,Inno.Native.Text.Result*,Inno.Native.Text.BgcsInnoTextError**) 886267b334e6fe763a700deae077dc7062f0d72481974d12fb1d1679cfb10728 -->
<!-- api: M:Inno.Native.Text.TextNativeContext.RawApi.RasterizeGlyph(Inno.Native.Text.InnoTextFontAdapterRuntime*,System.UInt64,System.UInt32,System.Single,System.Byte*,System.UInt32,Inno.Native.Text.InnoTextFontAdapterBitmapNative*,Inno.Native.Text.Result*,Inno.Native.Text.BgcsInnoTextError**) 1bcfd597bacf12d6e4b57b280201a81b66cbfa7dc24d959bf27239009008cd30 -->
<!-- api: M:Inno.Native.Text.TextNativeContext.RawApi.ReleaseFont(Inno.Native.Text.InnoTextFontAdapterRuntime*,System.UInt64,Inno.Native.Text.Result*,Inno.Native.Text.BgcsInnoTextError**) 34c0eb0dac2402e86f93adbe095dfa89bcb3f6cd00acdebac081a0ebbf587e97 -->
<!-- api: M:Inno.Native.Text.TextNativeContext.RawApi.ResultMessage(Inno.Native.Text.Result,System.SByte**,Inno.Native.Text.BgcsInnoTextError**) 13a769d973e9d950e9467d4a04d6f9be0120881a14230e7076de71c9c391cf29 -->
<!-- api: M:Inno.Native.Text.TextNativeContext.RawApi.ShapeUtf8(Inno.Native.Text.InnoTextFontAdapterRuntime*,System.UInt64,System.SByte*,System.UInt32,System.Single,Inno.Native.Text.Direction,System.SByte*,System.SByte*,Inno.Native.Text.InnoTextFontAdapterGlyphNative*,System.UInt32,System.UInt32*,Inno.Native.Text.Result*,Inno.Native.Text.BgcsInnoTextError**) ea7df91ae4acaf743ac6d1ca603feecd6a5f1a95daaffd97e6ebccf4b1692281 -->
<!-- api: P:Inno.Native.Text.TextNative.IsInitialized b5a9476b305aa1bdce159eafc0f918f2bde60f905b6c6ff0b5b1a5a2e24f8775 -->
<!-- api: P:Inno.Native.Text.TextNative.Raw 9ccd86a7f0c3bf8488f4b8fa0699d527b55a9eddf9a4c052c4eb09427ce9b3b8 -->
<!-- api: P:Inno.Native.Text.TextNativeContext.Raw 78e81000f40fad6045b08018e71c4464ff03f3811b6226d226f97283876331aa -->
<!-- api: T:Inno.Native.Text.BgcsInnoTextError cd7137dfc32cae89ad5851e7811a253a16102f2b337257bedd2fd197c0053e65 -->
<!-- api: T:Inno.Native.Text.Direction e92ce67986375f597530562ca79ab1a8b2734a183d316b84e8c8a564baec0ed9 -->
<!-- api: T:Inno.Native.Text.InnoTextFontAdapterBitmap 9f288101d07b38a0ecb3b28a56a9b82baf2cf2d49976bbe4665d356b673f9011 -->
<!-- api: T:Inno.Native.Text.InnoTextFontAdapterBitmapNative 90d315bfa4cf82382795bdeab91d2e35dc640cd60cbecb39b8e6e8132a37f1b5 -->
<!-- api: T:Inno.Native.Text.InnoTextFontAdapterGlyph e346ed51aec5cb36ad2e26db43a0b92832b39b2c9ac8c5ee40a0dfe39e986a81 -->
<!-- api: T:Inno.Native.Text.InnoTextFontAdapterGlyphNative 7d4d5aeffc8a89f3f8f18276d5e929b3161bb9baa1c337bb9be9c7dc5c8e6a59 -->
<!-- api: T:Inno.Native.Text.InnoTextFontAdapterMetrics d0e73dff5bc128c10e8a1d988c2d1b9dd609970858f01435e9a8f83a60425a48 -->
<!-- api: T:Inno.Native.Text.InnoTextFontAdapterMetricsNative e201b1c3bdb23696795ca036c07541b8ddec2c360d8be168f49a2745e792cc82 -->
<!-- api: T:Inno.Native.Text.InnoTextFontAdapterRuntime 5a10855f372e9fb9d253a3fe8af82c1cffcd16ae761af2f5c5711562cc55be28 -->
<!-- api: T:Inno.Native.Text.Result 8d7065ce2745939640cbbbfec8c7c31b89c498f5d8ad468d92536b7a88b0f7fd -->
<!-- api: T:Inno.Native.Text.TextNative 45c1c0ffbc1e3fe3c543ef53e11669e064481d19965440215597309b941a7f87 -->
<!-- api: T:Inno.Native.Text.TextNativeContext b270e229893370624b1467da5f60eafbd09bfd791cef1e31e3e6fec138b4a28c -->
<!-- api: T:Inno.Native.Text.TextNativeContext.RawApi c538104bc27f6bdfca6b89f586aa52aad6965420cfdd2ce629469722d95c40bb -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-60c2587c3b24367e"></a>[`Inno.Native.Text.Direction.Automatic = 0`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L264) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-9ddd6df890a88b5e"></a>[`Inno.Native.Text.Direction.BottomToTop = 4`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L268) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-b4bad6b800a5dcbe"></a>[`Inno.Native.Text.Direction.LeftToRight = 1`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L265) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-57ce75db06547618"></a>[`Inno.Native.Text.Direction.RightToLeft = 2`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L266) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-f136afff99356ec8"></a>[`Inno.Native.Text.Direction.TopToBottom = 3`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L267) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-62f5f27fecd5920b"></a>[`public float Inno.Native.Text.InnoTextFontAdapterBitmap.AdvanceX`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L290) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b45e82eb4969c177"></a>[`public int Inno.Native.Text.InnoTextFontAdapterBitmap.BearingX`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L286) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ba68dc525a8f98b"></a>[`public int Inno.Native.Text.InnoTextFontAdapterBitmap.BearingY`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L288) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d145ae687e655b95"></a>[`public uint Inno.Native.Text.InnoTextFontAdapterBitmap.ByteLength`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L292) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ced57812e9d07e62"></a>[`public int Inno.Native.Text.InnoTextFontAdapterBitmap.Height`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L284) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-71015b820c30067b"></a>[`public int Inno.Native.Text.InnoTextFontAdapterBitmap.Width`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L282) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd2b165f6f088609"></a>[`public float Inno.Native.Text.InnoTextFontAdapterGlyph.AdvanceX`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L321) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c3b978fb056d033"></a>[`public float Inno.Native.Text.InnoTextFontAdapterGlyph.AdvanceY`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L323) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a69b4e936f7140ef"></a>[`public uint Inno.Native.Text.InnoTextFontAdapterGlyph.Cluster`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L319) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d4b5cf230dc70723"></a>[`public uint Inno.Native.Text.InnoTextFontAdapterGlyph.GlyphId`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L317) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-93fcf610b9bf42ad"></a>[`public float Inno.Native.Text.InnoTextFontAdapterGlyph.OffsetX`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L325) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f14cea7254fcc10e"></a>[`public float Inno.Native.Text.InnoTextFontAdapterGlyph.OffsetY`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L327) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-82827439a056dc20"></a>[`public float Inno.Native.Text.InnoTextFontAdapterMetrics.Ascender`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L352) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8f64f97cceb30000"></a>[`public float Inno.Native.Text.InnoTextFontAdapterMetrics.Descender`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L354) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ff503281c91c5c8"></a>[`public float Inno.Native.Text.InnoTextFontAdapterMetrics.LineHeight`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L356) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d5c7c335b4f7bad"></a>[`public float Inno.Native.Text.InnoTextFontAdapterMetrics.UnderlinePosition`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L358) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e0229a8c69e5558b"></a>[`public float Inno.Native.Text.InnoTextFontAdapterMetrics.UnderlineThickness`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L360) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4beaa07cea10bb59"></a>[`Inno.Native.Text.Result.BackendError = 6`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L399) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-8015954522f3003f"></a>[`Inno.Native.Text.Result.BufferTooSmall = 7`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L400) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-5a78e5213e8e75d0"></a>[`Inno.Native.Text.Result.InvalidArgument = 2`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L395) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-79a4a13ec8e3223c"></a>[`Inno.Native.Text.Result.InvalidFont = 4`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L397) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-f3291eef250fa190"></a>[`Inno.Native.Text.Result.InvalidHandle = 5`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L398) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-01a38a2482d49731"></a>[`Inno.Native.Text.Result.OutOfMemory = 3`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L396) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-0c55a683841b2730"></a>[`Inno.Native.Text.Result.Success = 1`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L394) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-e6432c3cdb3db9f3"></a>[`Inno.Native.Text.Result.UnknownError = 0`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L393) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-28824d16509453fe"></a>[`public const string Inno.Native.Text.TextNative.NativeLibraryName = "inno-text"`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L18) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`inno-text` |
| <a id="api-9b750a12a3d8966d"></a>[`public const string Inno.Native.Text.TextNativeContext.NativeLibraryName = "inno-text"`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L48) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`inno-text` |
| <a id="api-2e1a18c806b744bb"></a>[`public static void Inno.Native.Text.TextNative.Dispose()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L32) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9cd1a47fea19e5ba"></a>[`public static System.Threading.Tasks.ValueTask Inno.Native.Text.TextNative.DisposeAsync()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L33) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-17a2cf003208c487"></a>[`public static void Inno.Native.Text.TextNative.FromProvider(BGCS.Runtime.INativeModuleProvider provider)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L30) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c1bcb294e727fe4"></a>[`public static void Inno.Native.Text.TextNative.Load(string libraryPath = "inno-text")`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L31) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-204d098d7d02478d"></a>[`public void Inno.Native.Text.TextNativeContext.Dispose()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L137) | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. |
| <a id="api-25ee478949eace35"></a>[`public System.Threading.Tasks.ValueTask Inno.Native.Text.TextNativeContext.DisposeAsync()`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L138) | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.<br>返回：A task that represents the asynchronous dispose operation. |
| <a id="api-b4c84101fe763b86"></a>[`public static Inno.Native.Text.TextNativeContext Inno.Native.Text.TextNativeContext.FromProvider(BGCS.Runtime.INativeModuleProvider provider)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L135) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84537a39be6ff0ec"></a>[`public static Inno.Native.Text.TextNativeContext Inno.Native.Text.TextNativeContext.Load(string libraryPath = "inno-text")`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L136) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd314985c63d0dcb"></a>[`public sbyte* Inno.Native.Text.TextNativeContext.RawApi.BgcsInnoTextErrorMessage(Inno.Native.Text.BgcsInnoTextError* error)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L237) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b125fc44dd90806f"></a>[`public void Inno.Native.Text.TextNativeContext.RawApi.BgcsInnoTextErrorRelease(Inno.Native.Text.BgcsInnoTextError* error)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L229) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42bf5edeb6bde86b"></a>[`public int Inno.Native.Text.TextNativeContext.RawApi.CreateRuntime(Inno.Native.Text.InnoTextFontAdapterRuntime** bgcs_result, Inno.Native.Text.BgcsInnoTextError** bgcs_error)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L205) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e8a307edc0b5416"></a>[`public int Inno.Native.Text.TextNativeContext.RawApi.DestroyRuntime(Inno.Native.Text.InnoTextFontAdapterRuntime* bgcs_self, Inno.Native.Text.BgcsInnoTextError** bgcs_error)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L221) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-451c132bd6915522"></a>[`public int Inno.Native.Text.TextNativeContext.RawApi.GetMetrics(Inno.Native.Text.InnoTextFontAdapterRuntime* bgcs_self, ulong font_handle, float font_size, Inno.Native.Text.InnoTextFontAdapterMetricsNative* metrics, Inno.Native.Text.Result* bgcs_result, Inno.Native.Text.BgcsInnoTextError** bgcs_error)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L165) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-88fe4466dbb4624f"></a>[`public int Inno.Native.Text.TextNativeContext.RawApi.LoadFont(Inno.Native.Text.InnoTextFontAdapterRuntime* bgcs_self, byte* data, uint length, int face_index, ulong* font_handle, Inno.Native.Text.Result* bgcs_result, Inno.Native.Text.BgcsInnoTextError** bgcs_error)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L173) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0481f502896ea4b6"></a>[`public int Inno.Native.Text.TextNativeContext.RawApi.RasterizeGlyph(Inno.Native.Text.InnoTextFontAdapterRuntime* bgcs_self, ulong font_handle, uint glyph_id, float font_size, byte* pixels, uint pixel_capacity, Inno.Native.Text.InnoTextFontAdapterBitmapNative* bitmap, Inno.Native.Text.Result* bgcs_result, Inno.Native.Text.BgcsInnoTextError** bgcs_error)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L181) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a364b2a35742c307"></a>[`public int Inno.Native.Text.TextNativeContext.RawApi.ReleaseFont(Inno.Native.Text.InnoTextFontAdapterRuntime* bgcs_self, ulong font_handle, Inno.Native.Text.Result* bgcs_result, Inno.Native.Text.BgcsInnoTextError** bgcs_error)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L189) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e42f27d633962207"></a>[`public int Inno.Native.Text.TextNativeContext.RawApi.ResultMessage(Inno.Native.Text.Result result, sbyte** bgcs_result, Inno.Native.Text.BgcsInnoTextError** bgcs_error)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L197) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b71efc41f900434e"></a>[`public int Inno.Native.Text.TextNativeContext.RawApi.ShapeUtf8(Inno.Native.Text.InnoTextFontAdapterRuntime* bgcs_self, ulong font_handle, sbyte* text, uint text_length, float font_size, Inno.Native.Text.Direction direction, sbyte* language, sbyte* script, Inno.Native.Text.InnoTextFontAdapterGlyphNative* glyphs, uint glyph_capacity, uint* glyph_count, Inno.Native.Text.Result* bgcs_result, Inno.Native.Text.BgcsInnoTextError** bgcs_error)`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L213) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f1fd18df33658fd8"></a>[`public static bool Inno.Native.Text.TextNative.IsInitialized { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L21) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e11513c3828b0b7a"></a>[`public static Inno.Native.Text.TextNativeContext.RawApi Inno.Native.Text.TextNative.Raw { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L22) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-133ced020633ee00"></a>[`public Inno.Native.Text.TextNativeContext.RawApi Inno.Native.Text.TextNativeContext.Raw { get; }`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L60) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d71b81e55c0e0be8"></a>[`public struct Inno.Native.Text.BgcsInnoTextError`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L252) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-747cf823a5f95180"></a>[`public enum Inno.Native.Text.Direction : uint`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L262) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a46827452fdc3ed"></a>[`public struct Inno.Native.Text.InnoTextFontAdapterBitmap`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L279) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8aa21769b3ef7de"></a>[`public struct Inno.Native.Text.InnoTextFontAdapterBitmapNative`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L302) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cbd1cd9e1471d3ee"></a>[`public struct Inno.Native.Text.InnoTextFontAdapterGlyph`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L314) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c8a79437dfe14b42"></a>[`public struct Inno.Native.Text.InnoTextFontAdapterGlyphNative`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L337) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4e3016e5e5b22d7d"></a>[`public struct Inno.Native.Text.InnoTextFontAdapterMetrics`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L349) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4b0cbd0cfc177b7f"></a>[`public struct Inno.Native.Text.InnoTextFontAdapterMetricsNative`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L370) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d3ea18f60973ea2"></a>[`public struct Inno.Native.Text.InnoTextFontAdapterRuntime`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L381) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-32a7e292670c1cb7"></a>[`public enum Inno.Native.Text.Result : uint`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L391) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ac6134c38063fce"></a>[`public static class Inno.Native.Text.TextNative`](../../../backends/Text/native/Inno.Native.Text/TextNative.cs#L8) | Initializes the explicitly deployed module through the generated BGCS API. |
| <a id="api-4ffae5ad3bfc2be7"></a>[`public sealed class Inno.Native.Text.TextNativeContext : System.IDisposable, System.IAsyncDisposable`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L46) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c0906d9eccae2b84"></a>[`public sealed class Inno.Native.Text.TextNativeContext.RawApi`](../../../backends/Text/native/Inno.Native.Text/Generated/Bindings.cs#L159) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。
