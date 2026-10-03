# Inno.Build

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Runtime](../runtime/Inno.Runtime.md)

## 职责与边界

Build bounded context 拥有 Game/Plugin 构建编排、组合 generation 检查、流式 Content Pack、Support Pack 验证、staging 和原子提交。它引用 authoring 服务，但绝不引用 Editor；平台布局由 `IGameBuildTarget` 提供。

Game Build 在内容打包前把已验证 Support Pack 作为目标运行时交给 Script Compiler。脚本仍先经过当前裁剪 API 校验，再针对 Pack 中实际部署的引擎程序集编译；因此部署兼容性不再由 Editor 进程加载顺序推导的全局指纹决定，也不会到 Player 启动后才以紫色错误画面暴露二进制不兼容。

## 公开 API

| API | 语义 |
| --- | --- |
| `BuildSettings`, `BuildSettingsStore` | 项目拥有的 Game/Plugin 导出默认值；使用当前 Inno Serialization 原子保存为 `Settings.Build.inno` |
| `BuildProfile`, `BuildProfileStore`, `BuildTargetId` | 一次 Game 构建所需的可验证 profile 与目标身份；显式 profile 文件可供 headless one-off 构建使用 |
| `GameBuildRequest`, `PluginBuildRequest` | 一次不可变构建请求 |
| `BuildProgress`, `BuildDiagnostic`, `BuildDiagnosticSeverity`, `BuildResult` | 进度、结构化诊断与最终结果 |
| `BuildPipeline` | Game/Plugin 的最小异步入口，并公开当前注册目标、adapter-selected 默认目标、显示名称查询与 `EnsurePlayerSupportPackAsync` 预备入口 |
| `IGameBuildTarget` | 真正可替换的平台目标 contract；目标自己声明稳定 ID、显示名称和当前 Host preference |
| `GameBuildContentContext`, `GameBuildPackageContext` | 平台目标获得的隔离 staging context；Package context 提供冻结的 runtime 程序集目录，供需要静态链接游戏代码的目标使用 |
| `PlayerSupportPackCatalog` | 验证并解析部署 closure；只将不存在的目标交给可选供给器 |
| `IPlayerSupportPackProvisioner` | Host 注入的异步缺包供给边界；独立发行可不安装 SDK |

Content writer、`.iplugin` archive writer、snapshot fingerprint、script stage、staging transaction 与 player composer 全部 internal。

## 工作流

```csharp
// Complete preparation first, then start this snapshot on the authoring owner's thread.
await pipeline.EnsurePlayerSupportPackAsync(profile.target, cancellationToken);
BuildResult result = await pipeline.BuildGameAsync(
    new GameBuildRequest
    {
        profile = profile,
        outputDirectory = outputDirectory
    },
    progress,
    cancellationToken);
```

构建开始后会捕获 Assets/Plugins/Settings revision 与 Serialization generation。任一代际变化、取消或 stage 失败都会清理 staging，不覆盖已提交产品。

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

调用者拥有 cancellation 和 progress；`BuildPipeline` 使用注入服务但不拥有其生命周期。源码工作区的 Editor/CLI 注入 [SourcePlayerSupportPackProvisioner](Inno.Build.SupportPacks.Core.md)：`EnsurePlayerSupportPackAsync` 在目标 Pack 缺失时运行隔离发布与验证；已有有效 Pack 直接使用，已有损坏 Pack 明确失败。
`BuildGameAsync` 要求 Pack 已准备，缺失时立即失败，不在资产快照阶段同步等待异步供给器。Editor 先异步准备，再在完成后的主线程帧启动构建，避免跨线程访问资产或冻结 UI。CLI 没有 UI 调度器，由宿主入口协调等待并保持资产 owner 线程，再启动异步构建。其他宿主同样必须在自己的 owner 安全点启动快照；普通 `await` 不自动保证切回原线程。

游戏目录安装复用 `AtomicDirectory.Install`；调用方协调两次目录移动期间同一目标的读写。
安装失败尝试恢复旧目录；提交后旧备份清理失败会抛出包含已安装目标与残留备份路径的 IO 异常，
保留完整新输出。不能把此类清理失败解释成新输出没有安装，也不能把两次移动当作无间隙的文件替换。
独立发行的 Editor 必须预装 Pack。输出不能位于 Assets、Plugins 或 Library。损坏 Artifact、无法供给的 Support Pack、无 runtime assembly、目标返回越界路径都会在提交前失败。Startup Scene 必须是已导入且可部署的 `SceneAsset`；位于任意 `~` 目录时会以 authoring-only 错误明确拒绝。

## 平台校验契约

`IPlayerSupportPackValidator.Validate(directory)` 由平台拥有，拒绝缺失/不兼容输入。IGameBuildTarget 继承该接口；`PlayerSupportPackCatalog.Resolve(target, validator)` 只做共有目录与部署输入约束，然后调用注册平台的 validator。Catalog 不维护封闭的平台名单，第三方目标不必修改 Core Build。Pack 中 PlayerLink 专门保存链接输入，最终游戏部署由平台 PackageAsync 决定。
