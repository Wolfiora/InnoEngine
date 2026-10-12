# Inno.Integration.Browser.Sdl3

[平台索引](README.md) · [Wiki 首页](../../README.md) · [SDL backend](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)

## 职责与边界

实现 ISdl3HostIntegration，提供 Browser 的 SDL 初始化、窗口创建、native surface 与初始焦点。只依赖共享 SDL Adapter 和私有 SDL Native，不依赖构建期 BGFX integration 或其他平台；命名平台策略不再存在于共享 SDL backend。

## 初始化与生命周期

平台产品向 Sdl3PlatformBackendProvider 显式传入实例。配置由 application 借用；成功创建的窗口归 application。包装失败由 application 销毁新窗口。外部窗口通过 AdoptWindow 登记，ReleaseWindow 使 wrapper 失效后由原 owner 销毁。GetWindows 只返回当前 application 的目录，不扫描全进程窗口。所有操作在原 owner thread 上执行。

```csharp
using Inno.Integration.Browser.Sdl3;
using Inno.Adapter.Platform.Sdl3;
using Inno.Platform;

using var app = new Sdl3PlatformApplication(new BrowserSdl3HostIntegration());
using var window = app.CreateWindow(new PlatformWindowOptions { title = "Application", width = 960, height = 540 });
```

Windows 解析 Win32 surface；macOS 解析 Cocoa surface 并设置当前按键初始化 hint；Browser 创建 Canvas/OpenGL 窗口，初始焦点由浏览器事件更新，不安装桌面 live-resize 流程。能力不同通过 Sdl3HostCapabilities 声明；不支持多窗口时在 SDK 创建前失败。无 protected 扩展点。

## 失败与测试

创建或 surface 不可用明确异常；借用窗口不重复销毁；已释放 wrapper 拒绝 nativeHandles。HostIntegrationTests、WindowOwnershipTests、WindowSurfaceTests 使用公开 host/window 契约验证。ImGui ViewportMetricsTests 验证同一 surface 与解登记顺序；实际平台状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration`

<!-- api: T:Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration 4c324e1b6c9a0637c16930cf5c9d259f7c4db67371fb67ad160b3fabac41a67a -->
<!-- api: P:Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.capabilities 68193d9df83a72ef12b77a02816ce7fe7fba84b5e8b6bedc09bd0c35806dc6d9 -->
<!-- api: M:Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.ConfigureInitialization 193fc8dc0487a9492b81c17b9811ebc1c529443ffdbf98bdf75a6913b4dc2fa3 -->
<!-- api: M:Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions) 6bc5fd8f3ee4545f3d64fc03764293eb0472f658c8d7a11afa14c43cdb821fef -->
<!-- api: M:Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.ResolveNativeSurface(System.IntPtr) 3515fbdbe8e2c5aabaf2c49fb8691e6163f3d1a2aca50e7877bf1924496b3c74 -->
<!-- api: M:Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.GetInitialFocus(System.IntPtr) e39154dce877e7c20cdde9f719eaf7ffd784beb4869df53c39dcd26d70fd0fe0 -->
<!-- api: M:Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.#ctor e4552115c1463ca5e173ef88f27ed71c7b7e205ba0ec554007b6898aa8af983f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-79597046946c03d3"></a>[`public Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.BrowserSdl3HostIntegration()`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Sdl3/BrowserSdl3HostIntegration.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ea46f9ab7b61533c"></a>[`public void Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.ConfigureInitialization()`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Sdl3/BrowserSdl3HostIntegration.cs#L19) | Applies host-specific SDK configuration before the application's SDL initialization. | — | — |
| <a id="api-7e4845e738415d5b"></a>[`public nint Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Sdl3/BrowserSdl3HostIntegration.cs#L24) | Creates one native window whose ownership transfers to the application on success.<br>返回：A nonzero opaque SDL window handle. Failure must throw without leaking a window.<br>`options`：The explicit window options supplied by the product. | — | — |
| <a id="api-d344d95efefeb09b"></a>[`public bool Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.GetInitialFocus(nint windowHandle)`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Sdl3/BrowserSdl3HostIntegration.cs#L46) | Reads the initial focus policy before platform events begin updating the wrapper.<br>返回：Whether game and editor input may initially treat the window as focused.<br>`windowHandle`：The live SDL window being registered. | — | — |
| <a id="api-ffbbd69781d7217a"></a>[`public Inno.Adapter.Platform.PlatformNativeHandles Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.ResolveNativeSurface(nint windowHandle)`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Sdl3/BrowserSdl3HostIntegration.cs#L30) | Resolves borrowed graphics surface information for a live SDL window.<br>返回：Borrowed surface handles valid until the native window is destroyed; unavailable surfaces fail.<br>`windowHandle`：The opaque handle created or explicitly adopted by the application. | — | — |
| <a id="api-48c1f582961ff224"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration.capabilities { get; }`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Sdl3/BrowserSdl3HostIntegration.cs#L16) | Gets the host's supported window and scheduling behavior. | — | — |
| <a id="api-4c44f8c957445577"></a>[`public sealed class Inno.Integration.Browser.Sdl3.BrowserSdl3HostIntegration : Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Sdl3/BrowserSdl3HostIntegration.cs#L13) | Provides the Browser SDK, native surface and focus policies for the shared SDL backend. | — | — |

## 项目依赖

- [Inno.Adapter.Platform.Sdl3](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)：公开引用边界由实际签名核对。
- [Inno.Native.Sdl3](../../backends/Sdl3/Inno.Native.Sdl3.md)：实现依赖，PrivateAssets="compile"。
