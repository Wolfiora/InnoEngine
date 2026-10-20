# Inno.Native.LibraryLoading

[分类索引](README.md) · [Wiki 首页](../../README.md) · [显式构建部署](../../build/Inno.Build.Toolchains.md)

## 职责与边界

只负责从当前产品的显式部署索引解析动态库和工具文件路径。模块加载、函数表和释放由 BGCS Runtime 或具体消费者拥有；源码发现、编译、产物选择和文件部署属于 Build 层。
输出根使用加载机制程序集所在目录，单文件发布使用应用目录；不会修改应用目录或访问源码缓存。

## 全部公开 API

| API | 当前语义 |
| --- | --- |
| `NativeDllConstants.NATIVE_DIR_NAME` | 应用中明确部署的 `native` 目录名。 |
| `NativeDllLoader.FindNativeLibrary(libraryName)` | 按逻辑模块 ID 解析并验证部署路径，返回绝对路径；由消费者取得并释放实际模块所有权。 |
| `NativeDllLoader.FindNativeFile(fileName)` | 按不含路径或通配符的精确文件名查找当前 target 文件。 |

无 protected 扩展点。缺失索引、逻辑映射或已声明的文件抛出 FileNotFoundException；操作系统无法载入实际动态库时由 NativeLibrary 报告加载错误。
空值、路径或通配符输入明确拒绝；索引中的重复映射、路径越界和未知字段明确拒绝，不选择另一个候选。
动态部署由产品组合生成 `native/Deployment.index`，明确目标与逻辑库名、工具名到实际文件的映射。loader 不识别 Windows/macOS/Linux 或 CPU 名称，不扫描源码缓存。新增系统只需提供实际部署映射；静态链接不使用此目录。
路径解析不登记程序集或保存模块句柄；消费者必须在自己的 owner 生命周期内保留实际模块。

```csharp
using System;
using System.Runtime.InteropServices;
using Inno.Native.LibraryLoading;

static void UseDeployedSymbol(
    string libraryName,
    string symbolName,
    Action<nint> useWhileLoaded
) {
    nint library = NativeLibrary.Load(NativeDllLoader.FindNativeLibrary(libraryName));
    try
    {
        nint function = NativeLibrary.GetExport(library, symbolName);
        useWhileLoaded(function);
    }
    finally
    {
        NativeLibrary.Free(library);
    }
}
```

静态链接使用生成的目标绑定和所属产品的链接输入，不需要动态部署路径。
Native handle 只存在于所属 Native/Adapter 边界，不进入服务层或脚本协议。

## 源码归属

