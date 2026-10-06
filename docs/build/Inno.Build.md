# Inno.Build

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Runtime](../runtime/Inno.Runtime.md)

## 职责与边界

Build bounded context 拥有 Game/Plugin 构建编排、组合 generation 检查、流式 Content Pack、Support Pack 验证、staging 和原子提交。它引用 authoring 服务，但绝不引用 Editor；平台布局由 `IGameBuildTarget` 提供。

Game Build 在内容打包前把已验证 Support Pack 作为目标运行时交给 Script Compiler。脚本仍先经过当前裁剪 API 校验，再针对 Pack 中实际部署的引擎程序集编译；因此部署兼容性不再由 Editor 进程加载顺序推导的全局指纹决定，也不会到 Player 启动后才以紫色错误画面暴露二进制不兼容。

## 公开 API

| API | 语义 |
| --- | --- |
| `BuildSettings`, `BuildSettingsStore` | 项目拥有的 Game/Plugin 导出默认值；使用当前 Inno Serialization 原子保存为 `Settings.Build.inno` |
| `BuildProfile`, `BuildProfileStore`, `BuildTargetId` | 一次 Game 构建所需的可验证 profile 与目标身份；`managedDeployment` 独立选择托管运行时，`Copy()` 创建隔离请求输入 |
| `GameBuildRequest`, `PluginBuildRequest` | 一次不可变构建请求 |
| `BuildProgress`, `BuildDiagnostic`, `BuildDiagnosticSeverity`, `BuildResult` | 进度、结构化诊断与最终结果 |
| `BuildPipeline` | Game/Plugin 的最小异步入口，并公开当前注册目标、adapter-selected 默认目标、显示名称查询与 `EnsurePlayerSupportPackAsync` 预备入口 |
| `IGameBuildTarget` | 可替换的平台目标，声明稳定 ID、显示名称、Host preference、runtimeIdentifier 和默认 managed deployment |
| `GameBuildContentContext`, `GameBuildPackageContext` | 隔离 staging context；Package context 提供已经校验的 `ManagedDeploymentResult`，packager 只组织平台布局 |
| `BuildPipeline.GetManagedDeployments(target)` | 返回所选平台可用的托管 provider ID，不通过封闭平台 switch 选择运行时 |
| `PlayerSupportPackCatalog` | 验证并解析不可变部署 closure；PublishAsync 发布完整候选并原子切换 current 索引 |
| `IPlayerSupportPackProvisioner` | Host 注入的当前源码与 SDK 异步准备边界；独立发行可不安装 SDK |

Content writer、`.iplugin` archive writer、snapshot fingerprint、script stage、staging transaction 与 player composer 全部 internal。

## 工作流

```csharp
// Complete preparation first, then start this snapshot on the authoring owner's thread.
await pipeline.EnsurePlayerSupportPackAsync(
    profile.target, profile.managedDeployment, cancellationToken);
BuildResult result = await pipeline.BuildGameAsync(
    new GameBuildRequest
    {
        profile = profile,
        outputDirectory = outputDirectory
    },
    progress,
    cancellationToken);
```

BuildPipeline 构造时注入独立 `ManagedDeploymentCatalog`。BuildGameAsync 首先复制 profile 并冻结输出路径，然后捕获 Assets/Plugins/Settings revision 与 Serialization generation。内容和逻辑代码闭包冻结后，生成静态注册组合、调用托管 compiler、验证输出身份与文件清单，最后交给平台 packager。任一代际变化、取消或 stage 失败都会清理 staging，不覆盖已提交产品。

`BuildProfile.Validate()` 只验证 target 是合法的 portable `BuildTargetId`，不维护 macOS/Windows 支持名单。
`BuildPipeline` 从 Composition Root 注册的 `IGameBuildTarget` 集合解析支持性：重复 ID、空显示名或多个
`isPreferredOnCurrentHost` 会在构造时失败；没有 adapter 声明 host preference 时，以稳定 ID 排序的第一个
target 作为默认值。Editor Export 与 Settings UI 枚举 `availableGameTargets`，已保存但未安装的 target 会保留
其 ID 并显示 `Missing (<id>)`，不会被静默改写。
`availableGameTargets` 是不可变集合视图，调用方不能通过数组或可写列表修改注册清单。

