# Inno.Native.LibraryLoading

[Native 索引](README.md) · [Wiki 首页](../README.md) · [显式构建部署](../build/Inno.Build.Toolchains.Host.md)

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
using System.Runtime.InteropServices;
using Inno.Native.LibraryLoading;

IntPtr library = NativeDllLoader.LoadNativeDll(libraryName, bindingOwner);
try
{
    IntPtr function = NativeLibrary.GetExport(library, symbolName);
}
finally
{
    NativeLibrary.Free(library);
}
```

浏览器静态链接的 import 使用生成的目标绑定和对应平台 interop，不能通过这里模拟动态加载。
Native handle 只存在于所属 Native/Adapter 边界，不进入服务层或脚本协议。
