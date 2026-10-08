# Inno.Build.Composition

[分类索引](README.md) · [Wiki 首页](../README.md) · [平台归属与扩展](../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

中立的构建组合契约。只依赖共同 Build、Managed、Support Pack、Toolchains 及必要创作输入，不注册平台、DotNet compiler 或 backend。

## 组合、生命周期与扩展

`BuildPlatformContribution` 将目标描述、工厂、Support Pack source、SDK provider 和可选 Editor 项目绑定成一个完整贡献。`BuildDistribution` 冻结贡献，拒绝重复 ID、错配 source、错误 Editor 能力和重复 Native-only ID。没有目录扫描或自动发现。

`BuildPipelineFactory.Create` 接收借用的作者端服务与冻结 distribution。运行目标和 `BuildCompositionContext.host` 分离；`toolsTarget` 描述作者工具的执行目标，不替代游戏目标。factory、catalog 与 provider 为借用；Pipeline 拥有每次构建事务，source 拥有准备任务。

具体注册属于 [标准发行组合](Inno.Build.Distribution.Standard.md)。共享 Editor Hosting 接收此项目的 distribution，不引用标准发行组合。新增发行或平台先组成完整贡献，然后在自己的发行入口注册。

```csharp
using System.Collections.Generic;
using Inno.Build.Composition;
using Inno.Build.Managed;

static BuildDistribution Compose(
    IReadOnlyList<BuildPlatformContribution> platforms,
    IReadOnlyList<IManagedDeploymentCompiler> deployments
) {
    return new BuildDistribution(platforms, deployments);
}
```

没有额外 protected 扩展点。SDK 不支持宿主/目标组合时明确失败；发布前的失败和取消保留旧完整输出。契约测试位于 `tests/build/Inno.Build.Tests/BuildCompositionTests.cs`。

平台贡献还声明目标自身的不可变 `nativeProducts`。`BuildDistribution.ResolveNativeProduct(targetId, productId)` 是普通 MSBuild Native 准备的唯一选择入口；Task 不维护 Editor/Player 的 backend 列表或产品 switch。不同平台可以给同一 product ID 选择不同闭包；未注册的产品在 SDK 解析和 staging 前明确失败。Browser 的聚合/最终链接由自身发布链处理，不注册普通 Editor Native 构建。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Composition.BuildCompositionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildTargetId Inno.Build.Composition.BuildCompositionContext.toolsTarget`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L67) | Gets the independently declared native target of build tools executed by this host. |
| [`Inno.Build.Composition.BuildCompositionContext`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L10) | Captures host-owned SDK selection and application location without probing global process state. |
| [`Inno.Build.Composition.BuildCompositionContext.BuildCompositionContext(string dotnetHost, string applicationDirectory, Inno.Build.Toolchains.BuildHostDescriptor host, Inno.Build.BuildTargetId toolsTarget)`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L30) | Captures the host dependencies used by deployment and Support Pack preparation. |
| [`Inno.Build.Toolchains.BuildHostDescriptor Inno.Build.Composition.BuildCompositionContext.host`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L62) | Gets the declared execution host without selecting a product target. |
| [`string Inno.Build.Composition.BuildCompositionContext.applicationDirectory`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L57) | Gets the host location used for source provisioning discovery. |
| [`string Inno.Build.Composition.BuildCompositionContext.dotnetHost`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L52) | Gets the explicitly selected SDK executable. |

### `Inno.Build.Composition.BuildDistribution`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.BuildDistribution`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L16) | Freezes platform factories, managed publishers, and matching Support Pack sources as one distribution. |
| [`Inno.Build.Composition.BuildDistribution.BuildDistribution(System.Collections.Generic.IReadOnlyList<Inno.Build.Composition.GameBuildContribution> games, System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.IManagedDeploymentCompiler> managedCompilers, System.Collections.Generic.IReadOnlyList<Inno.Build.Composition.NativeToolchainContribution>? nativeOnlyContributions = null)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L43) | Captures a complete distribution without creating authoring services or starting any tool. |
| [`Inno.Build.Managed.ManagedDeploymentCatalog Inno.Build.Composition.BuildDistribution.managedDeployments`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L89) | Gets the immutable deployment implementation catalog. |
| [`Inno.Build.SupportPacks.PlayerSupportPackPublisher Inno.Build.Composition.BuildDistribution.supportPacks`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L94) | Gets the publisher containing the same platform input providers. |
| [`Inno.Build.Toolchains.INativeToolchainProvider Inno.Build.Composition.BuildDistribution.ResolveNativeToolchain(string targetId)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L108) | Resolves the SDK integration bound to an explicitly registered publication target. |
| [`Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Composition.BuildDistribution.ResolveNativeProduct(string targetId, string productId)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L134) | Resolves a platform-contributed product closure without selecting backends inside the invoking host. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.BuildTargetId> Inno.Build.Composition.BuildDistribution.availableTargets`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L79) | Gets the immutable platform identities shared by all hosts. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.Composition.BuildPlatformContribution> Inno.Build.Composition.BuildDistribution.platforms`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L84) | Gets complete platform registrations in their explicit composition order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.GameBuildTargetBinding> Inno.Build.Composition.BuildDistribution.CreateBindings(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L165) | Creates platform targets and validates their identities and default deployment capabilities. |
| [`void Inno.Build.Composition.BuildDistribution.ValidateDeployment(Inno.Build.IGameBuildTarget target, Inno.Build.Managed.ManagedDeploymentId deployment)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L203) | Rejects unsupported target and deployment pairs before any staging or publication begins. |

