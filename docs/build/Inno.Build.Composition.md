# Inno.Build.Composition

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

唯一组合内置发布 target、managed compiler 与 Support Pack source 的库。Editor、CLI 与 MSBuild Task 使用同一 distribution，宿主只注入 SDK、输出根及资产服务。通用 Build 不反向引用本项目；本项目没有 Program。

## 工作流与扩展

`BuiltInBuildDistribution.Create(context)` 一次建立目标、部署与 Support Pack 的一致目录。`BuildDistribution` 冻结快照并预检配对；`BuildPipelineFactory` 为各宿主创建相同 Pipeline。

新增平台先实现 target、所需 managed compiler 与 Support Pack source，再在此组合入口注册。CLI、Editor 和 MSBuild 不各自维护平台名单。不支持的 target/deployment 配对在准备 SDK 和写输出之前明确失败。

宿主持有上下文与借用服务；Pipeline 负责构建事务，Support Pack publisher 负责完整验证和原子发布。失败、取消保留先前有效产物。

## 验证

`tests/build/Inno.Build.Tests/BuildCompositionTests.cs` 使用公开 provider 边界验证共享注册、配对和失败。CLI、Editor 与 Task 的实际构建验证与代码边界检查共同确认三入口一致。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.Composition.BuildCompositionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.BuildCompositionContext.BuildCompositionContext(string dotnetHost, string applicationDirectory)`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L23) | Captures the host dependencies used by deployment and Support Pack preparation. |
| [`string Inno.Build.Composition.BuildCompositionContext.applicationDirectory`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L43) | Gets the host location used for source provisioning discovery. |
| [`string Inno.Build.Composition.BuildCompositionContext.dotnetHost`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L38) | Gets the explicitly selected SDK executable. |
| [`Inno.Build.Composition.BuildCompositionContext`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L9) | Captures host-owned SDK selection and application location without probing global process state. |

### `Inno.Build.Composition.BuildDistribution`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.BuildDistribution.BuildDistribution(System.Collections.Generic.IReadOnlyList<Inno.Build.Composition.BuildTargetFactory> targetFactories, System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.IManagedDeploymentCompiler> managedCompilers, System.Collections.Generic.IReadOnlyList<Inno.Build.SupportPacks.IPlayerSupportPackSource> supportPackSources)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L38) | Captures a complete distribution without creating authoring services or starting any tool. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.IGameBuildTarget> Inno.Build.Composition.BuildDistribution.CreateTargets(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L93) | Creates platform targets and validates their identities and default deployment capabilities. |
| [`void Inno.Build.Composition.BuildDistribution.ValidateDeployment(Inno.Build.IGameBuildTarget target, Inno.Build.Managed.ManagedDeploymentId deployment)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L127) | Rejects unsupported target and deployment pairs before any staging or publication begins. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.BuildTargetId> Inno.Build.Composition.BuildDistribution.availableTargets`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L62) | Gets the immutable platform identities shared by all hosts. |
| [`Inno.Build.Managed.ManagedDeploymentCatalog Inno.Build.Composition.BuildDistribution.managedDeployments`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L67) | Gets the immutable deployment implementation catalog. |
| [`Inno.Build.SupportPacks.PlayerSupportPackPublisher Inno.Build.Composition.BuildDistribution.supportPacks`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L72) | Gets the publisher containing the same platform input providers. |
| [`Inno.Build.Composition.BuildDistribution`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L15) | Freezes platform factories, managed publishers, and matching Support Pack sources as one distribution. |

### `Inno.Build.Composition.BuildPipelineFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Build.BuildPipeline Inno.Build.Composition.BuildPipelineFactory.Create(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Composition.BuildDistribution distribution, Inno.Runtime.EngineHost engine, Inno.Assets.Pipeline.AssetPipeline assets, Inno.Plugins.Authoring.PluginEnvironment plugins, Inno.Core.Settings.ProjectSettingsStore settings, Inno.Scripting.Compiler.ScriptCompiler compiler, string supportPackRoot)`](../../build/composition/Inno.Build.Composition/BuildPipelineFactory.cs#L46) | Validates and binds the distribution to one authoring host without starting a build. |
| [`Inno.Build.Composition.BuildPipelineFactory`](../../build/composition/Inno.Build.Composition/BuildPipelineFactory.cs#L14) | Creates common build pipelines from a single distribution and explicitly borrowed authoring owners. |

### `Inno.Build.Composition.BuildTargetFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.BuildTargetFactory`](../../build/composition/Inno.Build.Composition/BuildTargetFactory.cs#L22) | Composes a platform packager over the caller's active authoring generation. |

### `Inno.Build.Composition.BuiltInBuildDistribution`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Build.Composition.BuildDistribution Inno.Build.Composition.BuiltInBuildDistribution.Create(Inno.Build.Composition.BuildCompositionContext context)`](../../build/composition/Inno.Build.Composition/BuiltInBuildDistribution.cs#L24) | Composes the built-in platform, managed deployment, and Support Pack implementations. |
| [`Inno.Build.Composition.BuiltInBuildDistribution`](../../build/composition/Inno.Build.Composition/BuiltInBuildDistribution.cs#L13) | Defines the single built-in distribution consumed by Editor, CLI, and MSBuild hosts. |

## 项目依赖

- [Inno.Build.Managed.DotNet](Inno.Build.Managed.DotNet.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Platform.Windows](Inno.Build.Platform.Windows.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Platform.MacOS](Inno.Build.Platform.MacOS.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Platform.Browser](Inno.Build.Platform.Browser.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.SupportPacks](Inno.Build.SupportPacks.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build](Inno.Build.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Build.Managed](Inno.Build.Managed.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Build.SupportPacks.Core](Inno.Build.SupportPacks.Core.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Runtime](../runtime/Inno.Runtime.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Scripting.Compiler](../scripting/Inno.Scripting.Compiler.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
