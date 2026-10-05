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