`Settings.Build.inno` 保存团队可版本控制的导出默认值；文件不存在时，composition root 以项目名、host target 和按路径排序的第一个已导入且可部署 Scene 建立隔离默认值，`~` authoring sample 中的 Scene 不会被自动选为 Startup Scene。Editor 的 Settings Apply 才会持久化该文件。每次打开导出 modal 都重新复制这些默认值，modal 内修改只属于本次请求，绝不回写 `Settings.Build.inno`。Game Application ID 与 Plugin ID 不是 Build 默认值，而是直接取 `Settings.Project.inno` 中的当前 Project ID；`BuildProfile` 仅保存 one-off 构建参数，加载后也会绑定当前 Project ID。

`BuildSettings.gamePersistentDataPath` 和 `BuildProfile.persistentDataPath` 指定 Player 在系统 Local Application Data 下使用的可移植相对子目录，空值采用 Application ID。它们允许发行游戏自行选择厂商/游戏目录，例如 `my-studio/flappybird`；路径段只能使用小写字母、数字、点、下划线和连字符，不能含 `.`、`..` 或绝对路径。Player manifest 在初始化引擎前帧定并核对这个目录，导出游戏不再在路径中加入 `InnoEngine`。

`Settings.Editor.inno`、`Settings.Project.inno` 和 `Settings.Build.inno` 不合并：它们分别属于本机 Editor 偏好、runtime 项目协议和 authoring/build 默认值。只有 `Settings.Project.inno` 进入 Player；Build Settings 和 Editor Settings 都不会进入 runtime closure。游戏内容的参考分辨率与保持比例策略属于项目 `GamePresentationSettings`，由 Game View 和 Player 共用。

## 错误与生命周期

调用者拥有 cancellation 和 progress；`BuildPipeline` 使用注入服务但不拥有其生命周期。源码工作区的 Editor/CLI 注入 [SourcePlayerSupportPackProvisioner](Inno.Build.SupportPacks.Core.md)：`EnsurePlayerSupportPackAsync` 在 owner-thread 构建前检查当前源码、SDK 和原生输入，并运行隔离准备与验证；已有损坏 Pack 明确失败。没有 provisioner 的发行 Host 使用已安装的不可变 Pack。
`BuildGameAsync` 要求 Pack 已准备，缺失时立即失败，不在资产快照阶段同步等待异步供给器。Editor 先异步准备，再在完成后的主线程帧启动构建，避免跨线程访问资产或冻结 UI。CLI 没有 UI 调度器，由宿主入口协调等待并保持资产 owner 线程，再启动异步构建。其他宿主同样必须在自己的 owner 安全点启动快照；普通 `await` 不自动保证切回原线程。

游戏目录安装复用 `AtomicDirectory.Install`；调用方协调两次目录移动期间同一目标的读写。
安装失败尝试恢复旧目录；提交后旧备份清理失败会抛出包含已安装目标与残留备份路径的 IO 异常，
保留完整新输出。不能把此类清理失败解释成新输出没有安装，也不能把两次移动当作无间隙的文件替换。
独立发行的 Editor 必须预装 Pack。输出不能位于 Assets、Plugins 或 Library。损坏 Artifact、无法供给的 Support Pack、无 runtime assembly、目标返回越界路径都会在提交前失败。Startup Scene 必须是已导入且可部署的 `SceneAsset`；位于任意 `~` 目录时会以 authoring-only 错误明确拒绝。

## 平台校验契约

`IPlayerSupportPackValidator.Validate(directory)` 由平台拥有，拒绝缺失/不兼容输入。IGameBuildTarget 继承该接口；`PlayerSupportPackCatalog.Resolve(target, validator)` 只做共有目录与部署输入约束，然后调用注册平台的 validator。Catalog 不维护封闭的平台名单，第三方目标不必修改 Core Build。Pack 中 PlayerLink 专门保存链接输入，最终游戏部署由平台 PackageAsync 决定。

