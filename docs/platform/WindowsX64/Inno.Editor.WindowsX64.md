# Inno.Editor.WindowsX64

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Windows Editor 的平台产品组合库。选择明确运行目标、共享 backend 和平台系统服务；共同功能不在此复制。

## 组合、生命周期与扩展

`WindowsX64EditorComposition.Run` 是唯一公开启动方法，由统一开发入口选择并调用。运行目标固定为当前已实现的 windows-x64；不同 CPU 复用产品源码，只增加已实现的目标描述和工具链参数。

产品调用共享 EditorApplication.RunAsync，显式注入键盘、作者端 provider、动态模块、序列化和构建 distribution。普通 IDE Build/Publish 准备 Editor Native 闭包；ImGui Shader 以当前 Editor 目标编译。

产品持有调用者资源与配置；共享应用入口接管自身创建的资源。OS 用户目录由 `Inno.Platform.WindowsX64` 提供。窗口、Input、Rendering、Audio、Text 和 UI 选择已有 backend，不在平台复制 SDL/BGFX/MiniAudio。

`WindowsX64EditorComposition.Run(string[] arguments, CancellationToken cancellationToken) : int` 在调用者主线程运行。空参数打开 SDL 文件夹选择窗口；取消返回 0，取消运行或错误参数返回 2，启动失败返回 1。资源位置来自本组合程序集，不借用 bootstrap 目录。日常启动见[产品启动指南](../PRODUCT_STARTUP.md)。手写启动签名只涉及 System 类型；生成目录的公开注册协议依赖 Foundation，单独声明公开引用，其余依赖为实现依赖。

## Backend Catalog 组合

下面展示产品如何选择平台与渲染工厂。Catalog 是不可变 provider 集合；这个例子不会创建窗口或设备。真正启动仍通过 `WindowsX64EditorComposition.Run`，由 Host 创建和释放会话资源。

```csharp
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Adapter.Rendering;
using Inno.Adapter.Rendering.Bgfx;
using Inno.Integration.WindowsX64.Bgfx.Runtime;
using Inno.Integration.WindowsX64.Sdl3;

var platform = new PlatformBackendCatalog(
    [new Sdl3PlatformBackendProvider(new WindowsX64Sdl3HostIntegration())]);
var rendering = new RenderingBackendCatalog(
    [new BgfxRenderingBackendProvider(new WindowsX64BgfxSurfaceIntegration())]);
```

自定义产品把这些工厂及明确 storage factory 传入 `DefaultAdapterCatalogOptions`，然后提供完整的 Editor 启动配置。集成对象被应用借用，窗口 handle 的生命周期仍属于 SDL 应用。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Editor.WindowsX64.WindowsX64EditorComposition`

<!-- api: T:Inno.Editor.WindowsX64.WindowsX64EditorComposition a06779fcf6546eb6f91584e929c2b4ae48af1a58dab9df02fafd45d05c635f35 -->
<!-- api: M:Inno.Editor.WindowsX64.WindowsX64EditorComposition.Run(System.String[],System.Threading.CancellationToken) 16844ebf84914aa7fadb4f3dda674b3005ed81938c05640c4a92edd1ab723ee9 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-68ec0586238a32a7"></a>[`public static int Inno.Editor.WindowsX64.WindowsX64EditorComposition.Run(string[] arguments, System.Threading.CancellationToken cancellationToken)`](../../../platforms/WindowsX64/editor/Inno.Editor.WindowsX64/WindowsX64EditorComposition.cs#L54) | Runs the platform Editor on the caller's main thread; no directory opens the native folder dialog.<br>`arguments`：An optional project directory and the shared Editor command-line options.<br>返回：Zero on normal exit or dialog cancellation, two on cancellation or invalid arguments, or one on startup failure.<br>`cancellationToken`：Cancels preparation or requests orderly shutdown of the shared Editor host. An open native folder dialog retains its callback owner until the user completes or closes it. |
| <a id="api-f54223242db76a35"></a>[`public static class Inno.Editor.WindowsX64.WindowsX64EditorComposition`](../../../platforms/WindowsX64/editor/Inno.Editor.WindowsX64/WindowsX64EditorComposition.cs#L39) | Composes the Windows Editor from explicit platform integrations and shared product hosting. |

## 项目依赖

- [Inno.Core.Serialization](../../core/Inno.Core.Serialization.md) 与 [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：生成注册 API 的公开签名依赖，独立公共引用组。
- [Inno.Integration.WindowsX64.Bgfx.Runtime](Inno.Integration.WindowsX64.Bgfx.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.WindowsX64.Sdl3](Inno.Integration.WindowsX64.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.WindowsX64.Bgfx](Inno.Integration.WindowsX64.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Presentation.ImGui.Bgfx](../../backends/ImGui/Inno.Adapter.Presentation.ImGui.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.WindowsX64](Inno.Build.WindowsX64.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Hosting](../../editor/Inno.Editor.Hosting.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Authoring.Default](../../runtime/Inno.Adapter.Authoring.Default.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Storage.FileSystem](../../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Modules.DotNet](../../backends/DotNet/Inno.Adapter.Modules.DotNet.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Serialization.DotNet](../../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Distribution.Standard](../../build/Inno.Build.Distribution.Standard.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Platform.WindowsX64](Inno.Platform.WindowsX64.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：实现依赖，PrivateAssets="compile"。

## 开发工程调用

统一 Editor entry 以条件 ProjectReference 引用本库，并在主线程直接调用 Run。Native 支持仍由本产品准备，其 native 目录通过 SDK 内容输出传递到 entry；不依赖预构建启动记录或运行时子 MSBuild。默认 Solution 中本产品不作为第二个独立构建根。
