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

创建或 surface 不可用明确异常；借用窗口不重复销毁；已释放 wrapper 拒绝 nativeHandles。HostIntegrationTests、WindowOwnershipTests、WindowSurfaceTests 使用公开 host/window 契约验证。ImGui ViewportMetricsTests 验证同一 surface 与解登记顺序；实际平台状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration`

<!-- api: T:Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration 4875109d5f9e580659210decf25a1176196f1029fca3675289267862c3a0e764 -->
<!-- api: P:Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.capabilities cb996c7dd83754f3ed2412569dd7ad9e0c56d569c1ba50bc58fc569250e2c28b -->
<!-- api: M:Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.ConfigureInitialization 5c4386584850757503c7125dd0cab82e69630a2f97026639129d144d35747d0f -->
<!-- api: M:Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions) 6055c8e36f280512f2ec2be05cee07c0188e7a3e6061dac348e943f67baac89b -->
<!-- api: M:Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.ResolveNativeSurface(System.IntPtr) 32263d3997c4722255dbd4f05c39f2f34f4622543e985de4ad8ba39b5652390f -->
<!-- api: M:Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.GetInitialFocus(System.IntPtr) 72834fe1693aea7995ab66f57641f99b43296f282ea750009c55d3a185e7b2f1 -->
<!-- api: M:Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.#ctor 7c3ce15640f638bdc1133aff36d9471183973df5414e30e8f1e0996d4c0278b8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4022cdbc079ad452"></a>[`public Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.MacOSSdl3HostIntegration()`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-04ce5c8dd0f1f068"></a>[`public void Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.ConfigureInitialization()`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L18) | Applies host-specific SDK configuration before the application's SDL initialization. | — | — |
| <a id="api-b3f725262330478a"></a>[`public nint Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L24) | Creates one native window whose ownership transfers to the application on success.<br>返回：A nonzero opaque SDL window handle. Failure must throw without leaking a window.<br>`options`：The explicit window options supplied by the product. | — | — |
| <a id="api-8285581f1eb5f695"></a>[`public bool Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.GetInitialFocus(nint windowHandle)`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L43) | Reads the initial focus policy before platform events begin updating the wrapper.<br>返回：Whether game and editor input may initially treat the window as focused.<br>`windowHandle`：The live SDL window being registered. | — | — |
| <a id="api-ecb0d6fe63eeb4a8"></a>[`public Inno.Adapter.Platform.PlatformNativeHandles Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.ResolveNativeSurface(nint windowHandle)`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L30) | Resolves borrowed graphics surface information for a live SDL window.<br>返回：Borrowed surface handles valid until the native window is destroyed; unavailable surfaces fail.<br>`windowHandle`：The opaque handle created or explicitly adopted by the application. | — | — |
| <a id="api-c59ee911d77ab014"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration.capabilities { get; }`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L15) | Gets the host's supported window and scheduling behavior. | — | — |
| <a id="api-9dea892aca66eba2"></a>[`public sealed class Inno.Integration.MacOS.Sdl3.MacOSSdl3HostIntegration : Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Sdl3/MacOSSdl3HostIntegration.cs#L12) | Provides the MacOS SDK, native surface and focus policies for the shared SDL backend. | — | — |

## 项目依赖

- [Inno.Adapter.Platform.Sdl3](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)：公开引用边界由实际签名核对。
- [Inno.Native.Sdl3](../../backends/Sdl3/Inno.Native.Sdl3.md)：实现依赖，PrivateAssets="compile"。
