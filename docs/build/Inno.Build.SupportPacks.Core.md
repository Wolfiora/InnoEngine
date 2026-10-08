# Inno.Build.SupportPacks.Core

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

拥有 Support Pack 的准备、完整性验证、不可变产物发布与自动供给事务。只依赖中立 Build 和 Core IO 契约；不注册内置平台，不引用具体工具链或 Editor。

## 初始化与工作流

Host 传入一组 IPlayerSupportPackSource，PlayerSupportPackPublisher 冻结唯一目标注册表。
`IPlayerSupportPackSource.CreatePlanAsync(PlayerSupportPackPlanningContext, CancellationToken)` 先验证源、模板、SDK 和宿主，
返回 `PlayerSupportPackPlan`。Planning context 没有输出目录；计划固定目标和 Native selection、managed host/CLI。
Publisher 核对目标并观察取消后才取得写 lease、创建 staging；`plan.PrepareAsync(stagingDirectory, token)` 执行冻结输入。
source 验证完整闭包，内容哈希形成产物身份；
完整目录发布后用 AtomicFile 切换 current。独占 FileLease 协调同一目标的多进程准备。

```csharp
using System.Collections.Generic;
using Inno.Build.SupportPacks;

static PlayerSupportPackPublisher CreatePublisher(IEnumerable<IPlayerSupportPackSource> sources)
{
    return new PlayerSupportPackPublisher(sources);
}
```

内置平台由 [Standard Distribution](Inno.Build.Distribution.Standard.md) 唯一注册，Editor、CLI 和 MSBuild 复用同一分布。
SourcePlayerSupportPackProvisioner 负责从 Host 定位源码并供给当前 Pack；纯发行 Host 可以不配置该能力。

## 所有权与失败

Publisher 拥有本次 staging、写 lease 与 current 提交，source 拥有其构建任务和取消。
提交前的失败或取消保留上次完整输出；已选中的 immutable artifact 不覆盖正在使用的目录。
缺少 source、重复 ID、无效 Native/managed 输入和工具失败明确报告。
真实校验见 BuildCompositionTests、Support Pack publication、取消与缺失启动源码测试。

第三方实现 `IPlayerSupportPackSource` 和继承 `PlayerSupportPackPlan`。protected 构造函数固定目标，
公开 `PrepareAsync` 完成全部任务后返回。计划只保存不可变配置与借用的实现，不持有跨阶段运行工作。
`PlayerSupportPackBuildContext.sdk` 是 `DotNetSdkDescriptor`，`PlayerSupportPackFiles` 使用记录的 CLI entry；
不在 staging 中再次根据 PATH/global.json 选择 SDK。
缺 SDK、预取消、计划为空或目标错误不创建输出；执行、校验和锁等待取消保留旧完整产物。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.SupportPacks.FilePlayerSupportPackPreparation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildTargetId Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.target`](../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L104) | Gets the platform identity whose closure this source prepares. |
| [`Inno.Build.SupportPacks.FilePlayerSupportPackPreparation`](../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L15) | Prepares file-based product inputs using injected target tools, source locations and native closure. |
| [`Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.FilePlayerSupportPackPreparation(Inno.Build.BuildTargetId target, string runtimeIdentifier, string nativePlatform, string nativeExtension, Inno.Build.IPlayerSupportPackValidator validator, string productProject, string templateFile, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.INativeToolchainProvider toolchainProvider, Inno.Build.Toolchains.BuildHostDescriptor host)`](../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L67) | Captures the platform layout and validator without preparing any tools or files. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L118) | Resolves tools and validates required source inputs before any staging exists. |
| [`void Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.Validate(string directory)`](../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L177) | Validates the prepared platform inputs before atomic installation. |

### `Inno.Build.SupportPacks.IPlayerSupportPackSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildTargetId Inno.Build.SupportPacks.IPlayerSupportPackSource.target`](../../build/support/Inno.Build.SupportPacks.Core/IPlayerSupportPackSource.cs#L15) | Gets the stable target identity implemented by this source. |
| [`Inno.Build.SupportPacks.IPlayerSupportPackSource`](../../build/support/Inno.Build.SupportPacks.Core/IPlayerSupportPackSource.cs#L10) | Prepares one platform's runtime and compiler inputs without owning installation or replacement. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.SupportPacks.IPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../build/support/Inno.Build.SupportPacks.Core/IPlayerSupportPackSource.cs#L29) | Validates discovery inputs and freezes a plan before publication output exists. |

### `Inno.Build.SupportPacks.PlayerSupportPackBuildContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.SupportPacks.PlayerSupportPackBuildContext`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L9) | Supplies preparation with explicit paths and a selected SDK, excluding installed output. |
| [`Inno.Build.SupportPacks.PlayerSupportPackBuildContext.PlayerSupportPackBuildContext(string engineRoot, string stagingDirectory, Inno.Build.Toolchains.DotNetSdkDescriptor sdk)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L26) | Creates immutable inputs for an isolated preparation operation. |
| [`Inno.Build.Toolchains.DotNetSdkDescriptor Inno.Build.SupportPacks.PlayerSupportPackBuildContext.sdk`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L52) | Gets the frozen SDK host and CLI entry used without resolving another SDK. |
| [`string Inno.Build.SupportPacks.PlayerSupportPackBuildContext.engineRoot`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L42) | Gets the engine source checkout. |
| [`string Inno.Build.SupportPacks.PlayerSupportPackBuildContext.stagingDirectory`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L47) | Gets the isolated directory that the source must populate. |

