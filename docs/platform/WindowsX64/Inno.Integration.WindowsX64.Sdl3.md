# Inno.Integration.WindowsX64.Sdl3

[平台索引](README.md) · [Wiki 首页](../../README.md) · [SDL backend](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)

## 职责与边界

实现 ISdl3HostIntegration，提供 Windows 的 SDL 初始化、窗口创建、native surface 与初始焦点。只依赖共享 SDL Adapter 和私有 SDL Native，不依赖构建期 BGFX integration 或其他平台；命名平台策略不再存在于共享 SDL backend。

## 初始化与生命周期

平台产品向 Sdl3PlatformBackendProvider 显式传入实例。配置由 application 借用；成功创建的窗口归 application。包装失败由 application 销毁新窗口。外部窗口通过 AdoptWindow 登记，ReleaseWindow 使 wrapper 失效后由原 owner 销毁。GetWindows 只返回当前 application 的目录，不扫描全进程窗口。所有操作在原 owner thread 上执行。

```csharp
using Inno.Integration.WindowsX64.Sdl3;
using Inno.Adapter.Platform.Sdl3;
using Inno.Platform;

using var app = new Sdl3PlatformApplication(new WindowsX64Sdl3HostIntegration());
using var window = app.CreateWindow(new PlatformWindowOptions { title = "Application", width = 960, height = 540 });
```

Windows 解析 Win32 surface；macOS 解析 Cocoa surface 并设置当前按键初始化 hint；Browser 创建 Canvas/OpenGL 窗口，初始焦点由浏览器事件更新，不安装桌面 live-resize 流程。能力不同通过 Sdl3HostCapabilities 声明；不支持多窗口时在 SDK 创建前失败。无 protected 扩展点。

## 失败与测试

创建或 surface 不可用明确异常；借用窗口不重复销毁；已释放 wrapper 拒绝 nativeHandles。HostIntegrationTests、WindowOwnershipTests、WindowSurfaceTests 使用公开 host/window 契约验证。ImGui ViewportMetricsTests 验证同一 surface 与解登记顺序；实际平台状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration`

<!-- api: T:Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration 35716570ab0d3037a8bd940571c3cdb2aafc916540a07185c85cd51f667ceec7 -->
<!-- api: P:Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.capabilities 2d7c800f455c446730af79a005e74fafcba7c5cdd2eae8c03bd77032623f2690 -->
<!-- api: M:Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.ConfigureInitialization 835d373b9d1d05c223b0c88b294e883a2d37b8de845f472740880c7472765224 -->
<!-- api: M:Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions) fad052b976a014c68a1e657a406b3e75ee547519d7fcd9b7fb38a12eabc78a7d -->
<!-- api: M:Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.ResolveNativeSurface(System.IntPtr) 7c9d93951667c3330095eec030d54571a3e17de411a1f75ca8990e7b61159f78 -->
<!-- api: M:Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.GetInitialFocus(System.IntPtr) 8e5263027f28501777afc05cdaa9986408602f9100c3daa8b22023b6cb83969c -->
<!-- api: M:Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.#ctor ab41281d316a2f8a223f2f6cce6035652f34e276c00b7af9fba6234b4fc6061a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dcf8337d686e6faa"></a>[`public Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.WindowsX64Sdl3HostIntegration()`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Sdl3/WindowsX64Sdl3HostIntegration.cs#L12) | C# 生成的声明；状态与值语义遵循所属类型契约。 | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-c2262af29453ae56"></a>[`public void Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.ConfigureInitialization()`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Sdl3/WindowsX64Sdl3HostIntegration.cs#L18) | Applies host-specific SDK configuration before the application's SDL initialization. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-500658734a7b84ae"></a>[`public nint Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Sdl3/WindowsX64Sdl3HostIntegration.cs#L23) | Creates one native window whose ownership transfers to the application on success.<br>返回：A nonzero opaque SDL window handle. Failure must throw without leaking a window.<br>`options`：The explicit window options supplied by the product. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-a7ea89f2699d0dbb"></a>[`public bool Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.GetInitialFocus(nint windowHandle)`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Sdl3/WindowsX64Sdl3HostIntegration.cs#L42) | Reads the initial focus policy before platform events begin updating the wrapper.<br>返回：Whether game and editor input may initially treat the window as focused.<br>`windowHandle`：The live SDL window being registered. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-f39142267b6c3dd3"></a>[`public Inno.Adapter.Platform.PlatformNativeHandles Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.ResolveNativeSurface(nint windowHandle)`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Sdl3/WindowsX64Sdl3HostIntegration.cs#L29) | Resolves borrowed graphics surface information for a live SDL window.<br>返回：Borrowed surface handles valid until the native window is destroyed; unavailable surfaces fail.<br>`windowHandle`：The opaque handle created or explicitly adopted by the application. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-a9ddf27c1a6349ca"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration.capabilities { get; }`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Sdl3/WindowsX64Sdl3HostIntegration.cs#L15) | Gets the host's supported window and scheduling behavior. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-ab43dd7b893be83e"></a>[`public sealed class Inno.Integration.WindowsX64.Sdl3.WindowsX64Sdl3HostIntegration : Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Sdl3/WindowsX64Sdl3HostIntegration.cs#L12) | Provides the Windows SDK, native surface and focus policies for the shared SDL backend. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Adapter.Platform.Sdl3](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)：公开引用边界由实际签名核对。
- [Inno.Native.Sdl3](../../backends/Sdl3/Inno.Native.Sdl3.md)：实现依赖，PrivateAssets="compile"。
