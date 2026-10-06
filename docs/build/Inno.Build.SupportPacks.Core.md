# Inno.Build.SupportPacks.Core

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

拥有 Support Pack 的准备、完整性验证、不可变产物发布与自动供给事务。只依赖中立 Build 和 Core IO 契约；不注册内置平台，不引用具体工具链或 Editor。

## 初始化与工作流

Host 传入一组 IPlayerSupportPackSource，PlayerSupportPackPublisher 冻结唯一目标注册表。
PrepareAsync 在隔离 staging 中准备输入；source 验证完整闭包，内容哈希形成产物身份；
完整目录发布后用 AtomicFile 切换 current。独占 FileLease 协调同一目标的多进程准备。

```csharp
using System.Collections.Generic;
using Inno.Build.SupportPacks;

static PlayerSupportPackPublisher CreatePublisher(IEnumerable<IPlayerSupportPackSource> sources)
{
    return new PlayerSupportPackPublisher(sources);
}
```

内置平台由 [Build Composition](Inno.Build.Composition.md) 唯一注册，Editor、CLI 和 MSBuild 复用同一分布。
SourcePlayerSupportPackProvisioner 负责从 Host 定位源码并供给当前 Pack；纯发行 Host 可以不配置该能力。

## 所有权与失败

Publisher 拥有本次 staging、写 lease 与 current 提交，source 拥有其构建任务和取消。
提交前的失败或取消保留上次完整输出；已选中的 immutable artifact 不覆盖正在使用的目录。
缺少 source、重复 ID、无效 Native/managed 输入和工具失败明确报告。
真实校验见 BuildCompositionTests、Support Pack publication、取消与缺失启动源码测试。

没有额外 protected 扩展点；第三方实现 IPlayerSupportPackSource 及其验证契约即可加入组合。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.SupportPacks.IPlayerSupportPackSource`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Threading.Tasks.ValueTask Inno.Build.SupportPacks.IPlayerSupportPackSource.PrepareAsync(Inno.Build.SupportPacks.PlayerSupportPackBuildContext context, System.Threading.CancellationToken cancellationToken)`](../../build/support/Inno.Build.SupportPacks.Core/IPlayerSupportPackSource.cs#L29) | Builds the complete pack in the supplied isolated directory. |
| [`Inno.Build.BuildTargetId Inno.Build.SupportPacks.IPlayerSupportPackSource.target`](../../build/support/Inno.Build.SupportPacks.Core/IPlayerSupportPackSource.cs#L15) | Gets the stable target identity implemented by this source. |
| [`Inno.Build.SupportPacks.IPlayerSupportPackSource`](../../build/support/Inno.Build.SupportPacks.Core/IPlayerSupportPackSource.cs#L10) | Prepares one platform's runtime and compiler inputs without owning installation or replacement. |

### `Inno.Build.SupportPacks.PlayerSupportPackBuildContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.SupportPacks.PlayerSupportPackBuildContext.PlayerSupportPackBuildContext(string engineRoot, string stagingDirectory, string dotnetHost)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L25) | Creates immutable inputs for an isolated preparation operation. |
| [`string Inno.Build.SupportPacks.PlayerSupportPackBuildContext.dotnetHost`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L51) | Gets the SDK executable selected by the composition host. |
| [`string Inno.Build.SupportPacks.PlayerSupportPackBuildContext.engineRoot`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L41) | Gets the engine source checkout. |
| [`string Inno.Build.SupportPacks.PlayerSupportPackBuildContext.stagingDirectory`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L46) | Gets the isolated directory that the source must populate. |
| [`Inno.Build.SupportPacks.PlayerSupportPackBuildContext`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L8) | Supplies preparation with explicit paths and a selected SDK, excluding installed output. |

### `Inno.Build.SupportPacks.PlayerSupportPackPublisher`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.SupportPacks.PlayerSupportPackPublisher.PlayerSupportPackPublisher(System.Collections.Generic.IEnumerable<Inno.Build.SupportPacks.IPlayerSupportPackSource> sources)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs#L27) | Freezes the platform sources used by this publisher. |
| [`System.Threading.Tasks.ValueTask<string> Inno.Build.SupportPacks.PlayerSupportPackPublisher.PublishAsync(string engineRoot, string outputRoot, Inno.Build.BuildTargetId target, string dotnetHost, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs#L79) | Prepares a target closure in isolation and publishes its immutable deployment inputs. |
| [`Inno.Build.SupportPacks.PlayerSupportPackPublisher`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs#L14) | Installs verified Support Packs with rollback protection using explicitly registered platform sources. |

### `Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.SourcePlayerSupportPackProvisioner(string engineRoot, Inno.Build.SupportPacks.PlayerSupportPackPublisher publisher, string dotnetHost)`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L33) | Creates a provisioner for a complete engine checkout. |
| [`System.Threading.Tasks.ValueTask Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.ProvisionAsync(Inno.Build.BuildTargetId target, string supportPackRoot, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L102) | Prepares current target inputs and atomically selects their immutable Support Pack. |
| [`static Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner? Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.TryCreateForHost(string startDirectory, Inno.Build.SupportPacks.PlayerSupportPackPublisher publisher, string dotnetHost)`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L66) | Finds the checkout that owns the running Editor or build command. |
| [`Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L12) | Prepares current checkout and SDK inputs and selects a verified, immutable Player Support Pack. |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build](Inno.Build.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