### `Inno.Build.SupportPacks.PlayerSupportPackFiles`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.SupportPacks.PlayerSupportPackFiles`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackFiles.cs#L17) | Prepares explicit product source and reference inputs for isolated Support Pack publication. |
| [`static System.Threading.Tasks.Task Inno.Build.SupportPacks.PlayerSupportPackFiles.CopyPlayerSourcesAsync(Inno.Build.SupportPacks.PlayerSupportPackBuildContext context, string project, string destination, System.Threading.CancellationToken cancellationToken, System.Collections.Generic.IReadOnlyDictionary<string, string>? environment = null)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackFiles.cs#L43) | Copies the selected project's evaluated source closure into isolated template input. |
| [`static void Inno.Build.SupportPacks.PlayerSupportPackFiles.CopyCompositionInputs(string engineRoot, string playerDirectory)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackFiles.cs#L90) | Stages shared registration generators and the checkout's actual SDK selection. |
| [`static void Inno.Build.SupportPacks.PlayerSupportPackFiles.CopyReferences(string buildOutput, string destination, string productAssemblyName)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackFiles.cs#L125) | Stages current runtime references while excluding the selected executable and project scripts. |

### `Inno.Build.SupportPacks.PlayerSupportPackPlan`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildTargetId Inno.Build.SupportPacks.PlayerSupportPackPlan.target`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlan.cs#L31) | Gets the immutable target checked by the publisher before creating output. |
| [`Inno.Build.SupportPacks.PlayerSupportPackPlan`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlan.cs#L11) | Holds validated, frozen preparation inputs without owning running processes or staging resources. |
| [`Inno.Build.SupportPacks.PlayerSupportPackPlan.PlayerSupportPackPlan(Inno.Build.BuildTargetId target)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlan.cs#L22) | Fixes the publication identity before preparation is permitted. |
| [`abstract System.Threading.Tasks.ValueTask Inno.Build.SupportPacks.PlayerSupportPackPlan.PrepareAsync(string stagingDirectory, System.Threading.CancellationToken cancellationToken)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlan.cs#L45) | Executes frozen inputs in publisher-owned staging without selecting tools again. |

### `Inno.Build.SupportPacks.PlayerSupportPackPlanningContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.SupportPacks.PlayerSupportPackPlanningContext`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlanningContext.cs#L8) | Supplies read-only discovery inputs before a publication directory exists. |
| [`Inno.Build.SupportPacks.PlayerSupportPackPlanningContext.PlayerSupportPackPlanningContext(string engineRoot, string dotnetHost)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlanningContext.cs#L22) | Captures the checkout and host from which project-scoped tools are resolved. |
| [`string Inno.Build.SupportPacks.PlayerSupportPackPlanningContext.dotnetHost`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlanningContext.cs#L40) | Gets the explicit host from which the source freezes its SDK selection. |
| [`string Inno.Build.SupportPacks.PlayerSupportPackPlanningContext.engineRoot`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlanningContext.cs#L35) | Gets the borrowed source checkout used for preflight. |

### `Inno.Build.SupportPacks.PlayerSupportPackPublisher`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.SupportPacks.PlayerSupportPackPublisher`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs#L14) | Installs verified Support Packs with rollback protection using explicitly registered platform sources. |
| [`Inno.Build.SupportPacks.PlayerSupportPackPublisher.PlayerSupportPackPublisher(System.Collections.Generic.IEnumerable<Inno.Build.SupportPacks.IPlayerSupportPackSource> sources)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs#L27) | Freezes the platform sources used by this publisher. |
| [`System.Threading.Tasks.ValueTask<string> Inno.Build.SupportPacks.PlayerSupportPackPublisher.PublishAsync(string engineRoot, string outputRoot, Inno.Build.BuildTargetId target, string dotnetHost, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs#L80) | Prepares a target closure in isolation and publishes its immutable deployment inputs. |

### `Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L12) | Prepares current checkout and SDK inputs and selects a verified, immutable Player Support Pack. |
| [`Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.SourcePlayerSupportPackProvisioner(string engineRoot, Inno.Build.SupportPacks.PlayerSupportPackPublisher publisher, string dotnetHost)`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L33) | Creates a provisioner for a complete engine checkout. |
| [`System.Threading.Tasks.ValueTask Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.ProvisionAsync(Inno.Build.BuildTargetId target, string supportPackRoot, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L102) | Prepares current target inputs and atomically selects their immutable Support Pack. |
| [`static Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner? Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.TryCreateForHost(string startDirectory, Inno.Build.SupportPacks.PlayerSupportPackPublisher publisher, string dotnetHost)`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L66) | Finds the checkout that owns the running Editor or build command. |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Build](Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