`PlayerSupportPackCatalog.PublishAsync(target, stagingDirectory, validator, cancellationToken)` 是 Pack 的共同发布边界：
校验部署闭包，计算按规范化相对路径与文件 bytes 排序的 SHA-256，安装 `<target>/<fingerprint>`，
以 `AtomicFile` 替换 `<target>/current`。`Resolve` 只返回与索引和内容哈希一致的完整目录。
每次导出保留这个具体目录路径，后续发布不修改它；构建代码不跟随变化中的 current 索引。
Pack 是可重建的链接输入，CoreCLR/AOT 的最终部署仍由独立 managed compiler 选择。






## 本轮边界与所有权

ContentPackWriter 与 ContentPackReader 共用 ContentKey/Index 协议。冻结实际集合、长度与 SHA-256，写唯一 Content.index，固定排序与时间；catalog 指向唯一 Pack 身份。Build 核心只消费契约，内置组合属于 Inno.Build.Composition；失败和取消保持上次完整输出。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.BuildDiagnostic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildDiagnostic.BuildDiagnostic(Inno.Build.BuildDiagnosticSeverity severity, string code, string message)`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L46) | Creates an immutable build diagnostic. |
| [`string Inno.Build.BuildDiagnostic.code`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L66) | Gets the stable diagnostic code. |
| [`string Inno.Build.BuildDiagnostic.message`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L71) | Gets the human-readable explanation. |
| [`Inno.Build.BuildDiagnosticSeverity Inno.Build.BuildDiagnostic.severity`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L61) | Gets the impact of the diagnostic on build success. |
| [`Inno.Build.BuildDiagnostic`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L29) | Describes one stable machine-readable build problem or informational event. |

### `Inno.Build.BuildDiagnosticSeverity`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildDiagnosticSeverity.Error`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L23) | Reports a condition that prevented a valid build. |
| [`Inno.Build.BuildDiagnosticSeverity.Information`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L13) | Provides non-failing build context. |
| [`Inno.Build.BuildDiagnosticSeverity.Warning`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L18) | Reports a condition that did not prevent output creation. |
| [`Inno.Build.BuildDiagnosticSeverity`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L8) | Identifies the severity of one structured build diagnostic. |

