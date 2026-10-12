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
| `BuildPipeline` | Game/Plugin 的最小异步入口，并公开当前注册目标、显示名称查询与 `EnsurePlayerSupportPackAsync` 预备入口 |
| `IGameBuildTarget` | 可替换的平台打包目标，声明稳定 ID、显示名称、runtimeIdentifier 和默认 managed deployment；内容编译器独立绑定 |
| `GameBuildContentContext`, `GameBuildPackageContext` | 隔离 staging context；Package context 提供已经校验的 `ManagedDeploymentResult`，packager 只组织平台布局 |
| `BuildPipeline.GetManagedDeployments(target)` | 返回所选平台可用的托管 provider ID，不通过封闭平台 switch 选择运行时 |
| `PlayerSupportPackCatalog` | 验证并解析不可变部署 closure；PublishAsync 发布完整候选并原子切换 current 索引 |
| `IPlayerSupportPackProvisioner` | Host 注入的当前源码与 SDK 异步准备边界；独立发行可不安装 SDK |

Content writer、`.iplugin` archive writer、snapshot fingerprint、script stage、staging transaction 与 player composer 全部 internal。

## 工作流

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;

static ValueTask<PlayerSupportPackLease> PrepareAsync(
    BuildPipeline pipeline,
    BuildProfile profile,
    CancellationToken cancellationToken
) => pipeline.EnsurePlayerSupportPackAsync(
    profile.target, profile.managedDeployment, cancellationToken);

static ValueTask<BuildResult> StartOnAuthoringThread(
    BuildPipeline pipeline,
    BuildProfile profile,
    string outputDirectory,
    IProgress<BuildProgress>? progress,
    CancellationToken cancellationToken
) {
    return pipeline.BuildGameAsync(new GameBuildRequest
    {
        profile = profile,
        outputDirectory = outputDirectory
    }, progress, cancellationToken);
}
```

宿主先等待 `PrepareAsync`，保存返回的 `PlayerSupportPackLease`，再在创作 owner 线程调用 `StartOnAuthoringThread`。消费结束后释放 lease。两个步骤由宿主自己的帧/线程调度协调；普通 `await` 不自动切回 owner 线程。

BuildPipeline 构造时注入独立 `ManagedDeploymentCatalog`。BuildGameAsync 首先复制 profile 并冻结输出路径，然后捕获 Assets/Plugins/Settings revision 与 Serialization generation。内容和逻辑代码闭包冻结后，生成静态注册组合、调用托管 compiler、验证输出身份与文件清单，最后交给平台 packager。任一代际变化、取消或 stage 失败都会清理 staging，不覆盖已提交产品。

`BuildProfile.Validate()` 只验证 target 是合法的 portable `BuildTargetId`，不维护 macOS/Windows 支持名单。
`BuildPipeline` 从 Composition Root 注册的 `GameBuildTargetBinding` 集合解析支持性：每项绑定平台 packager 与内容 compiler；重复 ID、空集合或空显示名会在构造时失败。明确目标由产品组合和构建设置提供，Pipeline 不探测当前 OS 来替换目标。Editor Export 与 Settings UI 枚举 `availableGameTargets`，已保存但未安装的 target 会保留
其 ID 并显示 `Missing (<id>)`，不会被静默改写。
`availableGameTargets` 是不可变集合视图，调用方不能通过数组或可写列表修改注册清单。

`Settings.Build.inno` 保存团队可版本控制的导出默认值；文件不存在时，composition root 以项目名、host target 和按路径排序的第一个已导入且可部署 Scene 建立隔离默认值，`~` authoring sample 中的 Scene 不会被自动选为 Startup Scene。Editor 的 Settings Apply 才会持久化该文件。每次打开导出 modal 都重新复制这些默认值，modal 内修改只属于本次请求，绝不回写 `Settings.Build.inno`。Game Application ID 与 Plugin ID 不是 Build 默认值，而是直接取 `Settings.Project.inno` 中的当前 Project ID；`BuildProfile` 仅保存 one-off 构建参数，加载后也会绑定当前 Project ID。

`BuildSettings.gamePersistentDataPath` 和 `BuildProfile.persistentDataPath` 指定 Player 在系统 Local Application Data 下使用的可移植相对子目录，空值采用 Application ID。它们允许发行游戏自行选择厂商/游戏目录，例如 `my-studio/my-game`；路径段只能使用小写字母、数字、点、下划线和连字符，不能含 `.`、`..` 或绝对路径。Player manifest 在初始化引擎前帧定并核对这个目录，导出游戏不再在路径中加入 `InnoEngine`。

`Settings.Editor.inno`、`Settings.Project.inno` 和 `Settings.Build.inno` 不合并：它们分别属于本机 Editor 偏好、runtime 项目协议和 authoring/build 默认值。只有 `Settings.Project.inno` 进入 Player；Build Settings 和 Editor Settings 都不会进入 runtime closure。游戏内容的参考分辨率与保持比例策略属于项目 `GamePresentationSettings`，由 Game View 和 Player 共用。

## 错误与生命周期

调用者拥有 cancellation 和 progress；`BuildPipeline` 使用注入服务但不拥有其生命周期。源码工作区的 Editor/CLI 注入 [SourcePlayerSupportPackProvisioner](Inno.Build.SupportPacks.Core.md)：`EnsurePlayerSupportPackAsync` 在 owner-thread 构建前检查当前源码、SDK 和原生输入，并运行隔离准备与验证；已有损坏 Pack 明确失败。没有 provisioner 的发行 Host 使用已安装的不可变 Pack。
`BuildGameAsync` 要求 Pack 已准备，缺失时立即失败，不在资产快照阶段同步等待异步供给器。Editor 先异步准备，再在完成后的主线程帧启动构建，避免跨线程访问资产或冻结 UI。CLI 没有 UI 调度器，由宿主入口协调等待并保持资产 owner 线程，再启动异步构建。其他宿主同样必须在自己的 owner 安全点启动快照；普通 `await` 不自动保证切回原线程。

游戏目录安装复用 `AtomicDirectory.Install`；调用方协调两次目录移动期间同一目标的读写。
安装失败尝试恢复旧目录；提交后旧备份清理失败会抛出包含已安装目标与残留备份路径的 IO 异常，
保留完整新输出。不能把此类清理失败解释成新输出没有安装，也不能把两次移动当作无间隙的文件替换。
独立发行的 Editor 必须预装 Pack。输出不能位于 Assets、Plugins 或 Library。损坏 Artifact、无法供给的 Support Pack、无 runtime assembly、目标返回越界路径都会在提交前失败。Startup Scene 必须是已导入且可部署的 `SceneAsset`；位于任意 `~` 目录时会以 authoring-only 错误明确拒绝。

## 平台校验契约

`IPlayerSupportPackValidator.Validate(directory)` 由平台拥有，拒绝缺失/不兼容输入。IGameBuildTarget 继承该接口；`PlayerSupportPackCatalog.Acquire(target, validator, cancellationToken)` 取得读取保护、校验目录内容并调用平台 validator，返回必须释放的 `PlayerSupportPackLease`。Catalog 不维护封闭的平台名单，第三方目标不必修改 Core Build。Pack 中 PlayerLink 专门保存链接输入，最终游戏部署由平台 PackageAsync 决定。

`PlayerSupportPackCatalog.PublishAsync(target, stagingDirectory, validator, cancellationToken)` 是 Pack 的共同发布边界：
校验部署闭包，计算按规范化相对路径与文件 bytes 排序的 SHA-256，安装 `<target>/<fingerprint>`，
以 `AtomicFile` 替换 `<target>/current`。`Acquire` 只返回与索引和内容哈希一致的完整目录及其读取保护。
每次导出保持该 lease 至消费结束；缓存修复等待 reader 退出，构建代码不跟随变化中的 current 索引。
Pack 是可重建的链接输入，CoreCLR/AOT 的最终部署仍由独立 managed compiler 选择。






## 本轮边界与所有权

ContentPackWriter 与 ContentPackReader 共用 ContentKey/Index 协议。冻结实际集合、长度与 SHA-256，写唯一 Content.index，固定排序与时间；catalog 指向唯一 Pack 身份。Build 核心只消费契约，内置组合属于 Inno.Build.Composition；失败和取消保持上次完整输出。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.BuildDiagnostic`

<!-- api: T:Inno.Build.BuildDiagnostic a2238b1ebf482c65340fafea0a69452d98dc4a9238c230fc4f446a94712f0fb4 -->
<!-- api: M:Inno.Build.BuildDiagnostic.#ctor(Inno.Build.BuildDiagnosticSeverity,System.String,System.String) c425ca94ef52df3e7d8b858ae0f8811140ca86108b2dceb70cfbf6d5026707bf -->
<!-- api: P:Inno.Build.BuildDiagnostic.severity e19cea421a5e9c082dcec607603faf95b56216a42d935a4bd05df928a85bcb9e -->
<!-- api: P:Inno.Build.BuildDiagnostic.code 4322757285143d2ab6d8feaeaa3b8f3d584030b8cde2644bba867c17d2a56a95 -->
<!-- api: P:Inno.Build.BuildDiagnostic.message 432f3345b5676b9f1000652ae7a6d55a9a6fee23d0f3e14ad198de3f0ca51d4c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7f5435e6c70c5bbb"></a>[`public Inno.Build.BuildDiagnostic.BuildDiagnostic(Inno.Build.BuildDiagnosticSeverity severity, string code, string message)`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L46) | Creates an immutable build diagnostic.<br>`severity`：The impact of the diagnostic on build success.<br>`code`：The stable diagnostic code.<br>`message`：The human-readable explanation. | — | `System.ArgumentException`：Thrown when code or message is empty. |
| <a id="api-c44689301c1ada4a"></a>[`public string Inno.Build.BuildDiagnostic.code { get; }`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L66) | Gets the stable diagnostic code. | — | — |
| <a id="api-6cede50506a85af4"></a>[`public string Inno.Build.BuildDiagnostic.message { get; }`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L71) | Gets the human-readable explanation. | — | — |
| <a id="api-4ccabb4c665aaebc"></a>[`public Inno.Build.BuildDiagnosticSeverity Inno.Build.BuildDiagnostic.severity { get; }`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L61) | Gets the impact of the diagnostic on build success. | — | — |
| <a id="api-612f22dbfa0296fc"></a>[`public sealed class Inno.Build.BuildDiagnostic`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L29) | Describes one stable machine-readable build problem or informational event. | — | — |

### `Inno.Build.BuildDiagnosticSeverity`

