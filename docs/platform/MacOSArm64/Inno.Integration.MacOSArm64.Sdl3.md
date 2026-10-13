# Inno.Integration.MacOSArm64.Sdl3

[平台索引](README.md) · [Wiki 首页](../../README.md) · [SDL backend](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)

## 职责与边界

实现 ISdl3HostIntegration，提供 MacOS 的 SDL 初始化、窗口创建、native surface 与初始焦点。只依赖共享 SDL Adapter 和私有 SDL Native，不依赖构建期 BGFX integration 或其他平台；命名平台策略不再存在于共享 SDL backend。

## 初始化与生命周期

平台产品向 Sdl3PlatformBackendProvider 显式传入实例。配置由 application 借用；成功创建的窗口归 application。包装失败由 application 销毁新窗口。外部窗口通过 AdoptWindow 登记，ReleaseWindow 使 wrapper 失效后由原 owner 销毁。GetWindows 只返回当前 application 的目录，不扫描全进程窗口。所有操作在原 owner thread 上执行。

```csharp
using Inno.Integration.MacOSArm64.Sdl3;
using Inno.Adapter.Platform.Sdl3;
using Inno.Platform;

using var app = new Sdl3PlatformApplication(new MacOSArm64Sdl3HostIntegration());
using var window = app.CreateWindow(new PlatformWindowOptions { title = "Application", width = 960, height = 540 });
```

Windows 解析 Win32 surface；macOS 解析 Cocoa surface 并设置当前按键初始化 hint；Browser 创建 Canvas/OpenGL 窗口，初始焦点由浏览器事件更新，不安装桌面 live-resize 流程。能力不同通过 Sdl3HostCapabilities 声明；不支持多窗口时在 SDK 创建前失败。无 protected 扩展点。

## 失败与测试

创建或 surface 不可用明确异常；借用窗口不重复销毁；已释放 wrapper 拒绝 nativeHandles。HostIntegrationTests、WindowOwnershipTests、WindowSurfaceTests 使用公开 host/window 契约验证。ImGui ViewportMetricsTests 验证同一 surface 与解登记顺序；实际平台状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration`

<!-- api: T:Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration 73d51ac5d332e366d2753920f4c72023d7c2379b53faa2a6a64a1629e70116c5 -->
<!-- api: P:Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.capabilities 7423036e93ad1b56660c5c1a1a9b3e8e887122908a66b8c076921d074f259a76 -->
<!-- api: M:Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.ConfigureInitialization 393fe2ecdd6e78f4141f40c99505a2c16ca5d82b297abe9427ffa2787b802b69 -->
<!-- api: M:Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions) f5f206862797df7268523cf0bfcc8d01d13a677c2df168683e33b460ef4660f5 -->
<!-- api: M:Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.ResolveNativeSurface(System.IntPtr) ee9d2acfc4aaf373caff2bc0e367eae3e8a3cc6d6fbb6775d765daaf29cc5131 -->
<!-- api: M:Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.GetInitialFocus(System.IntPtr) 0a0739116ed2760d070a5b044a62b8974e8aea5115d4a517d7a86ffb6ae52ae5 -->
<!-- api: M:Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.#ctor 4e7273bb972ed7a227766502ff49f9082494750dc495c64ce764df9b678d2071 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2469e37f832f9185"></a>[`public Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.MacOSArm64Sdl3HostIntegration()`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Sdl3/MacOSArm64Sdl3HostIntegration.cs#L12) | C# 生成的声明；状态与值语义遵循所属类型契约。 | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-cf1f8b01c7eb1585"></a>[`public void Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.ConfigureInitialization()`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Sdl3/MacOSArm64Sdl3HostIntegration.cs#L18) | Applies host-specific SDK configuration before the application's SDL initialization. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-58b48f8baf9befb1"></a>[`public nint Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Sdl3/MacOSArm64Sdl3HostIntegration.cs#L24) | Creates one native window whose ownership transfers to the application on success.<br>返回：A nonzero opaque SDL window handle. Failure must throw without leaking a window.<br>`options`：The explicit window options supplied by the product. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-330ee71004f9c187"></a>[`public bool Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.GetInitialFocus(nint windowHandle)`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Sdl3/MacOSArm64Sdl3HostIntegration.cs#L43) | Reads the initial focus policy before platform events begin updating the wrapper.<br>返回：Whether game and editor input may initially treat the window as focused.<br>`windowHandle`：The live SDL window being registered. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-08fd9fc5558a49f4"></a>[`public Inno.Adapter.Platform.PlatformNativeHandles Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.ResolveNativeSurface(nint windowHandle)`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Sdl3/MacOSArm64Sdl3HostIntegration.cs#L30) | Resolves borrowed graphics surface information for a live SDL window.<br>返回：Borrowed surface handles valid until the native window is destroyed; unavailable surfaces fail.<br>`windowHandle`：The opaque handle created or explicitly adopted by the application. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-6ee6ad3c48275041"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration.capabilities { get; }`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Sdl3/MacOSArm64Sdl3HostIntegration.cs#L15) | Gets the host's supported window and scheduling behavior. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-ff09f5668d7cd384"></a>[`public sealed class Inno.Integration.MacOSArm64.Sdl3.MacOSArm64Sdl3HostIntegration : Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Sdl3/MacOSArm64Sdl3HostIntegration.cs#L12) | Provides the MacOS SDK, native surface and focus policies for the shared SDL backend. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Adapter.Platform.Sdl3](../../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)：公开引用边界由实际签名核对。
- [Inno.Native.Sdl3](../../backends/Sdl3/Inno.Native.Sdl3.md)：实现依赖，PrivateAssets="compile"。
