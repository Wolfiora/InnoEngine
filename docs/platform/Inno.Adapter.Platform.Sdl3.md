# Inno.Adapter.Platform.Sdl3

[Platform 索引](README.md) · [Neutral contract](Inno.Platform.md) · [SDL3 Native](../native/Inno.Native.Sdl3.md)

## 公开 API

- `Sdl3PlatformApplication : IPlatformApplication`：SDL init、event pump、window ownership 和 extension composition。
- `Sdl3PlatformWindow : IPlatformWindow`：中立 window contract 的 SDL 实现。
- `ISdl3ApplicationExtension`：只供真实 SDL adapter extension 使用的生命周期接口。

Application/Window 的 SDL handle、event union 和 pointer 都停留在本程序集。上层通过 `PlatformNativeHandles` 的明确 kind/value 传递最小 surface interop，不依赖 SDL enum。

在浏览器宿主中，SDL 窗口标记为 `BrowserCanvas`，不尝试读取 Win32/Cocoa 指针。`windowHandle` 指向 SDL 窗口属性持有的 UTF-8 Canvas 选择器，图形后端只在窗口存活期间使用它。