<!-- api: T:Inno.Build.BuildDiagnosticSeverity 93d5283e8ae2c94e2e19ab3e4a783497727966302148cf9c0b79a7287df52509 -->
<!-- api: F:Inno.Build.BuildDiagnosticSeverity.Information 1191cb4d0d9265723925493b900dc80072fb252782fc71bfedd16b1ace92c3ce -->
<!-- api: F:Inno.Build.BuildDiagnosticSeverity.Warning c3345a8887c9a4848a84f3d110c8bcf9292d7791ac49df12ad88a731ed3e4b63 -->
<!-- api: F:Inno.Build.BuildDiagnosticSeverity.Error 2f4be1d3321469be46021a3efa3a9d6891c8dd5f4a16c72ecc59caa4958779b0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-43d16074cf675712"></a>[`Inno.Build.BuildDiagnosticSeverity.Error = 2`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L23) | Reports a condition that prevented a valid build. | — | — |
| <a id="api-4e0481b3b1ceef79"></a>[`Inno.Build.BuildDiagnosticSeverity.Information = 0`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L13) | Provides non-failing build context. | — | — |
| <a id="api-13e01bfbe217035d"></a>[`Inno.Build.BuildDiagnosticSeverity.Warning = 1`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L18) | Reports a condition that did not prevent output creation. | — | — |
| <a id="api-fcf00cfcc1d282aa"></a>[`public enum Inno.Build.BuildDiagnosticSeverity : int`](../../build/pipeline/Inno.Build/BuildDiagnostic.cs#L8) | Identifies the severity of one structured build diagnostic. | — | — |

### `Inno.Build.BuildPipeline`

<!-- api: T:Inno.Build.BuildPipeline 633a9794fa14eeace4990a3d626a6dbeaff3eb9dbd8f2bf7a3ef687e17d274e7 -->
<!-- api: M:Inno.Build.BuildPipeline.#ctor(Inno.Assets.Pipeline.AssetPipeline,Inno.Plugins.Authoring.PluginEnvironment,Inno.Core.Settings.ProjectSettingsStore,Inno.Core.Serialization.SerializationRegistry,Inno.Extensibility.Reload.GenerationCoordinator,Inno.Scripting.Compiler.ScriptCompiler,System.String,System.Collections.Generic.IEnumerable{Inno.Build.GameBuildTargetBinding},Inno.Build.Managed.ManagedDeploymentCatalog,Inno.Build.IPlayerSupportPackProvisioner) 26beff3ac6692ba417e269979fc5c8d79fed97d3dd709e5f708741017f3e0223 -->
<!-- api: P:Inno.Build.BuildPipeline.availableGameTargets d97e63216b2af4f796c5a8abbae449e8c3eae423682fbc20fef8726fe5d2c75b -->
<!-- api: M:Inno.Build.BuildPipeline.GetManagedDeployments(Inno.Build.BuildTargetId) fbeefaad95dac1ba6fe02e67b4ee96b91d787f40e624615c3c48e0bb936d05e8 -->
<!-- api: M:Inno.Build.BuildPipeline.TryGetGameTargetDisplayName(Inno.Build.BuildTargetId,System.String@) 190c8647c1d2bbb7f32315fa9a50e8b28efa16035cfb18f060cec8b8880c402c -->
<!-- api: M:Inno.Build.BuildPipeline.EnsurePlayerSupportPackAsync(Inno.Build.BuildTargetId,System.Nullable{Inno.Build.Managed.ManagedDeploymentId},System.Threading.CancellationToken) 847f5de9af9c7117120dc9f11ea952c1071bc3bef29272a956a5b009faf20cfb -->
<!-- api: M:Inno.Build.BuildPipeline.BuildGameAsync(Inno.Build.GameBuildRequest,System.IProgress{Inno.Build.BuildProgress},System.Threading.CancellationToken) 13beab469bc45d9e4f5ff3606cc9e482749d83d8f6338f3479ed0ac1883f71d4 -->
<!-- api: M:Inno.Build.BuildPipeline.BuildPluginAsync(Inno.Build.PluginBuildRequest,System.IProgress{Inno.Build.BuildProgress},System.Threading.CancellationToken) 248fd443897e7867147cf83d38f079ef66a0d0c41a44daf27b832463d1337dd4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e7c2c598bfa8e71b"></a>[`public Inno.Build.BuildPipeline.BuildPipeline(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Plugins.Authoring.PluginEnvironment plugins, Inno.Core.Settings.ProjectSettingsStore settings, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Reload.GenerationCoordinator generations, Inno.Scripting.Compiler.ScriptCompiler compiler, string supportPackRoot, System.Collections.Generic.IEnumerable<Inno.Build.GameBuildTargetBinding> gameTargets, Inno.Build.Managed.ManagedDeploymentCatalog managedDeployments, Inno.Build.IPlayerSupportPackProvisioner? supportPackProvisioner = null)`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L68) | Creates a build pipeline from installed Player Support Packs and platform packagers.<br>`supportPackRoot`：The directory containing one child directory per Inno.Build.BuildTargetId.<br>`assets`：The active authoring asset pipeline captured by builds.<br>`generations`：The host-wide admission gate that remains pinned until the build snapshot is released.<br>`plugins`：The active Plugin environment captured by builds.<br>`settings`：The current project settings store captured by builds.<br>`serialization`：The serialization registry used to pin and encode the build snapshot.<br>`compiler`：The project compiler used to produce a fresh runtime-only assembly generation for each game build.<br>`gameTargets`：The complete set of replaceable platform package implementations available to this host.<br>`managedDeployments`：Explicit managed publishers selected independently of the platform packagers.<br>`supportPackProvisioner`：Optional build-time provider that checks current source and SDK inputs during explicit pack preparation. | — | `System.ArgumentException`：Thrown when the Support Pack root is empty, no target is provided, or a target identity is duplicated. |
| <a id="api-7b56b04e5b2b9066"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.BuildResult> Inno.Build.BuildPipeline.BuildGameAsync(Inno.Build.GameBuildRequest request, System.IProgress<Inno.Build.BuildProgress>? progress = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L253) | Builds and installs one complete source-free game deployment with rollback protection.<br>返回：The durable output identity and deployment metrics.<br>`request`：The exact profile, destination, and activated runtime compilation generation.<br>`progress`：Optional observer for monotonic stage progress.<br>`cancellationToken`：The token that cancels work before directory installation begins. | Prepare current packs with EnsurePlayerSupportPackAsync before invoking this method on the authoring owner's thread. The synchronous snapshot phase never waits for asynchronous provisioning. Coordinate readers and writers of the output directory during its two installation moves. Backup cleanup failures preserve the installed candidate and report its remaining backup. | `System.OperationCanceledException`：Thrown when cancellation is requested before commit.<br>`System.IO.DirectoryNotFoundException`：The target Support Pack is missing; prepare it before starting the owner-thread build snapshot.<br>`System.IO.InvalidDataException`：The target Support Pack is present but invalid.<br>`System.IO.IOException`：Output installation fails, or cleanup of the previous output's backup fails after commit.<br>`System.AggregateException`：Output installation and restoration both fail; the previous tree remains at the reported backup path. |
| <a id="api-ad19fc6953e46b2d"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.BuildResult> Inno.Build.BuildPipeline.BuildPluginAsync(Inno.Build.PluginBuildRequest request, System.IProgress<Inno.Build.BuildProgress>? progress = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L287) | Builds and atomically commits one deterministic project Plugin package.<br>返回：The durable package identity and source-content metrics.<br>`request`：The package identity, destination, and dependency embedding policy.<br>`progress`：Optional observer for monotonic stage progress.<br>`cancellationToken`：The token that cancels work before atomic commit. | — | `System.OperationCanceledException`：Thrown when cancellation is requested before commit. |
| <a id="api-5973bde23f769109"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.PlayerSupportPackLease> Inno.Build.BuildPipeline.EnsurePlayerSupportPackAsync(Inno.Build.BuildTargetId target, Inno.Build.Managed.ManagedDeploymentId? deployment, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L203) | Validates the platform and managed deployment before asynchronously preparing its Support Pack.<br>返回：Reading ownership of the verified Pack, to be disposed after starting its consuming build. No authoring asset database operations run during preparation.<br>`target`：The registered platform and architecture target to prepare.<br>`deployment`：The selected managed publisher, or null to use the platform's default deployment. Unsupported pairs fail before any provisioner or external tool starts.<br>`cancellationToken`：Cancellation before the Pack installation commits. | — | `System.NotSupportedException`：No game target is registered for the requested identity.<br>`System.InvalidOperationException`：The managed publisher is absent or cannot publish the selected platform's runtime identifier.<br>`System.IO.DirectoryNotFoundException`：The Pack is absent and this host has no provisioner.<br>`System.IO.InvalidDataException`：An installed or newly published Pack fails deployment validation. |
| <a id="api-b45189e517aceed2"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.ManagedDeploymentId> Inno.Build.BuildPipeline.GetManagedDeployments(Inno.Build.BuildTargetId target)`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L144) | Gets the managed deployment choices supported by a registered publication platform.<br>返回：Immutable deployment identities in stable order; an unknown target throws explicitly.<br>`target`：The registered publication target whose managed runtime requirements are queried. | — | `System.ArgumentException`：The publication target is not registered in this build composition. |
| <a id="api-3dc09fc3df68c7e9"></a>[`public bool Inno.Build.BuildPipeline.TryGetGameTargetDisplayName(Inno.Build.BuildTargetId target, out string displayName)`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L161) | Tries to get the adapter-owned display name for a registered game target.<br>返回：true when the target is registered; otherwise, false.<br>`target`：The stable target identity to query.<br>`displayName`：Receives the registered user-facing name, or an empty value when no target is registered. | — | — |
| <a id="api-7a060a08e23c9bf9"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Build.BuildTargetId> Inno.Build.BuildPipeline.availableGameTargets { get; }`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L130) | Gets an immutable view of every registered build target in stable identity order. | — | — |
| <a id="api-9ecaf1e1ff65f104"></a>[`public sealed class Inno.Build.BuildPipeline`](../../build/pipeline/Inno.Build/BuildPipeline.cs#L21) | Orchestrates deterministic Plugin and game builds over isolated staging and atomic commits. | — | — |

### `Inno.Build.BuildProfile`

<!-- api: T:Inno.Build.BuildProfile 2d90d1c2750801755f3c688dff3f340502d1811ba98ef74d31db861c17d92902 -->
<!-- api: P:Inno.Build.BuildProfile.applicationId 05b589b0746ebc42d47b03f3f59a6b3727f03321dd29bbdf1352ab212182d39a -->
<!-- api: P:Inno.Build.BuildProfile.productName e758921e8f67527dbc29b8d7f228784d02347121b846c53f2c595227f54d7c4c -->
<!-- api: P:Inno.Build.BuildProfile.persistentDataPath 6c109d72b7b2c2393af7e4b7df4bf6b07e771f611c5c0d2e29383c4567b8929b -->
<!-- api: P:Inno.Build.BuildProfile.startupScene cb4b6d394ee40f6d1fbd0654ee73fd53410bcfea62017cdb8ffb458c29b1b7d5 -->
<!-- api: P:Inno.Build.BuildProfile.target 49394cf42de8914382ce32463d884014925e967e10df038026882dd3ef0732bf -->
<!-- api: P:Inno.Build.BuildProfile.managedDeployment 046be3eea460dac7f3ae783eec7beb4881c1dee8b16e6823b24f29f9ef8f387e -->
<!-- api: P:Inno.Build.BuildProfile.windowWidth a3d824c19c027c7bfd963da083e2d1c646445682cb1c333de999e0867e462d50 -->
<!-- api: P:Inno.Build.BuildProfile.windowHeight 553ad1e956787d15c2c4c563eb0454e2f9d0e51a89b17bd7c52a61ca58a6e98d -->
<!-- api: M:Inno.Build.BuildProfile.Copy eb4341a4130476e77e05a4ba2ed97c6f04c7241498a6f2e5e00a818bf2204bd9 -->
<!-- api: M:Inno.Build.BuildProfile.Validate beee6931edb36db457af7aa324734f7d36123b63585dce24a409c986cee8cea0 -->
<!-- api: M:Inno.Build.BuildProfile.#ctor 71a827bcef25a291763256121a966732957ccb71a68ede63891f2ad2c1bbc3ba -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-acb1f0875618aadf"></a>[`public Inno.Build.BuildProfile.BuildProfile()`](../../build/pipeline/Inno.Build/BuildProfile.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-68a22374f3b8d51c"></a>[`public Inno.Build.BuildProfile Inno.Build.BuildProfile.Copy()`](../../build/pipeline/Inno.Build/BuildProfile.cs#L91) | Copies product and deployment settings before an asynchronous build retains the request.<br>返回：A newly owned profile with the same current-format scalar settings and stable selections. | — | — |
| <a id="api-2e6c9aafd603bdf2"></a>[`public void Inno.Build.BuildProfile.Validate()`](../../build/pipeline/Inno.Build/BuildProfile.cs#L109) | Validates product identity, startup content, target, and window dimensions. | — | `System.IO.InvalidDataException`：Thrown when the profile cannot produce a portable Player deployment. |
| <a id="api-d9dd2399f665ba0f"></a>[`public string Inno.Build.BuildProfile.applicationId { get; set; }`](../../build/pipeline/Inno.Build/BuildProfile.cs#L28) | Gets or sets the stable lowercase identity used for persistent application data. | — | — |
| <a id="api-346622677e7e7a0f"></a>[`public Inno.Build.Managed.ManagedDeploymentId? Inno.Build.BuildProfile.managedDeployment { get; set; }`](../../build/pipeline/Inno.Build/BuildProfile.cs#L61) | Gets or sets an explicit managed publisher; null uses the platform composition's default. | — | — |
| <a id="api-3d22cd86e190ff8f"></a>[`public string Inno.Build.BuildProfile.persistentDataPath { get; set; }`](../../build/pipeline/Inno.Build/BuildProfile.cs#L41) | Gets or sets the writable data folder relative to the operating system's local application data directory. An empty value uses Inno.Build.BuildProfile.applicationId. | — | — |
| <a id="api-a8bc99a795c08de1"></a>[`public string Inno.Build.BuildProfile.productName { get; set; }`](../../build/pipeline/Inno.Build/BuildProfile.cs#L34) | Gets or sets the player-facing product and output name. | — | — |
| <a id="api-625ffe18d09ca2b8"></a>[`public string Inno.Build.BuildProfile.startupScene { get; set; }`](../../build/pipeline/Inno.Build/BuildProfile.cs#L47) | Gets or sets the mount-qualified startup scene path. | — | — |
| <a id="api-1ef31b4a355e2400"></a>[`public Inno.Build.BuildTargetId Inno.Build.BuildProfile.target { get; set; }`](../../build/pipeline/Inno.Build/BuildProfile.cs#L52) | Gets or sets the platform target. | — | — |
| <a id="api-4df0488e61f5d2d8"></a>[`public int Inno.Build.BuildProfile.windowHeight { get; set; }`](../../build/pipeline/Inno.Build/BuildProfile.cs#L80) | Gets or sets the initial logical window height. | — | — |
| <a id="api-119e3fc0c5ec143e"></a>[`public int Inno.Build.BuildProfile.windowWidth { get; set; }`](../../build/pipeline/Inno.Build/BuildProfile.cs#L74) | Gets or sets the initial logical window width. | — | — |
| <a id="api-bec857b0dcc96c8d"></a>[`public sealed class Inno.Build.BuildProfile : Inno.Core.Serialization.ISerializable`](../../build/pipeline/Inno.Build/BuildProfile.cs#L15) | Defines reusable product and startup settings for deterministic game builds. | — | — |

### `Inno.Build.BuildProfileStore`

<!-- api: T:Inno.Build.BuildProfileStore e7c04989e5beec822f9881221b33b64e80359d1f69dff08f350813a3150bb83b -->
<!-- api: M:Inno.Build.BuildProfileStore.#ctor(System.String,Inno.Core.Serialization.SerializationRegistry) cdf0a0f74084482bafe60becb077357eac0898df4e3c6f31a146e9d5d53b15f0 -->
<!-- api: P:Inno.Build.BuildProfileStore.exists 02e85ef0ef748d5f659c98bfea95d036d31b6b8b75044eefacba7af4cb2ead7c -->
<!-- api: M:Inno.Build.BuildProfileStore.Load 002040fdebbc717c2b0ee3d78f081433c91db89446533ec85275ec8aadcaabab -->
<!-- api: M:Inno.Build.BuildProfileStore.Save(Inno.Build.BuildProfile) 6cc366fcd716ab0b9bcee40f0c53606c32829380331bdb4a9c26ed32bd5954a9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d207c94b0fcc1823"></a>[`public Inno.Build.BuildProfileStore.BuildProfileStore(string path, Inno.Core.Serialization.SerializationRegistry serialization)`](../../build/pipeline/Inno.Build/BuildProfileStore.cs#L36) | Creates a store for one current-format build profile document.<br>`path`：The project-owned path of the build profile document.<br>`serialization`：The serialization registry whose active generation owns the generated converter. | — | `System.ArgumentException`：Thrown when path is empty.<br>`System.ArgumentNullException`：Thrown when serialization is null. |
| <a id="api-2a19c8cf235a43c9"></a>[`public Inno.Build.BuildProfile Inno.Build.BuildProfileStore.Load()`](../../build/pipeline/Inno.Build/BuildProfileStore.cs#L63) | Loads and validates the current build profile document.<br>返回：A newly owned build profile that callers may edit independently. | — | `System.IO.FileNotFoundException`：Thrown when the project does not contain a build profile document.<br>`System.IO.InvalidDataException`：Thrown when the document is corrupt or contains an invalid profile. |
| <a id="api-0d896fb82ddf8677"></a>[`public void Inno.Build.BuildProfileStore.Save(Inno.Build.BuildProfile profile)`](../../build/pipeline/Inno.Build/BuildProfileStore.cs#L99) | Validates and atomically saves the supplied build profile.<br>`profile`：The complete current profile to persist. | — | `System.ArgumentNullException`：Thrown when profile is null.<br>`System.IO.InvalidDataException`：Thrown when the profile cannot produce a supported Player deployment.<br>`System.IO.IOException`：Thrown when the candidate document cannot be written or atomically installed. |
| <a id="api-a87d2138a87ea198"></a>[`public bool Inno.Build.BuildProfileStore.exists { get; }`](../../build/pipeline/Inno.Build/BuildProfileStore.cs#L49) | Gets whether the current project contains a build profile document. | — | — |
| <a id="api-9ebaa157dd76d997"></a>[`public sealed class Inno.Build.BuildProfileStore`](../../build/pipeline/Inno.Build/BuildProfileStore.cs#L16) | Persists one project's current game build profile through the engine serialization contract. | The store writes a complete candidate beside the destination and replaces the active document only after serialization and validation succeed. | — |

### `Inno.Build.BuildProgress`

<!-- api: T:Inno.Build.BuildProgress c2fd68e75720b7dac89f255f657a27cd15466e478f45b9b88bd5e68bc6151f92 -->
<!-- api: M:Inno.Build.BuildProgress.#ctor(System.String,System.Double,System.String) b3bbccf3a66f5917e04b0f671aa68c7ff7388aba0b984eb7f2c3d7647327390f -->
<!-- api: P:Inno.Build.BuildProgress.stage bd20cc48f37a9314689a2f9e9bfcc8059f016fb333398d2f498230051c81f0ae -->
<!-- api: P:Inno.Build.BuildProgress.fraction e52db2e2be2e1120a6f8086fb09ac169f75d6ff88e04cfb1fcc18158bd19b893 -->
<!-- api: P:Inno.Build.BuildProgress.message aa2a8a89b1539fdf2c4bf98d34f1985b366ef54ca92cc25ae6e61d74e14aa8c4 -->
<!-- api: M:Inno.Build.BuildProgress.ToString 2ef930b4c2bec1864332d337171707b100b0fabc208bc16ec6ddc67d8e76218f -->
<!-- api: M:Inno.Build.BuildProgress.op_Inequality(Inno.Build.BuildProgress,Inno.Build.BuildProgress) 8e46a22a15508d2bbfb9e1eb64ca75ff4cb9c74f4ddc32aec8b5eb5d21b9f1bf -->
<!-- api: M:Inno.Build.BuildProgress.op_Equality(Inno.Build.BuildProgress,Inno.Build.BuildProgress) ded6a7460d166d2f2841423443aad2135b886177a9a08133855ba3b765e1b894 -->
<!-- api: M:Inno.Build.BuildProgress.GetHashCode 8237169447dfc241932ed0c583d9537a48745eb2db5b940a9efdd5c71d6ebe9c -->
<!-- api: M:Inno.Build.BuildProgress.Equals(System.Object) d7f1c7de4dc6ece136c69bef065d34c336bf8d4ab282c42e3672eece9d8529e7 -->
<!-- api: M:Inno.Build.BuildProgress.Equals(Inno.Build.BuildProgress) d3b4b25224058353f8266cc05683d05fae7a87b3d6d8eda025cafdf3385ec056 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ee31860a37c9022a"></a>[`public Inno.Build.BuildProgress.BuildProgress(string stage, double fraction, string message)`](../../build/pipeline/Inno.Build/BuildProgress.cs#L28) | Creates a validated progress update.<br>`stage`：The stable build stage name.<br>`fraction`：The completed fraction in the inclusive range from zero to one.<br>`message`：A concise human-readable status message. | — | `System.ArgumentException`：Thrown when stage or message is empty.<br>`System.ArgumentOutOfRangeException`：Thrown when fraction is outside the supported range. |
| <a id="api-7dc3ef1785ec23f3"></a>[`public bool Inno.Build.BuildProgress.Equals(Inno.Build.BuildProgress other)`](../../build/pipeline/Inno.Build/BuildProgress.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-a45adebd1eeddd46"></a>[`public override bool Inno.Build.BuildProgress.Equals(object obj)`](../../build/pipeline/Inno.Build/BuildProgress.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-e90150cae049b392"></a>[`public override int Inno.Build.BuildProgress.GetHashCode()`](../../build/pipeline/Inno.Build/BuildProgress.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-15fbb5c4fadeb5be"></a>[`public static bool Inno.Build.BuildProgress.operator ==(Inno.Build.BuildProgress left, Inno.Build.BuildProgress right)`](../../build/pipeline/Inno.Build/BuildProgress.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-4ae543d8dae01035"></a>[`public static bool Inno.Build.BuildProgress.operator !=(Inno.Build.BuildProgress left, Inno.Build.BuildProgress right)`](../../build/pipeline/Inno.Build/BuildProgress.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-969f96abc141f16e"></a>[`public override string Inno.Build.BuildProgress.ToString()`](../../build/pipeline/Inno.Build/BuildProgress.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-3beb0047b031076f"></a>[`public double Inno.Build.BuildProgress.fraction { get; }`](../../build/pipeline/Inno.Build/BuildProgress.cs#L52) | Gets the completed fraction in the inclusive range from zero to one. | — | — |
| <a id="api-0509a452ff304783"></a>[`public string Inno.Build.BuildProgress.message { get; }`](../../build/pipeline/Inno.Build/BuildProgress.cs#L57) | Gets a concise human-readable status message. | — | — |
| <a id="api-6e882afeaf2f6778"></a>[`public string Inno.Build.BuildProgress.stage { get; }`](../../build/pipeline/Inno.Build/BuildProgress.cs#L47) | Gets the stable build stage name. | — | — |
| <a id="api-ec371a3dbdfeea61"></a>[`public readonly record struct Inno.Build.BuildProgress : System.IEquatable<Inno.Build.BuildProgress>`](../../build/pipeline/Inno.Build/BuildProgress.cs#L8) | Reports monotonic progress for one asynchronous build operation. | — | — |

### `Inno.Build.BuildResult`

<!-- api: T:Inno.Build.BuildResult dd70536121ce32599e23ca5c14beb78d704b69eecd356aaae811e136c1ee21c8 -->
<!-- api: P:Inno.Build.BuildResult.succeeded 4ff4fbb4ef44d068e7b4ee9d41848eacaaac3b5ed9c99d3f6901ae53d0ca041c -->
<!-- api: P:Inno.Build.BuildResult.outputPath d5d1b976f46a9ea187558f395f219d52e40cafe5b46149347091252447bafe0f -->
<!-- api: P:Inno.Build.BuildResult.target 45efd01921696da5b744feae408870d8e96fbb9d34428f3d2fb8dbfd7accd6f9 -->
<!-- api: P:Inno.Build.BuildResult.contentHash d65b0d849ac3ebe81ee7f96699c8b2b50d05b1001ff9a4970fed95a173878713 -->
<!-- api: P:Inno.Build.BuildResult.assetCount 6b476a5d229c10fde5bf86ec9826cf3161aec8119f9036778903c1c609649f30 -->
<!-- api: P:Inno.Build.BuildResult.artifactBundleCount cb9c730f64c7b78795b94fd029788f1841e122a53722e30c1176a075e040c5ca -->
<!-- api: P:Inno.Build.BuildResult.runtimeAssemblyCount e7bc1d8c2fe712b68adf62c0ae9d08164fa670bdd04e985e6c20687c86b21fdd -->
<!-- api: P:Inno.Build.BuildResult.embeddedPluginCount f2f85cc00cdacfe16ab295cc1c0fcd8d0ebd3f9bce7a34ca84e9a4611451958b -->
<!-- api: P:Inno.Build.BuildResult.diagnostics 56f8d1a28aaa014716b6fbf3c9f93b7902cce5f22ddfa95d413c467f5b0352d7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c84fbd497497d411"></a>[`public int Inno.Build.BuildResult.artifactBundleCount { get; }`](../../build/pipeline/Inno.Build/BuildResult.cs#L61) | Gets the number of content-addressed artifact bundles represented by the output. | — | — |
| <a id="api-bc0394211ddf876f"></a>[`public int Inno.Build.BuildResult.assetCount { get; }`](../../build/pipeline/Inno.Build/BuildResult.cs#L56) | Gets the number of deployed runtime assets or packaged source assets. | — | — |
| <a id="api-78e9747d904e3e9a"></a>[`public string? Inno.Build.BuildResult.contentHash { get; }`](../../build/pipeline/Inno.Build/BuildResult.cs#L51) | Gets the deterministic content identity, or null when the build failed. | — | — |
| <a id="api-5ab8d2716d199435"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Build.BuildDiagnostic> Inno.Build.BuildResult.diagnostics { get; }`](../../build/pipeline/Inno.Build/BuildResult.cs#L76) | Gets structured diagnostics emitted during the build. | — | — |
| <a id="api-8f0bd8539464132e"></a>[`public int Inno.Build.BuildResult.embeddedPluginCount { get; }`](../../build/pipeline/Inno.Build/BuildResult.cs#L71) | Gets the number of embedded dependency Plugin packages. | — | — |
| <a id="api-8a0662ef753aa5c9"></a>[`public string? Inno.Build.BuildResult.outputPath { get; }`](../../build/pipeline/Inno.Build/BuildResult.cs#L41) | Gets the atomically committed output path, or null when the build failed. | — | — |
| <a id="api-4b630013ee9c2220"></a>[`public int Inno.Build.BuildResult.runtimeAssemblyCount { get; }`](../../build/pipeline/Inno.Build/BuildResult.cs#L66) | Gets the number of deployed runtime assemblies. | — | — |
| <a id="api-438494eec0823345"></a>[`public bool Inno.Build.BuildResult.succeeded { get; }`](../../build/pipeline/Inno.Build/BuildResult.cs#L36) | Gets whether the build committed a complete output. | — | — |
| <a id="api-397d8afea76f3071"></a>[`public Inno.Build.BuildTargetId? Inno.Build.BuildResult.target { get; }`](../../build/pipeline/Inno.Build/BuildResult.cs#L46) | Gets the game target, or null for a Plugin package. | — | — |
| <a id="api-fb2ad29b55151462"></a>[`public sealed class Inno.Build.BuildResult`](../../build/pipeline/Inno.Build/BuildResult.cs#L9) | Reports the immutable outcome, diagnostics, and optional durable output of one build. | — | — |

### `Inno.Build.BuildSettings`

<!-- api: T:Inno.Build.BuildSettings 18c1917711240ff91cf44308064a1cb61675a831f16290ed758afdd03c591040 -->
<!-- api: P:Inno.Build.BuildSettings.pluginDisplayName 21b4022105818f4fe396568faa78fc58a2ddbe8a70d50c52f1fad270e629cb76 -->
<!-- api: P:Inno.Build.BuildSettings.pluginOutputPath 7db2c2792582b16b28b955f90237d17545111eb257916259231e5057c5ee9b86 -->
<!-- api: P:Inno.Build.BuildSettings.includePluginDependencies 89bd83984ae8d6808e05c75115b4751d32d3a84c31b47a5ee44990c9b880f71d -->
<!-- api: P:Inno.Build.BuildSettings.gameProductName b9e4b7d3c3b11de0b4518dc1cdbd8aa3027f7fea4446a174083a979342a9198a -->
<!-- api: P:Inno.Build.BuildSettings.gamePersistentDataPath 1df50ee27b723991ed1503ede062c698e698dcf3780dda2d30f3cadd6cb81de6 -->
<!-- api: P:Inno.Build.BuildSettings.gameStartupScene 8194bf0a90a4c94cf01bfddd2834ad7a0ef58c13f68e2011d0324d4853b4351a -->
<!-- api: P:Inno.Build.BuildSettings.gameOutputDirectory 487d9b2ef0584cfb2d8a3c6bd37690dd7c4212dc53d0f921c61b8124904f5f33 -->
<!-- api: P:Inno.Build.BuildSettings.gameWindowWidth 0ce2f4c0ea12cf974b0d9e2ff8ab589ecf8f53b56446f63defd4b2189b0ec889 -->
<!-- api: P:Inno.Build.BuildSettings.gameWindowHeight 61e4851d14d9d0ae64581435fd5548e5d338523459ada90d12692662dc2deba6 -->
<!-- api: P:Inno.Build.BuildSettings.gameTarget e365d5ca89ee1ac16f5d40070587267b08a5f6a52dbffa3de52226e219b4720c -->
<!-- api: P:Inno.Build.BuildSettings.gameManagedDeployment dc9cdd7a4aef62b05109eae1c16bd58b2bd83e1447d64d1f6b5fe665cfb7f7b8 -->
<!-- api: M:Inno.Build.BuildSettings.CreateDefault(System.String,System.String,Inno.Build.BuildTargetId) 6317b92efed4e99d189bb05bc3eaa8e5d2c25abde925cd99d2d546563656686b -->
<!-- api: M:Inno.Build.BuildSettings.CreateGameProfile(Inno.Core.Settings.ProjectId) e906cba4c18356ad2d3038727aa1e7ea2fd6416a3e5e6194e53b5d43923a9471 -->
<!-- api: M:Inno.Build.BuildSettings.Copy 23d22bfc1cf73d328ce797378a754e81fc6e7891f8a8db6699db29bb52f7d440 -->
<!-- api: M:Inno.Build.BuildSettings.#ctor 3a3a861434a31a84630030e2c21eab20619b66a1b1f21c707b4c1af7ac21aaee -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e254f90d0eef0453"></a>[`public Inno.Build.BuildSettings.BuildSettings()`](../../build/pipeline/Inno.Build/BuildSettings.cs#L14) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-dac1bcebef4051f3"></a>[`public Inno.Build.BuildSettings Inno.Build.BuildSettings.Copy()`](../../build/pipeline/Inno.Build/BuildSettings.cs#L165) | Creates an isolated copy suitable for a temporary export draft.<br>返回：A newly owned settings object. | — | — |
| <a id="api-ab4fba55500f0140"></a>[`public static Inno.Build.BuildSettings Inno.Build.BuildSettings.CreateDefault(string projectName, string startupScene, Inno.Build.BuildTargetId target)`](../../build/pipeline/Inno.Build/BuildSettings.cs#L113) | Creates canonical defaults for a new project.<br>返回：A newly owned build settings object.<br>`projectName`：The project directory name used for initial labels and output paths.<br>`startupScene`：The initial mount-qualified startup Scene, or an empty value when unavailable.<br>`target`：The host-appropriate initial build target. | — | `System.ArgumentException`：Thrown when projectName is empty or target is invalid. |
| <a id="api-bfeb1a5a0186370c"></a>[`public Inno.Build.BuildProfile Inno.Build.BuildSettings.CreateGameProfile(Inno.Core.Settings.ProjectId projectId)`](../../build/pipeline/Inno.Build/BuildSettings.cs#L146) | Creates an isolated game profile from the current defaults.<br>返回：A newly owned profile that may be changed for one build without mutating these settings.<br>`projectId`：The authoritative project identity copied into the game profile. | — | — |
| <a id="api-76c0b29ddf84794c"></a>[`public Inno.Build.Managed.ManagedDeploymentId? Inno.Build.BuildSettings.gameManagedDeployment { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L86) | Gets or sets the default managed publisher; null follows the selected platform's default. | — | — |
| <a id="api-4173d0b38412ceca"></a>[`public string Inno.Build.BuildSettings.gameOutputDirectory { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L57) | Gets or sets the default game output directory, relative to the project root or absolute. | — | — |
| <a id="api-f05fa2bfe1ad9953"></a>[`public string Inno.Build.BuildSettings.gamePersistentDataPath { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L45) | Gets or sets the default writable data folder below the operating system's local application data directory. An empty value uses the project ID. | — | — |
| <a id="api-5803c5263aaadd68"></a>[`public string Inno.Build.BuildSettings.gameProductName { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L38) | Gets or sets the default player-facing product name. | — | — |
| <a id="api-48b11d1965da7ab5"></a>[`public string Inno.Build.BuildSettings.gameStartupScene { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L51) | Gets or sets the default mount-qualified startup Scene path. | — | — |
| <a id="api-18de8c5585bf788a"></a>[`public Inno.Build.BuildTargetId Inno.Build.BuildSettings.gameTarget { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L74) | Gets or sets the default game build target. | — | — |
| <a id="api-44751aae07bcee5b"></a>[`public int Inno.Build.BuildSettings.gameWindowHeight { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L69) | Gets or sets the default initial logical window height. | — | — |
| <a id="api-4646b7e5dd264847"></a>[`public int Inno.Build.BuildSettings.gameWindowWidth { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L63) | Gets or sets the default initial logical window width. | — | — |
| <a id="api-b0d05f50099b77b9"></a>[`public bool Inno.Build.BuildSettings.includePluginDependencies { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L32) | Gets or sets whether Plugin dependency packages are embedded by default. | — | — |
| <a id="api-b8226573b0ce338b"></a>[`public string Inno.Build.BuildSettings.pluginDisplayName { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L20) | Gets or sets the default user-facing Plugin name. | — | — |
| <a id="api-14219040e26c8e88"></a>[`public string Inno.Build.BuildSettings.pluginOutputPath { get; set; }`](../../build/pipeline/Inno.Build/BuildSettings.cs#L26) | Gets or sets the default Plugin package destination, relative to the project root or absolute. | — | — |
| <a id="api-0ebaf3c9612f9740"></a>[`public sealed class Inno.Build.BuildSettings : Inno.Core.Serialization.ISerializable`](../../build/pipeline/Inno.Build/BuildSettings.cs#L14) | Defines project-owned defaults copied into each temporary game or Plugin export request. | — | — |

### `Inno.Build.BuildSettingsStore`

<!-- api: T:Inno.Build.BuildSettingsStore 704c1166698ebc5bf419a20ca3c2ff9192d5b51eaf99bd709cce6f5db964ac1a -->
<!-- api: M:Inno.Build.BuildSettingsStore.#ctor(System.String,Inno.Core.Serialization.SerializationRegistry,Inno.Build.BuildSettings) 511ea56a50e56188da84f8fe3b07114e13e2f9a7dedca89ec3c964ae3062ce50 -->
<!-- api: P:Inno.Build.BuildSettingsStore.exists edd049517bdd20d5cd0c1d749692213d060cc1e2b30e5eaf5e2ee316583caac8 -->
<!-- api: P:Inno.Build.BuildSettingsStore.defaultSettings 0de44fdc532465706a08b24f6825c2155a0058fd70233613b1bce3dc7ff897e5 -->
<!-- api: M:Inno.Build.BuildSettingsStore.Load 1df765c23db032610e8b200b1262a8bc95a7c7b23d576d23046a6a539ad3b88a -->
<!-- api: M:Inno.Build.BuildSettingsStore.Save(Inno.Build.BuildSettings) d40b3428b563941136c036845e5b48b18d7627ba49b417ca4c0b82e65b36ccdb -->
<!-- api: M:Inno.Build.BuildSettingsStore.CaptureDocument d8c3afdd7e276c3dfe8bbf6c29a63f5491bc00e061f651d937efa3b5567215cb -->
<!-- api: M:Inno.Build.BuildSettingsStore.RestoreDocument(System.ReadOnlySpan{System.Byte}) 6d1118e4f273b87b26ab619a8dc6ab4ca6d8f22f7e0dd0785e1663b541414b09 -->
<!-- api: M:Inno.Build.BuildSettingsStore.ValidateDocument(System.ReadOnlySpan{System.Byte}) b406f6791d0ab8814474e14bbad4bec7feb0c0527ec3ea8bdce4f3d849f5bc35 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-66f652c6d554f4eb"></a>[`public Inno.Build.BuildSettingsStore.BuildSettingsStore(string path, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Build.BuildSettings defaultSettings)`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L37) | Creates a store for one project's current build settings document.<br>`path`：The project-owned Settings.Build.inno path.<br>`serialization`：The active engine serialization registry.<br>`defaultSettings`：Canonical project defaults used while no document exists. | — | `System.ArgumentException`：Thrown when path is empty.<br>`System.ArgumentNullException`：Thrown when serialization or defaultSettings is null. |
| <a id="api-17437a7080c999db"></a>[`public byte[] Inno.Build.BuildSettingsStore.CaptureDocument()`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L122) | Captures the current effective build defaults as a native document.<br>返回：A newly owned current-format document payload. | — | — |
| <a id="api-c1b91159cbff1433"></a>[`public Inno.Build.BuildSettings Inno.Build.BuildSettingsStore.Load()`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L87) | Loads the saved settings or returns an isolated copy of the canonical defaults.<br>返回：A newly owned settings object. | — | `System.IO.InvalidDataException`：Thrown when the saved document is malformed. |
| <a id="api-f5208248e391fe01"></a>[`public void Inno.Build.BuildSettingsStore.RestoreDocument(System.ReadOnlySpan<byte> document)`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L140) | Validates and atomically restores a native build settings document.<br>`document`：The complete document payload. | — | `System.IO.InvalidDataException`：Thrown when the document is malformed.<br>`System.IO.IOException`：Thrown when the document cannot be committed. |
| <a id="api-c1597ebc1f160e02"></a>[`public void Inno.Build.BuildSettingsStore.Save(Inno.Build.BuildSettings settings)`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L108) | Atomically replaces the project-owned build defaults.<br>`settings`：The complete defaults to persist. | — | `System.ArgumentNullException`：Thrown when settings is null.<br>`System.IO.InvalidDataException`：Thrown when the settings document is malformed.<br>`System.IO.IOException`：Thrown when the candidate cannot be committed. |
| <a id="api-f238675d651efa17"></a>[`public void Inno.Build.BuildSettingsStore.ValidateDocument(System.ReadOnlySpan<byte> document)`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L156) | Validates a native build settings document without changing the active file.<br>`document`：The document payload to inspect. | — | `System.IO.InvalidDataException`：Thrown when the document is malformed. |
| <a id="api-c51dad9fd24dc1ba"></a>[`public Inno.Build.BuildSettings Inno.Build.BuildSettingsStore.defaultSettings { get; }`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L69) | Gets an isolated copy of the canonical project defaults. | — | — |
| <a id="api-9917bae68863cd58"></a>[`public bool Inno.Build.BuildSettingsStore.exists { get; }`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L57) | Gets whether a project-owned build settings document exists. | — | — |
| <a id="api-373d844fdec060b6"></a>[`public sealed class Inno.Build.BuildSettingsStore`](../../build/pipeline/Inno.Build/BuildSettingsStore.cs#L13) | Persists project-owned export defaults independently from temporary export requests. | — | — |

### `Inno.Build.BuildTargetId`

<!-- api: T:Inno.Build.BuildTargetId d345f47a5166ccb2ce43117d52964ee74cd8ae7f2bfd8d4d5fd145808b8ba255 -->
<!-- api: P:Inno.Build.BuildTargetId.macOSArm64 a9329a7881be05b5eb08e1ad5227d77a8e2cb43f47070c2ef59bd23b8f04407c -->
<!-- api: P:Inno.Build.BuildTargetId.windowsX64 a7f48fe0e28b74c7b96627866eb4a0b88d28f0c9b3ad0ca626a2f7c53139b910 -->
<!-- api: P:Inno.Build.BuildTargetId.browserWasm 2d12a5d2db2c87dde3355e8b4790617bbef714773667a78a5d629ecb5b7ca246 -->
<!-- api: M:Inno.Build.BuildTargetId.#ctor(System.String) 6f8e223586a699f64eb708afa97491aaa6701b16af20542b23ba4a3bcfb6199c -->
<!-- api: P:Inno.Build.BuildTargetId.value b5aa307d967767592a7eca6cdb9d600cdb819113e631cebf2407619cc52570b2 -->
<!-- api: M:Inno.Build.BuildTargetId.ToString 6e9df431fcd728e544ff29682b7e74b87ee778b600bd320be197319f345a36a5 -->
<!-- api: M:Inno.Build.BuildTargetId.op_Inequality(Inno.Build.BuildTargetId,Inno.Build.BuildTargetId) 8adb83037b94ad7f0f661d06c5bf6376bd95e1934d1bed513375b58a9fd983f0 -->
<!-- api: M:Inno.Build.BuildTargetId.op_Equality(Inno.Build.BuildTargetId,Inno.Build.BuildTargetId) d9b5c62623de9eb551723c457ed45b74fb49616eee60e2ed47ad987cad11232c -->
<!-- api: M:Inno.Build.BuildTargetId.GetHashCode a1ce41ab3243fb0f1027550b813bf29e9cbf74597fd47bc493d0789f1b1c9294 -->
<!-- api: M:Inno.Build.BuildTargetId.Equals(System.Object) ff47f653fb9e95e251255ea6f7bbf8498163629fcf08fac3b55c83ccc01043af -->
<!-- api: M:Inno.Build.BuildTargetId.Equals(Inno.Build.BuildTargetId) 71fd5d0c64693d83172a6aa6cf05246662bc29d1362d7c3036e14dc7f737364d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-de498c980ec9970c"></a>[`public Inno.Build.BuildTargetId.BuildTargetId(string value)`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L34) | Creates a stable target identity.<br>`value`：A lowercase portable identifier composed of letters, numbers, and hyphens. | — | `System.ArgumentException`：Thrown when value is not a portable target identity. |
| <a id="api-dfd9a4532eb3a2d0"></a>[`public bool Inno.Build.BuildTargetId.Equals(Inno.Build.BuildTargetId other)`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-12d9a3a12b03e605"></a>[`public override bool Inno.Build.BuildTargetId.Equals(object obj)`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-f6095c5c11cf64fd"></a>[`public override int Inno.Build.BuildTargetId.GetHashCode()`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-fc99ce8fc4e4e745"></a>[`public static bool Inno.Build.BuildTargetId.operator ==(Inno.Build.BuildTargetId left, Inno.Build.BuildTargetId right)`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-5b3a2a5458737f1e"></a>[`public static bool Inno.Build.BuildTargetId.operator !=(Inno.Build.BuildTargetId left, Inno.Build.BuildTargetId right)`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c71dc3127ac279d5"></a>[`public override string Inno.Build.BuildTargetId.ToString()`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L56) | Formats the portable target identity.<br>返回：The exact stable target identity. | — | — |
| <a id="api-8802fd346b168037"></a>[`public static Inno.Build.BuildTargetId Inno.Build.BuildTargetId.browserWasm { get; }`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L23) | Identifies a browser Player using WebAssembly and WebGL 2. | — | — |
| <a id="api-63df90b81b906a57"></a>[`public static Inno.Build.BuildTargetId Inno.Build.BuildTargetId.macOSArm64 { get; }`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L13) | Identifies a native Apple-silicon macOS Player. | — | — |
| <a id="api-d5ae613ca8c8200c"></a>[`public string Inno.Build.BuildTargetId.value { get; }`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L48) | Gets the portable target identity. | — | — |
| <a id="api-d121f6c60dcdab02"></a>[`public static Inno.Build.BuildTargetId Inno.Build.BuildTargetId.windowsX64 { get; }`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L18) | Identifies a native 64-bit Windows Player. | — | — |
| <a id="api-6224264580047209"></a>[`public readonly record struct Inno.Build.BuildTargetId : System.IEquatable<Inno.Build.BuildTargetId>`](../../build/pipeline/Inno.Build/BuildTargetId.cs#L8) | Identifies one stable platform and architecture combination supported by the game build pipeline. | — | — |

### `Inno.Build.GameBuildContentContext`

<!-- api: T:Inno.Build.GameBuildContentContext 1b988a85321c2de2a59ca2b4b71dafed98a2dbb41cd57149d6d5b61e8703c279 -->
<!-- api: P:Inno.Build.GameBuildContentContext.profile 598e2915e79a7a8d7fba1e8aa7d8bb419121c656f4485769f7c8b222d18365f1 -->
<!-- api: P:Inno.Build.GameBuildContentContext.outputDirectory 1a98c0905f2017ebba257429cc06f8db017e52e8dcf05905a5ac1974edd685a8 -->
<!-- api: P:Inno.Build.GameBuildContentContext.serialization 824a8c1922646fde2ea3f8069d5875025786e2252268cb6f10251c8e8ac5d227 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4af1589354039879"></a>[`public string Inno.Build.GameBuildContentContext.outputDirectory { get; }`](../../build/pipeline/Inno.Build/Game/GameBuildContentContext.cs#L29) | Gets the empty staging directory that receives source-free target artifacts. | — | — |
| <a id="api-bbe891bde9795425"></a>[`public Inno.Build.BuildProfile Inno.Build.GameBuildContentContext.profile { get; }`](../../build/pipeline/Inno.Build/Game/GameBuildContentContext.cs#L24) | Gets the validated product and target profile. | — | — |
| <a id="api-ce33738d7f2ea673"></a>[`public Inno.Core.Serialization.SerializationGeneration Inno.Build.GameBuildContentContext.serialization { get; }`](../../build/pipeline/Inno.Build/Game/GameBuildContentContext.cs#L34) | Gets the immutable converter generation pinned by the surrounding build transaction. | — | — |
| <a id="api-1fa6413f61e74eca"></a>[`public sealed class Inno.Build.GameBuildContentContext`](../../build/pipeline/Inno.Build/Game/GameBuildContentContext.cs#L9) | Supplies isolated staging and profile data for target-specific offline artifact generation. | — | — |

### `Inno.Build.GameBuildPackageContext`

<!-- api: T:Inno.Build.GameBuildPackageContext f0e785beed09992ef837333b4876980afefd63c2f8d9f7862642904d3a38d9f3 -->
<!-- api: P:Inno.Build.GameBuildPackageContext.profile e92567e387e16f970367050b8589760da3976707bde8c20c2d190236a970e6e1 -->
<!-- api: P:Inno.Build.GameBuildPackageContext.supportPackDirectory 3b7e718a7e7bb01f9bb964f4f98e260460407f9b5246ffb0e6ffb6e69a3cbb77 -->
<!-- api: P:Inno.Build.GameBuildPackageContext.contentDirectory cfbe0bfdea4afeab97d90fba5b9f0844a08e2bbac41b370b2f188c9696a6a3e3 -->
<!-- api: P:Inno.Build.GameBuildPackageContext.managedDeployment f815ebcb3ad28fccfc7b3c9511a3dd135d9f7469a40938b7cd6cb2cbb7122faa -->
<!-- api: P:Inno.Build.GameBuildPackageContext.outputDirectory d3de75e45ae7ba77ef5ed4ca5559fed37b146e15d6d1848ecc5996d64db1f00d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0c83d12654ae761a"></a>[`public string Inno.Build.GameBuildPackageContext.contentDirectory { get; }`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L37) | Gets the source-free packaged content directory to deploy. | — | — |
| <a id="api-0bda9970854b612d"></a>[`public Inno.Build.Managed.ManagedDeploymentResult Inno.Build.GameBuildPackageContext.managedDeployment { get; }`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L42) | Gets the verified managed publication produced independently of platform packaging. | — | — |
| <a id="api-c82b9568a97ba70f"></a>[`public string Inno.Build.GameBuildPackageContext.outputDirectory { get; }`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L47) | Gets the empty staging parent where the target must create exactly one output. | — | — |
| <a id="api-72241322168c98ae"></a>[`public Inno.Build.BuildProfile Inno.Build.GameBuildPackageContext.profile { get; }`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L27) | Gets the validated product profile. | — | — |
| <a id="api-c01a31f80556a563"></a>[`public string Inno.Build.GameBuildPackageContext.supportPackDirectory { get; }`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L32) | Gets the verified read-only Support Pack directory for this target. | — | — |
| <a id="api-f7b1352fad491a22"></a>[`public sealed class Inno.Build.GameBuildPackageContext`](../../build/pipeline/Inno.Build/Game/GameBuildPackageContext.cs#L8) | Supplies an isolated staging layout to a replaceable platform packager. | — | — |

### `Inno.Build.GameBuildRequest`

<!-- api: T:Inno.Build.GameBuildRequest e2f038cd3115270676490aa226c8fe4b76ac1aae674a6ad1a27f3a195343bc86 -->
<!-- api: P:Inno.Build.GameBuildRequest.profile fad722c42fbd43180c9f590bca35015225ee8b5d34615eeec5399f9ebcd0b744 -->
<!-- api: P:Inno.Build.GameBuildRequest.outputDirectory 8fd55c7f4d2330095e0dcb0c3a55ed625a88df74c26ce011a5eb15c664c68759 -->
<!-- api: M:Inno.Build.GameBuildRequest.Validate aa2f4ea162e35d44c7ff310b06c0148ead03a6687aa758d1b99e118d9fb2764e -->
<!-- api: M:Inno.Build.GameBuildRequest.#ctor 993b654cbd2de53797f7f4c3f4225759e74997ab95f69c7de338af756f1a5fa6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-75189c8af56268d4"></a>[`public Inno.Build.GameBuildRequest.GameBuildRequest()`](../../build/pipeline/Inno.Build/GameBuildRequest.cs#L9) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-346e333bd49e0f3e"></a>[`public void Inno.Build.GameBuildRequest.Validate()`](../../build/pipeline/Inno.Build/GameBuildRequest.cs#L27) | Validates destination and product inputs without mutating output. | — | `System.IO.InvalidDataException`：Thrown when the request is incomplete. |
| <a id="api-b4531d9079a635aa"></a>[`public required string Inno.Build.GameBuildRequest.outputDirectory { get; init; }`](../../build/pipeline/Inno.Build/GameBuildRequest.cs#L19) | Gets or initializes the parent directory that receives the atomically committed platform output. | — | — |
| <a id="api-d501f43b4706a4de"></a>[`public required Inno.Build.BuildProfile Inno.Build.GameBuildRequest.profile { get; init; }`](../../build/pipeline/Inno.Build/GameBuildRequest.cs#L14) | Gets or initializes the reusable product profile. | — | — |
| <a id="api-b4f67d09390b1761"></a>[`public sealed class Inno.Build.GameBuildRequest`](../../build/pipeline/Inno.Build/GameBuildRequest.cs#L9) | Requests one game build from the current validated authoring generation. | — | — |

### `Inno.Build.GameBuildTargetBinding`

<!-- api: T:Inno.Build.GameBuildTargetBinding c0eea9db0f7c82b1c9753260546966b70dccad20039ea9de8c261886ebbdc369 -->
<!-- api: M:Inno.Build.GameBuildTargetBinding.#ctor(Inno.Build.IGameBuildTarget,Inno.Build.IGameContentCompiler) bebcc3f9c0a0db0d7152bc2d201d5ee0eeda886d7b370b65c6b708651078e75e -->
<!-- api: P:Inno.Build.GameBuildTargetBinding.packager 47c06ffd37563de5555b453dc231f4aacb707b9acbcc677728bdc96e8d31b829 -->
<!-- api: P:Inno.Build.GameBuildTargetBinding.compiler 33cadfbe3f178709bee52d36cdabb86044a1d093b8a71d3fdc77e0cb72408033 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-277682f347dbffdc"></a>[`public Inno.Build.GameBuildTargetBinding.GameBuildTargetBinding(Inno.Build.IGameBuildTarget packager, Inno.Build.IGameContentCompiler compiler)`](../../build/pipeline/Inno.Build/Game/GameBuildTargetBinding.cs#L25) | Validates a complete binding without starting tools or taking ownership of services.<br>`packager`：The borrowed platform packaging implementation.<br>`compiler`：The borrowed content implementation for the same target and authoring generation. | — | `System.ArgumentNullException`：Either implementation is null.<br>`System.ArgumentException`：A target is unassigned or the two implementation identities disagree. |
| <a id="api-79f4c5a40c626734"></a>[`public Inno.Build.IGameContentCompiler Inno.Build.GameBuildTargetBinding.compiler { get; }`](../../build/pipeline/Inno.Build/Game/GameBuildTargetBinding.cs#L45) | Gets the content implementation borrowing this pipeline's authoring generation. | — | — |
| <a id="api-4183e1a1b6e16cf2"></a>[`public Inno.Build.IGameBuildTarget Inno.Build.GameBuildTargetBinding.packager { get; }`](../../build/pipeline/Inno.Build/Game/GameBuildTargetBinding.cs#L40) | Gets the platform implementation responsible for layout and Support Pack validation. | — | — |
| <a id="api-5a3cef1c43b18a6f"></a>[`public sealed class Inno.Build.GameBuildTargetBinding`](../../build/pipeline/Inno.Build/Game/GameBuildTargetBinding.cs#L8) | Binds independent packaging and content implementations for one publication target. | — | — |

### `Inno.Build.IGameBuildTarget`

<!-- api: T:Inno.Build.IGameBuildTarget 20ad06c05dd6df219314ac128c3029cc132dbd956902f6264a0191c962744676 -->
<!-- api: P:Inno.Build.IGameBuildTarget.id 3e1dae1ad818e5c613ff854538f28c151557a8280fca2ca90f9ef6fbee0624dc -->
<!-- api: P:Inno.Build.IGameBuildTarget.defaultManagedDeployment fbcebe1c15e43e4b89fffe7ea5e4d4b233168a2f2fb3d7a2f0ea345e6134ebdf -->
<!-- api: P:Inno.Build.IGameBuildTarget.runtimeIdentifier dde0dfa015e3b9d2a594a1ad2151e3ec019d93a645bc54ae7c3d892c677286ea -->
<!-- api: P:Inno.Build.IGameBuildTarget.displayName 33791805cb5acdfc3c60f588118e9a648f45aeb62df5f6563953b493b50e2bfe -->
<!-- api: M:Inno.Build.IGameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext,System.Threading.CancellationToken) f268f1c623449444b828cc482346c8fd7a628ef030830e01367de2f2530b32bc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e7b851faefbde586"></a>[`System.Threading.Tasks.ValueTask<string> Inno.Build.IGameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L47) | Composes one platform output from a verified Support Pack and source-free content directory.<br>返回：The single platform output path created beneath Inno.Build.GameBuildPackageContext.outputDirectory.<br>`context`：The isolated package context owned by the current build.<br>`cancellationToken`：The token that cancels work before the build is committed. | — | `System.OperationCanceledException`：Thrown when packaging is canceled. |
| <a id="api-f1c08ac447d0f226"></a>[`Inno.Build.Managed.ManagedDeploymentId Inno.Build.IGameBuildTarget.defaultManagedDeployment { get; }`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L20) | Gets the managed publisher selected when the profile does not specify an override. | — | — |
| <a id="api-aeba8ef09659bc67"></a>[`string Inno.Build.IGameBuildTarget.displayName { get; }`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L30) | Gets the user-facing target name presented by authoring hosts. | — | — |
| <a id="api-984ab41695089b6b"></a>[`Inno.Build.BuildTargetId Inno.Build.IGameBuildTarget.id { get; }`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L15) | Gets the stable target identity implemented by this packager. | — | — |
| <a id="api-1fa7c8699b8cf505"></a>[`string Inno.Build.IGameBuildTarget.runtimeIdentifier { get; }`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L25) | Gets the managed toolchain target identifier required by this platform composition. | — | — |
| <a id="api-c97cb8056aa72b2c"></a>[`public interface Inno.Build.IGameBuildTarget : Inno.Build.IPlayerSupportPackValidator`](../../build/pipeline/Inno.Build/Game/IGameBuildTarget.cs#L10) | Defines the replaceable platform packaging boundary for one game build target. | — | — |

### `Inno.Build.IGameContentCompiler`

<!-- api: T:Inno.Build.IGameContentCompiler 5eb7306753852e76f551da62221ffa7c51fdfd0fafcc3b788646a71ac271c7c4 -->
<!-- api: P:Inno.Build.IGameContentCompiler.target dcb065582dc1f77bdd5c964936c6a7e045832ca062ae2e461d3f90266c69e11f -->
<!-- api: M:Inno.Build.IGameContentCompiler.CompileAsync(Inno.Build.GameBuildContentContext,System.Threading.CancellationToken) d0860937c76cfae79351e4c7d287527ac975a5a55ef26c42b15d52792ea22dcb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7375fe81fc3f5e90"></a>[`System.Threading.Tasks.ValueTask Inno.Build.IGameContentCompiler.CompileAsync(Inno.Build.GameBuildContentContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/Game/IGameContentCompiler.cs#L31) | Writes target content into the current operation's isolated staging directory.<br>返回：An operation completing after every required artifact is staged, or faulting on failure.<br>`context`：The borrowed authoring generation and operation-owned content destination.<br>`cancellationToken`：Cancels compilation before publication; associated work must finish before returning. | — | `System.OperationCanceledException`：Cancellation was requested before compilation completed. |
| <a id="api-f249563576a9cc24"></a>[`Inno.Build.BuildTargetId Inno.Build.IGameContentCompiler.target { get; }`](../../build/pipeline/Inno.Build/Game/IGameContentCompiler.cs#L14) | Gets the exact publication target whose content this compiler produces. | — | — |
| <a id="api-64c4fa07a7bef1f3"></a>[`public interface Inno.Build.IGameContentCompiler`](../../build/pipeline/Inno.Build/Game/IGameContentCompiler.cs#L9) | Compiles source-free runtime content independently of application packaging. | — | — |

### `Inno.Build.IPlayerSupportPackProvisioner`

<!-- api: T:Inno.Build.IPlayerSupportPackProvisioner f35810c7bd88de1cc0130407ee9b8bea6fc51dbdd9a21a83df73bae28d04d92a -->
<!-- api: M:Inno.Build.IPlayerSupportPackProvisioner.ProvisionAsync(Inno.Build.BuildTargetId,System.String,System.Threading.CancellationToken) 58da8fb0fa702c68982c8dc89ee4e96fbe69c0f688702239d4dd90059176fe98 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cdf8f9b8589542ac"></a>[`System.Threading.Tasks.ValueTask Inno.Build.IPlayerSupportPackProvisioner.ProvisionAsync(Inno.Build.BuildTargetId target, string supportPackRoot, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/Player/IPlayerSupportPackProvisioner.cs#L26) | Checks current source inputs and selects a complete immutable pack before the build resumes.<br>返回：An operation that completes after the pack has been installed.<br>`target`：The platform and architecture target whose current inputs are required.<br>`supportPackRoot`：The catalog directory where the verified pack must be installed.<br>`cancellationToken`：Cancellation before the installation commits. | — | — |
| <a id="api-99737b355d923abd"></a>[`public interface Inno.Build.IPlayerSupportPackProvisioner`](../../build/pipeline/Inno.Build/Player/IPlayerSupportPackProvisioner.cs#L9) | Prepares current Player Support Pack inputs without coupling game builds to an SDK or platform toolchain. | — | — |

### `Inno.Build.IPlayerSupportPackValidator`

<!-- api: T:Inno.Build.IPlayerSupportPackValidator 94457341ac2006b1fe022eee2f00c9ed8343134c62abedb7cb663d5ca469a3de -->
<!-- api: M:Inno.Build.IPlayerSupportPackValidator.Validate(System.String) ab7ccde80cc1ed48a95c3085c6275c369fc24ce3f065c0c7ba6a725e5bedbb3d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8005fc69f4ec3288"></a>[`void Inno.Build.IPlayerSupportPackValidator.Validate(string directory)`](../../build/pipeline/Inno.Build/Player/IPlayerSupportPackValidator.cs#L17) | Rejects an incomplete or incompatible platform closure.<br>`directory`：The prepared or installed Support Pack directory. | — | `System.IO.InvalidDataException`：A required platform input is absent or incompatible. |
| <a id="api-585520529c88e48c"></a>[`public interface Inno.Build.IPlayerSupportPackValidator`](../../build/pipeline/Inno.Build/Player/IPlayerSupportPackValidator.cs#L6) | Validates platform-owned runtime or link inputs without coupling the pack catalog to target names. | — | — |

### `Inno.Build.PlatformTargetDescriptor`

<!-- api: T:Inno.Build.PlatformTargetDescriptor 098ad18ffedd28703e7a6aed814cb3b6c209dd8fb935651937cdbf9241a88191 -->
<!-- api: M:Inno.Build.PlatformTargetDescriptor.#ctor(Inno.Build.BuildTargetId,System.String,System.String,System.String,System.String,System.Boolean) 50e28e7aee9a67d142da76053d991b6bcaa8ae0b686d055531bd9bb32987ff60 -->
<!-- api: P:Inno.Build.PlatformTargetDescriptor.id 857e31cbb10380571bbc8b3003cd31207435625dd4de25f717502d32275fc747 -->
<!-- api: P:Inno.Build.PlatformTargetDescriptor.platform 6c767dce1ca809022f19e7f5363f01a0ebcc733a521cd2a2735b6cdc0a0b02b4 -->
<!-- api: P:Inno.Build.PlatformTargetDescriptor.architecture 8cd942c95e2d010205075e692b678e294eae62cafba640fa2a98403fc0b67552 -->
<!-- api: P:Inno.Build.PlatformTargetDescriptor.abi c04a75f6e67dd2900afb8c85bf96dd1455fef88c9c4bf09e1f491f33fd5c3437 -->
<!-- api: P:Inno.Build.PlatformTargetDescriptor.runtimeIdentifier 4791062ae93eafe8605c737f8d64299c9609954661dbc5c8a3703b9d57bc9ac1 -->
<!-- api: P:Inno.Build.PlatformTargetDescriptor.supportsEditor 0da43852284f2af275d3512d81fa1b1ca4381f2c9d84486c05e1fdfc19bbbe61 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dcff370c8af796a9"></a>[`public Inno.Build.PlatformTargetDescriptor.PlatformTargetDescriptor(Inno.Build.BuildTargetId id, string platform, string architecture, string abi, string runtimeIdentifier, bool supportsEditor)`](../../build/pipeline/Inno.Build/PlatformTargetDescriptor.cs#L34) | Creates an explicit platform, processor, ABI and managed-runtime mapping.<br>`id`：The stable publication target identity.<br>`platform`：The platform package owning system integration.<br>`architecture`：The target processor architecture, such as x64, arm64 or wasm32.<br>`abi`：The native ABI selected by this platform implementation.<br>`runtimeIdentifier`：The exact managed toolchain runtime identifier.<br>`supportsEditor`：Whether this implemented target supports the shared Editor product. | — | `System.ArgumentException`：An identity or required target fact is unassigned. |
| <a id="api-e314b3496675ca12"></a>[`public string Inno.Build.PlatformTargetDescriptor.abi { get; }`](../../build/pipeline/Inno.Build/PlatformTargetDescriptor.cs#L73) | Gets the explicitly selected native ABI. | — | — |
| <a id="api-8145ef0936e7c2a2"></a>[`public string Inno.Build.PlatformTargetDescriptor.architecture { get; }`](../../build/pipeline/Inno.Build/PlatformTargetDescriptor.cs#L68) | Gets the explicitly declared processor architecture. | — | — |
| <a id="api-4d11f42154fca77d"></a>[`public Inno.Build.BuildTargetId Inno.Build.PlatformTargetDescriptor.id { get; }`](../../build/pipeline/Inno.Build/PlatformTargetDescriptor.cs#L58) | Gets the stable publication target identity. | — | — |
| <a id="api-e056491794bcfb44"></a>[`public string Inno.Build.PlatformTargetDescriptor.platform { get; }`](../../build/pipeline/Inno.Build/PlatformTargetDescriptor.cs#L63) | Gets the owning platform package. | — | — |
| <a id="api-63342bf22a2cf272"></a>[`public string Inno.Build.PlatformTargetDescriptor.runtimeIdentifier { get; }`](../../build/pipeline/Inno.Build/PlatformTargetDescriptor.cs#L78) | Gets the managed runtime mapping without deriving it from the target identity. | — | — |
| <a id="api-083806b422d7a925"></a>[`public bool Inno.Build.PlatformTargetDescriptor.supportsEditor { get; }`](../../build/pipeline/Inno.Build/PlatformTargetDescriptor.cs#L83) | Gets whether this target provides an implemented Editor host. | — | — |
| <a id="api-011491807c9f206e"></a>[`public sealed class Inno.Build.PlatformTargetDescriptor`](../../build/pipeline/Inno.Build/PlatformTargetDescriptor.cs#L8) | Declares immutable product target facts independently of the machine executing build tools. | — | — |

### `Inno.Build.PlayerSupportPackCatalog`

<!-- api: T:Inno.Build.PlayerSupportPackCatalog 6dae889299b205e4340980782a5f117d3ed102508c19f26a925c739877037dfb -->
<!-- api: M:Inno.Build.PlayerSupportPackCatalog.#ctor(System.String) b9eb8a4c3bbe90550591645b10128469d01654dc61a53a734f143a37a5be475f -->
<!-- api: M:Inno.Build.PlayerSupportPackCatalog.Acquire(Inno.Build.BuildTargetId,Inno.Build.IPlayerSupportPackValidator,System.Threading.CancellationToken) ed52be63f3dd8861b124ecb1ce374e0c0a5acb6ebef503351eeee20552d7897b -->
<!-- api: M:Inno.Build.PlayerSupportPackCatalog.PublishAsync(Inno.Build.BuildTargetId,System.String,Inno.Build.IPlayerSupportPackValidator,System.Threading.CancellationToken,System.Func{System.Threading.CancellationToken,System.Threading.Tasks.ValueTask}) f0ceccbabdf94dd006c13141c83b196aff6dc9a28dfb6a1d8471868cb40f253c -->

<!-- api: M:Inno.Build.PlayerSupportPackCatalog.SelectAsync(Inno.Build.BuildTargetId,System.String,Inno.Build.IPlayerSupportPackValidator,System.Func{System.Threading.CancellationToken,System.Threading.Tasks.ValueTask},System.Threading.CancellationToken) d27b46b691f0706ba699f8ef3b3cadac543baac2590785a597bdd27ed41dd479 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8fd63b0c07ce9b7b"></a>[`public Inno.Build.PlayerSupportPackCatalog.PlayerSupportPackCatalog(string root)`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackCatalog.cs#L49) | Creates a catalog rooted at the directory containing target-specific Player Support Packs.<br>`root`：The directory whose target children contain immutable fingerprint directories and a current index. | — | `System.ArgumentException`：Thrown when root is empty. |
| <a id="api-b7aab6f6e7113020"></a>[`public Inno.Build.PlayerSupportPackLease Inno.Build.PlayerSupportPackCatalog.Acquire(Inno.Build.BuildTargetId target, Inno.Build.IPlayerSupportPackValidator validator, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackCatalog.cs#L144) | Resolves one installed Support Pack after validating its deployment-only closure.<br>返回：Reading ownership of the complete pack; dispose it after its final consumer finishes.<br>`target`：The platform and architecture identity of the required Support Pack.<br>`validator`：The target-owned validator for runtime or linker inputs.<br>`cancellationToken`：Cancels ownership acquisition or complete content validation before returning a lease. | — | `System.IO.DirectoryNotFoundException`：Thrown when no Support Pack is installed for target.<br>`System.IO.InvalidDataException`：Thrown when the pack is empty, contains build-time payload, or lacks its Player executable or native runtimes.<br>`System.OperationCanceledException`：Acquisition or validation was canceled; no reading ownership is transferred.<br>`System.TimeoutException`：Exclusive repair or retirement retains the artifact for more than thirty seconds. |
| <a id="api-4c0b5a10e1671e9f"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.PlayerSupportPackLease> Inno.Build.PlayerSupportPackCatalog.PublishAsync(Inno.Build.BuildTargetId target, string stagingDirectory, Inno.Build.IPlayerSupportPackValidator validator, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken), System.Func<System.Threading.CancellationToken, System.Threading.Tasks.ValueTask>? verifyInputs = null)`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackCatalog.cs#L198) | 验证完整 output 后，在 publication wait 后调用可选 source gate，再原子安装和选择 current；只拥有已完成 staging 的直接调用者可省略 gate。 | 借用声明和 operation；完整任务在返回前结束。 | 缺输入、身份不一致或取消会拒绝接受候选；保留已有完整输出。 |
| <a id="api-b8cb057156e01ab9"></a>[`public sealed class Inno.Build.PlayerSupportPackCatalog`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackCatalog.cs#L17) | Resolves and validates installed Player Support Packs without loading authoring services. | — | — |
| <a id="api-0f5bea75087f755e"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.PlayerSupportPackLease> Inno.Build.PlayerSupportPackCatalog.SelectAsync(Inno.Build.BuildTargetId target, string fingerprint, Inno.Build.IPlayerSupportPackValidator validator, System.Func<System.Threading.CancellationToken, System.Threading.Tasks.ValueTask> verifyInputs, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackCatalog.cs#L85) | 按完整 content fingerprint 验证实际 Pack；持有读取保护，最终 input gate 成功后原子选择 current。 | verifyInputs 只用于当前 operation，返回读取 lease 后调用者负责 Dispose。 | 内容缺失/损坏、source gate 失败或取消时拒绝选择；原 current 保持。 |

### `Inno.Build.PlayerSupportPackLease`

<!-- api: T:Inno.Build.PlayerSupportPackLease 405903513eb880ceef5e9d7272309bddb8c0529698c26c83c2fe129078d7cdf9 -->
<!-- api: P:Inno.Build.PlayerSupportPackLease.directory 59d88b8cb5d000ef28dc5fe4337d69ddf112489071fb766c4d4f3c77b148122d -->
<!-- api: M:Inno.Build.PlayerSupportPackLease.Dispose b368ec6ccacad21998f623fee936080915c24e30c8049d7912182a0faee6cf00 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-df4fa2275a380476"></a>[`public void Inno.Build.PlayerSupportPackLease.Dispose()`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackLease.cs#L42) | Releases reading rights after all compilation, copying and linking have completed. Repeated disposal has no effect. | — | — |
| <a id="api-e091e78c9c6d7a0f"></a>[`public string Inno.Build.PlayerSupportPackLease.directory { get; }`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackLease.cs#L29) | Gets the verified absolute directory while this lease owns reading rights. | — | `System.ObjectDisposedException`：The consumer has already released its reading rights. |
| <a id="api-f4a5dace3737bf4c"></a>[`public sealed class Inno.Build.PlayerSupportPackLease : System.IDisposable`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackLease.cs#L10) | Keeps verified Player compilation and linker inputs alive until their consumer finishes. | — | — |

### `Inno.Build.PluginBuildRequest`

<!-- api: T:Inno.Build.PluginBuildRequest bdbd5feecb7747ee7b85a790780b2fa9e54134fc693db34e54dbd59d6f417bbc -->
<!-- api: P:Inno.Build.PluginBuildRequest.pluginId 8c49a0130995131515d88ed4cb7be24421ed81a6aa100d5e74d60b527d9ea423 -->
<!-- api: P:Inno.Build.PluginBuildRequest.displayName 1d66b953d24a0fc6300a1d64bdcf8f6b7dd90ce8710004ca1e004d34d1cd1966 -->
<!-- api: P:Inno.Build.PluginBuildRequest.outputPath a61da8f79bd5d5171cf7f0489d783f7bfdc67e2e3be3f2d40c08b17416903110 -->
<!-- api: P:Inno.Build.PluginBuildRequest.includeDependencies da0a04bb4e0e324e03f47ca49cfadcba0118a4fbc5d504ac7e86c550a2aea161 -->
<!-- api: P:Inno.Build.PluginBuildRequest.dependencies feb3612c41821999d4af62f071a051c4620967ea920fdba7ab3bf1a9dd5ad253 -->
<!-- api: M:Inno.Build.PluginBuildRequest.Validate afee44930a46819d3e2a4b93f36d79b0d8d389edfae0b0702df4374d3cc8ebd4 -->
<!-- api: M:Inno.Build.PluginBuildRequest.#ctor efde6b73c288a64c42296652d47eaa9a6f719ac2fdbc42d77b8350151eef672b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-36bc2353ccb2d8c2"></a>[`public Inno.Build.PluginBuildRequest.PluginBuildRequest()`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L11) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-f08255d86800cbcf"></a>[`public void Inno.Build.PluginBuildRequest.Validate()`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L45) | Validates package identity and destination syntax. | — | `System.IO.InvalidDataException`：Thrown when the request cannot identify a portable Plugin package. |
| <a id="api-c99b27fd2682424f"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Build.PluginBuildRequest.dependencies { get; init; }`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L37) | Gets or initializes the installed plugins this package actually requires. Their transitive dependencies are included in the package manifest. | — | — |
| <a id="api-11c9514811418f8c"></a>[`public required string Inno.Build.PluginBuildRequest.displayName { get; init; }`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L21) | Gets or initializes the user-facing Plugin name. | — | — |
| <a id="api-2fb293f3f5cdfe9a"></a>[`public bool Inno.Build.PluginBuildRequest.includeDependencies { get; init; }`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L31) | Gets or initializes whether the complete transitive dependency closure is embedded. | — | — |
| <a id="api-7eb4301495271984"></a>[`public required string Inno.Build.PluginBuildRequest.outputPath { get; init; }`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L26) | Gets or initializes the destination .iplugin package path. | — | — |
| <a id="api-b8fdad1f910ff64c"></a>[`public required string Inno.Build.PluginBuildRequest.pluginId { get; init; }`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L16) | Gets or initializes the globally stable lowercase Plugin identity. | — | — |
| <a id="api-6c48421785e2d8ae"></a>[`public sealed class Inno.Build.PluginBuildRequest`](../../build/pipeline/Inno.Build/PluginBuildRequest.cs#L11) | Requests one deterministic project-to-Plugin package without a companion authoring asset. | — | — |

## 项目依赖

- [Inno.Content](../assets/Inno.Content.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization.Generators](../core/Inno.Core.Serialization.Generators.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Runtime](../runtime/Inno.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scene.Assets](../scene/Inno.Scene.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Compiler](../scripting/Inno.Scripting.Compiler.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Plugins](../plugins/Inno.Plugins.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。

### `Inno.Build.PlayerSupportPackLease`

<!-- api: T:Inno.Build.PlayerSupportPackLease 405903513eb880ceef5e9d7272309bddb8c0529698c26c83c2fe129078d7cdf9 -->
<!-- api: P:Inno.Build.PlayerSupportPackLease.directory 59d88b8cb5d000ef28dc5fe4337d69ddf112489071fb766c4d4f3c77b148122d -->
<!-- api: M:Inno.Build.PlayerSupportPackLease.Dispose b368ec6ccacad21998f623fee936080915c24e30c8049d7912182a0faee6cf00 -->

| 声明 | 行为与结果 | 所有权/线程 | 失败或限制 |
| --- | --- | --- | --- |
| [`public void Inno.Build.PlayerSupportPackLease.Dispose()`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackLease.cs#L42) | Releases reading rights after all compilation, copying and linking have completed. Repeated disposal has no effect. | 当前实例唯一拥有本次读取权；可重复 Dispose。 | 释放后不能再取得 directory；不接管调用者进程。 |
| [`public string Inno.Build.PlayerSupportPackLease.directory { get; }`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackLease.cs#L29) | Gets the verified absolute directory while this lease owns reading rights. | 当前实例唯一拥有本次读取权；可重复 Dispose。 | 释放后不能再取得 directory；不接管调用者进程。 |
| [`Inno.Build.PlayerSupportPackLease`](../../build/pipeline/Inno.Build/Player/PlayerSupportPackLease.cs#L10) | Keeps verified Player compilation and linker inputs alive until their consumer finishes. | 当前实例唯一拥有本次读取权；可重复 Dispose。 | 释放后不能再取得 directory；不接管调用者进程。 |
