# Inno.Adapter.Platform.Sdl3

SDL 的进入后台/恢复前台通知转换为 Core Events 的 `ApplicationSuspensionChangedEvent`。
隐藏、显示、最小化和恢复通知转换为 `WindowVisibilityChangedEvent`；每个窗口分别保存隐藏与最小化原因，
不能用后一条通知错误清除另一原因。浏览器 document visibility 同样由 SDL 原始入口进入，领域服务无需认识浏览器。

[分类索引](README.md) · [Neutral contract](../../platform/Inno.Platform.md) · [SDL3 Native](Inno.Native.Sdl3.md) · [Wiki 首页](../../README.md)

## 公开 API

- `Sdl3PlatformApplication : IPlatformApplication`：SDL init、event pump、window ownership 和 extension composition。
- `Sdl3PlatformWindow : IPlatformWindow`：中立 window contract 的 SDL 实现。
- `ISdl3ApplicationExtension`：只供真实 SDL adapter extension 使用的生命周期接口。

Application/Window 的 SDL handle、event union 和 pointer 都停留在本程序集。`Sdl3PlatformWindow` 实现 Adapter SPI `INativeWindowSurface`，具体图形 Adapter 通过 `PlatformNativeHandles` 的开放 ABI ID 和借用句柄进行 surface interop；公共平台服务与产品宿主不依赖这些值或 SDL enum。

在浏览器宿主中，SDL 窗口标记为 `BrowserCanvas`，不尝试读取 Win32/Cocoa 指针。`windowHandle` 指向 SDL 窗口属性持有的 UTF-8 Canvas 选择器，图形后端只在窗口存活期间使用它。

## Composition provider

`Sdl3PlatformBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateApplication()` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。

## 源码归属

当前唯一源码 owner：`backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Inno.Adapter.Platform.Sdl3.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 显式宿主接入

应用构造必须接收 ISdl3HostIntegration；没有当前 OS 默认策略。Sdl3HostCapabilities 声明多窗口和 live-resize。自建窗口归应用，AdoptWindow 是借用登记，ReleaseWindow 先失效后由原 owner 销毁；GetWindows 仅返回本应用窗口。窗口包装失败销毁新建窗口，借用包装失败不销毁外部窗口。

## 共同窗口操作

Sdl3WindowOperations.CreateWindow 使用统一 SDL properties 并总是释放临时 properties；成功 handle 的销毁权交给应用。ReadFocus 只读取。Windows/macOS 不要求 OpenGL，Browser 要求 OpenGL 且初始焦点继续等待真实事件；surface 解析/SDK 配置属于各 integration。应用唯一管理登记、owner thread、包装失败清理和外部窗口借用。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L8) | Defines the synchronous SDL3 callbacks required by an SDL3-specific integration. |
| [`void Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.OnApplicationDisposing(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L44) | Releases application-bound integration state before platform resources are destroyed. |
| [`void Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.PrepareLiveResizeWindow(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application, uint windowId)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L33) | Synchronizes integration window state before the host renders a complete live-resize frame. |
| [`void Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.ProcessNativeEvent(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application, scoped System.ReadOnlySpan<byte> nativeEventData)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L19) | Processes one backend-native event before it is translated into an engine event. |

