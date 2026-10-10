# Inno.Editor.Hosting

[分类索引](README.md) · [Wiki 首页](../README.md) · [平台归属与扩展](../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

共享 Editor 的启动和生命周期库。没有 Program、具体平台、标准发行注册或后端默认选择；EditorHost 保持内部。

`src/composition/editor/EditorProduct.props` 是内置 Editor 功能、Panel 和呈现程序集的唯一产品依赖声明。Windows、macOS 产品共同导入它，保证仅由 attribute 发现、没有 CLR 调用引用的扩展仍进入部署目录和产品依赖清单。Hosting 的项目引用仅承担自身实现或公开契约所需依赖，不通过类型锚点、运行时目录扫描或重复平台列表补齐产品。

## 组合、生命周期与扩展

公开入口为 `EditorApplication.RunAsync(EditorLaunchOptions, CancellationToken)`。产品传入项目、catalog、selection、窗口、帧驱动、键盘约定、distribution、明确初始导出目标、SDK上下文、模块/序列化来源与日志工厂。

配置先完整预检：目标注册、selection、配对创作 provider、presentation、键盘约定和正 frame limit。预取消不创建 Project、不启动 Native、不调用资源工厂。错误配置不能落盘创建半成品。

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Editor.Hosting;

static Task<int> Run(EditorLaunchOptions options, CancellationToken cancellation)
{
    return EditorApplication.RunAsync(options, cancellation);
}
```

catalog、不可变配置和外部服务借用；factory 返回的新 engine、Session、设备、任务与 callback 由 Host 接管。退出先停止任务、注销回调、保存最新 Editor 状态与 layout，再退休场景/资源，最后释放窗口与 Native owner。Pending/Faulted 和完整 GC 弱监测继续有效，不因平台拆分绕过。

Project 创作 IO 属于创作服务；OS 用户位置由平台产品提供。不同导出目标不改变当前 Editor 的 Native 或 ImGui Shader 目标。入口不暴露内部 Host 给测试；测试通过本公开边界验证预检、取消和实际产品运行。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Editor.Hosting.EditorApplication`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Hosting.EditorApplication`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorApplication.cs#L11) | Runs the shared Editor product with explicitly supplied platform and publication composition. |
| [`static System.Threading.Tasks.Task<int> Inno.Editor.Hosting.EditorApplication.RunAsync(Inno.Editor.Hosting.EditorLaunchOptions options, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorApplication.cs#L40) | Validates composition, owns the created Editor host and retires resources on exit, cancellation or failure. |

### `Inno.Editor.Hosting.EditorCommandLineOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Hosting.EditorCommandLineOptions`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L10) | Parses the shared Editor project and verification workflow before platform composition starts. |
| [`Inno.Rendering.GraphicsApi? Inno.Editor.Hosting.EditorCommandLineOptions.graphicsApi`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L27) | Gets an explicit renderer preference, or null for backend policy. |
| [`int? Inno.Editor.Hosting.EditorCommandLineOptions.smokeFrameLimit`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L22) | Gets the optional bounded verification frame count. |
| [`static Inno.Editor.Hosting.EditorCommandLineOptions Inno.Editor.Hosting.EditorCommandLineOptions.Parse(string[] arguments)`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L41) | Parses the common Editor command line without selecting a platform. |
| [`string Inno.Editor.Hosting.EditorCommandLineOptions.projectDirectory`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L17) | Gets the caller-selected authoring project. |

### `Inno.Editor.Hosting.EditorLaunchOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Hosting.EditorLaunchOptions`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L17) | Supplies product-owned composition to the shared Editor lifecycle without selecting a platform or backend. |
| [`System.Func<Inno.Core.Logging.ILogSink?>? Inno.Editor.Hosting.EditorLaunchOptions.createHostLogSink`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L87) | Gets an optional factory transferring one host log sink to the Editor lifetime. |
| [`System.Func<Inno.Runtime.RuntimeSessionKind, Inno.Core.Logging.LogSessionId, Inno.Core.Logging.ILogSink>? Inno.Editor.Hosting.EditorLaunchOptions.createSessionLogSink`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L82) | Gets an optional factory transferring each edit or play log sink to its owning session. |
| [`int? Inno.Editor.Hosting.EditorLaunchOptions.smokeFrameLimit`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L92) | Gets an optional positive frame limit for bounded product verification. |
| [`required Inno.Adapter.Presentation.IAuthoringAdapterCatalog Inno.Editor.Hosting.EditorLaunchOptions.adapterCatalog`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L32) | Gets the borrowed immutable runtime, authoring and presentation provider catalogs. |
| [`required Inno.Adapter.Presentation.PresentationBackendId Inno.Editor.Hosting.EditorLaunchOptions.presentation`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L42) | Gets the explicit presentation implementation selected by the product. |
| [`required Inno.Build.BuildTargetId Inno.Editor.Hosting.EditorLaunchOptions.defaultBuildTarget`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L57) | Gets the explicit default game target for new project build settings. |
| [`required Inno.Build.Composition.BuildCompositionContext Inno.Editor.Hosting.EditorLaunchOptions.buildContext`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L62) | Gets the explicitly selected SDK and build execution host. |
| [`required Inno.Build.Composition.BuildDistribution Inno.Editor.Hosting.EditorLaunchOptions.distribution`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L52) | Gets the borrowed immutable publication distribution. |
| [`required Inno.Editor.Core.EditorKeyboardPolicy Inno.Editor.Hosting.EditorLaunchOptions.keyboard`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L27) | Gets explicit keyboard conventions selected by the Editor product. |
| [`required Inno.Shell.IShellFrameDriver Inno.Editor.Hosting.EditorLaunchOptions.frameDriver`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L47) | Gets the borrowed owner-thread scheduling policy. |
| [`required Inno.Shell.ShellOptions Inno.Editor.Hosting.EditorLaunchOptions.shell`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L37) | Gets explicit backend, primary-window and rendering policy selected by the product. |
| [`required System.Func<Inno.Runtime.EngineHost> Inno.Editor.Hosting.EditorLaunchOptions.createEngineHost`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L72) | Gets the factory transferring a newly created metadata-configured engine to the Editor owner. |
| [`required System.Func<Inno.Scripting.Compiler.ScriptModuleDeployment, Inno.Extensibility.Modules.IModuleSource> Inno.Editor.Hosting.EditorLaunchOptions.createScriptModuleSource`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L77) | Gets the factory transferring a candidate script module source to the existing reload transaction. |
| [`required string Inno.Editor.Hosting.EditorLaunchOptions.projectDirectory`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L22) | Gets the current authoring project location; shared authoring services own its project IO. |
| [`required string Inno.Editor.Hosting.EditorLaunchOptions.supportPackRoot`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L67) | Gets the explicit installed Support Pack location provided by product composition. |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Engine.Default](../runtime/Inno.Engine.Default.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter](../runtime/Inno.Adapter.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Audio](../audio/Inno.Adapter.Audio.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Animation.Assets](../animation/Inno.Animation.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Animation.Runtime](../animation/Inno.Animation.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Audio](Inno.Editor.Audio.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.ImGui](Inno.Editor.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.PlayMode](Inno.Editor.PlayMode.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Rendering](Inno.Editor.Rendering.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Scene](Inno.Editor.Scene.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Settings](Inno.Editor.Settings.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Audio.Assets](../audio/Inno.Audio.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Audio.Runtime](../audio/Inno.Audio.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Text.Assets](../text/Inno.Text.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.UI.Assets](../ui/Inno.UI.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.UI.Runtime](../ui/Inno.UI.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Layers](../core/Inno.Core.Layers.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Input.Runtime](../input/Inno.Input.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scene](../scene/Inno.Scene.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scene.Assets](../scene/Inno.Scene.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Platform](../platform/Inno.Platform.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering.Assets](../rendering/Inno.Rendering.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering.Runtime](../rendering/Inno.Rendering.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Storage.Runtime](../storage/Inno.Storage.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Managed](../build/Inno.Build.Managed.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering.Assets.Authoring](../rendering/Inno.Rendering.Assets.Authoring.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Build.Composition](../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Shell](../runtime/Inno.Shell.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Presentation](../platform/Inno.Adapter.Presentation.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Runtime](../runtime/Inno.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Build](../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](../rendering/Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Compiler](../scripting/Inno.Scripting.Compiler.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Core](Inno.Editor.Core.md)：公开引用边界由实际签名核对。