当前唯一源码 owner：`backends/Interop/native/Inno.Native.LibraryLoading/Inno.Native.LibraryLoading.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

以下签名来自当前源码与正式生成结果；原生所有权、线程及关闭顺序见本页前文。

<!-- api: F:Inno.Native.LibraryLoading.NativeDeploymentIndex.C_FILE_NAME 6fc5bb085b57a03623ae964ce3bd5df518254e5950f75b455e6cc35033e850ea -->
<!-- api: F:Inno.Native.LibraryLoading.NativeDllConstants.NATIVE_DIR_NAME 491a10f8cc2d6c75008daa779aaf1144e2129aada856dd6b4a2f7ad9ee427414 -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDeploymentIndex.#ctor(System.String,System.Collections.Generic.IReadOnlyDictionary{System.String,System.String},System.Collections.Generic.IReadOnlyDictionary{System.String,System.String}) 303aa37b963b9be2e7d8e0e1d0d48860a5ab98df9fad52d7e0e0854fe1aa0edb -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDeploymentIndex.Decode(System.ReadOnlySpan{System.Byte}) 24ca265f3f31135a8834f373f5f2319b73efe433bd3fdda65ef0d67850e9829c -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDeploymentIndex.Encode 84be1e08de5b3c8f4f77e7a7bb734bd014fe0f51ba5651c69c2cc71877206590 -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDeploymentIndex.ResolveFile(System.String,System.String) e3fc541d55f2f4606b7c2392b77eb0ac4de20b4572bd94e8b82da1260ac2f27c -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDeploymentIndex.ResolveLibrary(System.String,System.String) 96b57dbc7a46bf66b8e8ddb49c4df926626a2ffb8cc17d3a04f3f5854c9a5dca -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDllLoader.FindNativeFile(System.String) b31632c087365cc2564ee9864b016acd6b8d3a1617910d5502c031dbc5e2a154 -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDllLoader.FindNativeLibrary(System.String) c460e3340890b800c593684e7c94ec058e3f58544ebd6c98e5c452187d0dc7a1 -->
<!-- api: P:Inno.Native.LibraryLoading.NativeDeploymentIndex.files f20a167e04904409687a1f3f5cfc4a29a2e1f86ddf9a451d1962153ef5e32c2e -->
<!-- api: P:Inno.Native.LibraryLoading.NativeDeploymentIndex.libraries 96c6afe5ce80136f32f446cb1b2c580d36f7cea470ef34baa1b5f92dce39b848 -->
<!-- api: P:Inno.Native.LibraryLoading.NativeDeploymentIndex.targetId 5eb7cdea3f32025452b5f0cf8f407ab100828720aa45129ed2913004dcaebb05 -->
<!-- api: T:Inno.Native.LibraryLoading.NativeDeploymentIndex 41068dc5123fad39fe49a609d95e674efd19020dcceb4b4b6ac775d6d2e45182 -->
<!-- api: T:Inno.Native.LibraryLoading.NativeDllConstants 20426c0568bf0fd21fbe715f748b24e2b0db4c480902bd7867208d31680b16f4 -->
<!-- api: T:Inno.Native.LibraryLoading.NativeDllLoader 6c93dd3d26373308fa8f20fb7d904c23255d5b168e23a209224e63b3d1e22d33 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-db27cea2e426c97e"></a>[`public const string Inno.Native.LibraryLoading.NativeDeploymentIndex.C_FILE_NAME = "Deployment.index"`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L19) | Gets the index filename inside the explicitly deployed native directory.<br>常量：`Deployment.index` |
| <a id="api-3a95f4e3dc365390"></a>[`public const string Inno.Native.LibraryLoading.NativeDllConstants.NATIVE_DIR_NAME = "native"`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllConstants.cs#L11) | Output folder name for native binaries.<br>常量：`native` |
| <a id="api-9340e5954401aba0"></a>[`public Inno.Native.LibraryLoading.NativeDeploymentIndex.NativeDeploymentIndex(string targetId, System.Collections.Generic.IReadOnlyDictionary<string, string> libraries, System.Collections.Generic.IReadOnlyDictionary<string, string> files)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L39) | Freezes one product target and its explicit aliases without detecting the host OS.<br>`targetId`：The exact deployment target selected by the product.<br>`libraries`：Logical import names mapped to contained paths relative to the native directory.<br>`files`：Exact tool or resource aliases mapped to contained deployment paths. |
| <a id="api-76456c674b4d7eb3"></a>[`public static Inno.Native.LibraryLoading.NativeDeploymentIndex Inno.Native.LibraryLoading.NativeDeploymentIndex.Decode(System.ReadOnlySpan<byte> bytes)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L97) | Reads only the current index format and validates its complete path mappings.<br>返回：An immutable layout; malformed or incomplete input is rejected.<br>`bytes`：Complete encoded bytes borrowed for this call. |
| <a id="api-8d8ba2d448d9d802"></a>[`public byte[] Inno.Native.LibraryLoading.NativeDeploymentIndex.Encode()`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L71) | Encodes the deployment layout without recording SDK or build cache locations.<br>返回：Deterministic UTF-8 bytes for the current strict index format. |
| <a id="api-a9cba5bfae7cc8e7"></a>[`public string Inno.Native.LibraryLoading.NativeDeploymentIndex.ResolveFile(string nativeDirectory, string name)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L150) | Resolves a tool or resource alias from the explicit deployment mappings.<br>返回：The existing contained file path.<br>`nativeDirectory`：The absolute directory containing the index and payload.<br>`name`：The exact registered alias. |
| <a id="api-4cc55c8a58939258"></a>[`public string Inno.Native.LibraryLoading.NativeDeploymentIndex.ResolveLibrary(string nativeDirectory, string name)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L130) | Resolves a selected library without scanning directories or guessing suffixes.<br>返回：The existing contained library path.<br>`nativeDirectory`：The absolute directory whose owner deployed the index and payload.<br>`name`：The exact logical import name. |
| <a id="api-d4f6de88f4f13a13"></a>[`public static string Inno.Native.LibraryLoading.NativeDllLoader.FindNativeFile(string fileName)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllLoader.cs#L57) | Resolves an exact tool or resource alias from the product's explicit native deployment index.<br>返回：Full path to the file.<br>`fileName`：Exact file name to locate. |
| <a id="api-eed6375a8e8b128e"></a>[`public static string Inno.Native.LibraryLoading.NativeDllLoader.FindNativeLibrary(string libraryName)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllLoader.cs#L32) | Resolves one logical module from the product's explicit deployment index without loading it.<br>返回：The validated absolute library path; ownership is acquired by the consumer's module provider.<br>`libraryName`：The exact logical library identity declared by the selected product. |
| <a id="api-3e1957da382762a2"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Native.LibraryLoading.NativeDeploymentIndex.files { get; }`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L63) | Gets immutable exact tool and resource mappings selected by composition. |
| <a id="api-1ab183a2227ef957"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Native.LibraryLoading.NativeDeploymentIndex.libraries { get; }`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L58) | Gets immutable logical library mappings selected by composition. |
| <a id="api-ea6b568079241692"></a>[`public string Inno.Native.LibraryLoading.NativeDeploymentIndex.targetId { get; }`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L53) | Gets the explicit product target, without reconstructing it from OS or CPU names. |
| <a id="api-2b35cbcbccbb04f3"></a>[`public sealed class Inno.Native.LibraryLoading.NativeDeploymentIndex`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L14) | Describes exact native library and tool locations selected by a product's deployment owner. |
| <a id="api-8f3034abd4fd8251"></a>[`public static class Inno.Native.LibraryLoading.NativeDllConstants`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllConstants.cs#L6) | Shared constants for native dll layout. |
| <a id="api-a59b5df50444484c"></a>[`public static class Inno.Native.LibraryLoading.NativeDllLoader`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllLoader.cs#L10) | Resolves explicitly deployed native binaries and files without consulting repository caches. |

## 项目依赖

