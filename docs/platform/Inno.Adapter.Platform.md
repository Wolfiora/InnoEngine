# Inno.Adapter.Platform

[Platform 索引](README.md) · [中立 Platform](Inno.Platform.md) · [SDL3 implementation](Inno.Adapter.Platform.Sdl3.md)

该项目定义平台 Adapter family 的稳定边界，不包含 SDL3 引用。

## 公开 API

- `PlatformBackendId`：Composition 启动时使用的后端选择；默认注册 ID 为 `sdl3`。
- `IPlatformBackendFactory.CreateApplication`：把中立选择解析为 caller-owned `IPlatformApplication`。

Host 只保存稳定 ID、factory 和 `IPlatformApplication` / `IPlatformWindow`。未知或未安装的 backend 必须明确失败；不得返回伪实现。原生 surface 只在相互配合的具体 Adapter 之间传递，不进入服务、Shell、Player 或脚本契约。

```csharp
IPlatformApplication application = catalog.platform.CreateApplication(selection.platform);
using IPlatformWindow window = application.CreateWindow(windowOptions);
```

相邻职责：窗口/事件语义见 [Inno.Platform](Inno.Platform.md)，SDL3 映射见 [Inno.Adapter.Platform.Sdl3](Inno.Adapter.Platform.Sdl3.md)。

## 开放注册与生命周期

后端 ID 是开放的 ordinal 字符串，不能包含空白；默认 struct 未赋值。内置 ID `sdl3` 只提供默认组合，不限制第三方实现。

| API | 当前语义 |
| --- | --- |
| `PlatformBackendId(string)`；`value`、`isValid`、`ToString()` | 创建、检查并显示稳定 ID；无效构造抛出 `ArgumentException`。 |
| `PlatformBackendProvider(PlatformBackendId)`（protected）；`id` | composition 显式配置的不可变注册描述；provider 不执行类型发现。 |
| `PlatformBackendProvider.CreateApplication` | 实现者的创建扩展点；返回 caller-owned `IPlatformApplication`，不允许 null。 |
| `PlatformBackendCatalog(IEnumerable<PlatformBackendProvider>)` | 捕获完整注册快照；重复或 null provider 在构造时失败；不创建设备。 |
| `PlatformBackendCatalog.supportedBackends / CreateApplication` | 只解析当前快照中的 exact ID；未注册抛出 `NotSupportedException`，null 产品抛出 `InvalidOperationException`。 |
| `IPlatformBackendFactory.supportedBackends` | 启动前能力预检使用的只读注册列表。 |

provider 及其 delegate/资源由 composition owner 释放；catalog 不接管 provider。创建出的服务由调用方释放。源码扩展若通过 TypeRegistry 发现，其 ID 仍由发现协议的 Attribute 声明；此处是宿主明确传入的 provider 集合，不额外扫描程序集。

`AdapterSelection.Validate(catalog)` 在初始化任何窗口或设备前检查全部领域。可在 composition 为 provider 传入任意分配的 `PlatformBackendId`，无需新增枚举或修改中央分支。

## 原生呈现 SPI

| API | 当前语义 |
| --- | --- |
| `INativeWindowSurface.nativeHandles` | 窗口可选实现的 Adapter SPI，返回窗口 owner 持有的借用原生 surface。 |
| `PlatformNativeHandles(windowHandle, displayHandle, handleKind)` | 只读描述；默认 display handle 为零、ABI ID 未赋值。`windowHandle` / `displayHandle` 不转移所有权，窗口释放后失效。 |
| `PlatformNativeHandleId(string)`；`value`、`isValid`、`ToString()` | 开放、区分大小写的稳定 surface ABI ID；空白构造失败，默认值未赋值。 |
| `PlatformNativeHandleId.win32 / cocoa / browserCanvas` | 内置 HWND、Cocoa 窗口及 UTF-8 canvas selector ABI。第三方可以声明自己的 ID，不修改本项目。 |

具体图形 Adapter 验证它理解的 ID 与句柄；不支持必须明确失败。窗口释放后再次借用句柄抛出 `ObjectDisposedException`。BGFX 在接管原生进程资源之前拒绝未实现此 SPI 的窗口。新增平台 surface 的映射留在具体窗口和图形 Adapter，不能扩展为服务层原生访问 API。这里的 SPI 不进入 Scripting API；不参与对象 Identity、History 或序列化。
