# Inno.Integration.Windows.Sdl3

[平台索引](README.md) · [Wiki 首页](../../README.md) · [SDL backend](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)

## 职责与边界

实现 ISdl3HostIntegration，提供 Windows 的 SDL 初始化、窗口创建、native surface 与初始焦点。只依赖共享 SDL Adapter 和私有 SDL Native，不依赖构建期 BGFX integration 或其他平台；命名平台策略不再存在于共享 SDL backend。

## 初始化与生命周期

平台产品向 Sdl3PlatformBackendProvider 显式传入实例。配置由 application 借用；成功创建的窗口归 application。包装失败由 application 销毁新窗口。外部窗口通过 AdoptWindow 登记，ReleaseWindow 使 wrapper 失效后由原 owner 销毁。GetWindows 只返回当前 application 的目录，不扫描全进程窗口。所有操作在原 owner thread 上执行。

```csharp
using Inno.Integration.Windows.Sdl3;
using Inno.Adapter.Platform.Sdl3;
using Inno.Platform;

using var app = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
using var window = app.CreateWindow(new PlatformWindowOptions { title = "Application", width = 960, height = 540 });
```

Windows 解析 Win32 surface；macOS 解析 Cocoa surface 并设置当前按键初始化 hint；Browser 创建 Canvas/OpenGL 窗口，初始焦点由浏览器事件更新，不安装桌面 live-resize 流程。能力不同通过 Sdl3HostCapabilities 声明；不支持多窗口时在 SDK 创建前失败。无 protected 扩展点。

## 失败与测试

创建或 surface 不可用明确异常；借用窗口不重复销毁；已释放 wrapper 拒绝 nativeHandles。HostIntegrationTests、WindowOwnershipTests、WindowSurfaceTests 使用公开 host/window 契约验证。ImGui ViewportMetricsTests 验证同一 surface 与解登记顺序；实际平台状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration`

<!-- api: T:Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration bae0f377e45fdb7f16b3b56ecc3ed485c265e1d40b4fa910c3cc0a3e4a27e9d5 -->
<!-- api: P:Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.capabilities 7878a205efc808afd82a05b2c79aaae0c722ab85367de8516f048b647b1a97d9 -->
<!-- api: M:Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.ConfigureInitialization acd741ffc02a1994b06e9cd850fe4e794b92a7b389467a56a96be5e89bf9ae2d -->
<!-- api: M:Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions) 96c92f1575e6302268350ae79859318ffe2c001e45f1f74e9e0a98ef0580af52 -->
<!-- api: M:Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.ResolveNativeSurface(System.IntPtr) b5d5f11f8c58382181be7e99e43b03672383d15af610d35ccb8b445949b95038 -->
<!-- api: M:Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.GetInitialFocus(System.IntPtr) 95bd54f0bd66b124c0da98a0a66966d52b25fbd826e8edbdd805bee3f7b0cad1 -->
<!-- api: M:Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.#ctor 266665463e3e4e236db04bdc7c1cd98456d3bdc6e5591072b2b2323cd8ba898c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eb93e4624ef4d0ab"></a>[`public Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.WindowsSdl3HostIntegration()`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Sdl3/WindowsSdl3HostIntegration.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-069fefc69bac5c75"></a>[`public void Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.ConfigureInitialization()`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Sdl3/WindowsSdl3HostIntegration.cs#L18) | Applies host-specific SDK configuration before the application's SDL initialization. | — | — |
| <a id="api-70a245a79474b699"></a>[`public nint Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Sdl3/WindowsSdl3HostIntegration.cs#L23) | Creates one native window whose ownership transfers to the application on success.<br>返回：A nonzero opaque SDL window handle. Failure must throw without leaking a window.<br>`options`：The explicit window options supplied by the product. | — | — |
| <a id="api-3fb2af348b304bc6"></a>[`public bool Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.GetInitialFocus(nint windowHandle)`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Sdl3/WindowsSdl3HostIntegration.cs#L42) | Reads the initial focus policy before platform events begin updating the wrapper.<br>返回：Whether game and editor input may initially treat the window as focused.<br>`windowHandle`：The live SDL window being registered. | — | — |
| <a id="api-40e307356faab855"></a>[`public Inno.Adapter.Platform.PlatformNativeHandles Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.ResolveNativeSurface(nint windowHandle)`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Sdl3/WindowsSdl3HostIntegration.cs#L29) | Resolves borrowed graphics surface information for a live SDL window.<br>返回：Borrowed surface handles valid until the native window is destroyed; unavailable surfaces fail.<br>`windowHandle`：The opaque handle created or explicitly adopted by the application. | — | — |
| <a id="api-d974707982a31608"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration.capabilities { get; }`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Sdl3/WindowsSdl3HostIntegration.cs#L15) | Gets the host's supported window and scheduling behavior. | — | — |
| <a id="api-e9683a545f301a8c"></a>[`public sealed class Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration : Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Sdl3/WindowsSdl3HostIntegration.cs#L12) | Provides the Windows SDK, native surface and focus policies for the shared SDL backend. | — | — |

## 项目依赖

- [Inno.Adapter.Platform.Sdl3](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)：公开引用边界由实际签名核对。
- [Inno.Native.Sdl3](../../backends/Sdl3/Inno.Native.Sdl3.md)：实现依赖，PrivateAssets="compile"。