### `Inno.Build.BuildPipeline`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildPipeline.BuildPipeline(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Plugins.Authoring.PluginEnvironment plugins, Inno.Core.Settings.ProjectSettingsStore settings, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Reload.GenerationCoordinator generations, Inno.Scripting.Compiler.ScriptCompiler compiler, string supportPackRoot, System.Collections.Generic.IEnumerable<Inno.Build.IGameBuildTarget> gameTargets, Inno.Build.Managed.ManagedDeploymentCatalog managedDeployments, Inno.Build.IPlayerSupportPackProvisioner? supportPackProvisioner = null)`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L70) | Creates a build pipeline from installed Player Support Packs and platform packagers. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.BuildResult> Inno.Build.BuildPipeline.BuildGameAsync(Inno.Build.GameBuildRequest request, System.IProgress<Inno.Build.BuildProgress>? progress = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L271) | Builds and installs one complete source-free game deployment with rollback protection. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.BuildResult> Inno.Build.BuildPipeline.BuildPluginAsync(Inno.Build.PluginBuildRequest request, System.IProgress<Inno.Build.BuildProgress>? progress = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L305) | Builds and atomically commits one deterministic project Plugin package. |
| [`System.Threading.Tasks.ValueTask<string> Inno.Build.BuildPipeline.EnsurePlayerSupportPackAsync(Inno.Build.BuildTargetId target, Inno.Build.Managed.ManagedDeploymentId? deployment, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L222) | Validates the platform and managed deployment before asynchronously preparing its Support Pack. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.ManagedDeploymentId> Inno.Build.BuildPipeline.GetManagedDeployments(Inno.Build.BuildTargetId target)`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L164) | Gets the managed deployment choices supported by a registered publication platform. |
| [`bool Inno.Build.BuildPipeline.TryGetGameTargetDisplayName(Inno.Build.BuildTargetId target, out string displayName)`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L181) | Tries to get the adapter-owned display name for a registered game target. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.BuildTargetId> Inno.Build.BuildPipeline.availableGameTargets`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L145) | Gets an immutable view of every registered build target in stable identity order. |
| [`Inno.Build.BuildTargetId Inno.Build.BuildPipeline.defaultGameTarget`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L150) | Gets the single adapter-selected target preferred for new build settings on this host. |
| [`Inno.Build.BuildPipeline`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L21) | Orchestrates deterministic Plugin and game builds over isolated staging and atomic commits. |

### `Inno.Build.BuildProfile`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildProfile Inno.Build.BuildProfile.Copy()`](../../build/pipeline/Inno.Build/BuildProfile.cs#L91) | Copies product and deployment settings before an asynchronous build retains the request. |
| [`void Inno.Build.BuildProfile.Validate()`](../../build/pipeline/Inno.Build/BuildProfile.cs#L109) | Validates product identity, startup content, target, and window dimensions. |
| [`string Inno.Build.BuildProfile.applicationId`](../../build/pipeline/Inno.Build/BuildProfile.cs#L27) | Gets or sets the stable lowercase identity used for persistent application data. |
| [`Inno.Build.Managed.ManagedDeploymentId? Inno.Build.BuildProfile.managedDeployment`](../../build/pipeline/Inno.Build/BuildProfile.cs#L61) | Gets or sets an explicit managed publisher; null uses the platform composition's default. |
| [`string Inno.Build.BuildProfile.persistentDataPath`](../../build/pipeline/Inno.Build/BuildProfile.cs#L40) | Gets or sets the writable data folder relative to the operating system's local application data directory. An empty value uses . |
| [`string Inno.Build.BuildProfile.productName`](../../build/pipeline/Inno.Build/BuildProfile.cs#L33) | Gets or sets the player-facing product and output name. |
| [`string Inno.Build.BuildProfile.startupScene`](../../build/pipeline/Inno.Build/BuildProfile.cs#L46) | Gets or sets the mount-qualified startup scene path. |
| [`Inno.Build.BuildTargetId Inno.Build.BuildProfile.target`](../../build/pipeline/Inno.Build/BuildProfile.cs#L52) | Gets or sets the platform target. |
| [`int Inno.Build.BuildProfile.windowHeight`](../../build/pipeline/Inno.Build/BuildProfile.cs#L79) | Gets or sets the initial logical window height. |
| [`int Inno.Build.BuildProfile.windowWidth`](../../build/pipeline/Inno.Build/BuildProfile.cs#L73) | Gets or sets the initial logical window width. |
| [`Inno.Build.BuildProfile`](../../build/pipeline/Inno.Build/BuildProfile.cs#L14) | Defines reusable product and startup settings for deterministic game builds. |

### `Inno.Build.BuildProfileStore`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildProfileStore.BuildProfileStore(string path, Inno.Core.Serialization.SerializationRegistry serialization)`](../../build/pipeline/Inno.Build/BuildProfileStore.cs#L36) | Creates a store for one current-format build profile document. |
| [`Inno.Build.BuildProfile Inno.Build.BuildProfileStore.Load()`](../../build/pipeline/Inno.Build/BuildProfileStore.cs#L63) | Loads and validates the current build profile document. |
| [`void Inno.Build.BuildProfileStore.Save(Inno.Build.BuildProfile profile)`](../../build/pipeline/Inno.Build/BuildProfileStore.cs#L99) | Validates and atomically saves the supplied build profile. |
| [`bool Inno.Build.BuildProfileStore.exists`](../../build/pipeline/Inno.Build/BuildProfileStore.cs#L49) | Gets whether the current project contains a build profile document. |
| [`Inno.Build.BuildProfileStore`](../../build/pipeline/Inno.Build/BuildProfileStore.cs#L16) | Persists one project's current game build profile through the engine serialization contract. |

### `Inno.Build.BuildProgress`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildProgress.BuildProgress(string stage, double fraction, string message)`](../../build/pipeline/Inno.Build/BuildProgress.cs#L28) | Creates a validated progress update. |
| [`double Inno.Build.BuildProgress.fraction`](../../build/pipeline/Inno.Build/BuildProgress.cs#L52) | Gets the completed fraction in the inclusive range from zero to one. |
| [`string Inno.Build.BuildProgress.message`](../../build/pipeline/Inno.Build/BuildProgress.cs#L57) | Gets a concise human-readable status message. |
| [`string Inno.Build.BuildProgress.stage`](../../build/pipeline/Inno.Build/BuildProgress.cs#L47) | Gets the stable build stage name. |
| [`Inno.Build.BuildProgress`](../../build/pipeline/Inno.Build/BuildProgress.cs#L8) | Reports monotonic progress for one asynchronous build operation. |

### `Inno.Build.BuildResult`

| 当前声明 | 行为 |
| --- | --- |
| [`int Inno.Build.BuildResult.artifactBundleCount`](../../build/pipeline/Inno.Build/BuildResult.cs#L61) | Gets the number of content-addressed artifact bundles represented by the output. |
| [`int Inno.Build.BuildResult.assetCount`](../../build/pipeline/Inno.Build/BuildResult.cs#L56) | Gets the number of deployed runtime assets or packaged source assets. |
| [`string? Inno.Build.BuildResult.contentHash`](../../build/pipeline/Inno.Build/BuildResult.cs#L51) | Gets the deterministic content identity, or when the build failed. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.BuildDiagnostic> Inno.Build.BuildResult.diagnostics`](../../build/pipeline/Inno.Build/BuildResult.cs#L76) | Gets structured diagnostics emitted during the build. |
| [`int Inno.Build.BuildResult.embeddedPluginCount`](../../build/pipeline/Inno.Build/BuildResult.cs#L71) | Gets the number of embedded dependency Plugin packages. |
| [`string? Inno.Build.BuildResult.outputPath`](../../build/pipeline/Inno.Build/BuildResult.cs#L41) | Gets the atomically committed output path, or when the build failed. |
| [`int Inno.Build.BuildResult.runtimeAssemblyCount`](../../build/pipeline/Inno.Build/BuildResult.cs#L66) | Gets the number of deployed runtime assemblies. |
| [`bool Inno.Build.BuildResult.succeeded`](../../build/pipeline/Inno.Build/BuildResult.cs#L36) | Gets whether the build committed a complete output. |
| [`Inno.Build.BuildTargetId? Inno.Build.BuildResult.target`](../../build/pipeline/Inno.Build/BuildResult.cs#L46) | Gets the game target, or for a Plugin package. |
| [`Inno.Build.BuildResult`](../../build/pipeline/Inno.Build/BuildResult.cs#L9) | Reports the immutable outcome, diagnostics, and optional durable output of one build. |

### `Inno.Build.BuildSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildSettings Inno.Build.BuildSettings.Copy()`](../../build/pipeline/Inno.Build/BuildSettings.cs#L165) | Creates an isolated copy suitable for a temporary export draft. |
| [`static Inno.Build.BuildSettings Inno.Build.BuildSettings.CreateDefault(string projectName, string startupScene, Inno.Build.BuildTargetId target)`](../../build/pipeline/Inno.Build/BuildSettings.cs#L113) | Creates canonical defaults for a new project. |
| [`Inno.Build.BuildProfile Inno.Build.BuildSettings.CreateGameProfile(Inno.Core.Settings.ProjectId projectId)`](../../build/pipeline/Inno.Build/BuildSettings.cs#L146) | Creates an isolated game profile from the current defaults. |
| [`Inno.Build.Managed.ManagedDeploymentId? Inno.Build.BuildSettings.gameManagedDeployment`](../../build/pipeline/Inno.Build/BuildSettings.cs#L86) | Gets or sets the default managed publisher; null follows the selected platform's default. |
| [`string Inno.Build.BuildSettings.gameOutputDirectory`](../../build/pipeline/Inno.Build/BuildSettings.cs#L56) | Gets or sets the default game output directory, relative to the project root or absolute. |
| [`string Inno.Build.BuildSettings.gamePersistentDataPath`](../../build/pipeline/Inno.Build/BuildSettings.cs#L44) | Gets or sets the default writable data folder below the operating system's local application data directory. An empty value uses the project ID. |
| [`string Inno.Build.BuildSettings.gameProductName`](../../build/pipeline/Inno.Build/BuildSettings.cs#L37) | Gets or sets the default player-facing product name. |
| [`string Inno.Build.BuildSettings.gameStartupScene`](../../build/pipeline/Inno.Build/BuildSettings.cs#L50) | Gets or sets the default mount-qualified startup Scene path. |
| [`Inno.Build.BuildTargetId Inno.Build.BuildSettings.gameTarget`](../../build/pipeline/Inno.Build/BuildSettings.cs#L74) | Gets or sets the default game build target. |
| [`int Inno.Build.BuildSettings.gameWindowHeight`](../../build/pipeline/Inno.Build/BuildSettings.cs#L68) | Gets or sets the default initial logical window height. |
| [`int Inno.Build.BuildSettings.gameWindowWidth`](../../build/pipeline/Inno.Build/BuildSettings.cs#L62) | Gets or sets the default initial logical window width. |
| [`bool Inno.Build.BuildSettings.includePluginDependencies`](../../build/pipeline/Inno.Build/BuildSettings.cs#L31) | Gets or sets whether Plugin dependency packages are embedded by default. |
| [`string Inno.Build.BuildSettings.pluginDisplayName`](../../build/pipeline/Inno.Build/BuildSettings.cs#L19) | Gets or sets the default user-facing Plugin name. |
| [`string Inno.Build.BuildSettings.pluginOutputPath`](../../build/pipeline/Inno.Build/BuildSettings.cs#L25) | Gets or sets the default Plugin package destination, relative to the project root or absolute. |
| [`Inno.Build.BuildSettings`](../../build/pipeline/Inno.Build/BuildSettings.cs#L13) | Defines project-owned defaults copied into each temporary game or Plugin export request. |

### `Inno.Build.BuildSettingsStore`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildSettingsStore.BuildSettingsStore(string path, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Build.BuildSettings defaultSettings)`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L37) | Creates a store for one project's current build settings document. |
| [`byte[] Inno.Build.BuildSettingsStore.CaptureDocument()`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L122) | Captures the current effective build defaults as a native document. |
| [`Inno.Build.BuildSettings Inno.Build.BuildSettingsStore.Load()`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L87) | Loads the saved settings or returns an isolated copy of the canonical defaults. |
| [`void Inno.Build.BuildSettingsStore.RestoreDocument(System.ReadOnlySpan<byte> document)`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L140) | Validates and atomically restores a native build settings document. |
| [`void Inno.Build.BuildSettingsStore.Save(Inno.Build.BuildSettings settings)`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L108) | Atomically replaces the project-owned build defaults. |
| [`void Inno.Build.BuildSettingsStore.ValidateDocument(System.ReadOnlySpan<byte> document)`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L156) | Validates a native build settings document without changing the active file. |
| [`Inno.Build.BuildSettings Inno.Build.BuildSettingsStore.defaultSettings`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L69) | Gets an isolated copy of the canonical project defaults. |
| [`bool Inno.Build.BuildSettingsStore.exists`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L57) | Gets whether a project-owned build settings document exists. |
| [`Inno.Build.BuildSettingsStore`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L13) | Persists project-owned export defaults independently from temporary export requests. |

### `Inno.Build.BuildTargetId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildTargetId.BuildTargetId(string value)`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L34) | Creates a stable target identity. |
| [`override string Inno.Build.BuildTargetId.ToString()`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L56) | Formats the portable target identity. |
| [`static Inno.Build.BuildTargetId Inno.Build.BuildTargetId.browserWasm`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L23) | Identifies a browser Player using WebAssembly and WebGL 2. |
| [`static Inno.Build.BuildTargetId Inno.Build.BuildTargetId.macOSArm64`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L13) | Identifies a native Apple-silicon macOS Player. |
| [`string Inno.Build.BuildTargetId.value`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L48) | Gets the portable target identity. |
| [`static Inno.Build.BuildTargetId Inno.Build.BuildTargetId.windowsX64`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L18) | Identifies a native 64-bit Windows Player. |
| [`Inno.Build.BuildTargetId`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L8) | Identifies one stable platform and architecture combination supported by the game build pipeline. |

### `Inno.Build.GameBuildContentContext`

| 当前声明 | 行为 |
| --- | --- |
| [`string Inno.Build.GameBuildContentContext.outputDirectory`](../../build/pipeline/Inno.Build/Game/GameBuildContentContext.cs#L29) | Gets the empty staging directory that receives source-free target artifacts. |
| [`Inno.Build.BuildProfile Inno.Build.GameBuildContentContext.profile`](../../build/pipeline/Inno.Build/Game/GameBuildContentContext.cs#L24) | Gets the validated product and target profile. |
| [`Inno.Core.Serialization.SerializationGeneration Inno.Build.GameBuildContentContext.serialization`](../../build/pipeline/Inno.Build/Game/GameBuildContentContext.cs#L34) | Gets the immutable converter generation pinned by the surrounding build transaction. |
| [`Inno.Build.GameBuildContentContext`](../../build/pipeline/Inno.Build/Game/GameBuildContentContext.cs#L9) | Supplies isolated staging and profile data for target-specific offline artifact generation. |

### `Inno.Build.GameBuildPackageContext`

| 当前声明 | 行为 |
| --- | --- |
| [`string Inno.Build.GameBuildPackageContext.contentDirectory`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L37) | Gets the source-free packaged content directory to deploy. |
| [`Inno.Build.Managed.ManagedDeploymentResult Inno.Build.GameBuildPackageContext.managedDeployment`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L42) | Gets the verified managed publication produced independently of platform packaging. |
| [`string Inno.Build.GameBuildPackageContext.outputDirectory`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L47) | Gets the empty staging parent where the target must create exactly one output. |
| [`Inno.Build.BuildProfile Inno.Build.GameBuildPackageContext.profile`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L27) | Gets the validated product profile. |
| [`string Inno.Build.GameBuildPackageContext.supportPackDirectory`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L32) | Gets the verified read-only Support Pack directory for this target. |
| [`Inno.Build.GameBuildPackageContext`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L8) | Supplies an isolated staging layout to a replaceable platform packager. |

### `Inno.Build.GameBuildRequest`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Build.GameBuildRequest.Validate()`](../../build/pipeline/Inno.Build/GameBuildRequest.cs#L27) | Validates destination and product inputs without mutating output. |
| [`required string Inno.Build.GameBuildRequest.outputDirectory`](../../build/pipeline/Inno.Build/GameBuildRequest.cs#L19) | Gets or initializes the parent directory that receives the atomically committed platform output. |
| [`required Inno.Build.BuildProfile Inno.Build.GameBuildRequest.profile`](../../build/pipeline/Inno.Build/GameBuildRequest.cs#L14) | Gets or initializes the reusable product profile. |
| [`Inno.Build.GameBuildRequest`](../../build/pipeline/Inno.Build/GameBuildRequest.cs#L9) | Requests one game build from the current validated authoring generation. |

### `Inno.Build.IGameBuildTarget`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Threading.Tasks.ValueTask Inno.Build.IGameBuildTarget.BuildContentAsync(Inno.Build.GameBuildContentContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L52) | Produces every target-specific runtime artifact required by this platform. |
| [`System.Threading.Tasks.ValueTask<string> Inno.Build.IGameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L72) | Composes one platform output from a verified Support Pack and source-free content directory. |
| [`Inno.Build.Managed.ManagedDeploymentId Inno.Build.IGameBuildTarget.defaultManagedDeployment`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L20) | Gets the managed publisher selected when the profile does not specify an override. |
| [`string Inno.Build.IGameBuildTarget.displayName`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L30) | Gets the user-facing target name presented by authoring hosts. |
| [`Inno.Build.BuildTargetId Inno.Build.IGameBuildTarget.id`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L15) | Gets the stable target identity implemented by this packager. |
| [`bool Inno.Build.IGameBuildTarget.isPreferredOnCurrentHost`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L35) | Gets whether this target is the adapter's preferred default on the current host. |
| [`string Inno.Build.IGameBuildTarget.runtimeIdentifier`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L25) | Gets the managed toolchain target identifier required by this platform composition. |
| [`Inno.Build.IGameBuildTarget`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L10) | Defines the replaceable platform packaging boundary for one game build target. |

### `Inno.Build.IPlayerSupportPackProvisioner`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Threading.Tasks.ValueTask Inno.Build.IPlayerSupportPackProvisioner.ProvisionAsync(Inno.Build.BuildTargetId target, string supportPackRoot, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/Player/IPlayerSupportPackProvisioner.cs#L26) | Checks current source inputs and selects a complete immutable pack before the build resumes. |
| [`Inno.Build.IPlayerSupportPackProvisioner`](../../build/pipeline/Inno.Build/Player/IPlayerSupportPackProvisioner.cs#L9) | Prepares current Player Support Pack inputs without coupling game builds to an SDK or platform toolchain. |

### `Inno.Build.IPlayerSupportPackValidator`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Build.IPlayerSupportPackValidator.Validate(string directory)`](../../build/pipeline/Inno.Build/Player/IPlayerSupportPackValidator.cs#L17) | Rejects an incomplete or incompatible platform closure. |
| [`Inno.Build.IPlayerSupportPackValidator`](../../build/pipeline/Inno.Build/Player/IPlayerSupportPackValidator.cs#L6) | Validates platform-owned runtime or link inputs without coupling the pack catalog to target names. |

### `Inno.Build.PlayerSupportPackCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.PlayerSupportPackCatalog.PlayerSupportPackCatalog(string root)`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackCatalog.cs#L49) | Creates a catalog rooted at the directory containing target-specific Player Support Packs. |
| [`System.Threading.Tasks.ValueTask<string> Inno.Build.PlayerSupportPackCatalog.PublishAsync(Inno.Build.BuildTargetId target, string stagingDirectory, Inno.Build.IPlayerSupportPackValidator validator, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackCatalog.cs#L124) | Validates and publishes an immutable pack, then atomically selects it for new readers. |
| [`string Inno.Build.PlayerSupportPackCatalog.Resolve(Inno.Build.BuildTargetId target, Inno.Build.IPlayerSupportPackValidator validator)`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackCatalog.cs#L73) | Resolves one installed Support Pack after validating its deployment-only closure. |
| [`Inno.Build.PlayerSupportPackCatalog`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackCatalog.cs#L17) | Resolves and validates installed Player Support Packs without loading authoring services. |

### `Inno.Build.PluginBuildRequest`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Build.PluginBuildRequest.Validate()`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L45) | Validates package identity and destination syntax. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.PluginBuildRequest.dependencies`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L37) | Gets or initializes the installed plugins this package actually requires. Their transitive dependencies are included in the package manifest. |
| [`required string Inno.Build.PluginBuildRequest.displayName`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L21) | Gets or initializes the user-facing Plugin name. |
| [`bool Inno.Build.PluginBuildRequest.includeDependencies`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L31) | Gets or initializes whether the complete transitive dependency closure is embedded. |
| [`required string Inno.Build.PluginBuildRequest.outputPath`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L26) | Gets or initializes the destination .iplugin package path. |
| [`required string Inno.Build.PluginBuildRequest.pluginId`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L16) | Gets or initializes the globally stable lowercase Plugin identity. |
| [`Inno.Build.PluginBuildRequest`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L11) | Requests one deterministic project-to-Plugin package without a companion authoring asset. |

## 项目依赖

- [Inno.Content](../assets/Inno.Content.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Serialization.Generators](../core/Inno.Core.Serialization.Generators.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Assets](../assets/Inno.Assets.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Runtime](../runtime/Inno.Runtime.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Scene.Assets](../scene/Inno.Scene.Assets.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Scripting.Compiler](../scripting/Inno.Scripting.Compiler.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Plugins](../plugins/Inno.Plugins.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Build.Managed](Inno.Build.Managed.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
