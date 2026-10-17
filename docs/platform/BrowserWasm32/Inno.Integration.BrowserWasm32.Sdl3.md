# Inno.Integration.BrowserWasm32.Sdl3

[平台索引](README.md) · [Wiki 首页](../../README.md) · [SDL backend](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)

## 职责与边界

实现 ISdl3HostIntegration，提供 Browser 的 SDL 初始化、窗口创建、native surface 与初始焦点。只依赖共享 SDL Adapter 和私有 SDL Native，不依赖构建期 BGFX integration 或其他平台；命名平台策略不再存在于共享 SDL backend。

## 初始化与生命周期

平台产品向 Sdl3PlatformBackendProvider 显式传入实例。配置由 application 借用；成功创建的窗口归 application。包装失败由 application 销毁新窗口。外部窗口通过 AdoptWindow 登记，ReleaseWindow 使 wrapper 失效后由原 owner 销毁。GetWindows 只返回当前 application 的目录，不扫描全进程窗口。所有操作在原 owner thread 上执行。

```csharp
using Inno.Integration.BrowserWasm32.Sdl3;
using Inno.Adapter.Platform.Sdl3;
using Inno.Platform;

using var app = new Sdl3PlatformApplication(new BrowserWasm32Sdl3HostIntegration());
using var window = app.CreateWindow(new PlatformWindowOptions { title = "Application", width = 960, height = 540 });
```

Windows 解析 Win32 surface；macOS 解析 Cocoa surface 并设置当前按键初始化 hint；Browser 创建 Canvas/OpenGL 窗口，初始焦点由浏览器事件更新，不安装桌面 live-resize 流程。能力不同通过 Sdl3HostCapabilities 声明；不支持多窗口时在 SDK 创建前失败。无 protected 扩展点。

## 失败与测试

创建或 surface 不可用明确异常；借用窗口不重复销毁；已释放 wrapper 拒绝 nativeHandles。HostIntegrationTests、WindowOwnershipTests、WindowSurfaceTests 使用公开 host/window 契约验证。ImGui ViewportMetricsTests 验证同一 surface 与解登记顺序；实际平台状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration`

<!-- api: T:Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration 1abca6e36003d13751487cde6f86b2e430131e1f3117184e9fd4d8fc6ed0bcd9 -->
<!-- api: P:Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.capabilities 6540822868155d41dfa9f0b5d72b59110334bec77427d966685a8949e99f4414 -->
<!-- api: M:Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.ConfigureInitialization ec68d08d2152b27710f922fab4b5041d2e4bd4d6182acf84f6030370e821f4ff -->
<!-- api: M:Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions) 2379a885c122e7773f3c8655dbda22ec22b6e6123ce042e3504beb71bce10e71 -->
<!-- api: M:Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.ResolveNativeSurface(System.IntPtr) 0317b9cf36d987817dc85e78b2bdd134fcbc245a4b3f84213cafbbc7a27e5b41 -->
<!-- api: M:Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.GetInitialFocus(System.IntPtr) be4cde1c571be8fbd5a9e3f7bf36bad3bea06d4e1e549b48e1df6b3b27e9218c -->
<!-- api: M:Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.#ctor b2275854b8ac5b1bda5561c06fc93ba06ee5779f1ef7592363c01bca1b6f0223 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4222ff9774c7d71e"></a>[`public Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.BrowserWasm32Sdl3HostIntegration()`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Sdl3/BrowserWasm32Sdl3HostIntegration.cs#L13) | C# 生成的声明；状态与值语义遵循所属类型契约。 | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-e5aba1ed2a4fb21c"></a>[`public void Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.ConfigureInitialization()`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Sdl3/BrowserWasm32Sdl3HostIntegration.cs#L19) | Applies host-specific SDK configuration before the application's SDL initialization. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-28fe337b4b5a258f"></a>[`public nint Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Sdl3/BrowserWasm32Sdl3HostIntegration.cs#L24) | Creates one native window whose ownership transfers to the application on success.<br>返回：A nonzero opaque SDL window handle. Failure must throw without leaking a window.<br>`options`：The explicit window options supplied by the product. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-69c75e6039feadc0"></a>[`public bool Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.GetInitialFocus(nint windowHandle)`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Sdl3/BrowserWasm32Sdl3HostIntegration.cs#L46) | Reads the initial focus policy before platform events begin updating the wrapper.<br>返回：Whether game and editor input may initially treat the window as focused.<br>`windowHandle`：The live SDL window being registered. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-7c70cbbf50f58b3e"></a>[`public Inno.Adapter.Platform.PlatformNativeHandles Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.ResolveNativeSurface(nint windowHandle)`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Sdl3/BrowserWasm32Sdl3HostIntegration.cs#L30) | Resolves borrowed graphics surface information for a live SDL window.<br>返回：Borrowed surface handles valid until the native window is destroyed; unavailable surfaces fail.<br>`windowHandle`：The opaque handle created or explicitly adopted by the application. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-5976186a68fb11ce"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration.capabilities { get; }`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Sdl3/BrowserWasm32Sdl3HostIntegration.cs#L16) | Gets the host's supported window and scheduling behavior. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-c4a117d6695989f1"></a>[`public sealed class Inno.Integration.BrowserWasm32.Sdl3.BrowserWasm32Sdl3HostIntegration : Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Sdl3/BrowserWasm32Sdl3HostIntegration.cs#L13) | Provides the Browser SDK, native surface and focus policies for the shared SDL backend. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Adapter.Platform.Sdl3](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)：公开引用边界由实际签名核对。
- [Inno.Native.Sdl3](../../backends/Sdl3/Inno.Native.Sdl3.md)：实现依赖，PrivateAssets="compile"。
