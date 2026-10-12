# Inno.Editor.MacOS

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

MacOS Editor 的平台产品组合库。选择明确运行目标、共享 backend 和平台系统服务；共同功能不在此复制。

## 组合、生命周期与扩展

`MacOSEditorComposition.Run` 是唯一公开启动方法，由统一开发入口选择并调用。运行目标固定为当前已实现的 macos-arm64；不同 CPU 复用产品源码，只增加已实现的目标描述和工具链参数。

产品调用共享 EditorApplication.RunAsync，显式注入键盘、作者端 provider、动态模块、序列化和构建 distribution。普通 IDE Build/Publish 准备 Editor Native 闭包；ImGui Shader 以当前 Editor 目标编译。

产品持有调用者资源与配置；共享应用入口接管自身创建的资源。OS 用户目录由 `Inno.Platform.MacOS` 提供。窗口、Input、Rendering、Audio、Text 和 UI 选择已有 backend，不在平台复制 SDL/BGFX/MiniAudio。

`MacOSEditorComposition.Run(string[] arguments, CancellationToken cancellationToken) : int` 在调用者主线程运行。空参数打开 SDL 文件夹选择窗口；取消返回 0，取消运行或错误参数返回 2，启动失败返回 1。资源位置来自本组合程序集，不借用 bootstrap 目录。手写启动签名只涉及 System 类型；生成目录的公开注册协议依赖 Foundation，单独声明公开引用，其余依赖为实现依赖。日常启动见[产品启动指南](../PRODUCT_STARTUP.md)；本轮未实测 macOS Native 与产品运行。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Editor.MacOS.MacOSEditorComposition`

<!-- api: T:Inno.Editor.MacOS.MacOSEditorComposition 9a97d8f7d838d5105dd512fa518a0c231beabeb12aa187b68d53ba6bfe4d1bc3 -->
<!-- api: M:Inno.Editor.MacOS.MacOSEditorComposition.Run(System.String[],System.Threading.CancellationToken) f91d38c499fe6eed355270bff23967ee65de4848328c2cf611b53530dc95b154 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-72acba6608572bbe"></a>[`public static int Inno.Editor.MacOS.MacOSEditorComposition.Run(string[] arguments, System.Threading.CancellationToken cancellationToken)`](../../../platforms/MacOS/editor/Inno.Editor.MacOS/MacOSEditorComposition.cs#L54) | Runs the platform Editor on the caller's main thread; no directory opens the native folder dialog.<br>返回：Zero on normal exit or dialog cancellation, two on cancellation or invalid arguments, or one on startup failure.<br>`arguments`：An optional project directory and the shared Editor command-line options.<br>`cancellationToken`：Cancels preparation or requests orderly shutdown of the shared Editor host. An open native folder dialog retains its callback owner until the user completes or closes it. | — | — |
| <a id="api-ab48cbefdf773acf"></a>[`class Inno.Editor.MacOS.MacOSEditorComposition`](../../../platforms/MacOS/editor/Inno.Editor.MacOS/MacOSEditorComposition.cs#L39) | Composes the MacOS Editor from explicit platform integrations and shared product hosting. | — | — |

## 项目依赖

- [Inno.Core.Serialization](../../core/Inno.Core.Serialization.md) 与 [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：生成注册 API 的公开签名依赖，独立公共引用组。
- [Inno.Integration.MacOS.Bgfx.Runtime](Inno.Integration.MacOS.Bgfx.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.MacOS.Sdl3](Inno.Integration.MacOS.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.MacOS.Bgfx](Inno.Integration.MacOS.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Presentation.ImGui.Bgfx](../../backends/ImGui/Inno.Adapter.Presentation.ImGui.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.MacOS](Inno.Build.MacOS.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Hosting](../../editor/Inno.Editor.Hosting.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Authoring.Default](../../runtime/Inno.Adapter.Authoring.Default.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Storage.FileSystem](../../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Modules.DotNet](../../backends/DotNet/Inno.Adapter.Modules.DotNet.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Serialization.DotNet](../../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Distribution.Standard](../../build/Inno.Build.Distribution.Standard.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Platform.MacOS](Inno.Platform.MacOS.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：实现依赖，PrivateAssets="compile"。
