# Inno.Native.LibraryLoading

[分类索引](README.md) · [Wiki 首页](../../README.md) · [显式构建部署](../../build/Inno.Build.Toolchains.md)

## 职责与边界

只负责当前进程的动态库加载和 import resolver。源码发现、编译、产物选择和文件部署属于 Build 层。
输出根使用加载机制程序集所在目录，单文件发布使用应用目录；不会修改应用目录或访问源码缓存。

## 全部公开 API

| API | 当前语义 |
| --- | --- |
| `NativeDllConstants.NATIVE_DIR_NAME` | 应用中明确部署的 `native` 目录名。 |
| `NativeDllLoader.LoadNativeDll(libraryName, bindingAssembly)` | 为明确的 import owner 注册解析器，加载当前 process target 的唯一匹配文件；返回由调用方释放或转交 NativeContext 的 handle。 |
| `NativeDllLoader.FindNativeFile(fileName)` | 按不含路径或通配符的精确文件名查找当前 target 文件。 |

无 protected 扩展点。缺失索引、逻辑映射或已声明的文件抛出 FileNotFoundException；操作系统无法载入实际动态库时由 NativeLibrary 报告加载错误。
空值、路径或通配符输入明确拒绝；索引中的重复映射、路径越界和未知字段明确拒绝，不选择另一个候选。
动态部署由产品组合生成 `native/Deployment.index`，明确目标与逻辑库名、工具名到实际文件的映射。loader 不识别 Windows/macOS/Linux 或 CPU 名称，不扫描源码缓存。新增系统只需提供实际部署映射；静态链接不使用此目录。
resolver 的注册状态使用弱 Assembly key；不会因登记 collectible binding owner 而保留其 ALC。

```csharp
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Inno.Native.LibraryLoading;

static void UseDeployedSymbol(
    string libraryName,
    Assembly bindingOwner,
    string symbolName,
    Action<nint> useWhileLoaded
) {
    nint library = NativeDllLoader.LoadNativeDll(libraryName, bindingOwner);
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

浏览器静态链接的 import 使用生成的目标绑定和对应平台 interop，不能通过这里模拟动态加载。
Native handle 只存在于所属 Native/Adapter 边界，不进入服务层或脚本协议。

## 源码归属

当前唯一源码 owner：`backends/Interop/native/Inno.Native.LibraryLoading/Inno.Native.LibraryLoading.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按功能类型核对全部公开与可扩展 protected 声明。类型的组合与所有权从前文工作流进入。

### `Inno.Native.LibraryLoading.NativeDeploymentIndex`

