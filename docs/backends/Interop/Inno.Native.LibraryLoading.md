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

无 protected 扩展点。缺失库抛出 DllNotFoundException；缺失普通文件抛出 FileNotFoundException。
空值、路径或通配符输入明确拒绝；同目标的重复文件抛出 InvalidDataException，不选择第一个候选。
target 使用 process architecture，支持当前 Windows、macOS、Linux 动态库部署。
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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

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
| <a id="api-d4f6de88f4f13a13"></a>[`public static string Inno.Native.LibraryLoading.NativeDllLoader.FindNativeFile(string fileName)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllLoader.cs#L88) | Finds a file under the output native folder.<br>返回：Full path to the file.<br>`fileName`：Exact file name to locate. | — | `System.ArgumentException`：The file name is empty, contains a path or contains a wildcard.<br>`System.IO.FileNotFoundException`：No file with this exact name is deployed for the current process target.<br>`System.IO.InvalidDataException`：The deployment contains multiple matching files for the current target. |
| <a id="api-f972652e32085530"></a>[`public static nint Inno.Native.LibraryLoading.NativeDllLoader.LoadNativeDll(string libraryName, System.Reflection.Assembly bindingAssembly)`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllLoader.cs#L46) | Loads a native library from the output native folder and registers its binding owner's import resolver.<br>返回：A caller-owned handle. Transfer it to one native context or release it with NativeLibrary.Free.<br>`libraryName`：Library name without platform extension.<br>`bindingAssembly`：The explicit assembly containing imports for this library; stack inspection is never required. | — | `System.ArgumentNullException`：The binding assembly is null.<br>`System.ArgumentException`：The library name is empty, contains a path or contains a wildcard.<br>`System.DllNotFoundException`：No library matching the current process target is deployed.<br>`System.IO.InvalidDataException`：Multiple deployed files match the requested library and process target. |
| <a id="api-a59b5df50444484c"></a>[`public static class Inno.Native.LibraryLoading.NativeDllLoader`](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllLoader.cs#L16) | Loads explicitly deployed native binaries without changing the application or consulting repository caches. | — | — |

## 项目依赖

