# Inno.Integration.MacOS.Sdl3

[平台索引](README.md) · [Wiki 首页](../../README.md) · [SDL backend](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)

## 职责与边界

实现 ISdl3HostIntegration，提供 MacOS 的 SDL 初始化、窗口创建、native surface 与初始焦点。只依赖共享 SDL Adapter 和私有 SDL Native，不依赖构建期 BGFX integration 或其他平台；命名平台策略不再存在于共享 SDL backend。

## 初始化与生命周期

平台产品向 Sdl3PlatformBackendProvider 显式传入实例。配置由 application 借用；成功创建的窗口归 application。包装失败由 application 销毁新窗口。外部窗口通过 AdoptWindow 登记，ReleaseWindow 使 wrapper 失效后由原 owner 销毁。GetWindows 只返回当前 application 的目录，不扫描全进程窗口。所有操作在原 owner thread 上执行。

```csharp
using Inno.Integration.MacOS.Sdl3;
using Inno.Adapter.Platform.Sdl3;
using Inno.Platform;

using var app = new Sdl3PlatformApplication(new MacOSSdl3HostIntegration());
using var window = app.CreateWindow(new PlatformWindowOptions { title = "Application", width = 960, height = 540 });
```

Windows 解析 Win32 surface；macOS 解析 Cocoa surface 并设置当前按键初始化 hint；Browser 创建 Canvas/OpenGL 窗口，初始焦点由浏览器事件更新，不安装桌面 live-resize 流程。能力不同通过 Sdl3HostCapabilities 声明；不支持多窗口时在 SDK 创建前失败。无 protected 扩展点。

## 失败与测试

创建或 surface 不可用明确异常；借用窗口不重复销毁；已释放 wrapper 拒绝 nativeHandles。HostIntegrationTests、WindowOwnershipTests、WindowSurfaceTests 使用公开 host/window 契约验证。ImGui ViewportMetricsTests 验证同一 surface 与解登记顺序；实际平台状态见[验收](../../architecture/BACKEND_PLATFORM_INTEGRATION_ACCEPTANCE.md)。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Platform.PlatformNativeHandles Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.ResolveNativeSurface(nint windowHandle)`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L38) | See the implemented contract. |
| [`Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.capabilities`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L15) | See the implemented contract. |
| [`Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L12) | Provides the MacOS SDK, native surface and focus policies for the shared SDL backend. |
| [`bool Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.GetInitialFocus(nint windowHandle)`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L51) | See the implemented contract. |
| [`nint Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L24) | See the implemented contract. |
| [`void Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.ConfigureInitialization()`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L18) | See the implemented contract. |

## 项目依赖

- [Inno.Adapter.Platform.Sdl3](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)：公开引用边界由实际签名核对。
- [Inno.Native.Sdl3](../../backends/Sdl3/Inno.Native.Sdl3.md)：实现依赖，PrivateAssets="compile"。