<!-- api: T:Inno.Native.LibraryLoading.NativeDeploymentIndex 41068dc5123fad39fe49a609d95e674efd19020dcceb4b4b6ac775d6d2e45182 -->
<!-- api: F:Inno.Native.LibraryLoading.NativeDeploymentIndex.C_FILE_NAME 6fc5bb085b57a03623ae964ce3bd5df518254e5950f75b455e6cc35033e850ea -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDeploymentIndex.#ctor(System.String,System.Collections.Generic.IReadOnlyDictionary{System.String,System.String},System.Collections.Generic.IReadOnlyDictionary{System.String,System.String}) 303aa37b963b9be2e7d8e0e1d0d48860a5ab98df9fad52d7e0e0854fe1aa0edb -->
<!-- api: P:Inno.Native.LibraryLoading.NativeDeploymentIndex.targetId 5eb7cdea3f32025452b5f0cf8f407ab100828720aa45129ed2913004dcaebb05 -->
<!-- api: P:Inno.Native.LibraryLoading.NativeDeploymentIndex.libraries 96c6afe5ce80136f32f446cb1b2c580d36f7cea470ef34baa1b5f92dce39b848 -->
<!-- api: P:Inno.Native.LibraryLoading.NativeDeploymentIndex.files f20a167e04904409687a1f3f5cfc4a29a2e1f86ddf9a451d1962153ef5e32c2e -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDeploymentIndex.Encode 84be1e08de5b3c8f4f77e7a7bb734bd014fe0f51ba5651c69c2cc71877206590 -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDeploymentIndex.Decode(System.ReadOnlySpan{System.Byte}) 24ca265f3f31135a8834f373f5f2319b73efe433bd3fdda65ef0d67850e9829c -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDeploymentIndex.ResolveLibrary(System.String,System.String) 96b57dbc7a46bf66b8e8ddb49c4df926626a2ffb8cc17d3a04f3f5854c9a5dca -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDeploymentIndex.ResolveFile(System.String,System.String) e3fc541d55f2f4606b7c2392b77eb0ac4de20b4572bd94e8b82da1260ac2f27c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-db27cea2e426c97e"></a>[`public const string Inno.Native.LibraryLoading.NativeDeploymentIndex.C_FILE_NAME = "Deployment.index"`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L19) | Gets the index filename inside the explicitly deployed native directory. | — | — |
| <a id="api-9340e5954401aba0"></a>[`public Inno.Native.LibraryLoading.NativeDeploymentIndex.NativeDeploymentIndex(string targetId, System.Collections.Generic.IReadOnlyDictionary<string, string> libraries, System.Collections.Generic.IReadOnlyDictionary<string, string> files)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L39) | Freezes one product target and its explicit aliases without detecting the host OS.<br>`targetId`：The exact deployment target selected by the product.<br>`libraries`：Logical import names mapped to contained paths relative to the native directory.<br>`files`：Exact tool or resource aliases mapped to contained deployment paths. | — | T:System.ArgumentException: A target, alias or contained path is invalid.<br>T:System.ArgumentNullException: A mapping is absent. |
| <a id="api-76456c674b4d7eb3"></a>[`public static Inno.Native.LibraryLoading.NativeDeploymentIndex Inno.Native.LibraryLoading.NativeDeploymentIndex.Decode(System.ReadOnlySpan<byte> bytes)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L97) | Reads only the current index format and validates its complete path mappings.<br>返回：An immutable layout; malformed or incomplete input is rejected.<br>`bytes`：Complete encoded bytes borrowed for this call. | — | T:System.IO.InvalidDataException: The index is malformed, incomplete or contains invalid mappings. |
| <a id="api-8d8ba2d448d9d802"></a>[`public byte[] Inno.Native.LibraryLoading.NativeDeploymentIndex.Encode()`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L71) | Encodes the deployment layout without recording SDK or build cache locations.<br>返回：Deterministic UTF-8 bytes for the current strict index format. | — | — |
| <a id="api-a9cba5bfae7cc8e7"></a>[`public string Inno.Native.LibraryLoading.NativeDeploymentIndex.ResolveFile(string nativeDirectory, string name)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L150) | Resolves a tool or resource alias from the explicit deployment mappings.<br>返回：The existing contained file path.<br>`nativeDirectory`：The absolute directory containing the index and payload.<br>`name`：The exact registered alias. | — | T:System.IO.FileNotFoundException: The alias is absent or its payload is missing. |
| <a id="api-4cc55c8a58939258"></a>[`public string Inno.Native.LibraryLoading.NativeDeploymentIndex.ResolveLibrary(string nativeDirectory, string name)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L130) | Resolves a selected library without scanning directories or guessing suffixes.<br>返回：The existing contained library path.<br>`nativeDirectory`：The absolute directory whose owner deployed the index and payload.<br>`name`：The exact logical import name. | — | T:System.IO.FileNotFoundException: The name is absent or the deployed payload is missing. |
| <a id="api-3e1957da382762a2"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Native.LibraryLoading.NativeDeploymentIndex.files { get; }`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L63) | Gets immutable exact tool and resource mappings selected by composition. | — | — |
| <a id="api-1ab183a2227ef957"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Native.LibraryLoading.NativeDeploymentIndex.libraries { get; }`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L58) | Gets immutable logical library mappings selected by composition. | — | — |
| <a id="api-ea6b568079241692"></a>[`public string Inno.Native.LibraryLoading.NativeDeploymentIndex.targetId { get; }`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L53) | Gets the explicit product target, without reconstructing it from OS or CPU names. | — | — |
| <a id="api-2b35cbcbccbb04f3"></a>[`public sealed class Inno.Native.LibraryLoading.NativeDeploymentIndex`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDeploymentIndex.cs#L14) | Describes exact native library and tool locations selected by a product's deployment owner. | — | — |

### `Inno.Native.LibraryLoading.NativeDllConstants`

<!-- api: T:Inno.Native.LibraryLoading.NativeDllConstants 20426c0568bf0fd21fbe715f748b24e2b0db4c480902bd7867208d31680b16f4 -->
<!-- api: F:Inno.Native.LibraryLoading.NativeDllConstants.NATIVE_DIR_NAME 491a10f8cc2d6c75008daa779aaf1144e2129aada856dd6b4a2f7ad9ee427414 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3a95f4e3dc365390"></a>[`public const string Inno.Native.LibraryLoading.NativeDllConstants.NATIVE_DIR_NAME = "native"`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllConstants.cs#L11) | Output folder name for native binaries. | — | — |
| <a id="api-8f3034abd4fd8251"></a>[`public static class Inno.Native.LibraryLoading.NativeDllConstants`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllConstants.cs#L6) | Shared constants for native dll layout. | — | — |

### `Inno.Native.LibraryLoading.NativeDllLoader`

<!-- api: T:Inno.Native.LibraryLoading.NativeDllLoader 6c93dd3d26373308fa8f20fb7d904c23255d5b168e23a209224e63b3d1e22d33 -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDllLoader.LoadNativeDll(System.String,System.Reflection.Assembly) 67f290dbe38ba9044bc0d8f940a5f7fbe8d0fbfbd2368b6af05e8807db43f50d -->
<!-- api: M:Inno.Native.LibraryLoading.NativeDllLoader.FindNativeFile(System.String) b31632c087365cc2564ee9864b016acd6b8d3a1617910d5502c031dbc5e2a154 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d4f6de88f4f13a13"></a>[`public static string Inno.Native.LibraryLoading.NativeDllLoader.FindNativeFile(string fileName)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllLoader.cs#L78) | Resolves an exact tool or resource alias from the product's explicit native deployment index.<br>返回：Full path to the file.<br>`fileName`：Exact file name to locate. | — | T:System.ArgumentException: The file name is empty, contains a path or contains a wildcard.<br>T:System.IO.FileNotFoundException: The index, alias or selected payload is absent.<br>T:System.IO.InvalidDataException: The product's explicit deployment index is malformed. |
| <a id="api-f972652e32085530"></a>[`public static nint Inno.Native.LibraryLoading.NativeDllLoader.LoadNativeDll(string libraryName, System.Reflection.Assembly bindingAssembly)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllLoader.cs#L47) | Loads a native library from the output native folder and registers its binding owner's import resolver.<br>返回：A caller-owned handle. Transfer it to one native context or release it with NativeLibrary.Free.<br>`libraryName`：Library name without platform extension.<br>`bindingAssembly`：The explicit assembly containing imports for this library; stack inspection is never required. | — | T:System.ArgumentNullException: The binding assembly is null.<br>T:System.ArgumentException: The library name is empty, contains a path or contains a wildcard.<br>T:System.DllNotFoundException: The selected native library cannot be loaded by the current process.<br>T:System.IO.InvalidDataException: The product's explicit native deployment index is malformed.<br>T:System.IO.FileNotFoundException: The product index, exact logical library entry or its payload is absent. |
| <a id="api-a59b5df50444484c"></a>[`public static class Inno.Native.LibraryLoading.NativeDllLoader`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllLoader.cs#L14) | Loads explicitly deployed native binaries without changing the application or consulting repository caches. | — | — |

## 项目依赖

