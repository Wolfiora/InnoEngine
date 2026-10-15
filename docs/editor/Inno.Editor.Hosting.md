# Inno.Editor.Hosting

[分类索引](README.md) · [Wiki 首页](../README.md) · [平台归属与扩展](../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

共享 Editor 的启动和生命周期库。没有 Program、具体平台、标准发行注册或后端默认选择；EditorHost 保持内部。

`src/composition/editor/EditorProduct.props` 是内置 Editor 功能、Panel 和呈现程序集的唯一产品依赖声明。Windows、macOS 产品共同导入它，保证仅由 attribute 发现、没有 CLR 调用引用的扩展仍进入部署目录和产品依赖清单。Hosting 的项目引用仅承担自身实现或公开契约所需依赖，不通过类型锚点、运行时目录扫描或重复平台列表补齐产品。

## 组合、生命周期与扩展

公开入口为 `EditorApplication.RunAsync(EditorLaunchOptions, CancellationToken)`。产品传入项目、catalog、selection、窗口、帧驱动、键盘约定、distribution、明确初始导出目标、SDK上下文、模块/序列化来源与日志工厂。

配置先完整预检：目标注册、selection、配对创作 provider、presentation、键盘约定和正 frame limit。预取消不创建 Project、不启动 Native、不调用资源工厂。错误配置不能落盘创建半成品。

`EditorCommandLineOptions` 默认显示窗口。明确传入 Project 后，`--window-visible false --smoke-frames 60` 可以在隐藏窗口中运行同一 Host、设备和退出流程，不操作桌面焦点；它验证初始化和生命周期，不能证明 GUI、视觉或声学效果。可见性不是平台选择，也不替代正常交互验收。

启动诊断直接记录设备的 `primaryPresentationEncodesSrgb` 中立能力。Smoke 验证实际后端、颜色输出能力、完成帧数、错误诊断和正常退出；不依赖第三方调试构建是否输出某一条英文日志。`RunAsync` 返回前由 `using` 释放 Shell，设备释放失败会传播，不能仅凭产品资源的 Dispose 日志判断整个设备已经释放。

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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Editor.Hosting.EditorApplication`

<!-- api: T:Inno.Editor.Hosting.EditorApplication 779a9eb9c073b04b1a1da11647c0c4cb38687e967e4c6403f210f3a28890f997 -->
<!-- api: M:Inno.Editor.Hosting.EditorApplication.RunAsync(Inno.Editor.Hosting.EditorLaunchOptions,System.Threading.CancellationToken) f8ae63b8c40aa255a1de93943731019e708907c40efaf197c74214b122d2a8ff -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7caea7eb5327ca45"></a>[`public static System.Threading.Tasks.Task<int> Inno.Editor.Hosting.EditorApplication.RunAsync(Inno.Editor.Hosting.EditorLaunchOptions options, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorApplication.cs#L40) | Validates composition, owns the created Editor host and retires resources on exit, cancellation or failure.<br>返回：Zero after orderly completion; startup, execution and retirement failures propagate.<br>`options`：Borrowed catalogs, factories and product configuration; created resources transfer to the host.<br>`cancellationToken`：Cancels before creation or requests orderly owner-thread shutdown. | — | `System.ArgumentNullException`：Options or a required dependency is null.<br>`System.ArgumentException`：A required location or product selection is invalid.<br>`System.NotSupportedException`：A selected backend lacks its matching runtime or authoring provider.<br>`System.OperationCanceledException`：Startup or execution was canceled after resource retirement.<br>`System.InvalidOperationException`：Product startup or resource retirement fails. |
| <a id="api-ae0867da67a39e55"></a>[`public static class Inno.Editor.Hosting.EditorApplication`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorApplication.cs#L11) | Runs the shared Editor product with explicitly supplied platform and publication composition. | — | — |

### `Inno.Editor.Hosting.EditorCommandLineOptions`

<!-- api: P:Inno.Editor.Hosting.EditorCommandLineOptions.windowVisible 8b3cff7e40c4b8932296da36d374053ae1fdb5b08af7e1a2ebebc2ef72278a4e -->

<!-- api: T:Inno.Editor.Hosting.EditorCommandLineOptions fd15e5fd61d89471431593d80cd25cc48cce63b8249ae3eb5cf4191719d6ec0a -->
<!-- api: P:Inno.Editor.Hosting.EditorCommandLineOptions.projectDirectory d83304e24c2ae152e6692a7ccbc45b43bd7b06d162ffb774630c7287ebfc97f4 -->
<!-- api: P:Inno.Editor.Hosting.EditorCommandLineOptions.smokeFrameLimit d2f91776ab75dd1ad0976a408f2b09cb5cb949897e8901072dbbe6a78ded490a -->
<!-- api: P:Inno.Editor.Hosting.EditorCommandLineOptions.graphicsApi f3001171409a91f3c773db0a73c8c15407dd187110594292f00c1ae40f083e77 -->
<!-- api: M:Inno.Editor.Hosting.EditorCommandLineOptions.Parse(System.String[]) ce01e405a4995daffee7f58e043c222cd567a999e1c5b3a7bf86e90825f6e0c3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-17cdafb64bf2912c"></a>[`public bool Inno.Editor.Hosting.EditorCommandLineOptions.windowVisible { get; private init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L27) | 是否显示真实平台窗口；默认 true，false 保留同一 Editor 初始化、帧循环与退出。 | 启动配置不可变；产品组合将值传给 PlatformWindowOptions。 | 参数解析失败发生在创建设备前；隐藏运行不验证 GUI 或焦点。 |
| <a id="api-0d2fbb58476a4b3e"></a>[`public static Inno.Editor.Hosting.EditorCommandLineOptions Inno.Editor.Hosting.EditorCommandLineOptions.Parse(string[] arguments)`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L41) | Parses the common Editor command line without selecting a platform.<br>返回：The immutable project startup configuration.<br>`arguments`：The project path followed by optional name/value pairs. | — | `System.ArgumentException`：The project or an option is missing, repeated or invalid. |
| <a id="api-1071da5e2056ccf2"></a>[`public Inno.Rendering.GraphicsApi? Inno.Editor.Hosting.EditorCommandLineOptions.graphicsApi { get; private init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L27) | Gets an explicit renderer preference, or null for backend policy. | — | — |
| <a id="api-29f225c5e1e36c31"></a>[`public string Inno.Editor.Hosting.EditorCommandLineOptions.projectDirectory { get; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L17) | Gets the caller-selected authoring project. | — | — |
| <a id="api-3c3a465645ba3c3b"></a>[`public int? Inno.Editor.Hosting.EditorCommandLineOptions.smokeFrameLimit { get; private init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L22) | Gets the optional bounded verification frame count. | — | — |
| <a id="api-ad59f502c90c8a47"></a>[`public sealed class Inno.Editor.Hosting.EditorCommandLineOptions`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorCommandLineOptions.cs#L10) | Parses the shared Editor project and verification workflow before platform composition starts. | — | — |

### `Inno.Editor.Hosting.EditorLaunchOptions`

<!-- api: T:Inno.Editor.Hosting.EditorLaunchOptions 182a68f3071287ee6610e552b4d487318bc003843aa7a755c613751c517ee8d2 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.projectDirectory 40f33b2698088a4e2b88ddbb37c1c2c26d3927bf15a8aae911a87b4bcb693b33 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.keyboard 9f22d5759961536e336b74e4b40c2c6da84e4b033898e11e0d797a03afd34b72 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.adapterCatalog 1e764e265be2197d717b55ed58333dfda360a0d9ee6403f6e1d31b7f3f4189bc -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.shell 93f14642f5d0cf26d38912d99aa8b6fb6ffc3a3216965425589b735b0ff99be1 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.presentation 8b24904d36eceeef103bb837ed6f3fbd44773c9b18cfb56b81be6fc2364925c1 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.frameDriver 8e072ba306d84b935f8c2d912b6d8d9eca9d62056861cfdc8b71d9c29b981f07 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.distribution 7d0eefeba9f91d0f27fe865fee72bf7bd100caa0f8f07f7cb39c70b3eb2d80c4 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.defaultBuildTarget fccd95669a72443ca1665e209b9b21f0651634c73fc12ce35de5c56d34cfd055 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.buildContext 9564d2930ede1b82328eef1e053bafeac836b767e80f3b6892f5c364297eb615 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.supportPackRoot b8cce608c075fc6144dd68a0604005074f4d18429928d01b4341271d3e559a83 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.createEngineHost fe10e04c46c2c2597a3b8677d36994d30d58bb1b36667af7547cd1f191244ada -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.createScriptModuleSource c6cbc82744e7e6c46a03f824e04a64009236c33acb0bba93ffd557e8785e9db4 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.createSessionLogSink 1b62f7eb0b8f60f50bca0b0e3c226e6275134b08f29bbcd70409659a5d70499c -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.createHostLogSink e0cefa7e3ddd6a859e380dffab5f801bcfd641a53d183f68b254ed4e8a04bf26 -->
<!-- api: P:Inno.Editor.Hosting.EditorLaunchOptions.smokeFrameLimit ba9fc4a247c4982f72f7c31132b52d0c3b617fe7a8b6d0778d96aa52df051733 -->
<!-- api: M:Inno.Editor.Hosting.EditorLaunchOptions.#ctor a3ab6c277bc24e792116f7bd125739f56a08dc2cb3e5671300e632b3914ca678 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6da1e019bd1144d3"></a>[`public Inno.Editor.Hosting.EditorLaunchOptions.EditorLaunchOptions()`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L17) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-170ae73abd7cd2fa"></a>[`public required Inno.Adapter.Presentation.IAuthoringAdapterCatalog Inno.Editor.Hosting.EditorLaunchOptions.adapterCatalog { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L32) | Gets the borrowed immutable runtime, authoring and presentation provider catalogs. | — | — |
| <a id="api-1420b2d66b36ad67"></a>[`public required Inno.Build.Composition.BuildCompositionContext Inno.Editor.Hosting.EditorLaunchOptions.buildContext { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L62) | Gets the explicitly selected SDK and build execution host. | — | — |
| <a id="api-cbdaa13e65e6ced8"></a>[`public required System.Func<Inno.Runtime.EngineHost> Inno.Editor.Hosting.EditorLaunchOptions.createEngineHost { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L72) | Gets the factory transferring a newly created metadata-configured engine to the Editor owner. | — | — |
| <a id="api-aa962f56bdd0e036"></a>[`public System.Func<Inno.Core.Logging.ILogSink?>? Inno.Editor.Hosting.EditorLaunchOptions.createHostLogSink { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L87) | Gets an optional factory transferring one host log sink to the Editor lifetime. | — | — |
| <a id="api-cf613acc31313d59"></a>[`public required System.Func<Inno.Scripting.Compiler.ScriptModuleDeployment, Inno.Extensibility.Modules.IModuleSource> Inno.Editor.Hosting.EditorLaunchOptions.createScriptModuleSource { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L77) | Gets the factory transferring a candidate script module source to the existing reload transaction. | — | — |
| <a id="api-f1ccf3bf52ca5105"></a>[`public System.Func<Inno.Runtime.RuntimeSessionKind, Inno.Core.Logging.LogSessionId, Inno.Core.Logging.ILogSink>? Inno.Editor.Hosting.EditorLaunchOptions.createSessionLogSink { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L82) | Gets an optional factory transferring each edit or play log sink to its owning session. | — | — |
| <a id="api-7e95c28bc5a9139c"></a>[`public required Inno.Build.BuildTargetId Inno.Editor.Hosting.EditorLaunchOptions.defaultBuildTarget { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L57) | Gets the explicit default game target for new project build settings. | — | — |
| <a id="api-23b09222eab68717"></a>[`public required Inno.Build.Composition.BuildDistribution Inno.Editor.Hosting.EditorLaunchOptions.distribution { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L52) | Gets the borrowed immutable publication distribution. | — | — |
| <a id="api-a5e839f634c68d61"></a>[`public required Inno.Shell.IShellFrameDriver Inno.Editor.Hosting.EditorLaunchOptions.frameDriver { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L47) | Gets the borrowed owner-thread scheduling policy. | — | — |
| <a id="api-354603c534d553e4"></a>[`public required Inno.Editor.Core.EditorKeyboardPolicy Inno.Editor.Hosting.EditorLaunchOptions.keyboard { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L27) | Gets explicit keyboard conventions selected by the Editor product. | — | — |
| <a id="api-9cda60eef5d7e002"></a>[`public required Inno.Adapter.Presentation.PresentationBackendId Inno.Editor.Hosting.EditorLaunchOptions.presentation { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L42) | Gets the explicit presentation implementation selected by the product. | — | — |
| <a id="api-14d2d0c2a7a75287"></a>[`public required string Inno.Editor.Hosting.EditorLaunchOptions.projectDirectory { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L22) | Gets the current authoring project location; shared authoring services own its project IO. | — | — |
| <a id="api-9eb68534641c0804"></a>[`public required Inno.Shell.ShellOptions Inno.Editor.Hosting.EditorLaunchOptions.shell { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L37) | Gets explicit backend, primary-window and rendering policy selected by the product. | — | — |
| <a id="api-cd5812aaa8d39fbb"></a>[`public int? Inno.Editor.Hosting.EditorLaunchOptions.smokeFrameLimit { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L92) | Gets an optional positive frame limit for bounded product verification. | — | — |
| <a id="api-ca5b3ee2aa92d3ca"></a>[`public required string Inno.Editor.Hosting.EditorLaunchOptions.supportPackRoot { get; init; }`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L67) | Gets the explicit installed Support Pack location provided by product composition. | — | — |
| <a id="api-c9a90e0897b22d57"></a>[`public sealed class Inno.Editor.Hosting.EditorLaunchOptions`](../../src/composition/editor/hosting/Inno.Editor.Hosting/EditorLaunchOptions.cs#L17) | Supplies product-owned composition to the shared Editor lifecycle without selecting a platform or backend. | — | — |

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
