# Inno.Adapter.Platform.Sdl3

SDL 的进入后台/恢复前台通知转换为 Core Events 的 `ApplicationSuspensionChangedEvent`。
隐藏、显示、最小化和恢复通知转换为 `WindowVisibilityChangedEvent`；每个窗口分别保存隐藏与最小化原因，
不能用后一条通知错误清除另一原因。浏览器 document visibility 同样由 SDL 原始入口进入，领域服务无需认识浏览器。

[Platform 索引](README.md) · [Neutral contract](Inno.Platform.md) · [SDL3 Native](../native/Inno.Native.Sdl3.md)

## 公开 API

- `Sdl3PlatformApplication : IPlatformApplication`：SDL init、event pump、window ownership 和 extension composition。
- `Sdl3PlatformWindow : IPlatformWindow`：中立 window contract 的 SDL 实现。
- `ISdl3ApplicationExtension`：只供真实 SDL adapter extension 使用的生命周期接口。

Application/Window 的 SDL handle、event union 和 pointer 都停留在本程序集。`Sdl3PlatformWindow` 实现 Adapter SPI `INativeWindowSurface`，具体图形 Adapter 通过 `PlatformNativeHandles` 的开放 ABI ID 和借用句柄进行 surface interop；公共平台服务与产品宿主不依赖这些值或 SDL enum。

在浏览器宿主中，SDL 窗口标记为 `BrowserCanvas`，不尝试读取 Win32/Cocoa 指针。`windowHandle` 指向 SDL 窗口属性持有的 UTF-8 Canvas 选择器，图形后端只在窗口存活期间使用它。

## Composition provider

`Sdl3PlatformBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateApplication()` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.OnApplicationDisposing(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application)`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L44) | Releases application-bound integration state before platform resources are destroyed. |
| [`void Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.PrepareLiveResizeWindow(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application, uint windowId)`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L33) | Synchronizes integration window state before the host renders a complete live-resize frame. |
| [`void Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.ProcessNativeEvent(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application, scoped System.ReadOnlySpan<byte> nativeEventData)`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L19) | Processes one backend-native event before it is translated into an engine event. |
| [`Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L8) | Defines the synchronous SDL3 callbacks required by an SDL3-specific integration. |

### `Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Action<uint>? Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.redrawRequested`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L16) | Occurs after the observable redraw requested state changes. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.Sdl3PlatformApplication()`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L21) | Initializes platform subsystems required for windowing and input events. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L87) | Creates and validates a caller-owned window value. |
| [`void Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.Dispose()`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L301) | Releases the resources owned by this instance. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow> Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.GetWindows()`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L180) | Retrieves the requested windows value from current authoritative state. |
| [`bool Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.PollEvent(out Inno.Core.Events.Event? evnt)`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L154) | Attempts to dequeue the next backend-neutral platform event. |
| [`System.IDisposable Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.RegisterExtension(Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension extension)`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L237) | Registers a platform extension for the application lifetime. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L17) | Platform runtime entry point responsible for window creation and platform event polling. |

### `Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider.Sdl3PlatformBackendProvider()`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. |
| [`override Inno.Platform.IPlatformApplication Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider.CreateApplication()`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformBackendProvider.cs#L17) | See the implemented contract. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformBackendProvider.cs#L9) | Supplies the Sdl3 implementation through the neutral platform creation boundary. |

### `Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.Dispose()`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformWindow.Internal.cs#L133) | Releases the resources owned by this instance. |
| [`void Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.RequestClose()`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformWindow.Internal.cs#L125) | Queues a close request for processing at the next platform safety point. |
| [`int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.height`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L30) | Gets the current window height in platform-independent logical units. |
| [`bool Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.isClosed`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L45) | Gets whether this window has been marked as closed. |
| [`bool Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.isFocused`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L50) | Gets whether this window currently owns platform input focus. |
| [`Inno.Adapter.Platform.PlatformNativeHandles Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.nativeHandles`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L58) | Gets native window handles for graphics backends and platform integration. |
| [`int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.pixelHeight`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L40) | Gets the current drawable height in physical pixels. |
| [`int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.pixelWidth`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L35) | Gets the current drawable width in physical pixels. |
| [`nint Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.sdlWindowHandle`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L73) | Gets the opaque SDL3 window identity used only by cooperating SDL3 adapter assemblies. |
| [`string Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.title`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L20) | Gets the window title. |
| [`int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.width`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L25) | Gets the current window width in platform-independent logical units. |
| [`uint Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.windowId`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L15) | Gets the platform window identifier. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow`](../../src/adapters/platform/Inno.Adapter.Platform.Sdl3/Sdl3PlatformWindow.Internal.cs#L12) | Represents a native platform window. |

## 项目依赖

- [Inno.Native.Sdl3](../native/Inno.Native.Sdl3.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Platform](Inno.Adapter.Platform.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Platform](Inno.Platform.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
