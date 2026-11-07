# Inno.Native.Text

[分类索引](README.md) · [Wiki 首页](../../README.md) · [Text Toolchain](Inno.Build.Toolchains.Text.md)

手写 `Native/include/TextRuntime.hpp` 是窄 C++ 语义 facade，`Native/src/TextRuntime.cpp` 通过 PImpl 隐藏 FreeType 与 HarfBuzz。`Bindings/bindings.props` 选择正式 BGCS 配置；同一次 SDK 生成将 C++ facade 降为 C ABI，再产生 `Native/Generated` 和 `Generated/Bindings.cs`。本项目没有第二套手写 C export。ABI 只供所属 Text adapter、toolchain 和 native tests 使用，脚本及其他业务不引用。升级 facade、映射或第三方输入后，通过统一构建入口重新生成并编译同一目标闭包。

## ABI、初始化与失败

构建 profile 选择动态或静态符号解析。BGCS 生成的 `TextNative` 暴露 context 创建/销毁、face 加载/释放、metrics、UTF-8 shaping、glyph rasterization 及结果消息；`InnoText*` 数据结构只为 ABI 使用，不是引擎脚本协议。动态 profile 的类型初始化通过 `NativeDllLoader` 读取应用已部署的 `native/text/<platform>`，缺库即启动失败，不查询仓库构建缓存或提供伪排版结果。

`InnoTextFontAdapterRuntime*` 与 face ID 由 [Text adapter](Inno.Adapter.Text.FreeTypeHarfBuzz.md) 独占。Runtime 复制字体输入并拥有全部 face，在销毁底层字体库之前释放 face；调用方不能跨 session 使用句柄。Shape 和 Rasterize 接收 caller-owned 缓冲区及容量，容量不足返回明确结果；UTF-8 结果说明为 borrowed process-lifetime 文本。绑定映射与 ownership 位于本项目 `Bindings/bindgen.json`。

使用[绑定验收工具](../../development/build/Inno.Build.Cli.md)核对生成文件、实际库和完整部署输入。测试入口与当前环境结果分开记录；实际已执行范围见[当前验收](../../../.agents/records/acceptance/current.md)，单一宿主的通过不能推断其他平台已经通过。

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

组件仅维护公共 `Bindings/bindgen.json`。Browser Build integration 提供明确 wasm32 ABI、托管 runtime 和静态库名称；Windows、macOS integration 分别提供自己的 Profile，组件没有平台目标表。静态产品使用 `INNO_STATIC_NATIVE`，不初始化动态 loader。HostSource 输出属于组件的 `Generated/Bindings.cs` 和 `Native/Generated`；TargetArtifacts 的 managed/native 输出共同属于 `artifacts/bindings/text/<target>/<generationFingerprint>`。是否可运行以[当前验收](../../../.agents/records/acceptance/current.md)为准。

## 源码归属

唯一源码 owner：`backends/Text/native/Inno.Native.Text/Inno.Native.Text.csproj`。共同领域与平台产品通过明确契约组合。

## API 参考

[完整声明、行为与调用约束](Inno.Native.Text/api.md)

<!-- api-include: Inno.Native.Text/api.md -->

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。
