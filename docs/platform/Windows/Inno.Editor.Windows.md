# Inno.Editor.Windows

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Windows Editor 的平台产品组合库。选择明确运行目标、共享 backend 和平台系统服务；共同功能不在此复制。

## 组合、生命周期与扩展

`WindowsEditorComposition.Run` 是唯一公开启动方法，由统一开发入口选择并调用。运行目标固定为当前已实现的 windows-x64；不同 CPU 复用产品源码，只增加已实现的目标描述和工具链参数。

产品调用共享 EditorApplication.RunAsync，显式注入键盘、作者端 provider、动态模块、序列化和构建 distribution。普通 IDE Build/Publish 准备 Editor Native 闭包；ImGui Shader 以当前 Editor 目标编译。

产品持有调用者资源与配置；共享应用入口接管自身创建的资源。OS 用户目录由 `Inno.Platform.Windows` 提供。窗口、Input、Rendering、Audio、Text 和 UI 选择已有 backend，不在平台复制 SDL/BGFX/MiniAudio。

`WindowsEditorComposition.Run(string[] arguments, CancellationToken cancellationToken) : int` 在调用者主线程运行。空参数打开 SDL 文件夹选择窗口；取消返回 0，取消运行或错误参数返回 2，启动失败返回 1。资源位置来自本组合程序集，不借用 bootstrap 目录。日常启动见[产品启动指南](../PRODUCT_STARTUP.md)。手写启动签名只涉及 System 类型；生成目录的公开注册协议依赖 Foundation，单独声明公开引用，其余依赖为实现依赖。

## Backend Catalog 组合

下面展示产品如何选择平台与渲染工厂。Catalog 是不可变 provider 集合；这个例子不会创建窗口或设备。真正启动仍通过 `WindowsEditorComposition.Run`，由 Host 创建和释放会话资源。

```csharp
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Adapter.Rendering;
using Inno.Adapter.Rendering.Bgfx;
using Inno.Integration.Windows.Bgfx.Runtime;
using Inno.Integration.Windows.Sdl3;

var platform = new PlatformBackendCatalog(
    [new Sdl3PlatformBackendProvider(new WindowsSdl3HostIntegration())]);
var rendering = new RenderingBackendCatalog(
    [new BgfxRenderingBackendProvider(new WindowsBgfxSurfaceIntegration())]);
```

自定义产品把这些工厂及明确 storage factory 传入 `DefaultAdapterCatalogOptions`，然后提供完整的 Editor 启动配置。集成对象被应用借用，窗口 handle 的生命周期仍属于 SDL 应用。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Editor.Windows.WindowsEditorComposition`

<!-- api: T:Inno.Editor.Windows.WindowsEditorComposition 024e6a0f20288a836bc60fdb5cd783f973994b8a47bad4e0c28b0bba17256e54 -->
<!-- api: M:Inno.Editor.Windows.WindowsEditorComposition.Run(System.String[],System.Threading.CancellationToken) ce5cb811a570fa00ca29ecdcd0b921fd53075bf9b74a2a82e7320d73091f4fb0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-40270c2a863129ee"></a>[`public static int Inno.Editor.Windows.WindowsEditorComposition.Run(string[] arguments, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Windows/editor/Inno.Editor.Windows/WindowsEditorComposition.cs#L54) | Runs the platform Editor on the caller's main thread; no directory opens the native folder dialog.<br>返回：Zero on normal exit or dialog cancellation, two on cancellation or invalid arguments, or one on startup failure.<br>`arguments`：An optional project directory and the shared Editor command-line options.<br>`cancellationToken`：Cancels preparation or requests orderly shutdown of the shared Editor host. An open native folder dialog retains its callback owner until the user completes or closes it. | — | — |
| <a id="api-16a704ba7b8ac33c"></a>[`public static class Inno.Editor.Windows.WindowsEditorComposition`](../../../platforms/Windows/editor/Inno.Editor.Windows/WindowsEditorComposition.cs#L39) | Composes the Windows Editor from explicit platform integrations and shared product hosting. | — | — |

## 项目依赖

- [Inno.Core.Serialization](../../core/Inno.Core.Serialization.md) 与 [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：生成注册 API 的公开签名依赖，独立公共引用组。
- [Inno.Integration.Windows.Bgfx.Runtime](Inno.Integration.Windows.Bgfx.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.Windows.Sdl3](Inno.Integration.Windows.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.Windows.Bgfx](Inno.Integration.Windows.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Presentation.ImGui.Bgfx](../../backends/ImGui/Inno.Adapter.Presentation.ImGui.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Windows](Inno.Build.Windows.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Hosting](../../editor/Inno.Editor.Hosting.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Authoring.Default](../../runtime/Inno.Adapter.Authoring.Default.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Storage.FileSystem](../../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Modules.DotNet](../../backends/DotNet/Inno.Adapter.Modules.DotNet.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Serialization.DotNet](../../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Distribution.Standard](../../build/Inno.Build.Distribution.Standard.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Platform.Windows](Inno.Platform.Windows.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：实现依赖，PrivateAssets="compile"。