### `Inno.Build.Composition.BuildPipelineFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.BuildPipelineFactory`](../../build/composition/Inno.Build.Composition/BuildPipelineFactory.cs#L14) | Creates common build pipelines from a single distribution and explicitly borrowed authoring owners. |
| [`static Inno.Build.BuildPipeline Inno.Build.Composition.BuildPipelineFactory.Create(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Composition.BuildDistribution distribution, Inno.Runtime.EngineHost engine, Inno.Assets.Pipeline.AssetPipeline assets, Inno.Plugins.Authoring.PluginEnvironment plugins, Inno.Core.Settings.ProjectSettingsStore settings, Inno.Scripting.Compiler.ScriptCompiler compiler, string supportPackRoot)`](../../build/composition/Inno.Build.Composition/BuildPipelineFactory.cs#L46) | Validates and binds the distribution to one authoring host without starting a build. |

### `Inno.Build.Composition.BuildPlatformContribution`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.BuildPlatformContribution`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L13) | Binds one implemented target to its factory, Support Pack source and SDK provider as a single registration. |
| [`Inno.Build.Composition.BuildPlatformContribution.BuildPlatformContribution(Inno.Build.PlatformTargetDescriptor descriptor, Inno.Build.Composition.BuildTargetFactory factory, Inno.Build.SupportPacks.IPlayerSupportPackSource supportPackSource, Inno.Build.Toolchains.INativeToolchainProvider toolchainProvider, string? editorProject = null, System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.ProductNativeBuildPlan>? nativeProducts = null)`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L43) | Creates a complete target registration and rejects mismatched source identities immediately. |
| [`Inno.Build.Composition.BuildTargetFactory Inno.Build.Composition.BuildPlatformContribution.factory`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L83) | Gets the matching authoring target factory. |
| [`Inno.Build.PlatformTargetDescriptor Inno.Build.Composition.BuildPlatformContribution.descriptor`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L78) | Gets the platform-owned, explicit target mapping. |
| [`Inno.Build.SupportPacks.IPlayerSupportPackSource Inno.Build.Composition.BuildPlatformContribution.supportPackSource`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L88) | Gets the matching product input source. |
| [`Inno.Build.Toolchains.INativeToolchainProvider Inno.Build.Composition.BuildPlatformContribution.toolchainProvider`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L93) | Gets the explicit SDK resolver for this target. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.ProductNativeBuildPlan> Inno.Build.Composition.BuildPlatformContribution.nativeProducts`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L103) | Gets the frozen ordinary product-build closures explicitly contributed by this target. |
| [`string? Inno.Build.Composition.BuildPlatformContribution.editorProject`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L98) | Gets the implemented Editor product location without deriving a project from target naming. |

### `Inno.Build.Composition.BuildTargetFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.BuildTargetFactory`](../../build/composition/Inno.Build.Composition/BuildTargetFactory.cs#L9) | Creates a platform packager without borrowing authoring or content compiler services. |

### `Inno.Build.Composition.GameBuildContribution`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.BuildPlatformContribution Inno.Build.Composition.GameBuildContribution.platform`](../../build/composition/Inno.Build.Composition/GameBuildContribution.cs#L35) | Gets the platform-owned contribution, independent of backend selection. |
| [`Inno.Build.Composition.GameBuildContribution`](../../build/composition/Inno.Build.Composition/GameBuildContribution.cs#L8) | Registers a platform contribution with the content implementation selected by a distribution. |
| [`Inno.Build.Composition.GameBuildContribution.GameBuildContribution(Inno.Build.Composition.BuildPlatformContribution platform, Inno.Build.Composition.GameContentCompilerFactory compilerFactory)`](../../build/composition/Inno.Build.Composition/GameBuildContribution.cs#L22) | Captures the complete contribution without instantiating authoring services or tools. |
| [`Inno.Build.Composition.GameContentCompilerFactory Inno.Build.Composition.GameBuildContribution.compilerFactory`](../../build/composition/Inno.Build.Composition/GameBuildContribution.cs#L40) | Gets the distribution-selected factory borrowing each pipeline's authoring generation. |

### `Inno.Build.Composition.GameContentCompilerFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.GameContentCompilerFactory`](../../build/composition/Inno.Build.Composition/GameContentCompilerFactory.cs#L22) | Creates a content compiler borrowing the active authoring services for one pipeline. |

### `Inno.Build.Composition.NativeToolchainContribution`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.NativeToolchainContribution`](../../build/composition/Inno.Build.Composition/NativeToolchainContribution.cs#L12) | Registers an implemented native SDK capability without declaring a complete game product. |
| [`Inno.Build.Composition.NativeToolchainContribution.NativeToolchainContribution(Inno.Build.PlatformTargetDescriptor descriptor, Inno.Build.Toolchains.INativeToolchainProvider provider, System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.ProductNativeBuildPlan>? nativeProducts = null)`](../../build/composition/Inno.Build.Composition/NativeToolchainContribution.cs#L29) | Binds immutable target facts to the provider that owns SDK resolution. |
| [`Inno.Build.PlatformTargetDescriptor Inno.Build.Composition.NativeToolchainContribution.descriptor`](../../build/composition/Inno.Build.Composition/NativeToolchainContribution.cs#L48) | Gets explicit target facts independently of the current execution machine. |
| [`Inno.Build.Toolchains.INativeToolchainProvider Inno.Build.Composition.NativeToolchainContribution.provider`](../../build/composition/Inno.Build.Composition/NativeToolchainContribution.cs#L53) | Gets the registered SDK boundary; registration does not start tools or inspect the environment. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.ProductNativeBuildPlan> Inno.Build.Composition.NativeToolchainContribution.nativeProducts`](../../build/composition/Inno.Build.Composition/NativeToolchainContribution.cs#L57) | Gets explicitly implemented native-only product closures keyed by stable product identity. |

## 项目依赖

- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Build](Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Runtime](../runtime/Inno.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Compiler](../scripting/Inno.Scripting.Compiler.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