### `Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Platform.PlatformNativeHandles Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.ResolveNativeSurface(nint windowHandle)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L45) | Resolves borrowed graphics surface information for a live SDL window. |
| [`Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L13) | Connects an explicitly selected system host to SDL without selecting platforms inside the backend. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.capabilities`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L18) | Gets the host's supported window and scheduling behavior. |
| [`bool Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.GetInitialFocus(nint windowHandle)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L56) | Reads the initial focus policy before platform events begin updating the wrapper. |
| [`nint Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L34) | Creates one native window whose ownership transfers to the application on success. |
| [`void Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.ConfigureInitialization()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L23) | Applies host-specific SDK configuration before the application's SDL initialization. |

### `Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3HostCapabilities.cs#L6) | Declares immutable window and frame scheduling capabilities supplied by the selected host. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities.Sdl3HostCapabilities(bool multipleWindows, bool liveResize)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3HostCapabilities.cs#L17) | Captures host capabilities without initializing SDL or creating windows. |
| [`bool Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities.liveResize`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3HostCapabilities.cs#L33) | Gets whether the application installs the native live-resize scheduling integration. |
| [`bool Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities.multipleWindows`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3HostCapabilities.cs#L28) | Gets whether the application can register additional native windows. |

### `Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L17) | Platform runtime entry point responsible for window creation and platform event polling. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.Sdl3PlatformApplication(Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration hostIntegration)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L27) | Initializes platform subsystems required for windowing and input events. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.AdoptWindow(nint windowHandle)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Windows.cs#L63) | Registers an externally owned window using this application's host and surface integration. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Windows.cs#L27) | Creates and registers a window through the borrowed host integration. |
| [`System.Action<uint>? Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.redrawRequested`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L16) | Occurs after the observable redraw requested state changes. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow> Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.GetWindows()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L112) | Retrieves the requested windows value from current authoritative state. |
| [`System.IDisposable Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.RegisterExtension(Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension extension)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L132) | Registers a platform extension for the application lifetime. |
| [`bool Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.PollEvent(out Inno.Core.Events.Event? evnt)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L85) | Attempts to dequeue the next backend-neutral platform event. |
| [`void Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.Dispose()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Internal.cs#L197) | Releases the resources owned by this instance. |
| [`void Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.ReleaseWindow(Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow window)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Windows.cs#L84) | Invalidates and unregisters a borrowed window before its original owner destroys it. |

### `Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformBackendProvider.cs#L9) | Supplies the Sdl3 implementation through the neutral platform creation boundary. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider.Sdl3PlatformBackendProvider(Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration hostIntegration)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformBackendProvider.cs#L22) | Creates an explicitly composed registration for the bundled implementation. |
| [`override Inno.Platform.IPlatformApplication Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider.CreateApplication()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformBackendProvider.cs#L29) | See the implemented contract. |

### `Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Platform.PlatformNativeHandles Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.nativeHandles`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L58) | Gets native window handles for graphics backends and platform integration. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformWindow.Internal.cs#L11) | Represents a native platform window. |
| [`bool Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.isClosed`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L45) | Gets whether this window has been marked as closed. |
| [`bool Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.isFocused`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L50) | Gets whether this window currently owns platform input focus. |
| [`int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.height`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L30) | Gets the current window height in platform-independent logical units. |
| [`int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.pixelHeight`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L40) | Gets the current drawable height in physical pixels. |
| [`int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.pixelWidth`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L35) | Gets the current drawable width in physical pixels. |
| [`int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.width`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L25) | Gets the current window width in platform-independent logical units. |
| [`nint Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.sdlWindowHandle`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L73) | Gets the opaque SDL3 window identity used only by cooperating SDL3 adapter assemblies. |
| [`string Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.title`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L20) | Gets the window title. |
| [`uint Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.windowId`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L15) | Gets the platform window identifier. |
| [`void Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.Dispose()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformWindow.Internal.cs#L132) | Releases the resources owned by this instance. |
| [`void Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.RequestClose()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformWindow.Internal.cs#L124) | Queues a close request for processing at the next platform safety point. |

### `Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3WindowOperations.cs#L10) | Implements shared SDL window operations without selecting a system host or registering windows. |
| [`static bool Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations.ReadFocus(nint windowHandle)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3WindowOperations.cs#L75) | Reads native focus without registering the window or changing its input state. |
| [`static nint Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations.CreateWindow(Inno.Platform.PlatformWindowOptions options, bool requireOpenGl)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3WindowOperations.cs#L30) | Creates an SDL window and transfers its native destruction responsibility to the caller. |

## 项目依赖

- [Inno.Native.Sdl3](Inno.Native.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Input](../../core/Inno.Core.Input.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Platform](../../platform/Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Platform](../../platform/Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
