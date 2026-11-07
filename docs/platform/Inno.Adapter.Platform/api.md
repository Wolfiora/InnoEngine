# Inno.Adapter.Platform API

[项目入口](../Inno.Adapter.Platform.md)

## 公开 API

- `PlatformBackendId`：Composition 启动时使用的后端选择；默认注册 ID 为 `sdl3`。
- `IPlatformBackendFactory.CreateApplication`：把中立选择解析为 caller-owned `IPlatformApplication`。

Host 只保存稳定 ID、factory 和 `IPlatformApplication` / `IPlatformWindow`。未知或未安装的 backend 必须明确失败；不得返回伪实现。原生 surface 只在相互配合的具体 Adapter 之间传递，不进入服务、Shell、Player 或脚本契约。

```csharp
using System;
using Inno.Adapter.Platform;
using Inno.Platform;

static void RunWithWindow(
    IPlatformBackendFactory factory,
    PlatformBackendId selection,
    PlatformWindowOptions options,
    Action<IPlatformApplication, IPlatformWindow> run
) {
    using IPlatformApplication application = factory.CreateApplication(selection);
    using IPlatformWindow window = application.CreateWindow(options);
    run(application, window);
}
```

相邻职责：窗口/事件语义见 [Inno.Platform](../Inno.Platform.md)，SDL3 映射见 [Inno.Adapter.Platform.Sdl3](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)。
